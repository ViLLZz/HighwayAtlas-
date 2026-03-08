import re
content = open('/Users/velikodimitrov/Desktop/Motorway/src/Motorway.Infrastructure/NetworkData.cs', encoding='utf-8').read()
print('Has Sofia-Simitli:', 'Sofia \u2013 Simitli' in content)
print('Has Kresna Gorge:', 'Kresna Gorge' in content)
codes = re.findall(r'sectionCode: "([^"]+)"', content)
print('Section codes:', codes)
print('Total length:', len(content))
