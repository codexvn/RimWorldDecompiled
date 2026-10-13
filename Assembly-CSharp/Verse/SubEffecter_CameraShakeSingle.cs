using System;

namespace Verse
{
	// Token: 0x02000CB7 RID: 3255
	public class SubEffecter_CameraShakeSingle : SubEffecter_CameraShake
	{
		// Token: 0x06005360 RID: 21344 RVA: 0x001AD9B3 File Offset: 0x001ABBB3
		public SubEffecter_CameraShakeSingle(SubEffecterDef subDef, Effecter parent) : base(subDef, parent)
		{
		}

		// Token: 0x06005361 RID: 21345 RVA: 0x001AD9BD File Offset: 0x001ABBBD
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			base.DoShake(A);
		}
	}
}
