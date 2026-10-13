using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003650 RID: 13904
	public static class GenStuff
	{
		// Token: 0x170031CD RID: 12749
		// (get) Token: 0x060134DF RID: 79071 RVA: 0x005BC9F0 File Offset: 0x005BABF0
		private static List<ThingDef> StuffDefs
		{
			get
			{
				if (GenStuff.cachedStuffDefs == null)
				{
					GenStuff.cachedStuffDefs = new List<ThingDef>();
					foreach (ThingDef thingDef in DefDatabase<ThingDef>.AllDefsListForReading)
					{
						if (thingDef.IsStuff)
						{
							GenStuff.cachedStuffDefs.Add(thingDef);
						}
					}
				}
				return GenStuff.cachedStuffDefs;
			}
		}

		// Token: 0x060134E0 RID: 79072 RVA: 0x005BCA64 File Offset: 0x005BAC64
		public static ThingDef DefaultStuffFor(BuildableDef bd)
		{
			if (!bd.MadeFromStuff)
			{
				return null;
			}
			ThingDef thingDef = bd as ThingDef;
			if (thingDef != null)
			{
				if (thingDef.defaultStuff != null)
				{
					return thingDef.defaultStuff;
				}
				if (thingDef.IsMeleeWeapon)
				{
					if (ThingDefOf.Steel.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Steel;
					}
					if (ThingDefOf.Plasteel.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Plasteel;
					}
				}
				if (thingDef.IsApparel)
				{
					if (ThingDefOf.Cloth.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Cloth;
					}
					if (ThingDefOf.Leather_Plain.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Leather_Plain;
					}
					if (ThingDefOf.Steel.stuffProps.CanMake(bd))
					{
						return ThingDefOf.Steel;
					}
				}
			}
			if (ThingDefOf.WoodLog.stuffProps.CanMake(bd))
			{
				return ThingDefOf.WoodLog;
			}
			if (ThingDefOf.Steel.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Steel;
			}
			if (ThingDefOf.Plasteel.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Plasteel;
			}
			if (ThingDefOf.BlocksGranite.stuffProps.CanMake(bd))
			{
				return ThingDefOf.BlocksGranite;
			}
			if (ThingDefOf.Cloth.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Cloth;
			}
			if (ThingDefOf.Leather_Plain.stuffProps.CanMake(bd))
			{
				return ThingDefOf.Leather_Plain;
			}
			return GenStuff.AllowedStuffsFor(bd, TechLevel.Undefined, false).First<ThingDef>();
		}

		// Token: 0x060134E1 RID: 79073 RVA: 0x005BCBBC File Offset: 0x005BADBC
		public static ThingDef RandomStuffFor(ThingDef td)
		{
			if (!td.MadeFromStuff)
			{
				return null;
			}
			return GenStuff.AllowedStuffsFor(td, TechLevel.Undefined, true).RandomElement<ThingDef>();
		}

		// Token: 0x060134E2 RID: 79074 RVA: 0x005BCBD8 File Offset: 0x005BADD8
		public static ThingDef RandomStuffByCommonalityFor(ThingDef td, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				return null;
			}
			ThingDef result;
			if (!GenStuff.TryRandomStuffByCommonalityFor(td, out result, maxTechLevel))
			{
				result = GenStuff.DefaultStuffFor(td);
			}
			return result;
		}

		// Token: 0x060134E3 RID: 79075 RVA: 0x005BCC02 File Offset: 0x005BAE02
		public static IEnumerable<ThingDef> AllowedStuffsFor(BuildableDef td, TechLevel maxTechLevel = TechLevel.Undefined, bool checkAllowedInStuffGeneration = false)
		{
			if (!td.MadeFromStuff)
			{
				yield break;
			}
			ThingDef found = null;
			int yielded = 0;
			int num;
			for (int i = 0; i < GenStuff.StuffDefs.Count; i = num + 1)
			{
				ThingDef thingDef = GenStuff.StuffDefs[i];
				if (thingDef.IsStuff && (maxTechLevel == TechLevel.Undefined || thingDef.techLevel <= maxTechLevel) && thingDef.stuffProps.CanMake(td))
				{
					found = thingDef;
					if (!checkAllowedInStuffGeneration || thingDef.stuffProps.allowedInStuffGeneration)
					{
						num = yielded;
						yielded = num + 1;
						yield return thingDef;
					}
				}
				num = i;
			}
			if (yielded == 0 && found != null)
			{
				yield return found;
			}
			yield break;
		}

		// Token: 0x060134E4 RID: 79076 RVA: 0x005BCC20 File Offset: 0x005BAE20
		public static IEnumerable<ThingDef> AllowedStuffs(List<StuffCategoryDef> categories, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			int num;
			for (int i = 0; i < GenStuff.StuffDefs.Count; i = num + 1)
			{
				ThingDef thingDef = GenStuff.StuffDefs[i];
				if (thingDef.IsStuff && (maxTechLevel == TechLevel.Undefined || thingDef.techLevel <= maxTechLevel))
				{
					bool flag = false;
					for (int j = 0; j < thingDef.stuffProps.categories.Count; j++)
					{
						for (int k = 0; k < categories.Count; k++)
						{
							if (thingDef.stuffProps.categories[j] == categories[k])
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						yield return thingDef;
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x060134E5 RID: 79077 RVA: 0x005BCC37 File Offset: 0x005BAE37
		public static bool TryRandomStuffByCommonalityFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined)
		{
			if (!td.MadeFromStuff)
			{
				stuff = null;
				return true;
			}
			return GenStuff.AllowedStuffsFor(td, maxTechLevel, true).TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out stuff);
		}

		// Token: 0x060134E6 RID: 79078 RVA: 0x005BCC74 File Offset: 0x005BAE74
		public static bool TryRandomStuffFor(ThingDef td, out ThingDef stuff, TechLevel maxTechLevel = TechLevel.Undefined, Predicate<ThingDef> validator = null)
		{
			if (!td.MadeFromStuff)
			{
				stuff = null;
				return true;
			}
			if (validator != null)
			{
				GenStuff.allowedStuffTmp.Clear();
				using (IEnumerator<ThingDef> enumerator = GenStuff.AllowedStuffsFor(td, maxTechLevel, true).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ThingDef thingDef = enumerator.Current;
						if (validator(thingDef))
						{
							GenStuff.allowedStuffTmp.Add(thingDef);
						}
					}
					goto IL_75;
				}
			}
			GenStuff.allowedStuffTmp.Clear();
			GenStuff.allowedStuffTmp.AddRange(GenStuff.AllowedStuffsFor(td, maxTechLevel, false));
			IL_75:
			return GenStuff.allowedStuffTmp.TryRandomElement(out stuff);
		}

		// Token: 0x060134E7 RID: 79079 RVA: 0x005BCD14 File Offset: 0x005BAF14
		public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, Faction faction, Predicate<ThingDef> validator = null)
		{
			return GenStuff.RandomStuffInexpensiveFor(thingDef, (faction != null) ? faction.def.techLevel : TechLevel.Undefined, validator);
		}

		// Token: 0x060134E8 RID: 79080 RVA: 0x005BCD30 File Offset: 0x005BAF30
		public static ThingDef RandomStuffInexpensiveFor(ThingDef thingDef, TechLevel maxTechLevel = TechLevel.Undefined, Predicate<ThingDef> validator = null)
		{
			if (!thingDef.MadeFromStuff)
			{
				return null;
			}
			IEnumerable<ThingDef> enumerable = GenStuff.AllowedStuffsFor(thingDef, maxTechLevel, true);
			if (validator != null)
			{
				enumerable = from x in enumerable
				where validator(x)
				select x;
			}
			float cheapestPrice = -1f;
			foreach (ThingDef thingDef2 in enumerable)
			{
				float num = thingDef2.BaseMarketValue / thingDef2.VolumePerUnit;
				if (cheapestPrice == -1f || num < cheapestPrice)
				{
					cheapestPrice = num;
				}
			}
			enumerable = from x in enumerable
			where x.BaseMarketValue / x.VolumePerUnit <= cheapestPrice * 4f
			select x;
			ThingDef result;
			if (enumerable.TryRandomElementByWeight((ThingDef x) => x.stuffProps.commonality, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060134E9 RID: 79081 RVA: 0x005BCE28 File Offset: 0x005BB028
		public static void ResetStaticData()
		{
			GenStuff.cachedStuffDefs = null;
		}

		// Token: 0x0400D63C RID: 54844
		private static List<ThingDef> cachedStuffDefs;

		// Token: 0x0400D63D RID: 54845
		private static List<ThingDef> allowedStuffTmp = new List<ThingDef>();
	}
}
