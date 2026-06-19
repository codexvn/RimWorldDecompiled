# RimWorld 反编译源码

本目录由 `ilspycmd` 从本机 RimWorld 程序集反编译生成，仅用于本地源码阅读、检索和调试参考。

- 游戏版本：`1.6.4850 rev645`
- 来源目录：`F:\SteamLibrary\steamapps\common\RimWorld`
- 反编译工具：`ilspycmd 9.1.0.7988`
- 主程序集：`RimWorldWin64_Data\Managed\Assembly-CSharp.dll`
- 首轮程序集：`RimWorldWin64_Data\Managed\Assembly-CSharp-firstpass.dll`
- 资源目录：`Data`（不入库，开发者按需从本机游戏目录复制）

目录说明：

- `Assembly-CSharp/`：RimWorld/Verse 主游戏逻辑反编译项目。
- `Assembly-CSharp-firstpass/`：firstpass 程序集反编译项目。
- `Data/`：官方 Core 与 DLC 的 XML Def、语言文件、贴图、音频和元数据资源。本目录被 `.gitignore` 忽略，不随仓库提交；需要完整资源参考时，请从自己的 RimWorld 安装目录复制 `Data` 目录到仓库根目录。

复制示例：

```powershell
robocopy 'F:\SteamLibrary\steamapps\common\RimWorld\Data' '.\Data' /E
```
