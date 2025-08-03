# Immersal SDK Core
This is the core package for the Immersal Unity SDK.

## Compatibility
- Immersal SDK **2.1**
- Unity **2022.3 LTS**
- AR Foundation **5.1+**
- OpenXR **1.8.2+**
- XREAL **Air 2 Ultra** (via AR Foundation)

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
6. (Optional) Add scripting define symbol **`IMMERSAL_XREAL`** for the target platform under
**Edit → Project Settings → Player → Other Settings → Scripting Define Symbols**.

---

## Samples
For a sample scene demonstrating localization on XREAL glasses, open:

Samples~/Core/Scenes/CustomLocalizationSample.unity

---

## Basic usage
1. Add the **`ARFBridge`** component (from `Immersal.XR`) to a scene GameObject.
2. The bridge uses AR Foundation to forward CPU camera images, intrinsics, and pose to `ImmersalSDK.Session`.
3. When `IMMERSAL_XREAL` is not defined, the bridge compiles to a safe stub so the project builds without the XREAL plugin.

---

## License
© 2024 Immersal – Part of Hexagon. All rights reserved.

The Immersal SDK cannot be copied, distributed, or made available to third-parties for commercial purposes without written permission of Immersal Ltd.  
For licensing requests, please contact **sales@immersal.com**.
