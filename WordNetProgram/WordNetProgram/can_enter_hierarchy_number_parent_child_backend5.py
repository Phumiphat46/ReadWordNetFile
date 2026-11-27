import base64
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

# Formats a synset option for the C# multi-select dropdown
def format_dropdown_option(data_dict, offset):
    entry = data_dict.get(offset)
    if entry:
        thai_words = entry.get("ThaiWords", [])
        thai_str = ", ".join(thai_words) if thai_words else "-"
        eng_words = entry.get("Words", ["unknown"])
        eng_str = eng_words[0] if eng_words else "unknown"
    else:
        thai_str = "-"
        eng_str = "unknown"
    return f"Synset: {offset}, TH: {thai_str}, ENG: {eng_str}"

# Formats a depth-level synset option with its parent name(s) so mixed branches stay clear
def format_depth_dropdown_option(data_dict, offset, parent_offsets):
    base_text = format_dropdown_option(data_dict, offset)
    parent_names = []
    for p_off in sorted(list(parent_offsets)):
        p_entry = data_dict.get(p_off)
        if p_entry:
            p_words = p_entry.get("Words", [p_off])
            parent_names.append(p_words[0] if p_words else p_off)
        else:
            parent_names.append(p_off)

    if parent_names:
        return f"{base_text}  [Parent: {', '.join(parent_names)}]"
    return base_text

# ==========================================
# LEGACY FILTER (tpFilter) TRAVERSAL HELPERS
# ==========================================
def traverse_downward(data_dict, current_offset, edges, current_depth, max_depth, level_limits, visited_down):
    if current_depth > max_depth or (current_offset, current_depth) in visited_down:
        return
    visited_down.add((current_offset, current_depth))

    entry = data_dict.get(current_offset)
    if not entry:
        return

    hyponyms = entry.get("Hyponyms", [])
    limit = level_limits.get(current_depth, None)

    if limit is not None and len(hyponyms) > limit:
        shown_hyps = hyponyms[:limit]
        remaining = len(hyponyms) - limit
        if remaining > 0:
            edges.add((current_offset, f"OTHERS_{current_offset}_{remaining}"))
    else:
        shown_hyps = hyponyms

    for hyp in shown_hyps:
        child_offset = hyp["Target"]
        edges.add((current_offset, child_offset))
        traverse_downward(data_dict, child_offset, edges, current_depth + 1, max_depth, level_limits, visited_down)

def traverse_upward(data_dict, current_offset, edges, current_depth, max_depth, lateral_limits, visited_up):
    if current_depth > max_depth or (current_offset, current_depth) in visited_up:
        return
    visited_up.add((current_offset, current_depth))

    entry = data_dict.get(current_offset)
    if not entry:
        return

    for parent_hyp in entry.get("Hypernyms", []):
        parent_offset = parent_hyp["Target"]
        edges.add((parent_offset, current_offset))

        parent_entry = data_dict.get(parent_offset)
        if parent_entry:
            # Handle lateral branches (Siblings at depth 1, Uncles/Aunts at depth 2, Great-Uncles at depth 3)
            if current_depth in lateral_limits:
                sib_limit, child_limit = lateral_limits[current_depth]
                all_sibs = parent_entry.get("Hyponyms", [])
                actual_sibs = [h for h in all_sibs if h["Target"] != current_offset]

                if sib_limit is not None and len(actual_sibs) > sib_limit:
                    shown_sibs = actual_sibs[:sib_limit]
                    rem_sibs = len(actual_sibs) - sib_limit
                    if rem_sibs > 0:
                        edges.add((parent_offset, f"OTHERS_{parent_offset}_{rem_sibs}"))
                else:
                    shown_sibs = actual_sibs

                for sib_hyp in shown_sibs:
                    sib_offset = sib_hyp["Target"]
                    edges.add((parent_offset, sib_offset))

                    sib_entry = data_dict.get(sib_offset)
                    if sib_entry:
                        sib_children = sib_entry.get("Hyponyms", [])
                        if child_limit is not None and len(sib_children) > child_limit:
                            shown_children = sib_children[:child_limit]
                            rem_children = len(sib_children) - child_limit
                            if rem_children > 0:
                                edges.add((sib_offset, f"OTHERS_{sib_offset}_{rem_children}"))
                        else:
                            shown_children = sib_children

                        for ch_hyp in shown_children:
                            edges.add((sib_offset, ch_hyp["Target"]))

            # Continue climbing upward to the next ancestor level
            traverse_upward(data_dict, parent_offset, edges, current_depth + 1, max_depth, lateral_limits, visited_up)

