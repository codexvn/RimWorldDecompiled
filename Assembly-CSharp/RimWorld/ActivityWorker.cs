using System.Text;
using Verse;

namespace RimWorld;

public class ActivityWorker
{
	public virtual float GetChangeRatePerDay(ThingWithComps thing)
	{
		CompActivity comp = thing.GetComp<CompActivity>();
		if (!comp.IsDormant)
		{
			return 0f;
		}
		return comp.Props.changePerDayBase;
	}

	public void GetInspectString(ThingWithComps thing, StringBuilder sb)
	{
		CompActivity comp = thing.GetComp<CompActivity>();
		sb.Append($"{"ActivityLevel".Translate()}: {comp.ActivityLevel.ToStringPercent("0")} ({GetChangeRatePerDay(thing).ToStringPercentSigned("0")} / {"day".Translate()})");
	}

	public virtual void GetSummary(ThingWithComps thing, StringBuilder sb)
	{
		CompActivity comp = thing.GetComp<CompActivity>();
		sb.AppendLine("\n\n" + $"{"ActivityIncrease".Translate()}: {GetChangeRatePerDay(thing).ToStringPercent("0")} / {"day".Translate()}");
		if (comp.State == ActivityState.Active)
		{
			sb.Append($" - {"IsActive".Translate()}: x0");
		}
		else
		{
			sb.Append($" - {"BaseLevel".Translate()}: {comp.Props.changePerDayBase.ToStringPercent("0")}");
		}
	}
}
