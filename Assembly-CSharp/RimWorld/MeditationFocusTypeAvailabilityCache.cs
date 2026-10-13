using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001511 RID: 5393
	public static class MeditationFocusTypeAvailabilityCache
	{
		// Token: 0x0600808E RID: 32910 RVA: 0x0026BC6C File Offset: 0x00269E6C
		public static bool PawnCanUse(Pawn p, MeditationFocusDef type)
		{
			if (!MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached.ContainsKey(p))
			{
				MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached[p] = new Dictionary<MeditationFocusDef, bool>();
			}
			if (!MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached[p].ContainsKey(type))
			{
				MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached[p][type] = MeditationFocusTypeAvailabilityCache.PawnCanUseInt(p, type);
			}
			return MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached[p][type];
		}

		// Token: 0x0600808F RID: 32911 RVA: 0x0026BCD2 File Offset: 0x00269ED2
		public static void Notify_PawnDiedOrDestroyed(Pawn p)
		{
			MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached.Remove(p);
		}

		// Token: 0x06008090 RID: 32912 RVA: 0x0026BCE0 File Offset: 0x00269EE0
		public static void ClearFor(Pawn p)
		{
			if (MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached.ContainsKey(p))
			{
				MeditationFocusTypeAvailabilityCache.pawnCanUseMeditationTypeCached[p].Clear();
			}
		}

		// Token: 0x06008091 RID: 32913 RVA: 0x0026BD00 File Offset: 0x00269F00
		private static bool PawnCanUseInt(Pawn p, MeditationFocusDef type)
		{
			if (p.story != null)
			{
				for (int i = 0; i < p.story.traits.allTraits.Count; i++)
				{
					if (!p.story.traits.allTraits[i].Suppressed)
					{
						List<MeditationFocusDef> disallowedMeditationFocusTypes = p.story.traits.allTraits[i].CurrentData.disallowedMeditationFocusTypes;
						if (disallowedMeditationFocusTypes != null && disallowedMeditationFocusTypes.Contains(type))
						{
							return false;
						}
					}
				}
				BackstoryDef adulthood = p.story.Adulthood;
				List<string> list = (adulthood != null) ? adulthood.spawnCategories : null;
				BackstoryDef childhood = p.story.Childhood;
				List<string> list2 = (childhood != null) ? childhood.spawnCategories : null;
				for (int j = 0; j < type.incompatibleBackstoriesAny.Count; j++)
				{
					BackstoryCategoryAndSlot backstoryCategoryAndSlot = type.incompatibleBackstoriesAny[j];
					List<string> list3 = (backstoryCategoryAndSlot.slot == BackstorySlot.Adulthood) ? list : list2;
					if (list3 != null && list3.Contains(backstoryCategoryAndSlot.categoryName))
					{
						return false;
					}
				}
			}
			if (!type.requiresRoyalTitle)
			{
				if (p.story != null)
				{
					for (int k = 0; k < p.story.traits.allTraits.Count; k++)
					{
						if (!p.story.traits.allTraits[k].Suppressed)
						{
							List<MeditationFocusDef> allowedMeditationFocusTypes = p.story.traits.allTraits[k].CurrentData.allowedMeditationFocusTypes;
							if (allowedMeditationFocusTypes != null && allowedMeditationFocusTypes.Contains(type))
							{
								return true;
							}
						}
					}
					BackstoryDef adulthood2 = p.story.Adulthood;
					List<string> list4 = (adulthood2 != null) ? adulthood2.spawnCategories : null;
					BackstoryDef childhood2 = p.story.Childhood;
					List<string> list5 = (childhood2 != null) ? childhood2.spawnCategories : null;
					for (int l = 0; l < type.requiredBackstoriesAny.Count; l++)
					{
						BackstoryCategoryAndSlot backstoryCategoryAndSlot2 = type.requiredBackstoriesAny[l];
						List<string> list6 = (backstoryCategoryAndSlot2.slot == BackstorySlot.Adulthood) ? list4 : list5;
						if (list6 != null && list6.Contains(backstoryCategoryAndSlot2.categoryName))
						{
							return true;
						}
					}
				}
				Pawn_HealthTracker health = p.health;
				if (((health != null) ? health.hediffSet : null) != null)
				{
					for (int m = 0; m < p.health.hediffSet.hediffs.Count; m++)
					{
						if (p.health.hediffSet.hediffs[m].def.allowedMeditationFocusTypes.NotNullAndContains(type))
						{
							return true;
						}
					}
				}
				if (type.requiredBackstoriesAny.Count == 0)
				{
					bool flag = false;
					bool flag2 = false;
					int num = 0;
					while (num < DefDatabase<TraitDef>.AllDefsListForReading.Count && !flag)
					{
						TraitDef traitDef = DefDatabase<TraitDef>.AllDefsListForReading[num];
						for (int n = 0; n < traitDef.degreeDatas.Count; n++)
						{
							List<MeditationFocusDef> allowedMeditationFocusTypes2 = traitDef.degreeDatas[n].allowedMeditationFocusTypes;
							if (allowedMeditationFocusTypes2 != null && allowedMeditationFocusTypes2.Contains(type))
							{
								flag = true;
								break;
							}
						}
						num++;
					}
					for (int num2 = 0; num2 < DefDatabase<HediffDef>.AllDefsListForReading.Count; num2++)
					{
						if (DefDatabase<HediffDef>.AllDefsListForReading[num2].allowedMeditationFocusTypes.NotNullAndContains(type))
						{
							flag2 = true;
							break;
						}
					}
					if (!flag && !flag2)
					{
						return true;
					}
				}
				return false;
			}
			if (p.royalty != null)
			{
				return p.royalty.AllTitlesInEffectForReading.Any((RoyalTitle t) => t.def.allowDignifiedMeditationFocus);
			}
			return false;
		}

		// Token: 0x040055F5 RID: 22005
		private static Dictionary<Pawn, Dictionary<MeditationFocusDef, bool>> pawnCanUseMeditationTypeCached = new Dictionary<Pawn, Dictionary<MeditationFocusDef, bool>>();
	}
}
