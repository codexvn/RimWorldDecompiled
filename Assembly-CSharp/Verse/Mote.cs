using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008BB RID: 2235
	public abstract class Mote : Thing
	{
		// Token: 0x17000A55 RID: 2645
		// (set) Token: 0x0600387B RID: 14459 RVA: 0x0012CE75 File Offset: 0x0012B075
		public float Scale
		{
			set
			{
				this.linearScale = new Vector3(value, 1f, value);
			}
		}

		// Token: 0x17000A56 RID: 2646
		// (get) Token: 0x0600387C RID: 14460 RVA: 0x0012CE89 File Offset: 0x0012B089
		public float AgeSecs
		{
			get
			{
				if (this.def.mote.realTime)
				{
					return Time.realtimeSinceStartup - this.spawnRealTime;
				}
				return (float)(Find.TickManager.TicksGame - this.spawnTick - this.pausedTicks) / 60f;
			}
		}

		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x0600387D RID: 14461 RVA: 0x0012CEC9 File Offset: 0x0012B0C9
		public float AgeSecsPausable
		{
			get
			{
				return (float)this.currentAnimationTick / 60f;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x0600387E RID: 14462 RVA: 0x0012CED8 File Offset: 0x0012B0D8
		protected float SolidTime
		{
			get
			{
				if (this.solidTimeOverride >= 0f)
				{
					return this.solidTimeOverride;
				}
				return this.def.mote.solidTime;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x0600387F RID: 14463 RVA: 0x0012CEFE File Offset: 0x0012B0FE
		public Vector3 ExactScale
		{
			get
			{
				return Vector3.Scale(this.linearScale, this.curvedScale);
			}
		}

		// Token: 0x17000A5A RID: 2650
		// (get) Token: 0x06003880 RID: 14464 RVA: 0x0012CF14 File Offset: 0x0012B114
		public override Vector3 DrawPos
		{
			get
			{
				float z = 0f;
				if (this.def.mote.archDuration > 0f && this.AgeSecs < this.def.mote.archDuration + this.def.mote.archStartOffset)
				{
					z = (Mathf.Cos(Mathf.Clamp01((this.AgeSecs + this.def.mote.archStartOffset) / this.def.mote.archDuration) * 3.1415927f * 2f - 3.1415927f) + 1f) / 2f * this.def.mote.archHeight;
				}
				int num = this.GetHashCode();
				if (num == -2147483648)
				{
					num++;
				}
				float y = (float)Mathf.Abs(num) / 2.1474836E+09f * 0.03658537f * this.def.mote.yFightingOffsetScalar01;
				return this.exactPosition + this.def.mote.unattachedDrawOffset + new Vector3(0f, y, z);
			}
		}

		// Token: 0x17000A5B RID: 2651
		// (get) Token: 0x06003881 RID: 14465 RVA: 0x0012D030 File Offset: 0x0012B230
		protected virtual bool EndOfLife
		{
			get
			{
				return this.AgeSecs >= this.def.mote.Lifespan;
			}
		}

		// Token: 0x17000A5C RID: 2652
		// (get) Token: 0x06003882 RID: 14466 RVA: 0x0012D050 File Offset: 0x0012B250
		public virtual float Alpha
		{
			get
			{
				float ageSecs = this.AgeSecs;
				if (this.def.mote.fadeOutUnmaintained && Find.TickManager.TicksGame - this.lastMaintainTick > 0)
				{
					if (this.def.mote.fadeOutTime > 0f)
					{
						float num = (Find.TickManager.TicksGame - this.lastMaintainTick).TicksToSeconds();
						return 1f - num / this.def.mote.fadeOutTime;
					}
					return 1f;
				}
				else if (ageSecs <= this.def.mote.fadeInTime)
				{
					if (this.def.mote.fadeInTime > 0f)
					{
						return ageSecs / this.def.mote.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (ageSecs <= this.def.mote.fadeInTime + this.SolidTime)
					{
						return 1f;
					}
					if (this.def.mote.fadeOutTime > 0f)
					{
						return 1f - Mathf.InverseLerp(this.def.mote.fadeInTime + this.SolidTime, this.def.mote.fadeInTime + this.SolidTime + this.def.mote.fadeOutTime, ageSecs);
					}
					return 1f;
				}
			}
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x0012D1A4 File Offset: 0x0012B3A4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.spawnTick = Find.TickManager.TicksGame;
			this.spawnRealTime = Time.realtimeSinceStartup;
			RealTime.moteList.MoteSpawned(this);
			base.Map.moteCounter.Notify_MoteSpawned();
			if (this.exactPosition == Vector3.zero)
			{
				this.exactPosition = base.Position.ToVector3();
				this.exactPosition.y = this.def.altitudeLayer.AltitudeFor() + this.yOffset;
			}
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x0012D237 File Offset: 0x0012B437
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			RealTime.moteList.MoteDespawned(this);
			map.moteCounter.Notify_MoteDespawned();
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x0012D25C File Offset: 0x0012B45C
		protected override void Tick()
		{
			if (!this.def.mote.realTime)
			{
				this.TimeInterval(0.016666668f);
			}
			if (!this.animationPaused)
			{
				this.currentAnimationTick++;
			}
			if (this.paused)
			{
				this.pausedTicks++;
			}
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x0012D2B2 File Offset: 0x0012B4B2
		public void RealtimeUpdate()
		{
			if (this.def.mote.realTime)
			{
				this.TimeInterval(Time.deltaTime);
			}
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x0012D2D4 File Offset: 0x0012B4D4
		protected virtual void TimeInterval(float deltaTime)
		{
			if (this.EndOfLife && !base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.def.mote.needsMaintenance && Find.TickManager.TicksGame > this.lastMaintainTick)
			{
				int num = this.def.mote.fadeOutTime.SecondsToTicks();
				if (!this.def.mote.fadeOutUnmaintained || Find.TickManager.TicksGame - this.lastMaintainTick > num)
				{
					this.Destroy(DestroyMode.Vanish);
					return;
				}
			}
			if (this.def.mote.growthRate != 0f)
			{
				this.linearScale = new Vector3(this.linearScale.x + this.def.mote.growthRate * deltaTime, this.linearScale.y, this.linearScale.z + this.def.mote.growthRate * deltaTime);
				this.linearScale.x = Mathf.Max(this.linearScale.x, 0.0001f);
				this.linearScale.z = Mathf.Max(this.linearScale.z, 0.0001f);
			}
			if (this.def.mote.scalers != null)
			{
				this.curvedScale = this.def.mote.scalers.ScaleAtTime(this.AgeSecs);
			}
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x0012D440 File Offset: 0x0012B640
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.DrawMote(this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x0012D458 File Offset: 0x0012B658
		protected void DrawMote(float altitude)
		{
			if (this.paused || Find.UIRoot.HideMotes)
			{
				return;
			}
			this.exactPosition.y = altitude + this.yOffset;
			base.DrawAt(this.exactPosition, false);
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x0012D48F File Offset: 0x0012B68F
		public void Maintain()
		{
			this.lastMaintainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x0012D4A1 File Offset: 0x0012B6A1
		public void Attach(TargetInfo a, Vector3 offset, bool rotateWithTarget = false)
		{
			this.link1 = new MoteAttachLink(a, offset, rotateWithTarget);
		}

		// Token: 0x0600388C RID: 14476 RVA: 0x0012D4B1 File Offset: 0x0012B6B1
		public void Attach(TargetInfo a)
		{
			this.link1 = new MoteAttachLink(a, Vector3.zero, false);
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x0012D4C5 File Offset: 0x0012B6C5
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			RealTime.moteList.MoteDespawned(this);
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x0012D4D8 File Offset: 0x0012B6D8
		public void ForceSpawnTick(int tick)
		{
			this.spawnTick = tick;
		}

		// Token: 0x040029CF RID: 10703
		public Vector3 exactPosition;

		// Token: 0x040029D0 RID: 10704
		public float exactRotation;

		// Token: 0x040029D1 RID: 10705
		public Vector3 linearScale = new Vector3(1f, 1f, 1f);

		// Token: 0x040029D2 RID: 10706
		public Vector3 curvedScale = new Vector3(1f, 1f, 1f);

		// Token: 0x040029D3 RID: 10707
		public float rotationRate;

		// Token: 0x040029D4 RID: 10708
		public float yOffset;

		// Token: 0x040029D5 RID: 10709
		public Color instanceColor = Color.white;

		// Token: 0x040029D6 RID: 10710
		private int lastMaintainTick;

		// Token: 0x040029D7 RID: 10711
		private int currentAnimationTick;

		// Token: 0x040029D8 RID: 10712
		public float solidTimeOverride = -1f;

		// Token: 0x040029D9 RID: 10713
		public int pausedTicks;

		// Token: 0x040029DA RID: 10714
		public bool paused;

		// Token: 0x040029DB RID: 10715
		public int spawnTick;

		// Token: 0x040029DC RID: 10716
		public bool animationPaused;

		// Token: 0x040029DD RID: 10717
		public int detachAfterTicks = -1;

		// Token: 0x040029DE RID: 10718
		public float spawnRealTime;

		// Token: 0x040029DF RID: 10719
		public MoteAttachLink link1 = MoteAttachLink.Invalid;

		// Token: 0x040029E0 RID: 10720
		protected float skidSpeedMultiplierPerTick = Rand.Range(0.3f, 0.95f);

		// Token: 0x040029E1 RID: 10721
		public int offsetRandom = Rand.Range(0, 99999);

		// Token: 0x040029E2 RID: 10722
		protected const float MinSpeed = 0.02f;
	}
}
