using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200270A RID: 9994
	public class RitualObligationTargetWorker_LightBall : RitualObligationTargetWorker_ThingDef
	{
		// Token: 0x0600DB4B RID: 56139 RVA: 0x0040B609 File Offset: 0x00409809
		public RitualObligationTargetWorker_LightBall()
		{
		}

		// Token: 0x0600DB4C RID: 56140 RVA: 0x0040B611 File Offset: 0x00409811
		public RitualObligationTargetWorker_LightBall(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DB4D RID: 56141 RVA: 0x0040B61C File Offset: 0x0040981C
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!ModLister.CheckIdeology("Lightball target"))
			{
				return false;
			}
			if (!base.CanUseTargetInternal(target, obligation).canUse)
			{
				return false;
			}
			Thing thing = target.Thing;
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			if (compPowerTrader != null)
			{
				if (compPowerTrader.PowerNet == null || !compPowerTrader.PowerNet.HasActivePowerSource)
				{
					return "RitualTargetLightBallIsNotPowered".Translate();
				}
				List<Thing> forCell = target.Map.listerBuldingOfDefInProximity.GetForCell(target.Cell, (float)this.def.maxSpeakerDistance, ThingDefOf.Loudspeaker, null);
				bool flag = false;
				foreach (Thing thing2 in forCell)
				{
					CompPowerTrader compPowerTrader2 = thing2.TryGetComp<CompPowerTrader>();
					if (thing2.GetRoom(RegionType.Set_All) == thing.GetRoom(RegionType.Set_All) && compPowerTrader2.PowerNet != null && compPowerTrader2.PowerNet.HasActivePowerSource)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return "RitualTargetNoPoweredSpeakers".Translate();
				}
			}
			return true;
		}

		// Token: 0x0600DB4E RID: 56142 RVA: 0x0040B740 File Offset: 0x00409940
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return ThingDefOf.LightBall.label;
			yield return ThingDefOf.RitualSpot.label;
			yield break;
		}
	}
}
