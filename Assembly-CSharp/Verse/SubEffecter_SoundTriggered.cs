using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000CBE RID: 3262
	public class SubEffecter_SoundTriggered : SubEffecter
	{
		// Token: 0x06005372 RID: 21362 RVA: 0x0003A14D File Offset: 0x0003834D
		public SubEffecter_SoundTriggered(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005373 RID: 21363 RVA: 0x001ADDC0 File Offset: 0x001ABFC0
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			this.def.soundDef.PlayOneShot(new TargetInfo(A.Cell, A.Map, false));
		}
	}
}
