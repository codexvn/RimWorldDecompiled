using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200301E RID: 12318
	public class Alert_BrawlerHasRangedWeapon : Alert
	{
		// Token: 0x17002CE7 RID: 11495
		// (get) Token: 0x0601153E RID: 70974 RVA: 0x004F826C File Offset: 0x004F646C
		private List<Pawn> BrawlersWithRangedWeapon
		{
			get
			{
				this.brawlersWithRangedWeaponResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.story.traits.HasTrait(TraitDefOf.Brawler) && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
					{
						this.brawlersWithRangedWeaponResult.Add(pawn);
					}
				}
				return this.brawlersWithRangedWeaponResult;
			}
		}

		// Token: 0x0601153F RID: 70975 RVA: 0x004F8310 File Offset: 0x004F6510
		public Alert_BrawlerHasRangedWeapon()
		{
			this.defaultLabel = "BrawlerHasRangedWeapon".Translate();
			this.defaultExplanation = "BrawlerHasRangedWeaponDesc".Translate();
		}

		// Token: 0x06011540 RID: 70976 RVA: 0x004F834D File Offset: 0x004F654D
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BrawlersWithRangedWeapon);
		}

		// Token: 0x0400B384 RID: 45956
		private List<Pawn> brawlersWithRangedWeaponResult = new List<Pawn>();
	}
}
