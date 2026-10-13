using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003003 RID: 12291
	public class Alert_SlavesUnsuppressed : Alert
	{
		// Token: 0x17002CD6 RID: 11478
		// (get) Token: 0x060114D2 RID: 70866 RVA: 0x004F5C50 File Offset: 0x004F3E50
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
					{
						Need_Suppression need_Suppression;
						if (!pawn.Suspended && pawn.IsSlave && pawn.needs.TryGetNeed<Need_Suppression>(out need_Suppression) && need_Suppression.IsHigh)
						{
							this.targetsResult.Add(pawn);
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x060114D3 RID: 70867 RVA: 0x004F5D04 File Offset: 0x004F3F04
		public Alert_SlavesUnsuppressed()
		{
			this.defaultLabel = "SlavesUnsuppressedLabel".Translate();
			this.defaultExplanation = "SlavesUnsuppressedDesc".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireIdeology = true;
		}

		// Token: 0x060114D4 RID: 70868 RVA: 0x004F5D5A File Offset: 0x004F3F5A
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x060114D5 RID: 70869 RVA: 0x004F5D67 File Offset: 0x004F3F67
		public override TaggedString GetExplanation()
		{
			return "SlavesUnsuppressedDesc".Translate(this.Targets[0]);
		}

		// Token: 0x0400B356 RID: 45910
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
