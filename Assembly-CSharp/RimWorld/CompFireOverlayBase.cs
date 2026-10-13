using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C89 RID: 11401
	public abstract class CompFireOverlayBase : ThingComp
	{
		// Token: 0x17002864 RID: 10340
		// (get) Token: 0x0600FFE4 RID: 65508 RVA: 0x004A21E1 File Offset: 0x004A03E1
		public CompProperties_FireOverlay Props
		{
			get
			{
				return (CompProperties_FireOverlay)this.props;
			}
		}

		// Token: 0x17002865 RID: 10341
		// (get) Token: 0x0600FFE5 RID: 65509 RVA: 0x004A21F0 File Offset: 0x004A03F0
		public float FireSize
		{
			get
			{
				if (this.startedGrowingAtTick < 0)
				{
					return this.Props.fireSize;
				}
				return Mathf.Lerp(this.Props.fireSize, this.Props.finalFireSize, (float)(GenTicks.TicksAbs - this.startedGrowingAtTick) / this.Props.fireGrowthDurationTicks);
			}
		}

		// Token: 0x0600FFE6 RID: 65510 RVA: 0x004A2246 File Offset: 0x004A0446
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.startedGrowingAtTick, "startedGrowingAtTick", -1, false);
		}

		// Token: 0x0400A758 RID: 42840
		protected int startedGrowingAtTick = -1;
	}
}