def build_family_tree_edges(
    data_dict, target_offset, edges,
    max_children=None, max_grandchildren=None, max_great_grandchildren=None,
    max_siblings=None, max_nephews=None, max_uncles=None, max_cousins=None,
    max_great_uncles=None, max_first_cousins_once_removed=None,
    hierarchy_level=None
):
    target_entry = data_dict.get(target_offset)
    if not target_entry:
        return

    if hierarchy_level is not None and hierarchy_level >= 0:
        max_up_levels = hierarchy_level
        max_down_levels = hierarchy_level
    else:
        max_up_levels = 0
        max_down_levels = 0

    down_limits = {
        1: max_children,
        2: max_grandchildren,
        3: max_great_grandchildren
    }

    lateral_limits = {
        1: (max_siblings, max_nephews),
        2: (max_uncles, max_cousins),
        3: (max_great_uncles, max_first_cousins_once_removed)
    }

    visited_down = set()
    traverse_downward(data_dict, target_offset, edges, 1, max_down_levels, down_limits, visited_down)

    visited_up = set()
    traverse_upward(data_dict, target_offset, edges, 1, max_up_levels, lateral_limits, visited_up)

# ==========================================
# NEW FILTER (tpNewFilter) TRAVERSAL HELPERS
# ==========================================
def traverse_lateral_downward(
    data_dict, current_offset, edges, current_depth, max_depth,
    depth_limits, depth_selected, anc_level, visited, dropdown_options
):
    if (anc_level, current_offset, current_depth) in visited:
        return
    visited.add((anc_level, current_offset, current_depth))

    entry = data_dict.get(current_offset)
    if not entry:
        return

    hyponyms = entry.get("Hyponyms", [])

    # When lateral expansion stops at max_depth, show an "Others (+N more)" clue if hidden children exist
    if current_depth > max_depth:
        if len(hyponyms) > 0:
            edges.add((current_offset, f"OTHERS_DES_OF_{current_offset}_{len(hyponyms)}"))
        return

    # Always record all available hyponyms at this depth so the C# Depth node picker is populated
    depth_bucket = dropdown_options["anc_depth"].setdefault((anc_level, current_depth), {})
    for hyp in hyponyms:
        depth_bucket.setdefault(hyp["Target"], set()).add(current_offset)

    # Hybrid selection: if user picked specific nodes for this depth, use them; otherwise use numeric limit
    selected_at_depth = set(depth_selected.get(str(current_depth), [])) if depth_selected else set()
    if selected_at_depth:
        shown_hyps = [h for h in hyponyms if h["Target"] in selected_at_depth]
        remaining = len(hyponyms) - len(shown_hyps)
        if remaining > 0:
            edges.add((current_offset, f"OTHERS_DES_OF_{current_offset}_{remaining}"))
    else:
        limit = depth_limits.get(str(current_depth), None) if depth_limits else None
        if limit is not None and len(hyponyms) > limit:
            shown_hyps = hyponyms[:limit]
            remaining = len(hyponyms) - limit
            if remaining > 0:
                edges.add((current_offset, f"OTHERS_DES_OF_{current_offset}_{remaining}"))
        else:
            shown_hyps = hyponyms

    for child_hyp in shown_hyps:
        child_offset = child_hyp["Target"]
        edges.add((current_offset, child_offset))
        traverse_lateral_downward(
            data_dict, child_offset, edges, current_depth + 1, max_depth,
            depth_limits, depth_selected, anc_level, visited, dropdown_options
        )


