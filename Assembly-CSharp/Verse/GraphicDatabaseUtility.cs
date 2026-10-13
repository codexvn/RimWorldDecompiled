using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000885 RID: 2181
	public static class GraphicDatabaseUtility
	{
		// Token: 0x06003766 RID: 14182 RVA: 0x0012889E File Offset: 0x00126A9E
		public static IEnumerable<string> GraphicNamesInFolder(string folderPath)
		{
			HashSet<string> loadedAssetNames = new HashSet<string>();
			Texture2D[] array = Resources.LoadAll<Texture2D>("Textures/" + folderPath);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].name.Split('_', StringSplitOptions.None);
				string text = "";
				if (array2.Length <= 2)
				{
					text = array2[0];
				}
				else if (array2.Length == 3)
				{
					text = array2[0] + "_" + array2[1];
				}
				else if (array2.Length == 4)
				{
					text = string.Concat(new string[]
					{
						array2[0],
						"_",
						array2[1],
						"_",
						array2[2]
					});
				}
				else
				{
					Log.Error("Cannot load assets with >3 pieces.");
				}
				if (!loadedAssetNames.Contains(text))
				{
					loadedAssetNames.Add(text);
					yield return text;
				}
			}
			array = null;
			yield break;
		}
	}
}
