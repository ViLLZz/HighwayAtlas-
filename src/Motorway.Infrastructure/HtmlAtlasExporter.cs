using System.Text.Json;
using Motorway.Domain;
using Motorway.Engine;

namespace Motorway.Infrastructure;

public static class HtmlAtlasExporter
{
    public static string Export(HighwayRoute network)
    {
        var atlasJson = JsonSerializer.Serialize(BuildAtlasModel(network));

        return $$"""
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="copyright" content="© 2026 ViLLZz. All rights reserved." />
    <meta name="robots" content="noarchive,max-image-preview:none" />
    <title>Motorway Atlas — Premium Bulgaria Preview</title>
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Manrope:wght@400;500;600;700;800&family=Sora:wght@500;600;700;800&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" integrity="sha256-p4NxAoJBhIIN+hmNHrzRCf9tD/miZyoHS5obTRR9BMY=" crossorigin="" />
    <style>
        :root {
            color-scheme: dark;
            --bg: #030812;
            --panel: rgba(7, 15, 28, 0.86);
            --panel-elevated: linear-gradient(180deg, rgba(15, 28, 45, 0.88), rgba(6, 12, 24, 0.94));
            --panel-strong: rgba(6, 12, 24, 0.95);
            --panel-soft: rgba(13, 22, 39, 0.86);
            --text: #eef5ff;
            --muted: #92a6c6;
            --line: rgba(110, 144, 200, 0.16);
            --accent: #2ee6c0;
            --accent-2: #3ea2ff;
            --accent-3: #89b6ff;
            --accent-warm: #ffd38a;
            --open: #2ee6c0;
            --construction: #ffb357;
            --planned: #8f74ff;
            --closed: #ff648a;
            --shadow: 0 24px 80px rgba(0, 0, 0, 0.34);
            --shadow-soft: 0 16px 40px rgba(0, 0, 0, 0.22);
            --glow: 0 0 0 1px rgba(255,255,255,0.04), 0 24px 50px rgba(7, 14, 24, 0.28);
            --chrome: rgba(255,255,255,0.06);
            --radius-xl: 24px;
            --radius-lg: 18px;
            --radius-md: 14px;
            --space-1: 8px;
            --space-2: 12px;
            --space-3: 16px;
            --space-4: 20px;
            --body-font: "Manrope", "Segoe UI", sans-serif;
            --display-font: "Sora", "Segoe UI", sans-serif;
            --shell-padding: max(18px, env(safe-area-inset-left));
            --shell-padding-right: max(18px, env(safe-area-inset-right));
            --shell-padding-top: max(18px, env(safe-area-inset-top));
            --shell-padding-bottom: max(18px, env(safe-area-inset-bottom));
            --map-height: clamp(420px, 60vh, 820px);
            --map-min-height: 420px;
        }

        * { box-sizing: border-box; }

        html, body {
            margin: 0;
            min-height: 100%;
            background:
                radial-gradient(circle at 0 0, rgba(46, 230, 192, 0.12), transparent 20%),
                radial-gradient(circle at 100% 0, rgba(62, 162, 255, 0.10), transparent 20%),
                linear-gradient(180deg, #03070f 0%, #07111d 100%);
            color: var(--text);
            font-family: var(--body-font);
        }

        body::before,
        body::after {
            content: "";
            position: fixed;
            inset: 0;
            pointer-events: none;
            z-index: -1;
        }

        body::before {
            opacity: .16;
            background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='180' height='180' viewBox='0 0 180 180'%3E%3Cfilter id='n'%3E%3CfeTurbulence type='fractalNoise' baseFrequency='0.9' numOctaves='2' stitchTiles='stitch'/%3E%3C/filter%3E%3Crect width='180' height='180' filter='url(%23n)' opacity='0.18'/%3E%3C/svg%3E");
            mix-blend-mode: soft-light;
        }

        body::after {
            background:
                radial-gradient(circle at 18% 14%, rgba(46, 230, 192, 0.14), transparent 28%),
                radial-gradient(circle at 82% 20%, rgba(62, 162, 255, 0.12), transparent 26%),
                radial-gradient(circle at 50% 100%, rgba(143, 116, 255, 0.10), transparent 32%);
            opacity: .9;
        }

        body {
            padding: var(--shell-padding-top) var(--shell-padding-right) var(--shell-padding-bottom) var(--shell-padding);
        }

        .shell {
            display: grid;
            gap: 20px;
            max-width: 1860px;
            margin: 0 auto;
            min-height: calc(100vh - var(--shell-padding-top) - var(--shell-padding-bottom));
        }

        .topbar,
        .sidebar,
        .map-stage,
        .headline-strip,
        .panel,
        .floating-card,
        .kpi,
        .route-card,
        .lot-card {
            background: var(--panel);
            border: 1px solid var(--line);
            box-shadow: var(--shadow);
            backdrop-filter: blur(18px);
        }

        .topbar,
        .sidebar,
        .headline-strip,
        .panel,
        .map-stage,
        .floating-card { border-radius: var(--radius-xl); }
        .route-card,
        .lot-card,
        .kpi { border-radius: var(--radius-lg); }

        .topbar {
            padding: 16px 18px;
            display: grid;
            grid-template-columns: minmax(0, 1fr) auto;
            grid-template-areas:
                "brand actions"
                "routes routes";
            gap: 14px 16px;
            align-items: start;
            position: sticky;
            top: 20px;
            z-index: 40;
            background: linear-gradient(180deg, rgba(15, 28, 46, 0.78), rgba(8, 16, 29, 0.9));
            border-color: rgba(132, 169, 227, 0.18);
            backdrop-filter: blur(26px) saturate(1.15);
            box-shadow: 0 24px 60px rgba(0, 0, 0, 0.22), inset 0 1px 0 rgba(255,255,255,0.08);
            overflow: hidden;
        }

        .topbar::after {
            content: "";
            position: absolute;
            inset: auto 18px 0 18px;
            height: 1px;
            background: linear-gradient(90deg, rgba(46,230,192,0), rgba(46,230,192,0.55), rgba(62,162,255,0.55), rgba(46,230,192,0));
            opacity: .75;
        }

        .headline-strip {
            grid-area: headlines;
            padding: 14px;
            display: grid;
            grid-template-columns: repeat(3, minmax(0, 1fr));
            gap: 14px;
            align-self: start;
            align-items: stretch;
        }

        .headline-card {
            min-height: 126px;
            padding: 18px;
            border-radius: var(--radius-lg);
            border: 1px solid rgba(120, 154, 211, 0.14);
            background: linear-gradient(180deg, rgba(14, 28, 47, 0.78), rgba(7, 14, 26, 0.68));
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.04), var(--glow);
            display: grid;
            align-content: start;
            gap: 6px;
            position: relative;
            overflow: hidden;
        }

        .headline-card::after {
            content: "";
            position: absolute;
            inset: auto -18% -42% auto;
            width: 140px;
            height: 140px;
            border-radius: 999px;
            background: radial-gradient(circle, rgba(255,255,255,0.12), rgba(255,255,255,0));
            opacity: .42;
            pointer-events: none;
        }

        .headline-card:nth-child(2)::before {
            background: linear-gradient(180deg, rgba(255,211,138,0.95), rgba(62,162,255,0.78), rgba(143,116,255,0.58));
        }

        .headline-card:nth-child(3)::before {
            background: linear-gradient(180deg, rgba(143,116,255,0.92), rgba(62,162,255,0.78), rgba(46,230,192,0.6));
        }

        @keyframes shell-rise {
            from {
                opacity: 0;
                transform: translateY(18px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        body:not(.is-ready) .topbar,
        body:not(.is-ready) .workspace,
        body:not(.is-ready) .headline-strip,
        body:not(.is-ready) .legal-strip {
            opacity: 0;
            transform: translateY(18px);
        }

        body.is-ready .topbar,
        body.is-ready .workspace,
        body.is-ready .headline-strip,
        body.is-ready .legal-strip {
            animation: shell-rise .56s cubic-bezier(0.22, 1, 0.36, 1) both;
        }

        body.is-ready .workspace {
            animation-delay: .06s;
        }

        body.is-ready .headline-strip {
            animation-delay: .12s;
        }

        body.is-ready .legal-strip {
            animation-delay: .18s;
        }

        .headline-card::before {
            content: "";
            position: absolute;
            inset: 0 auto 0 0;
            width: 3px;
            background: linear-gradient(180deg, rgba(46,230,192,0.95), rgba(62,162,255,0.8), rgba(143,116,255,0.65));
            opacity: .9;
        }

        .headline-card strong {
            display: block;
            font-size: 15px;
            margin-top: 6px;
            line-height: 1.25;
        }

        .legal-strip {
            padding: 16px 18px;
            border-radius: var(--radius-xl);
            background: linear-gradient(180deg, rgba(15, 28, 46, 0.64), rgba(8, 16, 29, 0.84));
            border: 1px solid rgba(132, 169, 227, 0.16);
            box-shadow: var(--shadow-soft), inset 0 1px 0 rgba(255,255,255,0.04);
        }

        .route-pills {
            grid-area: routes;
            display: grid;
            grid-auto-flow: column;
            grid-auto-columns: max-content;
            align-items: stretch;
            justify-self: stretch;
            width: 100%;
            max-width: 100%;
            padding: 8px;
            gap: 10px;
            overflow-x: auto;
            overscroll-behavior-x: contain;
            scroll-snap-type: x proximity;
            border-radius: calc(var(--radius-xl) - 4px);
            background: linear-gradient(180deg, rgba(255,255,255,0.07), rgba(255,255,255,0.025));
            border: 1px solid rgba(142, 178, 232, 0.16);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.09), 0 14px 28px rgba(3, 8, 16, 0.14);
            scrollbar-width: none;
        }

        .route-pills::-webkit-scrollbar { display: none; }

        .brand {
            grid-area: brand;
            display: grid;
            gap: 4px;
        }

        .eyebrow {
            color: var(--muted);
            font-size: 11px;
            letter-spacing: .16em;
            text-transform: uppercase;
        }

        h1,h2,h3,p { margin: 0; }
        .brand h1 {
            font-family: var(--display-font);
            font-size: clamp(26px, 2.2vw, 34px);
            letter-spacing: -.045em;
            line-height: 1.02;
        }
        .helper, .tiny, .muted { color: var(--muted); }

        .toolbar,
        .route-pills,
        .filter-row,
        .legend,
        .detail-pills,
        .status-grid,
        .tabs,
        .selection-meta,
        .panel-list,
        .top-actions,
        .lot-actions { display: flex; flex-wrap: wrap; gap: 8px; }

        .top-actions {
            grid-area: actions;
            display: grid;
            grid-template-columns: minmax(0, auto) minmax(240px, 1fr);
            justify-content: end;
            align-items: end;
            gap: 10px;
            padding: 12px 14px;
            min-height: 68px;
            border-radius: 22px;
            background: linear-gradient(180deg, rgba(255,255,255,0.06), rgba(255,255,255,0.02));
            border: 1px solid rgba(142, 178, 232, 0.12);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.08), var(--glow);
        }

        .toolbar-block {
            display: grid;
            gap: 6px;
            min-width: 0;
        }

        .toolbar-select-block {
            align-self: stretch;
        }

        .control-label {
            color: rgba(192, 208, 233, 0.78);
            font-size: 10px;
            font-weight: 700;
            letter-spacing: .14em;
            text-transform: uppercase;
            padding-left: 4px;
        }

        .top-actions .tabs {
            padding: 4px;
            border-radius: 16px;
            background: linear-gradient(180deg, rgba(6, 14, 24, 0.72), rgba(11, 21, 35, 0.52));
            border: 1px solid rgba(136, 172, 229, 0.14);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.05);
        }

        .filter-stack {
            display: grid;
            gap: 10px;
        }

        .filter-copy {
            line-height: 1.45;
        }

        .toggle-group {
            display: grid;
            gap: 8px;
        }

        .toggle-group-label {
            color: var(--muted);
            font-size: 11px;
            letter-spacing: .12em;
            text-transform: uppercase;
        }

        @keyframes year-pop {
            0%   { transform: translateX(-50%) scale(0.88); opacity: 0; }
            65%  { transform: translateX(-50%) scale(1.04); }
            100% { transform: translateX(-50%) scale(1);    opacity: 1; }
        }

        .year-overlay {
            position: absolute;
            bottom: 90px;
            left: 50%;
            transform: translateX(-50%);
            background: rgba(3, 8, 18, 0.86);
            color: #fff;
            font-size: clamp(42px, 8vw, 96px);
            font-weight: 900;
            letter-spacing: -3px;
            line-height: 1;
            padding: 8px 32px 6px;
            border-radius: 14px;
            pointer-events: none;
            z-index: 1200;
            opacity: 0;
            visibility: hidden;
            transition: opacity 0.38s ease, visibility 0.38s ease;
            font-variant-numeric: tabular-nums;
            backdrop-filter: blur(14px) saturate(1.6);
            -webkit-backdrop-filter: blur(14px) saturate(1.6);
            border: 1px solid rgba(255,255,255,0.12);
            text-shadow: 0 2px 24px rgba(0,0,0,.6);
        }
        .year-overlay.visible {
            opacity: 1;
            visibility: visible;
            animation: year-pop 0.42s cubic-bezier(0.34, 1.56, 0.64, 1) both;
        }
        .year-overlay .year-km-sub {
            display: block;
            font-size: 0.22em;
            font-weight: 500;
            letter-spacing: 0.5px;
            opacity: 0.72;
            margin-top: 2px;
            text-align: center;
        }

        .funding-badge {
            display: inline-block;
            padding: 2px 8px;
            border-radius: 4px;
            font-size: 0.70em;
            font-weight: 600;
            border: 1px solid rgba(80,160,255,0.28);
            background: rgba(74,171,255,0.10);
            color: #74b5f8;
            letter-spacing: 0.03em;
            vertical-align: middle;
        }

        .playback-km-wrap {
            position: relative;
            height: 22px;
            background: rgba(255, 255, 255, 0.055);
            border-radius: 999px;
            overflow: hidden;
            margin-top: 8px;
            border: 1px solid rgba(255, 255, 255, 0.07);
        }
        .playback-km-fill {
            position: absolute;
            left: 0; top: 0; bottom: 0;
            background: linear-gradient(90deg, rgba(46,230,192,0.68) 0%, rgba(62,162,255,0.82) 100%);
            border-radius: 999px;
            width: 0%;
            transition: width 0.55s cubic-bezier(0.4, 0, 0.2, 1);
        }
        .playback-km-label {
            position: absolute;
            inset: 0;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 11px;
            font-weight: 600;
            color: rgba(255,255,255,0.88);
            letter-spacing: 0.3px;
            pointer-events: none;
        }

        .playback-meta {
            display: flex;
            justify-content: space-between;
            gap: 10px;
        }

        .playback-range {
            width: 100%;
            appearance: none;
            height: 6px;
            border-radius: 999px;
            background: linear-gradient(90deg, rgba(62,162,255,0.45), rgba(46,230,192,0.75));
            outline: none;
        }

        .playback-range::-webkit-slider-thumb {
            appearance: none;
            width: 16px;
            height: 16px;
            border-radius: 999px;
            background: #f3fbff;
            border: 2px solid rgba(62,162,255,0.8);
            box-shadow: 0 6px 18px rgba(0,0,0,0.28);
            cursor: pointer;
        }

        .playback-range::-moz-range-thumb {
            width: 16px;
            height: 16px;
            border-radius: 999px;
            background: #f3fbff;
            border: 2px solid rgba(62,162,255,0.8);
            box-shadow: 0 6px 18px rgba(0,0,0,0.28);
            cursor: pointer;
        }

        .workspace {
            display: grid;
            grid-template-columns: minmax(320px, 396px) minmax(0, 1fr);
            grid-template-areas:
                "sidebar map"
                "sidebar headlines";
            gap: 20px;
            align-items: start;
        }

        .sidebar {
            grid-area: sidebar;
            padding: 18px;
            display: grid;
            grid-template-columns: 1fr;
            gap: 14px;
            min-height: 0;
            position: sticky;
            top: 112px;
            max-height: calc(100vh - 132px);
            overflow: auto;
            background: linear-gradient(180deg, rgba(13, 26, 42, 0.82), rgba(7, 14, 26, 0.94));
            backdrop-filter: blur(26px) saturate(1.08);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.06), var(--shadow);
            position: sticky;
        }

        .sidebar-scroll {
            min-height: 0;
            display: grid;
            grid-template-columns: 1fr;
            gap: 12px;
            padding-right: 0;
        }

        .tablet-control-surface,
        .tablet-info-tabs {
            display: none;
        }

        .tablet-surface-summary,
        .tablet-control-card,
        .tablet-playback-card {
            padding: 18px;
            border-radius: 20px;
            border: 1px solid rgba(120, 154, 211, 0.14);
            background: linear-gradient(180deg, rgba(15, 28, 46, 0.9), rgba(7, 14, 26, 0.94));
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.05), var(--shadow-soft), var(--glow);
        }

        .tablet-surface-heading {
            display: grid;
            grid-template-columns: minmax(0, 1fr) auto;
            gap: 14px;
            align-items: start;
        }

        .tablet-surface-heading h3,
        .summary-main h2,
        .panel h3,
        .card-head strong {
            font-family: var(--display-font);
            letter-spacing: -.035em;
            line-height: 1.02;
        }

        .tablet-summary-band,
        .summary-main {
            display: grid;
            gap: 12px;
        }

        .tablet-surface-summary strong,
        .summary-main strong {
            font-family: var(--display-font);
            font-size: clamp(36px, 3vw, 44px);
            line-height: .96;
        }

        .tablet-kpi-ribbon {
            display: grid;
            grid-template-columns: repeat(4, minmax(0, 1fr));
            gap: 10px;
            margin-top: 4px;
        }

        .tablet-kpi-chip {
            padding: 11px 12px;
            border-radius: 16px;
            border: 1px solid rgba(120, 154, 211, 0.14);
            background: linear-gradient(180deg, rgba(255,255,255,0.08), rgba(255,255,255,0.03));
            display: grid;
            gap: 4px;
            min-height: 74px;
            align-content: start;
        }

        .tablet-kpi-chip .value {
            font-size: 18px;
            font-weight: 800;
            color: #f4fbff;
        }

        .tablet-control-cluster,
        .tablet-surface-lower,
        .tablet-surface-tools {
            display: grid;
            gap: 12px;
        }

        .tablet-control-surface {
            overflow: auto;
            scrollbar-width: thin;
            scrollbar-color: rgba(128, 164, 220, 0.42) transparent;
        }

        .tablet-control-surface::-webkit-scrollbar {
            width: 6px;
            height: 6px;
        }

        .tablet-control-surface::-webkit-scrollbar-thumb {
            background: rgba(128, 164, 220, 0.38);
            border-radius: 999px;
        }

        .tablet-control-cluster {
            grid-template-columns: repeat(3, minmax(0, 1fr));
        }

        .tablet-surface-lower {
            grid-template-columns: minmax(300px, 1.1fr) minmax(0, 1fr);
            align-items: start;
        }

        .tablet-surface-tools {
            grid-template-columns: repeat(2, minmax(0, 1fr));
        }

        .tablet-map-preset-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 10px;
        }

        .tablet-info-tabs {
            gap: 12px;
            padding: 16px 18px;
            border-radius: 24px;
            border: 1px solid rgba(120, 154, 211, 0.14);
            background: linear-gradient(180deg, rgba(15, 28, 46, 0.78), rgba(7, 14, 26, 0.9));
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.05), var(--shadow-soft);
        }

        .tablet-tab-row {
            display: grid;
            grid-template-columns: repeat(4, minmax(0, 1fr));
            gap: 8px;
        }

        .tablet-info-tab {
            min-height: 44px;
            justify-content: center;
            font-size: 11px;
        }

        .notes-body {
            display: grid;
            gap: 12px;
        }

        .note-block {
            padding: 14px 15px;
            border-radius: var(--radius-md);
            border: 1px solid rgba(120, 154, 211, 0.14);
            background: rgba(255, 255, 255, 0.025);
            line-height: 1.55;
        }

        .audit-badge {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            border-radius: 999px;
            padding: 5px 10px;
            font-size: 11px;
            border: 1px solid rgba(120, 154, 211, 0.18);
            background: rgba(255,255,255,0.04);
            color: var(--muted);
        }

        .audit-badge.strong {
            color: #dff8f2;
            border-color: rgba(46, 230, 192, 0.35);
            background: rgba(46, 230, 192, 0.10);
        }

        .summary-card {
            padding: 22px;
            border-radius: var(--radius-lg);
            background: linear-gradient(180deg, rgba(16, 31, 52, 0.98), rgba(7, 14, 26, 0.94));
            border: 1px solid rgba(105, 144, 205, 0.18);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.04), var(--shadow-soft);
            position: relative;
            overflow: hidden;
        }

        .summary-card::after,
        .panel::after {
            content: "";
            position: absolute;
            inset: auto -12% -32% auto;
            width: 120px;
            height: 120px;
            border-radius: 999px;
            background: radial-gradient(circle, rgba(255,255,255,0.08), rgba(255,255,255,0));
            pointer-events: none;
        }

        .summary-main {
            display: grid;
            gap: 10px;
        }

        .summary-main strong { font-size: 34px; letter-spacing: -.05em; }

        .progress-track {
            width: 100%;
            height: 8px;
            background: rgba(255,255,255,0.08);
            border-radius: 999px;
            overflow: hidden;
        }

        .progress-fill {
            height: 100%;
            width: 0;
            background: linear-gradient(90deg, var(--accent), #9afde8);
            border-radius: inherit;
            transition: width .45s ease;
        }

        .progress-mini {
            width: 100%;
            height: 7px;
            border-radius: 999px;
            background: rgba(255,255,255,0.08);
            overflow: hidden;
            margin-top: 10px;
        }

        .progress-mini > span {
            display: block;
            height: 100%;
            width: 0;
            border-radius: inherit;
            background: linear-gradient(90deg, var(--accent), #9afde8);
        }

        .kpi-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 10px;
        }

        .kpi { padding: 12px; }
        .kpi .value { font-size: 22px; font-weight: 800; margin-top: 6px; }
        .kpi.interactive { cursor: pointer; transition: transform .15s ease, border-color .15s ease, box-shadow .15s ease; }
        .kpi.interactive:hover { transform: translateY(-1px); border-color: rgba(62, 162, 255, 0.4); box-shadow: inset 0 1px 0 rgba(255,255,255,0.06), 0 18px 34px rgba(4, 10, 18, 0.22); }
        .kpi.interactive.active {
            background: linear-gradient(180deg, rgba(17, 38, 54, 0.96), rgba(8, 18, 29, 0.96));
            border-color: rgba(46, 230, 192, 0.42);
        }

        .pill,
        .chip,
        .toggle,
        .select,
        .status-pill,
        .micro,
        .route-pill,
        .tab {
            border-radius: 999px;
            border: 1px solid rgba(120, 154, 211, 0.20);
            background: linear-gradient(180deg, rgba(255,255,255,0.12), rgba(255,255,255,0.03));
            color: var(--text);
            padding: 8px 12px;
            font-size: 12px;
            transition: .18s ease;
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.08), 0 10px 24px rgba(4, 10, 18, 0.18);
            backdrop-filter: blur(18px) saturate(1.08);
        }

        .chip,
        .toggle,
        .route-pill,
        .tab { cursor: pointer; }
        .chip:hover,.toggle:hover,.route-pill:hover,.route-card:hover,.lot-card:hover,.tab:hover { transform: translateY(-1px); border-color: rgba(62, 162, 255, 0.4); }
        .chip.active,.toggle.active,.route-pill.active,.tab.active {
            background: linear-gradient(180deg, rgba(119, 221, 255, 0.18), rgba(46, 230, 192, 0.12));
            border-color: rgba(136, 210, 255, 0.44);
            color: #f6ffff;
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.16), 0 14px 28px rgba(24, 113, 155, 0.18);
        }
        .select { appearance: none; cursor: pointer; }

        .status-pill {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            font-weight: 700;
        }

        .legend .status-pill {
            cursor: pointer;
        }

        .route-pill {
            min-width: 0;
            min-width: 144px;
            min-height: 60px;
            padding: 12px 14px;
            display: grid;
            gap: 4px;
            align-content: center;
            text-align: left;
            scroll-snap-align: start;
            background: linear-gradient(180deg, rgba(255,255,255,0.14), rgba(255,255,255,0.04));
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.1), 0 12px 24px rgba(6, 12, 22, 0.16);
            position: relative;
            overflow: hidden;
        }

        .route-pill::after {
            content: "";
            position: absolute;
            inset: 0;
            background: linear-gradient(120deg, rgba(255,255,255,0), rgba(255,255,255,0.08), rgba(255,255,255,0));
            opacity: 0;
            transform: translateX(-120%);
            transition: transform .5s ease, opacity .22s ease;
            pointer-events: none;
        }

        .route-pill:hover::after,
        .route-pill.active::after {
            opacity: 1;
            transform: translateX(120%);
        }

        .route-pill .meta {
            font-size: 10px;
            letter-spacing: .04em;
            color: rgba(193, 210, 235, 0.74);
        }

        .route-pill-progress {
            width: 100%;
            height: 4px;
            border-radius: 999px;
            background: rgba(255,255,255,0.08);
            overflow: hidden;
            margin-top: 2px;
        }

        .route-pill-progress > span {
            display: block;
            height: 100%;
            border-radius: inherit;
            background: linear-gradient(90deg, var(--accent), var(--accent-3));
            box-shadow: 0 0 12px rgba(62, 162, 255, 0.3);
        }

        .route-pill.active .meta {
            color: rgba(233, 248, 255, 0.92);
        }

        .tab {
            min-width: 58px;
            justify-content: center;
            font-weight: 800;
            letter-spacing: .08em;
            text-transform: uppercase;
            background: transparent;
            box-shadow: none;
        }

        .top-actions .tab.active {
            background: linear-gradient(180deg, rgba(119, 221, 255, 0.2), rgba(46, 230, 192, 0.14));
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.14), 0 10px 22px rgba(24, 113, 155, 0.16);
        }

        .top-actions .select {
            width: 100%;
            min-height: 42px;
            padding-right: 34px;
        }

        .toggle {
            display: inline-flex;
            align-items: center;
            justify-content: space-between;
            gap: 8px;
            min-height: 40px;
        }

        .toggle.stage-toggle {
            min-width: 126px;
            padding: 10px 12px;
            background: linear-gradient(180deg, rgba(255,255,255,0.14), rgba(255,255,255,0.05));
        }

        .toggle.stage-toggle.active {
            border-color: rgba(136, 210, 255, 0.55);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.16), 0 16px 32px rgba(24, 113, 155, 0.22);
        }

        .toggle.stage-toggle .label-stack {
            display: grid;
            gap: 2px;
            text-align: left;
        }

        .toggle.stage-toggle .label-stack strong {
            font-size: 12px;
        }

        .toggle.stage-toggle .label-stack span {
            color: var(--muted);
            font-size: 10px;
            letter-spacing: .08em;
            text-transform: uppercase;
        }

        .route-pill .code {
            font-size: 11px;
            letter-spacing: .14em;
            text-transform: uppercase;
            color: var(--muted);
        }

        .route-pill .label {
            font-size: 13px;
            font-weight: 700;
            letter-spacing: -.02em;
            line-height: 1.2;
        }

        .map-stage {
            grid-area: map;
            position: relative;
            padding: 20px;
            overflow: hidden;
            min-height: var(--map-min-height);
            background:
                radial-gradient(circle at 14% 12%, rgba(92, 150, 255, 0.12), transparent 24%),
                radial-gradient(circle at 86% 14%, rgba(46, 230, 192, 0.10), transparent 22%),
                linear-gradient(180deg, rgba(12, 20, 34, 0.82), rgba(5, 10, 20, 0.96));
            border-color: rgba(126, 165, 223, 0.16);
            backdrop-filter: blur(18px) saturate(1.04);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.07), 0 26px 70px rgba(0,0,0,0.22);
        }

        .map-stage-quickbar {
            display: none;
            position: relative;
            z-index: 3;
            margin-bottom: 14px;
            padding: 12px;
            border-radius: 18px;
            border: 1px solid rgba(136, 172, 229, 0.16);
            background: linear-gradient(180deg, rgba(10, 20, 34, 0.92), rgba(7, 14, 26, 0.9));
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.06), var(--shadow-soft);
            backdrop-filter: blur(18px) saturate(1.08);
            gap: 10px;
        }

        .map-stage-quick-presets {
            display: grid;
            grid-auto-flow: column;
            grid-auto-columns: minmax(138px, max-content);
            gap: 10px;
            overflow-x: auto;
            overscroll-behavior-x: contain;
            scrollbar-width: none;
        }

        .map-stage-quick-presets::-webkit-scrollbar {
            display: none;
        }

        .map-stage-quickbar .toggle.stage-toggle {
            min-width: 138px;
            min-height: 54px;
            padding: 10px 12px;
            scroll-snap-align: start;
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.08), 0 12px 24px rgba(3, 10, 18, 0.18);
        }

        .desktop-map-dock {
            display: none;
            position: relative;
            z-index: 3;
            gap: 12px;
            padding: 12px 14px;
            border-radius: 18px;
            border: 1px solid rgba(136, 172, 229, 0.16);
            background: linear-gradient(180deg, rgba(9, 18, 31, 0.92), rgba(6, 12, 23, 0.88));
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.06), var(--shadow-soft);
            backdrop-filter: blur(18px) saturate(1.08);
            align-items: center;
        }

        .desktop-map-dock-main {
            display: grid;
            grid-template-columns: repeat(4, minmax(0, 1fr));
            gap: 10px;
            flex: 1 1 auto;
        }

        .desktop-map-dock .legend {
            justify-content: flex-end;
            flex: 0 0 auto;
            max-width: 44%;
        }

        .desktop-map-dock .legend .status-pill {
            min-height: 38px;
            padding: 8px 12px;
        }

        .map-stage::before {
            content: "";
            position: absolute;
            inset: 0;
            pointer-events: none;
            background:
                radial-gradient(circle at 50% 0%, rgba(255,255,255,0.08), transparent 42%),
                linear-gradient(180deg, rgba(255,255,255,0.03), transparent 30%, transparent 72%, rgba(0,0,0,0.22));
            opacity: .85;
        }

        #map {
            width: 100%;
            height: var(--map-height);
            min-height: var(--map-min-height);
            border-radius: calc(var(--radius-xl) - 4px);
            border: 1px solid rgba(136, 172, 229, 0.22);
            overflow: hidden;
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.08), 0 18px 44px rgba(0, 0, 0, 0.24);
            transform: translateZ(0);
            isolation: isolate;
        }

        .floating-card {
            position: absolute;
            z-index: 2;
            background: var(--panel-elevated);
            padding: 16px;
            border-color: rgba(132, 169, 227, 0.2);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.05), var(--shadow), var(--glow);
            backdrop-filter: blur(26px) saturate(1.1);
            transition: opacity .2s ease, transform .24s ease, width .24s ease, max-height .24s ease;
        }

        .floating-card::before {
            content: "";
            position: absolute;
            inset: 0 0 auto 0;
            height: 1px;
            background: linear-gradient(90deg, rgba(255,255,255,0.22), rgba(255,255,255,0));
            pointer-events: none;
        }

        .floating-card::after {
            content: "";
            position: absolute;
            inset: auto 14px 14px auto;
            width: 54px;
            height: 54px;
            border-radius: 50%;
            background: radial-gradient(circle, rgba(62,162,255,0.18), rgba(62,162,255,0));
            pointer-events: none;
            opacity: .6;
        }

        .floating-top-left {
            top: 18px;
            left: 18px;
            width: min(39%, 472px);
            background: linear-gradient(180deg, rgba(11, 24, 40, 0.96), rgba(8, 17, 30, 0.94));
            display: grid;
            gap: 12px;
        }

        .floating-top-right,
        .floating-bottom-right {
            width: min(300px, calc(100% - 36px));
            max-height: none;
            overflow: visible;
        }

        .floating-top-right {
            top: 18px;
            right: 18px;
            width: min(360px, calc(100% - 36px));
        }

        .floating-top-right.compact {
            width: min(312px, calc(100% - 36px));
        }

        .floating-top-right.passive {
            width: min(244px, calc(100% - 36px));
            padding: 12px;
            background: linear-gradient(180deg, rgba(10, 21, 36, 0.76), rgba(6, 13, 24, 0.82));
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.04), 0 18px 38px rgba(0,0,0,0.18);
        }

        .floating-top-right.compact #selection-note,
        .floating-top-right.compact #selection-facts,
        .floating-top-right.passive #selection-note,
        .floating-top-right.passive #selection-facts,
        .floating-top-right.passive #selection-pills,
        .floating-top-right.passive #selection-clear {
            display: none;
        }

        .floating-top-right.passive .selection-header {
            align-items: center;
        }

        .floating-top-right.passive .selection-title {
            font-size: 18px;
            line-height: 1.1;
        }

        .floating-top-right.passive #selection-subtitle {
            margin-top: 4px;
            font-size: 12px;
            line-height: 1.45;
        }

        .floating-top-right.passive #selection-status {
            opacity: .88;
            font-size: 11px;
        }

        .floating-bottom-right {
            right: 18px;
            bottom: 18px;
            max-height: 260px;
            overflow: auto;
        }

        .floating-bottom-left {
            left: 18px;
            bottom: 18px;
            width: min(360px, calc(100% - 36px));
        }

        .map-tools {
            display: grid;
            gap: 12px;
            background: linear-gradient(180deg, rgba(8, 16, 29, 0.92), rgba(5, 10, 20, 0.96));
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.06), var(--glow);
        }

        .map-preset-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 8px;
        }

        .map-preset {
            display: grid;
            gap: 5px;
            align-content: start;
            min-height: 74px;
            border-radius: 16px;
            border: 1px solid rgba(120, 154, 211, 0.18);
            background: linear-gradient(180deg, rgba(255,255,255,0.08), rgba(255,255,255,0.03));
            color: var(--text);
            padding: 12px 13px;
            text-align: left;
            cursor: pointer;
            transition: transform .16s ease, border-color .16s ease, background .16s ease;
        }

        .map-preset:hover {
            transform: translateY(-1px);
            border-color: rgba(136, 210, 255, 0.4);
        }

        .map-preset.active {
            border-color: rgba(136, 210, 255, 0.52);
            background: linear-gradient(180deg, rgba(67, 113, 172, 0.28), rgba(23, 38, 62, 0.44));
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.12), 0 16px 28px rgba(4, 10, 18, 0.18);
        }

        .map-preset strong {
            font-size: 12px;
            letter-spacing: -.01em;
        }

        .map-preset span {
            font-size: 10px;
            letter-spacing: .08em;
            text-transform: uppercase;
            color: var(--muted);
        }

        body.device-desktop:not(.viewport-short) .workspace {
            grid-template-columns: minmax(300px, 356px) minmax(0, 1fr);
        }

        body.device-desktop:not(.viewport-short) .map-stage {
            display: grid;
            grid-template-columns: minmax(0, 1fr) minmax(284px, 318px);
            grid-template-areas:
                "quick quick"
                "dock dock"
                "map map"
                "hero selection"
                "timeline timeline";
            align-items: start;
            gap: 16px;
        }

        body.device-desktop:not(.viewport-short) .map-stage-quickbar {
            display: grid;
            grid-area: quick;
            margin-bottom: 0;
        }

        body.device-desktop:not(.viewport-short) .desktop-map-dock {
            display: flex;
            grid-area: dock;
        }

        body.device-desktop:not(.viewport-short) #map {
            grid-area: map;
        }

        body.device-desktop:not(.viewport-short) .floating-card {
            position: static;
            width: auto;
            max-height: none;
            overflow: visible;
        }

        body.device-desktop:not(.viewport-short) .floating-top-left {
            grid-area: hero;
            width: auto;
        }

        body.device-desktop:not(.viewport-short) .floating-top-right {
            grid-area: selection;
            width: auto;
        }

        body.device-desktop:not(.viewport-short) .floating-bottom-left {
            display: none;
        }

        body.device-desktop:not(.viewport-short) .floating-bottom-right {
            grid-area: timeline;
            width: auto;
            max-height: none;
        }

        body.device-desktop:not(.viewport-short) {
            --map-height: clamp(540px, 65vh, 860px);
            --map-min-height: 560px;
        }

        body.device-desktop:not(.viewport-short) .year-overlay {
            top: calc(var(--map-height) - 120px);
            bottom: auto;
        }

        body.device-tablet .workspace,
        body.device-desktop.viewport-short .workspace {
            grid-template-columns: 1fr;
            grid-template-areas:
                "map"
                "headlines"
                "sidebar";
        }

        body.device-tablet .sidebar,
        body.device-desktop.viewport-short .sidebar {
            position: static;
            max-height: none;
            overflow: visible;
        }

        body.device-tablet .floating-card,
        body.device-desktop.viewport-short .floating-card {
            position: static;
        }

        body.device-tablet .map-stage,
        body.device-desktop.viewport-short .map-stage {
            display: grid;
            gap: 14px;
            min-height: auto;
        }

        body.device-tablet .map-stage {
            grid-template-columns: minmax(320px, 420px) minmax(0, 1fr);
            grid-template-areas:
                "quick quick"
                "surface map"
                "selection map";
            align-items: start;
            gap: 12px;
            padding: 16px;
            min-height: min(860px, calc(100vh - 174px));
        }

        body.device-tablet .map-stage-quickbar {
            grid-area: quick;
            margin-bottom: 0;
        }

        body.device-tablet .map-stage-quickbar,
        body.device-phone .map-stage-quickbar,
        body.device-desktop.viewport-short .map-stage-quickbar {
            display: grid;
        }

        body.device-tablet .tablet-control-surface {
            display: grid;
            grid-area: surface;
            gap: 10px;
            max-height: calc(100vh - 280px);
        }

        body.device-tablet .tablet-surface-summary,
        body.device-tablet .tablet-control-card,
        body.device-tablet .tablet-playback-card {
            padding: 13px 14px;
            border-radius: 16px;
        }

        body.device-tablet .tablet-surface-heading h3 {
            font-size: 25px;
        }

        body.device-tablet .tablet-surface-summary strong {
            font-size: clamp(38px, 4.2vw, 46px);
        }

        body.device-tablet .tablet-kpi-ribbon {
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 8px;
        }

        body.device-tablet .tablet-kpi-chip {
            min-height: 62px;
            padding: 9px 10px;
        }

        body.device-tablet .tablet-kpi-chip .value {
            font-size: 16px;
        }

        body.device-tablet .tablet-control-cluster {
            grid-template-columns: 1fr;
            gap: 9px;
        }

        body.device-tablet .tablet-control-card .filter-row {
            display: grid;
            grid-auto-flow: column;
            grid-auto-columns: max-content;
            overflow-x: auto;
            overscroll-behavior-x: contain;
            scrollbar-width: none;
            gap: 8px;
        }

        body.device-tablet .tablet-control-card .filter-row::-webkit-scrollbar {
            display: none;
        }

        body.device-tablet .tablet-control-card .toggle.stage-toggle {
            min-height: 38px;
            min-width: 122px;
            padding: 8px 10px;
        }

        body.device-tablet .tablet-control-card .toggle.stage-toggle .label-stack span {
            display: none;
        }

        body.device-tablet .tablet-surface-lower,
        body.device-tablet .tablet-surface-tools {
            grid-template-columns: 1fr;
            gap: 9px;
        }

        body.device-tablet .tablet-map-preset-grid {
            grid-template-columns: repeat(3, minmax(0, 1fr));
            gap: 8px;
        }

        body.device-tablet .tablet-map-preset-grid .map-preset {
            min-height: 56px;
            padding: 8px 10px;
        }

        body.device-tablet .tablet-map-preset-grid .map-preset span {
            display: none;
        }

        body.device-tablet .floating-top-left,
        body.device-tablet .floating-bottom-right,
        body.device-tablet .floating-bottom-left {
            display: none;
        }

        body.device-tablet .floating-top-right {
            grid-area: selection;
            order: initial;
            max-height: 210px;
            overflow: auto;
        }

        body.device-tablet .floating-top-right.passive {
            display: none;
        }

        body.device-tablet #map {
            grid-area: map;
            order: initial;
            height: 100%;
            min-height: 620px;
        }

        body.device-tablet .floating-top-left,
        body.device-tablet .floating-top-right,
        body.device-tablet .floating-bottom-right,
        body.device-tablet .floating-bottom-left,
        body.device-desktop.viewport-short .floating-top-left,
        body.device-desktop.viewport-short .floating-top-right,
        body.device-desktop.viewport-short .floating-bottom-right,
        body.device-desktop.viewport-short .floating-bottom-left {
            width: auto;
            max-height: none;
            overflow: visible;
        }

        body.device-tablet .topbar,
        body.device-phone .topbar,
        body.device-desktop.viewport-short .topbar {
            position: static;
        }

        body.device-tablet .top-actions,
        body.device-phone .top-actions,
        body.device-desktop.viewport-short .top-actions {
            grid-template-columns: 1fr;
            border-radius: 18px;
        }

        body.device-tablet .top-actions .tabs,
        body.device-tablet .top-actions .select,
        body.device-tablet .toolbar-block,
        body.device-phone .top-actions .tabs,
        body.device-phone .top-actions .select,
        body.device-phone .toolbar-block,
        body.device-desktop.viewport-short .top-actions .tabs,
        body.device-desktop.viewport-short .top-actions .select,
        body.device-desktop.viewport-short .toolbar-block {
            width: 100%;
        }

        body.device-tablet .headline-strip {
            grid-template-columns: repeat(2, minmax(0, 1fr));
        }

        body.device-tablet .headline-strip {
            display: none;
        }

        body.device-tablet .sidebar {
            padding: 0;
            gap: 14px;
            background: transparent;
            border: 0;
            box-shadow: none;
            backdrop-filter: none;
        }

        body.device-tablet .tablet-info-tabs {
            display: grid;
        }

        body.device-tablet #summary-panel,
        body.device-tablet #filters-panel {
            display: none;
        }

        body.device-tablet .sidebar-scroll {
            gap: 14px;
        }

        body.device-phone .headline-strip,
        body.device-desktop.viewport-short .headline-strip {
            grid-template-columns: 1fr;
        }

        body.device-tablet,
        body.device-desktop.viewport-short {
            --map-height: min(72vh, 760px);
            --map-min-height: 620px;
        }

        body.device-tablet {
            --map-height: min(40vh, 470px);
            --map-min-height: 360px;
        }

        body.device-phone {
            --map-height: 54vh;
            --map-min-height: 420px;
        }

        .stats-row {
            display: grid;
            grid-template-columns: repeat(4, minmax(0, 1fr));
            gap: 8px;
        }

        .hero-topline {
            display: grid;
            grid-template-columns: minmax(0, 1fr) auto;
            gap: 10px;
            align-items: start;
        }

        .hero-focus {
            appearance: none;
            display: inline-flex;
            align-items: center;
            gap: 8px;
            padding: 10px 14px;
            border-radius: 999px;
            border: 1px solid rgba(136, 172, 229, 0.18);
            background: rgba(255,255,255,0.04);
            color: #dbe9ff;
            font-size: 12px;
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.05), var(--glow);
            cursor: pointer;
            transition: .18s ease;
        }

        .hero-focus:hover {
            transform: translateY(-1px);
            border-color: rgba(136, 210, 255, 0.36);
        }

        .hero-focus.active {
            background: linear-gradient(180deg, rgba(119, 221, 255, 0.18), rgba(46, 230, 192, 0.12));
            border-color: rgba(136, 210, 255, 0.44);
            color: #f6ffff;
        }

        .map-stage.command-center .floating-top-left,
        .map-stage.command-center .floating-bottom-right,
        .map-stage.command-center .floating-bottom-left {
            opacity: 0;
            transform: translateY(-10px) scale(.985);
            pointer-events: none;
        }

        .map-stage.command-center .floating-top-right {
            width: min(280px, calc(100% - 36px));
        }

        .map-stage.command-center #map {
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.08), 0 28px 72px rgba(0, 0, 0, 0.32);
        }

        body.device-desktop:not(.viewport-short).command-center-mode .headline-strip {
            display: none;
        }

        body.device-desktop:not(.viewport-short).command-center-mode .sidebar {
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.04), 0 18px 42px rgba(0, 0, 0, 0.2);
            border-color: rgba(120, 154, 211, 0.12);
        }

        body.device-desktop:not(.viewport-short).command-center-mode .map-stage {
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.08), 0 32px 82px rgba(0,0,0,0.28);
        }

        .hero-kpis {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 10px;
        }

        .hero-kpis .kpi {
            padding: 10px;
            min-height: 74px;
        }

        .hero-kpis .kpi .value {
            font-size: 18px;
        }

        .route-card,
        .lot-card,
        .panel {
            padding: 20px 18px;
            background: linear-gradient(180deg, rgba(11, 22, 37, 0.88), rgba(7, 14, 26, 0.94));
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.04), var(--shadow-soft), var(--glow);
            position: relative;
            overflow: hidden;
        }

        .route-card.active,
        .lot-card.active,
        .tab.active {
            background: linear-gradient(180deg, rgba(17, 38, 54, 0.96), rgba(8, 18, 29, 0.96));
            border-color: rgba(46, 230, 192, 0.42);
        }

        .lot-card.derived {
            border-style: dashed;
            opacity: 0.9;
        }

        .card-head,
        .row {
            display: flex;
            justify-content: space-between;
            gap: 12px;
            align-items: start;
        }

        .route-list,
        .lot-list,
        .timeline { display: grid; gap: 12px; }

        .lot-marker {
            display: grid;
            place-items: center;
            width: 18px;
            height: 18px;
            border-radius: 999px;
            background: rgba(6, 14, 24, 0.88);
            border: 1.5px solid rgba(255,255,255,0.88);
            box-shadow: 0 8px 18px rgba(0,0,0,0.24);
            backdrop-filter: blur(10px);
        }

        .lot-marker-core {
            width: 5px;
            height: 5px;
            border-radius: 999px;
            background: white;
            box-shadow: 0 0 8px rgba(255,255,255,0.34);
        }

        .lot-marker.selected {
            transform: scale(1.1);
            border-color: rgba(255,255,255,0.98);
            box-shadow: 0 0 0 4px rgba(137, 182, 255, 0.14), 0 14px 28px rgba(0,0,0,0.3);
        }

        .segment-chip {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            padding: 5px 9px;
            border-radius: 999px;
            background: rgba(8, 16, 28, 0.78);
            border: 1px solid rgba(255,255,255,0.14);
            color: #e9f3ff;
            font-size: 10px;
            font-weight: 700;
            letter-spacing: .06em;
            text-transform: uppercase;
            box-shadow: 0 10px 24px rgba(0,0,0,0.24);
            backdrop-filter: blur(14px);
            white-space: nowrap;
        }

        .segment-chip .dot {
            width: 8px;
            height: 8px;
        }

        .selection-header {
            display: flex;
            justify-content: space-between;
            gap: 14px;
            align-items: start;
        }

        .selection-actions {
            display: grid;
            justify-items: end;
            gap: 8px;
            min-width: 114px;
        }

        .selection-clear {
            appearance: none;
            border: 1px solid rgba(136, 172, 229, 0.2);
            border-radius: 999px;
            padding: 8px 12px;
            min-height: 36px;
            background: linear-gradient(180deg, rgba(255,255,255,0.10), rgba(255,255,255,0.03));
            color: #f4fbff;
            font-size: 11px;
            font-weight: 700;
            letter-spacing: .06em;
            text-transform: uppercase;
            cursor: pointer;
            transition: transform .16s ease, border-color .16s ease, background .16s ease;
        }

        .selection-clear:hover {
            transform: translateY(-1px);
            border-color: rgba(136, 210, 255, 0.38);
            background: linear-gradient(180deg, rgba(119, 221, 255, 0.16), rgba(46, 230, 192, 0.10));
        }

        .selection-clear[hidden] {
            display: none;
        }

        .selection-title {
            font-family: var(--display-font);
            font-size: clamp(24px, 2vw, 30px);
            line-height: 1.04;
            letter-spacing: -.04em;
            text-wrap: balance;
        }
        .badge-line { display: inline-flex; align-items: center; gap: 8px; }
        .dot { width: 10px; height: 10px; border-radius: 999px; display: inline-block; }

        .selection-note {
            position: relative;
            overflow: hidden;
            padding: 13px 14px 13px 18px;
            border-radius: 14px;
            background: linear-gradient(180deg, rgba(255,255,255,0.05), rgba(255,255,255,0.02));
            border: 1px solid rgba(136, 172, 229, 0.12);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.04);
        }

        .selection-note::before {
            content: "";
            position: absolute;
            inset: 0 auto 0 0;
            width: 3px;
            background: linear-gradient(180deg, var(--accent-2), var(--accent));
            opacity: .9;
        }

        .fact-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0,1fr));
            gap: 10px;
        }

        .fact {
            padding: 13px;
            border-radius: var(--radius-md);
            background: linear-gradient(180deg, rgba(255,255,255,0.04), rgba(255,255,255,0.02));
            border: 1px solid rgba(255,255,255,0.06);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.04);
        }

        .fact strong {
            font-size: 11px;
            letter-spacing: .08em;
            text-transform: uppercase;
            color: #eef7ff;
        }

        .fact a {
            color: var(--accent-2);
            text-decoration: none;
            border-bottom: 1px solid rgba(62, 162, 255, 0.45);
        }

        .fact a:hover {
            color: #8fcbff;
            border-bottom-color: rgba(143, 203, 255, 0.9);
        }

        .timeline-item {
            display: grid;
            grid-template-columns: 56px 1fr;
            gap: 12px;
            align-items: start;
            padding: 12px 0;
            border-bottom: 1px solid rgba(255,255,255,0.05);
            position: relative;
        }

        .timeline-item::before {
            content: "";
            position: absolute;
            left: 27px;
            top: 0;
            bottom: 0;
            width: 1px;
            background: linear-gradient(180deg, rgba(62,162,255,0), rgba(62,162,255,0.28), rgba(46,230,192,0));
            pointer-events: none;
        }

        .timeline-item:last-child { border-bottom: 0; }
        .timeline-year { color: var(--accent); font-weight: 700; font-size: 13px; position: relative; z-index: 1; }
        .timeline-year.success { color: var(--open); }
        .timeline-year.warning { color: var(--construction); }
        .timeline-year.danger  { color: var(--closed); }
        .timeline-year.info    { color: var(--accent-2); }
        .timeline-state-badge {
            display: inline-flex;
            align-items: center;
            gap: 4px;
            font-size: 10px;
            font-weight: 600;
            padding: 2px 7px;
            border-radius: 999px;
            margin-top: 4px;
            letter-spacing: 0.3px;
            text-transform: uppercase;
        }
        .timeline-state-badge.success { background: rgba(46,230,192,0.14); color: var(--open); }
        .timeline-state-badge.warning { background: rgba(255,179,87,0.14); color: var(--construction); }
        .timeline-state-badge.danger  { background: rgba(255,100,138,0.14); color: var(--closed); }
        .timeline-state-badge.info    { background: rgba(62,162,255,0.14); color: var(--accent-2); }

        .mini-note {
            padding: 10px 12px;
            border-radius: 12px;
            background: rgba(255,255,255,0.04);
            border: 1px solid rgba(255,255,255,0.05);
        }

        .leaflet-container {
            background: linear-gradient(180deg, #0b1524 0%, #0d1b2d 100%);
            font-family: var(--body-font);
            transform: translateZ(0);
        }

        .leaflet-pane.leaflet-tile-pane img {
            filter: saturate(1.08) contrast(1.04) brightness(1.03);
        }

        .leaflet-tile {
            outline: 1px solid transparent;
            backface-visibility: hidden;
            -webkit-backface-visibility: hidden;
            image-rendering: auto;
        }

        .leaflet-zoom-animated,
        .leaflet-pane,
        .leaflet-tile-container {
            transform: translateZ(0);
            will-change: transform;
        }

        .leaflet-bar,
        .leaflet-control-zoom a,
        .leaflet-control-layers {
            background: rgba(10, 19, 33, 0.82);
            border-color: rgba(131, 168, 225, 0.2);
            color: var(--text);
            box-shadow: var(--shadow-soft), inset 0 1px 0 rgba(255,255,255,0.05);
            backdrop-filter: blur(14px);
        }

        .leaflet-control-zoom a {
            width: 34px;
            height: 34px;
            line-height: 34px;
        }

        .leaflet-popup-content-wrapper, .leaflet-popup-tip {
            background: linear-gradient(180deg, rgba(10, 19, 33, 0.96), rgba(5, 12, 22, 0.98));
            color: var(--text);
            border: 1px solid rgba(136, 172, 229, 0.24);
            box-shadow: 0 20px 60px rgba(0, 0, 0, 0.4), inset 0 1px 0 rgba(255,255,255,0.05);
        }

        .leaflet-control-attribution { background: rgba(8, 14, 25, 0.74); color: var(--muted); }

        .atlas-popup-card .leaflet-popup-content {
            margin: 12px 12px 13px;
        }

        .atlas-popup-card .leaflet-popup-content-wrapper {
            border-radius: 20px;
            min-width: 0;
        }

        .atlas-popup-card .leaflet-popup-close-button {
            color: #e6f0ff;
            opacity: .86;
            top: 10px;
            right: 10px;
            width: 28px;
            height: 28px;
            border-radius: 999px;
            background: rgba(255,255,255,0.06);
            line-height: 28px;
            text-align: center;
        }

        .atlas-popup-card .leaflet-popup-close-button:hover {
            opacity: 1;
            background: rgba(255,255,255,0.1);
        }

        .map-popup {
            min-width: 236px;
            max-width: 292px;
            display: grid;
            gap: 10px;
        }

        .map-popup strong {
            font-size: 14px;
            letter-spacing: -.02em;
        }

        .popup-header {
            display: grid;
            gap: 5px;
            padding-right: 24px;
        }

        .popup-subtitle {
            color: rgba(219, 231, 247, 0.72);
            font-size: 11px;
            line-height: 1.4;
        }

        .popup-metrics {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 7px;
        }

        .popup-metric {
            padding: 9px 10px;
            border-radius: 12px;
            border: 1px solid rgba(120,154,211,0.14);
            background: rgba(255,255,255,0.035);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.03);
        }

        .popup-metric .eyebrow {
            margin-bottom: 3px;
        }

        .popup-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 8px;
        }

        .popup-chip {
            padding: 10px;
            border-radius: 12px;
            border: 1px solid rgba(120,154,211,0.16);
            background: rgba(255,255,255,0.04);
            box-shadow: inset 0 1px 0 rgba(255,255,255,0.04);
        }

        .popup-lot-list {
            display: grid;
            gap: 8px;
        }

        .popup-menu {
            display: flex;
            flex-wrap: wrap;
            gap: 6px;
        }

        .popup-note {
            padding: 9px 11px;
            border-radius: 12px;
            background: rgba(255,255,255,0.035);
            border: 1px solid rgba(136, 172, 229, 0.1);
        }

        .popup-footer {
            font-size: 11px;
            color: rgba(219, 231, 247, 0.68);
            border-top: 1px solid rgba(255,255,255,0.06);
            padding-top: 9px;
        }

        .official-list {
            display: grid;
            gap: 8px;
        }

        .official-item {
            padding: 10px 12px;
            border-radius: 12px;
            background: rgba(255,255,255,0.04);
            border: 1px solid rgba(136, 172, 229, 0.14);
            display: grid;
            gap: 6px;
        }

        .official-item a {
            color: var(--accent-2);
            text-decoration: none;
        }

        .official-item a:hover {
            color: #8fcbff;
        }

        .popup-lot-item {
            padding: 10px 12px;
            border-radius: 12px;
            background: rgba(255,255,255,0.04);
            border: 1px solid rgba(136, 172, 229, 0.14);
            display: grid;
            gap: 6px;
        }

        @media (max-width: 1480px) {
            .workspace { grid-template-columns: minmax(300px, 360px) minmax(0, 1fr); }
            .floating-top-left { width: min(42%, 420px); }
        }

        @media (max-width: 1350px) {
            .workspace {
                grid-template-columns: 1fr;
                grid-template-areas:
                    "map"
                    "headlines"
                    "sidebar";
            }
            .sidebar {
                position: static;
                max-height: none;
                overflow: visible;
            }
            .floating-card {
                position: static;
            }
            .map-stage {
                display: grid;
                gap: 14px;
                min-height: auto;
            }
            .map-stage-quickbar {
                display: grid;
            }

            .desktop-map-dock {
                display: none;
            }
            :root {
                --map-height: min(72vh, 760px);
                --map-min-height: 640px;
            }
            .floating-top-left,
            .floating-top-right,
            .floating-bottom-right,
            .floating-bottom-left {
                width: auto;
                max-height: none;
                overflow: visible;
            }
        }

        @media (max-width: 1180px) {
            :root {
                --shell-padding: max(14px, env(safe-area-inset-left));
                --shell-padding-right: max(14px, env(safe-area-inset-right));
                --shell-padding-top: max(14px, env(safe-area-inset-top));
                --shell-padding-bottom: max(14px, env(safe-area-inset-bottom));
                --map-height: min(68vh, 700px);
                --map-min-height: 560px;
            }
            .topbar {
                position: static;
                gap: 12px;
            }
            .top-actions {
                grid-template-columns: 1fr;
                border-radius: 18px;
            }
            .top-actions .tabs,
            .top-actions .select,
            .toolbar-block {
                width: 100%;
            }
            .map-stage {
                padding: 16px;
                gap: 12px;
            }
            .map-preset-grid {
                grid-template-columns: repeat(4, minmax(0, 1fr));
            }
        }

        @media (max-width: 1024px) {
            .route-pill {
                min-width: 138px;
            }
            .selection-header {
                flex-direction: column;
                align-items: stretch;
            }
            .selection-actions {
                justify-items: start;
                min-width: 0;
            }
        }

        @media (max-width: 960px) {
            .topbar {
                grid-template-columns: 1fr;
                grid-template-areas:
                    "brand"
                    "actions"
                    "routes";
            }
            .headline-strip { grid-template-columns: 1fr; }
            .stats-row, .hero-kpis { grid-template-columns: repeat(2, minmax(0,1fr)); }
            .kpi-grid, .fact-grid { grid-template-columns: 1fr; }
            :root {
                --map-height: 60vh;
                --map-min-height: 500px;
            }
            .map-preset-grid { grid-template-columns: repeat(2, minmax(0, 1fr)); }
        }

        @media (max-width: 860px) {
            .route-pills {
                padding: 6px;
                gap: 8px;
            }
            .top-actions {
                padding: 10px;
            }
            .hero-kpis {
                grid-template-columns: 1fr;
            }
        }

        @media (max-width: 760px) {
            :root {
                --shell-padding: max(10px, env(safe-area-inset-left));
                --shell-padding-right: max(10px, env(safe-area-inset-right));
                --shell-padding-top: max(10px, env(safe-area-inset-top));
                --shell-padding-bottom: max(12px, env(safe-area-inset-bottom));
                --map-height: 54vh;
                --map-min-height: 420px;
            }
            .map-preset-grid { grid-template-columns: 1fr 1fr; }
            .map-stage { padding: 10px; }
            .map-stage-quickbar {
                margin-bottom: 10px;
                padding: 10px;
            }
            .map-stage-quick-presets {
                grid-auto-columns: minmax(124px, max-content);
                gap: 8px;
            }
            .route-pill {
                min-width: 128px;
                min-height: 56px;
            }
            .selection-title {
                font-size: 22px;
            }
        }

        @media (max-width: 640px) {
            .shell { min-height: calc(100vh - var(--shell-padding-top) - var(--shell-padding-bottom)); }
            .topbar,
            .sidebar,
            .map-stage,
            .headline-strip,
            .panel,
            .floating-card,
            .legal-strip {
                border-radius: 20px;
            }
            .map-preset-grid,
            .stats-row,
            .hero-kpis {
                grid-template-columns: 1fr;
            }
            .route-pill {
                min-width: 120px;
            }
        }

        @media (pointer: coarse) {
            .route-pill,
            .tab,
            .chip,
            .toggle,
            .selection-clear,
            .top-actions .select,
            .map-stage-quickbar .toggle.stage-toggle {
                min-height: 44px;
            }

            .route-pill {
                min-width: 148px;
            }
        }
    </style>
</head>
<body>
    <div class="shell">
        <header class="topbar">
            <div class="brand">
                <div class="eyebrow" id="brand-eyebrow"></div>
                <h1 id="brand-title"></h1>
                <div class="helper" id="brand-subtitle"></div>
            </div>
            <div class="route-pills" id="route-pills"></div>
            <div class="top-actions">
                <div class="toolbar-block">
                    <div class="control-label" id="language-label"></div>
                    <div class="tabs" id="language-tabs"></div>
                </div>
                <label class="toolbar-block toolbar-select-block">
                    <span class="control-label" id="basemap-label"></span>
                    <select class="select" id="basemap-select"></select>
                </label>
            </div>
        </header>

        <div class="workspace">
            <section class="map-stage">
                <section class="map-stage-quickbar" aria-label="Stage filters">
                    <div class="toggle-group-label" id="stage-quick-label"></div>
                    <div class="map-stage-quick-presets" id="stage-quick-presets"></div>
                </section>
                <section class="tablet-control-surface" aria-label="iPad control surface">
                    <article class="tablet-surface-summary">
                        <div class="eyebrow" id="tablet-surface-eyebrow"></div>
                        <div class="tablet-surface-heading">
                            <div>
                                <h3 id="tablet-summary-title"></h3>
                                <div class="helper" id="tablet-summary-subtitle"></div>
                            </div>
                            <span class="pill" id="tablet-generated-pill"></span>
                        </div>
                        <div class="tablet-summary-band">
                            <strong id="tablet-summary-km"></strong>
                            <div class="row tiny">
                                <span id="tablet-summary-state"></span>
                                <span id="tablet-summary-percent"></span>
                            </div>
                            <div class="progress-track"><div class="progress-fill" id="tablet-summary-progress"></div></div>
                        </div>
                        <div class="tablet-kpi-ribbon" id="tablet-summary-kpis"></div>
                    </article>

                    <div class="tablet-control-cluster">
                        <article class="tablet-control-card">
                            <div class="toggle-group-label" id="tablet-region-presets-label"></div>
                            <div class="filter-row" id="tablet-region-presets"></div>
                        </article>
                        <article class="tablet-control-card">
                            <div class="toggle-group-label" id="tablet-source-presets-label"></div>
                            <div class="filter-row" id="tablet-source-presets"></div>
                        </article>
                        <article class="tablet-control-card">
                            <div class="toggle-group-label" id="tablet-status-filters-label"></div>
                            <div class="filter-row" id="tablet-status-filters"></div>
                        </article>
                    </div>

                    <div class="tablet-surface-lower">
                        <article class="tablet-playback-card">
                            <div class="card-head" style="align-items:center">
                                <div class="toggle-group-label" id="tablet-playback-label"></div>
                                <button type="button" class="toggle" id="tablet-playback-button"></button>
                            </div>
                            <div class="playback-meta tiny"><span id="tablet-playback-year-label"></span><span id="tablet-playback-range-label"></span></div>
                            <input type="range" min="1970" max="2026" value="2026" class="playback-range" id="tablet-playback-range" />
                            <div class="playback-km-wrap"><div class="playback-km-fill" id="tablet-playback-km-fill"></div><span class="playback-km-label" id="tablet-playback-km-label"></span></div>
                        </article>

                        <div class="tablet-surface-tools">
                            <article class="tablet-control-card">
                                <div class="toggle-group-label" id="tablet-map-presets-label"></div>
                                <div class="tablet-map-preset-grid" id="tablet-map-presets"></div>
                            </article>
                            <article class="tablet-control-card">
                                <div class="toggle-group-label" id="tablet-legend-label"></div>
                                <div class="legend" id="tablet-legend"></div>
                            </article>
                        </div>
                    </div>
                </section>
                <section class="desktop-map-dock" aria-label="Desktop map controls">
                    <div class="desktop-map-dock-main" id="desktop-map-presets"></div>
                    <div class="legend" id="desktop-legend"></div>
                </section>
                <div id="map"></div>
                <div class="year-overlay" id="year-overlay"><span id="year-overlay-year"></span><span class="year-km-sub" id="year-overlay-km"></span></div>

                <section class="floating-card floating-top-left">
                    <div class="hero-topline">
                        <div>
                            <div class="eyebrow" id="hero-eyebrow"></div>
                            <h2 style="margin:6px 0 8px" id="hero-title"></h2>
                            <div class="helper" id="hero-subtitle"></div>
                        </div>
                        <button type="button" class="hero-focus" id="hero-focus"></button>
                    </div>
                    <div class="hero-kpis" id="hero-stats"></div>
                </section>

                <section class="floating-card floating-top-right" id="selection-panel">
                    <div class="eyebrow" id="selection-eyebrow"></div>
                    <div class="selection-header" style="margin:6px 0 10px">
                        <div>
                            <h3 class="selection-title" id="selection-title"></h3>
                            <div class="helper" id="selection-subtitle"></div>
                        </div>
                        <div class="selection-actions">
                            <button type="button" class="selection-clear" id="selection-clear"></button>
                            <span class="status-pill" id="selection-status"></span>
                        </div>
                    </div>
                    <div class="detail-pills" id="selection-pills"></div>
                    <div class="fact-grid" id="selection-facts" style="margin-top:12px"></div>
                    <div class="mini-note selection-note" id="selection-note" style="margin-top:12px"></div>
                </section>

                <section class="floating-card floating-bottom-right">
                    <div class="card-head" style="margin-bottom:10px">
                        <div>
                            <div class="eyebrow" id="timeline-eyebrow"></div>
                            <h3 id="timeline-title" style="margin-top:6px"></h3>
                        </div>
                        <div class="legend" id="legend"></div>
                    </div>
                    <div class="timeline" id="timeline"></div>
                </section>

                <section class="floating-card floating-bottom-left map-tools">
                    <div class="eyebrow" id="map-presets-eyebrow"></div>
                    <div class="map-preset-grid" id="map-presets"></div>
                    <div class="tiny" id="map-note"></div>
                </section>
            </section>

            <section class="headline-strip">
                <article class="headline-card">
                    <div class="eyebrow" id="headline-primary-eyebrow"></div>
                    <strong id="headline-primary-title"></strong>
                    <div class="tiny" id="headline-primary-copy" style="margin-top:6px"></div>
                </article>
                <article class="headline-card">
                    <div class="eyebrow" id="headline-secondary-eyebrow"></div>
                    <strong id="headline-secondary-title"></strong>
                    <div class="tiny" id="headline-secondary-copy" style="margin-top:6px"></div>
                </article>
                <article class="headline-card">
                    <div class="eyebrow" id="headline-tertiary-eyebrow"></div>
                    <strong id="headline-tertiary-title"></strong>
                    <div class="tiny" id="headline-tertiary-copy" style="margin-top:6px"></div>
                </article>
            </section>

            <aside class="sidebar">
                <section class="tablet-info-tabs" aria-label="Tablet infographic tabs">
                    <div class="eyebrow" id="tablet-tabs-eyebrow"></div>
                    <div class="tablet-tab-row">
                        <button type="button" class="tab tablet-info-tab" id="tablet-tab-facts" data-tablet-tab="facts"></button>
                        <button type="button" class="tab tablet-info-tab" id="tablet-tab-notes" data-tablet-tab="notes"></button>
                        <button type="button" class="tab tablet-info-tab" id="tablet-tab-routes" data-tablet-tab="routes"></button>
                        <button type="button" class="tab tablet-info-tab" id="tablet-tab-lots" data-tablet-tab="lots"></button>
                    </div>
                </section>

                <section class="summary-card" id="summary-panel">
                    <div class="eyebrow" id="summary-eyebrow"></div>
                    <div class="summary-main">
                        <div class="card-head">
                            <div>
                                <h2 id="summary-title"></h2>
                                <div class="helper" id="summary-subtitle"></div>
                            </div>
                            <span class="pill" id="generated-pill"></span>
                        </div>
                        <strong id="summary-km"></strong>
                        <div class="row tiny">
                            <span id="summary-state"></span>
                            <span id="summary-percent"></span>
                        </div>
                        <div class="progress-track"><div class="progress-fill" id="summary-progress"></div></div>
                    </div>
                </section>

                <section class="panel" id="filters-panel">
                    <div class="eyebrow" id="filters-eyebrow"></div>
                    <h3 id="filters-title" style="margin:6px 0 12px"></h3>
                    <div class="filter-stack">
                        <div class="tiny filter-copy" id="filters-copy"></div>
                        <div class="toggle-group">
                            <div class="toggle-group-label" id="stage-presets-label"></div>
                            <div class="filter-row" id="stage-presets"></div>
                        </div>
                        <div class="toggle-group">
                            <div class="toggle-group-label" id="region-presets-label"></div>
                            <div class="filter-row" id="region-presets"></div>
                        </div>
                        <div class="toggle-group">
                            <div class="toggle-group-label" id="source-presets-label"></div>
                            <div class="filter-row" id="source-presets"></div>
                        </div>
                        <div class="toggle-group">
                            <div class="toggle-group-label" id="status-filters-label"></div>
                            <div class="filter-row" id="status-filters"></div>
                        </div>
                        <div class="toggle-group">
                            <div class="card-head" style="align-items:center">
                                <div class="toggle-group-label" id="playback-label"></div>
                                <button type="button" class="toggle" id="playback-button"></button>
                            </div>
                            <div class="playback-meta tiny"><span id="playback-year-label"></span><span id="playback-range-label"></span></div>
                            <input type="range" min="1970" max="2026" value="2026" class="playback-range" id="playback-range" />
                            <div class="playback-km-wrap"><div class="playback-km-fill" id="playback-km-fill"></div><span class="playback-km-label" id="playback-km-label"></span></div>
                        </div>
                    </div>
                </section>

                <section class="panel" id="network-panel" data-tablet-panel="facts">
                    <div class="eyebrow" id="network-eyebrow"></div>
                    <h3 id="network-title" style="margin:6px 0 12px"></h3>
                    <div class="kpi-grid" id="sidebar-kpis"></div>
                </section>

                <section class="panel" id="notes-panel" data-tablet-panel="notes">
                    <div class="eyebrow" id="notes-eyebrow"></div>
                    <h3 id="notes-title" style="margin:6px 0 12px"></h3>
                    <div class="notes-body" id="notes-body"></div>
                </section>

                <div class="sidebar-scroll">
                    <section class="panel" id="routes-panel" data-tablet-panel="routes">
                        <div class="card-head" style="margin-bottom:12px">
                            <div>
                                <div class="eyebrow" id="routes-eyebrow"></div>
                                <h3 id="routes-title" style="margin-top:6px"></h3>
                            </div>
                            <span class="pill" id="routes-count"></span>
                        </div>
                        <div class="route-list" id="route-list"></div>
                    </section>

                    <section class="panel" id="lots-panel" data-tablet-panel="lots">
                        <div class="card-head" style="margin-bottom:12px">
                            <div>
                                <div class="eyebrow" id="lots-eyebrow"></div>
                                <h3 id="lots-title" style="margin-top:6px"></h3>
                            </div>
                            <span class="pill" id="lots-count"></span>
                        </div>
                        <div class="lot-list" id="lot-list"></div>
                    </section>
                </div>
            </aside>
        </div>

        <footer class="legal-strip">
            <div class="eyebrow" id="legal-eyebrow"></div>
            <strong id="legal-title"></strong>
            <div class="tiny" id="legal-copy" style="margin-top:6px"></div>
        </footer>
    </div>

    <script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js" integrity="sha256-20nQCchB9co0qIjJZRGuk2/Z9VM+kNiyxNV1lvTlZBo=" crossorigin=""></script>
    <script>
        const atlas = {{atlasJson}};
        const statusMeta = {
            Open: { color: '#2ee6c0', bg: 'open', en: 'open' },
            UnderConstruction: { color: '#ffb357', bg: 'construction', en: 'construction' },
            Planned: { color: '#8f74ff', bg: 'planned', en: 'planned' },
            Closed: { color: '#ff648a', bg: 'closed', en: 'closed' }
        };

        const text = {
            bg: {
                brandEyebrow: 'Премиум инфографски атлас',
                summaryEyebrow: 'Национален фокус',
                controlSurfaceEyebrow: 'iPad контролна повърхност',
                infographicTabsEyebrow: 'Инфографски изгледи',
                factsTab: 'Факти',
                notesTab: 'Бележки',
                routesTab: 'Маршрути',
                lotsTab: 'Лотове',
                statusLegend: 'Статуси',
                headlineNow: 'Текущ избор',
                headlineAudit: 'Колко сигурни са данните',
                headlineScope: 'Какво виждате',
                filtersEyebrow: 'Контроли',
                filtersTitle: 'Какво да се вижда',
                filtersCopy: 'Първо изберете готовите, строящите се или бъдещите пътища. После, ако искате, стеснете изгледа по регион или по източник.',
                stagePresets: 'Режими по етап',
                regionPresets: 'Регионален фокус',
                sourcePresets: 'Източниково покритие',
                granularStatus: 'Детайлни статуси',
                playbackLabel: 'История на строителството',
                playbackPlay: 'Пусни',
                playbackPause: 'Пауза',
                playbackYear: 'Година',
                playbackRange: 'От първите пускания до днес',
                networkEyebrow: 'Накратко',
                networkTitle: 'Основни числа',
                notesEyebrow: 'Обяснение',
                notesTitle: 'Бележки на разбираем език',
                routesEyebrow: 'Маршрути',
                routesTitle: 'Коридори в изгледа',
                lotsEyebrow: 'Лотове',
                lotsTitle: 'Видими лотове',
                heroEyebrow: 'Карта на България',
                heroTitle: 'Българските автомагистрали, обяснени просто',
                heroSubtitle: '9 основни коридора · от София до Черно море · 1 506 km в текущия модел',
                heroFocus: 'Режим на команден център',
                heroBoard: 'Инфографски обзор',
                heroRoutes: 'Водещи коридори',
                heroSignalLength: 'Видима дължина',
                heroSignalReadiness: 'Отворени сега',
                heroSignalAudit: 'Дял с официален източник',
                allStages: 'Всички етапи',
                allRegions: 'Цяла България',
                westRegion: 'Запад',
                eastRegion: 'Изток',
                builtStage: 'Изградени',
                buildingStage: 'В строеж',
                plannedStage: 'Предстоят',
                builtHint: 'отворени сега',
                buildingHint: 'активни строежи',
                plannedHint: 'планирани отсечки',
                allHint: 'пълна мрежа',
                westHint: 'София и западни коридори',
                eastHint: 'черноморски и източни коридори',
                commandCenterOn: 'Команден режим: ВКЛ',
                commandCenterOff: 'Команден режим: ИЗКЛ',
                commandCenterShow: 'Покажи панелите',
                commandCenterHide: 'Скрий панелите',
                selectionEyebrow: 'Активен избор',
                timelineEyebrow: 'Времева линия',
                timelineTitle: 'Ключови събития',
                allRoutes: 'Всички',
                open: 'Отворени',
                construction: 'В строеж',
                planned: 'Планирани',
                closed: 'Затворени',
                totalLength: 'Обща дължина',
                completion: 'Готовност',
                visibleLots: 'Видими лотове',
                focusMode: 'Фокус',
                nationalOverview: 'Национален обзор',
                routeOverview: 'Маршрутен обзор',
                lotOverview: 'Лот',
                segmentOverview: 'Участък',
                generated: 'Генерирано',
                openToday: 'Отворени сега',
                inBuild: 'В строеж',
                plannedNow: 'Планирани',
                avgSpeed: 'Проектна скорост',
                openYear: 'Открита',
                travelTime: 'Време за преминаване',
                targetYear: 'Целева година',
                budget: 'Бюджет',
                contractor: 'Изпълнител',
                length: 'Дължина',
                status: 'Статус',
                sectionCode: 'Код',
                noSelection: 'Изберете маршрут, участък или лот от картата, за да видите кратко обяснение и основните данни.',
                noLots: 'Няма видими лотове за текущия избор.',
                noRoutes: 'Няма видими маршрути за текущия избор.',
                pending: 'Нужна е проверка',
                lotCount: 'лота',
                modeledSpan: 'оценен участък',
                routeCount: 'маршрута',
                years: 'години',
                allSources: 'Всички източници',
                officialLots: 'Участъци с официален източник',
                mixedSources: 'Частично проверени участъци',
                modeledSources: 'Оценени участъци',
                allSourceHint: 'целият слой',
                officialSourceHint: 'само с официален източник',
                mixedSourceHint: 'частично проверени',
                modeledSourceHint: 'без пълна официална разбивка',
                sourceQuality: 'Сигурност на източника',
                evidenceGrade: 'Сила на източника',
                importance: 'Стратегическо значение',
                routeStory: 'Маршрутно значение',
                basedOn: 'Базирано на',
                sourceLink: 'Публикуван източник',
                officialRecord: 'Официален запис',
                supportingReference: 'Подкрепящ източник',
                sourceVerifiedOn: 'Проверено',
                evidenceRoute: 'официален и за точно този маршрут',
                evidenceNetwork: 'официален, но за мрежата като цяло',
                evidenceHybrid: 'официален плюс допълващ източник',
                evidenceSecondary: 'само допълващ източник',
                evidenceMissing: 'източникът още липсва',
                latestUpdate: 'Последно обновяване',
                currentScope: 'Текущ фокус',
                selectedContext: 'Какво е избрано',
                bulgariaScopeNote: 'Атласът в момента обхваща 9 маршрута (А1–А6, E79, RVT, MB) – общо 1 506 km, от които 991 km са отворени.',
                auditSeeded: 'проверен лот',
                auditDerived: 'оценен участък',
                auditedLots: 'лота са с ръчно въведени дължини',
                derivedLots: 'лотове с производни дължини',
                popupOpenMap: 'Детайл на картата',
                popupPinnedHint: 'Можете да затворите този прозорец и изборът ще остане в панела вдясно.',
                segmentLots: 'Лотове в участъка',
                officialFeed: 'Официални известия',
                statusWindow: 'Статус прозорец',
                latestBulletin: 'Официален бюлетин',
                officialSource: 'Източник АПИ',
                publishedSource: 'Публикуван източник',
                atlasBuild: 'български атлас',
                languageLabel: 'Език',
                mapStyle: 'Карта',
                clearSelection: 'Изчисти избора',
                resetView: 'Нулирай изгледа',
                focusHintTitle: 'Изберете от картата',
                focusHintCopy: 'Докоснете маршрут, участък или лот за детайлен панел, без да закривате картата.',
                auditAccuracy: 'Колко надеждни са данните',
                auditVerifiedShare: 'Дял с официален източник',
                auditModeledNotice: 'Линиите на пътищата са изградени от текущия seed модел и публични бюлетини. За геодезична точност по лотове все още е нужен официален GIS или API източник.',
                mapPresetsEyebrow: 'Картни стилове',
                mapNote: 'Тъмните карти помагат маршрутите да изпъкнат по-ясно. Сателитният изглед е полезен за терен и крайбрежие.',
                sectionCount: 'участъка',
                timelineOpened: 'Отворен',
                timelineDelayed: 'Забавяне',
                timelineIssue: 'Проблем',
                timelineUpdate: 'Актуализация',
                timelineCancelled: 'Отказан',
                timelineBaseline: 'База',
                timelinePlanning: 'Планиране',
                timelineForecast: 'Хоризонт',
                yearOpenedSuffix: 'отворени',
                yearKmOpen: 'km отворена мрежа',
                copyrightLabel: 'Права и ползване',
                ownershipNotice: '© 2026 ViLLZz. Атласът, визуалният дизайн и подбраната структура са авторски; използване и споделяне с атрибуция към проекта.'
            },
            en: {
                brandEyebrow: 'Premium infographic atlas',
                summaryEyebrow: 'National focus',
                controlSurfaceEyebrow: 'iPad control surface',
                infographicTabsEyebrow: 'Infographic views',
                factsTab: 'Facts',
                notesTab: 'Notes',
                routesTab: 'Routes',
                lotsTab: 'Lots',
                statusLegend: 'Statuses',
                headlineNow: 'Current focus',
                headlineAudit: 'How solid the data is',
                headlineScope: 'What you are looking at',
                filtersEyebrow: 'Controls',
                filtersTitle: 'What to show',
                filtersCopy: 'Choose built, in-progress, or future roads first. Then narrow the view by region or source if you want more detail.',
                stagePresets: 'Stage modes',
                regionPresets: 'Regional focus',
                sourcePresets: 'Source coverage',
                granularStatus: 'Detailed statuses',
                playbackLabel: 'Build history playback',
                playbackPlay: 'Play',
                playbackPause: 'Pause',
                playbackYear: 'Year',
                playbackRange: 'From first openings to today',
                networkEyebrow: 'At a glance',
                networkTitle: 'Main numbers',
                notesEyebrow: 'Plain-language notes',
                notesTitle: 'Notes in plain language',
                routesEyebrow: 'Routes',
                routesTitle: 'Visible corridors',
                lotsEyebrow: 'Lots',
                lotsTitle: 'Visible lots',
                heroEyebrow: 'Map of Bulgaria',
                heroTitle: 'Bulgaria\'s motorways, explained clearly',
                heroSubtitle: '9 main corridors · from Sofia to the Black Sea · 1,506 km in the current model',
                heroFocus: 'Command center mode',
                heroBoard: 'Infographic overview',
                heroRoutes: 'Leading corridors',
                heroSignalLength: 'Visible length',
                heroSignalReadiness: 'Open now',
                heroSignalAudit: 'Share with official source',
                allStages: 'All stages',
                allRegions: 'Whole country',
                westRegion: 'West',
                eastRegion: 'East',
                builtStage: 'Open now',
                buildingStage: 'In progress',
                plannedStage: 'Still to build',
                builtHint: 'already open',
                buildingHint: 'active construction',
                plannedHint: 'future sections',
                allHint: 'full network',
                westHint: 'Sofia and western corridors',
                eastHint: 'Black Sea and eastern corridors',
                commandCenterOn: 'Command mode: ON',
                commandCenterOff: 'Command mode: OFF',
                commandCenterShow: 'Show panels',
                commandCenterHide: 'Hide panels',
                selectionEyebrow: 'Active selection',
                timelineEyebrow: 'Timeline',
                timelineTitle: 'Key events',
                allRoutes: 'All',
                open: 'Open',
                construction: 'Construction',
                planned: 'Planned',
                closed: 'Closed',
                totalLength: 'Total length',
                completion: 'Completion',
                visibleLots: 'Visible lots',
                focusMode: 'Focus',
                nationalOverview: 'National overview',
                routeOverview: 'Route overview',
                lotOverview: 'Lot',
                segmentOverview: 'Section',
                generated: 'Generated',
                openToday: 'Open now',
                inBuild: 'Under construction',
                plannedNow: 'Planned',
                avgSpeed: 'Design speed',
                openYear: 'Opened',
                travelTime: 'Drive time',
                targetYear: 'Target year',
                budget: 'Budget',
                contractor: 'Contractor',
                length: 'Length',
                status: 'Status',
                sectionCode: 'Code',
                noSelection: 'Pick a route, section, or lot on the map to see a short explanation and the key facts.',
                noLots: 'No lots are visible for the current selection.',
                noRoutes: 'No routes are visible for the current selection.',
                pending: 'Needs checking',
                lotCount: 'lots',
                modeledSpan: 'estimated section',
                routeCount: 'routes',
                years: 'years',
                allSources: 'All sources',
                officialLots: 'Sections with official source',
                mixedSources: 'Partly verified sections',
                modeledSources: 'Estimated sections',
                allSourceHint: 'entire layer',
                officialSourceHint: 'official-source sections only',
                mixedSourceHint: 'partly verified',
                modeledSourceHint: 'no full official breakdown yet',
                sourceQuality: 'Source confidence',
                evidenceGrade: 'Source strength',
                importance: 'Strategic importance',
                routeStory: 'Route significance',
                basedOn: 'Based on',
                sourceLink: 'Published source',
                officialRecord: 'Official record',
                supportingReference: 'Supporting reference',
                sourceVerifiedOn: 'Verified on',
                evidenceRoute: 'official and route-specific',
                evidenceNetwork: 'official at network level',
                evidenceHybrid: 'official plus supporting source',
                evidenceSecondary: 'supporting source only',
                evidenceMissing: 'source still missing',
                latestUpdate: 'Last refresh',
                currentScope: 'Current scope',
                selectedContext: 'What is selected',
                bulgariaScopeNote: 'The atlas currently covers 9 routes (A1–A6, E79, RVT, MB) with 1,506 km in total, of which 991 km are open.',
                auditSeeded: 'verified lot',
                auditDerived: 'estimated section',
                auditedLots: 'lots use hand-entered lengths',
                derivedLots: 'lots with derived lengths',
                popupOpenMap: 'Map detail',
                popupPinnedHint: 'You can close this card and keep the selection in the right-hand panel.',
                segmentLots: 'Lots in section',
                officialFeed: 'Official updates',
                statusWindow: 'Status window',
                latestBulletin: 'Official bulletin',
                officialSource: 'API source',
                publishedSource: 'Published source',
                atlasBuild: 'Bulgarian atlas build',
                languageLabel: 'Language',
                mapStyle: 'Map',
                clearSelection: 'Clear selection',
                resetView: 'Reset view',
                focusHintTitle: 'Select from map',
                focusHintCopy: 'Tap a route, section, or lot to open detailed context without covering the map.',
                auditAccuracy: 'How reliable the data is',
                auditVerifiedShare: 'Share with official source',
                auditModeledNotice: 'Road lines are drawn from the current seed model and public bulletins. For survey-grade precision, the project would still need official lot-level GIS or API data.',
                mapPresetsEyebrow: 'Map modes',
                mapNote: 'Dark map styles help the motorway overlays stand out. Satellite is useful for terrain and coastline context.',
                sectionCount: 'sections',
                timelineOpened: 'Opened',
                timelineDelayed: 'Delayed',
                timelineIssue: 'Issue',
                timelineUpdate: 'Update',
                timelineCancelled: 'Cancelled',
                timelineBaseline: 'Baseline',
                timelinePlanning: 'Planning',
                timelineForecast: 'Forecast',
                yearOpenedSuffix: 'opened',
                yearKmOpen: 'km open',
                copyrightLabel: 'Rights and usage',
                ownershipNotice: '© 2026 ViLLZz. The atlas, visual design, and curated structure remain copyrighted; reuse and sharing require attribution to the project.'
            }
        };

        const tileDefaults = { detectRetina: true, updateWhenIdle: true, updateWhenZooming: false, keepBuffer: 6, crossOrigin: true };
        const ALL_STATUSES = ['Open', 'UnderConstruction', 'Planned', 'Closed'];
        const basemaps = {
            monitor: { name: { bg: 'Monitor Dark', en: 'Monitor Dark' }, tone: { bg: 'Нисък отблясък', en: 'Low-glare dark' }, featured: true, url: 'https://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}{r}.png', options: { ...tileDefaults, attribution: '&copy; OpenStreetMap contributors &copy; CARTO', subdomains: 'abcd', maxZoom: 20 } },
            graphite: { name: { bg: 'Graphite', en: 'Graphite' }, tone: { bg: 'Минимален контраст', en: 'Minimal contrast' }, featured: true, url: 'https://server.arcgisonline.com/ArcGIS/rest/services/Canvas/World_Dark_Gray_Base/MapServer/tile/{z}/{y}/{x}', options: { ...tileDefaults, attribution: 'Tiles &copy; Esri', maxZoom: 16 } },
            voyager: { name: { bg: 'Voyager', en: 'Voyager' }, tone: { bg: 'Инфографска светлина', en: 'Editorial light' }, featured: true, url: 'https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png', options: { ...tileDefaults, attribution: '&copy; OpenStreetMap contributors &copy; CARTO', subdomains: 'abcd', maxZoom: 20 } },
            light: { name: { bg: 'Positron', en: 'Positron' }, tone: { bg: 'Чист светъл фон', en: 'Clean light base' }, featured: true, url: 'https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}{r}.png', options: { ...tileDefaults, attribution: '&copy; OpenStreetMap contributors &copy; CARTO', subdomains: 'abcd', maxZoom: 20 } },
            street: { name: { bg: 'Street', en: 'Street' }, tone: { bg: 'Детайлен градски слой', en: 'Detailed street layer' }, featured: false, url: 'https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', options: { ...tileDefaults, attribution: '&copy; OpenStreetMap contributors', maxZoom: 19 } },
            osmhot: { name: { bg: 'HOT', en: 'HOT' }, tone: { bg: 'Плътен пътен контекст', en: 'Dense road context' }, featured: false, url: 'https://{s}.tile.openstreetmap.fr/hot/{z}/{x}/{y}.png', options: { ...tileDefaults, attribution: '&copy; OpenStreetMap contributors, Tiles style by Humanitarian OpenStreetMap Team', maxZoom: 20 } },
            terrain: { name: { bg: 'Релеф', en: 'Terrain' }, tone: { bg: 'Топография', en: 'Topography' }, featured: false, url: 'https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png', options: { ...tileDefaults, attribution: '&copy; OpenStreetMap contributors, SRTM | OpenTopoMap', maxZoom: 17 } },
            gray: { name: { bg: 'Gray Canvas', en: 'Gray Canvas' }, tone: { bg: 'Неутрален фон', en: 'Neutral canvas' }, featured: false, url: 'https://server.arcgisonline.com/ArcGIS/rest/services/Canvas/World_Light_Gray_Base/MapServer/tile/{z}/{y}/{x}', options: { ...tileDefaults, attribution: 'Tiles &copy; Esri', maxZoom: 16 } },
            satellite: { name: { bg: 'Сателит', en: 'Satellite' }, tone: { bg: 'Терен и крайбрежие', en: 'Terrain and coast' }, featured: true, url: 'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}', options: { ...tileDefaults, attribution: 'Tiles &copy; Esri', maxZoom: 20 } }
        };

        const bulgariaBounds = [[atlas.geo.bounds.south, atlas.geo.bounds.west], [atlas.geo.bounds.north, atlas.geo.bounds.east]];

        const state = {
            lang: 'bg',
            basemap: 'monitor',
            activeRoute: 'ALL',
            regionFilter: 'ALL',
            sourceScope: 'ALL',
            activeStatuses: new Set(ALL_STATUSES),
            selectedSegmentId: null,
            selectedLotKey: null,
            commandCenterMode: false,
            tabletInfoTab: 'facts',
            playbackYear: new Date(atlas.generatedAtUtc).getFullYear(),
            playbackPlaying: false
        };

        const $ = id => document.getElementById(id);
        const el = {
            brandEyebrow: $('brand-eyebrow'), brandTitle: $('brand-title'), brandSubtitle: $('brand-subtitle'),
            headlinePrimaryEyebrow: $('headline-primary-eyebrow'), headlinePrimaryTitle: $('headline-primary-title'), headlinePrimaryCopy: $('headline-primary-copy'),
            headlineSecondaryEyebrow: $('headline-secondary-eyebrow'), headlineSecondaryTitle: $('headline-secondary-title'), headlineSecondaryCopy: $('headline-secondary-copy'),
            headlineTertiaryEyebrow: $('headline-tertiary-eyebrow'), headlineTertiaryTitle: $('headline-tertiary-title'), headlineTertiaryCopy: $('headline-tertiary-copy'),
            routePills: $('route-pills'), languageLabel: $('language-label'), languageTabs: $('language-tabs'), basemapLabel: $('basemap-label'), basemapSelect: $('basemap-select'), mapPresetsEyebrow: $('map-presets-eyebrow'), mapPresets: $('map-presets'), desktopMapPresets: $('desktop-map-presets'), mapNote: $('map-note'),
            summaryEyebrow: $('summary-eyebrow'), summaryTitle: $('summary-title'), summarySubtitle: $('summary-subtitle'), generatedPill: $('generated-pill'), summaryKm: $('summary-km'), summaryState: $('summary-state'), summaryPercent: $('summary-percent'), summaryProgress: $('summary-progress'),
            tabletSurfaceEyebrow: $('tablet-surface-eyebrow'), tabletSummaryTitle: $('tablet-summary-title'), tabletSummarySubtitle: $('tablet-summary-subtitle'), tabletGeneratedPill: $('tablet-generated-pill'), tabletSummaryKm: $('tablet-summary-km'), tabletSummaryState: $('tablet-summary-state'), tabletSummaryPercent: $('tablet-summary-percent'), tabletSummaryProgress: $('tablet-summary-progress'), tabletSummaryKpis: $('tablet-summary-kpis'),
            filtersEyebrow: $('filters-eyebrow'), filtersTitle: $('filters-title'), filtersCopy: $('filters-copy'), stagePresetsLabel: $('stage-presets-label'), stagePresets: $('stage-presets'), stageQuickLabel: $('stage-quick-label'), stageQuickPresets: $('stage-quick-presets'), regionPresetsLabel: $('region-presets-label'), regionPresets: $('region-presets'), tabletRegionPresetsLabel: $('tablet-region-presets-label'), tabletRegionPresets: $('tablet-region-presets'), sourcePresetsLabel: $('source-presets-label'), sourcePresets: $('source-presets'), tabletSourcePresetsLabel: $('tablet-source-presets-label'), tabletSourcePresets: $('tablet-source-presets'), statusFiltersLabel: $('status-filters-label'), statusFilters: $('status-filters'), tabletStatusFiltersLabel: $('tablet-status-filters-label'), tabletStatusFilters: $('tablet-status-filters'), playbackLabel: $('playback-label'), playbackButton: $('playback-button'), playbackYearLabel: $('playback-year-label'), playbackRangeLabel: $('playback-range-label'), playbackRange: $('playback-range'), tabletPlaybackLabel: $('tablet-playback-label'), tabletPlaybackButton: $('tablet-playback-button'), tabletPlaybackYearLabel: $('tablet-playback-year-label'), tabletPlaybackRangeLabel: $('tablet-playback-range-label'), tabletPlaybackRange: $('tablet-playback-range'), yearOverlay: $('year-overlay'), yearOverlayYear: $('year-overlay-year'), yearOverlayKm: $('year-overlay-km'), playbackKmFill: $('playback-km-fill'), playbackKmLabel: $('playback-km-label'), tabletPlaybackKmFill: $('tablet-playback-km-fill'), tabletPlaybackKmLabel: $('tablet-playback-km-label'),
            networkEyebrow: $('network-eyebrow'), networkTitle: $('network-title'), sidebarKpis: $('sidebar-kpis'),
            notesEyebrow: $('notes-eyebrow'), notesTitle: $('notes-title'), notesBody: $('notes-body'),
            routesEyebrow: $('routes-eyebrow'), routesTitle: $('routes-title'), routesCount: $('routes-count'), routeList: $('route-list'),
            lotsEyebrow: $('lots-eyebrow'), lotsTitle: $('lots-title'), lotsCount: $('lots-count'), lotList: $('lot-list'),
            tabletTabsEyebrow: $('tablet-tabs-eyebrow'), tabletTabFacts: $('tablet-tab-facts'), tabletTabNotes: $('tablet-tab-notes'), tabletTabRoutes: $('tablet-tab-routes'), tabletTabLots: $('tablet-tab-lots'),
            summaryPanel: $('summary-panel'), filtersPanel: $('filters-panel'), networkPanel: $('network-panel'), notesPanel: $('notes-panel'), routesPanel: $('routes-panel'), lotsPanel: $('lots-panel'),
            heroEyebrow: $('hero-eyebrow'), heroTitle: $('hero-title'), heroSubtitle: $('hero-subtitle'), heroFocus: $('hero-focus'), heroStats: $('hero-stats'), mapStage: document.querySelector('.map-stage'), selectionPanel: $('selection-panel'),
            selectionEyebrow: $('selection-eyebrow'), selectionTitle: $('selection-title'), selectionSubtitle: $('selection-subtitle'), selectionClear: $('selection-clear'), selectionStatus: $('selection-status'), selectionPills: $('selection-pills'), selectionFacts: $('selection-facts'), selectionNote: $('selection-note'),
            legalEyebrow: $('legal-eyebrow'), legalTitle: $('legal-title'), legalCopy: $('legal-copy'),
            timelineEyebrow: $('timeline-eyebrow'), timelineTitle: $('timeline-title'), timeline: $('timeline'), legend: $('legend'), desktopLegend: $('desktop-legend'), tabletLegend: $('tablet-legend'), tabletLegendLabel: $('tablet-legend-label'), tabletMapPresets: $('tablet-map-presets'), tabletMapPresetsLabel: $('tablet-map-presets-label')
        };

        const pathRenderer = L.canvas({ padding: 0.55, tolerance: 5 });
        const map = L.map('map', {
            zoomControl: true,
            preferCanvas: true,
            zoomSnap: 0.5,
            zoomDelta: 0.5,
            minZoom: 6.9,
            zoomAnimationThreshold: 4,
            wheelPxPerZoomLevel: 120,
            maxBoundsViscosity: 1.0,
            markerZoomAnimation: false,
            renderer: pathRenderer
        });

        map.setMaxBounds([[41.05, 22.05], [44.35, 28.95]]);

        let basemapLayer = null;
    const contextLayer = L.layerGroup().addTo(map);
        const segmentLayer = L.layerGroup().addTo(map);
        const lotLineLayer = L.layerGroup().addTo(map);
        const lotLayer = L.layerGroup().addTo(map);
        const statusLayer = L.layerGroup().addTo(map);
        const highlightLayer = L.layerGroup().addTo(map);
        const lotMarkers = new Map();
        let activePopup = null;
        let lastViewportKey = null;
        let suppressPopupReset = false;
        let playbackTimer = null;
        let resizeTimer = null;
        let renderFrame = null;
        let renderPendingReason = 'initial';

        const t = key => text[state.lang][key];
        const pick = value => !value ? '' : (state.lang === 'bg' ? value.bg : value.en);
        const formatKm = value => `${Math.round(Number(value) || 0)} km`;
        const formatPercent = value => `${Number(value || 0).toFixed(1)}%`;
        const formatLotTarget = lot => lot.forecastOpenYear || (lot.isDerived ? t('modeledSpan') : t('pending'));
        const formatBudget = value => value == null ? t('pending') : `${new Intl.NumberFormat(state.lang === 'bg' ? 'bg-BG' : 'en-US', { maximumFractionDigits: 1, minimumFractionDigits: 1 }).format(value)} M€`;
        const formatFunding = program => {
            if (!program) return '';
            const palette = [['CEF','#4aabff'],['ISPA','#5db8ff'],['Cohesion','#82c5ff'],['EIB','#a3b9ff'],['EBRD','#72d5f5'],['national','#7dc46a']];
            const match = palette.find(([k]) => program.toLowerCase().includes(k.toLowerCase()));
            const color = match ? match[1] : '#89a6cb';
            return `<span class="funding-badge" style="background:${color}18;border-color:${color}33;color:${color}">${program}</span>`;
        };
        const formatSource = (name, url) => {
            if (!name) return t('pending');
            if (!url) return name;
            return `<a href="${url}" target="_blank" rel="noreferrer noopener">${name}</a>`;
        };
        const formatSupportingSource = source => source?.name ? formatSource(source.name, source.url) : t('pending');
        const labelForStatus = status => t(statusMeta[status].bg);
        const labelForLotAudit = lot => lot.isDerived ? t('auditDerived') : t('auditSeeded');
        const labelForSourceQuality = sourceQuality => {
            if (sourceQuality === 'official') return t('officialLots');
            if (sourceQuality === 'mixed') return t('mixedSources');
            return t('modeledSources');
        };
        const labelForEvidenceGrade = evidenceGrade => {
            if (evidenceGrade === 'official-route' || evidenceGrade === 'official-route-plus-secondary') return t('evidenceRoute');
            if (evidenceGrade === 'official-network') return t('evidenceNetwork');
            if (evidenceGrade === 'official-network-plus-secondary') return t('evidenceHybrid');
            if (evidenceGrade === 'secondary-only') return t('evidenceSecondary');
            return t('evidenceMissing');
        };
        const shortRouteName = route => {
            const title = pick(route.title) || '';
            const quoted = title.match(/„([^“]+)“|"([^"]+)"/);
            if (quoted) return quoted[1] || quoted[2];
            return title
                .replace(/\s+(Motorway|Expressway|upgrade)$/i, '')
                .replace(/^Автомагистрала\s+/i, '')
                .replace(/^Скоростен път\s+/i, '')
                .trim();
        };
        const formatRouteCount = value => `${value} ${t('routeCount')}`;
        const formatSectionCount = value => `${value} ${t('sectionCount')}`;
        const getTimelineStateMeta = stateKey => {
            const stateClass = { success: 'success', warning: 'warning', danger: 'danger', info: 'info', delayed: 'warning', cancelled: 'danger', baseline: 'info', planning: 'info', forecast: 'info' };
            const stateIcon = { success: '✓', warning: '⏳', danger: '✗', info: 'ℹ', delayed: '⏳', cancelled: '✗', baseline: 'ℹ', planning: '📋', forecast: '→' };
            const stateLabel = {
                success: t('timelineOpened'),
                warning: t('timelineDelayed'),
                danger: t('timelineIssue'),
                info: t('timelineUpdate'),
                delayed: t('timelineDelayed'),
                cancelled: t('timelineCancelled'),
                baseline: t('timelineBaseline'),
                planning: t('timelinePlanning'),
                forecast: t('timelineForecast')
            };

            return {
                cls: stateClass[stateKey] || 'info',
                icon: stateIcon[stateKey] || '·',
                label: stateLabel[stateKey] || stateKey
            };
        };
        const renderTimelineItems = items => items.length
            ? items.map(item => {
                const meta = getTimelineStateMeta(item.state);
                return `<div class="timeline-item"><div class="timeline-year ${meta.cls}">${item.year}</div><div><div style="font-weight:500;font-size:13px">${pick(item.label)}</div><div class="timeline-state-badge ${meta.cls}">${meta.icon} ${meta.label}</div></div></div>`;
            }).join('')
            : `<div class="tiny">${t('pending')}</div>`;
        const STAGE_PRESETS = [
            { key: 'ALL', label: 'allStages', hint: 'allHint', statuses: ['Open', 'UnderConstruction', 'Planned', 'Closed'], accent: 'var(--accent-2)' },
            { key: 'BUILT', label: 'builtStage', hint: 'builtHint', statuses: ['Open'], accent: 'var(--open)' },
            { key: 'BUILDING', label: 'buildingStage', hint: 'buildingHint', statuses: ['UnderConstruction'], accent: 'var(--construction)' },
            { key: 'TO_BUILD', label: 'plannedStage', hint: 'plannedHint', statuses: ['Planned'], accent: 'var(--planned)' }
        ];
        const REGION_PRESETS = [
            { key: 'ALL', label: 'allRegions', hint: 'allHint' },
            { key: 'WEST', label: 'westRegion', hint: 'westHint' },
            { key: 'EAST', label: 'eastRegion', hint: 'eastHint' }
        ];
        const SOURCE_PRESETS = [
            { key: 'ALL', label: 'allSources', hint: 'allSourceHint' },
            { key: 'official', label: 'officialLots', hint: 'officialSourceHint' },
            { key: 'mixed', label: 'mixedSources', hint: 'mixedSourceHint' },
            { key: 'modeled', label: 'modeledSources', hint: 'modeledSourceHint' }
        ];

        const areSetsEqual = (left, right) => left.size === right.size && Array.from(left).every(item => right.has(item));

        function getViewportPadding(mode) {
            const width = window.innerWidth || 1280;
            if (mode === 'lot') {
                if (width <= 640) return [44, 20];
                if (width <= 960) return [62, 30];
                return [88, 88];
            }

            if (mode === 'segment') {
                if (width <= 640) return [26, 16];
                if (width <= 960) return [34, 22];
                return [40, 40];
            }

            if (mode === 'route') {
                if (width <= 640) return [22, 14];
                if (width <= 960) return [28, 18];
                return [34, 34];
            }

            if (width <= 640) return [12, 10];
            if (width <= 960) return [18, 14];
            return [10, 10];
        }

        function getDeviceProfile() {
            const width = window.innerWidth || 1280;
            const height = window.innerHeight || 900;
            const coarsePointer = window.matchMedia('(pointer: coarse)').matches;

            if (width <= 760) return 'phone';
            if (coarsePointer || width <= 1366) return 'tablet';
            return 'desktop';
        }

        function syncDeviceProfile() {
            const profile = getDeviceProfile();
            const height = window.innerHeight || 900;
            const isShortViewport = height < 980;

            document.body.classList.toggle('device-desktop', profile === 'desktop');
            document.body.classList.toggle('device-tablet', profile === 'tablet');
            document.body.classList.toggle('device-phone', profile === 'phone');
            document.body.classList.toggle('viewport-short', isShortViewport);
            document.body.classList.toggle('viewport-tall', !isShortViewport);
            document.body.dataset.deviceProfile = profile;
        }

        function renderNow() {
            syncDeviceProfile();
            renderFrame = null;
            renderToolbar();
            renderHeadlineStrip();
            renderSidebar();
            renderHero();
            renderSelection();
            renderLegend();
            renderMap();
        }

        function render(reason = 'state-change') {
            renderPendingReason = reason;
            if (renderFrame !== null) return;
            renderFrame = window.requestAnimationFrame(() => {
                const nextReason = renderPendingReason;
                renderPendingReason = 'state-change';
                renderNow(nextReason);
            });
        }

        function getActivePresetKey() {
            const active = new Set(state.activeStatuses);
            const preset = STAGE_PRESETS.find(item => areSetsEqual(active, new Set(item.statuses)));
            return preset ? preset.key : null;
        }

        function clearFilteredSelection() {
            if (state.selectedSegmentId && !getVisibleSegments().some(segment => segment.id === state.selectedSegmentId)) state.selectedSegmentId = null;
            if (state.selectedLotKey && !getVisibleLots().some(lot => lot.key === state.selectedLotKey)) state.selectedLotKey = null;
        }

        function hasDefaultScope() {
            const currentYear = new Date(atlas.generatedAtUtc).getFullYear();
            return state.activeRoute === 'ALL'
                && state.regionFilter === 'ALL'
                && state.sourceScope === 'ALL'
                && areSetsEqual(state.activeStatuses, new Set(ALL_STATUSES))
                && state.playbackYear === currentYear;
        }

        function clearInteractiveContext() {
            stopPlayback();
            closeActivePopup();

            if (state.selectedLotKey || state.selectedSegmentId) {
                state.selectedLotKey = null;
                state.selectedSegmentId = null;
                render();
                return;
            }

            if (!hasDefaultScope()) {
                state.activeRoute = 'ALL';
                state.regionFilter = 'ALL';
                state.sourceScope = 'ALL';
                state.activeStatuses = new Set(ALL_STATUSES);
                state.playbackYear = new Date(atlas.generatedAtUtc).getFullYear();
                render();
            }
        }

        function setActiveStatuses(statuses) {
            stopPlayback();
            state.activeStatuses = new Set(statuses);
            clearFilteredSelection();
            render();
        }

        function stopPlayback() {
            state.playbackPlaying = false;
            if (playbackTimer) {
                clearInterval(playbackTimer);
                playbackTimer = null;
            }
        }

        function getPlaybackOperationalYear(segment) {
            if (segment.status !== 'Open') return new Date(atlas.generatedAtUtc).getFullYear();
            // openYear = C#-computed last success milestone (section fully open)
            return segment.openYear || segment.startYear || segment.forecastOpenYear || new Date(atlas.generatedAtUtc).getFullYear();
        }

        function getSegmentConstructionStartYear(segment) {
            return segment.startYear || getPlaybackOperationalYear(segment);
        }

        // Returns 0.0-1.0 fraction of segment built at current playbackYear
        function getSegmentPlaybackFraction(segment) {
            const currentYear = new Date(atlas.generatedAtUtc).getFullYear();
            if (state.playbackYear >= currentYear) return segment.completionPercent / 100;
            if (segment.status === 'Open') {
                const openingYears = Array.isArray(segment.openingYears) ? [...segment.openingYears].sort((left, right) => left - right) : [];
                const opYear = getPlaybackOperationalYear(segment);
                const startY = getSegmentConstructionStartYear(segment);
                if (openingYears.length > 1) {
                    if (state.playbackYear >= opYear) return 1.0;
                    const achieved = openingYears.filter(year => year <= state.playbackYear).length;
                    if (achieved > 0) return achieved / openingYears.length;
                    if (state.playbackYear < startY) return 0.0;
                    const initialSpan = Math.max(1, openingYears[0] - startY);
                    return Math.min(1 / openingYears.length, Math.max(0.06, (state.playbackYear - startY) / initialSpan / openingYears.length));
                }
                if (state.playbackYear >= opYear) return 1.0;
                if (state.playbackYear < startY) return 0.0;
                const span = Math.max(1, opYear - startY);
                return Math.min(0.98, (state.playbackYear - startY) / span);
            }
            if (segment.status === 'UnderConstruction') {
                const startY = getSegmentConstructionStartYear(segment);
                if (state.playbackYear < startY) return 0.0;
                const yearsTotal = Math.max(1, currentYear - startY);
                const yearsPlayed = state.playbackYear - startY;
                return Math.min(segment.completionPercent / 100,
                    (yearsPlayed / yearsTotal) * (segment.completionPercent / 100));
            }
            return 0;
        }

        // Clip a lat/lon polyline to a fraction [0..1] of its arc length
        function truncatePolylineToFraction(latLngs, fraction) {
            if (!latLngs || latLngs.length < 2) return latLngs || [];
            if (fraction <= 0) return [];
            if (fraction >= 1.0) return latLngs;
            let totalLen = 0;
            const lengths = [];
            for (let i = 1; i < latLngs.length; i++) {
                const dy = latLngs[i][0] - latLngs[i-1][0];
                const dx = latLngs[i][1] - latLngs[i-1][1];
                const len = Math.sqrt(dx*dx + dy*dy);
                lengths.push(len);
                totalLen += len;
            }
            const target = totalLen * fraction;
            let cum = 0;
            const result = [latLngs[0]];
            for (let i = 0; i < lengths.length; i++) {
                if (cum + lengths[i] >= target) {
                    const t = (target - cum) / Math.max(lengths[i], 1e-9);
                    result.push([
                        latLngs[i][0] + t * (latLngs[i+1][0] - latLngs[i][0]),
                        latLngs[i][1] + t * (latLngs[i+1][1] - latLngs[i][1])
                    ]);
                    break;
                }
                cum += lengths[i];
                result.push(latLngs[i+1]);
            }
            return result;
        }

        function getPlaybackMinYear() {
            const active = atlas.segments.filter(segment => segment.status !== 'Planned');
            if (!active.length) return new Date(atlas.generatedAtUtc).getFullYear();
            return Math.min(...active.map(getSegmentConstructionStartYear));
        }

        function getSegmentRegion(segment) {
            const avgLon = segment.shape.reduce((sum, point) => sum + point.lon, 0) / Math.max(1, segment.shape.length);
            return avgLon < 25.6 ? 'WEST' : 'EAST';
        }

        function passesRegionFilter(segment) {
            return state.regionFilter === 'ALL' || getSegmentRegion(segment) === state.regionFilter;
        }

        function passesSourceFilter(segment) {
            return state.sourceScope === 'ALL' || segment.sourceQuality === state.sourceScope;
        }

        function passesPlaybackFilter(segment) {
            const currentYear = new Date(atlas.generatedAtUtc).getFullYear();
            if (state.playbackYear >= currentYear) return true;
            if (segment.status === 'Planned') return false;
            const startY = getSegmentConstructionStartYear(segment);
            return startY <= state.playbackYear;
        }

        function syncMapStageMode() {
            if (!el.mapStage) return;
            el.mapStage.classList.toggle('command-center', state.commandCenterMode);
            document.body.classList.toggle('command-center-mode', state.commandCenterMode);
            el.heroFocus.classList.toggle('active', state.commandCenterMode);
            el.heroFocus.setAttribute('aria-pressed', state.commandCenterMode ? 'true' : 'false');
            el.heroFocus.title = state.commandCenterMode ? t('commandCenterShow') : t('commandCenterHide');
        }

        const closeActivePopup = () => {
            if (!activePopup) return;
            suppressPopupReset = true;
            map.closePopup(activePopup);
            activePopup = null;
        };

        function updateViewport(viewKey, action) {
            if (lastViewportKey === viewKey) return;
            lastViewportKey = viewKey;
            action();
        }

        function getSingleActiveStatus() {
            return state.activeStatuses.size === 1 ? Array.from(state.activeStatuses)[0] : null;
        }

        function getOfficialUpdates() {
            if (state.playbackYear < new Date(atlas.generatedAtUtc).getFullYear()) return [];
            return atlas.officialUpdates.filter(item => (state.activeRoute === 'ALL' || item.routeCode === state.activeRoute) && (!item.status || state.activeStatuses.has(item.status)));
        }

        function interpolateShapeKm(points, targetKm) {
            if (!points.length) return null;
            if (points.length === 1) return [points[0].lat, points[0].lon];

            const clampedKm = Math.max(0, Math.min(targetKm, points[points.length - 1].cumulativeKm));
            for (let index = 1; index < points.length; index++) {
                const prev = points[index - 1];
                const point = points[index];
                if (point.cumulativeKm >= clampedKm) {
                    const span = Math.max(0.0001, point.cumulativeKm - prev.cumulativeKm);
                    const ratio = (clampedKm - prev.cumulativeKm) / span;
                    return [prev.lat + ((point.lat - prev.lat) * ratio), prev.lon + ((point.lon - prev.lon) * ratio)];
                }
            }

            const last = points[points.length - 1];
            return [last.lat, last.lon];
        }

        function buildLotPath(segment, lot) {
            const startKm = lot.startKm;
            const endKm = lot.endKm;
            const points = segment.shape;
            if (!points.length) return [];

            const result = [];
            for (let index = 0; index < points.length; index++) {
                const point = points[index];
                const prev = points[Math.max(0, index - 1)];
                const next = points[Math.min(points.length - 1, index + 1)];

                if (index > 0 && prev.cumulativeKm < startKm && point.cumulativeKm >= startKm) {
                    const ratio = (startKm - prev.cumulativeKm) / Math.max(0.0001, point.cumulativeKm - prev.cumulativeKm);
                    result.push([prev.lat + ((point.lat - prev.lat) * ratio), prev.lon + ((point.lon - prev.lon) * ratio)]);
                }

                if (point.cumulativeKm >= startKm && point.cumulativeKm <= endKm) {
                    result.push([point.lat, point.lon]);
                }

                if (index < points.length - 1 && point.cumulativeKm <= endKm && next.cumulativeKm > endKm) {
                    const ratio = (endKm - point.cumulativeKm) / Math.max(0.0001, next.cumulativeKm - point.cumulativeKm);
                    result.push([point.lat + ((next.lat - point.lat) * ratio), point.lon + ((next.lon - point.lon) * ratio)]);
                }
            }

            if (result.length >= 2) return result;

            const startPoint = interpolateShapeKm(points, startKm);
            const endPoint = interpolateShapeKm(points, endKm);
            return startPoint && endPoint ? [startPoint, endPoint] : points.map(point => [point.lat, point.lon]);
        }

        function getLotBounds(segment, lot) {
            const lotPath = buildLotPath(segment, lot);
            return lotPath.length >= 2 ? lotPath : [[lot.position.lat, lot.position.lon]];
        }

        function renderHeadlineStrip() {
            const selectedLot = getSelectedLot();
            const selectedSegment = selectedLot ? atlas.segments.find(item => item.id === selectedLot.segmentId) : getSelectedSegment();
            const selectedRoute = selectedSegment
                ? atlas.routes.find(item => item.routeCode === selectedSegment.routeCode)
                : atlas.routes.find(item => item.routeCode === state.activeRoute);
            const singleStatus = getSingleActiveStatus();
            const visibleSegments = getVisibleSegments();
            const officialUpdates = getOfficialUpdates();

            el.headlinePrimaryEyebrow.textContent = t('headlineNow');
            el.headlinePrimaryTitle.textContent = selectedLot
                ? `${selectedLot.routeCode} · ${selectedLot.lotCode}`
                : selectedSegment
                    ? `${selectedSegment.routeCode} · ${pick(selectedSegment.sectionName)}`
                    : singleStatus
                        ? `${labelForStatus(singleStatus)} · ${t('statusWindow')}`
                    : selectedRoute
                        ? `${selectedRoute.routeCode} · ${pick(selectedRoute.title)}`
                        : pick(atlas.network.title);
            el.headlinePrimaryCopy.textContent = selectedLot
                ? (pick(selectedLot.note) || pick(selectedSegment?.description))
                : selectedSegment
                    ? pick(selectedSegment.description)
                    : singleStatus
                        ? `${formatRouteCount(new Set(visibleSegments.map(item => item.routeCode)).size)} · ${getVisibleLots().length} ${t('lotCount')}`
                    : selectedRoute
                        ? pick(selectedRoute.highlight)
                        : pick(atlas.network.message);

            el.headlineSecondaryEyebrow.textContent = officialUpdates.length ? t('officialFeed') : t('headlineAudit');
            el.headlineSecondaryTitle.textContent = officialUpdates.length
                ? pick(officialUpdates[0].title)
                : `${atlas.summary.routeSpecificReferenceCount} ${t('evidenceRoute')}`;
            el.headlineSecondaryCopy.textContent = officialUpdates.length
                ? `${officialUpdates[0].publishedOn} · ${pick(officialUpdates[0].routeLabel)}`
                : `${atlas.summary.secondaryReferenceCount} ${t('supportingReference')} · ${atlas.summary.officialReferencePercent.toFixed(0)}%`;

            el.headlineTertiaryEyebrow.textContent = officialUpdates.length ? t('latestBulletin') : t('headlineScope');
            el.headlineTertiaryTitle.textContent = officialUpdates.length
                ? `${atlas.officialFeed.label} · ${atlas.officialFeed.publishedOn}`
                : `${atlas.summary.networkWideReferenceCount} ${t('evidenceNetwork')}`;
            el.headlineTertiaryCopy.textContent = officialUpdates.length
                ? atlas.officialFeed.sourceName
                : t('ownershipNotice');
        }

        function renderLotPopup(lot, segment) {
            return `<div class="map-popup">
                <div class="popup-header">
                    <div class="eyebrow">${t('popupOpenMap')}</div>
                    <strong>${pick(segment.routeName)} · ${lot.lotCode}</strong>
                    <div class="popup-subtitle">${pick(lot.title)}</div>
                </div>
                <div class="popup-menu"><span class="micro">${lot.routeCode}</span><span class="micro">${lot.lotCode}</span><span class="micro">${labelForStatus(lot.status)}</span><span class="micro">${formatLotTarget(lot)}</span></div>
                <div class="detail-pills"><span class="status-pill"><span class="dot" style="background:${statusMeta[lot.status].color}"></span>${labelForStatus(lot.status)}</span><span class="audit-badge ${lot.isDerived ? '' : 'strong'}">${labelForLotAudit(lot)}</span></div>
                <div class="progress-mini"><span style="width:${Math.max(4, Math.min(100, lot.completionPercent))}%; background:linear-gradient(90deg, ${statusMeta[lot.status].color}, #ffffff99)"></span></div>
                <div class="popup-metrics">
                    <div class="popup-metric"><div class="eyebrow">${t('length')}</div><div>${lot.startKm.toFixed(1)}–${lot.endKm.toFixed(1)} km</div></div>
                    <div class="popup-metric"><div class="eyebrow">${t('completion')}</div><div>${formatPercent(lot.completionPercent)}</div></div>
                    <div class="popup-metric"><div class="eyebrow">${t('targetYear')}</div><div>${formatLotTarget(lot)}</div></div>
                    <div class="popup-metric"><div class="eyebrow">${t('budget')}</div><div>${formatBudget(lot.budgetMillionEur)}</div></div>
                </div>
                <div class="popup-note tiny">${pick(lot.note) || pick(segment.description)}</div>
                <div class="popup-footer">${t('popupPinnedHint')}</div>
            </div>`;
        }

        function renderSegmentLotSummary(segment) {
            return segment.lots.map(lot => `<article class="popup-lot-item">
                <div class="card-head"><strong>${lot.lotCode}</strong><span class="status-pill"><span class="dot" style="background:${statusMeta[lot.status].color}"></span>${labelForStatus(lot.status)}</span></div>
                <div class="tiny">${pick(lot.title)}</div>
                <div class="detail-pills"><span class="micro">${lot.startKm.toFixed(1)}–${lot.endKm.toFixed(1)} km</span><span class="micro">${formatPercent(lot.completionPercent)}</span><span class="micro">${formatLotTarget(lot)}</span></div>
            </article>`).join('');
        }

        function renderSegmentPopup(segment) {
            const openYearStr = segment.openYear ? segment.openYear : (segment.forecastOpenYear ? `~${segment.forecastOpenYear}` : t('pending'));
            const travelTime = segment.estimatedTravelMinutes ? `${segment.estimatedTravelMinutes} min` : '—';
            const fundingBadge = formatFunding(segment.fundingProgram);
            const visibleLots = segment.lots.slice(0, 3);
            const hiddenLotCount = Math.max(0, segment.lots.length - visibleLots.length);
            return `<div class="map-popup">
                <div class="popup-header">
                    <div class="eyebrow">${t('segmentOverview')}</div>
                    <strong>${pick(segment.routeName)}</strong>
                    <div class="popup-subtitle">${pick(segment.sectionName)} · ${pick(segment.description)}</div>
                </div>
                <div class="detail-pills">
                    <span class="status-pill"><span class="dot" style="background:${statusMeta[segment.status].color}"></span>${labelForStatus(segment.status)}</span>
                    ${fundingBadge}
                    <span class="audit-badge">${labelForEvidenceGrade(segment.evidenceGrade)}</span>
                </div>
                <div class="popup-metrics">
                    <div class="popup-metric"><div class="eyebrow">${t('length')}</div><div>${formatKm(segment.lengthKm)}</div></div>
                    <div class="popup-metric"><div class="eyebrow">${t('completion')}</div><div>${formatPercent(segment.completionPercent)}</div></div>
                    <div class="popup-metric"><div class="eyebrow">${t('avgSpeed')}</div><div>${segment.maxSpeedKph ? `${segment.maxSpeedKph} km/h` : '—'}</div></div>
                    <div class="popup-metric"><div class="eyebrow">${t('openYear')}</div><div>${openYearStr}</div></div>
                </div>
                <div>
                    <div class="eyebrow">${t('segmentLots')}</div>
                    <div class="popup-lot-list" style="margin-top:8px">${renderSegmentLotSummary({ ...segment, lots: visibleLots })}</div>
                </div>
                ${hiddenLotCount ? `<div class="popup-footer">+${hiddenLotCount} ${t('lotCount')} · ${t('popupPinnedHint')}</div>` : `<div class="popup-footer">${t('popupPinnedHint')}</div>`}
            </div>`;
        }

        function renderOfficialList(items) {
            return items.length
                ? items.map(item => `<article class="official-item"><div class="card-head"><strong>${pick(item.title)}</strong><span class="micro">${item.publishedOn}</span></div><div class="tiny">${pick(item.detail)}</div><div class="tiny"><a href="${item.sourceUrl}" target="_blank" rel="noreferrer noopener">${t('publishedSource')}</a></div></article>`).join('')
                : `<div class="tiny">${t('pending')}</div>`;
        }

        function getVisibleSegments() {
            return atlas.segments.filter(segment =>
                (state.activeRoute === 'ALL' || segment.routeCode === state.activeRoute)
                && state.activeStatuses.has(segment.status)
                && passesRegionFilter(segment)
                && passesSourceFilter(segment)
                && passesPlaybackFilter(segment));
        }

        function getVisibleRoutes() {
            const routeCodes = new Set(getVisibleSegments().map(segment => segment.routeCode));
            return atlas.routes.filter(route => state.activeRoute === 'ALL' ? routeCodes.has(route.routeCode) : route.routeCode === state.activeRoute);
        }

        function getVisibleLots() {
            return getVisibleSegments().flatMap(segment => segment.lots.map(lot => ({ ...lot, routeCode: segment.routeCode, segmentId: segment.id, sectionCode: segment.sectionCode, sectionName: segment.sectionName, segmentStatus: segment.status })));
        }

        function getSelectedSegment() {
            return atlas.segments.find(segment => segment.id === state.selectedSegmentId) || null;
        }

        function getSelectedLot() {
            return getVisibleLots().find(lot => lot.key === state.selectedLotKey) || atlas.segments.flatMap(segment => segment.lots.map(lot => ({ ...lot, routeCode: segment.routeCode, segmentId: segment.id, sectionCode: segment.sectionCode, sectionName: segment.sectionName, segmentStatus: segment.status }))).find(lot => lot.key === state.selectedLotKey) || null;
        }

        function computeMetrics() {
            const segments = getVisibleSegments();
            const totalKm = segments.reduce((sum, item) => sum + item.lengthKm, 0);
            const openKm = segments.filter(item => item.status === 'Open').reduce((sum, item) => sum + item.lengthKm, 0);
            const constructionKm = segments.filter(item => item.status === 'UnderConstruction').reduce((sum, item) => sum + item.lengthKm, 0);
            const plannedKm = segments.filter(item => item.status === 'Planned').reduce((sum, item) => sum + item.lengthKm, 0);
            const closedKm = segments.filter(item => item.status === 'Closed').reduce((sum, item) => sum + item.lengthKm, 0);
            return {
                totalKm,
                openKm,
                constructionKm,
                plannedKm,
                closedKm,
                routeCount: new Set(segments.map(item => item.routeCode)).size,
                segmentCount: segments.length,
                lotCount: getVisibleLots().length,
                officialSegmentCount: segments.filter(item => item.sourceQuality === 'official').length,
                mixedSegmentCount: segments.filter(item => item.sourceQuality === 'mixed').length,
                modeledSegmentCount: segments.filter(item => item.sourceQuality === 'modeled').length,
                avgSpeed: segments.length ? Math.round(segments.reduce((sum, item) => sum + (item.maxSpeedKph || 0), 0) / segments.length) : 0,
                completionPercent: totalKm === 0 ? 0 : (openKm / totalKm) * 100,
                statusCounts: {
                    Open: segments.filter(item => item.status === 'Open').length,
                    UnderConstruction: segments.filter(item => item.status === 'UnderConstruction').length,
                    Planned: segments.filter(item => item.status === 'Planned').length,
                    Closed: segments.filter(item => item.status === 'Closed').length
                }
            };
        }

        function setBasemap() {
            if (basemapLayer) map.removeLayer(basemapLayer);
            const config = basemaps[state.basemap];
            basemapLayer = L.tileLayer(config.url, config.options).addTo(map);
        }

        function renderMapPresets() {
            const featuredMaps = Object.entries(basemaps).filter(([, config]) => config.featured);
            const presetMarkup = featuredMaps.map(([key, config]) => `<button type="button" class="map-preset ${state.basemap === key ? 'active' : ''}" data-map-preset="${key}"><strong>${pick(config.name)}</strong><span>${pick(config.tone)}</span></button>`).join('');
            const bindPresetClicks = container => {
                if (!container) return;
                container.querySelectorAll('[data-map-preset]').forEach(node => node.addEventListener('click', () => {
                    const presetKey = node.dataset.mapPreset;
                    if (!presetKey || presetKey === state.basemap) return;
                    state.basemap = presetKey;
                    setBasemap();
                    renderToolbar();
                }));
            };

            el.mapPresetsEyebrow.textContent = t('mapPresetsEyebrow');
            el.mapNote.textContent = t('mapNote');
            el.mapPresets.innerHTML = presetMarkup;
            if (el.desktopMapPresets) el.desktopMapPresets.innerHTML = presetMarkup;
            if (el.tabletMapPresets) el.tabletMapPresets.innerHTML = presetMarkup;
            bindPresetClicks(el.mapPresets);
            bindPresetClicks(el.desktopMapPresets);
            bindPresetClicks(el.tabletMapPresets);
        }

        function renderToolbar() {
            el.brandEyebrow.textContent = t('brandEyebrow');
            el.brandTitle.textContent = pick(atlas.network.title);
            el.brandSubtitle.textContent = pick(atlas.network.subtitle);
            el.languageLabel.textContent = t('languageLabel');
            el.basemapLabel.textContent = t('mapStyle');

            el.routePills.innerHTML = '';
            const allRoutes = document.createElement('button');
            allRoutes.className = `route-pill ${state.activeRoute === 'ALL' ? 'active' : ''}`;
            allRoutes.innerHTML = `<span class="code">GRID</span><span class="label">${t('allRoutes')}</span><span class="meta">${atlas.summary.routeCount} ${t('routeCount')} · ${formatPercent(atlas.summary.completionPercent)}</span><span class="route-pill-progress"><span style="width:${Math.max(4, Math.min(100, atlas.summary.completionPercent))}%"></span></span>`;
            allRoutes.addEventListener('click', () => {
                stopPlayback();
                state.activeRoute = 'ALL';
                state.selectedSegmentId = null;
                state.selectedLotKey = null;
                render();
            });
            el.routePills.appendChild(allRoutes);

            atlas.routes.forEach(route => {
                const button = document.createElement('button');
                button.className = `route-pill ${state.activeRoute === route.routeCode ? 'active' : ''}`;
                button.innerHTML = `<span class="code">${route.routeCode}</span><span class="label">${shortRouteName(route)}</span><span class="meta">${formatKm(route.totalKm)} · ${formatPercent(route.completionPercent)}</span><span class="route-pill-progress"><span style="width:${Math.max(4, Math.min(100, route.completionPercent))}%"></span></span>`;
                button.addEventListener('click', () => {
                    stopPlayback();
                    state.activeRoute = state.activeRoute === route.routeCode ? 'ALL' : route.routeCode;
                    state.selectedSegmentId = null;
                    state.selectedLotKey = null;
                    render();
                });
                el.routePills.appendChild(button);
            });

            el.languageTabs.innerHTML = ['bg', 'en'].map(lang => `<button class="tab ${state.lang === lang ? 'active' : ''}" type="button" data-lang="${lang}">${lang.toUpperCase()}</button>`).join('');
            el.languageTabs.querySelectorAll('[data-lang]').forEach(node => node.addEventListener('click', () => {
                state.lang = node.dataset.lang;
                render();
            }));

            el.basemapSelect.innerHTML = Object.entries(basemaps).map(([key, config]) => `<option value="${key}" ${state.basemap === key ? 'selected' : ''}>${pick(config.name)} · ${pick(config.tone)}</option>`).join('');
            renderMapPresets();
        }

        function renderStagePresetControls(container, segments) {
            if (!container) return;
            const activePresetKey = getActivePresetKey();
            container.innerHTML = STAGE_PRESETS.map(preset => {
                const count = segments.filter(segment => preset.statuses.includes(segment.status)).length;
                return `<button class="toggle stage-toggle ${activePresetKey === preset.key ? 'active' : ''}" data-preset="${preset.key}"><span class="label-stack"><strong>${t(preset.label)}</strong><span>${t(preset.hint)}</span></span><span class="micro" style="padding:2px 8px; border-color:${preset.accent}; color:${preset.accent}">${count}</span></button>`;
            }).join('');

            container.querySelectorAll('[data-preset]').forEach(node => node.addEventListener('click', () => {
                const preset = STAGE_PRESETS.find(item => item.key === node.dataset.preset);
                if (!preset) return;
                setActiveStatuses(activePresetKey === preset.key ? ALL_STATUSES : preset.statuses);
            }));
        }

        el.basemapSelect.addEventListener('change', event => {
            state.basemap = event.target.value;
            setBasemap();
            renderToolbar();
        });

        el.heroFocus.addEventListener('click', () => {
            state.commandCenterMode = !state.commandCenterMode;
            render();
        });

        el.selectionClear.addEventListener('click', () => {
            clearInteractiveContext();
        });

        window.addEventListener('keydown', event => {
            if (event.key !== 'Escape') return;
            if (state.selectedLotKey || state.selectedSegmentId || !hasDefaultScope()) {
                clearInteractiveContext();
            }
        });

        window.addEventListener('resize', () => {
            if (resizeTimer) clearTimeout(resizeTimer);
            resizeTimer = window.setTimeout(() => {
                syncDeviceProfile();
                map.invalidateSize({ pan: false, debounceMoveend: true });
                render('resize');
            }, 120);
        });

        window.addEventListener('orientationchange', () => {
            if (resizeTimer) clearTimeout(resizeTimer);
            resizeTimer = window.setTimeout(() => {
                syncDeviceProfile();
                map.invalidateSize({ pan: false, debounceMoveend: true });
                render('orientation');
            }, 180);
        });

        function renderSidebar() {
            const generated = new Date(atlas.generatedAtUtc);
            const metrics = computeMetrics();
            const selectedRoute = atlas.routes.find(route => route.routeCode === state.activeRoute) || null;
            const baseSegments = atlas.segments.filter(segment => (state.activeRoute === 'ALL' || segment.routeCode === state.activeRoute) && passesPlaybackFilter(segment));
            const scopedSegments = baseSegments.filter(segment => passesRegionFilter(segment));
            const officialUpdates = getOfficialUpdates();

            el.summaryEyebrow.textContent = t('summaryEyebrow');
            el.summaryTitle.textContent = selectedRoute ? `${selectedRoute.routeCode} · ${pick(selectedRoute.title)}` : pick(atlas.network.title);
            el.summarySubtitle.textContent = selectedRoute ? pick(selectedRoute.highlight) : pick(atlas.network.message);
            el.generatedPill.textContent = `${t('generated')} ${generated.toLocaleString(state.lang === 'bg' ? 'bg-BG' : 'en-US')}`;
            el.summaryKm.textContent = selectedRoute ? formatKm(selectedRoute.totalKm) : formatKm(metrics.totalKm || atlas.summary.totalKm);
            el.summaryState.textContent = selectedRoute ? pick(selectedRoute.sectionLabel) : `${metrics.routeCount} ${t('routeCount')} · ${metrics.lotCount} ${t('lotCount')}`;
            el.summaryPercent.textContent = selectedRoute ? formatPercent(selectedRoute.completionPercent) : formatPercent(metrics.completionPercent || atlas.summary.completionPercent);
            el.summaryProgress.style.width = `${selectedRoute ? selectedRoute.completionPercent : (metrics.completionPercent || atlas.summary.completionPercent)}%`;

            if (el.tabletSurfaceEyebrow) el.tabletSurfaceEyebrow.textContent = t('controlSurfaceEyebrow');
            if (el.tabletSummaryTitle) el.tabletSummaryTitle.textContent = el.summaryTitle.textContent;
            if (el.tabletSummarySubtitle) el.tabletSummarySubtitle.textContent = el.summarySubtitle.textContent;
            if (el.tabletGeneratedPill) el.tabletGeneratedPill.textContent = el.generatedPill.textContent;
            if (el.tabletSummaryKm) el.tabletSummaryKm.textContent = el.summaryKm.textContent;
            if (el.tabletSummaryState) el.tabletSummaryState.textContent = el.summaryState.textContent;
            if (el.tabletSummaryPercent) el.tabletSummaryPercent.textContent = el.summaryPercent.textContent;
            if (el.tabletSummaryProgress) el.tabletSummaryProgress.style.width = el.summaryProgress.style.width;
            if (el.tabletSummaryKpis) {
                el.tabletSummaryKpis.innerHTML = [
                    { label: t('openToday'), value: formatKm(metrics.openKm) },
                    { label: t('inBuild'), value: formatKm(metrics.constructionKm) },
                    { label: t('plannedNow'), value: formatKm(metrics.plannedKm) },
                    { label: t('visibleLots'), value: `${metrics.lotCount}` }
                ].map(item => `<article class="tablet-kpi-chip"><div class="eyebrow">${item.label}</div><div class="value">${item.value}</div></article>`).join('');
            }

            el.filtersEyebrow.textContent = t('filtersEyebrow');
            el.filtersTitle.textContent = t('filtersTitle');
            el.filtersCopy.textContent = t('filtersCopy');
            el.stagePresetsLabel.textContent = t('stagePresets');
            if (el.stageQuickLabel) el.stageQuickLabel.textContent = t('stagePresets');
            el.regionPresetsLabel.textContent = t('regionPresets');
            if (el.tabletRegionPresetsLabel) el.tabletRegionPresetsLabel.textContent = t('regionPresets');
            el.sourcePresetsLabel.textContent = t('sourcePresets');
            if (el.tabletSourcePresetsLabel) el.tabletSourcePresetsLabel.textContent = t('sourcePresets');
            el.statusFiltersLabel.textContent = t('granularStatus');
            if (el.tabletStatusFiltersLabel) el.tabletStatusFiltersLabel.textContent = t('granularStatus');
            el.playbackLabel.textContent = t('playbackLabel');
            if (el.tabletPlaybackLabel) el.tabletPlaybackLabel.textContent = t('playbackLabel');
            el.playbackButton.textContent = state.playbackPlaying ? t('playbackPause') : t('playbackPlay');
            if (el.tabletPlaybackButton) el.tabletPlaybackButton.textContent = el.playbackButton.textContent;
            el.playbackYearLabel.textContent = `${t('playbackYear')}: ${state.playbackYear}`;
            if (el.tabletPlaybackYearLabel) el.tabletPlaybackYearLabel.textContent = el.playbackYearLabel.textContent;
            el.playbackRangeLabel.textContent = t('playbackRange');
            if (el.tabletPlaybackRangeLabel) el.tabletPlaybackRangeLabel.textContent = el.playbackRangeLabel.textContent;
            el.playbackRange.min = `${getPlaybackMinYear()}`;
            el.playbackRange.max = `${generated.getFullYear()}`;
            el.playbackRange.value = `${state.playbackYear}`;
            if (el.tabletPlaybackRange) {
                el.tabletPlaybackRange.min = el.playbackRange.min;
                el.tabletPlaybackRange.max = el.playbackRange.max;
                el.tabletPlaybackRange.value = el.playbackRange.value;
            }
            if (el.tabletMapPresetsLabel) el.tabletMapPresetsLabel.textContent = t('mapPresetsEyebrow');
            if (el.tabletLegendLabel) el.tabletLegendLabel.textContent = t('statusLegend');

            // ── Year-overlay + km progress bar ────────────────────────────────────
            const currentYear = generated.getFullYear();
            const isHistoricMode = state.playbackYear < currentYear;
            const showYearOverlay = state.playbackPlaying || isHistoricMode;
            if (el.yearOverlay) {
                el.yearOverlay.classList.toggle('visible', showYearOverlay);
                if (el.yearOverlayYear) {
                    const prevYear = parseInt(el.yearOverlay.dataset.lastYear || '0');
                    if (prevYear !== state.playbackYear && showYearOverlay) {
                        el.yearOverlayYear.animate(
                            [{opacity: 0.2, transform: 'scale(1.18)'}, {opacity: 1, transform: 'scale(1)'}],
                            {duration: 300, easing: 'cubic-bezier(0.34,1.56,0.64,1)', fill: 'forwards'}
                        );
                    }
                    el.yearOverlay.dataset.lastYear = state.playbackYear;
                    el.yearOverlayYear.textContent = state.playbackYear;
                }
                if (el.yearOverlayKm && showYearOverlay) {
                    // Show milestone achievement text if any segment opened exactly this year
                    const justOpened = atlas.segments.filter(s => s.status === 'Open' && Array.isArray(s.openingYears) && s.openingYears.includes(state.playbackYear));
                    if (justOpened.length > 0) {
                        const names = justOpened.map(s => pick(s.sectionName) || s.routeCode).join(', ');
                        el.yearOverlayKm.textContent = `🛣 ${names} ${t('yearOpenedSuffix')}`;
                    } else {
                        const builtKm = atlas.segments
                            .filter(s => s.status === 'Open')
                            .reduce((sum, s) => sum + ((s.lengthKm || 0) * getSegmentPlaybackFraction(s)), 0);
                        el.yearOverlayKm.textContent = builtKm > 0 ? `${Math.round(builtKm)} ${t('yearKmOpen')}` : '';
                    }
                }
            }
            // km progress bar in the playback control panel
            if (el.playbackKmFill) {
                const allOpen = atlas.segments.filter(s => s.status === 'Open');
                const totalOpenKm = allOpen.reduce((sum, s) => sum + (s.lengthKm || 0), 0);
                const builtKm = allOpen.reduce((sum, s) => sum + ((s.lengthKm || 0) * getSegmentPlaybackFraction(s)), 0);
                const pct = totalOpenKm > 0 ? Math.round(builtKm / totalOpenKm * 100) : 0;
                el.playbackKmFill.style.width = `${pct}%`;
                if (el.playbackKmLabel) {
                    el.playbackKmLabel.textContent = isHistoricMode ? `${Math.round(builtKm)} km  ·  ${pct}%` : `${Math.round(totalOpenKm)} km  ·  100%`;
                }
                if (el.tabletPlaybackKmFill) el.tabletPlaybackKmFill.style.width = `${pct}%`;
                if (el.tabletPlaybackKmLabel) el.tabletPlaybackKmLabel.textContent = el.playbackKmLabel.textContent;
            }
            el.networkEyebrow.textContent = t('networkEyebrow');
            el.networkTitle.textContent = t('networkTitle');
            el.notesEyebrow.textContent = t('notesEyebrow');
            el.notesTitle.textContent = t('notesTitle');
            el.routesEyebrow.textContent = t('routesEyebrow');
            el.routesTitle.textContent = t('routesTitle');
            el.lotsEyebrow.textContent = t('lotsEyebrow');
            el.lotsTitle.textContent = t('lotsTitle');
            if (el.tabletTabsEyebrow) el.tabletTabsEyebrow.textContent = t('infographicTabsEyebrow');
            if (el.tabletTabFacts) el.tabletTabFacts.textContent = t('factsTab');
            if (el.tabletTabNotes) el.tabletTabNotes.textContent = t('notesTab');
            if (el.tabletTabRoutes) el.tabletTabRoutes.textContent = t('routesTab');
            if (el.tabletTabLots) el.tabletTabLots.textContent = t('lotsTab');

            renderStagePresetControls(el.stagePresets, scopedSegments);
            renderStagePresetControls(el.stageQuickPresets, scopedSegments);

            el.regionPresets.innerHTML = REGION_PRESETS.map(preset => {
                const count = baseSegments.filter(segment => preset.key === 'ALL' || getSegmentRegion(segment) === preset.key).length;
                return `<button class="toggle stage-toggle ${state.regionFilter === preset.key ? 'active' : ''}" data-region="${preset.key}"><span class="label-stack"><strong>${t(preset.label)}</strong><span>${t(preset.hint)}</span></span><span class="micro" style="padding:2px 8px">${count}</span></button>`;
            }).join('');
            el.regionPresets.querySelectorAll('[data-region]').forEach(node => node.addEventListener('click', () => {
                stopPlayback();
                state.regionFilter = state.regionFilter === node.dataset.region ? 'ALL' : node.dataset.region;
                clearFilteredSelection();
                render();
            }));
            if (el.tabletRegionPresets) {
                el.tabletRegionPresets.innerHTML = el.regionPresets.innerHTML;
                el.tabletRegionPresets.querySelectorAll('[data-region]').forEach(node => node.addEventListener('click', () => {
                    stopPlayback();
                    state.regionFilter = state.regionFilter === node.dataset.region ? 'ALL' : node.dataset.region;
                    clearFilteredSelection();
                    render();
                }));
            }

            const sourceScopedSegments = scopedSegments.filter(segment => state.activeStatuses.has(segment.status));
            el.sourcePresets.innerHTML = SOURCE_PRESETS.map(preset => {
                const count = sourceScopedSegments.filter(segment => preset.key === 'ALL' || segment.sourceQuality === preset.key).length;
                return `<button class="toggle stage-toggle ${state.sourceScope === preset.key ? 'active' : ''}" data-source-scope="${preset.key}"><span class="label-stack"><strong>${t(preset.label)}</strong><span>${t(preset.hint)}</span></span><span class="micro" style="padding:2px 8px">${count}</span></button>`;
            }).join('');
            el.sourcePresets.querySelectorAll('[data-source-scope]').forEach(node => node.addEventListener('click', () => {
                stopPlayback();
                state.sourceScope = state.sourceScope === node.dataset.sourceScope ? 'ALL' : node.dataset.sourceScope;
                clearFilteredSelection();
                render();
            }));
            if (el.tabletSourcePresets) {
                el.tabletSourcePresets.innerHTML = el.sourcePresets.innerHTML;
                el.tabletSourcePresets.querySelectorAll('[data-source-scope]').forEach(node => node.addEventListener('click', () => {
                    stopPlayback();
                    state.sourceScope = state.sourceScope === node.dataset.sourceScope ? 'ALL' : node.dataset.sourceScope;
                    clearFilteredSelection();
                    render();
                }));
            }

            el.statusFilters.innerHTML = ['Open', 'UnderConstruction', 'Planned', 'Closed'].map(status => {
                const count = scopedSegments.filter(segment => segment.status === status).length;
                return `<button class="toggle ${state.activeStatuses.has(status) ? 'active' : ''}" data-status="${status}"><span>${labelForStatus(status)}</span><span class="micro" style="padding:2px 8px">${count}</span></button>`;
            }).join('');
            el.statusFilters.querySelectorAll('[data-status]').forEach(node => node.addEventListener('click', () => {
                stopPlayback();
                const status = node.dataset.status;
                if (state.activeStatuses.has(status) && state.activeStatuses.size > 1) state.activeStatuses.delete(status); else state.activeStatuses.add(status);
                clearFilteredSelection();
                render();
            }));
            if (el.tabletStatusFilters) {
                el.tabletStatusFilters.innerHTML = el.statusFilters.innerHTML;
                el.tabletStatusFilters.querySelectorAll('[data-status]').forEach(node => node.addEventListener('click', () => {
                    stopPlayback();
                    const status = node.dataset.status;
                    if (state.activeStatuses.has(status) && state.activeStatuses.size > 1) state.activeStatuses.delete(status); else state.activeStatuses.add(status);
                    clearFilteredSelection();
                    render();
                }));
            }

            el.playbackButton.onclick = () => {
                if (state.playbackPlaying) {
                    stopPlayback();
                    render();
                    return;
                }

                state.playbackPlaying = true;
                playbackTimer = setInterval(() => {
                    const maxYear = generated.getFullYear();
                    if (state.playbackYear >= maxYear) {
                        stopPlayback();
                        render();
                        return;
                    }

                    state.playbackYear += 1;
                    clearFilteredSelection();
                    render();
                }, 600);
                render();
            };
            if (el.tabletPlaybackButton) el.tabletPlaybackButton.onclick = el.playbackButton.onclick;

            el.playbackRange.oninput = event => {
                stopPlayback();
                state.playbackYear = Number(event.target.value);
                clearFilteredSelection();
                render();
            };
            if (el.tabletPlaybackRange) el.tabletPlaybackRange.oninput = el.playbackRange.oninput;

            const kpis = [
                { label: t('openToday'), value: formatKm(metrics.openKm), statuses: ['Open'] },
                { label: t('inBuild'), value: formatKm(metrics.constructionKm), statuses: ['UnderConstruction'] },
                { label: t('plannedNow'), value: formatKm(metrics.plannedKm), statuses: ['Planned'] },
                { label: t('visibleLots'), value: `${metrics.lotCount}`, statuses: null },
                { label: t('officialLots'), value: `${metrics.officialSegmentCount}`, statuses: null },
                { label: t('modeledSources'), value: `${metrics.modeledSegmentCount}`, statuses: null }
            ];
            el.sidebarKpis.innerHTML = kpis.map(item => `<article class="kpi ${item.statuses ? 'interactive' : ''} ${item.statuses && areSetsEqual(new Set(item.statuses), state.activeStatuses) ? 'active' : ''}" ${item.statuses ? `data-kpi-statuses="${item.statuses.join(',')}"` : ''}><div class="eyebrow">${item.label}</div><div class="value">${item.value}</div></article>`).join('');
            el.sidebarKpis.querySelectorAll('[data-kpi-statuses]').forEach(node => node.addEventListener('click', () => {
                const statuses = node.dataset.kpiStatuses.split(',');
                const isActive = areSetsEqual(new Set(statuses), state.activeStatuses);
                setActiveStatuses(isActive ? ALL_STATUSES : statuses);
            }));

            const selectedLot = getSelectedLot();
            const selectedSegment = selectedLot ? atlas.segments.find(item => item.id === selectedLot.segmentId) : getSelectedSegment();
            const activeContextRoute = selectedSegment
                ? atlas.routes.find(item => item.routeCode === selectedSegment.routeCode)
                : selectedRoute;
            const contextTitle = selectedLot
                ? `${selectedLot.routeCode} · ${selectedLot.lotCode}`
                : selectedSegment
                    ? `${selectedSegment.routeCode} · ${pick(selectedSegment.sectionName)}`
                    : activeContextRoute
                        ? `${activeContextRoute.routeCode} · ${pick(activeContextRoute.title)}`
                        : pick(atlas.network.title);
            const contextText = selectedLot
                ? (pick(selectedLot.note) || pick(selectedSegment?.description))
                : selectedSegment
                    ? (pick(selectedSegment.description) || pick(selectedSegment.importance))
                    : activeContextRoute
                        ? pick(activeContextRoute.highlight)
                        : pick(atlas.network.message);
            el.notesBody.innerHTML = `
                <div class="note-block">
                    <strong>${t('selectedContext')}</strong>
                    <div class="tiny" style="margin-top:8px">${contextTitle}</div>
                    <div class="tiny" style="margin-top:8px">${contextText}</div>
                </div>
                <div class="note-block">
                    <strong>${t('latestUpdate')}</strong>
                    <div class="tiny" style="margin-top:8px">${generated.toLocaleString(state.lang === 'bg' ? 'bg-BG' : 'en-US')}</div>
                    <div class="tiny" style="margin-top:8px">${atlas.summary.explicitLotCount}/${atlas.summary.explicitLotCount + atlas.summary.derivedLotCount} ${t('auditedLots')}</div>
                </div>
                <div class="note-block">
                    <strong>${t('currentScope')}</strong>
                    <div class="tiny" style="margin-top:8px">${t('bulgariaScopeNote')}</div>
                    <div class="detail-pills" style="margin-top:10px"><span class="audit-badge strong">${atlas.summary.sourceCoveragePercent.toFixed(0)}%</span><span class="audit-badge">${atlas.summary.officialSegmentCount} ${t('officialLots')}</span><span class="audit-badge">${atlas.summary.modeledSegmentCount} ${t('modeledSources')}</span></div>
                </div>
                <div class="note-block">
                    <strong>${t('auditAccuracy')}</strong>
                    <div class="tiny" style="margin-top:8px">${t('auditModeledNotice')}</div>
                    <div class="detail-pills" style="margin-top:10px"><span class="audit-badge strong">${t('auditVerifiedShare')}: ${atlas.summary.sourceCoveragePercent.toFixed(0)}%</span><span class="audit-badge">${atlas.summary.routeSpecificReferenceCount} ${t('evidenceRoute')}</span><span class="audit-badge">${atlas.summary.secondaryReferenceCount} ${t('supportingReference')}</span></div>
                </div>
                <div class="note-block">
                    <strong>${t('officialFeed')}</strong>
                    <div class="official-list" style="margin-top:8px">${renderOfficialList(officialUpdates.slice(0, 3))}</div>
                </div>`;

            el.legalEyebrow.textContent = t('copyrightLabel');
            el.legalTitle.textContent = pick(atlas.copyright.notice);
            el.legalCopy.textContent = pick(atlas.copyright.usage);

            const routes = getVisibleRoutes();
            el.routesCount.textContent = `${routes.length} ${t('routeCount')}`;
            el.routeList.innerHTML = routes.length
                ? routes.map(route => `<article class="route-card ${state.activeRoute === route.routeCode ? 'active' : ''}" data-route="${route.routeCode}"><div class="card-head"><div><strong>${shortRouteName(route)}</strong><div class="tiny" style="margin-top:6px">${route.routeCode} · ${pick(route.sectionLabel)}</div></div><span class="status-pill"><span class="dot" style="background:var(--accent)"></span>${formatPercent(route.completionPercent)}</span></div><div class="selection-meta" style="margin-top:10px"><span class="micro">${formatKm(route.totalKm)}</span><span class="micro">${route.lotCount} ${t('lotCount')}</span><span class="micro">${route.storyYears} ${t('years')}</span></div><div class="tiny" style="margin-top:10px">${pick(route.highlight)}</div></article>`).join('')
                : `<div class="tiny">${t('noRoutes')}</div>`;
            el.routeList.querySelectorAll('[data-route]').forEach(node => node.addEventListener('click', () => {
                stopPlayback();
                state.activeRoute = state.activeRoute === node.dataset.route ? 'ALL' : node.dataset.route;
                state.selectedSegmentId = null;
                state.selectedLotKey = null;
                render();
            }));

            const lots = getVisibleLots();
            el.lotsCount.textContent = `${lots.length} ${t('lotCount')}`;
            el.lotList.innerHTML = lots.length
                ? lots.map(lot => `<article class="lot-card ${lot.isDerived ? 'derived' : ''} ${state.selectedLotKey === lot.key ? 'active' : ''}" data-lot="${lot.key}"><div class="card-head"><div><strong>${lot.lotCode}</strong><div class="tiny" style="margin-top:6px">${pick(lot.title)}</div></div><span class="status-pill"><span class="dot" style="background:${statusMeta[lot.status].color}"></span>${labelForStatus(lot.status)}</span></div><div class="lot-actions" style="margin-top:10px"><span class="micro">${formatKm(lot.endKm - lot.startKm)}</span><span class="micro">${formatPercent(lot.completionPercent)}</span><span class="micro">${formatLotTarget(lot)}</span><span class="audit-badge ${lot.isDerived ? '' : 'strong'}">${labelForLotAudit(lot)}</span></div><div class="tiny" style="margin-top:10px">${pick(lot.note) || pick(lot.title)}</div></article>`).join('')
                : `<div class="tiny">${t('noLots')}</div>`;
            el.lotList.querySelectorAll('[data-lot]').forEach(node => node.addEventListener('click', () => {
                stopPlayback();
                const lot = lots.find(item => item.key === node.dataset.lot);
                if (state.selectedLotKey === node.dataset.lot) {
                    state.selectedLotKey = null;
                    state.selectedSegmentId = null;
                }
                else {
                    state.selectedLotKey = node.dataset.lot;
                    state.selectedSegmentId = lot ? lot.segmentId : state.selectedSegmentId;
                }
                render();
            }));

            const tabletPanels = [
                { key: 'facts', button: el.tabletTabFacts, panel: el.networkPanel },
                { key: 'notes', button: el.tabletTabNotes, panel: el.notesPanel },
                { key: 'routes', button: el.tabletTabRoutes, panel: el.routesPanel },
                { key: 'lots', button: el.tabletTabLots, panel: el.lotsPanel }
            ];
            tabletPanels.forEach(item => {
                if (item.button) {
                    item.button.classList.toggle('active', state.tabletInfoTab === item.key);
                    item.button.onclick = () => {
                        state.tabletInfoTab = item.key;
                        render();
                    };
                }
                if (item.panel) {
                    item.panel.hidden = document.body.classList.contains('device-tablet') && state.tabletInfoTab !== item.key;
                }
            });
        }

        function renderHero() {
            const metrics = computeMetrics();
            const visibleRoutes = getVisibleRoutes();
            const auditedTotal = atlas.summary.explicitLotCount + atlas.summary.derivedLotCount;
            const auditedShare = auditedTotal === 0 ? 0 : (atlas.summary.explicitLotCount / auditedTotal) * 100;
            const dominantStatus = Object.entries(metrics.statusCounts)
                .sort((left, right) => right[1] - left[1])[0]?.[0] || 'Open';
            el.heroEyebrow.textContent = t('heroEyebrow');
            el.heroTitle.textContent = t('heroTitle');
            el.heroSubtitle.textContent = t('heroSubtitle');
            el.heroFocus.innerHTML = `<span class="dot" style="background:${state.commandCenterMode ? 'var(--accent)' : 'var(--accent-3)'}"></span>${state.commandCenterMode ? t('commandCenterOn') : t('commandCenterOff')}`;
            el.heroStats.innerHTML = [
                { label: t('totalLength'), value: formatKm(metrics.totalKm || atlas.summary.totalKm) },
                { label: t('completion'), value: formatPercent(metrics.completionPercent || atlas.summary.completionPercent) },
                { label: t('visibleLots'), value: `${metrics.lotCount}` },
                { label: t('heroSignalReadiness'), value: `${formatKm(metrics.openKm)}` },
                { label: t('heroSignalAudit'), value: `${auditedShare.toFixed(0)}%` },
                { label: t('status'), value: labelForStatus(dominantStatus) },
                { label: t('heroRoutes'), value: `${visibleRoutes.length}` },
                { label: t('avgSpeed'), value: metrics.avgSpeed ? `${metrics.avgSpeed} km/h` : '—' }
            ].map(item => `<article class="kpi"><div class="eyebrow">${item.label}</div><div class="value">${item.value}</div></article>`).join('');
            syncMapStageMode();
        }

        function renderSelection() {
            const lot = getSelectedLot();
            const segment = lot ? atlas.segments.find(item => item.id === lot.segmentId) : getSelectedSegment();
            const route = segment ? atlas.routes.find(item => item.routeCode === segment.routeCode) : atlas.routes.find(item => item.routeCode === state.activeRoute);
            const activeStatus = getSingleActiveStatus();
            const visibleSegments = getVisibleSegments();
            const visibleLots = getVisibleLots();
            const officialUpdates = getOfficialUpdates();
            const hasSelectionOrScope = Boolean(lot || segment || route || activeStatus || !hasDefaultScope());
            const panelMode = lot || segment ? 'full' : (route || activeStatus ? 'compact' : 'passive');

            el.selectionPanel.classList.toggle('compact', panelMode === 'compact');
            el.selectionPanel.classList.toggle('passive', panelMode === 'passive');

            el.selectionClear.hidden = !hasSelectionOrScope;
            el.selectionClear.textContent = (lot || segment) ? t('clearSelection') : t('resetView');

            el.selectionEyebrow.textContent = t('selectionEyebrow');

            if (lot) {
                el.selectionTitle.textContent = `${pick(segment.routeName)} · ${lot.lotCode}`;
                el.selectionSubtitle.textContent = `${pick(lot.title)} · ${pick(segment.sectionName)} · ${lot.routeCode}`;
                el.selectionStatus.innerHTML = `<span class="dot" style="background:${statusMeta[lot.status].color}"></span>${labelForStatus(lot.status)}`;
                el.selectionPills.innerHTML = `<span class="micro">${formatKm(lot.endKm - lot.startKm)}</span><span class="micro">${formatPercent(lot.completionPercent)}</span><span class="micro">${formatLotTarget(lot)}</span>`;
                el.selectionFacts.innerHTML = `
                    <div class="fact"><strong>${t('length')}</strong><div class="tiny">${lot.startKm.toFixed(1)}–${lot.endKm.toFixed(1)} km</div></div>
                    <div class="fact"><strong>${t('budget')}</strong><div class="tiny">${formatBudget(lot.budgetMillionEur)}</div></div>
                    <div class="fact"><strong>${t('contractor')}</strong><div class="tiny">${lot.contractor || t('pending')}</div></div>
                    <div class="fact"><strong>${t('targetYear')}</strong><div class="tiny">${formatLotTarget(lot)}</div></div>
                    <div class="fact"><strong>${t('sourceQuality')}</strong><div class="tiny">${labelForSourceQuality(segment.sourceQuality)}</div></div>
                    <div class="fact"><strong>${t('evidenceGrade')}</strong><div class="tiny">${labelForEvidenceGrade(segment.evidenceGrade)}</div></div>
                    <div class="fact"><strong>${t('officialRecord')}</strong><div class="tiny">${formatSource(segment?.officialSource?.name, segment?.officialSource?.url)}</div></div>
                    <div class="fact"><strong>${t('supportingReference')}</strong><div class="tiny">${formatSupportingSource(segment?.secondarySource)}</div></div>
                    <div class="fact"><strong>${t('sourceVerifiedOn')}</strong><div class="tiny">${segment?.officialSource?.verifiedOn || t('pending')}</div></div>`;
                el.selectionNote.textContent = pick(lot.note) || pick(segment.description);
                renderTimeline(segment, lot);
                return;
            }

            if (segment) {
                el.selectionTitle.textContent = `${pick(segment.routeName)}`;
                el.selectionSubtitle.textContent = `${pick(segment.sectionName)} · ${segment.routeCode}`;
                el.selectionStatus.innerHTML = `<span class="dot" style="background:${statusMeta[segment.status].color}"></span>${labelForStatus(segment.status)}`;
                el.selectionPills.innerHTML = `<span class="micro">${segment.sectionCode || '—'}</span><span class="micro">${formatKm(segment.lengthKm)}</span><span class="micro">${segment.lots.length} ${t('lotCount')}</span>`;
                el.selectionFacts.innerHTML = `
                    <div class="fact"><strong>${t('completion')}</strong><div class="tiny">${formatPercent(segment.completionPercent)}</div></div>
                    <div class="fact"><strong>${t('avgSpeed')}</strong><div class="tiny">${segment.maxSpeedKph ? `${segment.maxSpeedKph} km/h` : '—'}</div></div>
                    <div class="fact"><strong>${t('importance')}</strong><div class="tiny">${pick(segment.importance) || t('pending')}</div></div>
                    <div class="fact"><strong>${t('sourceQuality')}</strong><div class="tiny">${labelForSourceQuality(segment.sourceQuality)}</div></div>
                    <div class="fact"><strong>${t('evidenceGrade')}</strong><div class="tiny">${labelForEvidenceGrade(segment.evidenceGrade)}</div></div>
                    <div class="fact"><strong>${t('officialRecord')}</strong><div class="tiny">${formatSource(segment.officialSource?.name, segment.officialSource?.url)}</div></div>
                    <div class="fact"><strong>${t('supportingReference')}</strong><div class="tiny">${formatSupportingSource(segment.secondarySource)}</div></div>
                    <div class="fact"><strong>${t('sourceVerifiedOn')}</strong><div class="tiny">${segment.officialSource?.verifiedOn || t('pending')}</div></div>`;
                el.selectionNote.textContent = pick(segment.importance) || pick(segment.description);
                renderTimeline(segment, null);
                return;
            }

            if (route) {
                el.selectionTitle.textContent = `${shortRouteName(route)}`;
                el.selectionSubtitle.textContent = `${route.routeCode} · ${pick(route.sectionLabel)}`;
                el.selectionStatus.innerHTML = `<span class="dot" style="background:var(--accent)"></span>${t('routeOverview')}`;
                el.selectionPills.innerHTML = `<span class="micro">${formatKm(route.totalKm)}</span><span class="micro">${route.lotCount} ${t('lotCount')}</span><span class="micro">${route.storyYears} ${t('years')}</span>`;
                el.selectionFacts.innerHTML = `
                    <div class="fact"><strong>${t('completion')}</strong><div class="tiny">${formatPercent(route.completionPercent)}</div></div>
                    <div class="fact"><strong>${t('openToday')}</strong><div class="tiny">${formatKm(route.openKm)}</div></div>
                    <div class="fact"><strong>${t('inBuild')}</strong><div class="tiny">${formatKm(route.constructionKm)}</div></div>
                    <div class="fact"><strong>${t('plannedNow')}</strong><div class="tiny">${formatKm(route.plannedKm)}</div></div>`;
                el.selectionNote.textContent = pick(route.highlight);
                el.timeline.innerHTML = renderTimelineItems(route.milestones);
                return;
            }

            if (activeStatus) {
                const totalKm = visibleSegments.reduce((sum, item) => sum + item.lengthKm, 0);
                const avgCompletion = visibleSegments.length ? visibleSegments.reduce((sum, item) => sum + item.completionPercent, 0) / visibleSegments.length : 0;
                el.selectionTitle.textContent = `${labelForStatus(activeStatus)} · ${t('statusWindow')}`;
                el.selectionSubtitle.textContent = `${formatRouteCount(new Set(visibleSegments.map(item => item.routeCode)).size)} · ${visibleLots.length} ${t('lotCount')}`;
                el.selectionStatus.innerHTML = `<span class="dot" style="background:${statusMeta[activeStatus].color}"></span>${labelForStatus(activeStatus)}`;
                el.selectionPills.innerHTML = `<span class="micro">${formatKm(totalKm)}</span><span class="micro">${formatRouteCount(new Set(visibleSegments.map(item => item.routeCode)).size)}</span><span class="micro">${officialUpdates.length} ${t('officialFeed')}</span>`;
                el.selectionFacts.innerHTML = `
                    <div class="fact"><strong>${t('length')}</strong><div class="tiny">${formatKm(totalKm)}</div></div>
                    <div class="fact"><strong>${t('lotCount')}</strong><div class="tiny">${visibleLots.length}</div></div>
                    <div class="fact"><strong>${t('completion')}</strong><div class="tiny">${formatPercent(avgCompletion)}</div></div>
                    <div class="fact"><strong>${t('latestBulletin')}</strong><div class="tiny">${atlas.officialFeed.publishedOn}</div></div>`;
                el.selectionNote.textContent = officialUpdates.length ? pick(officialUpdates[0].detail) : t('noSelection');
                el.timeline.innerHTML = officialUpdates.length
                    ? officialUpdates.map(item => `<div class="timeline-item"><div class="timeline-year">${item.publishedOn}</div><div><div>${pick(item.title)}</div><div class="tiny" style="margin-top:4px">${pick(item.routeLabel)}</div></div></div>`).join('')
                    : `<div class="tiny">${t('pending')}</div>`;
                return;
            }

            el.selectionTitle.textContent = t('focusHintTitle');
            el.selectionSubtitle.textContent = t('focusHintCopy');
            el.selectionStatus.innerHTML = `<span class="dot" style="background:var(--accent-2)"></span>${t('focusMode')}`;
            el.selectionPills.innerHTML = '';
            el.selectionFacts.innerHTML = '';
            el.selectionNote.textContent = '';
            el.timeline.innerHTML = renderTimelineItems(atlas.network.milestones);
        }

        function renderTimeline(segment, lot) {
            el.timelineEyebrow.textContent = t('timelineEyebrow');
            el.timelineTitle.textContent = lot ? `${t('lotOverview')} ${lot.lotCode}` : t('timelineTitle');
            const items = lot?.milestones?.length ? lot.milestones : segment.milestones;
            el.timeline.innerHTML = renderTimelineItems(items);
        }

        function renderLegend() {
            const legendMarkup = Object.entries(statusMeta).map(([status, meta]) => `<button class="status-pill ${state.activeStatuses.has(status) ? 'active' : ''}" data-legend-status="${status}"><span class="dot" style="background:${meta.color}"></span>${labelForStatus(status)}</button>`).join('');
            const bindLegendClicks = container => {
                if (!container) return;
                container.querySelectorAll('[data-legend-status]').forEach(node => node.addEventListener('click', () => {
                    const status = node.dataset.legendStatus;
                    if (!status) return;
                    setActiveStatuses(state.activeStatuses.size === 1 && state.activeStatuses.has(status) ? ALL_STATUSES : [status]);
                }));
            };

            el.legend.innerHTML = legendMarkup;
            if (el.desktopLegend) el.desktopLegend.innerHTML = legendMarkup;
            if (el.tabletLegend) el.tabletLegend.innerHTML = legendMarkup;
            bindLegendClicks(el.legend);
            bindLegendClicks(el.desktopLegend);
            bindLegendClicks(el.tabletLegend);
        }

        function focusDefault() {
            updateViewport('default', () => map.fitBounds(bulgariaBounds, { padding: getViewportPadding('default'), animate: true, duration: 0.7, maxZoom: 7.2 }));
        }

        function focusRoute() {
            const segments = getVisibleSegments();
            if (!segments.length) return focusDefault();
            const bounds = segments.flatMap(segment => segment.shape.map(point => [point.lat, point.lon]));
            const key = `route:${state.activeRoute}:${state.regionFilter}:${state.playbackYear}:${Array.from(state.activeStatuses).sort().join(',')}`;
            updateViewport(key, () => map.fitBounds(bounds, { padding: getViewportPadding('route'), animate: true, duration: 0.75 }));
        }

        function focusLot() {
            const lot = getSelectedLot();
            if (!lot) return;
            const segment = atlas.segments.find(item => item.id === lot.segmentId);
            if (!segment) return;
            const bounds = getLotBounds(segment, lot);
            updateViewport(`lot:${lot.key}`, () => map.fitBounds(bounds, { padding: getViewportPadding('lot'), animate: true, duration: 0.55, maxZoom: 10.2 }));
        }

        function openLotPopup(lot, segment) {
            const marker = lotMarkers.get(lot.key);
            const popupHtml = renderLotPopup(lot, segment);
            closeActivePopup();

            if (marker) {
                marker.setPopupContent(popupHtml);
                marker.openPopup();
                activePopup = marker.getPopup();
                return;
            }

            activePopup = L.popup({ maxWidth: 320, autoPan: true, closeButton: true, closeOnClick: false, autoClose: false, keepInView: true, className: 'atlas-popup-card' })
                .setLatLng([lot.position.lat, lot.position.lon])
                .setContent(popupHtml);
            map.openPopup(activePopup);
        }

        function openSegmentPopup(segment) {
            closeActivePopup();
            const anchor = segment.shape[Math.max(0, Math.floor(segment.shape.length / 2))];
            activePopup = L.popup({ maxWidth: 320, autoPan: true, closeButton: true, closeOnClick: false, autoClose: false, keepInView: true, className: 'atlas-popup-card' })
                .setLatLng([anchor.lat, anchor.lon])
                .setContent(renderSegmentPopup(segment));
            map.openPopup(activePopup);
        }

        function renderMap() {
            if (!state.playbackPlaying) closeActivePopup();
            const currentYearForMap = new Date(atlas.generatedAtUtc).getFullYear();
            const isHistoricModeActive = state.playbackYear < currentYearForMap;
            contextLayer.clearLayers();
            segmentLayer.clearLayers();
            lotLineLayer.clearLayers();
            lotLayer.clearLayers();
            statusLayer.clearLayers();
            highlightLayer.clearLayers();
            lotMarkers.clear();

            const segments = getVisibleSegments();
            const selectedLot = getSelectedLot();

            atlas.segments.forEach(segment => {
                const latLngs = segment.shape.map(point => [point.lat, point.lon]);
                const segStart = segment.startYear || 9999;
                const isFutureGhost = isHistoricModeActive && (segStart > state.playbackYear);
                L.polyline(latLngs, {
                    color: isFutureGhost ? 'rgba(100,125,160,0.10)' : 'rgba(140, 165, 198, 0.22)',
                    weight: isFutureGhost ? 3 : 4,
                    opacity: isFutureGhost ? 0.18 : 0.38,
                    lineCap: 'round',
                    lineJoin: 'round',
                    interactive: false
                }).addTo(contextLayer);
            });

            segments.forEach(segment => {
                const color = statusMeta[segment.status].color;
                const isSelectedSegment = state.selectedSegmentId === segment.id;
                const shouldShowStatusChip = isSelectedSegment || state.selectedLotKey || state.activeRoute !== 'ALL' || map.getZoom() >= 8.35;
                const currentYear = new Date(atlas.generatedAtUtc).getFullYear();
                const isHistoricMode = state.playbackYear < currentYear;
                const fraction = isHistoricMode ? getSegmentPlaybackFraction(segment) : 1.0;
                if (fraction < 0.01) return;
                const baseLatLngs = segment.shape.map(point => [point.lat, point.lon]);
                const latLngs = (isHistoricMode && fraction < 0.999) ? truncatePolylineToFraction(baseLatLngs, fraction) : baseLatLngs;
                if (!latLngs || latLngs.length < 2) return;

                L.polyline(latLngs, {
                    color: `${color}33`,
                    weight: isSelectedSegment ? 13 : 10,
                    opacity: segment.status === 'Planned' ? 0.2 : 0.34,
                    lineCap: 'round',
                    lineJoin: 'round',
                    smoothFactor: 1.2,
                    interactive: false
                }).addTo(segmentLayer);

                L.polyline(latLngs, {
                    color: 'rgba(4, 10, 18, 0.92)',
                    weight: isSelectedSegment ? 10 : 8,
                    opacity: 0.92,
                    lineCap: 'round',
                    lineJoin: 'round',
                    smoothFactor: 1.2
                }).addTo(segmentLayer);

                const line = L.polyline(latLngs, {
                    color,
                    weight: isSelectedSegment ? 5.8 : 4.1,
                    opacity: 0.86,
                    dashArray: segment.status === 'Planned' ? '12 10' : segment.status === 'UnderConstruction' ? '14 8' : null,
                    lineCap: 'round',
                    lineJoin: 'round',
                    smoothFactor: 1.2
                }).addTo(segmentLayer);

                if (segment.status !== 'Planned') {
                    L.polyline(latLngs, {
                        color: 'rgba(255,255,255,0.46)',
                        weight: isSelectedSegment ? 2.2 : 1.4,
                        opacity: segment.status === 'Open' ? 0.5 : 0.3,
                        lineCap: 'round',
                        lineJoin: 'round',
                        smoothFactor: 1.2,
                        interactive: false
                    }).addTo(segmentLayer);
                }

                line.on('click', () => {
                    stopPlayback();
                    if (state.selectedSegmentId === segment.id && !state.selectedLotKey) {
                        state.selectedSegmentId = null;
                    }
                    else {
                        state.selectedSegmentId = segment.id;
                        state.selectedLotKey = null;
                    }
                    render();
                });

                if (segment.status !== 'Open' && shouldShowStatusChip) {
                    const anchor = segment.shape[Math.max(0, Math.floor(segment.shape.length / 2))];
                    const chip = L.marker([anchor.lat, anchor.lon], {
                        keyboard: false,
                        zIndexOffset: 1200,
                        icon: L.divIcon({ className: '', html: `<div class="segment-chip"><span class="dot" style="background:${statusMeta[segment.status].color}"></span><span>${labelForStatus(segment.status)}</span></div>` })
                    }).addTo(statusLayer);

                    chip.on('click', () => {
                        stopPlayback();
                        if (state.selectedSegmentId === segment.id && !state.selectedLotKey) {
                            state.selectedSegmentId = null;
                        }
                        else {
                            state.selectedSegmentId = segment.id;
                            state.selectedLotKey = null;
                        }
                        render();
                    });
                }

                segment.lots.forEach(lot => {
                    const isSelectedLot = state.selectedLotKey === lot.key;
                    const lotPath = buildLotPath(segment, lot);
                    const lotLine = L.polyline(lotPath, {
                        color: statusMeta[lot.status].color,
                        weight: isSelectedLot ? 6.1 : (lot.isDerived ? 3.8 : 4.8),
                        opacity: isSelectedLot ? 0.98 : (lot.isDerived ? 0.58 : 0.88),
                        dashArray: lot.status === 'Planned' ? '10 8' : lot.status === 'UnderConstruction' ? '14 10' : null,
                        lineCap: 'round',
                        lineJoin: 'round',
                        smoothFactor: 1.2
                    }).addTo(lotLineLayer);

                    lotLine.on('click', () => {
                        stopPlayback();
                        if (state.selectedLotKey === lot.key) {
                            state.selectedSegmentId = null;
                            state.selectedLotKey = null;
                        }
                        else {
                            state.selectedSegmentId = segment.id;
                            state.selectedLotKey = lot.key;
                        }
                        render();
                    });

                    const marker = L.marker([lot.position.lat, lot.position.lon], {
                        riseOnHover: true,
                        bubblingMouseEvents: false,
                        zIndexOffset: isSelectedLot ? 2000 : 0,
                        icon: L.divIcon({ className: '', html: `<div class="lot-marker ${isSelectedLot ? 'selected' : ''}" style="background:${statusMeta[lot.status].color}; box-shadow: 0 0 0 ${isSelectedLot ? '4px' : '2.5px'} rgba(3,8,18,.44), 0 10px 20px ${statusMeta[lot.status].color}4d"><span class="lot-marker-core" style="background:${lot.status === 'Planned' ? '#f3ecff' : lot.status === 'UnderConstruction' ? '#fff3de' : '#ffffff'}"></span></div>`, iconSize: [18, 18], iconAnchor: [9, 9] })
                    }).addTo(lotLayer);

                    marker.bindPopup(renderLotPopup(lot, segment), { maxWidth: 320, autoPan: true, closeButton: true, closeOnClick: false, autoClose: false, keepInView: true, className: 'atlas-popup-card' });
                    lotMarkers.set(lot.key, marker);

                    marker.on('click', () => {
                        stopPlayback();
                        if (state.selectedLotKey === lot.key) {
                            state.selectedSegmentId = null;
                            state.selectedLotKey = null;
                        }
                        else {
                            state.selectedSegmentId = segment.id;
                            state.selectedLotKey = lot.key;
                        }
                        render();
                    });
                });

                if (selectedLot && selectedLot.segmentId === segment.id) {
                    const radius = L.circle([selectedLot.position.lat, selectedLot.position.lon], {
                        radius: selectedLot.isDerived ? 9000 : 12000,
                        color: statusMeta[selectedLot.status].color,
                        weight: 1,
                        fillColor: statusMeta[selectedLot.status].color,
                        fillOpacity: 0.08
                    }).addTo(highlightLayer);
                }
            });

            if (selectedLot) {
                focusLot();
                const segment = atlas.segments.find(item => item.id === selectedLot.segmentId);
                if (segment) {
                    openLotPopup(selectedLot, segment);
                }
            }
            else if (state.selectedSegmentId) {
                const segment = getSelectedSegment();
                if (segment) {
                    updateViewport(`segment:${segment.id}`, () => map.fitBounds(segment.shape.map(point => [point.lat, point.lon]), { padding: getViewportPadding('segment'), animate: true, duration: 0.8 }));
                    openSegmentPopup(segment);
                }
            }
            else if (state.activeRoute !== 'ALL') {
                focusRoute();
            }
            else {
                focusDefault();
            }
        }

        setBasemap();
        syncDeviceProfile();
        focusDefault();
        map.on('popupclose', () => {
            activePopup = null;
            if (suppressPopupReset) {
                suppressPopupReset = false;
                return;
            }
        });
        renderNow();
        window.requestAnimationFrame(() => document.body.classList.add('is-ready'));
    </script>
</body>
</html>
""";
    }

