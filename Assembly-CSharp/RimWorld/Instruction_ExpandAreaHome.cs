using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020034D6 RID: 13526
	public class Instruction_ExpandAreaHome : Instruction_ExpandArea
	{
		// Token: 0x17003182 RID: 12674
		// (get) Token: 0x06013022 RID: 77858 RVA: 0x005A30C1 File Offset: 0x005A12C1
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.Home;
			}
		}
	}
}
