# GIBS.Module.TideCharts

**Tide Charts Module for Oqtane (Blazor)**

## Overview

GIBS.Module.TideCharts is an Oqtane/Blazor module that displays NOAA tide predictions for selected stations. It provides interactive charts, maps, and a grid view for visualizing tide data, making it ideal for marine, coastal, and recreational applications.

## Features

- **Station Selection:** Choose from a list of NOAA tide stations.
- **Tide Predictions:** View high/low tide predictions for a configurable number of days.
- **Interactive Charts:** Line and bar charts powered by Chart.js.
- **Map Integration:** Station location displayed using Leaflet.js.
- **Grid View:** Tabular display of tide events.
- **Oqtane Integration:** Supports module permissions, settings, and multi-tenancy.
- **Localization:** Ready for multi-language support via resource files.
- **Data Source:** NOAA Tides & Currents API.

## Getting Started

### Prerequisites

- Oqtane Framework 6.1.2+ (Blazor, .NET 9)
- Access to NOAA Tides & Currents API (public)

### Installation

1. **Build and Deploy:**
   - Add the module project to your Oqtane solution.
   - Build the solution.
   - Deploy the output to your Oqtane site's `Modules` directory.

2. **Install the Module:**
   - Log in as Host/Admin.
   - Go to the Admin > Modules page.
   - Click "Install Module" and select `GIBS.Module.TideCharts`.

3. **Add to a Page:**
   - Edit a page and add the "Tide Charts" module to a pane.

### Configuration

- **Settings:**
  - Number of Tide Days (default: 7)
  - Map Zoom Level (default: 13)
  - Show Line Chart (default: true)
  - Show Bar Chart (default: false)
  - Show Grid (default: true)
  - Module Title

- **Permissions:**
  - Control who can view, edit, or manage tide stations.

## Usage

1. Select a station from the right panel.
2. View tide predictions, charts, and map for the selected station.
3. Admins can add, edit, or delete stations.

## Development

- **Technologies:** Blazor, C#, Oqtane, Chart.js, Leaflet.js
- **Key Files:**
  - `Index.razor` – Main UI and logic
  - `TideChartsManager.cs` – Server-side logic and data access
  - `Module.js` – Client-side chart/map initialization

## Data Model
