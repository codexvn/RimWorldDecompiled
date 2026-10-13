using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BA0 RID: 11168
	public class PawnActivityWorker : ActivityWorker
	{
		// Token: 0x0600F99C RID: 63900 RVA: 0x00489A84 File Offset: 0x00487C84
		public override float GetChangeRatePerDay(ThingWithComps thing)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			CompHoldingPlatformTarget compHoldingPlatformTarget = thing.TryGetComp<CompHoldingPlatformTarget>();
			Pawn pawn = (Pawn)thing;
			if (comp.State == ActivityState.Active)
			{
				return --0f;
			}
			float num = 0f;
			if (!pawn.health.hediffSet.IsPsychicallySuppressed)
			{
				num += base.GetChangeRatePerDay(thing);
				if (thing.IsOutside() && comp.Props.changePerDayOutside != null)
				{
					num += comp.Props.changePerDayOutside.Value;
				}
				if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform && compHoldingPlatformTarget.HeldPlatform.HasAttachedElectroharvester)
				{
					num += comp.Props.changePerDayElectroharvester;
				}
			}
			return num;
		}

		// Token: 0x0600F99D RID: 63901 RVA: 0x00489B2C File Offset: 0x00487D2C
		public override void GetSummary(ThingWithComps thing, StringBuilder sb)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			CompHoldingPlatformTarget compHoldingPlatformTarget = thing.TryGetComp<CompHoldingPlatformTarget>();
			base.GetSummary(thing, sb);
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.health.hediffSet.IsPsychicallySuppressed)
			{
				sb.Append(string.Format(" - {0}: x0", "IsPsychicallySuppressed".Translate()));
				return;
			}
			if (thing.IsOutside() && comp.Props.changePerDayOutside != null)
			{
				sb.Append(string.Format("\n - {0}: {1}", "NotInSealedRoom".Translate(), comp.Props.changePerDayOutside.Value.ToStringPercent("0")));
			}
			if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform && compHoldingPlatformTarget.HeldPlatform.HasAttachedElectroharvester)
			{
				sb.Append(string.Format("\n - {0}: {1}", "EnergyHarvesting".Translate(), comp.Props.changePerDayElectroharvester.ToStringPercent("0")));
			}
		}
	}
}
