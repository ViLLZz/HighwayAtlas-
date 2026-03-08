#!/usr/bin/env python3
"""
Targeted fix for A2/A3 sections that failed in bulk rewrite.
Uses sectionCode anchor + field-by-field replacement.
"""
import re, sys

PATH = "/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs"

with open(PATH, encoding="utf-8") as f:
    content = f.read()

changes = 0

def rep(old, new, label):
    global content, changes
    if old not in content:
        print(f"  MISSING: {label}")
        print(f"    Looking for: {repr(old[:80])}")
        return
    content = content.replace(old, new, 1)
    changes += 1
    print(f"  OK: {label}")

# ==========================================================
# Helper: find the block start for a given sectionCode
# ==========================================================
def find_section_block(code):
    """Return start/end indices of the CreateSegment call for sectionCode."""
    marker = f'sectionCode: "{code}"'
    idx = content.find(marker)
    if idx < 0:
        return None, None
    # Go backwards to find the CreateSegment line
    start = content.rfind("CreateSegment(", 0, idx)
    # Find the matching close
    # The block ends after the last milestones closing bracket
    depth = 0
    i = start
    while i < len(content):
        c = content[i]
        if c == '(':
            depth += 1
        elif c == ')':
            depth -= 1
            if depth == 0:
                return start, i + 1
        i += 1
    return start, None

# Diagnostic – show each A2 sectionName and description
for code in ["A2-01", "A2-02", "A2-03", "A2-04", "A3-03"]:
    s, e = find_section_block(code)
    if s is None:
        print(f"{code}: NOT FOUND")
        continue
    block = content[s:e]
    # Find sectionName line
    m = re.search(r'sectionName: Text\("([^"]+)", "([^"]+)"\)', block)
    if m:
        print(f"{code} sectionName: BG={m.group(1)[:30]!r}  EN={m.group(2)!r}")
    m2 = re.search(r'lengthKm: (\d+)', block)
    if m2:
        print(f"  lengthKm={m2.group(1)}")
    m3 = re.search(r'status: SegmentStatus\.(\w+)', block)
    if m3:
        print(f"  status={m3.group(1)}")
    print()
