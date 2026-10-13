using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C0 RID: 6336
	public class ThoughtWorker_KillThirst : ThoughtWorker
	{
		// Token: 0x060090A0 RID: 37024 RVA: 0x002A9AE0 File Offset: 0x002A7CE0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			Need_KillThirst need_KillThirst;
			if (p.needs == null || !p.needs.TryGetNeed<Need_KillThirst>(out need_KillThirst))
			{
				return ThoughtState.Inactive;
			}
			return need_KillThirst.CurLevel <= 0.3f;
		}

		// Token: 0x04005C48 RID: 23624
		public const float MinLevelForThought = 0.3f;
	}
}
