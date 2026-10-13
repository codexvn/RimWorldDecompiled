using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D4F RID: 11599
	public abstract class FocusStrengthOffset_Curve : FocusStrengthOffset
	{
		// Token: 0x060104C5 RID: 66757
		protected abstract float SourceValue(Thing parent);

		// Token: 0x17002973 RID: 10611
		// (get) Token: 0x060104C6 RID: 66758
		protected abstract string ExplanationKey { get; }

		// Token: 0x060104C7 RID: 66759 RVA: 0x004B5383 File Offset: 0x004B3583
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return Mathf.Round(this.curve.Evaluate(this.SourceValue(parent)) * 100f) / 100f;
		}

		// Token: 0x060104C8 RID: 66760 RVA: 0x004B53A8 File Offset: 0x004B35A8
		public override string GetExplanation(Thing parent)
		{
			return this.ExplanationKey.Translate() + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
		}

		// Token: 0x060104C9 RID: 66761 RVA: 0x004B53DC File Offset: 0x004B35DC
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return this.ExplanationKey.Translate() + ": " + (this.curve[0].y.ToStringWithSign("0%") + " " + "RangeTo".Translate() + " " + this.curve[this.curve.PointsCount - 1].y.ToStringWithSign("0%"));
		}

		// Token: 0x060104CA RID: 66762 RVA: 0x004B5478 File Offset: 0x004B3678
		public override float MaxOffset(Thing parent = null)
		{
			float num = 0f;
			for (int i = 0; i < this.curve.PointsCount; i++)
			{
				float y = this.curve[i].y;
				if (Mathf.Abs(y) > Mathf.Abs(num))
				{
					num = y;
				}
			}
			return num;
		}

		// Token: 0x0400AA2D RID: 43565
		public SimpleCurve curve;
	}
}
