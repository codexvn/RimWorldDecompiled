using System;
using System.Text;
using RimWorld.Utility;
using Verse;

namespace RimWorld
{
	// Token: 0x0200371B RID: 14107
	public class StatPart_ReloadMarketValue : StatPart
	{
		// Token: 0x060138E9 RID: 80105 RVA: 0x005CCAFF File Offset: 0x005CACFF
		public override void TransformValue(StatRequest req, ref float val)
		{
			StatPart_ReloadMarketValue.TransformAndExplain(req, ref val, null);
		}

		// Token: 0x060138EA RID: 80106 RVA: 0x005CCB0C File Offset: 0x005CAD0C
		public override string ExplanationPart(StatRequest req)
		{
			float num = 1f;
			StringBuilder stringBuilder = new StringBuilder();
			StatPart_ReloadMarketValue.TransformAndExplain(req, ref num, stringBuilder);
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x060138EB RID: 80107 RVA: 0x005CCB3C File Offset: 0x005CAD3C
		private static void TransformAndExplain(StatRequest req, ref float val, StringBuilder explanation)
		{
			Thing thing = req.Thing;
			IReloadableComp reloadableComp = (thing != null) ? thing.TryGetComp<CompApparelReloadable>() : null;
			IReloadableComp reloadableComp2;
			if ((reloadableComp2 = reloadableComp) == null)
			{
				Thing thing2 = req.Thing;
				reloadableComp2 = ((thing2 != null) ? thing2.TryGetComp<CompEquippableAbilityReloadable>() : null);
			}
			IReloadableComp reloadableComp3 = reloadableComp2;
			if (reloadableComp3 == null)
			{
				return;
			}
			if (reloadableComp3.RemainingCharges == reloadableComp3.MaxCharges)
			{
				return;
			}
			if (reloadableComp3.AmmoDef != null)
			{
				int num = reloadableComp3.MaxAmmoNeeded(true);
				float num2 = -reloadableComp3.AmmoDef.BaseMarketValue * (float)num;
				val += num2;
				if (explanation != null)
				{
					explanation.AppendLine("StatsReport_ReloadMarketValue".Translate(reloadableComp3.AmmoDef.Named("AMMO"), num.Named("COUNT")) + ": " + num2.ToStringMoneyOffset(null));
				}
			}
			else
			{
				CompApparelVerbOwner_Charged compApparelVerbOwner_Charged = reloadableComp3 as CompApparelVerbOwner_Charged;
				if (compApparelVerbOwner_Charged != null && compApparelVerbOwner_Charged.Props.destroyOnEmpty)
				{
					float num3 = (float)reloadableComp3.RemainingCharges / (float)reloadableComp3.MaxCharges;
					if (explanation != null)
					{
						explanation.AppendLine("StatsReport_ReloadRemainingChargesMultipler".Translate(compApparelVerbOwner_Charged.Props.ChargeNounArgument, reloadableComp3.LabelRemaining) + ": x" + num3.ToStringPercent());
					}
					val *= num3;
				}
			}
			if (val < 0f)
			{
				val = 0f;
			}
		}
	}
}
