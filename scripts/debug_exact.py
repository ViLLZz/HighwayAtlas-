filepath = '/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs'
with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

lines = content.split('\n')

# Print exact chars around the problematic displayName areas
for lineno in [26, 59, 93, 124]:
    line = lines[lineno - 1]
    print(f"\n=== Line {lineno} ({len(line)} chars) ===")
    for i, ch in enumerate(line):
        if ord(ch) > 127 or ch in ('"', "'", '„', '"', '"'):
            print(f"  [{i}] U+{ord(ch):04X} = {ch!r}")
    print(f"  Full: {repr(line)}")
