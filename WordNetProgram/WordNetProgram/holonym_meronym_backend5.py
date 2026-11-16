import json
import graphviz
import os
import sys
import argparse

def load_json_data(filename):
    if not os.path.exists(filename):
        print(f"ERROR: Could not find {filename}")
        return None
    with open(filename, 'r', encoding='utf-8') as f:
        return json.load(f)

def find_target_synsets(data_dict, query):
    matches = []
    
    # Clean extra spaces and create an underscore version (e.g., "hot dog" -> "hot_dog")
    query_clean = " ".join(query.strip().split())
    query_underscore = query_clean.replace(" ", "_")
    
    query_lower = query_clean.lower()
    query_underscore_lower = query_underscore.lower()
    
    for entry in data_dict.values():
        # 1. Match by exact Offset
        if entry["Offset"] == query_clean:
            matches.append(entry)
            continue
            
        # 2. Match English words (checks both "hot dog" and "hot_dog" case-insensitively)
        eng_words_lower = [w.lower() for w in entry.get("Words", [])]
        if query_lower in eng_words_lower or query_underscore_lower in eng_words_lower:
            matches.append(entry)
            continue
            
        # 3. Match Thai words (checks both with space and with underscore)
        thai_words = entry.get("ThaiWords", [])
        if query_clean in thai_words or query_underscore in thai_words:
            matches.append(entry)
            continue
            
    return matches

def get_node_label(data_dict, offset):
    if str(offset).startswith("OTHERS_"):
        count = offset.split("_")[-1]
        return f"Others (+{count} more)"

    entry = data_dict.get(offset)
    if entry:
        english_word = entry.get("Words", ["unknown"])[0]
        thai_words = entry.get("ThaiWords", [])
        source = entry.get("ThaiWordsSource", "")
        
        if thai_words:
            thai_display = ", ".join(thai_words)
            label = f"{offset}: {thai_display}\\n({english_word})"
            if source:
                label += f"\\n{source}"
            return label
        else:
            return f"{offset}: {english_word}"
            
    return f"{offset}: unknown"

# Helper function to format a target offset as a clickable link with its Thai word
def format_synset_link(data_dict, target_offset):
    target_entry = data_dict.get(target_offset)
    if target_entry:
        thai_list = target_entry.get("ThaiWords", [])
        if thai_list:
            word_part = thai_list[0].replace(" ", "_")
        else:
            eng_list = target_entry.get("Words", ["unknown"])
            word_part = eng_list[0].replace(" ", "_")
    else:
        word_part = "unknown"
        
    return f"{target_offset}-{word_part}"

# Builds a pipe-delimited string with both button text and full details (Holonyms & Meronyms) for C#
def get_synset_payload(data_dict, offset):
    entry = data_dict.get(offset)
    if entry:
        words_list = entry.get("Words", ["unknown"])
        first_eng = words_list[0]
        all_eng = ", ".join(words_list)
        
        thai_words = entry.get("ThaiWords", [])
        thai_display = ", ".join(thai_words) if thai_words else "-"
        
        if thai_words:
            btn_text = f"{offset}: {thai_display} ({first_eng})"
        else:
            btn_text = f"{offset}: ({first_eng})"
            
        source = entry.get("ThaiWordsSource", "-") or "-"
        gloss = entry.get("Gloss", "No gloss available").replace("\n", " ").replace("|", "/")
        
        # Format immediate wholes (HolonymsPart) and parts (MeronymsPart) with Thai words and links
        holo_ids = [h["Target"] for h in entry.get("HolonymsPart", [])]
        mero_ids = [m["Target"] for m in entry.get("MeronymsPart", [])]
        
        holo_links = [format_synset_link(data_dict, h_id) for h_id in holo_ids]
        mero_links = [format_synset_link(data_dict, m_id) for m_id in mero_ids]
        
        holo_str = ", ".join(holo_links) if holo_links else "None"
        mero_str = f"{len(mero_links)} synset(s)" + (f" ({', '.join(mero_links)})" if mero_links else "")
        
        return f"{offset}|{btn_text}|{all_eng}|{thai_display}|{source}|{gloss}|{holo_str}|{mero_str}"
    return f"{offset}|{offset}: unknown|unknown|-|-|No details available|None|0"

