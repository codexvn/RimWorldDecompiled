using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02003019 RID: 12313
	public class Alert_Boredom : Alert
	{
		// Token: 0x0601152A RID: 70954 RVA: 0x004F7BB3 File Offset: 0x004F5DB3
		public Alert_Boredom()
		{
			this.defaultLabel = "Boredom".Translate();
			this.defaultPriority = AlertPriority.Medium;
		}

		// Token: 0x17002CE5 RID: 11493
		// (get) Token: 0x0601152B RID: 70955 RVA: 0x004F7BF0 File Offset: 0x004F5DF0
		private List<Pawn> BoredPawns
		{
			get
			{
				this.boredPawnsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						List<Pawn> freeColonistsSpawned = maps[i].mapPawns.FreeColonistsSpawned;
						if (freeColonistsSpawned.Any<Pawn>())
						{
							List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(maps[i]);
							try
							{
								foreach (Pawn pawn in freeColonistsSpawned)
								{
									if (pawn.needs.joy != null && (pawn.needs.joy.CurLevelPercentage < 0.24000001f || pawn.GetTimeAssignment() == TimeAssignmentDefOf.Joy) && pawn.needs.joy.tolerances.BoredOfKinds(list))
									{
										this.boredPawnsResult.Add(pawn);
									}
								}
							}
							finally
							{
								list.Clear();
							}
						}
					}
				}
				return this.boredPawnsResult;
			}
		}

		// Token: 0x0601152C RID: 70956 RVA: 0x004F7D10 File Offset: 0x004F5F10
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BoredPawns);
		}

		// Token: 0x0601152D RID: 70957 RVA: 0x004F7D20 File Offset: 0x004F5F20
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			Pawn pawn = null;
			foreach (Pawn pawn2 in this.boredPawnsResult)
			{
				this.sb.AppendLine("  - " + pawn2.NameShortColored.Resolve());
				if (pawn == null)
				{
					pawn = pawn2;
				}
			}
			string value = (pawn != null) ? JoyUtility.JoyKindsOnMapString(pawn.Map) : string.Empty;
			return "BoredomDesc".Translate(this.sb.ToString().TrimEndNewlines(), pawn.LabelShort, value, pawn.Named("PAWN"));
		}

		// Token: 0x0400B37D RID: 45949
		private const float JoyNeedThreshold = 0.24000001f;

		// Token: 0x0400B37E RID: 45950
		private List<Pawn> boredPawnsResult = new List<Pawn>();

		// Token: 0x0400B37F RID: 45951
		private StringBuilder sb = new StringBuilder();
	}
}
