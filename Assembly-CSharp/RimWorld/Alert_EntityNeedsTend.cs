using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FFD RID: 12285
	public class Alert_EntityNeedsTend : Alert
	{
		// Token: 0x060114BA RID: 70842 RVA: 0x004F5369 File Offset: 0x004F3569
		public Alert_EntityNeedsTend()
		{
			this.defaultLabel = "EntityNeedsTreatment".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x17002CD0 RID: 11472
		// (get) Token: 0x060114BB RID: 70843 RVA: 0x004F53A4 File Offset: 0x004F35A4
		private List<Pawn> NeedingEntities
		{
			get
			{
				this.needingEntitiesResult.Clear();
				foreach (Map map in Find.Maps)
				{
					foreach (Thing potentialPlatform in map.listerThings.ThingsInGroup(ThingRequestGroup.EntityHolder))
					{
						Pawn tendableEnityFromPotentialPlatform = WorkGiver_TendEntity.GetTendableEnityFromPotentialPlatform(potentialPlatform);
						if (tendableEnityFromPotentialPlatform != null && HealthAIUtility.ShouldBeTendedNowByPlayerUrgent(tendableEnityFromPotentialPlatform))
						{
							this.needingEntitiesResult.Add(tendableEnityFromPotentialPlatform);
						}
					}
				}
				return this.needingEntitiesResult;
			}
		}

		// Token: 0x060114BC RID: 70844 RVA: 0x004F545C File Offset: 0x004F365C
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.needingEntitiesResult)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "EntityNeedsTreatmentDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x060114BD RID: 70845 RVA: 0x004F54F8 File Offset: 0x004F36F8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.NeedingEntities);
		}

		// Token: 0x0400B34C RID: 45900
		private List<Pawn> needingEntitiesResult = new List<Pawn>();

		// Token: 0x0400B34D RID: 45901
		private StringBuilder sb = new StringBuilder();
	}
}
