using System;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020008C2 RID: 2242
	public class MoteThrown : Mote
	{
		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x060038B1 RID: 14513 RVA: 0x0012DFC0 File Offset: 0x0012C1C0
		protected bool Flying
		{
			get
			{
				return this.airTimeLeft > 0f;
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x060038B2 RID: 14514 RVA: 0x0012DFCF File Offset: 0x0012C1CF
		protected bool Skidding
		{
			get
			{
				return !this.Flying && this.Speed > 0.01f;
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x060038B3 RID: 14515 RVA: 0x0012DFE8 File Offset: 0x0012C1E8
		// (set) Token: 0x060038B4 RID: 14516 RVA: 0x0012DFF0 File Offset: 0x0012C1F0
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

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x060038B5 RID: 14517 RVA: 0x0012DFF9 File Offset: 0x0012C1F9
		// (set) Token: 0x060038B6 RID: 14518 RVA: 0x0012E006 File Offset: 0x0012C206
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

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x060038B7 RID: 14519 RVA: 0x0012E015 File Offset: 0x0012C215
		// (set) Token: 0x060038B8 RID: 14520 RVA: 0x0012E024 File Offset: 0x0012C224
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

		// Token: 0x060038B9 RID: 14521 RVA: 0x0012E088 File Offset: 0x0012C288
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (base.Destroyed)
			{
				return;
			}
			if (!this.Flying && !this.Skidding)
			{
				return;
			}
			Vector3 vector = this.NextExactPosition(deltaTime);
			IntVec3 intVec = new IntVec3(vector);
			if (intVec != base.Position)
			{
				if (!intVec.InBounds(base.Map))
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
				if (this.def.mote.collide && intVec.Filled(base.Map))
				{
					this.WallHit();
					return;
				}
			}
			base.Position = intVec;
			this.exactPosition = vector;
			if (this.def.mote.rotateTowardsMoveDirection && this.velocity != default(Vector3))
			{
				this.exactRotation = this.velocity.AngleFlat();
			}
			else
			{
				this.exactRotation += this.rotationRate * deltaTime;
			}
			this.velocity += this.def.mote.acceleration * deltaTime;
			if (this.def.mote.speedPerTime != 0f)
			{
				this.Speed = Mathf.Max(this.Speed + this.def.mote.speedPerTime * deltaTime, 0f);
			}
			if (this.airTimeLeft > 0f)
			{
				this.airTimeLeft -= deltaTime;
				if (this.airTimeLeft < 0f)
				{
					this.airTimeLeft = 0f;
				}
				if (this.airTimeLeft <= 0f && !this.def.mote.landSound.NullOrUndefined())
				{
					this.def.mote.landSound.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
			}
			if (this.Skidding)
			{
				this.Speed *= this.skidSpeedMultiplierPerTick;
				this.rotationRate *= this.skidSpeedMultiplierPerTick;
				if (this.Speed < 0.02f)
				{
					this.Speed = 0f;
				}
			}
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x0012E29F File Offset: 0x0012C49F
		protected virtual Vector3 NextExactPosition(float deltaTime)
		{
			return this.exactPosition + this.velocity * deltaTime;
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x0012E2B8 File Offset: 0x0012C4B8
		public void SetVelocity(float angle, float speed)
		{
			this.velocity = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward * speed;
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x0012E2DB File Offset: 0x0012C4DB
		protected virtual void WallHit()
		{
			this.airTimeLeft = 0f;
			this.Speed = 0f;
			this.rotationRate = 0f;
		}

		// Token: 0x040029F3 RID: 10739
		public float airTimeLeft = 999999f;

		// Token: 0x040029F4 RID: 10740
		protected Vector3 velocity = Vector3.zero;
	}
}
