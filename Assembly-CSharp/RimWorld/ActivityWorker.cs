using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B9F RID: 11167
	public class ActivityWorker
	{
		// Token: 0x0600F998 RID: 63896 RVA: 0x0048991C File Offset: 0x00487B1C
		public virtual float GetChangeRatePerDay(ThingWithComps thing)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			if (!comp.IsDormant)
			{
				return 0f;
			}
			return comp.Props.changePerDayBase;
		}

		// Token: 0x0600F999 RID: 63897 RVA: 0x0048994C File Offset: 0x00487B4C
		public void GetInspectString(ThingWithComps thing, StringBuilder sb)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			sb.Append(string.Format("{0}: {1} ({2} / {3})", new object[]
			{
				"ActivityLevel".Translate(),
				comp.ActivityLevel.ToStringPercent("0"),
				this.GetChangeRatePerDay(thing).ToStringPercentSigned("0"),
				"day".Translate()
			}));
		}

		// Token: 0x0600F99A RID: 63898 RVA: 0x004899C4 File Offset: 0x00487BC4
		public virtual void GetSummary(ThingWithComps thing, StringBuilder sb)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			sb.AppendLine("\n\n" + string.Format("{0}: {1} / {2}", "ActivityIncrease".Translate(), this.GetChangeRatePerDay(thing).ToStringPercent("0"), "day".Translate()));
			if (comp.State == ActivityState.Active)
			{
				sb.Append(string.Format(" - {0}: x0", "IsActive".Translate()));
				return;
			}
			sb.Append(string.Format(" - {0}: {1}", "BaseLevel".Translate(), comp.Props.changePerDayBase.ToStringPercent("0")));
		}
	}
}
