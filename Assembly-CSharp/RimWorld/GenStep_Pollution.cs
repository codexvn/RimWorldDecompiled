using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FDE RID: 8158
	public class GenStep_Pollution : GenStep
	{
		// Token: 0x17001B6B RID: 7019
		// (get) Token: 0x0600B3D7 RID: 46039 RVA: 0x00338C2C File Offset: 0x00336E2C
		public override int SeedPart
		{
			get
			{
				return 7593785;
			}
		}

		// Token: 0x0600B3D8 RID: 46040 RVA: 0x00338C33 File Offset: 0x00336E33
		public override void Generate(Map map, GenStepParams parms)
		{
			PollutionUtility.PolluteMapToPercent(map, Find.WorldGrid[map.Tile].pollution, 1000);
		}

		// Token: 0x04007B52 RID: 31570
		private static readonly FloatRange LightPollutionCellRange = new FloatRange(0.1f, 0.3f);

		// Token: 0x04007B53 RID: 31571
		private static readonly FloatRange ModeratePollutionCellRange = new FloatRange(0.4f, 0.6f);

		// Token: 0x04007B54 RID: 31572
		private const int PollutionGlobSize = 1000;
	}
}
