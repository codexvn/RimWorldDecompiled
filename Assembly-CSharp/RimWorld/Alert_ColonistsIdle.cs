using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200300C RID: 12300
	public class Alert_ColonistsIdle : Alert
	{
		// Token: 0x17002CDD RID: 11485
		// (get) Token: 0x060114F6 RID: 70902 RVA: 0x004F6598 File Offset: 0x004F4798
		private List<Pawn> IdleColonists
		{
			get
			{
				this.idleColonistsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if (pawn.mindState.IsIdle && !pawn.IsQuestLodger())
							{
								if (pawn.royalty != null)
								{
									RoyalTitle mostSeniorTitle = pawn.royalty.MostSeniorTitle;
									if (mostSeniorTitle == null || !mostSeniorTitle.def.suppressIdleAlert)
									{
										this.idleColonistsResult.Add(pawn);
									}
								}
								else
								{
									this.idleColonistsResult.Add(pawn);
								}
							}
						}
					}
				}
				return this.idleColonistsResult;
			}
		}

		// Token: 0x060114F7 RID: 70903 RVA: 0x004F6684 File Offset: 0x004F4884
		public override string GetLabel()
		{
			if (this.idleColonistsResult.Count == 1)
			{
				return "ColonistIdle".Translate();
			}
			return "ColonistsIdle".Translate(this.idleColonistsResult.Count.ToStringCached());
		}

		// Token: 0x060114F8 RID: 70904 RVA: 0x004F66D4 File Offset: 0x004F48D4
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.idleColonistsResult)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "ColonistsIdleDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x060114F9 RID: 70905 RVA: 0x004F6770 File Offset: 0x004F4970
		public override AlertReport GetReport()
		{
			if (GenDate.DaysPassed < 1)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.IdleColonists);
		}

		// Token: 0x0400B366 RID: 45926
		public const int MinDaysPassed = 1;

		// Token: 0x0400B367 RID: 45927
		private List<Pawn> idleColonistsResult = new List<Pawn>();

		// Token: 0x0400B368 RID: 45928
		private StringBuilder sb = new StringBuilder();
	}
}
