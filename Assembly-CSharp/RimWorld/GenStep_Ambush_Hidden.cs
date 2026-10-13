using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002048 RID: 8264
	public class GenStep_Ambush_Hidden : GenStep_Ambush
	{
		// Token: 0x17001BA7 RID: 7079
		// (get) Token: 0x0600B588 RID: 46472 RVA: 0x00342DB8 File Offset: 0x00340FB8
		public override int SeedPart
		{
			get
			{
				return 921085483;
			}
		}

		// Token: 0x0600B589 RID: 46473 RVA: 0x00342DBF File Offset: 0x00340FBF
		protected override RectTrigger MakeRectTrigger()
		{
			RectTrigger rectTrigger = base.MakeRectTrigger();
			rectTrigger.activateOnExplosion = true;
			return rectTrigger;
		}

		// Token: 0x0600B58A RID: 46474 RVA: 0x00342DD0 File Offset: 0x00340FD0
		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root, parms);
			if (root.IsValid)
			{
				signalAction_Ambush.spawnNear = root;
			}
			else
			{
				signalAction_Ambush.spawnAround = rectToDefend;
			}
			return signalAction_Ambush;
		}
	}
}
