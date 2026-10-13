using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000408 RID: 1032
	public struct FleckStatic : IFleck
	{
		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001A25 RID: 6693 RVA: 0x00083DC5 File Offset: 0x00081FC5
		public float SolidTime
		{
			get
			{
				if (this.solidTimeOverride >= 0f)
				{
					return this.solidTimeOverride;
				}
				return this.def.solidTime;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001A26 RID: 6694 RVA: 0x00083DE6 File Offset: 0x00081FE6
		public Vector3 DrawPos
		{
			get
			{
				return this.position.ExactPosition;
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001A27 RID: 6695 RVA: 0x00083DF3 File Offset: 0x00081FF3
		public float Lifespan
		{
			get
			{
				return this.def.fadeInTime + this.SolidTime + this.def.fadeOutTime;
			}
		}

		// Token: 0x170004AC RID: 1196
		// (get) Token: 0x06001A28 RID: 6696 RVA: 0x00083E13 File Offset: 0x00082013
		public bool EndOfLife
		{
			get
			{
				return this.ageSecs >= this.Lifespan;
			}
		}

		// Token: 0x170004AD RID: 1197
		// (get) Token: 0x06001A29 RID: 6697 RVA: 0x00083E28 File Offset: 0x00082028
		public float Alpha
		{
			get
			{
				float num = this.ageSecs;
				if (num <= this.def.fadeInTime)
				{
					if (this.def.fadeInTime > 0f)
					{
						return num / this.def.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (num <= this.def.fadeInTime + this.SolidTime)
					{
						return 1f;
					}
					if (this.def.fadeOutTime > 0f)
					{
						return 1f - Mathf.InverseLerp(this.def.fadeInTime + this.SolidTime, this.def.fadeInTime + this.SolidTime + this.def.fadeOutTime, num);
					}
					return 1f;
				}
			}
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001A2A RID: 6698 RVA: 0x00083EDF File Offset: 0x000820DF
		public Vector3 ExactScale
		{
			get
			{
				return Vector3.Scale(this.linearScale, this.curvedScale);
			}
		}

		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x06001A2B RID: 6699 RVA: 0x00083EF2 File Offset: 0x000820F2
		public Vector3 AddedScale
		{
			get
			{
				return this.ExactScale - this.originalScale;
			}
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x00083F08 File Offset: 0x00082108
		public void Setup(FleckCreationData creationData)
		{
			this.def = creationData.def;
			this.linearScale = Vector3.one;
			this.instanceColor = (creationData.instanceColor ?? Color.white);
			this.solidTimeOverride = (creationData.solidTimeOverride ?? -1f);
			this.skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);
			this.ageSecs = 0f;
			if (creationData.exactScale != null)
			{
				this.linearScale = creationData.exactScale.Value;
			}
			else
			{
				this.linearScale = new Vector3(creationData.scale, 1f, creationData.scale);
			}
			this.originalScale = this.ExactScale;
			this.position = new FleckDrawPosition(creationData.spawnPosition, 0f, Vector3.zero, this.def.unattachedDrawOffset);
			this.spawnPosition = creationData.spawnPosition;
			this.exactRotation = creationData.rotation;
			this.setupTick = Find.TickManager.TicksGame;
			List<CurvedScaler> scalers = this.def.scalers;
			this.curvedScale = ((scalers != null) ? scalers.ScaleAtTime(0f) : Vector3.one);
			if (creationData.ageTicksOverride != -1)
			{
				this.ForceSpawnTick(creationData.ageTicksOverride);
			}
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x00084068 File Offset: 0x00082268
		public bool TimeInterval(float deltaTime, Map map)
		{
			if (this.EndOfLife)
			{
				return true;
			}
			this.ageSecs += deltaTime;
			this.ageTicks++;
			if (this.def.growthRate != 0f)
			{
				float num = Mathf.Sign(this.linearScale.x);
				float num2 = Mathf.Sign(this.linearScale.z);
				this.linearScale = new Vector3(this.linearScale.x + num * (this.def.growthRate * deltaTime), this.linearScale.y, this.linearScale.z + num2 * (this.def.growthRate * deltaTime));
				this.linearScale.x = ((num > 0f) ? Mathf.Max(this.linearScale.x, 0.0001f) : Mathf.Min(this.linearScale.x, -0.0001f));
				this.linearScale.z = ((num2 > 0f) ? Mathf.Max(this.linearScale.z, 0.0001f) : Mathf.Min(this.linearScale.z, -0.0001f));
			}
			if (this.def.scalers != null)
			{
				this.curvedScale = this.def.scalers.ScaleAtTime(this.ageSecs);
			}
			return false;
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x000841C6 File Offset: 0x000823C6
		public void Draw(DrawBatch batch)
		{
			this.Draw(this.def.altitudeLayer.AltitudeFor(this.def.altitudeLayerIncOffset), batch);
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x000841EC File Offset: 0x000823EC
		public void Draw(float altitude, DrawBatch batch)
		{
			this.position.worldPosition.y = altitude;
			int num = this.setupTick + this.spawnPosition.GetHashCode();
			((Graphic_Fleck)this.def.GetGraphicData(num).Graphic).DrawFleck(new FleckDrawData
			{
				alpha = this.Alpha,
				color = this.instanceColor,
				drawLayer = 0,
				pos = this.DrawPos,
				rotation = this.exactRotation,
				scale = this.ExactScale,
				ageSecs = this.ageSecs,
				id = (float)num
			}, batch);
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x000842A7 File Offset: 0x000824A7
		public void ForceSpawnTick(int tick)
		{
			this.ageTicks = Find.TickManager.TicksGame - tick;
			this.ageSecs = this.ageTicks.TicksToSeconds();
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x000842CC File Offset: 0x000824CC
		public Vector3 GetPosition()
		{
			return this.position.worldPosition;
		}

		// Token: 0x0400171A RID: 5914
		public FleckDef def;

		// Token: 0x0400171B RID: 5915
		public Map map;

		// Token: 0x0400171C RID: 5916
		public FleckDrawPosition position;

		// Token: 0x0400171D RID: 5917
		public float exactRotation;

		// Token: 0x0400171E RID: 5918
		public Vector3 originalScale;

		// Token: 0x0400171F RID: 5919
		public Vector3 linearScale;

		// Token: 0x04001720 RID: 5920
		public Vector3 curvedScale;

		// Token: 0x04001721 RID: 5921
		public Color instanceColor;

		// Token: 0x04001722 RID: 5922
		public float solidTimeOverride;

		// Token: 0x04001723 RID: 5923
		public float ageSecs;

		// Token: 0x04001724 RID: 5924
		public int ageTicks;

		// Token: 0x04001725 RID: 5925
		public int setupTick;

		// Token: 0x04001726 RID: 5926
		public Vector3 spawnPosition;

		// Token: 0x04001727 RID: 5927
		public float skidSpeedMultiplierPerTick;
	}
}
