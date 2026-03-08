# Motorway Atlas 100-Step Modernization Plan — 2026-03-07

A structured roadmap for debugging, redesigning, and maturing the canonical Bulgaria motorway atlas.

## Phase 1 — Stabilize core interaction

1. Audit current lot click flow from marker click to popup render.
2. Audit current segment click flow from polyline click to inspector sync.
3. Verify popup behavior after repeated clicks on the same lot.
4. Verify popup behavior when switching between lots in the same segment.
5. Verify popup behavior when switching between lots in different routes.
6. Verify popup behavior after language changes.
7. Verify popup behavior after basemap changes.
8. Verify popup behavior after route filter changes.
9. Verify popup behavior after status filter changes.
10. Verify popup behavior after manual popup close.

## Phase 2 — Eliminate map sluggishness

11. Measure full render frequency on each interaction.
12. Separate map-only updates from panel-only updates where practical.
13. Avoid redundant viewport animations.
14. Avoid redundant popup recreation.
15. Avoid unnecessary layer destruction on non-map-only updates.
16. Cache visible route collections per render cycle.
17. Cache visible lot collections per render cycle.
18. Reduce repeated DOM writes in inspector panels.
19. Reduce popup flicker during state changes.
20. Tune animation duration for route and lot focus.

## Phase 3 — Improve popup UX

21. Increase popup readability and spacing.
22. Improve popup close-button contrast.
23. Keep popup open while interacting with map panels.
24. Ensure popup content always matches active lot title.
25. Ensure popup content always matches active lot status.
26. Ensure popup content always matches active lot source metadata.
27. Add stronger selected-marker emphasis.
28. Add larger popup width for dense lot data.
29. Keep popup in view on smaller screens.
30. Add clear fallback behavior when selected marker is filtered out.

## Phase 4 — Upgrade map presentation

31. Increase map panel size within the layout.
32. Increase map height while preserving dashboard balance.
33. Improve map edge chrome and contained-panel framing.
34. Improve default basemap choice for motorway visibility.
35. Fine-tune tile contrast and brightness.
36. Fine-tune line casing thickness.
37. Fine-tune active route glow and visibility.
38. Fine-tune planned versus construction dash styles.
39. Improve marker layering over route polylines.
40. Improve national default framing around Bulgaria.

## Phase 5 — Redesign hero infographic section

41. Restore more structured dashboard hierarchy inspired by the older project.
42. Make the hero area read as a command-center summary board.
43. Add clearer top-level route signals.
44. Add a corridor summary stack inside the hero.
45. Add compact audit indicators inside the hero.
46. Add a stronger focus-mode badge.
47. Improve the relationship between hero content and the map.
48. Improve typography scale in the hero.
49. Improve spacing consistency in hero cards.
50. Ensure the hero remains responsive on smaller screens.

## Phase 6 — Redesign selection and inspector panels

51. Improve selected lot title hierarchy.
52. Improve selected segment hierarchy.
53. Improve note treatment with stronger emphasis styling.
54. Improve fact-grid readability.
55. Improve source-link visibility and trust cues.
56. Improve timeline panel density and spacing.
57. Add better differentiation between route and lot overview states.
58. Reduce visual clutter in selection pills.
59. Improve right-rail consistency across states.
60. Align inspector chrome with hero and map chrome.

## Phase 7 — Improve sidebar structure

61. Rebalance sidebar width versus map width.
62. Improve route list card hierarchy.
63. Improve lot list card hierarchy.
64. Improve note-block readability.
65. Improve KPI card legibility.
66. Improve list active-state treatment.
67. Improve sidebar scroll behavior.
68. Reduce repeated microcopy in lists.
69. Surface route significance more clearly.
70. Surface lot audit status more clearly.

## Phase 8 — Data and audit integrity

71. Expand explicit lot definitions beyond Hemus.
72. Replace derived lots for A1 with explicit lots.
73. Replace derived lots for A3 with explicit lots.
74. Replace derived lots for A4 with explicit lots.
75. Replace derived lots for A5 with explicit lots.
76. Replace derived lots for A6 with explicit lots.
77. Replace derived lots for E79 with explicit lots.
78. Replace derived lots for RVT with explicit lots.
79. Replace derived lots for MB with explicit lots.
80. Recompute audit percentages after each corridor upgrade.

## Phase 9 — Codebase cleanup

81. Separate exporter CSS generation from model generation concerns.
82. Separate inline JS responsibilities into clearer logical blocks.
83. Document canonical exporter ownership in project docs.
84. Archive legacy artifacts that are no longer primary.
85. Reduce duplicated wording across README and docs.
86. Add a dedicated atlas design notes document.
87. Add a popup-debugging checklist document.
88. Add a rendering-performance notes document.
89. Add a data-quality expansion tracking document.
90. Prepare the exporter for future template extraction.

## Phase 10 — Final polish and validation

91. Build and export after each major design pass.
92. Validate no syntax or compile errors remain.
93. Validate generated HTML loads cleanly in browser.
94. Validate popup behavior across all visible routes.
95. Validate BG and EN text fit new panel layouts.
96. Validate responsive behavior on tablet widths.
97. Validate responsive behavior on smaller laptop widths.
98. Validate map framing after route selection resets.
99. Validate canonical outputs remain synchronized.
100. Publish a final change log summarizing the completed atlas refinement cycle.
