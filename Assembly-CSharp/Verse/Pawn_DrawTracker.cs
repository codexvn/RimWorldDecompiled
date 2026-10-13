using System;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005E1 RID: 1505
	public class Pawn_DrawTracker
	{
		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x0600268B RID: 9867 RVA: 0x000CC3FC File Offset: 0x000CA5FC
		public Vector3 DrawPos
		{
			get
			{
				this.tweener.PreDrawPosCalculation();
				return (this.tweener.TweenedPos + this.jitterer.CurrentOffset + this.leaner.LeanOffset + this.OffsetForcedByJob() + this.FlyingOffset()).WithY(this.pawn.def.Altitude + this.FlightYOffset() + this.SeededYOffset);
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x0600268C RID: 9868 RVA: 0x000CC478 File Offset: 0x000CA678
		public float SeededYOffset { get; }

		// Token: 0x0600268D RID: 9869 RVA: 0x000CC480 File Offset: 0x000CA680
		public Pawn_DrawTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.tweener = new PawnTweener(pawn);
			this.jitterer = new JitterHandler();
			this.leaner = new PawnLeaner(pawn);
			this.renderer = new PawnRenderer(pawn);
			this.ui = new PawnUIOverlay(pawn);
			this.footprintMaker = new PawnFootprintMaker(pawn);
			this.breathMoteMaker = new PawnBreathMoteMaker(pawn);
			this.waterRippleMaker = new PawnWaterRippleMaker(pawn);
			this.SeededYOffset = Rand.RangeSeeded(-0.03658537f, 0.03658537f, pawn.thingIDNumber);
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x000CC514 File Offset: 0x000CA714
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			this.jitterer.ProcessPostTickVisuals(ticksPassed);
			this.footprintMaker.ProcessPostTickVisuals(ticksPassed);
			this.breathMoteMaker.ProcessPostTickVisuals(ticksPassed);
			this.waterRippleMaker.ProcessPostTickVisuals(ticksPassed);
			this.leaner.ProcessPostTickVisuals(ticksPassed);
			this.renderer.ProcessPostTickVisuals(ticksPassed);
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x000CC578 File Offset: 0x000CA778
		public void DrawShadowAt(Vector3 loc)
		{
			using (new ProfilerBlock("Draw Shadow At()"))
			{
				this.renderer.RenderShadowOnlyAt(loc);
			}
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x000CC5C0 File Offset: 0x000CA7C0
		private Vector3 OffsetForcedByJob()
		{
			Pawn_JobTracker jobs = this.pawn.jobs;
			if (((jobs != null) ? jobs.curDriver : null) != null)
			{
				return this.pawn.jobs.curDriver.ForcedBodyOffset;
			}
			return Vector3.zero;
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x000CC5F6 File Offset: 0x000CA7F6
		private Vector3 FlyingOffset()
		{
			if (this.pawn.flight == null)
			{
				return Vector3.zero;
			}
			return new Vector3(0f, 0f, 0.6f) * this.pawn.flight.PositionOffsetFactor;
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x000CC634 File Offset: 0x000CA834
		private float FlightYOffset()
		{
			if (this.pawn.flight == null)
			{
				return 0f;
			}
			return 0.03658537f * this.pawn.flight.PositionOffsetFactor;
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x000CC65F File Offset: 0x000CA85F
		public void Notify_Spawned()
		{
			this.tweener.ResetTweenedPosToRoot();
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x000CC66C File Offset: 0x000CA86C
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
		{
			this.leaner.Notify_WarmingCastAlongLine(newShootLine, ShootPosition);
		}

		// Token: 0x06002695 RID: 9877 RVA: 0x000CC67B File Offset: 0x000CA87B
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (this.pawn.Destroyed || !this.pawn.Spawned)
			{
				return;
			}
			this.jitterer.Notify_DamageApplied(dinfo);
			this.renderer.Notify_DamageApplied(dinfo);
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x000CC6B0 File Offset: 0x000CA8B0
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (this.pawn.Destroyed)
			{
				return;
			}
			this.jitterer.Notify_DamageDeflected(dinfo);
		}

		// Token: 0x06002697 RID: 9879 RVA: 0x000CC6CC File Offset: 0x000CA8CC
		public void Notify_MeleeAttackOn(Thing Target)
		{
			if (Target.Position != this.pawn.Position)
			{
				this.jitterer.AddOffset(0.5f, (Target.Position - this.pawn.Position).AngleFlat);
				return;
			}
			if (Target.DrawPos != this.pawn.DrawPos)
			{
				this.jitterer.AddOffset(0.25f, (Target.DrawPos - this.pawn.DrawPos).AngleFlat());
			}
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x000CC764 File Offset: 0x000CA964
		public void Notify_DebugAffected()
		{
			for (int i = 0; i < 10; i++)
			{
				FleckMaker.ThrowAirPuffUp(this.pawn.DrawPosHeld.Value, this.pawn.MapHeld);
			}
			this.jitterer.AddOffset(0.05f, (float)Rand.Range(0, 360));
		}

		// Token: 0x04001FF6 RID: 8182
		private Pawn pawn;

		// Token: 0x04001FF7 RID: 8183
		public PawnTweener tweener;

		// Token: 0x04001FF8 RID: 8184
		private JitterHandler jitterer;

		// Token: 0x04001FF9 RID: 8185
		public PawnLeaner leaner;

		// Token: 0x04001FFA RID: 8186
		public PawnRenderer renderer;

		// Token: 0x04001FFB RID: 8187
		public PawnUIOverlay ui;

		// Token: 0x04001FFC RID: 8188
		private PawnFootprintMaker footprintMaker;

		// Token: 0x04001FFD RID: 8189
		private PawnBreathMoteMaker breathMoteMaker;

		// Token: 0x04001FFE RID: 8190
		private PawnWaterRippleMaker waterRippleMaker;

		// Token: 0x04001FFF RID: 8191
		private const float MeleeJitterDistance = 0.5f;
	}
}
