using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000CB6 RID: 3254
	public abstract class SubEffecter_CameraShake : SubEffecter
	{
		// Token: 0x0600535D RID: 21341 RVA: 0x0003A14D File Offset: 0x0003834D
		public SubEffecter_CameraShake(SubEffecterDef subDef, Effecter parent) : base(subDef, parent)
		{
		}

		// Token: 0x0600535E RID: 21342 RVA: 0x001AD8D2 File Offset: 0x001ABAD2
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			if (force)
			{
				this.DoShake(A);
			}
		}

		// Token: 0x0600535F RID: 21343 RVA: 0x001AD8E0 File Offset: 0x001ABAE0
		public void DoShake(TargetInfo tgt)
		{
			float num = 1f - Find.CameraDriver.ZoomRootSize.Remap(11f, 60f, 0f, 1f);
			IntVec3 mapPosition = Find.CameraDriver.MapPosition;
			float lengthHorizontal = (tgt.Cell - mapPosition).LengthHorizontal;
			float num2 = 1f - Mathf.Clamp01(lengthHorizontal / this.def.distanceAttenuationMax);
			num2 *= num;
			float randomInRange = this.def.cameraShake.RandomInRange;
			Find.CameraDriver.shaker.DoShake(randomInRange * Mathf.Lerp(1f, num2, this.def.distanceAttenuationScale));
			if (this.def.soundDef != null)
			{
				this.def.soundDef.PlayOneShot(tgt);
			}
		}
	}
}
