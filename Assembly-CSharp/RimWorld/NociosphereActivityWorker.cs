using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BA2 RID: 11170
	public class NociosphereActivityWorker : PawnActivityWorker
	{
		// Token: 0x0600F9A2 RID: 63906 RVA: 0x00489DC0 File Offset: 0x00487FC0
		public override float GetChangeRatePerDay(ThingWithComps thing)
		{
			CompNociosphere comp = thing.GetComp<CompNociosphere>();
			float num = base.GetChangeRatePerDay(thing);
			if (comp.IsUnstable)
			{
				num += -0.05f;
			}
			return num;
		}

		// Token: 0x0600F9A3 RID: 63907 RVA: 0x00489DEC File Offset: 0x00487FEC
		public override void GetSummary(ThingWithComps thing, StringBuilder sb)
		{
			CompNociosphere comp = thing.GetComp<CompNociosphere>();
			base.GetSummary(thing, sb);
			if (comp.IsUnstable)
			{
				sb.Append(string.Format("\n - {0}: {1}", "Unstable".Translate(), -0.05f.ToStringPercent("0")));
			}
		}

		// Token: 0x0400A421 RID: 42017
		private const float UnstableChangePerDay = -0.05f;
	}
}
