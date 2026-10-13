using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003700 RID: 14080
	public class StatPart_Genes : StatPart
	{
		// Token: 0x06013866 RID: 79974 RVA: 0x005CB2B0 File Offset: 0x005C94B0
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			if (req.HasThing)
			{
				Genepack genepack = req.Thing as Genepack;
				if (genepack != null && genepack.GeneSet != null)
				{
					val *= this.NumGenesFactor(genepack.GeneSet);
					val *= this.ArchiteCostFactor(genepack.GeneSet);
					val *= this.GenesFactorsDefined(genepack.GeneSet);
				}
			}
		}

		// Token: 0x06013867 RID: 79975 RVA: 0x005CB319 File Offset: 0x005C9519
		private float NumGenesFactor(GeneSet geneSet)
		{
			return Mathf.Max(3.5f - 0.5f * (float)geneSet.GenesListForReading.Count, 0.5f);
		}

		// Token: 0x06013868 RID: 79976 RVA: 0x005CB33D File Offset: 0x005C953D
		private float ArchiteCostFactor(GeneSet geneSet)
		{
			return 1f + 3f * (float)geneSet.ArchitesTotal;
		}

		// Token: 0x06013869 RID: 79977 RVA: 0x005CB354 File Offset: 0x005C9554
		private float GenesFactorsDefined(GeneSet geneSet)
		{
			float num = 1f;
			for (int i = 0; i < geneSet.GenesListForReading.Count; i++)
			{
				num *= geneSet.GenesListForReading[i].marketValueFactor;
			}
			return num;
		}

		// Token: 0x0601386A RID: 79978 RVA: 0x005CB394 File Offset: 0x005C9594
		public override string ExplanationPart(StatRequest req)
		{
			string text = null;
			if (!ModsConfig.BiotechActive)
			{
				return text;
			}
			if (req.HasThing)
			{
				Genepack genepack = req.Thing as Genepack;
				if (genepack != null && genepack.GeneSet != null)
				{
					int count = genepack.GeneSet.GenesListForReading.Count;
					if (count > 0)
					{
						if (!text.NullOrEmpty())
						{
							text += "\n";
						}
						text += string.Format("{0} ({1}): x{2}", "NumberOfGenes".Translate(), count, this.NumGenesFactor(genepack.GeneSet).ToStringPercent());
					}
					int architesTotal = genepack.GeneSet.ArchitesTotal;
					if (architesTotal != 0)
					{
						if (!text.NullOrEmpty())
						{
							text += "\n";
						}
						text += string.Format("{0} ({1}): x{2}", "ArchitesTotal".Translate(), architesTotal, this.ArchiteCostFactor(genepack.GeneSet).ToStringPercent());
					}
					if (this.GenesFactorsDefined(genepack.GeneSet) != 1f)
					{
						if (!text.NullOrEmpty())
						{
							text += "\n";
						}
						text += "GenePriceFactors".Translate() + ":";
						for (int i = 0; i < genepack.GeneSet.GenesListForReading.Count; i++)
						{
							GeneDef geneDef = genepack.GeneSet.GenesListForReading[i];
							if (geneDef.marketValueFactor != 1f)
							{
								text += string.Format("\n  - {0} x{1}", geneDef.LabelCap, geneDef.marketValueFactor.ToStringPercent());
							}
						}
					}
					return text.TrimEndNewlines();
				}
			}
			return text;
		}
	}
}