    private static object BuildAtlasModel(HighwayRoute network)
    {
        var engine = new RouteEngine();
        var lengthsByStatus = network.GetLengthByStatus();
        var currentYear = DateTime.UtcNow.Year;
        var segments = network.Segments
            .OrderBy(segment => GetRouteSortOrder(segment.RouteCode))
            .ThenBy(segment => segment.SectionCode, StringComparer.OrdinalIgnoreCase)
            .Select((segment, index) =>
            {
                var lots = BuildLots(segment)
                    .Select(lot => new
                    {
                        key = $"{segment.SectionCode}:{lot.LotCode}",
                        lotCode = lot.LotCode,
                        title = new { bg = lot.Title.Bg, en = lot.Title.En },
                        status = lot.Status.ToString(),
                        startKm = Math.Round(lot.StartKm, 1),
                        endKm = Math.Round(lot.EndKm, 1),
                        completionPercent = Math.Round(lot.CompletionPercent, 1),
                        forecastOpenYear = lot.ForecastOpenYear,
                        budgetMillionEur = lot.BudgetMillionEur,
                        contractor = lot.Contractor,
                        isDerived = lot.IsDerived,
                        note = lot.Note is null ? null : new { bg = lot.Note.Bg, en = lot.Note.En },
                        milestones = BuildLotMilestones(segment, lot, currentYear),
                        position = ToPoint(InterpolatePoint(segment.Shape, segment.LengthKm == 0 ? 0 : ((lot.StartKm + lot.EndKm) / 2.0) / segment.LengthKm))
                    })
                    .ToArray();
                var sourceQuality = DeriveSourceQuality(lots.Select(lot => lot.isDerived));

                return new
                {
                    id = index + 1,
                    routeCode = segment.RouteCode,
                    routeName = new { bg = segment.EffectiveDisplayName.Bg, en = segment.EffectiveDisplayName.En },
                    sectionCode = segment.SectionCode,
                    sectionName = new { bg = segment.EffectiveSectionName.Bg, en = segment.EffectiveSectionName.En },
                    description = new { bg = segment.Description?.Bg ?? string.Empty, en = segment.Description?.En ?? string.Empty },
                    importance = new { bg = segment.StrategicImportance?.Bg ?? string.Empty, en = segment.StrategicImportance?.En ?? string.Empty },
                    status = segment.Status.ToString(),
                    lengthKm = Math.Round(segment.LengthKm, 1),
                    maxSpeedKph = segment.MaxSpeedKph,
                    startYear = segment.StartYear,
                    forecastOpenYear = segment.ForecastOpenYear,
                    completionPercent = Math.Round(segment.EffectiveCompletionPercent, 1),
                    budgetMillionEur = segment.BudgetMillionEur,
                    fundingProgram = segment.FundingProgram,
                    contractor = segment.Contractor,
                    sourceName = segment.SourceName,
                    sourceUrl = segment.SourceUrl,
                    officialSource = new
                    {
                        name = segment.OfficialSourceName,
                        url = segment.OfficialSourceUrl,
                        kind = segment.OfficialSourceKind,
                        verifiedOn = segment.OfficialSourceVerifiedOn
                    },
                    secondarySource = string.IsNullOrWhiteSpace(segment.SecondarySourceName)
                        ? null
                        : new
                        {
                            name = segment.SecondarySourceName,
                            url = segment.SecondarySourceUrl
                        },
                    sourceQuality,
                    evidenceGrade = DeriveEvidenceGrade(segment),
                    estimatedTravelMinutes = (int)Math.Round(engine.EstimateTravelTime([segment]).TotalMinutes),
                    openYear = segment.Status == SegmentStatus.Open
                        ? (segment.Milestones.OrderBy(m => m.Year).LastOrDefault(m => m.State == "success")?.Year ?? segment.StartYear)
                        : (int?)null,
                    openingYears = segment.Status == SegmentStatus.Open
                        ? segment.Milestones.Where(item => item.State == "success").Select(item => item.Year).Distinct().OrderBy(year => year).ToArray()
                        : Array.Empty<int>(),
                    milestones = segment.Milestones.OrderBy(item => item.Year).Select(item => new { year = item.Year, label = new { bg = item.Label.Bg, en = item.Label.En }, state = item.State }).ToArray(),
                    shape = segment.Shape.Select(ToPoint).ToArray(),
                    lots
                };
            })
            .ToArray();
        var routes = network.Segments
            .GroupBy(segment => segment.RouteCode, StringComparer.OrdinalIgnoreCase)
            .OrderBy(group => GetRouteSortOrder(group.Key))
            .Select(group =>
            {
                var routeSegments = segments.Where(item => string.Equals(item.routeCode, group.Key, StringComparison.OrdinalIgnoreCase)).ToArray();
                var totalKm = routeSegments.Sum(item => item.lengthKm);
                var openKm = routeSegments.Where(item => string.Equals(item.status, SegmentStatus.Open.ToString(), StringComparison.OrdinalIgnoreCase)).Sum(item => item.lengthKm);
                var constructionKm = routeSegments.Where(item => string.Equals(item.status, SegmentStatus.UnderConstruction.ToString(), StringComparison.OrdinalIgnoreCase)).Sum(item => item.lengthKm);
                var plannedKm = routeSegments.Where(item => string.Equals(item.status, SegmentStatus.Planned.ToString(), StringComparison.OrdinalIgnoreCase)).Sum(item => item.lengthKm);
                var startYear = group.Min(item => item.StartYear ?? currentYear);
                var first = group.First();
                var milestones = group
                    .SelectMany(item => item.Milestones)
                    .GroupBy(item => new { item.Year, item.Label.Bg, item.Label.En, item.State })
                    .Select(items => items.First())
                    .OrderBy(item => item.Year)
                    .Select(item => new { year = item.Year, label = new { bg = item.Label.Bg, en = item.Label.En }, state = item.State })
                    .ToArray();

                return new
                {
                    routeCode = group.Key,
                    title = new { bg = first.EffectiveDisplayName.Bg, en = first.EffectiveDisplayName.En },
                    sectionLabel = new { bg = $"{group.First().Start.Name} – {group.Last().End.Name}", en = $"{group.First().Start.Name} – {group.Last().End.Name}" },
                    highlight = BuildRouteHighlight(group.Key, first.StrategicImportance, currentYear, startYear),
                    totalKm = Math.Round(totalKm, 1),
                    openKm = Math.Round(openKm, 1),
                    constructionKm = Math.Round(constructionKm, 1),
                    plannedKm = Math.Round(plannedKm, 1),
                    completionPercent = totalKm == 0 ? 0 : Math.Round(openKm / totalKm * 100.0, 1),
                    storyYears = Math.Max(0, currentYear - startYear),
                    startYear,
                    missingKm = Math.Round(totalKm - openKm, 1),
                    lotCount = routeSegments.Sum(item => item.lots.Length),
                    milestones
                };
            })
            .ToArray();

