filepath = '/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs'
with open(filepath, 'r', encoding='utf-8') as f:
    lines = f.readlines()

# Check ALL displayName lines to compare working vs broken ones
for lineno, line in enumerate(lines, 1):
    if 'displayName:' in line or 'sectionName:' in line:
        quotes = [(i+1, f'U+{ord(ch):04X}', ch) for i, ch in enumerate(line[:150]) 
                  if ord(ch) in (0x0022, 0x201C, 0x201D, 0x201E)]
        print(f"L{lineno}: {line.strip()[:70]}")
        print(f"  Quotes: {quotes}")
