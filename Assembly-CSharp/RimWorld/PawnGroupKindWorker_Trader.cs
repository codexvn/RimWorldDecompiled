using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200237A RID: 9082
	public class PawnGroupKindWorker_Trader : PawnGroupKindWorker
	{
		// Token: 0x0600C4DB RID: 50395 RVA: 0x00006AAC File Offset: 0x00004CAC
		public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker, FactionDef faction, PawnGroupMakerParms parms = null)
		{
			return 0f;
		}

		// Token: 0x0600C4DC RID: 50396 RVA: 0x003943D4 File Offset: 0x003925D4
		public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			return base.CanGenerateFrom(parms, groupMaker) && groupMaker.traders.Any<PawnGenOption>() && (!parms.tile.Valid || groupMaker.carriers.Any((PawnGenOption x) => Find.WorldGrid[parms.tile].PrimaryBiome.IsPackAnimalAllowed(x.kind.race)));
		}

		// Token: 0x0600C4DD RID: 50397 RVA: 0x00394438 File Offset: 0x00392638
		protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
		{
			if (!this.CanGenerateFrom(parms, groupMaker))
			{
				if (errorOnZeroResults)
				{
					string str = "Cannot generate trader caravan for ";
					Faction faction = parms.faction;
					Log.Error(str + ((faction != null) ? faction.ToString() : null) + ".");
				}
				return;
			}
			if (!parms.faction.def.caravanTraderKinds.Any<TraderKindDef>())
			{
				string str2 = "Cannot generate trader caravan for ";
				Faction faction2 = parms.faction;
				Log.Error(str2 + ((faction2 != null) ? faction2.ToString() : null) + " because it has no trader kinds.");
				return;
			}
			PawnGenOption pawnGenOption = groupMaker.traders.FirstOrDefault((PawnGenOption x) => !x.kind.trader);
			if (pawnGenOption != null)
			{
				string str3 = "Cannot generate arriving trader caravan for ";
				Faction faction3 = parms.faction;
				Log.Error(str3 + ((faction3 != null) ? faction3.ToString() : null) + " because there is a pawn kind (" + pawnGenOption.kind.LabelCap + ") who is not a trader but is in a traders list.");
				return;
			}
			PawnGenOption pawnGenOption2 = groupMaker.carriers.FirstOrDefault((PawnGenOption x) => !x.kind.RaceProps.packAnimal);
			if (pawnGenOption2 != null)
			{
				string str4 = "Cannot generate arriving trader caravan for ";
				Faction faction4 = parms.faction;
				Log.Error(str4 + ((faction4 != null) ? faction4.ToString() : null) + " because there is a pawn kind (" + pawnGenOption2.kind.LabelCap + ") who is not a carrier but is in a carriers list.");
				return;
			}
			if (parms.seed != null)
			{
				Log.Warning("Deterministic seed not implemented for this pawn group kind worker. The result will be random anyway.");
			}
			TraderKindDef traderKindDef;
			if (parms.traderKind == null)
			{
				traderKindDef = parms.faction.def.caravanTraderKinds.RandomElementByWeight((TraderKindDef traderDef) => traderDef.CalculatedCommonality);
			}
			else
			{
				traderKindDef = parms.traderKind;
			}
			TraderKindDef traderKindDef2 = traderKindDef;
			Pawn pawn = this.GenerateTrader(parms, groupMaker, traderKindDef2);
			outPawns.Add(pawn);
			ThingSetMakerParams parms2 = default(ThingSetMakerParams);
			parms2.traderDef = traderKindDef2;
			parms2.tile = new PlanetTile?(parms.tile);
			parms2.makingFaction = parms.faction;
			List<Thing> wares = ThingSetMakerDefOf.TraderStock.root.Generate(parms2).InRandomOrder(null).ToList<Thing>();
			foreach (Pawn item in this.GetSlavesAndAnimalsFromWares(parms, pawn, wares))
			{
				outPawns.Add(item);
			}
			this.GenerateCarriers(parms, groupMaker, pawn, wares, outPawns);
			this.GenerateGuards(parms, groupMaker, pawn, wares, outPawns);
		}

		// Token: 0x0600C4DE RID: 50398 RVA: 0x003946C0 File Offset: 0x003928C0
		private Pawn GenerateTrader(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, TraderKindDef traderKind)
		{
			PawnKindDef kind = groupMaker.traders.RandomElementByWeight((PawnGenOption x) => x.selectionWeight).kind;
			Faction faction = parms.faction;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			PlanetTile? tile = new PlanetTile?(parms.tile);
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
			Ideo ideo = parms.ideo;
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, allowFood, allowAddictions, parms.inhabitants, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, ideo, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			pawn.mindState.wantsToTradeWithColony = true;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			pawn.trader.traderKind = traderKind;
			parms.points -= pawn.kindDef.combatPower;
			TradeUtility.CheckGiveTraderQuest(pawn);
			return pawn;
		}

		// Token: 0x0600C4DF RID: 50399 RVA: 0x003947DC File Offset: 0x003929DC
		private void GenerateCarriers(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, Pawn trader, List<Thing> wares, List<Pawn> outPawns)
		{
			List<Thing> list = (from x in wares
			where !(x is Pawn)
			select x).ToList<Thing>();
			int i = 0;
			int num = Mathf.CeilToInt((float)list.Count / 8f);
			PawnKindDef kind = (from x in groupMaker.carriers
			where !parms.tile.Valid || Find.WorldGrid[parms.tile].PrimaryBiome.IsPackAnimalAllowed(x.kind.race)
			select x).RandomElementByWeight((PawnGenOption x) => x.selectionWeight).kind;
			List<Pawn> list2 = new List<Pawn>();
			for (int j = 0; j < num; j++)
			{
				PawnKindDef kind2 = kind;
				Faction faction = parms.faction;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				PlanetTile? tile = new PlanetTile?(parms.tile);
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
				Ideo ideo = parms.ideo;
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind2, faction, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, allowFood, allowAddictions, parms.inhabitants, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, ideo, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				if (i < list.Count)
				{
					pawn.inventory.innerContainer.TryAdd(list[i], true);
					i++;
				}
				list2.Add(pawn);
				outPawns.Add(pawn);
			}
			while (i < list.Count)
			{
				list2.RandomElement<Pawn>().inventory.innerContainer.TryAdd(list[i], true);
				i++;
			}
		}

		// Token: 0x0600C4E0 RID: 50400 RVA: 0x003949BC File Offset: 0x00392BBC
		private IEnumerable<Pawn> GetSlavesAndAnimalsFromWares(PawnGroupMakerParms parms, Pawn trader, List<Thing> wares)
		{
			int num;
			for (int i = 0; i < wares.Count; i = num + 1)
			{
				Pawn pawn = wares[i] as Pawn;
				if (pawn != null)
				{
					if (pawn.Faction != parms.faction)
					{
						pawn.SetFaction(parms.faction, null);
					}
					yield return pawn;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600C4E1 RID: 50401 RVA: 0x003949D4 File Offset: 0x00392BD4
		private void GenerateGuards(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, Pawn trader, List<Thing> wares, List<Pawn> outPawns)
		{
			if (!groupMaker.guards.Any<PawnGenOption>())
			{
				return;
			}
			foreach (PawnGenOptionWithXenotype pawnGenOptionWithXenotype in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.guards, parms))
			{
				PawnKindDef kind = pawnGenOptionWithXenotype.Option.kind;
				Faction faction = parms.faction;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				XenotypeDef xenotype = pawnGenOptionWithXenotype.Xenotype;
				PlanetTile? tile = new PlanetTile?(parms.tile);
				bool forceGenerateNewPawn = false;
				bool allowDead = false;
				bool allowDowned = false;
				bool canGeneratePawnRelations = true;
				bool mustBeCapableOfViolence = true;
				float colonistRelationChanceFactor = 1f;
				bool forceAddFreeWarmLayerIfNeeded = false;
				bool allowGay = true;
				bool allowPregnant = false;
				bool allowFood = true;
				bool allowAddictions = true;
				bool inhabitants = parms.inhabitants;
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
				Ideo ideo = parms.ideo;
				Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, allowFood, allowAddictions, inhabitants, certainlyBeenInCryptosleep, forceRedressWorldPawnIfFormerColonist, worldPawnFactionDoesntMatter, biocodeWeaponChance, biocodeApparelChance, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, validatorPreGear, validatorPostGear, forcedTraits, prohibitedTraits, null, null, null, null, null, null, null, ideo, false, false, false, false, null, null, xenotype, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				outPawns.Add(item);
			}
		}

		// Token: 0x0600C4E2 RID: 50402 RVA: 0x00066B11 File Offset: 0x00064D11
		public override IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			throw new NotImplementedException();
		}
	}
}
