using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200270C RID: 9996
	public class RitualObligationTargetWorker_CampfireParty : RitualObligationTargetWorker_ThingDef
	{
		// Token: 0x0600DB57 RID: 56151 RVA: 0x0040B609 File Offset: 0x00409809
		public RitualObligationTargetWorker_CampfireParty()
		{
		}

		// Token: 0x0600DB58 RID: 56152 RVA: 0x0040B611 File Offset: 0x00409811
		public RitualObligationTargetWorker_CampfireParty(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DB59 RID: 56153 RVA: 0x0040B81C File Offset: 0x00409A1C
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!ModLister.CheckIdeology("Campfire party"))
			{
				return false;
			}
			if (!base.CanUseTargetInternal(target, obligation).canUse)
			{
				return false;
			}
			Thing thing = target.Thing;
			CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
			if (compRefuelable != null && !compRefuelable.HasFuel)
			{
				return "RitualTargetCampfireNoFuel".Translate();
			}
			List<Thing> forCell = target.Map.listerBuldingOfDefInProximity.GetForCell(target.Cell, (float)this.def.maxDrumDistance, ThingDefOf.Drum, null);
			bool flag = false;
			using (List<Thing>.Enumerator enumerator = forCell.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetRoom(RegionType.Set_All) == thing.GetRoom(RegionType.Set_All))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return "RitualTargetNoDrum".Translate();
			}
			return true;
		}

		// Token: 0x0600DB5A RID: 56154 RVA: 0x0040B910 File Offset: 0x00409B10
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetCampfirePartyInfo".Translate();
			yield return ThingDefOf.RitualSpot.label;
			yield break;
		}
	}
}
