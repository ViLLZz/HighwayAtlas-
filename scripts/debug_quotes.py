filepath = '/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs'
with open(filepath, 'r', encoding='utf-8') as f:
    lines = f.readlines()

# Check lines 59, 93, 124 for character details
for lineno in [59, 93, 124, 546]:
    line = lines[lineno - 1]
    print(f"\nLine {lineno} ({len(line)} chars):")
    print(repr(line[:120]))
    # Find problematic chars
    for i, ch in enumerate(line[:120]):
        if ord(ch) in (0x0022, 0x201C, 0x201D, 0x201E, 0x2018, 0x2019):
            print(f"  col {i+1}: U+{ord(ch):04X} {ch!r}")
