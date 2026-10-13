using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02003C64 RID: 15460
	public class WorldCameraConfig_Car : WorldCameraConfig
	{
		// Token: 0x06014FEC RID: 85996 RVA: 0x00649019 File Offset: 0x00647219
		public WorldCameraConfig_Car()
		{
			this.dollyRateKeys = 0f;
			this.dollyRateScreenEdge = 0f;
			this.camRotationDecayFactor = 1f;
			this.rotationSpeedScale = 0.15f;
		}

		// Token: 0x06014FED RID: 85997 RVA: 0x00649050 File Offset: 0x00647250
		public override void ConfigFixedUpdate_60(ref Vector2 rotationVelocity)
		{
			base.ConfigFixedUpdate_60(ref rotationVelocity);
			float num = 0.016666668f;
			if (KeyBindingDefOf.MapDolly_Left.IsDown)
			{
				this.targetAngle += 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Right.IsDown)
			{
				this.targetAngle -= 0.72f * num;
			}
			if (KeyBindingDefOf.MapDolly_Up.IsDown)
			{
				this.speed += 1.5f * num;
			}
			if (KeyBindingDefOf.MapDolly_Down.IsDown)
			{
				this.speed -= 1.5f * num;
				if (this.speed < 0f)
				{
					this.speed = 0f;
				}
			}
			this.angle = Mathf.Lerp(this.angle, this.targetAngle, 0.02f);
			rotationVelocity.x = Mathf.Cos(this.angle) * this.speed;
			rotationVelocity.y = Mathf.Sin(this.angle) * this.speed;
		}

		// Token: 0x0400EA2B RID: 59947
		private float targetAngle;

		// Token: 0x0400EA2C RID: 59948
		private float angle;

		// Token: 0x0400EA2D RID: 59949
		private float speed;

		// Token: 0x0400EA2E RID: 59950
		private const float SpeedChangeSpeed = 1.5f;

		// Token: 0x0400EA2F RID: 59951
		private const float AngleChangeSpeed = 0.72f;
	}
}
