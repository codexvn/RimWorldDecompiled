using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200373A RID: 14138
	public class StatWorker_SuppressionFallRate : StatWorker
	{
		// Token: 0x060139B5 RID: 80309 RVA: 0x005D2700 File Offset: 0x005D0900
		public override bool ShouldShowFor(StatRequest req)
		{
			if (!base.ShouldShowFor(req))
			{
				return false;
			}
			Pawn pawn = req.Thing as Pawn;
			return pawn != null && pawn.IsSlave;
		}

		// Token: 0x060139B6 RID: 80310 RVA: 0x005D2730 File Offset: 0x005D0930
		private static float CurrentFallRateBasedOnSuppression(float suppression)
		{
			if (suppression > 0.3f)
			{
				return 0.2f;
			}
			if (suppression > 0.15f)
			{
				return 0.1f;
			}
			return 0.05f;
		}

		// Token: 0x060139B7 RID: 80311 RVA: 0x005D2754 File Offset: 0x005D0954
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			Need_Suppression need_Suppression;
			((Pawn)req.Thing).needs.TryGetNeed<Need_Suppression>(out need_Suppression);
			return StatWorker_SuppressionFallRate.CurrentFallRateBasedOnSuppression(need_Suppression.CurLevelPercentage);
		}

		// Token: 0x060139B8 RID: 80312 RVA: 0x005D2788 File Offset: 0x005D0988
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			Need_Suppression need_Suppression;
			((Pawn)req.Thing).needs.TryGetNeed<Need_Suppression>(out need_Suppression);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetExplanationUnfinalized(req, numberSense));
			stringBuilder.Append(string.Format("{0} ({1}): {2}", "CurrentSuppression".Translate(), need_Suppression.CurLevelPercentage.ToStringPercent(), StatWorker_SuppressionFallRate.CurrentFallRateBasedOnSuppression(need_Suppression.CurLevelPercentage).ToStringPercent()));
			return stringBuilder.ToString();
		}

		// Token: 0x060139B9 RID: 80313 RVA: 0x005D2804 File Offset: 0x005D0A04
		public string GetExplanationForTooltip(StatRequest req)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("SuppressionFallRate".Translate() + ": " + base.GetValue(req.Thing, true, -1).ToStringPercent());
			Need_Suppression need_Suppression;
			((Pawn)req.Thing).needs.TryGetNeed<Need_Suppression>(out need_Suppression);
			stringBuilder.AppendLine(string.Format("   {0} ({1}): {2}", "CurrentSuppression".Translate(), need_Suppression.CurLevelPercentage.ToStringPercent(), StatWorker_SuppressionFallRate.CurrentFallRateBasedOnSuppression(need_Suppression.CurLevelPercentage).ToStringPercent()));
			if (this.stat.parts != null)
			{
				for (int i = 0; i < this.stat.parts.Count; i++)
				{
					string text = this.stat.parts[i].ExplanationPart(req);
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine("   " + text);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400D93B RID: 55611
		public const float FastFallRate = 0.2f;

		// Token: 0x0400D93C RID: 55612
		public const float MediumFallRate = 0.1f;

		// Token: 0x0400D93D RID: 55613
		public const float SlowFallRate = 0.05f;

		// Token: 0x0400D93E RID: 55614
		public const float FastFallRateThreshold = 0.3f;

		// Token: 0x0400D93F RID: 55615
		public const float MediumFallRateThreshold = 0.15f;
	}
}
