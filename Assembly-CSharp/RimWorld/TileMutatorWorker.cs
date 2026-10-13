using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020020B8 RID: 8376
	public abstract class TileMutatorWorker
	{
		// Token: 0x0600B74E RID: 46926 RVA: 0x0034CD49 File Offset: 0x0034AF49
		public TileMutatorWorker(TileMutatorDef def)
		{
			this.def = def;
		}

		// Token: 0x0600B74F RID: 46927 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool IsValidTile(PlanetTile tile, PlanetLayer layer)
		{
			return true;
		}

		// Token: 0x0600B750 RID: 46928 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void OnAddedToTile(PlanetTile tile)
		{
		}

		// Token: 0x0600B751 RID: 46929 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Init(Map map)
		{
		}

		// Token: 0x0600B752 RID: 46930 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Tick(Map map)
		{
		}

		// Token: 0x0600B753 RID: 46931 RVA: 0x0034CD58 File Offset: 0x0034AF58
		public virtual string GetLabel(PlanetTile tile)
		{
			return this.def.label;
		}

		// Token: 0x0600B754 RID: 46932 RVA: 0x0034CD65 File Offset: 0x0034AF65
		public virtual string GetDescription(PlanetTile tile)
		{
			return this.def.description;
		}

		// Token: 0x0600B755 RID: 46933 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GeneratePostElevationFertility(Map map)
		{
		}

		// Token: 0x0600B756 RID: 46934 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GeneratePostTerrain(Map map)
		{
		}

		// Token: 0x0600B757 RID: 46935 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GenerateCriticalStructures(Map map)
		{
		}

		// Token: 0x0600B758 RID: 46936 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GenerateNonCriticalStructures(Map map)
		{
		}

		// Token: 0x0600B759 RID: 46937 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GeneratePostFog(Map map)
		{
		}

		// Token: 0x0600B75A RID: 46938 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void MutateWeatherCommonalityFor(WeatherDef weather, PlanetTile tile, ref float commonality)
		{
		}

		// Token: 0x0600B75B RID: 46939 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public virtual float AnimalCommonalityFactorFor(PawnKindDef animal, PlanetTile tile)
		{
			return 1f;
		}

		// Token: 0x0600B75C RID: 46940 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public virtual float PlantCommonalityFactorFor(ThingDef plant, PlanetTile tile)
		{
			return 1f;
		}

		// Token: 0x0600B75D RID: 46941 RVA: 0x0034CD72 File Offset: 0x0034AF72
		public virtual IEnumerable<BiomePlantRecord> AdditionalWildPlants(PlanetTile tile)
		{
			return this.def.additionalWildPlants;
		}

		// Token: 0x04007DF0 RID: 32240
		public TileMutatorDef def;
	}
}
