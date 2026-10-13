using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022E4 RID: 8932
	public class PawnBreathMoteMaker
	{
		// Token: 0x0600C228 RID: 49704 RVA: 0x003849A1 File Offset: 0x00382BA1
		public PawnBreathMoteMaker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600C229 RID: 49705 RVA: 0x003849B0 File Offset: 0x00382BB0
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (!this.pawn.RaceProps.Humanlike || this.pawn.RaceProps.IsMechanoid || this.pawn.IsShambler)
			{
				return;
			}
			if (ModsConfig.OdysseyActive && this.pawn.Position.GetVacuum(this.pawn.MapHeld) > 0f)
			{
				return;
			}
			int num = Mathf.Abs(Find.TickManager.TicksGame + this.pawn.HashOffset()) % 320;
			if (num < ticksPassed)
			{
				this.doThisBreath = (this.pawn.AmbientTemperature < 0f && this.pawn.GetPosture() == PawnPosture.Standing);
			}
			if (this.doThisBreath && num < 80 && num % 8 < ticksPassed)
			{
				this.TryMakeBreathMote();
			}
		}

		// Token: 0x0600C22A RID: 49706 RVA: 0x00384A84 File Offset: 0x00382C84
		private void TryMakeBreathMote()
		{
			Vector3 loc = this.pawn.Drawer.DrawPos + this.pawn.Drawer.renderer.BaseHeadOffsetAt(this.pawn.Rotation) + this.pawn.Rotation.FacingCell.ToVector3() * 0.21f + PawnBreathMoteMaker.BreathOffset;
			Vector3 lastTickTweenedVelocity = this.pawn.Drawer.tweener.LastTickTweenedVelocity;
			FleckMaker.ThrowBreathPuff(loc, this.pawn.Map, this.pawn.Rotation.AsAngle, lastTickTweenedVelocity);
		}

		// Token: 0x0400848B RID: 33931
		private Pawn pawn;

		// Token: 0x0400848C RID: 33932
		private bool doThisBreath;

		// Token: 0x0400848D RID: 33933
		private const int BreathDuration = 80;

		// Token: 0x0400848E RID: 33934
		private const int BreathInterval = 320;

		// Token: 0x0400848F RID: 33935
		private const int MoteInterval = 8;

		// Token: 0x04008490 RID: 33936
		private const float MaxBreathTemperature = 0f;

		// Token: 0x04008491 RID: 33937
		private const float BreathRotationOffsetDist = 0.21f;

		// Token: 0x04008492 RID: 33938
		private static readonly Vector3 BreathOffset = new Vector3(0f, 0f, -0.04f);
	}
}