def build_part_whole_edges(data_dict, target_offset, edges, visited, target_offset_ref, max_parts=None, max_indirect_parts=None, max_co_parts=None, max_indirect_co_parts=None):
    if target_offset in visited:
        return
    visited.add(target_offset)
    
    entry = data_dict.get(target_offset)
    if not entry:
        return
        
    all_parts = entry.get("MeronymsPart", [])
    
    if target_offset == target_offset_ref and max_parts is not None and len(all_parts) > max_parts:
        shown_parts = all_parts[:max_parts]
        hidden_count = len(all_parts) - max_parts
        if hidden_count > 0:
            edges.add((target_offset, f"OTHERS_{target_offset}_{hidden_count}"))
    else:
        shown_parts = all_parts
        
    for part in shown_parts:
        part_offset = part["Target"]
        edges.add((target_offset, part_offset)) 
        
        part_entry = data_dict.get(part_offset)
        if part_entry and target_offset == target_offset_ref:
            all_indirect_parts = part_entry.get("MeronymsPart", [])
            
            if max_indirect_parts is not None and len(all_indirect_parts) > max_indirect_parts:
                shown_indirect_parts = all_indirect_parts[:max_indirect_parts]
                hidden_indirect_count = len(all_indirect_parts) - max_indirect_parts
                if hidden_indirect_count > 0:
                    edges.add((part_offset, f"OTHERS_{part_offset}_{hidden_indirect_count}"))
            else:
                shown_indirect_parts = all_indirect_parts
                
            for indirect_part in shown_indirect_parts:
                edges.add((part_offset, indirect_part["Target"]))
        
    for whole_hyp in entry.get("HolonymsPart", []):
        whole_offset = whole_hyp["Target"]
        edges.add((whole_offset, target_offset)) 
        
        whole_entry = data_dict.get(whole_offset)
        if whole_entry:
            all_co_parts = whole_entry.get("MeronymsPart", [])
            actual_co_parts = [p for p in all_co_parts if p["Target"] != target_offset_ref]
            
            if target_offset == target_offset_ref and max_co_parts is not None and len(actual_co_parts) > max_co_parts:
                shown_co_parts = actual_co_parts[:max_co_parts]
                hidden_count = len(actual_co_parts) - max_co_parts
                if hidden_count > 0:
                    edges.add((whole_offset, f"OTHERS_{whole_offset}_{hidden_count}"))
            else:
                shown_co_parts = actual_co_parts
                
            for co_part in shown_co_parts:
                edges.add((whole_offset, co_part["Target"]))
                
            if target_offset == target_offset_ref:
                for indirect_whole_hyp in whole_entry.get("HolonymsPart", []):
                    indirect_whole_offset = indirect_whole_hyp["Target"]
                    edges.add((indirect_whole_offset, whole_offset))
                    
                    indirect_whole_entry = data_dict.get(indirect_whole_offset)
                    if indirect_whole_entry:
                        all_indirect_co_parts = indirect_whole_entry.get("MeronymsPart", [])
                        actual_indirect_co_parts = [p for p in all_indirect_co_parts if p["Target"] != whole_offset]
                        
                        if max_indirect_co_parts is not None and len(actual_indirect_co_parts) > max_indirect_co_parts:
                            shown_indirect_co_parts = actual_indirect_co_parts[:max_indirect_co_parts]
                            hidden_indirect_count = len(actual_indirect_co_parts) - max_indirect_co_parts
                            if hidden_indirect_count > 0:
                                edges.add((indirect_whole_offset, f"OTHERS_{indirect_whole_offset}_{hidden_indirect_count}"))
                        else:
                            shown_indirect_co_parts = actual_indirect_co_parts
                            
                        for indirect_co_part in shown_indirect_co_parts:
                            edges.add((indirect_whole_offset, indirect_co_part["Target"]))

# MODIFIED: Removed the 10-node cap. Negative numbers or None mean unlimited (None).
def parse_limit(val):
    if val is None:
        return None
    try:
        v = int(val)
        return v if v >= 0 else None
    except ValueError:
        return None

