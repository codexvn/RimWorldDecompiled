using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020034D7 RID: 13527
	public class Instruction_ExpandAreaBuildRoof : Instruction_ExpandArea
	{
		// Token: 0x17003183 RID: 12675
		// (get) Token: 0x06013024 RID: 77860 RVA: 0x005A30DB File Offset: 0x005A12DB
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.BuildRoof;
			}
		}
	}
}
