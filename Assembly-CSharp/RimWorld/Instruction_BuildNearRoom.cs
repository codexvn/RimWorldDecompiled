using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020034CB RID: 13515
	public class Instruction_BuildNearRoom : Instruction_BuildAtRoom
	{
		// Token: 0x17003178 RID: 12664
		// (get) Token: 0x06012FE4 RID: 77796 RVA: 0x005A2489 File Offset: 0x005A0689
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ExpandedBy(10);
			}
		}

		// Token: 0x06012FE5 RID: 77797 RVA: 0x005A249C File Offset: 0x005A069C
		protected override bool AllowBuildAt(IntVec3 c)
		{
			return base.AllowBuildAt(c) && !Find.TutorialState.roomRect.Contains(c);
		}
	}
}
