using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002481 RID: 9345
	public class Need_Joy : Need
	{
		// Token: 0x17001ECC RID: 7884
		// (get) Token: 0x0600CAB8 RID: 51896 RVA: 0x003B5FB8 File Offset: 0x003B41B8
		public JoyCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return JoyCategory.Empty;
				}
				if (this.CurLevel < 0.15f)
				{
					return JoyCategory.VeryLow;
				}
				if (this.CurLevel < 0.3f)
				{
					return JoyCategory.Low;
				}
				if (this.CurLevel < 0.7f)
				{
					return JoyCategory.Satisfied;
				}
				if (this.CurLevel < 0.85f)
				{
					return JoyCategory.High;
				}
				return JoyCategory.Extreme;
			}
		}

		// Token: 0x17001ECD RID: 7885
		// (get) Token: 0x0600CAB9 RID: 51897 RVA: 0x003B6014 File Offset: 0x003B4214
		private float FallPerInterval
		{
			get
			{
				switch (this.CurCategory)
				{
				case JoyCategory.Empty:
					return 0.0015f;
				case JoyCategory.VeryLow:
					return 0.0006f;
				case JoyCategory.Low:
					return 0.00105f;
				case JoyCategory.Satisfied:
					return 0.0015f;
				case JoyCategory.High:
					return 0.0015f;
				case JoyCategory.Extreme:
					return 0.0015f;
				default:
					throw new InvalidOperationException();
				}
			}
		}

		// Token: 0x17001ECE RID: 7886
		// (get) Token: 0x0600CABA RID: 51898 RVA: 0x003B6071 File Offset: 0x003B4271
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				if (!this.GainingJoy)
				{
					return -1;
				}
				return 1;
			}
		}

		// Token: 0x17001ECF RID: 7887
		// (get) Token: 0x0600CABB RID: 51899 RVA: 0x003B6088 File Offset: 0x003B4288
		private bool GainingJoy
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastGainTick + 15;
			}
		}

		// Token: 0x0600CABC RID: 51900 RVA: 0x003B60A0 File Offset: 0x003B42A0
		public Need_Joy(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.15f);
			this.threshPercents.Add(0.3f);
			this.threshPercents.Add(0.7f);
			this.threshPercents.Add(0.85f);
		}

		// Token: 0x0600CABD RID: 51901 RVA: 0x003B6115 File Offset: 0x003B4315
		public override void ExposeData()
		{
			base.ExposeData();
			this.tolerances.ExposeData();
		}

		// Token: 0x0600CABE RID: 51902 RVA: 0x003B6128 File Offset: 0x003B4328
		public override void SetInitialLevel()
		{
			this.CurLevel = Rand.Range(0.5f, 0.6f);
		}

		// Token: 0x0600CABF RID: 51903 RVA: 0x003B6140 File Offset: 0x003B4340
		public void GainJoy(float amount, JoyKindDef joyKind)
		{
			if (amount <= 0f)
			{
				return;
			}
			amount *= this.tolerances.JoyFactorFromTolerance(joyKind);
			amount = Mathf.Min(amount, 1f - this.CurLevel);
			this.curLevelInt += amount;
			if (joyKind != null)
			{
				this.tolerances.Notify_JoyGained(amount, joyKind);
			}
			this.lastGainTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600CAC0 RID: 51904 RVA: 0x003B61A8 File Offset: 0x003B43A8
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.tolerances.NeedInterval(this.pawn);
				if (!this.GainingJoy)
				{
					this.CurLevel -= this.FallPerInterval * (this.pawn.IsFormingCaravan() ? 0.5f : 1f) * this.pawn.GetStatValue(StatDefOf.JoyFallRateFactor, true, -1);
				}
			}
		}

		// Token: 0x0600CAC1 RID: 51905 RVA: 0x003B6218 File Offset: 0x003B4418
		public override string GetTipString()
		{
			TaggedString t = base.GetTipString();
			string text = this.tolerances.TolerancesString();
			if (!string.IsNullOrEmpty(text))
			{
				t += "\n\n" + text;
			}
			if (this.pawn.MapHeld != null)
			{
				ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(this.pawn);
				t += "\n\n" + "CurrentExpectationsAndRecreation".Translate(expectationDef.label, expectationDef.joyToleranceDropPerDay.ToStringPercent(), expectationDef.joyKindsNeeded);
				t += "\n\n" + JoyUtility.JoyKindsOnMapString(this.pawn.MapHeld);
			}
			else
			{
				Caravan caravan = this.pawn.GetCaravan();
				if (caravan != null)
				{
					float num = caravan.needs.GetCurrentJoyGainPerTick(this.pawn) * 2500f;
					if (num > 0f)
					{
						t += "\n\n" + "GainingJoyBecauseCaravanNotMoving".Translate() + ": +" + num.ToStringPercent() + "/" + "LetterHour".Translate();
					}
				}
			}
			return t.Resolve();
		}

		// Token: 0x04008A28 RID: 35368
		public JoyToleranceSet tolerances = new JoyToleranceSet();

		// Token: 0x04008A29 RID: 35369
		private int lastGainTick = -999;
	}
}
