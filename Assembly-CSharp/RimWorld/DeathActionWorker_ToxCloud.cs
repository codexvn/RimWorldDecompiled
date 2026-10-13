using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020022D9 RID: 8921
	public class DeathActionWorker_ToxCloud : DeathActionWorker
	{
		// Token: 0x17001DB0 RID: 7600
		// (get) Token: 0x0600C204 RID: 49668 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C205 RID: 49669 RVA: 0x003832D0 File Offset: 0x003814D0
		public override void PawnDied(Corpse corpse, Lord prevLord)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			float radius;
			if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 0)
			{
				radius = 0.9f;
			}
			else if (corpse.InnerPawn.ageTracker.CurLifeStageIndex == 1)
			{
				radius = 1.9f;
			}
			else
			{
				radius = 2.9f;
			}
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, radius, DamageDefOf.ToxGas, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, new GasType?(GasType.ToxGas), null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}
	}
}
