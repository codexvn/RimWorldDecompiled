using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FE9 RID: 12265
	public class Alert_NeedDoctor : Alert
	{
		// Token: 0x0601146B RID: 70763 RVA: 0x004F2FCB File Offset: 0x004F11CB
		public Alert_NeedDoctor()
		{
			this.defaultLabel = "NeedDoctor".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x17002CCD RID: 11469
		// (get) Token: 0x0601146C RID: 70764 RVA: 0x004F3008 File Offset: 0x004F1208
		private List<Pawn> Patients
		{
			get
			{
				this.patientsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						bool flag = false;
						foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if ((pawn.Spawned || pawn.BrieflyDespawned()) && !pawn.Downed && pawn.workSettings != null && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Doctor))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							foreach (Pawn pawn2 in maps[i].mapPawns.FreeColonistsSpawned)
							{
								if (pawn2.Spawned || pawn2.BrieflyDespawned())
								{
									if (pawn2.Downed && !LifeStageUtility.AlwaysDowned(pawn2))
									{
										Pawn_NeedsTracker needs = pawn2.needs;
										if (((needs != null) ? needs.food : null) != null && pawn2.needs.food.CurCategory > HungerCategory.Fed && pawn2.InBed())
										{
											goto IL_128;
										}
									}
									if (!HealthAIUtility.ShouldBeTendedNowByPlayer(pawn2))
									{
										continue;
									}
									IL_128:
									this.patientsResult.Add(pawn2);
								}
							}
						}
					}
				}
				return this.patientsResult;
			}
		}

		// Token: 0x0601146D RID: 70765 RVA: 0x004F3198 File Offset: 0x004F1398
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.patientsResult)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "NeedDoctorDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x0601146E RID: 70766 RVA: 0x004F3234 File Offset: 0x004F1434
		public override AlertReport GetReport()
		{
			if (Find.AnyPlayerHomeMap == null)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.Patients);
		}

		// Token: 0x0400B321 RID: 45857
		private List<Pawn> patientsResult = new List<Pawn>();

		// Token: 0x0400B322 RID: 45858
		private StringBuilder sb = new StringBuilder();
	}
}
