using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020020C2 RID: 8386
	public class AncientStructureGenParms
	{
		// Token: 0x04007DFD RID: 32253
		public LayoutDef structureLayoutDef;

		// Token: 0x04007DFE RID: 32254
		public IntRange structureSizeRange = IntRange.Zero;

		// Token: 0x04007DFF RID: 32255
		public List<ScatteredPrefabs> scatteredPrefabs = new List<ScatteredPrefabs>();

		// Token: 0x04007E00 RID: 32256
		public ThingDef perimeterWallDef;

		// Token: 0x04007E01 RID: 32257
		public int perimeterExpandBy;

		// Token: 0x04007E02 RID: 32258
		public float perimeterWallChance = 1f;

		// Token: 0x04007E03 RID: 32259
		public TerrainDef perimeterTerrainDef;

		// Token: 0x04007E04 RID: 32260
		public List<LayoutScatterTerrainParms> perimeterScatterTerrain = new List<LayoutScatterTerrainParms>();

		// Token: 0x04007E05 RID: 32261
		public List<AncientStructureGenParms.ScatteredThing> perimeterScatteredThings = new List<AncientStructureGenParms.ScatteredThing>();

		// Token: 0x020020C3 RID: 8387
		public class ScatteredThing
		{
			// Token: 0x0600B778 RID: 46968 RVA: 0x0034D489 File Offset: 0x0034B689
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				XmlHelper.ParseElements(this, xmlRoot, "thingDef", "chancePer100Cells");
			}

			// Token: 0x04007E06 RID: 32262
			public ThingDef thingDef;

			// Token: 0x04007E07 RID: 32263
			public float chancePer100Cells = 1f;
		}
	}
}
