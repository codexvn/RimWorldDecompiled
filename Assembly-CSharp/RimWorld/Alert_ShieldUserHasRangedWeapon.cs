using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200301F RID: 12319
	public class Alert_ShieldUserHasRangedWeapon : Alert
	{
		// Token: 0x17002CE8 RID: 11496
		// (get) Token: 0x06011541 RID: 70977 RVA: 0x004F835C File Offset: 0x004F655C
		private List<Pawn> ShieldUsersWithRangedWeapon
		{
			get
			{
				this.shieldUsersWithRangedWeaponResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					ThingWithComps primary = pawn.equipment.Primary;
					if (primary != null && primary.def.IsRangedWeapon)
					{
						bool flag = false;
						List<VerbProperties> verbs = primary.def.Verbs;
						for (int i = 0; i < verbs.Count; i++)
						{
							if (typeof(Verb_LaunchProjectile).IsAssignableFrom(verbs[i].verbClass))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							List<Apparel> wornApparel = pawn.apparel.WornApparel;
							for (int j = 0; j < wornApparel.Count; j++)
							{
								if (wornApparel[j].def.IsShieldThatBlocksRanged)
								{
									this.shieldUsersWithRangedWeaponResult.Add(pawn);
									break;
								}
							}
						}
					}
				}
				return this.shieldUsersWithRangedWeaponResult;
			}
		}

		// Token: 0x06011542 RID: 70978 RVA: 0x004F8474 File Offset: 0x004F6674
		public Alert_ShieldUserHasRangedWeapon()
		{
			this.defaultLabel = "ShieldUserHasRangedWeapon".Translate();
			this.defaultExplanation = "ShieldUserHasRangedWeaponDesc".Translate();
		}

		// Token: 0x06011543 RID: 70979 RVA: 0x004F84B1 File Offset: 0x004F66B1
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ShieldUsersWithRangedWeapon);
		}

		// Token: 0x0400B385 RID: 45957
		private List<Pawn> shieldUsersWithRangedWeaponResult = new List<Pawn>();
	}
}
