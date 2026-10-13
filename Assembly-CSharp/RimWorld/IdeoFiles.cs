using System;
using System.Collections.Generic;
using System.IO;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E8D RID: 7821
	public static class IdeoFiles
	{
		// Token: 0x17001A88 RID: 6792
		// (get) Token: 0x0600AD9E RID: 44446 RVA: 0x00318F65 File Offset: 0x00317165
		public static IEnumerable<Ideo> AllIdeosLocal
		{
			get
			{
				return IdeoFiles.ideosLocal;
			}
		}

		// Token: 0x0600AD9F RID: 44447 RVA: 0x00318F6C File Offset: 0x0031716C
		public static void RecacheData()
		{
			IdeoFiles.ideosLocal.Clear();
			using (IEnumerator<FileInfo> enumerator = GenFilePaths.AllCustomIdeoFiles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Ideo item;
					if (GameDataSaveLoader.TryLoadIdeo(enumerator.Current.FullName, out item))
					{
						IdeoFiles.ideosLocal.Add(item);
					}
				}
			}
		}

		// Token: 0x04007780 RID: 30592
		private static List<Ideo> ideosLocal = new List<Ideo>();
	}
}
