using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002047 RID: 8263
	public class GenStep_Ambush_Edge : GenStep_Ambush
	{
		// Token: 0x17001BA6 RID: 7078
		// (get) Token: 0x0600B585 RID: 46469 RVA: 0x00342D97 File Offset: 0x00340F97
		public override int SeedPart
		{
			get
			{
				return 1412216193;
			}
		}

		// Token: 0x0600B586 RID: 46470 RVA: 0x00342D9E File Offset: 0x00340F9E
		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root, parms);
			signalAction_Ambush.spawnPawnsOnEdge = true;
			return signalAction_Ambush;
		}
	}
}
