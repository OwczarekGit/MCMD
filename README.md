# The "MineCraft Mod Downloader" (MCMD)
## A CLI app for downloading Minecraft mods from curseforge.

#### If you're on Windows consider using alternative Terminal program, i would recommend [Alacritty](https://github.com/alacritty/alacritty) or [Windows Terminal](https://www.microsoft.com/pl-pl/p/windows-terminal/9n0dx20hk701?activetab=pivot:overviewtab), as both CMD and PowerShell have issues with the ammount of redraws that are happening in MCMD.


# For app to run you need to install [.NET 5.0](https://dotnet.microsoft.com/download)


![Screenshot from 2021-05-14 20-28-50](https://user-images.githubusercontent.com/32412218/118313465-0c384900-b4f3-11eb-85cc-4e5a12ff3ff3.png)

## Shortcuts
  * [ F ] - Search for mods.
  * [ TAB ] - Switch between the panels.
  * [ Space ] - Mark/Unmark mod.
  * [ C ] - Remove unmarked mods from list.
  * [ O ] - Open mod URL.
  * [ D ] - Download selected mods. (Right panel must be selected)
  * [ ↑, k ] - Move cursor up.
  * [ ↓, j ] - Move cursor down.
  * [ Ctrl + Q ] - Close app.

## Usage: ./MCMD "/full/path/to/target/directory"
##### example: ./MCMD "/home/myuser/.minecraft/mods/"

# Planned features:
- [x] Searching CurseForge for mods and downloading them and, their dependencies with a press of a button.
- [x] Dual panel setup with exclusive mod placement. 
- [ ] Searching specified directory for existing mods and provide an easy way of updating them.
- [ ] Easy forge/fabric mod loader download and installation.

### Keep in mind that MCMD is at the early stage of development, any help highly appreciated. Thanks!
