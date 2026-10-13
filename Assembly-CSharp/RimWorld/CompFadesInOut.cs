using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C86 RID: 11398
	public class CompFadesInOut : ThingComp
	{
		// Token: 0x17002861 RID: 10337
		// (get) Token: 0x0600FFDA RID: 65498 RVA: 0x004A1FBD File Offset: 0x004A01BD
		public CompProperties_FadesInOut Props
		{
			get
			{
				return (CompProperties_FadesInOut)this.props;
			}
		}

		// Token: 0x0600FFDB RID: 65499 RVA: 0x004A1FCA File Offset: 0x004A01CA
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Spawned)
			{
				this.ageTicks++;
			}
		}

		// Token: 0x0600FFDC RID: 65500 RVA: 0x004A1FF0 File Offset: 0x004A01F0
		public float Opacity()
		{
			float num = this.ageTicks.TicksToSeconds();
			if (num <= this.Props.fadeInSecs)
			{
				if (this.Props.fadeInSecs > 0f)
				{
					return num / this.Props.fadeInSecs;
				}
				return 1f;
			}
			else
			{
				if (num <= this.Props.fadeInSecs + this.Props.solidTimeSecs)
				{
					return 1f;
				}
				if (this.Props.fadeOutSecs > 0f)
				{
					return 1f - Mathf.InverseLerp(this.Props.fadeInSecs + this.Props.solidTimeSecs, this.Props.fadeInSecs + this.Props.solidTimeSecs + this.Props.fadeOutSecs, num);
				}
				return 1f;
			}
		}

		// Token: 0x0600FFDD RID: 65501 RVA: 0x004A20BB File Offset: 0x004A02BB
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
		}

		// Token: 0x0400A753 RID: 42835
		private int ageTicks;
	}
}
