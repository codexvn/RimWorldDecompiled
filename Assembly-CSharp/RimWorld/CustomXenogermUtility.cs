using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003148 RID: 12616
	public static class CustomXenogermUtility
	{
		// Token: 0x06011BF0 RID: 72688 RVA: 0x0051F4AC File Offset: 0x0051D6AC
		public static AcceptanceReport SaveXenogermTemplate(string xenoTypeName, XenotypeIconDef iconDef, List<Genepack> selectedGenepacks)
		{
			if (selectedGenepacks.NullOrEmpty<Genepack>())
			{
				return "CannotSaveXenogermTemplate".Translate("GenepackPlural".Translate());
			}
			if (xenoTypeName.NullOrEmpty())
			{
				return "CannotSaveXenogermTemplate".Translate("XenotypeName".Translate());
			}
			CustomXenogerm customXenogerm = new CustomXenogerm(xenoTypeName, iconDef, selectedGenepacks);
			Find.CustomXenogermDatabase.Add(customXenogerm);
			Messages.Message("XenogermTemplateSaved".Translate(xenoTypeName.Named("NAME")), MessageTypeDefOf.PositiveEvent, false);
			return true;
		}

		// Token: 0x06011BF1 RID: 72689 RVA: 0x0051F546 File Offset: 0x0051D746
		public static IEnumerable<Genepack> GetMatchingGenepacks(IEnumerable<GeneSet> templateGenesets, IEnumerable<Genepack> genepacks)
		{
			CustomXenogermUtility.tmpGenepacks.Clear();
			CustomXenogermUtility.tmpGenepacks.AddRange(genepacks);
			using (IEnumerator<GeneSet> enumerator = templateGenesets.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GeneSet templateGeneset = enumerator.Current;
					Genepack genePack = CustomXenogermUtility.tmpGenepacks.FirstOrDefault((Genepack gp) => gp.GeneSet.Matches(templateGeneset));
					if (genePack != null)
					{
						yield return genePack;
						CustomXenogermUtility.tmpGenepacks.Remove(genePack);
					}
					genePack = null;
				}
			}
			IEnumerator<GeneSet> enumerator = null;
			CustomXenogermUtility.tmpGenepacks.Clear();
			yield break;
			yield break;
		}

		// Token: 0x0400B714 RID: 46868
		private static List<Genepack> tmpGenepacks = new List<Genepack>();
	}
}