        var totalLots = segments.Sum(segment => segment.lots.Length);
        var derivedLots = segments.Sum(segment => segment.lots.Count(lot => lot.isDerived));
        var explicitLots = totalLots - derivedLots;
        var officialSegmentCount = segments.Count(segment => string.Equals(segment.sourceQuality, "official", StringComparison.OrdinalIgnoreCase));
        var mixedSegmentCount = segments.Count(segment => string.Equals(segment.sourceQuality, "mixed", StringComparison.OrdinalIgnoreCase));
        var modeledSegmentCount = segments.Count(segment => string.Equals(segment.sourceQuality, "modeled", StringComparison.OrdinalIgnoreCase));
        var routeSpecificReferenceCount = network.Segments.Count(segment => string.Equals(segment.OfficialSourceKind, "route-specific", StringComparison.OrdinalIgnoreCase));
        var networkWideReferenceCount = network.Segments.Count(segment => string.Equals(segment.OfficialSourceKind, "network-wide", StringComparison.OrdinalIgnoreCase));
        var secondaryReferenceCount = network.Segments.Count(segment => segment.HasSecondarySource);
        var officialReferenceCount = network.Segments.Count(segment => segment.HasOfficialSource);

        return new
        {
            generatedAtUtc = DateTime.UtcNow,
            copyright = new
            {
                notice = new
                {
                    bg = "© 2026 ViLLZz · Bulgarian Motorway Atlas",
                    en = "© 2026 ViLLZz · Bulgarian Motorway Atlas"
                },
                usage = new
                {
                    bg = "Публичен преглед за тестване и анализ. Данните са компилирани от официални публични източници и вторични справки; препубликуване с атрибуция към проекта.",
                    en = "Public preview for testing and analysis. Data is compiled from official public sources and secondary references; republishing requires attribution to the project."
                }
            },
            officialFeed = new
            {
                label = "API Bulgaria bulletin snapshot",
                publishedOn = "2026-03-07",
                sourceName = "api.bg public bulletins and motorway notices"
            },
            geo = new
            {
                center = new { lat = 42.7339, lon = 25.4858 },
                bounds = new { south = 41.2, west = 22.3, north = 44.3, east = 28.8 }
            },
            network = new
            {
                title = new { bg = "Атлас на българските автомагистрали", en = "Bulgarian Motorway Atlas" },
                subtitle = new { bg = "Актуален преглед на 9 маршрута, 20 участъка и над 50 лота – от първите открития до днешните строежи.", en = "Live snapshot of 9 routes, 20 sections, 50+ lots – from first openings to today's active construction." },
                message = new { bg = "Мрежата обхваща 1 506 km; 991 km са отворени, 250 km в строеж, 265 km планирани.", en = "The network spans 1,506 km: 991 km open, 250 km under construction, 265 km planned." },
                milestones = routes
                    .SelectMany(route => route.milestones)
                    .GroupBy(item => new { item.year, item.state, item.label.bg, item.label.en })
                    .Select(items => items.First())
                    .OrderBy(item => item.year)
                    .TakeLast(8)
                    .ToArray()
            },
            summary = new
            {
                routeCount = routes.Length,
                segmentCount = network.Segments.Count,
                explicitLotCount = explicitLots,
                derivedLotCount = derivedLots,
                officialSegmentCount,
                mixedSegmentCount,
                modeledSegmentCount,
                officialReferenceCount,
                routeSpecificReferenceCount,
                networkWideReferenceCount,
                secondaryReferenceCount,
                officialReferencePercent = network.Segments.Count == 0 ? 0 : Math.Round(officialReferenceCount / (double)network.Segments.Count * 100.0, 1),
                sourceCoveragePercent = totalLots == 0 ? 0 : Math.Round(explicitLots / (double)totalLots * 100.0, 1),
                totalKm = Math.Round(network.TotalKm, 1),
                openKm = Math.Round(network.OpenKm, 1),
                constructionKm = Math.Round(lengthsByStatus.GetValueOrDefault(SegmentStatus.UnderConstruction), 1),
                plannedKm = Math.Round(lengthsByStatus.GetValueOrDefault(SegmentStatus.Planned), 1),
                completionPercent = Math.Round(network.CompletionPercent, 1)
            },
            officialUpdates = BuildOfficialUpdates(),
            routes,
            segments
        };
    }

