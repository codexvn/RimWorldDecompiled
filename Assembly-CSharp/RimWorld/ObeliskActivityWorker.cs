using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BA1 RID: 11169
	public class ObeliskActivityWorker : ActivityWorker
	{
		// Token: 0x0600F99F RID: 63903 RVA: 0x00489C38 File Offset: 0x00487E38
		public override float GetChangeRatePerDay(ThingWithComps thing)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			float num = base.GetChangeRatePerDay(thing);
			if (thing.IsOutside() && comp.Props.changePerDayOutside != null)
			{
				num += comp.Props.changePerDayOutside.Value;
			}
			return num + this.ActivityGainPerDayFromHealthCurve.Evaluate((float)thing.HitPoints / (float)thing.MaxHitPoints);
		}

		// Token: 0x0600F9A0 RID: 63904 RVA: 0x00489CA0 File Offset: 0x00487EA0
		public override void GetSummary(ThingWithComps thing, StringBuilder sb)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			base.GetSummary(thing, sb);
			float num = this.ActivityGainPerDayFromHealthCurve.Evaluate((float)thing.HitPoints / (float)thing.MaxHitPoints);
			if (num > 0f)
			{
				sb.Append(string.Format("\n - {0}: {1}", "HealthFactor".Translate(), num.ToStringPercent("0")));
			}
			if (thing.IsOutside() && comp.Props.changePerDayOutside != null)
			{
				sb.Append(string.Format("\n - {0}: {1}", "NotInSealedRoom".Translate(), comp.Props.changePerDayOutside.Value.ToStringPercent("0")));
			}
		}

		// Token: 0x0400A420 RID: 42016
		private SimpleCurve ActivityGainPerDayFromHealthCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 47.9f),
				true
			},
			{
				new CurvePoint(0.1f, 23.9f),
				true
			},
			{
				new CurvePoint(0.8f, 0f),
				true
			}
		};
	}
}
