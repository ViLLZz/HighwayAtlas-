"""
Fix displayName strings: remove spurious U+201C (LEFT double quotation mark)
that appears before U+201D in patterns like „Тракия"" → „Тракия""
The U+201C is extraneous; only U+201D (right curly) is needed.
After this fix, pattern becomes „Тракия"" → „Тракия"" (U+201E text U+201D ASCII-closer).
"""

filepath = '/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs'
with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

# Count occurrences
count_before = content.count('\u201c\u201d')
print(f"U+201C + U+201D pairs found: {count_before}")

# Remove U+201C when immediately followed by U+201D
content_fixed = content.replace('\u201c\u201d', '\u201d')

count_after = content_fixed.count('\u201c\u201d')
print(f"After fix: {count_after} remaining")

# Also check for standalone U+201C (not followed by U+201D) which shouldn't exist in C# string content
standalone_201c = content_fixed.count('\u201c')
print(f"Standalone U+201C remaining: {standalone_201c}")
if standalone_201c > 0:
    idx = 0
    for i, lineno_offset in enumerate(content_fixed.split('\n')):
        if '\u201c' in lineno_offset:
            print(f"  Line {i+1}: {repr(lineno_offset[:80])}")

with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content_fixed)
print("Saved.")
