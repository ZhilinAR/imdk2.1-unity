# Immersal SDK Core
This is the core package for the Immersal Unity SDK.

## Compatibility
- Immersal SDK **2.1**
- Unity **2022.3 LTS**
- AR Foundation **5.1+**
- OpenXR **1.8.2+**
- XREAL XR Plugin **3.x**
- XREAL **Air 2 Ultra**

> Note: Earlier versions of Unity and AR Foundation may still work with minimal script changes.

---

## Installation

1. Remove any existing Immersal SDK package via **Package Manager**.
2. Add this fork by selecting **Add package from git URL...** and using:
   `https://github.com/immersal/imdk2.1-unity.git`
3. Install the **XREAL XR Plugin**:
   - Download the `com.xreal.xr` tarball from the XREAL distribution.
   - In **Package Manager**, choose **Add package from tarball...** and select the file.
4. Open **Edit → Project Settings → XR Plug-in Management**.
5. Enable **XREAL** for your target platform and disable **ARCore**.
6. (If needed) add the scripting define symbol **`IMMERSAL_XREAL`** under
   **Edit → Project Settings → Player → Other Settings → Scripting Define Symbols**.

### Dependency note
This package depends on the **XREAL XR Plugin** (`com.xreal.xr`) distributed as a tarball by XREAL.

## Android build requirements
- Minimum API level **26**
- Scripting backend **IL2CPP**
- Target architecture **ARM64**
- Graphics API **GLES3**
- Camera permission in the Android manifest

## Minimal scene setup
A basic scene should include:
- **XR Origin**
- **AR Session**
- **ImmersalSDK** component
- **Localizer** component
- **ARF bridge** component

---

## Samples
For a sample scene demonstrating localization on XREAL glasses, open:

Samples~/Core/Scenes/CustomLocalizationSample.unity

---

## Basic usage
1. Add the **`XrealSupport`** component (from `Immersal.XR`) to a scene GameObject.
2. Initialize/configure the platform at runtime; RGB camera frames and pose will be provided through the Immersal platform interfaces.
3. If the XREAL XR Plugin is missing or `IMMERSAL_XREAL` is not defined, a safe stub implementation is used.

---

## License
© 2024 Immersal – Part of Hexagon. All rights reserved.

The Immersal SDK cannot be copied, distributed, or made available to third-parties for commercial purposes without written permission of Immersal Ltd.  
For licensing requests, please contact **sales@immersal.com**.
