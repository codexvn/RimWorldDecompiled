using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003012 RID: 12306
	public class Alert_TitleRequiresBedroom : Alert
	{
		// Token: 0x06011511 RID: 70929 RVA: 0x004F7280 File Offset: 0x004F5480
		public Alert_TitleRequiresBedroom()
		{
			this.defaultLabel = "NeedBedroomAssigned".Translate();
			this.defaultExplanation = "NeedBedroomAssignedDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x17002CE3 RID: 11491
		// (get) Token: 0x06011512 RID: 70930 RVA: 0x004F72D0 File Offset: 0x004F54D0
		public List<Pawn> Targets
		{
			get
			{
				this.targetsResult.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if (pawn.royalty != null && pawn.royalty.CanRequireBedroom() && pawn.royalty.HighestTitleWithBedroomRequirements() != null && !pawn.Suspended && !pawn.royalty.HasPersonalBedroom())
							{
								this.targetsResult.Add(pawn);
							}
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x06011513 RID: 70931 RVA: 0x004F73A8 File Offset: 0x004F55A8
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x06011514 RID: 70932 RVA: 0x004F73B8 File Offset: 0x004F55B8
		public override TaggedString GetExplanation()
		{
			string text = this.defaultExplanation;
			if (MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive)
			{
				text += "\n\n" + "RoomRequirementGracePeriodDesc".Translate(MoveColonyUtility.TitleAndRoleRequirementGracePeriodTicksLeft.TicksToDays().ToString("0.0"));
			}
			return text;
		}

		// Token: 0x0400B372 RID: 45938
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
