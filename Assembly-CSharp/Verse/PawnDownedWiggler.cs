using System;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005EF RID: 1519
	public class PawnDownedWiggler
	{
		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x060026E1 RID: 9953 RVA: 0x000CE28C File Offset: 0x000CC48C
		private static float RandomDownedAngle
		{
			get
			{
				float num = Rand.Range(45f, 135f);
				if (Rand.Value < 0.5f)
				{
					num += 180f;
				}
				return num;
			}
		}

		// Token: 0x060026E2 RID: 9954 RVA: 0x000CE2BE File Offset: 0x000CC4BE
		public PawnDownedWiggler(Pawn pawn)
		{
			this.pawn = pawn;
			this.wiggleOffset = Mathf.Abs(pawn.HashOffset()) % 600;
		}

		// Token: 0x060026E3 RID: 9955 RVA: 0x000CE2F0 File Offset: 0x000CC4F0
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			if (this.pawn.Downed && this.pawn.Spawned && !this.pawn.InBed())
			{
				this.ticksToIncapIcon -= ticksPassed;
				if (this.ticksToIncapIcon <= 0)
				{
					this.ticksToIncapIcon = 200 + this.ticksToIncapIcon;
					if (HealthAIUtility.WantsToBeRescued(this.pawn))
					{
						FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.IncapIcon, 0.42f);
					}
				}
				if (this.pawn.Awake())
				{
					if (ModsConfig.BiotechActive)
					{
						Job curJob = this.pawn.CurJob;
						if (((curJob != null) ? curJob.def : null) == JobDefOf.Breastfeed)
						{
							return;
						}
					}
					if (ticksPassed > 600)
					{
						Log.Warning("Too many ticks passed during a single frame for sensical wiggling");
					}
					int num = (Find.TickManager.TicksGame + this.wiggleOffset) % 600;
					this.ProcessWigglePeriod(num, ticksPassed, true);
					this.ProcessWigglePeriod((num + 300) % 600, ticksPassed, false);
				}
			}
		}

		// Token: 0x060026E4 RID: 9956 RVA: 0x000CE404 File Offset: 0x000CC604
		private void ProcessWigglePeriod(int wigglePeriodTick, int ticksPassed, bool positiveAngleWiggle)
		{
			int a = 0;
			int a2 = 90;
			int b = wigglePeriodTick - ticksPassed;
			int num = Mathf.Min(a2, wigglePeriodTick) - Mathf.Max(a, b);
			if (num > 0)
			{
				this.downedAngle += 0.35f * (float)num * (float)(positiveAngleWiggle ? 1 : -1);
			}
		}

		// Token: 0x060026E5 RID: 9957 RVA: 0x000CE44A File Offset: 0x000CC64A
		public void SetToCustomRotation(float rot)
		{
			this.downedAngle = rot;
			this.usingCustomRotation = true;
		}

		// Token: 0x060026E6 RID: 9958 RVA: 0x000CE45C File Offset: 0x000CC65C
		public void Notify_DamageApplied(DamageInfo dam)
		{
			if ((this.pawn.Downed || this.pawn.Dead) && dam.Def.hasForcefulImpact)
			{
				this.downedAngle += 10f * Rand.Range(-1f, 1f);
				if (!this.usingCustomRotation)
				{
					if (this.downedAngle > 315f)
					{
						this.downedAngle = 315f;
					}
					if (this.downedAngle < 45f)
					{
						this.downedAngle = 45f;
					}
					if (this.downedAngle > 135f && this.downedAngle < 225f)
					{
						if (this.downedAngle > 180f)
						{
							this.downedAngle = 225f;
							return;
						}
						this.downedAngle = 135f;
						return;
					}
				}
				else
				{
					if (this.downedAngle >= 360f)
					{
						this.downedAngle -= 360f;
					}
					if (this.downedAngle < 0f)
					{
						this.downedAngle += 360f;
					}
				}
			}
		}

		// Token: 0x04002038 RID: 8248
		private Pawn pawn;

		// Token: 0x04002039 RID: 8249
		public float downedAngle = PawnDownedWiggler.RandomDownedAngle;

		// Token: 0x0400203A RID: 8250
		public int ticksToIncapIcon;

		// Token: 0x0400203B RID: 8251
		private bool usingCustomRotation;

		// Token: 0x0400203C RID: 8252
		private int wiggleOffset;

		// Token: 0x0400203D RID: 8253
		private const float DownedAngleWidth = 45f;

		// Token: 0x0400203E RID: 8254
		private const float DamageTakenDownedAngleShift = 10f;

		// Token: 0x0400203F RID: 8255
		private const int IncapWigglePeriod = 300;

		// Token: 0x04002040 RID: 8256
		private const int IncapWiggleLength = 90;

		// Token: 0x04002041 RID: 8257
		private const float IncapWiggleSpeed = 0.35f;

		// Token: 0x04002042 RID: 8258
		private const int TicksBetweenIncapIcons = 200;
	}
}
