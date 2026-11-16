import json
import graphviz
import os
import sys

def load_json_data(filename):
    if not os.path.exists(filename):
        print(f"Error: Could not find {filename}")
        return None
    with open(filename, 'r', encoding='utf-8') as f:
        return json.load(f)

# Retrieves a list of ALL matching synsets to handle polysemy
def find_target_synsets(data_dict, query):
    matches = []
    
    # Clean extra spaces and create underscore versions (e.g., "hot dog" -> "hot_dog")
    query_clean = " ".join(query.strip().split())
    query_underscore = query_clean.replace(" ", "_")
    
    query_lower = query_clean.lower()
    query_underscore_lower = query_underscore.lower()
    
    for entry in data_dict.values():
        # 1. Match by exact offset
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

# Builds a multi-line string containing all Thai words, English word, and Source
def get_node_label(data_dict, offset):
    # Catch the special "Others" node
    if str(offset).startswith("OTHERS_"):
        count = offset.split("_")[-1]
        return f"Others (+{count} more)"

    # INSTANT O(1) LOOKUP
    entry = data_dict.get(offset)
    if entry:
        english_word = entry.get("Words", ["unknown"])[0]
        thai_words = entry.get("ThaiWords", [])
        source = entry.get("ThaiWordsSource", "")
        
        if thai_words:
            # Join all Thai words for this specific taxonomy view
            thai_display = ", ".join(thai_words)
            label = f"{offset}: {thai_display}\\n({english_word})"
            if source:
                label += f"\\n{source}"
            return label
        else:
            return f"{offset}: {english_word}"
            
    # Fallback if the offset is completely missing from the JSON
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

# Builds a pipe-delimited string with both button text and full details for C#
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
        
        # Format immediate parents (Hypernyms) and children (Hyponyms) with Thai words and links
        hyper_ids = [h["Target"] for h in entry.get("Hypernyms", [])]
        hypo_ids = [h["Target"] for h in entry.get("Hyponyms", [])]
        
        hyper_links = [format_synset_link(data_dict, h_id) for h_id in hyper_ids]
        hypo_links = [format_synset_link(data_dict, h_id) for h_id in hypo_ids]
        
        hyper_str = ", ".join(hyper_links) if hyper_links else "None"
        hypo_str = f"{len(hypo_links)} synset(s)" + (f" ({', '.join(hypo_links)})" if hypo_links else "")
        
        return f"{offset}|{btn_text}|{all_eng}|{thai_display}|{source}|{gloss}|{hyper_str}|{hypo_str}"
    return f"{offset}|{offset}: unknown|unknown|-|-|No details available|None|0"

