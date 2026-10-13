using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002068 RID: 8296
	public class GenStep_InsectLairSite : GenStep
	{
		// Token: 0x17001BD0 RID: 7120
		// (get) Token: 0x0600B613 RID: 46611 RVA: 0x00344CA9 File Offset: 0x00342EA9
		public override int SeedPart
		{
			get
			{
				return 49817324;
			}
		}

		// Token: 0x0600B614 RID: 46612 RVA: 0x00344CB0 File Offset: 0x00342EB0
		public override void Generate(Map map, GenStepParams parms)
		{
			IntVec3 entranceCell;
			if (!TileMutatorWorker_InsectMegahive.TryGetEntranceCell(map, out entranceCell))
			{
				return;
			}
			TileMutatorWorker_InsectMegahive.GenerateMegahiveEntrance(map, entranceCell, true);
		}
	}
}
