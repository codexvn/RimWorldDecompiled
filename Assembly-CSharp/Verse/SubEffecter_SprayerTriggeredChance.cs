using System;

namespace Verse
{
	// Token: 0x02000CC3 RID: 3267
	public class SubEffecter_SprayerTriggeredChance : SubEffecter_Sprayer
	{
		// Token: 0x06005381 RID: 21377 RVA: 0x001AEB52 File Offset: 0x001ACD52
		public SubEffecter_SprayerTriggeredChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005382 RID: 21378 RVA: 0x001AEB68 File Offset: 0x001ACD68
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			float effectiveChancePerTick = base.EffectiveChancePerTick;
			if (Rand.Value < effectiveChancePerTick || force)
			{
				base.MakeMote(A, B, overrideSpawnTick);
			}
		}
	}
}