def build_family_tree_edges(data_dict, target_offset, edges, max_children=None, max_grandchildren=None, max_great_grandchildren=None, max_siblings=None, max_nephews=None, max_uncles=None, max_cousins=None, max_great_uncles=None, max_first_cousins_once_removed=None):
    # INSTANT O(1) LOOKUP
    target_entry = data_dict.get(target_offset)
    if not target_entry:
        return
        
    target_hyponyms = target_entry.get("Hyponyms", [])
    if max_children is not None and len(target_hyponyms) > max_children:
        shown_hyps = target_hyponyms[:max_children]
        remaining = len(target_hyponyms) - max_children
        if remaining > 0:
            edges.add((target_offset, f"OTHERS_{target_offset}_{remaining}"))
    else:
        shown_hyps = target_hyponyms
        
    for hyp in shown_hyps:
        child_offset = hyp["Target"]
        edges.add((target_offset, child_offset))
        
        child_entry = data_dict.get(child_offset)
        if child_entry:
            child_hyponyms = child_entry.get("Hyponyms", [])
            if max_grandchildren is not None and len(child_hyponyms) > max_grandchildren:
                shown_grandchildren = child_hyponyms[:max_grandchildren]
                remaining_gc = len(child_hyponyms) - max_grandchildren
                if remaining_gc > 0:
                    edges.add((child_offset, f"OTHERS_{child_offset}_{remaining_gc}"))
            else:
                shown_grandchildren = child_hyponyms

            for grandchild_hyp in shown_grandchildren:
                grandchild_offset = grandchild_hyp["Target"]
                edges.add((child_offset, grandchild_offset))
                
                grandchild_entry = data_dict.get(grandchild_offset)
                if grandchild_entry:
                    great_grandchild_hyps = grandchild_entry.get("Hyponyms", [])
                    if max_great_grandchildren is not None and len(great_grandchild_hyps) > max_great_grandchildren:
                        shown_great_grandchildren = great_grandchild_hyps[:max_great_grandchildren]
                        remaining_ggc = len(great_grandchild_hyps) - max_great_grandchildren
                        if remaining_ggc > 0:
                            edges.add((grandchild_offset, f"OTHERS_{grandchild_offset}_{remaining_ggc}"))
                    else:
                        shown_great_grandchildren = great_grandchild_hyps

                    for great_grandchild_hyp in shown_great_grandchildren:
                        edges.add((grandchild_offset, great_grandchild_hyp["Target"]))
        
    for parent_hyp in target_entry.get("Hypernyms", []):
        parent_offset = parent_hyp["Target"]
        edges.add((parent_offset, target_offset))
        
        parent_entry = data_dict.get(parent_offset)
        if parent_entry:
            all_sibling_hyps = parent_entry.get("Hyponyms", [])
            actual_siblings = [hyp for hyp in all_sibling_hyps if hyp["Target"] != target_offset]
            
            if max_siblings is not None and len(actual_siblings) > max_siblings:
                shown_siblings = actual_siblings[:max_siblings]
                remaining_sibs = len(actual_siblings) - max_siblings
                if remaining_sibs > 0:
                    edges.add((parent_offset, f"OTHERS_{parent_offset}_{remaining_sibs}"))
            else:
                shown_siblings = actual_siblings

            for sibling_hyp in shown_siblings:
                sibling_offset = sibling_hyp["Target"]
                edges.add((parent_offset, sibling_offset))
                
                sibling_entry = data_dict.get(sibling_offset)
                if sibling_entry:
                    sibling_children = sibling_entry.get("Hyponyms", [])
                    if max_nephews is not None and len(sibling_children) > max_nephews:
                        shown_nephews = sibling_children[:max_nephews]
                        remaining_nephews = len(sibling_children) - max_nephews
                        if remaining_nephews > 0:
                            edges.add((sibling_offset, f"OTHERS_{sibling_offset}_{remaining_nephews}"))
                    else:
                        shown_nephews = sibling_children

                    for niece_nephew_hyp in shown_nephews:
                        edges.add((sibling_offset, niece_nephew_hyp["Target"]))

            for grand_hyp in parent_entry.get("Hypernyms", []):
                grand_offset = grand_hyp["Target"]
                edges.add((grand_offset, parent_offset))
                
                grand_entry = data_dict.get(grand_offset)
                if grand_entry:
                    all_uncle_hyps = grand_entry.get("Hyponyms", [])
                    actual_uncles = [hyp for hyp in all_uncle_hyps if hyp["Target"] != parent_offset]
                    
                    if max_uncles is not None and len(actual_uncles) > max_uncles:
                        shown_uncles = actual_uncles[:max_uncles]
                        remaining_uncles = len(actual_uncles) - max_uncles
                        if remaining_uncles > 0:
                            edges.add((grand_offset, f"OTHERS_{grand_offset}_{remaining_uncles}"))
                    else:
                        shown_uncles = actual_uncles

                    for aunt_uncle_hyp in shown_uncles:
                        aunt_uncle_offset = aunt_uncle_hyp["Target"]
                        edges.add((grand_offset, aunt_uncle_offset))
                        
                        aunt_uncle_entry = data_dict.get(aunt_uncle_offset)
                        if aunt_uncle_entry:
                            cousin_hyps = aunt_uncle_entry.get("Hyponyms", [])
                            if max_cousins is not None and len(cousin_hyps) > max_cousins:
                                shown_cousins = cousin_hyps[:max_cousins]
                                remaining_cousins = len(cousin_hyps) - max_cousins
                                if remaining_cousins > 0:
                                    edges.add((aunt_uncle_offset, f"OTHERS_{aunt_uncle_offset}_{remaining_cousins}"))
                            else:
                                shown_cousins = cousin_hyps

                            for cousin_hyp in shown_cousins:
                                edges.add((aunt_uncle_offset, cousin_hyp["Target"]))
                                
                    for great_grand_hyp in grand_entry.get("Hypernyms", []):
                        great_grand_offset = great_grand_hyp["Target"]
                        edges.add((great_grand_offset, grand_offset))
                        
                        great_grand_entry = data_dict.get(great_grand_offset)
                        if great_grand_entry:
                            all_great_uncle_hyps = great_grand_entry.get("Hyponyms", [])
                            actual_great_uncles = [hyp for hyp in all_great_uncle_hyps if hyp["Target"] != grand_offset]
                            
                            if max_great_uncles is not None and len(actual_great_uncles) > max_great_uncles:
                                shown_great_uncles = actual_great_uncles[:max_great_uncles]
                                remaining_great_uncles = len(actual_great_uncles) - max_great_uncles
                                if remaining_great_uncles > 0:
                                    edges.add((great_grand_offset, f"OTHERS_{great_grand_offset}_{remaining_great_uncles}"))
                            else:
                                shown_great_uncles = actual_great_uncles

                            for great_uncle_hyp in shown_great_uncles:
                                great_uncle_offset = great_uncle_hyp["Target"]
                                edges.add((great_grand_offset, great_uncle_offset))
                                
                                great_uncle_entry = data_dict.get(great_uncle_offset)
                                if great_uncle_entry:
                                    fcor_hyps = great_uncle_entry.get("Hyponyms", [])
                                    if max_first_cousins_once_removed is not None and len(fcor_hyps) > max_first_cousins_once_removed:
                                        shown_fcor = fcor_hyps[:max_first_cousins_once_removed]
                                        remaining_fcor = len(fcor_hyps) - max_first_cousins_once_removed
                                        if remaining_fcor > 0:
                                            edges.add((great_uncle_offset, f"OTHERS_{great_uncle_offset}_{remaining_fcor}"))
                                    else:
                                        shown_fcor = fcor_hyps

                                    for fcor_hyp in shown_fcor:
                                        edges.add((great_uncle_offset, fcor_hyp["Target"]))