def traverse_downward_new(data_dict, current_offset, edges, current_depth, max_depth, desc_cfg, visited_down, dropdown_options):
    if (current_offset, current_depth) in visited_down:
        return
    visited_down.add((current_offset, current_depth))

    entry = data_dict.get(current_offset)
    if not entry:
        return

    hyponyms = entry.get("Hyponyms", [])

    # When downward traversal stops at the hierarchy limit, show an "Others (+N more)" clue if hidden children exist
    if current_depth > max_depth:
        if len(hyponyms) > 0:
            edges.add((current_offset, f"OTHERS_DES_OF_{current_offset}_{len(hyponyms)}"))
        return

    cfg = desc_cfg.get(str(current_depth), {})
    show_desc = cfg.get("show_descendants", cfg.get("show_other_children", True))
    if not show_desc:
        if len(hyponyms) > 0:
            edges.add((current_offset, f"OTHERS_DES_OF_{current_offset}_{len(hyponyms)}"))
        return

    limit = cfg.get("number_of_descendant", cfg.get("other_children", None))

    if limit is not None and len(hyponyms) > limit:
        shown_hyps = hyponyms[:limit]
        remaining = len(hyponyms) - limit
        if remaining > 0:
            edges.add((current_offset, f"OTHERS_DES_OF_{current_offset}_{remaining}"))
    else:
        shown_hyps = hyponyms

    for hyp in shown_hyps:
        child_offset = hyp["Target"]
        edges.add((current_offset, child_offset))
        traverse_downward_new(data_dict, child_offset, edges, current_depth + 1, max_depth, desc_cfg, visited_down, dropdown_options)


def traverse_upward_new(data_dict, current_offset, edges, current_depth, max_depth, anc_cfg, visited_up, visited_lat, dropdown_options):
    if (current_offset, current_depth) in visited_up:
        return
    visited_up.add((current_offset, current_depth))

    entry = data_dict.get(current_offset)
    if not entry:
        return

    hypernyms = entry.get("Hypernyms", [])

    # When upward traversal stops at the hierarchy limit, show an "Others (+N more)" clue if hidden parents exist
    if current_depth > max_depth:
        if len(hypernyms) > 0:
            edges.add((f"OTHERS_ANC_OF_{current_offset}_{len(hypernyms)}", current_offset))
        return

    cfg = anc_cfg.get(str(current_depth), {})
    show_other = cfg.get("show_other_children", True)
    selected_other = set(cfg.get("selected_other_children", []))
    lateral_depth = cfg.get("depth", 0) or 0
    depth_target_offsets = set(cfg.get("depth_target_offsets", []))
    depth_limits = cfg.get("depth_limits", {})
    depth_selected = cfg.get("depth_selected", {})

    for parent_hyp in hypernyms:
        parent_offset = parent_hyp["Target"]
        edges.add((parent_offset, current_offset))

        parent_entry = data_dict.get(parent_offset)
        if parent_entry:
            all_sibs = parent_entry.get("Hyponyms", [])
            actual_sibs = [h for h in all_sibs if h["Target"] != current_offset]

            # Always send all available siblings so the Other Children dropdown is populated on the first run
            for sib_hyp in actual_sibs:
                dropdown_options["ancestors"].setdefault(current_depth, set()).add(sib_hyp["Target"])

            if show_other and selected_other:
                displayed_sibs = [h for h in actual_sibs if h["Target"] in selected_other]
            else:
                displayed_sibs = []

            rem_sibs = len(actual_sibs) - len(displayed_sibs)
            if rem_sibs > 0:
                edges.add((parent_offset, f"OTHERS_{parent_offset}_{rem_sibs}"))

            for sib_hyp in displayed_sibs:
                sib_offset = sib_hyp["Target"]
                edges.add((parent_offset, sib_offset))

                if depth_target_offsets and sib_offset not in depth_target_offsets:
                    effective_depth = 0
                else:
                    effective_depth = lateral_depth

                traverse_lateral_downward(
                    data_dict, sib_offset, edges, 1, effective_depth,
                    depth_limits, depth_selected, current_depth, visited_lat, dropdown_options
                )

            traverse_upward_new(data_dict, parent_offset, edges, current_depth + 1, max_depth, anc_cfg, visited_up, visited_lat, dropdown_options)

