using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02003010 RID: 12304
	public class Alert_UndignifiedThroneroom : Alert
	{
		// Token: 0x06011507 RID: 70919 RVA: 0x004F6D04 File Offset: 0x004F4F04
		public Alert_UndignifiedThroneroom()
		{
			this.defaultLabel = "UndignifiedThroneroom".Translate();
			this.defaultExplanation = "UndignifiedThroneroomDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x17002CE1 RID: 11489
		// (get) Token: 0x06011508 RID: 70920 RVA: 0x004F6D60 File Offset: 0x004F4F60
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
						if (pawn.royalty != null && !pawn.Suspended && pawn.royalty.AnyUnmetThroneroomRequirements())
						{
							this.targetsResult.Add(pawn);
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x06011509 RID: 70921 RVA: 0x004F6E08 File Offset: 0x004F5008
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0601150A RID: 70922 RVA: 0x004F6E15 File Offset: 0x004F5015
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + "\n" + this.targetsResult.Select(delegate(Pawn t)
			{
				RoyalTitle royalTitle = t.royalty.HighestTitleWithThroneRoomRequirements();
				RoyalTitleDef royalTitleDef = royalTitle.RoomRequirementGracePeriodActive(t) ? royalTitle.def.GetPreviousTitle(royalTitle.faction) : royalTitle.def;
				string[] array = t.royalty.GetUnmetThroneroomRequirements(false, false).ToArray<string>();
				string[] array2 = t.royalty.GetUnmetThroneroomRequirements(true, true).ToArray<string>();
				this.sb.Length = 0;
				if (array.Length != 0)
				{
					this.sb.Append(string.Concat(new string[]
					{
						t.LabelShort,
						" (",
						royalTitleDef.GetLabelFor(t.gender),
						"):\n",
						array.ToLineList("  - ")
					}));
				}
				if (array2.Length != 0)
				{
					if (array.Length != 0)
					{
						this.sb.Append("\n\n");
					}
					this.sb.Append(t.LabelShort + " (" + royalTitle.def.GetLabelFor(t.gender) + ", " + "RoomRequirementGracePeriodDesc".Translate(royalTitle.RoomRequirementGracePeriodDaysLeft.ToString("0.0")) + ")" + ":\n" + array2.ToLineList("  - "));
				}
				return this.sb.ToString();
			}).ToLineList("\n", false);
		}

		// Token: 0x0400B36E RID: 45934
		private List<Pawn> targetsResult = new List<Pawn>();

		// Token: 0x0400B36F RID: 45935
		private StringBuilder sb = new StringBuilder();
	}
}
