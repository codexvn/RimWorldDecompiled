using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002126 RID: 8486
	public class TileMutatorWorker_WildPlants : TileMutatorWorker
	{
		// Token: 0x0600B90E RID: 47374 RVA: 0x0034CD7F File Offset: 0x0034AF7F
		public TileMutatorWorker_WildPlants(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B90F RID: 47375 RVA: 0x00356DF4 File Offset: 0x00354FF4
		public override string GetLabel(PlanetTile tile)
		{
			return "WildPlants".Translate(this.GetPlantKind(tile).label);
		}

		// Token: 0x0600B910 RID: 47376 RVA: 0x00356E16 File Offset: 0x00355016
		public override string GetDescription(PlanetTile tile)
		{
			return this.def.description.Formatted(this.GetPlantKind(tile).label);
		}

		// Token: 0x0600B911 RID: 47377 RVA: 0x00356E3E File Offset: 0x0035503E
		public override IEnumerable<BiomePlantRecord> AdditionalWildPlants(PlanetTile tile)
		{
			if (!ModsConfig.OdysseyActive)
			{
				yield break;
			}
			yield return new BiomePlantRecord
			{
				plant = this.GetPlantKind(tile),
				commonality = 0.1f
			};
			yield break;
		}

		// Token: 0x0600B912 RID: 47378 RVA: 0x00356E55 File Offset: 0x00355055
		private ThingDef GetPlantKind(PlanetTile tile)
		{
			Rand.PushState();
			Rand.Seed = tile.GetHashCode();
			ThingDef result = this.def.plantKinds.RandomElement<ThingDef>();
			Rand.PopState();
			return result;
		}

		// Token: 0x04007FAF RID: 32687
		private const float PlantCommonality = 0.1f;
	}
}
