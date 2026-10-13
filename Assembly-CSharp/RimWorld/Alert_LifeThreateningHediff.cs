using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FC8 RID: 12232
	public class Alert_LifeThreateningHediff : Alert_Critical
	{
		// Token: 0x17002CB2 RID: 11442
		// (get) Token: 0x060113D9 RID: 70617 RVA: 0x004F08CC File Offset: 0x004EEACC
		private List<Pawn> SickPawns
		{
			get
			{
				this.sickPawnsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonistsAndPrisoners_NoCryptosleep)
				{
					bool flag = ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.HasActiveGene(GeneDefOf.Deathless);
					for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
					{
						Hediff hediff = pawn.health.hediffSet.hediffs[i];
						if (hediff.IsCurrentlyLifeThreatening && !hediff.FullyImmune())
						{
							if (flag)
							{
								HediffStage curStage = hediff.CurStage;
								if (curStage == null || !curStage.mtbDeathDestroysBrain)
								{
									goto IL_9B;
								}
							}
							this.sickPawnsResult.Add(pawn);
							break;
						}
						IL_9B:;
					}
				}
				return this.sickPawnsResult;
			}
		}

		// Token: 0x060113DA RID: 70618 RVA: 0x004F09C4 File Offset: 0x004EEBC4
		public override string GetLabel()
		{
			return "PawnsWithLifeThreateningDisease".Translate();
		}

		// Token: 0x060113DB RID: 70619 RVA: 0x004F09D8 File Offset: 0x004EEBD8
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			foreach (Pawn pawn in this.SickPawns)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
				foreach (Hediff hediff in pawn.health.hediffSet.hediffs)
				{
					if (hediff.CurStage != null && hediff.IsCurrentlyLifeThreatening && hediff.Part != null && hediff.Part != pawn.RaceProps.body.corePart)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return "PawnsWithLifeThreateningDiseaseAmputationDesc".Translate(stringBuilder.ToString().TrimEndNewlines());
			}
			return "PawnsWithLifeThreateningDiseaseDesc".Translate(stringBuilder.ToString().TrimEndNewlines());
		}

		// Token: 0x060113DC RID: 70620 RVA: 0x004F0B0C File Offset: 0x004EED0C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.SickPawns);
		}

		// Token: 0x0400B2F4 RID: 45812
		private List<Pawn> sickPawnsResult = new List<Pawn>();
	}
}
