using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F87 RID: 12167
	public class StockGenerator_Animals : StockGenerator
	{
		// Token: 0x06011236 RID: 70198 RVA: 0x004EA1A7 File Offset: 0x004E83A7
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			StockGenerator_Animals.<>c__DisplayClass9_0 CS$<>8__locals1 = new StockGenerator_Animals.<>c__DisplayClass9_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.forTile = forTile;
			int numKinds = this.kindCountRange.RandomInRange;
			int count = this.countRange.RandomInRange;
			if (count > 1 && !this.createMatingPair.NullOrEmpty<string>())
			{
				StockGenerator_Animals.<>c__DisplayClass9_1 CS$<>8__locals2 = new StockGenerator_Animals.<>c__DisplayClass9_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.CanCreateMatingPair = delegate(PawnKindDef k)
				{
					if (k.race.tradeTags == null || CS$<>8__locals2.CS$<>8__locals1.<>4__this.createMatingPair.NullOrEmpty<string>())
					{
						return false;
					}
					for (int l = 0; l < k.race.tradeTags.Count; l++)
					{
						if (CS$<>8__locals2.CS$<>8__locals1.<>4__this.createMatingPair.Contains(k.race.tradeTags[l]))
						{
							return true;
						}
					}
					return false;
				};
				PawnKindDef matingKind;
				(from k in DefDatabase<PawnKindDef>.AllDefs
				where CS$<>8__locals2.CS$<>8__locals1.<>4__this.PawnKindAllowed(k, CS$<>8__locals2.CS$<>8__locals1.forTile) && CS$<>8__locals2.CanCreateMatingPair(k)
				select k).TryRandomElementByWeight((PawnKindDef k) => (PawnUtility.PlayerHasReproductivePair(k) ? 0.5f : 1f) * CS$<>8__locals2.CS$<>8__locals1.<>4__this.SelectionChance(k), out matingKind);
				if (matingKind != null)
				{
					PawnKindDef kind = matingKind;
					Faction faction2 = null;
					PawnGenerationContext context = PawnGenerationContext.NonPlayer;
					PlanetTile? tile = new PlanetTile?(CS$<>8__locals2.CS$<>8__locals1.forTile);
					bool forceGenerateNewPawn = false;
					bool allowDead = false;
					bool allowDowned = false;
					bool canGeneratePawnRelations = true;
					bool mustBeCapableOfViolence = false;
					float colonistRelationChanceFactor = 1f;
					bool forceAddFreeWarmLayerIfNeeded = false;
					bool allowGay = true;
					bool allowPregnant = false;
					bool allowFood = true;
					bool allowAddictions = true;
					bool inhabitant = false;
					bool certainlyBeenInCryptosleep = false;
					bool forceRedressWorldPawnIfFormerColonist = false;
					bool worldPawnFactionDoesntMatter = false;
					float biocodeWeaponChance = 0f;
					float biocodeApparelChance = 0f;
					Pawn extraPawnForExtraRelationChance = null;
					float relationWithExtraPawnChanceFactor = 1f;
					Predicate<Pawn> validatorPreGear = null;
					Predicate<Pawn> validatorPostGear = null;
					IEnumerable<TraitDef> forcedTraits = null;
					IEnumerable<TraitDef> prohibitedTraits = null;
					Gender? fixedGender = new Gender?(Gender.Female);
					PawnGenerationRequest request = new PawnGenerationRequest(kind, faction2, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, allowFood, allowAddictions, inhabitant, certainlyBeenInCryptosleep, forceRedressWorldPawnIfFormerColonist, worldPawnFactionDoesntMatter, biocodeWeaponChance, biocodeApparelChance, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, validatorPreGear, validatorPostGear, forcedTraits, prohibitedTraits, null, null, null, fixedGender, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
					yield return PawnGenerator.GeneratePawn(request);
					PawnKindDef kind2 = matingKind;
					Faction faction3 = null;
					PawnGenerationContext context2 = PawnGenerationContext.NonPlayer;
					PlanetTile? tile2 = new PlanetTile?(CS$<>8__locals2.CS$<>8__locals1.forTile);
					bool forceGenerateNewPawn2 = false;
					bool allowDead2 = false;
					bool allowDowned2 = false;
					bool canGeneratePawnRelations2 = true;
					bool mustBeCapableOfViolence2 = false;
					float colonistRelationChanceFactor2 = 1f;
					bool forceAddFreeWarmLayerIfNeeded2 = false;
					bool allowGay2 = true;
					bool allowPregnant2 = false;
					bool allowFood2 = true;
					bool allowAddictions2 = true;
					bool inhabitant2 = false;
					bool certainlyBeenInCryptosleep2 = false;
					bool forceRedressWorldPawnIfFormerColonist2 = false;
					bool worldPawnFactionDoesntMatter2 = false;
					float biocodeWeaponChance2 = 0f;
					float biocodeApparelChance2 = 0f;
					Pawn extraPawnForExtraRelationChance2 = null;
					float relationWithExtraPawnChanceFactor2 = 1f;
					Predicate<Pawn> validatorPreGear2 = null;
					Predicate<Pawn> validatorPostGear2 = null;
					IEnumerable<TraitDef> forcedTraits2 = null;
					IEnumerable<TraitDef> prohibitedTraits2 = null;
					fixedGender = new Gender?(Gender.Male);
					PawnGenerationRequest request2 = new PawnGenerationRequest(kind2, faction3, context2, tile2, forceGenerateNewPawn2, allowDead2, allowDowned2, canGeneratePawnRelations2, mustBeCapableOfViolence2, colonistRelationChanceFactor2, forceAddFreeWarmLayerIfNeeded2, allowGay2, allowPregnant2, allowFood2, allowAddictions2, inhabitant2, certainlyBeenInCryptosleep2, forceRedressWorldPawnIfFormerColonist2, worldPawnFactionDoesntMatter2, biocodeWeaponChance2, biocodeApparelChance2, extraPawnForExtraRelationChance2, relationWithExtraPawnChanceFactor2, validatorPreGear2, validatorPostGear2, forcedTraits2, prohibitedTraits2, null, null, null, fixedGender, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
					yield return PawnGenerator.GeneratePawn(request2);
					count -= 2;
				}
				CS$<>8__locals2 = null;
				matingKind = null;
			}
			if (count <= 0)
			{
				yield break;
			}
			CS$<>8__locals1.kinds = new List<PawnKindDef>();
			for (int j = 0; j < numKinds; j++)
			{
				IEnumerable<PawnKindDef> allDefs = DefDatabase<PawnKindDef>.AllDefs;
				Func<PawnKindDef, bool> predicate;
				if ((predicate = CS$<>8__locals1.<>9__3) == null)
				{
					predicate = (CS$<>8__locals1.<>9__3 = ((PawnKindDef k) => !CS$<>8__locals1.kinds.Contains(k) && CS$<>8__locals1.<>4__this.PawnKindAllowed(k, CS$<>8__locals1.forTile)));
				}
				IEnumerable<PawnKindDef> source = allDefs.Where(predicate);
				Func<PawnKindDef, float> weightSelector;
				if ((weightSelector = CS$<>8__locals1.<>9__4) == null)
				{
					weightSelector = (CS$<>8__locals1.<>9__4 = ((PawnKindDef k) => CS$<>8__locals1.<>4__this.SelectionChance(k)));
				}
				PawnKindDef item;
				if (!source.TryRandomElementByWeight(weightSelector, out item))
				{
					break;
				}
				CS$<>8__locals1.kinds.Add(item);
			}
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				PawnKindDef pawnKindDef;
				if (!CS$<>8__locals1.kinds.TryRandomElement(out pawnKindDef))
				{
					yield break;
				}
				PawnKindDef kind3 = pawnKindDef;
				Faction faction4 = null;
				PawnGenerationContext context3 = PawnGenerationContext.NonPlayer;
				PlanetTile? tile3 = new PlanetTile?(CS$<>8__locals1.forTile);
				bool forceGenerateNewPawn3 = false;
				bool allowDead3 = false;
				bool allowDowned3 = false;
				bool canGeneratePawnRelations3 = true;
				bool mustBeCapableOfViolence3 = false;
				float colonistRelationChanceFactor3 = 1f;
				bool forceAddFreeWarmLayerIfNeeded3 = false;
				bool allowGay3 = true;
				bool allowPregnant3 = false;
				bool allowFood3 = true;
				bool allowAddictions3 = true;
				bool inhabitant3 = false;
				bool certainlyBeenInCryptosleep3 = false;
				bool forceRedressWorldPawnIfFormerColonist3 = false;
				bool worldPawnFactionDoesntMatter3 = false;
				float biocodeWeaponChance3 = 0f;
				float biocodeApparelChance3 = 0f;
				Pawn extraPawnForExtraRelationChance3 = null;
				float relationWithExtraPawnChanceFactor3 = 1f;
				Predicate<Pawn> validatorPreGear3 = null;
				Predicate<Pawn> validatorPostGear3 = null;
				IEnumerable<TraitDef> forcedTraits3 = null;
				IEnumerable<TraitDef> prohibitedTraits3 = null;
				float? minChanceToRedressWorldPawn = null;
				float? fixedBiologicalAge = null;
				float? fixedChronologicalAge = null;
				Gender? fixedGender = null;
				PawnGenerationRequest request3 = new PawnGenerationRequest(kind3, faction4, context3, tile3, forceGenerateNewPawn3, allowDead3, allowDowned3, canGeneratePawnRelations3, mustBeCapableOfViolence3, colonistRelationChanceFactor3, forceAddFreeWarmLayerIfNeeded3, allowGay3, allowPregnant3, allowFood3, allowAddictions3, inhabitant3, certainlyBeenInCryptosleep3, forceRedressWorldPawnIfFormerColonist3, worldPawnFactionDoesntMatter3, biocodeWeaponChance3, biocodeApparelChance3, extraPawnForExtraRelationChance3, relationWithExtraPawnChanceFactor3, validatorPreGear3, validatorPostGear3, forcedTraits3, prohibitedTraits3, minChanceToRedressWorldPawn, fixedBiologicalAge, fixedChronologicalAge, fixedGender, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
				yield return PawnGenerator.GeneratePawn(request3);
				num = i;
			}
			yield break;
		}

		// Token: 0x06011237 RID: 70199 RVA: 0x004EA1BE File Offset: 0x004E83BE
		private float SelectionChance(PawnKindDef k)
		{
			return StockGenerator_Animals.SelectionChanceFromWildnessCurve.Evaluate(k.race.GetStatValueAbstract(StatDefOf.Wildness, null));
		}

		// Token: 0x06011238 RID: 70200 RVA: 0x004EA1DC File Offset: 0x004E83DC
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Animal && thingDef.tradeability != Tradeability.None && (this.tradeTagsSell.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)) || this.tradeTagsBuy.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)));
		}

		// Token: 0x06011239 RID: 70201 RVA: 0x004EA258 File Offset: 0x004E8458
		public override Tradeability TradeabilityFor(ThingDef thingDef)
		{
			if (!this.HandlesThingDef(thingDef))
			{
				return Tradeability.None;
			}
			bool flag = false;
			bool flag2 = false;
			if ((thingDef.tradeability == Tradeability.All || thingDef.tradeability == Tradeability.Buyable) && this.tradeTagsSell.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)))
			{
				flag = true;
			}
			if ((thingDef.tradeability == Tradeability.All || thingDef.tradeability == Tradeability.Sellable) && this.tradeTagsBuy.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)))
			{
				flag2 = true;
			}
			if (flag2 && flag)
			{
				return Tradeability.All;
			}
			if (flag2)
			{
				return Tradeability.Sellable;
			}
			if (flag)
			{
				return Tradeability.Buyable;
			}
			return Tradeability.None;
		}

		// Token: 0x0601123A RID: 70202 RVA: 0x004EA308 File Offset: 0x004E8508
		private bool PawnKindAllowed(PawnKindDef kind, PlanetTile forTile)
		{
			float statValueAbstract = kind.race.GetStatValueAbstract(StatDefOf.Wildness, null);
			if (!kind.RaceProps.Animal || statValueAbstract < this.minWildness || statValueAbstract > this.maxWildness || statValueAbstract > 1f)
			{
				return false;
			}
			if (this.checkTemperature)
			{
				PlanetTile tile = forTile;
				if (!tile.Valid && Find.AnyPlayerHomeMap != null)
				{
					tile = Find.AnyPlayerHomeMap.Tile;
				}
				if (tile.Valid && !Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, kind.race))
				{
					return false;
				}
			}
			return kind.race.tradeTags != null && this.tradeTagsSell.Any((string x) => kind.race.tradeTags.Contains(x)) && kind.race.tradeability.TraderCanSell();
		}

		// Token: 0x0601123B RID: 70203 RVA: 0x004EA400 File Offset: 0x004E8600
		public void LogAnimalChances()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (PawnKindDef pawnKindDef in DefDatabase<PawnKindDef>.AllDefs)
			{
				stringBuilder.AppendLine(pawnKindDef.defName + ": " + this.SelectionChance(pawnKindDef).ToString("F2"));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0601123C RID: 70204 RVA: 0x004EA484 File Offset: 0x004E8684
		[DebugOutput]
		private static void StockGenerationAnimals()
		{
			new StockGenerator_Animals
			{
				tradeTagsSell = new List<string>(),
				tradeTagsSell = 
				{
					"AnimalCommon",
					"AnimalUncommon"
				}
			}.LogAnimalChances();
		}

		// Token: 0x17002C55 RID: 11349
		// (get) Token: 0x0601123D RID: 70205 RVA: 0x004EA4BB File Offset: 0x004E86BB
		public IEnumerable<string> AllRelevantTradeTags
		{
			get
			{
				return this.tradeTagsSell.Concat(this.tradeTagsBuy).Concat(this.createMatingPair);
			}
		}

		// Token: 0x0601123E RID: 70206 RVA: 0x004EA4DC File Offset: 0x004E86DC
		public string GetInterestInTradeTag(string tag)
		{
			bool flag = this.tradeTagsBuy.Contains(tag);
			bool flag2 = this.tradeTagsSell.Contains(tag);
			if (flag && flag2)
			{
				return "Buy/Sell";
			}
			if (flag)
			{
				return "Buy";
			}
			if (flag2)
			{
				return "Sell";
			}
			return "-";
		}

		// Token: 0x0400B1C9 RID: 45513
		[NoTranslate]
		private List<string> tradeTagsSell = new List<string>();

		// Token: 0x0400B1CA RID: 45514
		[NoTranslate]
		private List<string> tradeTagsBuy = new List<string>();

		// Token: 0x0400B1CB RID: 45515
		[NoTranslate]
		private List<string> createMatingPair = new List<string>();

		// Token: 0x0400B1CC RID: 45516
		private IntRange kindCountRange = new IntRange(1, 1);

		// Token: 0x0400B1CD RID: 45517
		private float minWildness;

		// Token: 0x0400B1CE RID: 45518
		private float maxWildness = 1f;

		// Token: 0x0400B1CF RID: 45519
		private bool checkTemperature = true;

		// Token: 0x0400B1D0 RID: 45520
		private static readonly SimpleCurve SelectionChanceFromWildnessCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 100f),
				true
			},
			{
				new CurvePoint(0.25f, 60f),
				true
			},
			{
				new CurvePoint(0.5f, 30f),
				true
			},
			{
				new CurvePoint(0.75f, 12f),
				true
			},
			{
				new CurvePoint(1f, 2f),
				true
			}
		};

		// Token: 0x0400B1D1 RID: 45521
		private const float SelectionChanceFactorIfExistingMatingPair = 0.5f;
	}
}
