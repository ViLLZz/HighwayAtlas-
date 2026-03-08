# Bulgarian Motorway Atlas v5.0 - Design & Architecture Plan

## Project Vision

Transform the visualization into a **state-of-the-art infrastructure dashboard** inspired by **Tesla's energy/vehicle interface** and **Apple's minimalist design philosophy**. The application should serve as the authoritative reference for Bulgaria's motorway network status, accurate down to the individual lot level.

## Design Philosophy

### Tesla Design Language
- **Minimalist interface** - Only essential information visible
- **Dark mode-first** - Premium aesthetic with high contrast
- **Data hierarchy** - Important metrics prominent and scannable
- **Tactile interaction** - Every click provides immediate, satisfying feedback
- **Futuristic typography** - Clean, modern sans-serif (Inter or similar)

### Apple Design Language
- **Spacious layout** - Generous whitespace/darkness between elements
- **Clear typography hierarchy** - DIN Next or similar
- **Depth through shadow** - Subtle elevation rather than borders
- **Focused content** - Context appears only when needed
- **Invisible technology** - Actions feel natural, not complicated

### Infrastructure Infographic Style
- **Authoritative appearance** - Professional, government-agency credibility
- **Data-first UI** - Accurate numbers take precedence over aesthetics
- **Status visualization** - Color-coding matches official standards
- **Real-world accuracy** - Paths follow actual planned/built routes
- **Expert detail** - Engineers can verify accuracy

## Information Architecture

### Primary Motorways (Complete Network)
```
A1 - Sofia-Dragoman (40 km)
A2 - Sofia-Blagoevgrad / Hemus (434 km) 
A3 - Sofia-Burgas / Maritsa (390 km)
A4 - Sofia-Ruse (310 km)
A6 - Sofia-Plovdiv (160 km)
A7 - Burgas-Malko Tŭrnovo (295 km)
A8-1 - Sofia Ring North (35 km)
A8-2 - Sofia Ring East (38 km)
A8-3 - Sofia Ring South (80 km)
A10 - Kalotina-Kostolac (regional)
R1 - Sofia Bypass extensions
Regional expressways
```

### Data Structure Per Motorway
```javascript
{
  id: "A2",
  name: "Hemus Motorway",
  route: "Sofia-Blagoevgrad",
  totalKm: 434,
  
  // Geographic
  bbox: [[...], [...]], // Bounding box
  center: [lat, lon],
  
  // Status metrics
  operationalKm: 206,
  underConstructionKm: 115,
  plannedKm: 113,
  
  // Financial
  totalBudget: 3.2e9, // EUR
  euFunding: 1.8e9,
  nationalFunding: 1.4e9,
  
  // Timeline
  announced: 1974,
  estimatedCompletion: 2031,
  
  // Segments/Lots with precise data
  segments: [
    {
      id: "A2-01",
      name: "Sofia-Samokov",
      km: { start: 0, end: 65 },
      status: "operational", // operational|construction|planned|discontinued
      completion: 100,
      contractor: "Company Name",
      
      lots: [
        {
          id: "A2-01-L1",
          code: "Л01", // Official designation
          name: "Sofia-Kokalyane",
          kmStart: 0,
          kmEnd: 30,
          status: "operational",
          completion: 100,
          budget: 150e6,
          actualCost: 180e6,
          
          // Geographic precision
          coordinates: [[...], [...], ...], // GeoJSON LineString
          
          // Detailed metadata
          openedYear: 2014,
          contractors: ["Company A", "Company B"],
          
          // Display data
          color: "#22c55e", // status-based
          width: 4,
        }
      ]
    }
  ]
}
```

## UI Layout (Premium, Minimal)

### Desktop Layout
```
┌────────────────────────────────────────────────────────────────┐
│ [≡] Bulgaria Motorway Atlas          [Motorways] [Data] [•] [✓]│
├────────────┬────────────────────────────────────────────────────┤
│ Motorways  │                                                     │
│ ─────────  │                    INTERACTIVE MAP                  │
│ □ A1       │                     (Leaflet)                       │
│ □ A2       │                                                     │
│ □ A3       │                                                     │
│ □ A4       │                                                     │
│ □ A6       │                                                     │
│ □ A7       │                                                     │
│            │                                                     │
│ Status     │                                                     │
│ ─────────  │                                                     │
│ ✓ Open     │                                                     │
│ ◈ Building │                                                     │
│ ◇ Planned  │                                                     │
│            │                                                     │
│ Legend     │                                                     │
│ ─────────  │                                                     │
│ ▬ Lot info │                                                     │
│ opens →    │                                                     │
├────────────┴────────────────────────────────────────────────────┤
│ [Lot Detail Panel - slides up from bottom when selected]        │
│ Shows: Lot name, status, km, budget, completion, contractors   │
└────────────────────────────────────────────────────────────────┘
```

