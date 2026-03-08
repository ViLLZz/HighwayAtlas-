"""
Fix C# syntax: replace embedded ASCII double-quotes inside Bulgarian strings
with proper Unicode curly quotes (U+201D = RIGHT DOUBLE QUOTATION MARK).
Also fixes MB-01 Wikipedia URL which has URL-encoded chars that might cause issues.
"""
import re

filepath = '/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs'
with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

# The problematic lines use „word" where „ is U+201E but closing " is ASCII U+0022.
# Replace closing ASCII quote after „...text... with U+201D (RIGHT DOUBLE QUOTATION MARK).
# Pattern: inside a C# string, find „text" where the final " is ASCII.
# Simple targeted fix: replace known problem strings.

fixes = [
    # E79-01 importance
    ('мост \u201eДунав мост 2" при', 'мост \u201eДунав мост 2\u201c при'),
    # Any other similar pattern: „ … " where closing is ASCII
]

# General fix: find all „...ascii_quote patterns
# Regex: „ followed by content then ASCII "
import re

def fix_embedded_quotes(content):
    # Replace ASCII " that follows „...content (without intervening C# string boundaries)
    # Strategy: find „xyz" where " is ASCII (0x22) and replace closing " with U+201D
    result = []
    i = 0
    while i < len(content):
        if content[i] == '\u201e':  # Found „ (LEFT LOW-9 QUOTATION MARK)
            # Look for matching closing ASCII " (U+0022)
            j = i + 1
            while j < len(content):
                if content[j] == '"':  # ASCII double-quote
                    # Replace with right curly quote
                    result.append(content[i:j])
                    result.append('\u201d')  # U+201D RIGHT DOUBLE QUOTATION MARK
                    i = j + 1
                    break
                elif content[j] in ('\n', '\r'):
                    # No closing quote on same line - leave as-is
                    result.append(content[i])
                    i += 1
                    break
                j += 1
            else:
                result.append(content[i])
                i += 1
        else:
            result.append(content[i])
            i += 1
    return ''.join(result)

original_content = content
content = fix_embedded_quotes(content)

changed = sum(1 for a, b in zip(original_content, content) if a != b)
print(f"Characters changed: {changed}")

# Also fix the MB-01 sourceUrl - the %D0%... URL encoding is fine in C# strings
# but let's verify line 665 area
lines = content.split('\n')
for i, line in enumerate(lines[640:670], start=641):
    if 'MB-01' in line or 'return network' in line or 'sourceUrl' in line:
        print(f"L{i}: {line[:100]}")

with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)
print("Saved.")
