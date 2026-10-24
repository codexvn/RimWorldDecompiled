# RimWorld 反编译源码（1.7）

本仓库由 `ilspycmd` 从本机 RimWorld 安装目录反编译生成，仅用于源码阅读、检索和调试参考，不可独立编译运行。

## 版本信息

- 游戏版本：`1.7.4985 rev1269`（程序集版本 1.7.4985，Steam buildid 26115268）
- 来源目录：`F:\SteamLibrary\steamapps\common\RimWorld`
- 反编译工具：`ilspycmd 9.x`
- 运行时：.NET 10（自定义 CLR 构建 `waylithclr 10.0.10`）
- 引擎：RimWorld 1.7 放弃 Unity Mono，迁移到 RocketWerkz Brutal / Waylith 自研引擎；`RimWorldWin64_Data\Managed` 目录已不存在

## 目录说明

| 目录 | 内容 |
|------|------|
| `Assembly-CSharp/` | 游戏主逻辑（Verse / RimWorld 命名空间完整保留，约 9300 个源文件） |
| `RimWorld/` | 新引擎引导入口程序集（RimWorldWaylith 命名空间：Bootstrap、Program、崩溃处理等） |
| `UnityStubs/` | UnityEngine 兼容层程序集：原 UnityEngine.* 的类型（MonoBehaviour、Vector3、Texture2D 等）全部定义于此，运行时由 CLR 重定向（`UnityEngine*=UnityStubs`）承接旧引用 |
| `Waylith/` | Waylith 引擎中间件核心（UnityStubs 的主要依赖） |
| `Waylith.UnityCompat/` | Waylith 引擎与 Unity 兼容层之间的胶水程序集 |
| `Waylith.AssetBake/` | Waylith 引擎资产烘焙程序集（1.7.4985 新增） |
| `Waylith.VideoDecode/` | Waylith 引擎视频解码程序集（1.7.4985 新增） |
| `PsdSharp/` | PSD 图像解析库（1.7.4985 新增，Assembly-CSharp 引用） |
| `Fmod5Sharp/` | FMOD 音频封装（UnityStubs 依赖，Verse.Sound 音频 API 的底层） |
| `AssetsTools.NET/` | Unity 资产文件读写库（UnityStubs 依赖） |
| `com.rlabrecque.steamworks.net/` | Steamworks .NET 封装（Assembly-CSharp 直接引用，Verse.Steam 的底层） |
| `Data/` | 官方 Core 与 DLC 的 XML Def、语言文件、贴图、音频和元数据资源。本目录被 `.gitignore` 忽略，不随仓库提交；需要完整资源参考时，从自己的 RimWorld 安装目录复制 `Data` 目录到仓库根目录 |

引擎本体（`Brutal.*`）、coreclr 与 `System.*` 运行时库、公开第三方库（NAudio、NVorbis、ISharpZipLib 等）不参与 mod 适配，未反编译。

## 版本线说明

- `main` 分支：跟随最新 1.7 反编译。
- `1.7` 分支：RimWorld 1.7 反编译（本分支）。
- `1.6-archive` 分支：RimWorld 1.6 反编译归档。

## 1.7 mod 适配关键事实

- mod 目标框架应对齐 `net10.0`（游戏与官方生态程序集均为 .NETCoreApp v10.0）。
- UnityEngine 引用编译期指向游戏根目录 `UnityStubs.dll`；运行时由 `WaylithClr.AssemblyNameRedirects` 重定向承接。
- Harmony 需使用创意工坊 1.7 版（Harmony 3.0.0-rc.2，net10.0）；引擎在 CLR 层为运行时 IL 补丁保留了 `JitPatchableAssemblies` 等开关。
- `Unity.Newtonsoft.Json` 被重定向为空且游戏无 Newtonsoft 程序集，依赖它的代码无解析目标。

## 反编译更新流程

游戏版本更新后，按以下步骤重建仓库内容并提交。

### 1. 确认版本信息

```powershell
Get-Content 'F:\SteamLibrary\steamapps\common\RimWorld\Version.txt'           # 形如 1.7.4985 rev1269
Select-String 'buildid' 'F:\SteamLibrary\steamapps\appmanifest_294100.acf'    # Steam buildid，记入版本信息
```

### 2. 反编译程序集

使用 dotnet 全局工具 `ilspycmd`（9.x，`dotnet tool install -g ilspycmd`）；`-p` 让每个程序集输出为带 `.csproj` 的项目结构。当前跟踪 11 个程序集：

```bash
cd <仓库根目录>
G='F:/SteamLibrary/steamapps/common/RimWorld'
for d in Assembly-CSharp RimWorld UnityStubs Waylith Waylith.UnityCompat \
         Waylith.AssetBake Waylith.VideoDecode PsdSharp Fmod5Sharp \
         AssetsTools.NET com.rlabrecque.steamworks.net; do
    ilspycmd "$G/$d.dll" -p -o ".decomp_tmp/$d"
    rm -rf "$d" && mv ".decomp_tmp/$d" "$d"
done
rm -rf .decomp_tmp
```

游戏新增托管程序集时（游戏根目录出现新的非运行时 `.dll`），将其加入上面列表，并在「目录说明」表格补一行。

### 3. 同步 Data 目录

```powershell
robocopy 'F:\SteamLibrary\steamapps\common\RimWorld\Data' '.\Data' /MIR
```

### 4. 更新版本信息并提交

将「版本信息」段落改为本次 Version.txt 的版本号与 buildid，然后提交到 `1.7` 分支、快进 `main` 并同时推送两个分支，保持同线：

```bash
git switch 1.7
git add -A && git commit -m "chore: RimWorld <版本号> <rev> 反编译更新"
git branch -f main 1.7 && git push origin 1.7 main
```