    private static string DeriveSourceQuality(IEnumerable<bool> derivedFlags)
    {
        var flags = derivedFlags.ToArray();
        if (flags.Length == 0)
        {
            return "modeled";
        }

        var derivedCount = flags.Count(flag => flag);
        if (derivedCount == 0)
        {
            return "official";
        }

        return derivedCount == flags.Length ? "modeled" : "mixed";
    }

    private static string DeriveEvidenceGrade(RouteSegment segment)
    {
        if (!segment.HasOfficialSource && !segment.HasSecondarySource)
        {
            return "unattributed";
        }

        if (!segment.HasOfficialSource)
        {
            return "secondary-only";
        }

        if (string.Equals(segment.OfficialSourceKind, "route-specific", StringComparison.OrdinalIgnoreCase))
        {
            return segment.HasSecondarySource ? "official-route-plus-secondary" : "official-route";
        }

        return segment.HasSecondarySource ? "official-network-plus-secondary" : "official-network";
    }

    private static object ToPoint(RoutePoint point) => new
    {
        lat = point.Latitude,
        lon = point.Longitude,
        name = point.Name,
        cumulativeKm = point.CumulativeKm
    };

    private static RoutePoint InterpolatePoint(IReadOnlyList<RoutePoint> shape, double ratio)
    {
        if (shape.Count == 0)
        {
            return new RoutePoint(42.7339, 25.4858, "Bulgaria");
        }

        if (shape.Count == 1)
        {
            return shape[0];
        }

        // Use cumulativeKm (Haversine-based) for consistent interpolation with JS lot-path clipping
        var targetKm = Math.Clamp(ratio, 0, 1) * shape[shape.Count - 1].CumulativeKm;

        for (var i = 1; i < shape.Count; i++)
        {
            if (shape[i].CumulativeKm >= targetKm)
            {
                var lower = shape[i - 1];
                var upper = shape[i];
                var span = upper.CumulativeKm - lower.CumulativeKm;
                var factor = span > 0 ? (targetKm - lower.CumulativeKm) / span : 0.0;

                return new RoutePoint(
                    lower.Latitude + ((upper.Latitude - lower.Latitude) * factor),
                    lower.Longitude + ((upper.Longitude - lower.Longitude) * factor),
                    upper.Name,
                    lower.CumulativeKm + ((upper.CumulativeKm - lower.CumulativeKm) * factor));
            }
        }

        return shape[shape.Count - 1];
    }

