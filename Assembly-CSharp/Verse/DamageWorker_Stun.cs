using System;

namespace Verse
{
	// Token: 0x02000672 RID: 1650
	public class DamageWorker_Stun : DamageWorker
	{
		// Token: 0x06002975 RID: 10613 RVA: 0x000DA935 File Offset: 0x000D8B35
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult damageResult = base.Apply(dinfo, victim);
			damageResult.stunned = true;
			return damageResult;
		}
	}
}
