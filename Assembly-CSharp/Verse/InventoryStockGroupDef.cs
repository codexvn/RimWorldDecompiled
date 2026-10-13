using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000296 RID: 662
	public class InventoryStockGroupDef : Def
	{
		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06001082 RID: 4226 RVA: 0x0004E5AB File Offset: 0x0004C7AB
		public ThingDef DefaultThingDef
		{
			get
			{
				return this.defaultThingDef ?? this.thingDefs.First<ThingDef>();
			}
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x0004E5C2 File Offset: 0x0004C7C2
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.defaultThingDef != null && !this.thingDefs.Contains(this.defaultThingDef))
			{
				yield return "Default thing def " + this.defaultThingDef.defName + " should be in thingDefs but not found.";
			}
			if (this.min > this.max)
			{
				yield return "Min should be less than max.";
			}
			if (this.min < 0 || this.max < 0)
			{
				yield return "Min/max should be greater than zero.";
			}
			if (this.thingDefs.NullOrEmpty<ThingDef>())
			{
				yield return "thingDefs cannot be null or empty.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000D95 RID: 3477
		public List<ThingDef> thingDefs;

		// Token: 0x04000D96 RID: 3478
		public int min;

		// Token: 0x04000D97 RID: 3479
		public int max = 3;

		// Token: 0x04000D98 RID: 3480
		public ThingDef defaultThingDef;
	}
}
