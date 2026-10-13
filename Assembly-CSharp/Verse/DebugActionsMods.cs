using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LudeonTK;

namespace Verse
{
	// Token: 0x0200098C RID: 2444
	public class DebugActionsMods
	{
		// Token: 0x06003E32 RID: 15922 RVA: 0x00145810 File Offset: 0x00143A10
		[DebugAction("Mods", null, false, false, false, false, false, 0, false, allowedGameStates = AllowedGameStates.Entry)]
		private static void LoadedFilesForMod()
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (List<ModContentPack>.Enumerator enumerator = LoadedModManager.RunningModsListForReading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DebugActionsMods.<>c__DisplayClass0_0 CS$<>8__locals1 = new DebugActionsMods.<>c__DisplayClass0_0();
					CS$<>8__locals1.mod = enumerator.Current;
					list.Add(new DebugMenuOption(CS$<>8__locals1.mod.Name, DebugMenuOptionMode.Action, delegate()
					{
						ModMetaData metaData = ModLister.GetModWithIdentifier(CS$<>8__locals1.mod.PackageId, false);
						if (metaData.loadFolders != null && metaData.loadFolders.DefinedVersions().Count != 0)
						{
							Find.WindowStack.Add(new Dialog_DebugOptionListLister(from ver in metaData.loadFolders.DefinedVersions()
							select new DebugMenuOption(ver, DebugMenuOptionMode.Action, delegate()
							{
								DebugActionsMods.<>c__DisplayClass0_0 CS$<>8__locals4 = CS$<>8__locals1;
								IEnumerable<LoadFolder> source = metaData.loadFolders.FoldersForVersion(ver);
								Func<LoadFolder, string> selector;
								if ((selector = CS$<>8__locals1.<>9__13) == null)
								{
									selector = (CS$<>8__locals1.<>9__13 = ((LoadFolder f) => Path.Combine(CS$<>8__locals1.mod.RootDir, f.folderName)));
								}
								CS$<>8__locals4.<LoadedFilesForMod>g__ShowTable|1(source.Select(selector).Reverse<string>().ToList<string>());
							}), null));
							return;
						}
						base.<LoadedFilesForMod>g__ShowTable|1(null);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}
	}
}
