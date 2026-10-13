using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02002664 RID: 9828
	public class PreceptWorker_Apparel : PreceptWorker
	{
		// Token: 0x0600D799 RID: 55193 RVA: 0x003FF0D8 File Offset: 0x003FD2D8
		public override bool ShouldSkipThing(Ideo ideo, ThingDef thingDef)
		{
			foreach (Precept precept in ideo.PreceptsListForReading)
			{
				Precept_Apparel precept_Apparel = precept as Precept_Apparel;
				if (precept_Apparel != null && precept_Apparel.apparelDef == thingDef)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D79A RID: 55194 RVA: 0x003FF13C File Offset: 0x003FD33C
		public override float GetThingOrder(PreceptThingChance thingChance)
		{
			if (thingChance.def.IsApparel)
			{
				return (float)((thingChance.def.apparel.LastLayer == ApparelLayerDefOf.Overhead || thingChance.def.apparel.LastLayer == ApparelLayerDefOf.EyeCover) ? 1 : 2);
			}
			return 0f;
		}

		// Token: 0x0600D79B RID: 55195 RVA: 0x003FF190 File Offset: 0x003FD390
		private bool IsValidApparel(ThingDef td)
		{
			if (!td.IsApparel || !td.MadeFromStuff)
			{
				return false;
			}
			if (!td.apparel.canBeDesiredForIdeo)
			{
				return false;
			}
			if (td.thingCategories != null && (td.thingCategories.Contains(ThingCategoryDefOf.ArmorHeadgear) || td.thingCategories.Contains(ThingCategoryDefOf.ApparelArmor)))
			{
				return false;
			}
			for (int i = 0; i < this.def.comps.Count; i++)
			{
				PreceptComp_Apparel preceptComp_Apparel = this.def.comps[i] as PreceptComp_Apparel;
				if (preceptComp_Apparel != null && !preceptComp_Apparel.CanApplyToApparel(td))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600D79C RID: 55196 RVA: 0x003FF230 File Offset: 0x003FD430
		public override AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
		{
			PreceptWorker_Apparel.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.def = def;
			if (this.IsValidApparel(CS$<>8__locals1.def))
			{
				if (generatingFor != null)
				{
					AcceptanceReport result = PreceptWorker_Apparel.<CanUse>g__CheckFaction|3_0(generatingFor, ref CS$<>8__locals1);
					if (!result.Accepted)
					{
						return result;
					}
				}
				if (Find.World != null && Find.FactionManager != null)
				{
					foreach (Faction faction in Find.FactionManager.AllFactions)
					{
						if (faction.def != generatingFor && faction.ideos != null && faction.ideos.PrimaryIdeo == ideo)
						{
							AcceptanceReport acceptanceReport = PreceptWorker_Apparel.<CanUse>g__CheckFaction|3_0(faction.def, ref CS$<>8__locals1);
							if (!acceptanceReport)
							{
								return acceptanceReport;
							}
						}
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x17002104 RID: 8452
		// (get) Token: 0x0600D79D RID: 55197 RVA: 0x003FF308 File Offset: 0x003FD508
		public override IEnumerable<PreceptThingChance> ThingDefs
		{
			get
			{
				foreach (ThingDef def in from x in DefDatabase<ThingDef>.AllDefs
				where this.IsValidApparel(x)
				select x)
				{
					yield return new PreceptThingChance
					{
						def = def,
						chance = 1f
					};
				}
				IEnumerator<ThingDef> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x0600D79F RID: 55199 RVA: 0x003FF318 File Offset: 0x003FD518
		[CompilerGenerated]
		internal static AcceptanceReport <CanUse>g__CheckFaction|3_0(FactionDef faction, ref PreceptWorker_Apparel.<>c__DisplayClass3_0 A_1)
		{
			if (A_1.def.apparel.ideoDesireAllowedFactionCategoryTags != null && !A_1.def.apparel.ideoDesireAllowedFactionCategoryTags.Contains(faction.categoryTag))
			{
				return new AcceptanceReport("RoleApparelRequirementIncompatibleFaction".Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle((from t in A_1.def.apparel.ideoDesireAllowedFactionCategoryTags
				select ("RoleApparelRequirementIncompatibleFaction_Allowed_" + t).Translate().Resolve()).ToCommaListOr(false), false, false)));
			}
			if (A_1.def.apparel.ideoDesireDisallowedFactionCategoryTags != null && A_1.def.apparel.ideoDesireDisallowedFactionCategoryTags.Contains(faction.categoryTag))
			{
				return new AcceptanceReport("RoleApparelRequirementIncompatibleFaction".Translate(Find.ActiveLanguageWorker.WithIndefiniteArticle((from t in A_1.def.apparel.ideoDesireDisallowedFactionCategoryTags
				select ("RoleApparelRequirementIncompatibleFaction_Disallowed_" + t).Translate().Resolve()).ToCommaListOr(false), false, false)));
			}
			return true;
		}
	}
}
