using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005DA RID: 1498
	public class PawnLeaner
	{
		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x0600266D RID: 9837 RVA: 0x000CB3B8 File Offset: 0x000C95B8
		public Vector3 LeanOffset
		{
			get
			{
				return this.shootSourceOffset.ToVector3() * 0.5f * this.leanOffsetCurPct;
			}
		}

		// Token: 0x0600266E RID: 9838 RVA: 0x000CB3DA File Offset: 0x000C95DA
		public PawnLeaner(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600266F RID: 9839 RVA: 0x000CB3F8 File Offset: 0x000C95F8
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (this.ShouldLean())
			{
				this.leanOffsetCurPct += 0.075f * (float)ticksPassed;
				if (this.leanOffsetCurPct > 1f)
				{
					this.leanOffsetCurPct = 1f;
					return;
				}
			}
			else
			{
				this.leanOffsetCurPct -= 0.075f * (float)ticksPassed;
				if (this.leanOffsetCurPct < 0f)
				{
					this.leanOffsetCurPct = 0f;
				}
			}
		}

		// Token: 0x06002670 RID: 9840 RVA: 0x000CB468 File Offset: 0x000C9668
		public bool ShouldLean()
		{
			return this.pawn.stances.curStance is Stance_Busy && !(this.shootSourceOffset == new IntVec3(0, 0, 0));
		}

		// Token: 0x06002671 RID: 9841 RVA: 0x000CB49B File Offset: 0x000C969B
		public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 shootPosition)
		{
			this.shootSourceOffset = newShootLine.Source - this.pawn.Position;
		}

		// Token: 0x04001FD0 RID: 8144
		private readonly Pawn pawn;

		// Token: 0x04001FD1 RID: 8145
		private IntVec3 shootSourceOffset = new IntVec3(0, 0, 0);

		// Token: 0x04001FD2 RID: 8146
		private float leanOffsetCurPct;

		// Token: 0x04001FD3 RID: 8147
		private const float LeanOffsetPctChangeRate = 0.075f;

		// Token: 0x04001FD4 RID: 8148
		private const float LeanOffsetDistanceMultiplier = 0.5f;
	}
}