def main():
    sys.stdout.reconfigure(encoding='utf-8')

    parser = argparse.ArgumentParser()
    parser.add_argument("--query", required=True)
    parser.add_argument("--json", default="Test_Case_Eurasia_Eastern_and_North_Hemisphere_to_Paris.json")
    parser.add_argument("--p", type=int, default=None)
    parser.add_argument("--ip", type=int, default=None)
    parser.add_argument("--cp", type=int, default=None)
    parser.add_argument("--icp", type=int, default=None)
    parser.add_argument("--poly_idx", type=int, default=-1)
    args = parser.parse_args()

    raw_data = load_json_data(args.json)
    if not raw_data:
        sys.exit(1)

    data_dict = {item["Offset"]: item for item in raw_data}

    target_synsets = find_target_synsets(data_dict, args.query)
    if not target_synsets:
        print(f"ERROR: Could not find '{args.query}' in the dataset.")
        sys.exit(1)
        
    if len(target_synsets) > 1 and args.poly_idx == -1:
        exact_match = [s for s in target_synsets if s["Offset"] == args.query.strip()]
        if len(exact_match) == 1:
            target_synset = exact_match[0]
        else:
            print("POLYSEMY_START")
            for synset in target_synsets:
                words = ", ".join(synset.get("Words", []))
                thai_words = ", ".join(synset.get("ThaiWords", []))
                gloss = synset.get("Gloss", "No gloss available").replace("\n", " ")
                print(f"{synset['Offset']}||{words}||{thai_words}||{gloss}")
            print("POLYSEMY_END")
            sys.exit(0)
    else:
        poly_idx = args.poly_idx if args.poly_idx >= 0 else 0
        target_synset = target_synsets[poly_idx if poly_idx < len(target_synsets) else 0]

    target_offset = target_synset["Offset"]

    # Parse limits without clamping to 10
    max_p = parse_limit(args.p)
    max_ip = parse_limit(args.ip)
    max_cp = parse_limit(args.cp)
    max_icp = parse_limit(args.icp)

    edges = set()
    visited = set()
    
    build_part_whole_edges(data_dict, target_offset, edges, visited, target_offset, max_p, max_ip, max_cp, max_icp)

    dot = graphviz.Digraph(name="Tree")
    dot.attr('node', shape='box', fontname='Tahoma', style='filled', fillcolor='#F9F9F9')
    dot.attr('edge', fontname='Tahoma', color='#555555')
    
    all_nodes = set()
    all_nodes.add(target_offset)
    for parent_offset, child_offset in edges:
        all_nodes.add(parent_offset)
        all_nodes.add(child_offset)

    for node_id in sorted(list(all_nodes)):
        label = get_node_label(data_dict, node_id)
        if node_id == target_offset:
            dot.body.append(f'\t"{node_id}" [label="{label}" fillcolor="#86f8a6" penwidth=2]')
        else:
            dot.body.append(f'\t"{node_id}" [label="{label}"]')
    
    sorted_edges = sorted(list(edges), key=lambda x: (str(x[0]), str(x[1])), reverse=True)
    for parent_offset, child_offset in sorted_edges:
        dot.body.append(f'\t"{parent_offset}" -> "{child_offset}"')

    # Collect all real synsets displayed in the graph (excluding "OTHERS_" summary nodes)
    real_synsets = [n for n in all_nodes if not str(n).startswith("OTHERS_")]
    other_synsets = sorted([n for n in real_synsets if n != target_offset], reverse=True)
    ordered_synsets = [target_offset] + other_synsets

    output_filename = f"part_whole_tree_{target_offset}"
    try:
        # Render SVG directly via native Graphviz C++ file writer (faster than dot.pipe)
        dot.render(output_filename, format="svg", view=False)
        output_path = os.path.abspath(output_filename + ".svg")
        
        print("SYNSETS_START")
        for synset_id in ordered_synsets:
            print(get_synset_payload(data_dict, synset_id))
        print("SYNSETS_END")
        
        print(f"SUCCESS:{output_path}")
    except Exception as e:
        print(f"ERROR: Graphviz rendering failed. {e}")
        sys.exit(1)

if __name__ == "__main__":
    main()