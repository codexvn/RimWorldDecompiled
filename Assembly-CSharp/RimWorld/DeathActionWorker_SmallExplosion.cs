using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020022DA RID: 8922
	public class DeathActionWorker_SmallExplosion : DeathActionWorker
	{
		// Token: 0x17001DB1 RID: 7601
		// (get) Token: 0x0600C207 RID: 49671 RVA: 0x00383205 File Offset: 0x00381405
		public override RulePackDef DeathRules
		{
			get
			{
				return RulePackDefOf.Transition_DiedExplosive;
			}
		}

		// Token: 0x17001DB2 RID: 7602
		// (get) Token: 0x0600C208 RID: 49672 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool DangerousInMelee
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C209 RID: 49673 RVA: 0x0038339C File Offset: 0x0038159C
		public override void PawnDied(Corpse corpse, Lord prevLord)
		{
			GenExplosion.DoExplosion(corpse.Position, corpse.Map, 1.9f, DamageDefOf.Flame, corpse.InnerPawn, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}
	}
}
