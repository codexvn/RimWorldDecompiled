using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020022D8 RID: 8920
	public class DeathActionWorker_BigExplosion : DeathActionWorker
	{
		// Token: 0x17001DAE RID: 7598
		// (get) Token: 0x0600C200 RID: 49664 RVA: 0x00383205 File Offset: 0x00381405
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x17001DAF RID: 7599
		// (get) Token: 0x0600C201 RID: 49665 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C202 RID: 49666 RVA: 0x0038320C File Offset: 0x0038140C
		public override void PawnDied(Corpse corpse, Lord prevLord)
		{
			float radius;
			if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 0)
			{
				radius = 1.9f;
			}
			else if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 1)
			{
				radius = 2.9f;
			}
			else
			{
				radius = 4.9f;
			}
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, DamageDefOf.Flame, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}
	}
}
