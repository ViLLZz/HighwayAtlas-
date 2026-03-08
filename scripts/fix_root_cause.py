"""
Root cause: fix_quotes.py saw U+201E and replaced the NEXT ASCII quote (U+0022)
with U+201D - but that ASCII quote was the C# STRING CLOSER, not an embedded quote.
Result: C# string has no closer.

Pattern to fix: U+201C (closing Bulgarian curly) + U+201D (wrongly-changed string closer)
Should be:       U+201C                              + U+0022 (ASCII string closer)

Also fix U+201D that wrongly replaced ASCII string closers in other patterns.
"""

filepath = '/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs'
with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

# Undo the damage done by fix_quotes.py:
# It replaced U+0022 (ASCII ") with U+201D where that quote followed U+201E...text...
# Detection: U+201C + U+201D should be U+201C + U+0022
fixes = [
    ('\u201c\u201d', '\u201c"'),   # Bulgarian closing curly + wrong U+201D → restore ASCII
]
for old, new in fixes:
    n = content.count(old)
    content = content.replace(old, new)
    print(f"Replaced {old!r} -> {new!r}: {n} occurrences")

# Verify: check all displayName patterns now
import re
problems = []
for lineno, line in enumerate(content.split('\n'), 1):
    if 'displayName:' in line or 'sectionName:' in line or 'description:' in line or 'importance:' in line:
        # A C# string literal "..." must have matching ASCII quotes
        # Count ASCII quotes - should be even
        ascii_quotes = line.count('"')
        if ascii_quotes % 2 != 0:
            problems.append((lineno, ascii_quotes, line[:80]))

if problems:
    print(f"\n{len(problems)} lines with odd ASCII quote count:")
    for lineno, count, text in problems[:10]:
        print(f"  L{lineno} ({count} quotes): {text}")
else:
    print("\nNo odd-quote-count lines found. Looks clean!")

with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)
print(f"Saved. {len(content)} chars.")
