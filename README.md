# CheckM8 Activator

A Windows tool for bypassing iCloud activation on jailbroken iOS devices. Connects over USB SSH, installs the required tweaks, patches the activation record and MobileGestalt cache, then triggers a userspace reboot to apply the changes.

---

## Requirements

- Windows 10 or 11 (64-bit recommended)
- iTunes installed (required for USB device detection)
- Target device must be **jailbroken** with SSH enabled (root / alpine)
- Find My iPhone (FMI) must be **disabled**

---

## Supported Devices

**iPhones:** 5S through 16 series (including SE 1/2/3, X, XS, XR, XS Max)

**iPads:** iPad 2–10, iPad Air 1–5 (M2), iPad Mini 1–6, iPad Pro 9.7" through 13" (M4)

**iPod Touch:** 4th through 7th generation

---

## How It Works

1. Connects to the device over USB using `iproxy` to tunnel SSH
2. Verifies the jailbreak is accessible via SSH (`root@127.0.0.1:22`)
3. Remounts the root filesystem read/write
4. Uploads and extracts **ElleKit** (`ref/ellekit`) to `/var/jb/`
5. Installs **HASNIDylib** (activation patch tweak) via MobileSubstrate
6. Clears existing activation records from the system
7. Uploads `activation_record.plist` and locks it immutable with `chflags`
8. Patches **MobileGestalt** cache using bundled `getkey`, `z`, and `recache` tools
9. Disables OTA update daemons
10. Triggers `launchctl reboot userspace`

---

## Usage

1. Jailbreak the device and make sure SSH is running
2. Connect the device via USB
3. Launch `CheckM8.exe` (iTunes must be installed)
4. Wait for the device info to populate (Model, S/N, iOS version)
5. Click **Activate iDevice**
6. Wait for the process to complete — the device will reboot userspace automatically

---

## Additional Features

| Button | What it does |
|---|---|
| Activate iDevice | Runs the full activation bypass flow |
| MDM Unlock | Removes MDM enrollment via backup restore |
| OTA Block | Pushes a backup that disables OTA update checks |

---

## Project Structure

```
CheckM8.csproj          - Main project file (.NET 4.8 WinForms)
Titan/
  Form1.cs              - Main application logic
Core/
  DropShadow.cs         - Window chrome
libs/                   - Reference DLLs (Guna UI, LibUsbDotNet, SSH.NET, etc.)
ref/                    - Runtime files deployed to the device
  ellekit               - ElleKit tweak injector (tar)
  libirecovery-1.0.3.dll - Setup skip plist (deployed as purplebuddy.plist)
  activation_record.plist - Fallback activation record
  ios.exe               - idevice pairing utility
  libplist.exe          - Plist merge/patch tool
  imobiledevice/        - Working directory for gestalt plist operations
win-x64/                - Full idevice toolchain (64-bit Windows)
win-x86/                - Full idevice toolchain (32-bit Windows)
x64/                    - iproxy + USB libs (64-bit)
x86/                    - iproxy + USB libs (32-bit)
OTA/swp/                - OTA block backup payload
Backup/swp/             - MDM working directory (populated at runtime)
scripts/                - Logo/icon generation utilities
```

---

## Building

Requires Windows with Visual Studio or MSBuild installed.

```
dotnet restore CheckM8.csproj
msbuild CheckM8.csproj /p:Configuration=Release /p:Platform="Any CPU"
```

Output: `bin/Any CPU/Release/net48/CheckM8.exe`

GitHub Actions builds automatically on push to `main`.

---

## Customizing Logos

Two scripts in `scripts/` handle branding. Both require `pip install Pillow`.

| Script | What it replaces |
|---|---|
| `replace_exe_icon__app_ico.py` | `app.ico` — EXE file icon, taskbar, title bar |
| `replace_inapp_logo__Resources_logo1.py` | In-app logo shown in the center of the UI |

Edit the `CONFIGURATION` block at the top of each script, run it, then rebuild.

---

## Notes

- The tool requires iTunes for USB device enumeration via `iOSDeviceManager`
- SSH credentials are hardcoded to `root` / `alpine` (standard jailbreak defaults)
- The `Backup/swp/` directory is populated at runtime during MDM operations
- Anti-analysis checks run in the background — close debuggers/proxies before launching
