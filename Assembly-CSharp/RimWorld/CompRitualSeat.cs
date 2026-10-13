using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DEB RID: 11755
	public class CompRitualSeat : ThingComp
	{
		// Token: 0x060108BB RID: 67771 RVA: 0x004C5538 File Offset: 0x004C3738
		public override string CompInspectStringExtra()
		{
			string ideosString = this.GetIdeosString(null);
			if (string.IsNullOrEmpty(ideosString))
			{
				return null;
			}
			return "RitualSeatOf".Translate(ideosString.Named("IDEOS")).Resolve();
		}

		// Token: 0x060108BC RID: 67772 RVA: 0x004C5574 File Offset: 0x004C3774
		private string GetIdeosString(List<Ideo> outIdeos = null)
		{
			CompRitualSeat.tmpStringBuilder.Clear();
			foreach (Ideo ideo in Find.IdeoManager.IdeosInViewOrder)
			{
				bool flag = false;
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_RitualSeat precept_RitualSeat = precept as Precept_RitualSeat;
					if (precept_RitualSeat != null && precept_RitualSeat.ThingDef == this.parent.def)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (outIdeos != null)
					{
						outIdeos.Add(ideo);
					}
					if (CompRitualSeat.tmpStringBuilder.Length > 0)
					{
						CompRitualSeat.tmpStringBuilder.Append(", ");
					}
					CompRitualSeat.tmpStringBuilder.Append(ideo.name.ApplyTag(ideo).Resolve());
				}
			}
			return CompRitualSeat.tmpStringBuilder.ToString();
		}

		// Token: 0x060108BD RID: 67773 RVA: 0x004C5684 File Offset: 0x004C3884
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompRitualSeat.tmpIdeos.Clear();
			string ideosString = this.GetIdeosString(CompRitualSeat.tmpIdeos);
			if (string.IsNullOrEmpty(ideosString))
			{
				yield break;
			}
			Dialog_InfoCard.Hyperlink[] array = new Dialog_InfoCard.Hyperlink[CompRitualSeat.tmpIdeos.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = new Dialog_InfoCard.Hyperlink(CompRitualSeat.tmpIdeos[i]);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Thing_RelatedToIdeos_Name".Translate(), ideosString, "Stat_Thing_RelatedToIdeos_Desc".Translate(), 1110, null, array, false, false);
			yield break;
		}

		// Token: 0x0400AC8D RID: 44173
		private static StringBuilder tmpStringBuilder = new StringBuilder();

		// Token: 0x0400AC8E RID: 44174
		private static List<Ideo> tmpIdeos = new List<Ideo>();
	}
}
