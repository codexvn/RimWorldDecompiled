using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005DE RID: 1502
	public class PawnTweener
	{
		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x0600267E RID: 9854 RVA: 0x000CBA62 File Offset: 0x000C9C62
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x0600267F RID: 9855 RVA: 0x000CBA6A File Offset: 0x000C9C6A
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x06002680 RID: 9856 RVA: 0x000CBA80 File Offset: 0x000C9C80
		public PawnTweener(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x000CBAD4 File Offset: 0x000C9CD4
		public void PreDrawPosCalculation()
		{
			if (this.lastDrawFrame == RealTime.frameCount)
			{
				return;
			}
			if (!this.pawn.Spawned)
			{
				this.tweenedPos = this.pawn.Position.ToVector3Shifted();
				return;
			}
			if (this.lastDrawFrame < RealTime.frameCount - 1 && this.lastDrawTick < GenTicks.TicksGame - 1)
			{
				this.ResetTweenedPosToRoot();
			}
			else
			{
				this.lastTickSpringPos = this.tweenedPos;
				float tickRateMultiplier = Find.TickManager.TickRateMultiplier;
				if (tickRateMultiplier < 5f)
				{
					Vector3 a = this.TweenedPosRoot() - this.tweenedPos;
					float num = 0.09f * (RealTime.deltaTime * 60f * tickRateMultiplier);
					if (RealTime.deltaTime > 0.05f)
					{
						num = Mathf.Min(num, 1f);
					}
					this.tweenedPos += a * num;
				}
				else
				{
					this.ResetTweenedPosToRoot();
				}
			}
			this.lastDrawFrame = RealTime.frameCount;
			this.lastDrawTick = GenTicks.TicksGame;
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x000CBBCF File Offset: 0x000C9DCF
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot();
			this.lastTickSpringPos = this.tweenedPos;
			this.lastDrawFrame = RealTime.frameCount;
			this.lastDrawTick = GenTicks.TicksGame;
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x000CBBFF File Offset: 0x000C9DFF
		public void Notify_Teleported()
		{
			this.lastDrawFrame = -1;
			this.lastDrawTick = -1;
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x000CBC10 File Offset: 0x000C9E10
		private Vector3 TweenedPosRoot()
		{
			if (!this.pawn.Spawned)
			{
				return this.pawn.Position.ToVector3Shifted();
			}
			float z = 0f;
			if (this.pawn.Spawned && this.pawn.ageTracker.CurLifeStage.sittingOffset != null && !this.pawn.pather.MovingNow && this.pawn.GetPosture() == PawnPosture.Standing)
			{
				Building edifice = this.pawn.Position.GetEdifice(this.pawn.Map);
				if (edifice != null && edifice.def.building != null && edifice.def.building.isSittable)
				{
					z = this.pawn.ageTracker.CurLifeStage.sittingOffset.Value;
				}
			}
			float num = this.pawn.pather.MovePercentage;
			if (this.pawn.Crawling)
			{
				num = num - num % 0.33333334f + Mathf.Pow(num % 0.33333334f * 3f, 3f) * 0.33333334f;
			}
			Vector3 b;
			if (GenTicks.TicksGame == this.lastOffsetTick)
			{
				b = this.lastOffset;
			}
			else
			{
				b = (this.lastOffset = PawnCollisionTweenerUtility.PawnCollisionPosOffsetFor(this.pawn));
				this.lastOffsetTick = GenTicks.TicksGame;
			}
			return this.pawn.pather.nextCell.ToVector3Shifted() * num + this.pawn.Position.ToVector3Shifted() * (1f - num) + new Vector3(0f, 0f, z) + b;
		}

		// Token: 0x04001FE6 RID: 8166
		private Pawn pawn;

		// Token: 0x04001FE7 RID: 8167
		private Vector3 tweenedPos = new Vector3(0f, 0f, 0f);

		// Token: 0x04001FE8 RID: 8168
		private int lastDrawFrame = -1;

		// Token: 0x04001FE9 RID: 8169
		private int lastDrawTick = -1;

		// Token: 0x04001FEA RID: 8170
		private int lastOffsetTick = -1;

		// Token: 0x04001FEB RID: 8171
		private Vector3 lastOffset = Vector3.zero;

		// Token: 0x04001FEC RID: 8172
		private Vector3 lastTickSpringPos;

		// Token: 0x04001FED RID: 8173
		private const float SpringTightness = 0.09f;

		// Token: 0x04001FEE RID: 8174
		private const float CrawlsPerTile = 3f;

		// Token: 0x04001FEF RID: 8175
		private const float CrawlingLurchFactor = 3f;
	}
}
