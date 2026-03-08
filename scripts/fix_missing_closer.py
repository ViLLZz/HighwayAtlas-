"""
Targeted fix: insert missing ASCII " closer after U+201D in broken lines.

Broken: U+201D immediately followed by ', "' (no ASCII " as C# string closer)
Fixed:  U+201D + '"' (ASCII) + ', "'

This affects Bulgarian display names like „Тракия", „Хемус", etc. where the
closing curly " (U+201D) was not followed by the ASCII string-closer.
"""
filepath = '/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs'
with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

import re

# Find all places where U+201D is followed by ', "' (meaning the ASCII closer is missing)
# and add the ASCII " between U+201D and ','
# The pattern: U+201D + ', ' where the whole line is part of a Text("...", "...") call.
# But we need to be conservative: only do this inside displayName, sectionName contexts.

# General approach: scan each line in Text() string contexts and fix unclosed strings.
# Simpler: scan for the exact pattern U+201D followed by comma (not preceded by another quote)
# i.e. U+201D + U+201D is wrong (fixed already), U+201D + ',' means missing closer.

# Pattern in text lines: \u201d, " → \u201d", "
before = content.count('\u201d,')
print(f'U+201D + comma patterns: {before}')

# Show them
for i, m in enumerate(re.finditer('\u201d,', content)):
    start = max(0, m.start() - 20)
    end = min(len(content), m.end() + 20)
    print(f'  [{i}] {repr(content[start:end])}')

# Fix: U+201D + ',' → U+201D + '"' + ','
content_fixed = content.replace('\u201d,', '\u201d",')
after = content_fixed.count('\u201d,')
print(f'\nAfter fix: {after} remaining')

# Verify the 4 broken lines
lines = content_fixed.split('\n')
for lineno in [24, 59, 93, 124, 154]:
    line = lines[lineno - 1]
    ascii_qs = line.count('"')
    print(f'L{lineno}: {ascii_qs} ASCII quotes - {"OK" if ascii_qs % 2 == 0 else "BROKEN"}')

with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content_fixed)
print('\nSaved.')
