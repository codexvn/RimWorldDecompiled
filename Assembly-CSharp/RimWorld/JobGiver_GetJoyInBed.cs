using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014E1 RID: 5345
	public class JobGiver_GetJoyInBed : JobGiver_GetJoy
	{
		// Token: 0x170013AD RID: 5037
		// (get) Token: 0x06007FAF RID: 32687 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanDoDuringMedicalRest
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007FB0 RID: 32688 RVA: 0x00266E83 File Offset: 0x00265083
		protected override bool JoyGiverAllowed(JoyGiverDef def)
		{
			return def.canDoWhileInBed;
		}

		// Token: 0x06007FB1 RID: 32689 RVA: 0x00266E8B File Offset: 0x0026508B
		protected override Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			return def.Worker.TryGiveJobWhileInBed(pawn);
		}

		// Token: 0x06007FB2 RID: 32690 RVA: 0x00266E9C File Offset: 0x0026509C
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.CurJob == null || !pawn.InBed() || !pawn.Awake() || pawn.needs.joy == null)
			{
				return null;
			}
			if (pawn.health.hediffSet.InLabor(true))
			{
				return null;
			}
			if (pawn.needs.joy.CurLevel > 0.5f)
			{
				return null;
			}
			return base.TryGiveJob(pawn);
		}

		// Token: 0x0400556A RID: 21866
		private const float MaxJoyLevel = 0.5f;
	}
}
