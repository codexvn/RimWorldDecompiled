using System;
using System.Collections.Generic;
using LudeonTK;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A18 RID: 6680
	public class DebugTabMenuDef : Def
	{
		// Token: 0x0600969A RID: 38554 RVA: 0x002C0E63 File Offset: 0x002BF063
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(DebugTabMenu).IsAssignableFrom(this.menuClass))
			{
				yield return "menuClass does not derive from DebugTabMenu.";
			}
			yield break;
			yield break;
		}

		// Token: 0x040062F2 RID: 25330
		public Type menuClass;

		// Token: 0x040062F3 RID: 25331
		public int displayOrder = 99999;
	}
}
