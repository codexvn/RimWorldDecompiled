using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FBB RID: 8123
	public class GenStep_HarbingerTrees : GenStep_SpecialTrees
	{
		// Token: 0x17001B45 RID: 6981
		// (get) Token: 0x0600B324 RID: 45860 RVA: 0x003353F9 File Offset: 0x003335F9
		public override int SeedPart
		{
			get
			{
				return 786491238;
			}
		}

		// Token: 0x0600B325 RID: 45861 RVA: 0x0004FC72 File Offset: 0x0004DE72
		protected override float GetGrowth()
		{
			return 1f;
		}

		// Token: 0x0600B326 RID: 45862 RVA: 0x00335400 File Offset: 0x00333600
		public override int DesiredTreeCountForMap(Map map)
		{
			return Find.Anomaly.LevelDef.desiredHarbingerTreeCount;
		}

		// Token: 0x04007ABC RID: 31420
		private const float Growth = 1f;
	}
}
