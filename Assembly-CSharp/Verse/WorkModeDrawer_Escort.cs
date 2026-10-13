using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020002A3 RID: 675
	public class WorkModeDrawer_Escort : WorkModeDrawer
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool DrawIconAtTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x0004F04A File Offset: 0x0004D24A
		public override GlobalTargetInfo GetTargetForLine(MechanitorControlGroup group)
		{
			return group.Tracker.Pawn;
		}
	}
}
