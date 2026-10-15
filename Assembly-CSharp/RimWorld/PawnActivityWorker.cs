using System.Text;
using Verse;

namespace RimWorld;

public class PawnActivityWorker : ActivityWorker
{
	public override float GetChangeRatePerDay(ThingWithComps thing)
	{
		CompActivity comp = thing.GetComp<CompActivity>();
		CompHoldingPlatformTarget compHoldingPlatformTarget = thing.TryGetComp<CompHoldingPlatformTarget>();
		Pawn pawn = (Pawn)thing;
		if (comp.State == ActivityState.Active)
		{
			return -0f;
		}
		float num = 0f;
		if (!pawn.health.hediffSet.IsPsychicallySuppressed)
		{
			num += base.GetChangeRatePerDay(thing);
			if (thing.IsOutside() && comp.Props.changePerDayOutside.HasValue)
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

	public override void GetSummary(ThingWithComps thing, StringBuilder sb)
	{
		CompActivity comp = thing.GetComp<CompActivity>();
		CompHoldingPlatformTarget compHoldingPlatformTarget = thing.TryGetComp<CompHoldingPlatformTarget>();
		base.GetSummary(thing, sb);
		if (thing is Pawn pawn && pawn.health.hediffSet.IsPsychicallySuppressed)
		{
			sb.Append($" - {"IsPsychicallySuppressed".Translate()}: x0");
			return;
		}
		if (thing.IsOutside() && comp.Props.changePerDayOutside.HasValue)
		{
			sb.Append($"\n - {"NotInSealedRoom".Translate()}: {comp.Props.changePerDayOutside.Value.ToStringPercent("0")}");
		}
		if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform && compHoldingPlatformTarget.HeldPlatform.HasAttachedElectroharvester)
		{
			sb.Append($"\n - {"EnergyHarvesting".Translate()}: {comp.Props.changePerDayElectroharvester.ToStringPercent("0")}");
		}
	}
}
