# RimWorld 反编译源码（1.7）

本仓库由 `ilspycmd` 从本机 RimWorld 安装目录反编译生成，仅用于源码阅读、检索和调试参考，不可独立编译运行。

## 版本信息

- 游戏版本：`1.7.4977 rev926`（程序集版本 1.7.9782.27799，Steam buildid 25908500）
- 来源目录：`F:\SteamLibrary\steamapps\common\RimWorld`
- 反编译工具：`ilspycmd 9.x`
- 运行时：.NET 10（自定义 CLR 构建 `waylithclr 10.0.10`）
- 引擎：RimWorld 1.7 放弃 Unity Mono，迁移到 RocketWerkz Brutal / Waylith 自研引擎；`RimWorldWin64_Data\Managed` 目录已不存在

## 目录说明

| 目录 | 内容 |
|------|------|
| `Assembly-CSharp/` | 游戏主逻辑（Verse / RimWorld 命名空间完整保留，约 9200 个源文件） |
| `RimWorld/` | 新引擎引导入口程序集（RimWorldWaylith 命名空间：Bootstrap、Program、崩溃处理等） |
| `UnityStubs/` | UnityEngine 兼容层程序集：原 UnityEngine.* 的类型（MonoBehaviour、Vector3、Texture2D 等）全部定义于此，运行时由 CLR 重定向（`UnityEngine*=UnityStubs`）承接旧引用 |
| `Waylith/` | Waylith 引擎中间件核心（UnityStubs 的主要依赖） |
| `Waylith.UnityCompat/` | Waylith 引擎与 Unity 兼容层之间的胶水程序集 |
| `Fmod5Sharp/` | FMOD 音频封装（UnityStubs 依赖，Verse.Sound 音频 API 的底层） |
| `AssetsTools.NET/` | Unity 资产文件读写库（UnityStubs 依赖） |
| `com.rlabrecque.steamworks.net/` | Steamworks .NET 封装（Assembly-CSharp 直接引用，Verse.Steam 的底层） |
| `Data/` | 官方 Core 与 DLC 的 XML Def、语言文件、贴图、音频和元数据资源。本目录被 `.gitignore` 忽略，不随仓库提交；需要完整资源参考时，从自己的 RimWorld 安装目录复制 `Data` 目录到仓库根目录（当前本地副本为 1.6 内容，1.7 需重新复制） |

引擎本体（`Brutal.*`）、coreclr 与 `System.*` 运行时库、公开第三方库（NAudio、NVorbis、ISharpZipLib 等）不参与 mod 适配，未反编译。

## 版本线说明

- `main` 分支：RimWorld 1.6 反编译。
- `1.7` 分支：RimWorld 1.7 反编译（本分支）。

## 1.7 mod 适配关键事实

- mod 目标框架应对齐 `net10.0`（游戏与官方生态程序集均为 .NETCoreApp v10.0）。
- UnityEngine 引用编译期指向游戏根目录 `UnityStubs.dll`；运行时由 `WaylithClr.AssemblyNameRedirects` 重定向承接。
- Harmony 需使用创意工坊 1.7 版（Harmony 3.0.0-rc.2，net10.0）；引擎在 CLR 层为运行时 IL 补丁保留了 `JitPatchableAssemblies` 等开关。
- `Unity.Newtonsoft.Json` 被重定向为空且游戏无 Newtonsoft 程序集，依赖它的代码无解析目标。

## Data 目录复制示例

```powershell
robocopy 'F:\SteamLibrary\steamapps\common\RimWorld\Data' '.\Data' /E
```