    private static IReadOnlyList<LotDescriptor> BuildLots(RouteSegment segment) => (segment.SectionCode ?? string.Empty).ToUpperInvariant() switch
    {
        "A2-01" =>
        [
            Lot("Фаза I", "София – Ябланица", "Sofia – Yablanitsa", SegmentStatus.Open, 0, 62, 100, 1999, 342, noteBg: "Историческият първи завършен етап на АМ „Хемус“ в експлоатация от 1999 г.", noteEn: "Historic first completed Hemus phase, open since 1999."),
            Lot("Фаза II", "Ябланица – Боаза", "Yablanitsa – Boaza", SegmentStatus.Open, 62, 71.3, 100, 2019, 78, noteBg: "Лот 0 по официалната терминология на АПИ; открит през 2019 г.", noteEn: "Lot 0 in the official API terminology; opened in 2019."),
            Lot("Фаза III", "Боаза – Угърчин / Дерманци", "Boaza – Ugarchin / Dermantsi", SegmentStatus.Open, 71.3, 103, 100, 2025, 300, noteBg: "По официалните страници на АПИ участък 1 е Боаза–Дерманци; временният край при Угърчин е въведен през 2025 г.", noteEn: "On the official API pages section 1 is Boaza–Dermantsi; the temporary Ugarchin end opened in 2025.")
        ],
        "A2-02" =>
            BuildScaledLots(segment,
            [
                new OfficialLotTemplate("Участък 2", "Край на п.в. „Дерманци“ – п.в. „Каленик“", "Dermantsi interchange end – Kalenik interchange", SegmentStatus.UnderConstruction, 19.2, 23, null, null, "Автомагистрали ЕАД", "Официален участък 2 на АПИ: км 103+060 до км 122+260, срок 48 месеца след Протокол обр. 2а.", "Official API section 2: km 103+060 to km 122+260, execution term 48 months after Protocol 2a."),
                new OfficialLotTemplate("Участък 3", "П.в. „Каленик“ – п.в. „Плевен“", "Kalenik interchange – Pleven interchange", SegmentStatus.UnderConstruction, 17.08, 53, null, null, "Автомагистрали ЕАД", "Официален участък 3 на АПИ: км 122+260 до км 139+340; през март 2024 г. АПИ отчита 53% изпълнение.", "Official API section 3: km 122+260 to km 139+340; in March 2024 API reported 53% completion."),
                new OfficialLotTemplate("Участък 4", "П.в. „Плевен“ – п.в. „Летница“", "Pleven interchange – Letnitsa interchange", SegmentStatus.UnderConstruction, 26.804, 0, null, null, "Автомагистрали ЕАД", "Официален участък 4 на АПИ: км 139+340 до км 166+144.09; договорният срок е 180 дни за проектиране и 36 месеца за СМР след Протокол обр. 2а.", "Official API section 4: km 139+340 to km 166+144.09; contract term is 180 days for design and 36 months for works after Protocol 2a.")
            ]),
        "A2-03" =>
            BuildScaledLots(segment,
            [
                new OfficialLotTemplate("Участък 5", "П.в. „Летница“ – път III-303", "Letnitsa interchange – road III-303", SegmentStatus.Planned, 23.2, 0, null, null, "Автомагистрали ЕАД", "Официален участък 5 на АПИ: км 166+144.09 до км 189+344; 180 дни за проектиране и 36 месеца за СМР след Протокол обр. 2а.", "Official API section 5: km 166+144.09 to km 189+344; 180 days for design and 36 months for works after Protocol 2a."),
                new OfficialLotTemplate("Участък 6", "Път III-303 – път I-5 Русе – Велико Търново", "Road III-303 – road I-5 Ruse – Veliko Tarnovo", SegmentStatus.Planned, 32.656, 0, null, null, "Автомагистрали ЕАД", "Официален участък 6 на АПИ: км 189+344 до км 222+000; 180 дни за проектиране и 42 месеца за СМР след Протокол обр. 2а.", "Official API section 6: km 189+344 to km 222+000; 180 days for design and 42 months for works after Protocol 2a."),
                new OfficialLotTemplate("Участък 7", "Път I-5 – п.в. „Ковачевско кале“", "Road I-5 – Kovachevsko Kale interchange", SegmentStatus.Planned, 43.6, 0, null, null, "Автомагистрали ЕАД", "Официален участък 7 на АПИ: км 222+000 до км 265+600, срок 36 месеца след Протокол обр. 2а.", "Official API section 7: km 222+000 to km 265+600, execution term 36 months after Protocol 2a.")
            ]),
        "A2-04" =>
            BuildScaledLots(segment,
            [
                new OfficialLotTemplate("Участък 8", "П.в. „Ковачевско кале“ – п.в. „Лозница“", "Kovachevsko Kale interchange – Loznitsa interchange", SegmentStatus.Planned, 33.4, 0, null, null, "Автомагистрали ЕАД", "Официален участък 8 на АПИ: км 265+600 до км 299+000, срок 36 месеца след Протокол обр. 2а.", "Official API section 8: km 265+600 to km 299+000, execution term 36 months after Protocol 2a."),
                new OfficialLotTemplate("Участък 9", "П.в. „Лозница“ – п.в. „Буховци-юг“", "Loznitsa interchange – Buhovtsi-south interchange", SegmentStatus.Planned, 11.94, 0, null, null, "Автомагистрали ЕАД", "Официален участък 9 на АПИ: км 299+000 до км 310+940; срокът за изграждане е 30 месеца след разрешение за строеж и Протокол обр. 2а.", "Official API section 9: km 299+000 to km 310+940; the execution term is 30 months after the building permit and Protocol 2a.")
            ]),
        "A2-05" =>
        [
            Lot("Фаза I", "Буховци – Белокопитово", "Buhovtsi – Belokopitovo", SegmentStatus.Open, 0, 28, 100, 2022, 175, noteBg: "Открит през октомври 2022 г.; затваря последния незавършен дял към Шумен.", noteEn: "Opened in October 2022; closed the last unfinished gap toward Shumen."),
            Lot("Фаза II", "Шумен / Белокопитово – Провадия", "Shumen / Belokopitovo – Provadiya", SegmentStatus.Open, 28, 67, 100, 2015, 190, noteBg: "Включва възела „Белокопитово“ и следва модернизираната шуменска ос към Варна.", noteEn: "Includes the Belokopitovo interchange and follows the modernized Shumen axis toward Varna."),
            Lot("Фаза III", "Провадия – Варна", "Provadiya – Varna", SegmentStatus.Open, 67, 107, 100, 1974, 210, noteBg: "Историческата източна магистрална връзка Шумен – Варна, по-късно интегрирана в A2.", noteEn: "The historic eastern Shumen–Varna motorway link later integrated into A2.")
        ],
        "MB-01" =>
            BuildScaledLots(segment,
            [
                new OfficialLotTemplate("Лот 2", "Мездра – Лютидол", "Mezdra – Lyutidol", SegmentStatus.UnderConstruction, 13.433, segment.EffectiveCompletionPercent, null, 136.5, "Консорциум ДЗЗД „МБ ЛОТ 2-2019“", "Официален лот 2 на АПИ: км 161+367 до км 174+800, договорен срок 1096 дни; по договор край на 2023 г., но без потвърдена актуална дата за въвеждане в експлоатация.", "Official API lot 2: km 161+367 to km 174+800, contract term 1096 days; contract finish was end-2023, but there is no confirmed current opening date."),
                new OfficialLotTemplate("Лот 1", "Лютидол – Ботевград", "Lyutidol – Botevgrad", SegmentStatus.UnderConstruction, 19.322, segment.EffectiveCompletionPercent, null, 81.6, "Обединение „ГЕОПЪТ ЛОТ 1“", "Официален лот 1 на АПИ: км 174+800 до км 194+122, договорен срок 1096 дни; без потвърдена актуална дата за въвеждане в експлоатация.", "Official API lot 1: km 174+800 to km 194+122, contract term 1096 days; there is no confirmed current opening date.")
            ]),
        _ => BuildGeneratedLots(segment)
    };

