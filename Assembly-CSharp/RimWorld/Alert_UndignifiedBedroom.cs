using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02003011 RID: 12305
	public class Alert_UndignifiedBedroom : Alert
	{
		// Token: 0x0601150C RID: 70924 RVA: 0x004F6FBC File Offset: 0x004F51BC
		public Alert_UndignifiedBedroom()
		{
			this.defaultLabel = "UndignifiedBedroom".Translate();
			this.defaultExplanation = "UndignifiedBedroomDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x17002CE2 RID: 11490
		// (get) Token: 0x0601150D RID: 70925 RVA: 0x004F7018 File Offset: 0x004F5218
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
						if (pawn.royalty != null && !pawn.Suspended && pawn.royalty.AnyUnmetBedroomRequirements())
						{
							this.targetsResult.Add(pawn);
						}
					}
				}
				return this.targetsResult;
			}
		}

		// Token: 0x0601150E RID: 70926 RVA: 0x004F70C0 File Offset: 0x004F52C0
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0601150F RID: 70927 RVA: 0x004F70CD File Offset: 0x004F52CD
		public override TaggedString GetExplanation()
		{
			return this.defaultExplanation + "\n" + this.targetsResult.Select(delegate(Pawn t)
			{
				RoyalTitle royalTitle = t.royalty.HighestTitleWithBedroomRequirements();
				RoyalTitleDef royalTitleDef = royalTitle.RoomRequirementGracePeriodActive(t) ? royalTitle.def.GetPreviousTitle(royalTitle.faction) : royalTitle.def;
				string[] array = t.royalty.GetUnmetBedroomRequirements(false, false).ToArray<string>();
				string[] array2 = t.royalty.GetUnmetBedroomRequirements(true, true).ToArray<string>();
				bool flag = royalTitleDef != null && array.Length != 0;
				this.sb.Length = 0;
				if (flag)
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
					if (flag)
					{
						this.sb.Append("\n\n");
					}
					this.sb.Append(t.LabelShort + " (" + royalTitle.def.GetLabelFor(t.gender) + ", " + "RoomRequirementGracePeriodDesc".Translate(royalTitle.RoomRequirementGracePeriodDaysLeft.ToString("0.0")) + ")" + ":\n" + array2.ToLineList("  - "));
				}
				return this.sb.ToString();
			}).ToLineList("\n", false);
		}

		// Token: 0x0400B370 RID: 45936
		private List<Pawn> targetsResult = new List<Pawn>();

		// Token: 0x0400B371 RID: 45937
		private StringBuilder sb = new StringBuilder();
	}
}
