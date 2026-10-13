using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200293F RID: 10559
	public class Building_Art : Building
	{
		// Token: 0x0600E732 RID: 59186 RVA: 0x00436088 File Offset: 0x00434288
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (!string.IsNullOrEmpty(text))
			{
				text += "\n";
			}
			return text + string.Format("{0}: {1}", StatDefOf.Beauty.LabelCap, StatDefOf.Beauty.ValueToString(this.GetStatValue(StatDefOf.Beauty, true, -1), ToStringNumberSense.Absolute, true));
		}
	}
}
