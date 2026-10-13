using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002470 RID: 9328
	public class Need_Beauty : Need_Seeker
	{
		// Token: 0x17001EA5 RID: 7845
		// (get) Token: 0x0600CA66 RID: 51814 RVA: 0x003B480C File Offset: 0x003B2A0C
		public override float CurInstantLevel
		{
			get
			{
				if (this.lastInstantBeautyCheckTick != Find.TickManager.TicksGame)
				{
					if (!this.pawn.Spawned)
					{
						this.lastInstantBeauty = 0.5f;
					}
					else if (PawnUtility.IsBiologicallyOrArtificiallyBlind(this.pawn))
					{
						this.lastInstantBeauty = 0.5f;
					}
					else
					{
						this.lastInstantBeauty = this.LevelFromBeauty(this.CurrentInstantBeauty());
					}
					this.lastInstantBeautyCheckTick = Find.TickManager.TicksGame;
				}
				return this.lastInstantBeauty;
			}
		}

		// Token: 0x17001EA6 RID: 7846
		// (get) Token: 0x0600CA67 RID: 51815 RVA: 0x003B4888 File Offset: 0x003B2A88
		public BeautyCategory CurCategory
		{
			get
			{
				if (this.CurLevel > 0.99f)
				{
					return BeautyCategory.Beautiful;
				}
				if (this.CurLevel > 0.85f)
				{
					return BeautyCategory.VeryPretty;
				}
				if (this.CurLevel > 0.65f)
				{
					return BeautyCategory.Pretty;
				}
				if (this.CurLevel > 0.35f)
				{
					return BeautyCategory.Neutral;
				}
				if (this.CurLevel > 0.15f)
				{
					return BeautyCategory.Ugly;
				}
				if (this.CurLevel > 0.01f)
				{
					return BeautyCategory.VeryUgly;
				}
				return BeautyCategory.Hideous;
			}
		}

		// Token: 0x0600CA68 RID: 51816 RVA: 0x003B48F0 File Offset: 0x003B2AF0
		public Need_Beauty(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.15f);
			this.threshPercents.Add(0.35f);
			this.threshPercents.Add(0.65f);
			this.threshPercents.Add(0.85f);
		}

		// Token: 0x0600CA69 RID: 51817 RVA: 0x003B4965 File Offset: 0x003B2B65
		private float LevelFromBeauty(float beauty)
		{
			return Mathf.Clamp01(this.def.baseLevel + beauty * 0.1f);
		}

		// Token: 0x0600CA6A RID: 51818 RVA: 0x003B497F File Offset: 0x003B2B7F
		public float CurrentInstantBeauty()
		{
			if (!this.pawn.SpawnedOrAnyParentSpawned)
			{
				return 0.5f;
			}
			return BeautyUtility.AverageBeautyPerceptible(this.pawn.PositionHeld, this.pawn.MapHeld);
		}

		// Token: 0x040089BA RID: 35258
		private const float BeautyImpactFactor = 0.1f;

		// Token: 0x040089BB RID: 35259
		private const float ThreshVeryUgly = 0.01f;

		// Token: 0x040089BC RID: 35260
		private const float ThreshUgly = 0.15f;

		// Token: 0x040089BD RID: 35261
		private const float ThreshNeutral = 0.35f;

		// Token: 0x040089BE RID: 35262
		private const float ThreshPretty = 0.65f;

		// Token: 0x040089BF RID: 35263
		private const float ThreshVeryPretty = 0.85f;

		// Token: 0x040089C0 RID: 35264
		private const float ThreshBeautiful = 0.99f;

		// Token: 0x040089C1 RID: 35265
		private int lastInstantBeautyCheckTick = -9999;

		// Token: 0x040089C2 RID: 35266
		private float lastInstantBeauty = -1f;
	}
}
