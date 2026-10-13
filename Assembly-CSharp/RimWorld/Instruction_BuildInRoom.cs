using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020034CA RID: 13514
	public class Instruction_BuildInRoom : Instruction_BuildAtRoom
	{
		// Token: 0x17003177 RID: 12663
		// (get) Token: 0x06012FE2 RID: 77794 RVA: 0x005A246F File Offset: 0x005A066F
		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ContractedBy(1);
			}
		}
	}
}
