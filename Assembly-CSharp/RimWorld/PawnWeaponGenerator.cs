using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002370 RID: 9072
	public static class PawnWeaponGenerator
	{
		// Token: 0x17001DE2 RID: 7650
		// (get) Token: 0x0600C4A7 RID: 50343 RVA: 0x00393251 File Offset: 0x00391451
		public static List<ThingStuffPair> AllWeaponPairs
		{
			get
			{
				return PawnWeaponGenerator.allWeaponPairs;
			}
		}

		// Token: 0x0600C4A8 RID: 50344 RVA: 0x00393258 File Offset: 0x00391458
		public static void Reset()
		{
			PawnWeaponGenerator.allWeaponPairs = ThingStuffPair.AllWith(new Predicate<ThingDef>(PawnWeaponGenerator.<Reset>g__IsWeapon|6_0));
			using (IEnumerator<ThingDef> enumerator = DefDatabase<ThingDef>.AllDefs.Where(new Func<ThingDef, bool>(PawnWeaponGenerator.<Reset>g__IsWeapon|6_0)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ThingDef thingDef = enumerator.Current;
					float num = (from pa in PawnWeaponGenerator.allWeaponPairs
					where pa.thing == thingDef
					select pa).Sum((ThingStuffPair pa) => pa.Commonality);
					if (num != 0f)
					{
						float num2 = thingDef.generateCommonality / num;
						if (!Mathf.Approximately(num2, 1f))
						{
							for (int i = 0; i < PawnWeaponGenerator.allWeaponPairs.Count; i++)
							{
								ThingStuffPair thingStuffPair = PawnWeaponGenerator.allWeaponPairs[i];
								if (thingStuffPair.thing == thingDef)
								{
									PawnWeaponGenerator.allWeaponPairs[i] = new ThingStuffPair(thingStuffPair.thing, thingStuffPair.stuff, thingStuffPair.commonalityMultiplier * num2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600C4A9 RID: 50345 RVA: 0x00393394 File Offset: 0x00391594
		public static void TryGenerateWeaponFor(Pawn pawn, PawnGenerationRequest request)
		{
			PawnWeaponGenerator.workingWeapons.Clear();
			if (pawn.kindDef.weaponTags == null || pawn.kindDef.weaponTags.Count == 0)
			{
				return;
			}
			if (!pawn.RaceProps.ToolUser)
			{
				return;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return;
			}
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return;
			}
			float randomInRange = pawn.kindDef.weaponMoney.RandomInRange;
			for (int i = 0; i < PawnWeaponGenerator.allWeaponPairs.Count; i++)
			{
				ThingStuffPair w = PawnWeaponGenerator.allWeaponPairs[i];
				if (w.Price <= randomInRange && (pawn.kindDef.weaponTags == null || pawn.kindDef.weaponTags.Any((string tag) => w.thing.weaponTags.Contains(tag))) && (pawn.kindDef.weaponStuffOverride == null || w.stuff == pawn.kindDef.weaponStuffOverride) && (!w.thing.IsRangedWeapon || !pawn.WorkTagIsDisabled(WorkTags.Shooting)) && (w.stuff == null || w.stuff.stuffProps.allowedInStuffGeneration) && (w.thing.generateAllowChance >= 1f || Rand.ChanceSeeded(w.thing.generateAllowChance, pawn.thingIDNumber ^ (int)w.thing.shortHash ^ 28554824)))
				{
					PawnWeaponGenerator.workingWeapons.Add(w);
				}
			}
			if (PawnWeaponGenerator.workingWeapons.Count == 0)
			{
				return;
			}
			pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
			ThingStuffPair thingStuffPair;
			if (PawnWeaponGenerator.workingWeapons.TryRandomElementByWeight((ThingStuffPair w) => PawnWeaponGenerator.GetCommonality(pawn, w), out thingStuffPair))
			{
				ThingWithComps thingWithComps = (ThingWithComps)ThingMaker.MakeThing(thingStuffPair.thing, thingStuffPair.stuff);
				PawnGenerator.PostProcessGeneratedGear(thingWithComps, pawn);
				CompEquippable compEquippable = thingWithComps.TryGetComp<CompEquippable>();
				if (compEquippable != null)
				{
					if (pawn.kindDef.weaponStyleDef != null)
					{
						compEquippable.parent.StyleDef = pawn.kindDef.weaponStyleDef;
					}
					else if (pawn.Ideo != null)
					{
						compEquippable.parent.StyleDef = pawn.Ideo.GetStyleFor(thingWithComps.def);
					}
				}
				float num = (request.BiocodeWeaponChance > 0f) ? request.BiocodeWeaponChance : pawn.kindDef.biocodeWeaponChance;
				if (Rand.Value < num)
				{
					CompBiocodable compBiocodable = thingWithComps.TryGetComp<CompBiocodable>();
					if (compBiocodable != null)
					{
						compBiocodable.CodeFor(pawn);
					}
				}
				pawn.equipment.AddEquipment(thingWithComps);
			}
			PawnWeaponGenerator.workingWeapons.Clear();
		}

		// Token: 0x0600C4AA RID: 50346 RVA: 0x003936D0 File Offset: 0x003918D0
		private static float GetCommonality(Pawn pawn, ThingStuffPair pair)
		{
			return pair.Commonality * pair.Price * PawnWeaponGenerator.GetWeaponCommonalityFromIdeo(pawn, pair) * PawnWeaponGenerator.GetWeaponCommonalityFromXenotype(pawn, pair);
		}

		// Token: 0x0600C4AB RID: 50347 RVA: 0x003936F4 File Offset: 0x003918F4
		private static float GetWeaponCommonalityFromIdeo(Pawn pawn, ThingStuffPair pair)
		{
			if (pawn.Ideo == null)
			{
				return 1f;
			}
			switch (pawn.Ideo.GetDispositionForWeapon(pair.thing))
			{
			case IdeoWeaponDisposition.Noble:
				return 100f;
			case IdeoWeaponDisposition.Despised:
				return 0.001f;
			}
			return 1f;
		}

		// Token: 0x0600C4AC RID: 50348 RVA: 0x00393748 File Offset: 0x00391948
		private static float GetWeaponCommonalityFromXenotype(Pawn pawn, ThingStuffPair pair)
		{
			Pawn_GeneTracker genes = pawn.genes;
			bool flag;
			if (genes == null)
			{
				flag = (null != null);
			}
			else
			{
				XenotypeDef xenotype = genes.Xenotype;
				flag = (((xenotype != null) ? xenotype.forbiddenWeaponClasses : null) != null);
			}
			if (flag && !pair.thing.weaponClasses.NullOrEmpty<WeaponClassDef>())
			{
				foreach (WeaponClassDef item in pawn.genes.Xenotype.forbiddenWeaponClasses)
				{
					if (pair.thing.weaponClasses.Contains(item))
					{
						return 0f;
					}
				}
			}
			return 1f;
		}

		// Token: 0x0600C4AD RID: 50349 RVA: 0x003937F4 File Offset: 0x003919F4
		public static bool IsDerpWeapon(ThingDef thing, ThingDef stuff)
		{
			if (stuff == null)
			{
				return false;
			}
			if (thing.IsMeleeWeapon)
			{
				if (thing.tools.NullOrEmpty<Tool>())
				{
					return false;
				}
				DamageDef damageDef = ThingUtility.PrimaryMeleeWeaponDamageType(thing);
				if (damageDef == null)
				{
					return false;
				}
				DamageArmorCategoryDef armorCategory = damageDef.armorCategory;
				if (armorCategory != null && armorCategory.multStat != null && stuff.GetStatValueAbstract(armorCategory.multStat, null) < 0.7f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C4AE RID: 50350 RVA: 0x00393854 File Offset: 0x00391A54
		public static float CheapestNonDerpPriceFor(ThingDef weaponDef)
		{
			float num = 9999999f;
			for (int i = 0; i < PawnWeaponGenerator.allWeaponPairs.Count; i++)
			{
				ThingStuffPair thingStuffPair = PawnWeaponGenerator.allWeaponPairs[i];
				if (thingStuffPair.thing == weaponDef && !PawnWeaponGenerator.IsDerpWeapon(thingStuffPair.thing, thingStuffPair.stuff) && thingStuffPair.Price < num)
				{
					num = thingStuffPair.Price;
				}
			}
			return num;
		}

		// Token: 0x0600C4AF RID: 50351 RVA: 0x003938B8 File Offset: 0x00391AB8
		[DebugOutput]
		private static void WeaponPairs()
		{
			IEnumerable<ThingStuffPair> dataSources = from p in PawnWeaponGenerator.allWeaponPairs
			orderby p.thing.defName descending
			select p;
			TableDataGetter<ThingStuffPair>[] array = new TableDataGetter<ThingStuffPair>[7];
			array[0] = new TableDataGetter<ThingStuffPair>("thing", (ThingStuffPair p) => p.thing.defName);
			array[1] = new TableDataGetter<ThingStuffPair>("stuff", delegate(ThingStuffPair p)
			{
				if (p.stuff == null)
				{
					return "";
				}
				return p.stuff.defName;
			});
			array[2] = new TableDataGetter<ThingStuffPair>("price", (ThingStuffPair p) => p.Price.ToString());
			array[3] = new TableDataGetter<ThingStuffPair>("commonality", (ThingStuffPair p) => p.Commonality.ToString("F5"));
			array[4] = new TableDataGetter<ThingStuffPair>("commMult", (ThingStuffPair p) => p.commonalityMultiplier.ToString("F5"));
			array[5] = new TableDataGetter<ThingStuffPair>("generateCommonality", (ThingStuffPair p) => p.thing.generateCommonality.ToString("F2"));
			array[6] = new TableDataGetter<ThingStuffPair>("derp", delegate(ThingStuffPair p)
			{
				if (!PawnWeaponGenerator.IsDerpWeapon(p.thing, p.stuff))
				{
					return "";
				}
				return "D";
			});
			DebugTables.MakeTablesDialog<ThingStuffPair>(dataSources, array);
		}

		// Token: 0x0600C4B0 RID: 50352 RVA: 0x00393A2D File Offset: 0x00391C2D
		[DebugOutput]
		private static void WeaponPairsByThing()
		{
			DebugOutputsGeneral.MakeTablePairsByThing(PawnWeaponGenerator.allWeaponPairs);
		}

		// Token: 0x0600C4B2 RID: 50354 RVA: 0x00393A45 File Offset: 0x00391C45
		[CompilerGenerated]
		internal static bool <Reset>g__IsWeapon|6_0(ThingDef td)
		{
			return td.equipmentType == EquipmentType.Primary && !td.weaponTags.NullOrEmpty<string>();
		}

		// Token: 0x04008628 RID: 34344
		private static List<ThingStuffPair> allWeaponPairs;

		// Token: 0x04008629 RID: 34345
		private static List<ThingStuffPair> workingWeapons = new List<ThingStuffPair>();

		// Token: 0x0400862A RID: 34346
		private const float WeaponSelectFactor_NobleByIdeo = 100f;

		// Token: 0x0400862B RID: 34347
		private const float WeaponSelectFactor_DespisedByIdeo = 0.001f;
	}
}
