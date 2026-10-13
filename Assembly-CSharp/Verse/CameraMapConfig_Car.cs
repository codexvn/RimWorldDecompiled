using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001D9 RID: 473
	public class CameraMapConfig_Car : CameraMapConfig
	{
		// Token: 0x06000CEC RID: 3308 RVA: 0x0003D2F2 File Offset: 0x0003B4F2
		public CameraMapConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateScreenEdge = 0f;
			this.camSpeedDecayFactor = 1f;
			this.moveSpeedScale = 1f;
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0003D328 File Offset: 0x0003B528
		public override void ConfigFixedUpdate_60(ref Vector3 rootPos, ref Vector3 velocity)
		{
			float num = 0.016666668f;
			if (KeyBindingDefOf.MapDolly_Left.IsDown)
			{
				this.autoPanTargetAngle += 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Right.IsDown)
			{
				this.autoPanTargetAngle -= 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Up.IsDown)
			{
				this.autoPanSpeed += 1.2f * num;
			}
			if (KeyBindingDefOf.MapDolly_Down.IsDown)
			{
				this.autoPanSpeed -= 1.2f * num;
				if (this.autoPanSpeed < 0f)
				{
					this.autoPanSpeed = 0f;
				}
			}
			this.autoPanAngle = Mathf.Lerp(this.autoPanAngle, this.autoPanTargetAngle, 0.02f);
			base.ConfigFixedUpdate_60(ref rootPos, ref velocity);
		}

		// Token: 0x04000781 RID: 1921
		private const float SpeedChangeSpeed = 1.2f;

		// Token: 0x04000782 RID: 1922
		private const float AngleChangeSpeed = 0.72f;
	}
}
