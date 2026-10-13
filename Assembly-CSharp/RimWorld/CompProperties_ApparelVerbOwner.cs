using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BDF RID: 11231
	public class CompProperties_ApparelVerbOwner : CompProperties
	{
		// Token: 0x0600FB5B RID: 64347 RVA: 0x00490196 File Offset: 0x0048E396
		public CompProperties_ApparelVerbOwner()
		{
			this.compClass = typeof(CompApparelVerbOwner);
		}

		// Token: 0x0600FB5C RID: 64348 RVA: 0x004901BC File Offset: 0x0048E3BC
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(Apparel).IsAssignableFrom(parentDef.thingClass))
			{
				yield return string.Format("Comp {0} can only be added to Apparel", this.compClass);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400A4F7 RID: 42231
		public KeyBindingDef hotKey;

		// Token: 0x0400A4F8 RID: 42232
		public bool displayGizmoWhileUndrafted = true;

		// Token: 0x0400A4F9 RID: 42233
		public bool displayGizmoWhileDrafted = true;
	}
}
