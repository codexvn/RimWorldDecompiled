using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002A0 RID: 672
	public class MapGeneratorDef : Def
	{
		// Token: 0x04000DEA RID: 3562
		public bool isUnderground;

		// Token: 0x04000DEB RID: 3563
		public bool forceCaves;

		// Token: 0x04000DEC RID: 3564
		public List<GenStepDef> genSteps;

		// Token: 0x04000DED RID: 3565
		public PocketMapProperties pocketMapProperties;

		// Token: 0x04000DEE RID: 3566
		public List<Type> customMapComponents = new List<Type>();

		// Token: 0x04000DEF RID: 3567
		public bool ignoreAreaRevealedLetter;

		// Token: 0x04000DF0 RID: 3568
		public RoofDef roofDef;

		// Token: 0x04000DF1 RID: 3569
		public bool disableShadows;

		// Token: 0x04000DF2 RID: 3570
		public bool disableCallAid;

		// Token: 0x04000DF3 RID: 3571
		public bool disableMapClippers;

		// Token: 0x04000DF4 RID: 3572
		public bool renderWorld;

		// Token: 0x04000DF5 RID: 3573
		public bool defeatRequiresCantReachUnfogged = true;

		// Token: 0x04000DF6 RID: 3574
		public TerrainDef defaultUnderGridTerrain;

		// Token: 0x04000DF7 RID: 3575
		public bool validScenarioMap;
	}
}
