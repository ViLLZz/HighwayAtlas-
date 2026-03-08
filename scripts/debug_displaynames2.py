filepath = '/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs'
with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

lines = content.split('\n')

# Find displayName lines
for lineno, line in enumerate(lines, 1):
    if 'displayName:' in line and '\u201e' in line:
        print(f"\n=== Line {lineno} ({len(line)} chars) ===")
        print(f"  Content: {repr(line)}")
        for i, ch in enumerate(line):
            if ch in ('"', '\u201c', '\u201d', '\u201e'):
                print(f"  [{i}] U+{ord(ch):04X}")
        if lineno > 130:
            break