def parse_limit(arg):
    try:
        val = int(arg)
        # Interpret negative numbers as "unlimited" (None)
        return val if val >= 0 else None
    except ValueError:
        # Default to unlimited if parsing fails
        return None

def main():
    # Force standard output to UTF-8 to prevent Thai text encoding errors in C#
    sys.stdout.reconfigure(encoding='utf-8')

    if len(sys.argv) < 3:
        print("ERROR: Missing arguments.")
        sys.exit(1)

    json_filename = sys.argv[1]
    query = sys.argv[2]

    raw_data = load_json_data(json_filename)
    if not raw_data:
        print("ERROR: Could not load JSON data.")
        sys.exit(1)

    # CONVERT LIST TO DICTIONARY FOR INSTANT O(1) LOOKUPS
    data_dict = {item["Offset"]: item for item in raw_data}

    target_synsets = find_target_synsets(data_dict, query)
    if not target_synsets:
        print(f"ERROR: Could not find '{query}' in the dataset.")
        sys.exit(1)
        
    # === POLYSEMY CHECK ===
    if len(target_synsets) > 1:
        exact_match = [s for s in target_synsets if s["Offset"] == query]
        if len(exact_match) == 1:
            target_synset = exact_match[0]
        else:
            print("POLYSEMY_START")
            for s in target_synsets:
                offset = s["Offset"]
                words = ", ".join(s.get("Words", []))
                thai_words = ", ".join(s.get("ThaiWords", []))
                gloss = s.get("Gloss", "No gloss available").replace("\n", " ")
                print(f"{offset}|{words}|{thai_words}|{gloss}")
            print("POLYSEMY_END")
            sys.exit(0)
    else:
        target_synset = target_synsets[0]

    target_offset = target_synset["Offset"]

    max_children = parse_limit(sys.argv[3]) if len(sys.argv) > 3 else None
    max_grandchildren = parse_limit(sys.argv[4]) if len(sys.argv) > 4 else None
    max_great_grandchildren = parse_limit(sys.argv[5]) if len(sys.argv) > 5 else None
    max_siblings = parse_limit(sys.argv[6]) if len(sys.argv) > 6 else None
    max_nephews = parse_limit(sys.argv[7]) if len(sys.argv) > 7 else None
    max_uncles = parse_limit(sys.argv[8]) if len(sys.argv) > 8 else None
    max_cousins = parse_limit(sys.argv[9]) if len(sys.argv) > 9 else None
    max_great_uncles = parse_limit(sys.argv[10]) if len(sys.argv) > 10 else None
    max_first_cousins_once_removed = parse_limit(sys.argv[11]) if len(sys.argv) > 11 else None
    
    edges = set()
    build_family_tree_edges(data_dict, target_offset, edges, max_children, max_grandchildren, max_great_grandchildren, max_siblings, max_nephews, max_uncles, max_cousins, max_great_uncles, max_first_cousins_once_removed)

    dot = graphviz.Digraph(name="Tree")
    dot.attr('node', shape='box', fontname='Tahoma', style='round,filled', fillcolor='#F9F9F9')
    dot.attr('edge', fontname='Tahoma', color='#555555')
    
    all_nodes = set()
    all_nodes.add(target_offset) 
    for parent_offset, child_offset in edges:
        all_nodes.add(parent_offset)
        all_nodes.add(child_offset)

    for node_id in sorted(list(all_nodes)):
        label = get_node_label(data_dict, node_id)
        if node_id == target_offset:
            dot.body.append(f'\t"{node_id}" [label="{label}" fillcolor="#FFE0B2" penwidth=2]')
        else:
            dot.body.append(f'\t"{node_id}" [label="{label}"]')
    
    sorted_edges = sorted(list(edges), key=lambda x: (str(x[0]), str(x[1])), reverse=True)
    
    for parent_offset, child_offset in sorted_edges:
        dot.body.append(f'\t"{parent_offset}" -> "{child_offset}"')

    # Collect all real synsets displayed in the graph (excluding "OTHERS_" summary nodes)
    real_synsets = [n for n in all_nodes if not str(n).startswith("OTHERS_")]
    other_synsets = sorted([n for n in real_synsets if n != target_offset], reverse=True)
    ordered_synsets = [target_offset] + other_synsets

    output_filename = f"hypernym_tree_{target_offset}"
    try:
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