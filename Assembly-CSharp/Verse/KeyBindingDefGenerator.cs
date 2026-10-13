using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D27 RID: 3367
	public static class KeyBindingDefGenerator
	{
		// Token: 0x060055D8 RID: 21976 RVA: 0x001BB6A1 File Offset: 0x001B98A1
		public static IEnumerable<KeyBindingCategoryDef> ImpliedKeyBindingCategoryDefs(bool hotReload = false)
		{
			List<KeyBindingCategoryDef> gameUniversalCats = (from d in DefDatabase<KeyBindingCategoryDef>.AllDefs
			where d.isGameUniversal
			select d).ToList<KeyBindingCategoryDef>();
			foreach (DesignationCategoryDef designationCategoryDef in DefDatabase<DesignationCategoryDef>.AllDefs)
			{
				string defName = "Architect_" + designationCategoryDef.defName;
				KeyBindingCategoryDef keyBindingCategoryDef = hotReload ? (DefDatabase<KeyBindingCategoryDef>.GetNamed(defName, false) ?? new KeyBindingCategoryDef()) : new KeyBindingCategoryDef();
				keyBindingCategoryDef.defName = defName;
				keyBindingCategoryDef.label = designationCategoryDef.label + " tab";
				keyBindingCategoryDef.description = "Key bindings for the \"" + designationCategoryDef.LabelCap + "\" section of the Architect menu";
				keyBindingCategoryDef.modContentPack = designationCategoryDef.modContentPack;
				keyBindingCategoryDef.checkForConflicts.AddRange(gameUniversalCats);
				for (int i = 0; i < gameUniversalCats.Count; i++)
				{
					gameUniversalCats[i].checkForConflicts.Add(keyBindingCategoryDef);
				}
				designationCategoryDef.bindingCatDef = keyBindingCategoryDef;
				yield return keyBindingCategoryDef;
			}
			IEnumerator<DesignationCategoryDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060055D9 RID: 21977 RVA: 0x001BB6B1 File Offset: 0x001B98B1
		public static IEnumerable<KeyBindingDef> ImpliedKeyBindingDefs(bool hotReload = false)
		{
			foreach (MainButtonDef mainButtonDef in from td in DefDatabase<MainButtonDef>.AllDefs
			orderby td.order
			select td)
			{
				if (mainButtonDef.defaultHotKey != KeyCode.None)
				{
					string defName = "MainTab_" + mainButtonDef.defName;
					KeyBindingDef keyBindingDef = hotReload ? (DefDatabase<KeyBindingDef>.GetNamed(defName, false) ?? new KeyBindingDef()) : new KeyBindingDef();
					keyBindingDef.label = "Toggle " + mainButtonDef.label + " tab";
					keyBindingDef.defName = defName;
					keyBindingDef.category = KeyBindingCategoryDefOf.MainTabs;
					keyBindingDef.defaultKeyCodeA = mainButtonDef.defaultHotKey;
					keyBindingDef.modContentPack = mainButtonDef.modContentPack;
					mainButtonDef.hotKey = keyBindingDef;
					yield return keyBindingDef;
				}
			}
			IEnumerator<MainButtonDef> enumerator = null;
			yield break;
			yield break;
		}
	}
}
