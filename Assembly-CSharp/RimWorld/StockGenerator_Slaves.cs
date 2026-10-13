using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F99 RID: 12185
	public class StockGenerator_Slaves : StockGenerator
	{
		// Token: 0x06011298 RID: 70296 RVA: 0x004EB7EF File Offset: 0x004E99EF
		public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
		{
			if (this.respectPopulationIntent && Rand.Value > StorytellerUtilityPopulation.PopulationIntent)
			{
				yield break;
			}
			if (faction != null && faction.ideos != null)
			{
				bool flag = true;
				using (IEnumerator<Ideo> enumerator = faction.ideos.AllIdeos.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IdeoApprovesOfSlavery())
						{
							flag = false;
							break;
						}
					}
				}
				if (!flag)
				{
					yield break;
				}
			}
			int count = this.countRange.RandomInRange;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				Faction faction2;
				if (!(from fac in Find.FactionManager.AllFactionsVisible
				where fac != Faction.OfPlayer && fac.def.humanlikeFaction && !fac.temporary
				select fac).TryRandomElement(out faction2))
				{
					yield break;
				}
				DevelopmentalStage developmentalStage = Find.Storyteller.difficulty.ChildrenAllowed ? (DevelopmentalStage.Child | DevelopmentalStage.Adult) : DevelopmentalStage.Adult;
				PawnKindDef kind = (this.slaveKindDef != null) ? this.slaveKindDef : PawnKindDefOf.Slave;
				Faction faction3 = faction2;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				PlanetTile? tile = new PlanetTile?(forTile);
				bool forceGenerateNewPawn = false;
				bool allowDead = false;
				bool allowDowned = false;
				bool canGeneratePawnRelations = true;
				bool mustBeCapableOfViolence = false;
				float colonistRelationChanceFactor = 1f;
				bool forceAddFreeWarmLayerIfNeeded = !this.trader.orbital;
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
				DevelopmentalStage developmentalStages = developmentalStage;
				PawnGenerationRequest request = new PawnGenerationRequest(kind, faction3, context, tile, forceGenerateNewPawn, allowDead, allowDowned, canGeneratePawnRelations, mustBeCapableOfViolence, colonistRelationChanceFactor, forceAddFreeWarmLayerIfNeeded, allowGay, allowPregnant, allowFood, allowAddictions, inhabitant, certainlyBeenInCryptosleep, forceRedressWorldPawnIfFormerColonist, worldPawnFactionDoesntMatter, biocodeWeaponChance, biocodeApparelChance, extraPawnForExtraRelationChance, relationWithExtraPawnChanceFactor, validatorPreGear, validatorPostGear, forcedTraits, prohibitedTraits, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, developmentalStages, null, null, null, false, false, false, -1, 0, false);
				yield return PawnGenerator.GeneratePawn(request);
				num = i;
			}
			yield break;
		}

		// Token: 0x06011299 RID: 70297 RVA: 0x004E9A83 File Offset: 0x004E7C83
		public override bool HandlesThingDef(ThingDef thingDef)
		{
			return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability > Tradeability.None;
		}

		// Token: 0x0400B21B RID: 45595
		private bool respectPopulationIntent;

		// Token: 0x0400B21C RID: 45596
		public PawnKindDef slaveKindDef;
	}
}
