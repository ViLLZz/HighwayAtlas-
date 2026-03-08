filepath = '/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs'
with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()
lines = content.split('\n')

# Inspect exactly lines 24, 59, 93, 124
for lineno in [24, 59, 93, 124, 154]:
    line = lines[lineno - 1]
    print(f"\nL{lineno}: {repr(line)}")
    for i, ch in enumerate(line):
        if ord(ch) in (0x22, 0x201C, 0x201D, 0x201E):
            print(f"  col {i+1}: U+{ord(ch):04X} = {ch!r}")
