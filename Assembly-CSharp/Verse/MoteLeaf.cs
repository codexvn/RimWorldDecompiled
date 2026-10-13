using System;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008C0 RID: 2240
	public class MoteLeaf : Mote
	{
		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x060038A4 RID: 14500 RVA: 0x0012DC6F File Offset: 0x0012BE6F
		protected override bool EndOfLife
		{
			get
			{
				return base.AgeSecs >= this.spawnDelay + this.FallTime + base.SolidTime + this.def.mote.fadeOutTime;
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x060038A5 RID: 14501 RVA: 0x0012DCA1 File Offset: 0x0012BEA1
		private float FallTime
		{
			get
			{
				return this.startSpatialPosition.y / MoteLeaf.FallSpeed;
			}
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x060038A6 RID: 14502 RVA: 0x0012DCB4 File Offset: 0x0012BEB4
		public override float Alpha
		{
			get
			{
				float num = base.AgeSecs;
				if (num <= this.spawnDelay)
				{
					return 0f;
				}
				num -= this.spawnDelay;
				if (num <= this.def.mote.fadeInTime)
				{
					if (this.def.mote.fadeInTime > 0f)
					{
						return num / this.def.mote.fadeInTime;
					}
					return 1f;
				}
				else
				{
					if (num <= this.FallTime + base.SolidTime)
					{
						return 1f;
					}
					num -= this.FallTime + base.SolidTime;
					if (num <= this.def.mote.fadeOutTime)
					{
						return 1f - Mathf.InverseLerp(0f, this.def.mote.fadeOutTime, num);
					}
					num -= this.def.mote.fadeOutTime;
					return 0f;
				}
			}
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x0012DD96 File Offset: 0x0012BF96
		public void Initialize(Vector3 position, float spawnDelay, bool front, float treeHeight)
		{
			this.startSpatialPosition = position;
			this.spawnDelay = spawnDelay;
			this.front = front;
			this.treeHeight = treeHeight;
			this.TimeInterval(0f);
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x0012DDC0 File Offset: 0x0012BFC0
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (base.Destroyed)
			{
				return;
			}
			float ageSecs = base.AgeSecs;
			this.exactPosition = this.startSpatialPosition;
			if (ageSecs > this.spawnDelay)
			{
				this.exactPosition.y = this.exactPosition.y - MoteLeaf.FallSpeed * (ageSecs - this.spawnDelay);
			}
			this.exactPosition.y = Mathf.Max(this.exactPosition.y, 0f);
			this.currentSpatialPosition = this.exactPosition;
			this.exactPosition.z = this.exactPosition.z + this.exactPosition.y;
			this.exactPosition.y = 0f;
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x0012DE6C File Offset: 0x0012C06C
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawMote(this.front ? (this.def.altitudeLayer.AltitudeFor() + 0.1f * GenMath.InverseLerp(0f, this.treeHeight, this.currentSpatialPosition.y) * 2f) : this.def.altitudeLayer.AltitudeFor());
		}

		// Token: 0x040029EA RID: 10730
		private Vector3 startSpatialPosition;

		// Token: 0x040029EB RID: 10731
		private Vector3 currentSpatialPosition;

		// Token: 0x040029EC RID: 10732
		private float spawnDelay;

		// Token: 0x040029ED RID: 10733
		private bool front;

		// Token: 0x040029EE RID: 10734
		private float treeHeight;

		// Token: 0x040029EF RID: 10735
		[TweakValue("Graphics", 0f, 5f)]
		private static float FallSpeed = 0.5f;
	}
}
