using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020020D0 RID: 8400
	public class TileMutatorWorker_AnimalHabitat : TileMutatorWorker
	{
		// Token: 0x0600B79D RID: 47005 RVA: 0x0034E11D File Offset: 0x0034C31D
		public TileMutatorWorker_AnimalHabitat(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B79E RID: 47006 RVA: 0x0034E134 File Offset: 0x0034C334
		public override string GetLabel(PlanetTile tile)
		{
			PawnKindDef animalKind = this.GetAnimalKind(tile);
			return "AnimalHabitat".Translate(animalKind.Named("ANIMALKIND"));
		}

		// Token: 0x0600B79F RID: 47007 RVA: 0x0034E164 File Offset: 0x0034C364
		public override string GetDescription(PlanetTile tile)
		{
			PawnKindDef animalKind = this.GetAnimalKind(tile);
			return this.def.description.Formatted(animalKind.Named("ANIMALKIND"));
		}

		// Token: 0x0600B7A0 RID: 47008 RVA: 0x0034E199 File Offset: 0x0034C399
		public override float AnimalCommonalityFactorFor(PawnKindDef animal, PlanetTile tile)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return 1f;
			}
			if (animal != this.GetAnimalKind(tile))
			{
				return 1f;
			}
			return 10f;
		}

		// Token: 0x0600B7A1 RID: 47009 RVA: 0x0034E1C0 File Offset: 0x0034C3C0
		public PawnKindDef GetAnimalKind(PlanetTile tile)
		{
			BiomeDef primaryBiome = Find.WorldGrid[tile].PrimaryBiome;
			foreach (PawnKindDef pawnKindDef in primaryBiome.AllWildAnimals)
			{
				if (primaryBiome.CommonalityOfAnimal(pawnKindDef) > 0.1f)
				{
					this.tmpAnimals.Add(pawnKindDef);
				}
				else if (tile.Tile.IsCoastal && primaryBiome.CommonalityOfCoastalAnimal(pawnKindDef) > 0.1f)
				{
					this.tmpAnimals.Add(pawnKindDef);
				}
			}
			if (this.tmpAnimals.Empty<PawnKindDef>())
			{
				Log.ErrorOnce(string.Format("TileMutatorWorker_AnimalHabitat: Could not find animal in biome with commonality > {0}, either increase some commonalities, or decrease animalDensity in the biomeDef below this mutators min of {1}", 0.1f, this.def.animalDensityRange.min), 123498);
				return null;
			}
			Rand.PushState();
			Rand.Seed = tile.GetHashCode();
			PawnKindDef result = this.tmpAnimals.RandomElement<PawnKindDef>();
			Rand.PopState();
			this.tmpAnimals.Clear();
			return result;
		}

		// Token: 0x04007E1F RID: 32287
		private const float AnimalCommonalityFactor = 10f;

		// Token: 0x04007E20 RID: 32288
		private const float MinAnimalCommonality = 0.1f;

		// Token: 0x04007E21 RID: 32289
		private readonly List<PawnKindDef> tmpAnimals = new List<PawnKindDef>();
	}
}
