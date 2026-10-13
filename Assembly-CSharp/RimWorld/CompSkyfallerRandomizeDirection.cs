using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F1D RID: 12061
	public class CompSkyfallerRandomizeDirection : ThingComp
	{
		// Token: 0x17002BF0 RID: 11248
		// (get) Token: 0x0601106A RID: 69738 RVA: 0x004E5B7B File Offset: 0x004E3D7B
		public CompProperties_SkyfallerRandomizeDirection Props
		{
			get
			{
				return (CompProperties_SkyfallerRandomizeDirection)this.props;
			}
		}

		// Token: 0x17002BF1 RID: 11249
		// (get) Token: 0x0601106B RID: 69739 RVA: 0x004E5B88 File Offset: 0x004E3D88
		public Skyfaller Skyfaller
		{
			get
			{
				return (Skyfaller)this.parent;
			}
		}

		// Token: 0x17002BF2 RID: 11250
		// (get) Token: 0x0601106C RID: 69740 RVA: 0x004E5B95 File Offset: 0x004E3D95
		public Vector3 Offset
		{
			get
			{
				return this.currentOffset;
			}
		}

		// Token: 0x17002BF3 RID: 11251
		// (get) Token: 0x0601106D RID: 69741 RVA: 0x004E5BA0 File Offset: 0x004E3DA0
		public float ExtraDrawAngle
		{
			get
			{
				return Mathf.Lerp(this.lastAngle, this.currentAngle, Mathf.Clamp((float)(Find.TickManager.TicksGame + CompSkyfallerRandomizeDirection.DirectionChangeBlendDuration / 2 - this.lastDirectionChange), 0f, (float)CompSkyfallerRandomizeDirection.DirectionChangeBlendDuration) / (float)CompSkyfallerRandomizeDirection.DirectionChangeBlendDuration) / 2f;
			}
		}

		// Token: 0x0601106E RID: 69742 RVA: 0x004E5BF5 File Offset: 0x004E3DF5
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.initialAngle = this.Skyfaller.angle;
			this.directionChangeInterval = this.Props.directionChangeInterval.RandomInRange;
			this.lastDirectionChange = Find.TickManager.TicksGame;
		}

		// Token: 0x0601106F RID: 69743 RVA: 0x004E5C34 File Offset: 0x004E3E34
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(this.directionChangeInterval))
			{
				this.lastAngle = this.currentAngle;
				this.currentAngle = Rand.Value * this.Props.maxDeviationFromStartingAngle * (float)Rand.Sign;
				this.lastDirectionChange = Find.TickManager.TicksGame;
			}
			Quaternion rotation = Quaternion.AngleAxis(this.currentAngle - this.initialAngle, Vector3.up);
			this.currentOffset += rotation * Vector3.forward * Time.deltaTime;
		}

		// Token: 0x06011070 RID: 69744 RVA: 0x004E5CD4 File Offset: 0x004E3ED4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.directionChangeInterval, "directionChangeInterval", 0, false);
			Scribe_Values.Look<float>(ref this.currentAngle, "currentAngle", 0f, false);
			Scribe_Values.Look<float>(ref this.lastAngle, "lastAngle", 0f, false);
			Scribe_Values.Look<Vector3>(ref this.currentOffset, "currentOffset", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.initialAngle, "initialAngle", 0f, false);
		}

		// Token: 0x0400B0F3 RID: 45299
		private static int DirectionChangeBlendDuration = 100;

		// Token: 0x0400B0F4 RID: 45300
		private int directionChangeInterval;

		// Token: 0x0400B0F5 RID: 45301
		private int lastDirectionChange;

		// Token: 0x0400B0F6 RID: 45302
		private float initialAngle;

		// Token: 0x0400B0F7 RID: 45303
		private float currentAngle;

		// Token: 0x0400B0F8 RID: 45304
		private float lastAngle;

		// Token: 0x0400B0F9 RID: 45305
		private Vector3 currentOffset;
	}
}
