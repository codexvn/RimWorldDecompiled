using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020028C3 RID: 10435
	public class ScenPart_AutoActivateMonolith : ScenPart
	{
		// Token: 0x0600E43C RID: 58428 RVA: 0x0042AB2C File Offset: 0x00428D2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
		}

		// Token: 0x0600E43D RID: 58429 RVA: 0x0042AB48 File Offset: 0x00428D48
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			float num = (float)this.delayTicks / 60000f;
			Widgets.TextFieldNumericLabeled<float>(listing.GetScenPartRect(this, ScenPart.RowHeight), "delayDays".Translate().CapitalizeFirst(), ref num, ref this.delayTicksBuf, 0f, 1E+09f);
			this.delayTicks = (int)(num * 60000f);
		}

		// Token: 0x0600E43E RID: 58430 RVA: 0x0042ABAC File Offset: 0x00428DAC
		public override void PostGameStart()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			if (!Find.Anomaly.GenerateMonolith)
			{
				return;
			}
			if (Find.Anomaly.monolith == null)
			{
				return;
			}
			Find.Anomaly.monolith.AutoActivate(Find.TickManager.TicksGame + this.delayTicks);
		}

		// Token: 0x040098C4 RID: 39108
		public int delayTicks;

		// Token: 0x040098C5 RID: 39109
		private string delayTicksBuf;
	}
}
