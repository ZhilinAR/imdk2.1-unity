# Immersal SDK Core
Это ядро Unity-SDK для Immersal.

## Совместимость
- Immersal SDK **2.1**
- Unity **2022.3 LTS**
- AR Foundation **5.1+**
- OpenXR **1.8.2+**
- **XREAL XR Plugin 3.x**
- Устройства **XREAL Air 2 Ultra** / Beam Pro

> Ранние версии Unity и AR Foundation могут работать при минимальных правках скриптов.

---

## Установка

1. **Удалите** прежний пакет Immersal SDK через **Window → Package Manager** (если он уже добавлен как `com.immersal.core`).
2. Добавьте этот форк как Git-пакет:  
   **Window → Package Manager → + → Add package from git URL…**  
https://github.com/ZhilinAR/imdk2.1-unity.git

3. Установите **XREAL XR Plugin** (`com.xreal.xr`):
- Скачайте tar-архив плагина из дистрибутива XREAL (`com.xreal.xr.tar.gz`).
- В **Package Manager** выберите **Add package from tarball…** и укажите файл.
4. Откройте **Edit → Project Settings → XR Plug-in Management**.
- Для **Android** включите **XREAL**.
- **Отключите ARCore** (на Beam Pro ARCore не поддерживается).
5. (Опционально) добавьте символ компиляции **`IMMERSAL_XREAL`**:  
**Edit → Project Settings → Player → Other Settings → Scripting Define Symbols**.

### Зависимости
Пакет ожидает установленный **XREAL XR Plugin** (`com.xreal.xr`). Он поставляется XREAL в виде tar-архива и не доступен в Unity Registry.

---

## Требования к Android-сборке
- **Min API Level**: 26+
- **Scripting Backend**: **IL2CPP**
- **Target Architecture**: **ARM64**
- **Graphics API**: **OpenGLES3**
- Разрешения в манифесте: **Camera**, **Internet** (для облачной локализации Immersal), при необходимости — **Write External Storage** (лог/кеш).

---

## Минимальная настройка сцены
Добавьте в сцену:
- **XR Origin (XR Rig)**  
- **AR Session**
- Объект **ImmersalSDK** (компонент)
- Объект **Localizer** (компонент) и нужные **Localization Method** (например, *DeviceLocalization* и/или *ServerLocalization*)
- **XrealSupport** (мост платформы для XREAL) — см. ниже  
*(Если работаете через обычный AR Foundation на поддерживаемом устройстве, можно использовать **ARFBridge**.)*

---

## Образцы (Samples)
Сцена, показывающая пользовательскую локализацию (в т.ч. для XREAL):

Samples~/Core/Scenes/CustomLocalizationSample.unity


---

## Базовое использование

### Вариант с XREAL (Beam Pro, Air 2 Ultra)
1. Добавьте компонент **`XrealSupport`** (пространство имён `Immersal.XR`) на GameObject в сцене.
2. Убедитесь, что в **XR Plug-in Management** включён **XREAL**, а **ARCore** — выключен.
3. На объекте **ImmersalSDK** укажите **Developer Token**, выберите методы локализации и добавьте ваши **XR Map** (через *embed* либо облако).
4. При запуске `XrealSupport` будет поставлять позу/кадры RGB и пр. данные в `ImmersalSDK.Session`.

> Если XREAL плагин не установлен или символ `IMMERSAL_XREAL` отсутствует, используется безопасная заглушка — проект соберётся, но XREAL-специфическая функциональность будет отключена.

### Вариант через AR Foundation
1. Добавьте компонент **`ARFBridge`** (из `Immersal.XR`) в сцену.
2. Мост передаёт CPU-кадры камеры, intrinsics и позу из AR Foundation в `ImmersalSDK.Session`.
3. Подходит для устройств с поддержкой ARCore/ARKit.

---

## Подсказки по Beam Pro
- Beam Pro официально не поддерживает ARCore, поэтому **ARCore следует отключить**.  
- В сцене используйте **XrealSupport**; для локализации Immersal применяйте **DeviceLocalization** (локально) и/или **ServerLocalization** (облако).

---

## Лицензия
© 2024 Immersal – часть Hexagon. Все права защищены.  
Immersal SDK не может копироваться, распространяться или предоставляться третьим лицам в коммерческих целях без письменного разрешения Immersal Ltd.  
По вопросам лицензирования: **sales@immersal.com**.

