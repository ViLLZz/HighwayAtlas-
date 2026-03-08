"""
Targeted fix for NetworkData.cs string quote issues.

Problem 1 (introduced by fix_quotes.py): 
  Pattern like „Тракия"" was corrupted to „Тракия""
  (the ASCII C# string closer was changed to U+201D)
  Fix: replace U+201D + U+201D + , or ) with U+201D + " + , or )

Problem 2 (original issue):
  E79-01 importance had „Дунав мост 2" where closing " was ASCII (U+0022)
  which truncated the C# string.
  Fix: ensure all „...text" embedded quotes use U+201D (already done by problem 1 fix).
"""

filepath = '/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs'
with open(filepath, 'r', encoding='utf-8') as f:
    content = f.read()

print(f"File: {len(content)} chars, {content.count(chr(0x201D))} right-curly-quotes")

# Fix: where we have two consecutive right double quotation marks (U+201D U+201D),
# the second one should be an ASCII quote mark (it's a C# string closer).
# Replace „...text\u201D\u201D with „...text\u201D"
before = content.count('\u201d\u201d')
print(f"Double-curly-quote instances: {before}")

content = content.replace('\u201d\u201d', '\u201d"')

after = content.count('\u201d\u201d')
print(f"After fix: {after} remaining")

# Verify the specific problematic lines
import re
for lineno, line in enumerate(content.split('\n'), 1):
    if '\u201e' in line and '"' not in line.split('\u201e')[0]:
        # line contains „ but no ASCII " before the „ 
        # Check if there's an ASCII " after
        after_open = line.split('\u201e', 1)[1]
        if '"' not in after_open and '\u201d' not in after_open:
            print(f"WARNING L{lineno}: no closing quote found: {line[:80]}")

with open(filepath, 'w', encoding='utf-8') as f:
    f.write(content)
print("Saved.")
