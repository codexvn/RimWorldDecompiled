using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FDF RID: 8159
	public class GenStep_PoluxTrees : GenStep_SpecialTrees
	{
		// Token: 0x17001B6C RID: 7020
		// (get) Token: 0x0600B3DB RID: 46043 RVA: 0x00338C7F File Offset: 0x00336E7F
		public override int SeedPart
		{
			get
			{
				return 537455645;
			}
		}

		// Token: 0x0600B3DC RID: 46044 RVA: 0x00338C88 File Offset: 0x00336E88
		public override int DesiredTreeCountForMap(Map map)
		{
			PollutionLevel pollutionLevel = Find.WorldGrid[map.Tile].PollutionLevel();
			switch (pollutionLevel)
			{
			case PollutionLevel.None:
				return this.pollutionNone;
			case PollutionLevel.Light:
				return this.pollutionLight;
			case PollutionLevel.Moderate:
				return this.pollutionModerate;
			case PollutionLevel.Extreme:
				return this.pollutionExtreme;
			default:
				Log.Error("Unrecognized pollution level '" + pollutionLevel.ToString() + "'.");
				return 0;
			}
		}

		// Token: 0x0600B3DD RID: 46045 RVA: 0x0004FC72 File Offset: 0x0004DE72
		protected override float GetGrowth()
		{
			return 1f;
		}

		// Token: 0x04007B55 RID: 31573
		private const float Growth = 1f;
	}
}
