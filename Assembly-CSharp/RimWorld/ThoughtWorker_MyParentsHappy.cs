using System;

namespace RimWorld
{
	// Token: 0x020018E4 RID: 6372
	public class ThoughtWorker_MyParentsHappy : ThoughtWorker_MyParentHappy
	{
		// Token: 0x170015F9 RID: 5625
		// (get) Token: 0x0600910F RID: 37135 RVA: 0x000E96BB File Offset: 0x000E78BB
		protected override int RequiredParentCount
		{
			get
			{
				return 2;
			}
		}
	}
}
