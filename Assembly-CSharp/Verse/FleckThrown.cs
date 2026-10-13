using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200040B RID: 1035
	public struct FleckThrown : IFleck
	{
		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06001A36 RID: 6710 RVA: 0x00084371 File Offset: 0x00082571
		public bool Flying
		{
			get
			{
				return this.airTimeLeft > 0f;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x00084380 File Offset: 0x00082580
		public bool Skidding
		{
			get
			{
				return !this.Flying && this.Speed > 0.01f;
			}
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x06001A38 RID: 6712 RVA: 0x00084399 File Offset: 0x00082599
		public bool Orbiting
		{
			get
			{
				return this.orbitSpeed != 0f;
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001A39 RID: 6713 RVA: 0x000843AB File Offset: 0x000825AB
		public bool Arching
		{
			get
			{
				return this.archHeight != 0f;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001A3A RID: 6714 RVA: 0x000843BD File Offset: 0x000825BD
		// (set) Token: 0x06001A3B RID: 6715 RVA: 0x000843C5 File Offset: 0x000825C5
		public Vector3 Velocity
		{
			get
			{
				return this.velocity;
			}
			set
			{
				this.velocity = value;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001A3C RID: 6716 RVA: 0x000843CE File Offset: 0x000825CE
		// (set) Token: 0x06001A3D RID: 6717 RVA: 0x000843DB File Offset: 0x000825DB
		public float MoveAngle
		{
			get
			{
				return this.velocity.AngleFlat();
			}
			set
			{
				this.SetVelocity(value, this.Speed);
			}
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x000843EC File Offset: 0x000825EC
		private void ApplyOrbit(float deltaTime, ref Vector3 nextPosition)
		{
			this.orbitAccum += deltaTime * this.orbitSpeed;
			Vector3 b = new Vector3(Mathf.Cos(this.orbitAccum), 0f, Mathf.Sin(this.orbitAccum)) * this.orbitDistance;
			Vector3 vector = this.link.Target.CenterVector3 + b;
			this.MoveAngle = (vector - this.baseData.position.worldPosition).AngleFlat();
			nextPosition = Vector3.Lerp(nextPosition, vector, this.orbitSnapStrength);
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001A3F RID: 6719 RVA: 0x0008448D File Offset: 0x0008268D
		// (set) Token: 0x06001A40 RID: 6720 RVA: 0x0008449C File Offset: 0x0008269C
		public float Speed
		{
			get
			{
				return this.velocity.MagnitudeHorizontal();
			}
			set
			{
				if (value == 0f)
				{
					this.velocity = Vector3.zero;
					return;
				}
				if (this.velocity == Vector3.zero)
				{
					this.velocity = new Vector3(value, 0f, 0f);
					return;
				}
				this.velocity = this.velocity.normalized * value;
			}
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x00084500 File Offset: 0x00082700
		public void Setup(FleckCreationData creationData)
		{
			this.baseData = default(FleckStatic);
			this.baseData.Setup(creationData);
			this.airTimeLeft = (creationData.airTimeLeft ?? 999999f);
			this.attacheeLastPosition = new Vector3(-1000f, -1000f, -1000f);
			this.link = creationData.link;
			if (this.link.Linked)
			{
				this.attacheeLastPosition = this.link.LastDrawPos;
			}
			this.rotationRate = creationData.rotationRate;
			this.SetVelocity(creationData.velocityAngle, creationData.velocitySpeed);
			if (creationData.velocity != null)
			{
				this.velocity += creationData.velocity.Value;
			}
			this.orbitSpeed = creationData.orbitSpeed;
			this.orbitSnapStrength = creationData.orbitSnapStrength;
			if (this.Orbiting)
			{
				this.orbitDistance = (this.link.Target.CenterVector3 - this.baseData.position.worldPosition).MagnitudeHorizontal();
				this.orbitAccum = Rand.Range(0f, 6.2831855f);
				if (Mathf.Abs(this.orbitSpeed) < 0.2f)
				{
					this.orbitSpeed = 0.2f * GenMath.Sign(this.orbitSpeed);
				}
			}
			this.archHeight = creationData.def.archHeight.RandomInRange;
			this.archDuration = new float?(creationData.def.archDuration.RandomInRange);
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x00084698 File Offset: 0x00082898
		public bool TimeInterval(float deltaTime, Map map)
		{
			if (this.baseData.TimeInterval(deltaTime, map))
			{
				return true;
			}
			if (!this.Flying && !this.Skidding)
			{
				return false;
			}
			if (this.baseData.def.rotateTowardsMoveDirection && this.velocity != default(Vector3))
			{
				this.baseData.exactRotation = this.velocity.AngleFlat() + this.baseData.def.rotateTowardsMoveDirectionExtraAngle;
			}
			else
			{
				this.baseData.exactRotation = this.baseData.exactRotation + this.rotationRate * deltaTime;
			}
			this.velocity += this.baseData.def.acceleration * deltaTime;
			if (this.baseData.def.speedPerTime != FloatRange.Zero)
			{
				this.Speed = Mathf.Max(this.Speed + this.baseData.def.speedPerTime.RandomInRange * deltaTime, 0f);
			}
			if (this.airTimeLeft > 0f)
			{
				this.airTimeLeft -= deltaTime;
				if (this.airTimeLeft < 0f)
				{
					this.airTimeLeft = 0f;
				}
				if (this.airTimeLeft <= 0f && !this.baseData.def.landSound.NullOrUndefined())
				{
					this.baseData.def.landSound.PlayOneShot(new TargetInfo(new IntVec3(this.baseData.position.worldPosition), map, false));
				}
			}
			if (this.Skidding)
			{
				this.Speed *= this.baseData.skidSpeedMultiplierPerTick;
				this.rotationRate *= this.baseData.skidSpeedMultiplierPerTick;
				if (this.Speed < 0.02f)
				{
					this.Speed = 0f;
				}
			}
			FleckDrawPosition fleckDrawPosition = this.NextPosition(deltaTime);
			if (this.Orbiting)
			{
				this.ApplyOrbit(deltaTime, ref fleckDrawPosition.worldPosition);
			}
			IntVec3 intVec = new IntVec3(fleckDrawPosition.worldPosition);
			if (intVec != new IntVec3(this.baseData.position.worldPosition))
			{
				if (!intVec.InBounds(map))
				{
					return true;
				}
				if (this.baseData.def.collide && intVec.Filled(map))
				{
					this.WallHit();
					return false;
				}
			}
			this.baseData.position = fleckDrawPosition;
			return false;
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x00084904 File Offset: 0x00082B04
		private FleckDrawPosition NextPosition(float deltaTime)
		{
			Vector3 vector = this.baseData.position.worldPosition + this.velocity * deltaTime;
			float height = 0f;
			Vector3 attachedOffset = Vector3.zero;
			if (this.Arching)
			{
				float x = Mathf.Clamp01(this.baseData.ageSecs / this.archDuration.Value);
				if (this.baseData.def.archCurve == null)
				{
					height = this.archHeight * GenMath.InverseParabola(x);
				}
				else
				{
					height = this.archHeight * this.baseData.def.archCurve.Evaluate(x);
				}
			}
			if (this.link.Target.HasThing)
			{
				bool flag = this.link.detachAfterTicks == -1 || this.baseData.ageTicks < this.link.detachAfterTicks;
				if (!this.link.Target.ThingDestroyed && flag)
				{
					this.link.UpdateDrawPos();
				}
				Vector3 attachedDrawOffset = this.baseData.def.attachedDrawOffset;
				if (this.baseData.def.attachedToHead)
				{
					Pawn pawn = this.link.Target.Thing as Pawn;
					if (pawn != null && pawn.story != null && pawn.SpawnedOrAnyParentSpawned)
					{
						attachedOffset = pawn.Drawer.renderer.BaseHeadOffsetAt((pawn.GetPosture() == PawnPosture.Standing) ? Rot4.North : pawn.Drawer.renderer.LayingFacing()).RotatedBy(pawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None));
					}
				}
				Vector3 b = this.link.LastDrawPos - this.attacheeLastPosition;
				vector += b;
				vector += attachedDrawOffset;
				vector.y = AltitudeLayer.MoteOverhead.AltitudeFor();
				this.attacheeLastPosition = this.link.LastDrawPos;
			}
			Vector3 anchorOffset = (new Vector3(0.5f, 0f, 0.5f) - this.baseData.def.scalingAnchor).ScaledBy(this.baseData.AddedScale);
			return new FleckDrawPosition(vector, height, anchorOffset, this.baseData.def.unattachedDrawOffset, attachedOffset);
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x00084B4D File Offset: 0x00082D4D
		public void SetVelocity(float angle, float speed)
		{
			this.velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * speed;
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x00084B70 File Offset: 0x00082D70
		public void Draw(DrawBatch batch)
		{
			this.baseData.Draw(batch);
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x00084B7E File Offset: 0x00082D7E
		private void WallHit()
		{
			this.airTimeLeft = 0f;
			this.Speed = 0f;
			this.rotationRate = 0f;
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x00084BA1 File Offset: 0x00082DA1
		public Vector3 GetPosition()
		{
			return this.baseData.position.worldPosition;
		}

		// Token: 0x0400172D RID: 5933
		public FleckStatic baseData;

		// Token: 0x0400172E RID: 5934
		public float airTimeLeft;

		// Token: 0x0400172F RID: 5935
		public Vector3 velocity;

		// Token: 0x04001730 RID: 5936
		public float rotationRate;

		// Token: 0x04001731 RID: 5937
		public FleckAttachLink link;

		// Token: 0x04001732 RID: 5938
		private Vector3 attacheeLastPosition;

		// Token: 0x04001733 RID: 5939
		public float orbitSpeed;

		// Token: 0x04001734 RID: 5940
		public float archHeight;

		// Token: 0x04001735 RID: 5941
		public float? archDuration;

		// Token: 0x04001736 RID: 5942
		public float orbitSnapStrength;

		// Token: 0x04001737 RID: 5943
		public const float MinSpeed = 0.02f;

		// Token: 0x04001738 RID: 5944
		public const float MinOrbitSpeed = 0.2f;

		// Token: 0x04001739 RID: 5945
		public float orbitDistance;

		// Token: 0x0400173A RID: 5946
		private float orbitAccum;
	}
}
