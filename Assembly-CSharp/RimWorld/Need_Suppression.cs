using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002494 RID: 9364
	public class Need_Suppression : Need
	{
		// Token: 0x17001EEE RID: 7918
		// (get) Token: 0x0600CB0B RID: 51979 RVA: 0x003B7BA2 File Offset: 0x003B5DA2
		public bool CanBeSuppressedNow
		{
			get
			{
				return this.CurLevel < 0.7f;
			}
		}

		// Token: 0x17001EEF RID: 7919
		// (get) Token: 0x0600CB0C RID: 51980 RVA: 0x003B7A82 File Offset: 0x003B5C82
		public bool IsHigh
		{
			get
			{
				return this.CurLevel < 0.3f;
			}
		}

		// Token: 0x0600CB0D RID: 51981 RVA: 0x003B5049 File Offset: 0x003B3249
		public Need_Suppression(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600CB0E RID: 51982 RVA: 0x003B7BB1 File Offset: 0x003B5DB1
		public override void NeedInterval()
		{
			if (!this.IsFrozen)
			{
				this.CurLevel -= 0.0025f * this.pawn.GetStatValue(StatDefOf.SlaveSuppressionFallRate, true, -1);
			}
		}

		// Token: 0x0600CB0F RID: 51983 RVA: 0x003B7BE0 File Offset: 0x003B5DE0
		public void DrawSuppressionBar(Rect rect)
		{
			Widgets.FillableBar(rect, base.CurLevelPercentage, GuestUtility.SlaveSuppressionFillTex);
			base.DrawBarThreshold(rect, 0.3f);
			base.DrawBarThreshold(rect, 0.15f);
		}

		// Token: 0x04008A85 RID: 35461
		private const float CanSuppressMaxThreshold = 0.7f;

		// Token: 0x04008A86 RID: 35462
		private const float SuppressCriticalThreshold = 0.3f;
	}
}