def build_family_tree_edges_new_filter(data_dict, target_offset, edges, config, dropdown_options):
    if not data_dict.get(target_offset):
        return

    hierarchy_level = config.get("hierarchy", 0)
    if hierarchy_level < 0:
        hierarchy_level = 0

    anc_cfg = config.get("ancestors", {})
    desc_cfg = config.get("descendants", {})

    traverse_downward_new(data_dict, target_offset, edges, 1, hierarchy_level, desc_cfg, set(), dropdown_options)
    traverse_upward_new(data_dict, target_offset, edges, 1, hierarchy_level, anc_cfg, set(), set(), dropdown_options)

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

    # === LIGHTWEIGHT ON-DEMAND DEPTH PICKER LOOKUP (NO GRAPHVIZ RENDER) ===
    if query == "--depth-options" and len(sys.argv) >= 4:
        try:
            req_json = base64.b64decode(sys.argv[3]).decode("utf-8")
            req = json.loads(req_json)
        except Exception as e:
            print(f"ERROR: Invalid depth-options config. {e}")
            sys.exit(1)

        anc_level = req.get("anc_level", 1)
        root_offsets = req.get("root_offsets", [])
        max_depth = req.get("max_depth", 0) or 0
        depth_limits = req.get("depth_limits", {})
        depth_selected = req.get("depth_selected", {})

        dummy_edges = set()
        visited_lat = set()
        dropdown_options = {"ancestors": {}, "descendants": {}, "anc_depth": {}}

        for root_off in root_offsets:
            traverse_lateral_downward(
                data_dict, root_off, dummy_edges, 1, max_depth,
                depth_limits, depth_selected, anc_level, visited_lat, dropdown_options
            )

        print("DROPDOWN_OPTIONS_START")
        for (a_lvl, depth_idx), child_map in sorted(dropdown_options["anc_depth"].items()):
            for off in sorted(list(child_map.keys()), reverse=True):
                display_text = format_depth_dropdown_option(data_dict, off, child_map[off]).replace("|", "/")
                print(f"ANC_DEPTH|{a_lvl}|{depth_idx}|{off}|{display_text}")
        print("DROPDOWN_OPTIONS_END")
        sys.exit(0)

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
    edges = set()
    dropdown_options = {"ancestors": {}, "descendants": {}, "anc_depth": {}}

    if len(sys.argv) >= 5 and sys.argv[3] == "--new-filter":
        try:
            config_json = base64.b64decode(sys.argv[4]).decode("utf-8")
            new_filter_config = json.loads(config_json)
        except Exception as e:
            print(f"ERROR: Invalid new-filter config. {e}")
            sys.exit(1)

        build_family_tree_edges_new_filter(data_dict, target_offset, edges, new_filter_config, dropdown_options)
    else:
        max_children = parse_limit(sys.argv[3]) if len(sys.argv) > 3 else None
        max_grandchildren = parse_limit(sys.argv[4]) if len(sys.argv) > 4 else None
        max_great_grandchildren = parse_limit(sys.argv[5]) if len(sys.argv) > 5 else None
        max_siblings = parse_limit(sys.argv[6]) if len(sys.argv) > 6 else None
        max_nephews = parse_limit(sys.argv[7]) if len(sys.argv) > 7 else None
        max_uncles = parse_limit(sys.argv[8]) if len(sys.argv) > 8 else None
        max_cousins = parse_limit(sys.argv[9]) if len(sys.argv) > 9 else None
        max_great_uncles = parse_limit(sys.argv[10]) if len(sys.argv) > 10 else None
        max_first_cousins_once_removed = parse_limit(sys.argv[11]) if len(sys.argv) > 11 else None
        hierarchy_level = parse_limit(sys.argv[12]) if len(sys.argv) > 12 else None

        build_family_tree_edges(
            data_dict, target_offset, edges,
            max_children, max_grandchildren, max_great_grandchildren,
            max_siblings, max_nephews, max_uncles, max_cousins,
            max_great_uncles, max_first_cousins_once_removed,
            hierarchy_level
        )

    dot = graphviz.Digraph(name="Tree")
    dot.attr(ranksep='1.5', nodesep='0.35', splines='polyline', concentrate='true')
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

        print("DROPDOWN_OPTIONS_START")
        for group_key in ("ancestors", "descendants"):
            group_prefix = "ANC" if group_key == "ancestors" else "DESC"
            for level, offsets in sorted(dropdown_options[group_key].items()):
                for off in sorted(list(offsets), reverse=True):
                    display_text = format_dropdown_option(data_dict, off).replace("|", "/")
                    print(f"{group_prefix}|{level}|{off}|{display_text}")

        for (anc_level, depth_idx), child_map in sorted(dropdown_options["anc_depth"].items()):
            for off in sorted(list(child_map.keys()), reverse=True):
                display_text = format_depth_dropdown_option(data_dict, off, child_map[off]).replace("|", "/")
                print(f"ANC_DEPTH|{anc_level}|{depth_idx}|{off}|{display_text}")
        print("DROPDOWN_OPTIONS_END")

        print(f"SUCCESS:{output_path}")
    except Exception as e:
        print(f"ERROR: Graphviz rendering failed. {e}")
        sys.exit(1)

if __name__ == "__main__":
    main()