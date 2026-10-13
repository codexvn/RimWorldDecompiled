using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200300F RID: 12303
	public class Alert_RoyalNoThroneAssigned : Alert
	{
		// Token: 0x06011503 RID: 70915 RVA: 0x004F6B38 File Offset: 0x004F4D38
		public Alert_RoyalNoThroneAssigned()
		{
			this.defaultLabel = "NeedThroneAssigned".Translate();
			this.defaultExplanation = "NeedThroneAssignedDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x17002CE0 RID: 11488
		// (get) Token: 0x06011504 RID: 70916 RVA: 0x004F6B88 File Offset: 0x004F4D88
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
						if (pawn.royalty != null && !pawn.Suspended && pawn.royalty.CanRequireThroneroom())
						{
							bool flag = false;
							List<RoyalTitle> allTitlesForReading = pawn.royalty.AllTitlesForReading;
							for (int j = 0; j < allTitlesForReading.Count; j++)
							{
								if (!allTitlesForReading[j].def.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
								{
									flag = true;
									break;
								}
							}
							if (flag && pawn.ownership.AssignedThrone == null)
							{
								this.targetsResult.Add(pawn);
							}
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x06011505 RID: 70917 RVA: 0x004F6C94 File Offset: 0x004F4E94
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x06011506 RID: 70918 RVA: 0x004F6CA4 File Offset: 0x004F4EA4
		public override TaggedString GetExplanation()
		{
			string text = this.defaultExplanation;
			if (MoveColonyUtility.TitleAndRoleRequirementsGracePeriodActive)
			{
				text += "\n\n" + "RoomRequirementGracePeriodDesc".Translate(MoveColonyUtility.TitleAndRoleRequirementGracePeriodTicksLeft.TicksToDays().ToString("0.0"));
			}
			return text;
		}

		// Token: 0x0400B36D RID: 45933
		private List<Pawn> targetsResult = new List<Pawn>();
	}
}
