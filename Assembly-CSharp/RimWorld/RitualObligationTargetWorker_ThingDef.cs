using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026E3 RID: 9955
	public class RitualObligationTargetWorker_ThingDef : RitualObligationTargetFilter
	{
		// Token: 0x0600DA3B RID: 55867 RVA: 0x00408B0F File Offset: 0x00406D0F
		public RitualObligationTargetWorker_ThingDef()
		{
		}

		// Token: 0x0600DA3C RID: 55868 RVA: 0x00408B17 File Offset: 0x00406D17
		public RitualObligationTargetWorker_ThingDef(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DA3D RID: 55869 RVA: 0x00408B20 File Offset: 0x00406D20
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (this.def.thingDefs.NullOrEmpty<ThingDef>())
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.def.thingDefs.Count; i = num + 1)
			{
				ThingDef def = this.def.thingDefs[i];
				List<Thing> things = map.listerThings.ThingsOfDef(def);
				for (int j = 0; j < things.Count; j = num + 1)
				{
					if (base.CanUseTarget(things[j], obligation).canUse)
					{
						yield return things[j];
					}
					num = j;
				}
				things = null;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600DA3E RID: 55870 RVA: 0x00408B40 File Offset: 0x00406D40
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			return target.HasThing && this.def.thingDefs.Contains(target.Thing.def) && (!this.def.colonistThingsOnly || (target.Thing.Faction != null && target.Thing.Faction.IsPlayer));
		}

		// Token: 0x0600DA3F RID: 55871 RVA: 0x00408BAE File Offset: 0x00406DAE
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			int num;
			for (int i = 0; i < this.def.thingDefs.Count; i = num + 1)
			{
				yield return this.def.thingDefs[i].label;
				num = i;
			}
			yield break;
		}
	}
}
