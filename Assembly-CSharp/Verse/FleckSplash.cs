using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000406 RID: 1030
	public struct FleckSplash : IFleck
	{
		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001A1B RID: 6683 RVA: 0x00083A8F File Offset: 0x00081C8F
		public bool EndOfLife
		{
			get
			{
				return this.ageSecs >= this.targetSize / this.velocity;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001A1C RID: 6684 RVA: 0x00083AAC File Offset: 0x00081CAC
		public float Alpha
		{
			get
			{
				Mathf.Clamp01(this.ageSecs * 10f);
				float num = 1f;
				float num2 = Mathf.Clamp01(1f - this.ageSecs / (this.targetSize / this.velocity));
				return num * num2 * this.CalculatedIntensity();
			}
		}

		// Token: 0x06001A1D RID: 6685 RVA: 0x00083AFC File Offset: 0x00081CFC
		public bool TimeInterval(float deltaTime, Map map)
		{
			if (this.EndOfLife)
			{
				return true;
			}
			this.ageSecs += deltaTime;
			if (this.def.growthRate != 0f)
			{
				this.exactScale = new Vector3(this.exactScale.x + this.def.growthRate * deltaTime, this.exactScale.y, this.exactScale.z + this.def.growthRate * deltaTime);
				this.exactScale.x = Mathf.Max(this.exactScale.x, 0.0001f);
				this.exactScale.z = Mathf.Max(this.exactScale.z, 0.0001f);
			}
			float d = this.ageSecs * this.velocity;
			this.exactScale = Vector3.one * d;
			this.position += map.waterInfo.GetWaterMovement(this.position) * deltaTime;
			return false;
		}

		// Token: 0x06001A1E RID: 6686 RVA: 0x00083C08 File Offset: 0x00081E08
		public void Draw(DrawBatch batch)
		{
			this.position.y = this.def.altitudeLayer.AltitudeFor(this.def.altitudeLayerIncOffset);
			int num = this.setupTick + this.spawnPosition.GetHashCode();
			((Graphic_Fleck)this.def.GetGraphicData(num).Graphic).DrawFleck(new FleckDrawData
			{
				alpha = this.Alpha,
				color = Color.white,
				drawLayer = 0,
				pos = this.position,
				rotation = 0f,
				scale = this.exactScale,
				ageSecs = this.ageSecs,
				calculatedShockwaveSpan = this.CalculatedShockwaveSpan(),
				id = (float)num
			}, batch);
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x00083CE4 File Offset: 0x00081EE4
		public void Setup(FleckCreationData creationData)
		{
			this.def = creationData.def;
			this.position = creationData.spawnPosition;
			this.spawnPosition = creationData.spawnPosition;
			this.velocity = creationData.velocitySpeed;
			this.targetSize = creationData.targetSize;
			this.setupTick = Find.TickManager.TicksGame;
			if (creationData.ageTicksOverride != -1)
			{
				this.ForceSpawnTick(creationData.ageTicksOverride);
			}
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x00083D52 File Offset: 0x00081F52
		public float CalculatedIntensity()
		{
			return Mathf.Sqrt(this.targetSize) / 10f;
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x00083D65 File Offset: 0x00081F65
		public float CalculatedShockwaveSpan()
		{
			return Mathf.Min(Mathf.Sqrt(this.targetSize) * 0.8f, this.exactScale.x) / this.exactScale.x;
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x00083D94 File Offset: 0x00081F94
		public void ForceSpawnTick(int tick)
		{
			this.setupTick = tick;
			this.ageSecs = (Find.TickManager.TicksGame - tick).TicksToSeconds();
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x00083DB4 File Offset: 0x00081FB4
		public Vector3 GetPosition()
		{
			return this.position;
		}

		// Token: 0x0400170C RID: 5900
		public const float VelocityFootstep = 1.5f;

		// Token: 0x0400170D RID: 5901
		public const float SizeFootstep = 2f;

		// Token: 0x0400170E RID: 5902
		public const float VelocityGunfire = 4f;

		// Token: 0x0400170F RID: 5903
		public const float SizeGunfire = 1f;

		// Token: 0x04001710 RID: 5904
		public const float VelocityExplosion = 20f;

		// Token: 0x04001711 RID: 5905
		public const float SizeExplosion = 6f;

		// Token: 0x04001712 RID: 5906
		public FleckDef def;

		// Token: 0x04001713 RID: 5907
		private float ageSecs;

		// Token: 0x04001714 RID: 5908
		private int setupTick;

		// Token: 0x04001715 RID: 5909
		private float targetSize;

		// Token: 0x04001716 RID: 5910
		private float velocity;

		// Token: 0x04001717 RID: 5911
		private Vector3 position;

		// Token: 0x04001718 RID: 5912
		private Vector3 spawnPosition;

		// Token: 0x04001719 RID: 5913
		private Vector3 exactScale;
	}
}
