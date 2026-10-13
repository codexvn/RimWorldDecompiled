using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F2 RID: 6386
	public class ThoughtMemory_Inhumanized : Thought_Memory
	{
		// Token: 0x170015FA RID: 5626
		// (get) Token: 0x06009134 RID: 37172 RVA: 0x002AB5A8 File Offset: 0x002A97A8
		public override int CurStageIndex
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return 0;
				}
				if (!this.pawn.Inhumanized())
				{
					return 0;
				}
				return 1;
			}
		}
	}
}
