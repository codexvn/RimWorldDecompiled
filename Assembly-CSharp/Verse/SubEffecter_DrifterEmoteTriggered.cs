using System;

namespace Verse
{
	// Token: 0x020001B6 RID: 438
	public class SubEffecter_DrifterEmoteTriggered : SubEffecter_DrifterEmote
	{
		// Token: 0x06000C3F RID: 3135 RVA: 0x0003A2E7 File Offset: 0x000384E7
		public SubEffecter_DrifterEmoteTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000C40 RID: 3136 RVA: 0x0003A33F File Offset: 0x0003853F
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			base.MakeMote(A, overrideSpawnTick);
		}
	}
}
