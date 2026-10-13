using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003027 RID: 12327
	public class Alert_HunterHasShieldAndRangedWeapon : Alert
	{
		// Token: 0x17002CF0 RID: 11504
		// (get) Token: 0x0601155A RID: 71002 RVA: 0x004F8B24 File Offset: 0x004F6D24
		private List<Pawn> BadHunters
		{
			get
			{
				this.badHuntersResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.workSettings.WorkIsActive(WorkTypeDefOf.Hunting) && WorkGiver_HunterHunt.HasShieldAndRangedWeapon(pawn))
					{
						this.badHuntersResult.Add(pawn);
					}
				}
				return this.badHuntersResult;
			}
		}

		// Token: 0x0601155B RID: 71003 RVA: 0x004F8BA8 File Offset: 0x004F6DA8
		public Alert_HunterHasShieldAndRangedWeapon()
		{
			this.defaultLabel = "HunterHasShieldAndRangedWeapon".Translate();
			this.defaultExplanation = "HunterHasShieldAndRangedWeaponDesc".Translate();
		}

		// Token: 0x0601155C RID: 71004 RVA: 0x004F8BE5 File Offset: 0x004F6DE5
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadHunters);
		}

		// Token: 0x0400B38E RID: 45966
		private List<Pawn> badHuntersResult = new List<Pawn>();
	}
}