    private static IReadOnlyList<LotDescriptor> BuildGeneratedLots(RouteSegment segment)
    {
        var sectionName = segment.EffectiveSectionName;
        var count = segment.LengthKm switch
        {
            >= 140 => 3,
            >= 70 => 2,
            _ => 1
        };

        var results = new List<LotDescriptor>(count);
        var piece = segment.LengthKm / count;

        for (var index = 0; index < count; index++)
        {
            var startKm = Math.Round(piece * index, 1);
            var endKm = Math.Round(index == count - 1 ? segment.LengthKm : piece * (index + 1), 1);
            var startPoint = InterpolatePoint(segment.Shape, segment.LengthKm == 0 ? 0 : startKm / segment.LengthKm);
            var endPoint = InterpolatePoint(segment.Shape, segment.LengthKm == 0 ? 1 : endKm / segment.LengthKm);
            var band = DescribeLotBand(index, count);
            var sameAnchor = string.Equals(startPoint.Name, endPoint.Name, StringComparison.OrdinalIgnoreCase);
            results.Add(new LotDescriptor(
                $"Span {index + 1}",
                new LocalizedText($"{sectionName.Bg} · {band.Bg}", $"{sectionName.En} · {band.En}"),
                segment.Status,
                startKm,
                endKm,
                segment.EffectiveCompletionPercent,
                null,
                segment.BudgetMillionEur,
                segment.Contractor,
                new LocalizedText(
                    sameAnchor
                        ? $"{band.Bg} около {startPoint.Name}. Това е моделиран дял по оста {sectionName.Bg}, а не официално номериран лот ({startKm:0.0}–{endKm:0.0} km)."
                        : $"{band.Bg} между {startPoint.Name} и {endPoint.Name}. Това е моделиран дял по оста {sectionName.Bg}, а не официално номериран лот ({startKm:0.0}–{endKm:0.0} km).",
                    sameAnchor
                        ? $"{band.En} around {startPoint.Name}. This is a modeled span along {sectionName.En}, not an official numbered lot ({startKm:0.0}–{endKm:0.0} km)."
                        : $"{band.En} between {startPoint.Name} and {endPoint.Name}. This is a modeled span along {sectionName.En}, not an official numbered lot ({startKm:0.0}–{endKm:0.0} km)."),
                true));
        }

        return results;
    }

