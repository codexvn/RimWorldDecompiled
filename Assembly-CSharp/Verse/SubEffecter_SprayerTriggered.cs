using System;

namespace Verse
{
	// Token: 0x02000CC2 RID: 3266
	public class SubEffecter_SprayerTriggered : SubEffecter_Sprayer
	{
		// Token: 0x0600537F RID: 21375 RVA: 0x001AEB52 File Offset: 0x001ACD52
		public SubEffecter_SprayerTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005380 RID: 21376 RVA: 0x001AEB5C File Offset: 0x001ACD5C
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			base.MakeMote(A, B, overrideSpawnTick);
		}
	}
}
