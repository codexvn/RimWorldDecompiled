using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002655 RID: 9813
	public class Precept_RitualSeat : Precept_ThingDef
	{
		// Token: 0x170020E6 RID: 8422
		// (get) Token: 0x0600D70D RID: 55053 RVA: 0x003FC40E File Offset: 0x003FA60E
		public override string UIInfoSecondLine
		{
			get
			{
				return base.ThingDef.LabelCap;
			}
		}

		// Token: 0x0600D70E RID: 55054 RVA: 0x003FC420 File Offset: 0x003FA620
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			using (IEnumerator<PreceptThingChance> enumerator = this.def.Worker.ThingDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PreceptThingChance d = enumerator.Current;
					if (d.def != base.ThingDef)
					{
						yield return new FloatMenuOption("PreceptReplaceWith".Translate() + ": " + d.def.LabelCap, delegate()
						{
							this.ThingDef = d.def;
						}, d.def, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null);
					}
				}
			}
			IEnumerator<PreceptThingChance> enumerator = null;
			yield break;
			yield break;
		}
	}
}
