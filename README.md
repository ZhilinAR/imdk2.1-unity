# Immersal SDK Core
This is the core package for the Immersal Unity SDK.

## Compatibility
- Immersal SDK **2.1**
- Unity **2022.3 LTS**
- AR Foundation **5.1+**
- OpenXR **1.8.2+**
- XREAL NRSDK **1.7.0+**
- XREAL **Air 2 Ultra** (tested via NRSDK)

> Note: Earlier versions of Unity and AR Foundation may still work with minimal script changes.

---

## Installation

To install the latest available version:

1. Copy the URL to this repository:
https://github.com/immersal/imdk-unity.git
2. Open **Unity → Window → Package Manager**.
3. Click the **+** button (Add).
4. Choose **“Add package from git URL…”**.
5. Paste the URL above and click **Add**.
6. Import the **XREAL NRSDK** Unity package (either from Package Manager or the official XREAL distribution).
7. Go to **Edit → Project Settings → XR Plug-in Management** and enable **XrealSupport** (for the target platform).
8. (If needed) Add scripting define symbol **`IMMERSAL_XREAL`** for the target platform under  
**Edit → Project Settings → Player → Other Settings → Scripting Define Symbols**.

### Dependency note
This package depends on the official **XREAL NRSDK**. Unity’s Package Manager can fetch `com.xreal.nrsdk` if your project has access to the XREAL registry.  
If the registry is not configured, install the NRSDK package manually from the official XREAL distribution.

---

## Samples
For a sample scene demonstrating localization on XREAL glasses, open:

Samples~/Core/Scenes/CustomLocalizationSample.unity

---

## Basic usage
1. Add the **`XrealSupport`** component (from `Immersal.XR`) to a scene GameObject.
2. Initialize/configure the platform at runtime; RGB camera frames and pose will be provided through the Immersal platform interfaces when NRSDK is present.
3. On platforms without NRSDK or when `IMMERSAL_XREAL` is not defined, a safe stub implementation is used.

---

## License
© 2024 Immersal – Part of Hexagon. All rights reserved.

The Immersal SDK cannot be copied, distributed, or made available to third-parties for commercial purposes without written permission of Immersal Ltd.  
For licensing requests, please contact **sales@immersal.com**.