### Detail Panel (Bottom Sheet)
```
┌─────────────────────────────────────────────────┐
│ A2-01-L1: Sofia-Kokalyane                    [×]│
├─────────────────────────────────────────────────┤
│ STATUS         │ OPERATIONAL                     │
│ COMPLETION     │ ████████████████████ 100%      │
│ LENGTH         │ 30 km                           │
│ BUDGET         │ €150M → €180M (actual)         │
│ CONTRACTOR     │ Company Name                    │
│ OPENED         │ 2014-05-15                      │
│                                                 │
│ SEGMENT        │ A2-01: Sofia-Samokov           │
│ MOTORWAY       │ A2: Hemus (Sofia-Blagoevgrad) │
│                                                 │
│ [Expand for more details]                      │
└─────────────────────────────────────────────────┘
```

## Visual Design System

### Color Palette (Dark Mode)
```
Background:    #0A0E27  (Dark navy)
Secondary:     #11162F  (Slightly lighter)
Accent:        #00D4FF  (Tesla cyan)
Text Primary:  #FFFFFF  (Clean white)
Text Secondary:#94A3B8  (Muted gray)
Success:       #10B981  (Green - operational)
Warning:       #F59E0B  (Orange - construction)
Info:          #8B5CF6  (Purple - planned)
Error:         #EF4444  (Red - discontinued)
```

### Typography
- **Headlines**: Inter 600, 32px (motorway names)
- **Subheadings**: Inter 500, 18px
- **Body**: Inter 400, 14px
- **Mono**: Fira Code 400, 12px (km, budget, dates)

### Components
- **Buttons**: Minimal, outline style initially, filled on hover
- **Cards**: Subtle shadows (0 2px 8px rgba(0,0,0,0.2))
- **Sliders**: Track at 8px height, thumb at 16px
- **Maps**: Leaflet with premium basemap (Carto Positron or custom)
- **Charts**: Minimal gridlines, clear data emphasis

## Data Accuracy Requirements

### Official Sources
- ✓ Bulgarian Road Agency (BRA) - motorway data
- ✓ Ministry of Transport - official timeline
- ✓ EU Cohesion Fund - budget information
- ✓ OpenStreetMap - geographic accuracy
- ✓ Tender databases - contractor information

### Validation Checklist
- [ ] All motorway coordinates match official planning documents
- [ ] Lot boundaries align with published engineering plans
- [ ] Budget figures match EU/national fund announcements
- [ ] Completion dates based on official forecasts
- [ ] Contractor names from published contracts
- [ ] Status reflects latest official announcements (Q1 2026)

## Feature Set (MVP + Premium)

### Essential Features (MVP)
1. **Interactive Map**
   - All 7 major motorways displayed
   - Real geographic paths (not abstract)
   - Zoom/pan with smooth transitions
   - Status color-coding

2. **Lot Selection**
   - Click any lot for details
   - Highlight animation (glow effect)
   - Smooth bottom-sheet reveal

3. **Detail Panel**
   - Status, completion %, budget, contractors
   - Dimension and technical specs
   - Timeline (start/estimated completion)
   - Edit/share (placeholder for future)

4. **Filtering**
   - Toggle motorway visibility
   - Filter by status (operational | construction | planned)
   - Quick metrics (total km, completion %)

5. **Data Legend**
   - Color meanings
   - Status definitions
   - Keyboard shortcuts

### Premium Features
- Comparison view (2024 vs 2026 progress)
- Statistical dashboard (budget efficiency, delays by region)
- Export capabilities (PNG, PDF)
- Share with zoom/lot pre-selected
- Search lot by name/code
- Historical timeline (20-year progress)

## Technical Architecture

### Frontend Stack
- **HTML5 + ES2020+** (no frameworks)
- **Tailwind CSS 3.x** (utility-first, customizable)
- **Leaflet 1.9.4** (lightweight mapping)
- **Chart.js 4.x** (analytics, if included)
- **Showdown.js** (markdown to HTML for data descriptions)

### State Management
```javascript
const AppState = {
  mode: "explore", // explore | details | compare | analytics
  selectedMotorway: null, // "A1", "A2", etc.
  selectedLot: null, // "A2-01-L1"
  mapBounds: [[41.2, 22.3], [44.3, 28.8]],
  filters: {
    motorways: new Set(["A1", "A2", "A3", "A4", "A6", "A7"]),
    statuses: new Set(["operational", "construction", "planned"]),
  },
  lang: "en", // en | bg
}
```

## Performance Targets
- **First Paint**: < 1.5s
- **Interactive**: < 2.5s
- **Lot Click Response**: < 100ms
- **Pan/Zoom Smoothness**: 60 FPS
- **Bundle Size**: < 200KB (all assets)

## Success Metrics
- Accurate to ±0.5 km (lot boundaries)
- 100% data verified against official sources
- Zero console errors
- Mobile responsive (375px - 1440px screens)
- Accessibility WCAG 2.1 AA
- Load time optimized for 4G

---

**Timeline**: 2-4 hours comprehensive implementation
**Owner**: Velikodimitrov
**Status**: Design finalized, ready for development