    private static IReadOnlyList<LotDescriptor> BuildScaledLots(RouteSegment segment, params OfficialLotTemplate[] templates)
    {
        if (templates.Length == 0)
        {
            return BuildGeneratedLots(segment);
        }

        var totalTemplateKm = templates.Sum(item => item.LengthKm);
        var scale = totalTemplateKm <= 0 ? 1.0 : segment.LengthKm / totalTemplateKm;
        var cursor = 0.0;
        var lots = new List<LotDescriptor>(templates.Length);

        foreach (var template in templates)
        {
            var scaledLength = Math.Round(template.LengthKm * scale, 1);
            var startKm = Math.Round(cursor, 1);
            var endKm = Math.Round(template == templates[^1] ? segment.LengthKm : cursor + scaledLength, 1);
            cursor = endKm;

            lots.Add(new LotDescriptor(
                template.Code,
                new LocalizedText(template.TitleBg, template.TitleEn),
                template.Status,
                startKm,
                endKm,
                template.CompletionPercent,
                template.ForecastOpenYear,
                template.BudgetMillionEur,
                template.Contractor,
                new LocalizedText(template.NoteBg, template.NoteEn),
                false));
        }

        return lots;
    }

    private static LotDescriptor Lot(
        string code,
        string bg,
        string en,
        SegmentStatus status,
        double startKm,
        double endKm,
        double completionPercent,
        int? forecastOpenYear = null,
        double? budgetMillionEur = null,
        string? contractor = null,
        string? noteBg = null,
        string? noteEn = null)
        => new(
            code,
            new LocalizedText(bg, en),
            status,
            startKm,
            endKm,
            completionPercent,
            forecastOpenYear,
            budgetMillionEur,
            contractor,
            noteBg is null || noteEn is null ? null : new LocalizedText(noteBg, noteEn),
            false);

