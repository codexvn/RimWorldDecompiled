using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001975 RID: 6517
	public class CompNoctolEyes : ThingComp
	{
		// Token: 0x17001670 RID: 5744
		// (get) Token: 0x060093E6 RID: 37862 RVA: 0x002B7C69 File Offset: 0x002B5E69
		public CompProperties_NoctolEyes Props
		{
			get
			{
				return (CompProperties_NoctolEyes)this.props;
			}
		}

		// Token: 0x17001671 RID: 5745
		// (get) Token: 0x060093E7 RID: 37863 RVA: 0x002B7C76 File Offset: 0x002B5E76
		public Pawn Parent
		{
			get
			{
				if (this.parentPawn == null)
				{
					this.parentPawn = (this.parent as Pawn);
				}
				return this.parentPawn;
			}
		}

		// Token: 0x17001672 RID: 5746
		// (get) Token: 0x060093E8 RID: 37864 RVA: 0x002B7C97 File Offset: 0x002B5E97
		public float EyeBrightness
		{
			get
			{
				if (!this.Parent.Downed)
				{
					return 1f - this.hediff.Severity;
				}
				return 0f;
			}
		}

		// Token: 0x060093E9 RID: 37865 RVA: 0x002B7CC0 File Offset: 0x002B5EC0
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Spawned)
			{
				if (this.mote == null || this.mote.Destroyed)
				{
					this.mote = MoteMaker.MakeAttachedOverlay(this.parent, this.Props.moteDef, Vector3.zero, 1f, -1f);
					this.mote.link1.rotateWithTarget = true;
				}
				if (this.hediff == null)
				{
					this.Parent.health.hediffSet.TryGetHediff(HediffDefOf.LightExposure, out this.hediff);
				}
				this.mote.instanceColor = new Color(1f, 1f, 1f, this.EyeBrightness);
				this.mote.Maintain();
			}
		}

		// Token: 0x04005F73 RID: 24435
		private Mote mote;

		// Token: 0x04005F74 RID: 24436
		private Hediff hediff;

		// Token: 0x04005F75 RID: 24437
		private Pawn parentPawn;
	}
}