    private static int GetRouteSortOrder(string routeCode) => routeCode.ToUpperInvariant() switch
    {
        "A1" => 1,
        "A2" => 2,
        "A3" => 3,
        "A4" => 4,
        "A5" => 5,
        "A6" => 6,
        "E79" => 7,
        "RVT" => 8,
        "MB" => 9,
        _ => 100
    };

    private static object[] BuildOfficialUpdates() =>
    [
        new
        {
            routeCode = "A3",
            status = SegmentStatus.UnderConstruction.ToString(),
            routeLabel = new { bg = "АМ „Струма“", en = "A3 Struma" },
            publishedOn = "2026-03-06",
            title = new
            {
                bg = "Ограничение в тунел „Железница“ за тестове и профилактика",
                en = "Restrictions in Zheleznitsa tunnel for testing and maintenance"
            },
            detail = new
            {
                bg = "АПИ публикува известие за временна организация на движението на 11 и 12 март в тунел „Железница“ на АМ „Струма“.",
                en = "API Bulgaria published a notice for temporary traffic organization on March 11–12 in Zheleznitsa tunnel on A3 Struma."
            },
            sourceUrl = "https://www.api.bg/bg/novini/na-11-i-12-mart-shche-se-ogranichi-dvizhenieto-v-tunel-zheleznitsa-na-am-struma-za-testvane-i-profilaktika-na-sistemata-za-upravlenie-na-suoruzhenieto.html"
        },
        new
        {
            routeCode = "A3",
            status = SegmentStatus.UnderConstruction.ToString(),
            routeLabel = new { bg = "АМ „Струма“", en = "A3 Struma" },
            publishedOn = "2026-03-06",
            title = new
            {
                bg = "Ремонт на фуга при 58-и км в посока София",
                en = "Joint repair at km 58 towards Sofia"
            },
            detail = new
            {
                bg = "Официалното съобщение на АПИ описва ограничение на движението за ремонтни дейности в посока София при 58-и км на АМ „Струма“.",
                en = "An official API Bulgaria notice describes lane restrictions for repair works towards Sofia at km 58 of A3 Struma."
            },
            sourceUrl = "https://www.api.bg/bg/novini/ot-9-mart-za-remont-na-fuga-shche-se-ogranichava-dvizhenieto-v-posoka-sofiya-po-aktivnata-lenta-pri-58-i-km-na-am-struma-v-oblast-kyustendil.html"
        },
        new
        {
            routeCode = "A1",
            status = SegmentStatus.Open.ToString(),
            routeLabel = new { bg = "АМ „Тракия“", en = "A1 Trakia" },
            publishedOn = "2026-03-04",
            title = new
            {
                bg = "Ремонт в платното за Бургас на 6-и км от АМ „Тракия“",
                en = "Repair works in the Burgas carriageway at km 6 on A1 Trakia"
            },
            detail = new
            {
                bg = "Последното официално известие на АПИ за АМ „Тракия“ въвежда временна организация за ремонт в ранния софийски участък към Бургас.",
                en = "The latest official API Bulgaria notice for A1 Trakia introduces temporary traffic organization for repair works in the early Sofia-to-Burgas reach."
            },
            sourceUrl = "https://www.api.bg/bg/novini/ot-utre-5-mart-zapochva-remont-v-platnoto-za-burgas-na-6-km-ot-am-trakiya.html"
        },
        new
        {
            routeCode = "A3",
            status = SegmentStatus.UnderConstruction.ToString(),
            routeLabel = new { bg = "АМ „Струма“", en = "A3 Struma" },
            publishedOn = "2026-03-06",
            title = new
            {
                bg = "Обрушване на скатове в област Кюстендил",
                en = "Slope scaling works in Kyustendil region"
            },
            detail = new
            {
                bg = "АПИ предупреждава за временни промени в организацията на движението по АМ „Струма“ заради обезопасяване на скатове.",
                en = "API Bulgaria warns of temporary traffic changes on A3 Struma due to slope safety works."
            },
            sourceUrl = "https://www.api.bg/bg/novini/prez-sledvashchata-sedmitsa-za-obrushvane-na-skatove-vremenno-shche-se-promenya-organizatsiyata-na-dvizhenie-po-am-struma-v-oblast-kyustendil.html"
        }
    ];

    private static object BuildRouteHighlight(string routeCode, LocalizedText? defaultText, int currentYear, int startYear)
    {
        if (string.Equals(routeCode, "A2", StringComparison.OrdinalIgnoreCase))
        {
            var years = Math.Max(0, currentYear - startYear);
            return new
            {
                bg = $"{years} години след началото „Хемус“ остава символ на това колко важно е лотовете да бъдат завършвани навреме.",
                en = $"{years} years after launch, Hemus remains a symbol of why motorway lots must be delivered on time."
            };
        }

        return new
        {
            bg = defaultText?.Bg ?? "Стратегически коридор за национална свързаност.",
            en = defaultText?.En ?? "Strategic corridor for national connectivity."
        };
    }

    private static object[] BuildLotMilestones(RouteSegment segment, LotDescriptor lot, int currentYear)
    {
        var items = new List<(int Year, string Bg, string En, string State)>();

        if (segment.StartYear is int startYear)
        {
            items.Add((startYear, "Начало на участъка", "Section baseline", "baseline"));
        }

        if (lot.IsDerived)
        {
            items.Add((currentYear, "Моделиран дял", "Modeled span", "info"));

            return items
                .OrderBy(item => item.Year)
                .Select(item => new
                {
                    year = item.Year,
                    label = new { bg = item.Bg, en = item.En },
                    state = item.State
                })
                .ToArray();
        }

        if (lot.Status == SegmentStatus.Open)
        {
            items.Add((lot.ForecastOpenYear ?? currentYear, "Лотът е в експлоатация", "Lot operational", "success"));
        }
        else if (lot.Status == SegmentStatus.UnderConstruction)
        {
            items.Add((Math.Max(segment.StartYear ?? currentYear, currentYear - 2), $"Активно строителство · {lot.CompletionPercent:0}%", $"Active construction · {lot.CompletionPercent:0}%", "warning"));
            if (lot.ForecastOpenYear is int forecastOpenYear)
            {
                items.Add((forecastOpenYear, "Очаквано пускане", "Expected opening", "forecast"));
            }
        }
        else if (lot.Status == SegmentStatus.Planned)
        {
            items.Add((currentYear, "Планиране и подготовка", "Planning and preparation", "planning"));
            if (lot.ForecastOpenYear is int forecastOpenYear)
            {
                items.Add((forecastOpenYear, "Целеви хоризонт", "Target horizon", "forecast"));
            }
        }

        return items
            .OrderBy(item => item.Year)
            .Select(item => new
            {
                year = item.Year,
                label = new { bg = item.Bg, en = item.En },
                state = item.State
            })
            .ToArray();
    }

    private static LocalizedText DescribeLotBand(int index, int count) => (index, count) switch
    {
        (0, 1) => new LocalizedText("основен лот", "main lot"),
        (0, 2) => new LocalizedText("западен лот", "western lot"),
        (1, 2) => new LocalizedText("източен лот", "eastern lot"),
        (0, 3) => new LocalizedText("западен дял", "western reach"),
        (1, 3) => new LocalizedText("централен дял", "central reach"),
        (2, 3) => new LocalizedText("източен дял", "eastern reach"),
        _ => new LocalizedText($"лот {index + 1}", $"lot {index + 1}")
    };

    private sealed record LotDescriptor(
        string LotCode,
        LocalizedText Title,
        SegmentStatus Status,
        double StartKm,
        double EndKm,
        double CompletionPercent,
        int? ForecastOpenYear,
        double? BudgetMillionEur,
        string? Contractor,
        LocalizedText? Note,
        bool IsDerived);

    private sealed record OfficialLotTemplate(
        string Code,
        string TitleBg,
        string TitleEn,
        SegmentStatus Status,
        double LengthKm,
        double CompletionPercent,
        int? ForecastOpenYear,
        double? BudgetMillionEur,
        string? Contractor,
        string NoteBg,
        string NoteEn);
}