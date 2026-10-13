using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003D0 RID: 976
	public static class BloodRainUtility
	{
		// Token: 0x06001919 RID: 6425 RVA: 0x0007E864 File Offset: 0x0007CA64
		public static bool ExposedToBloodRain(Pawn pawn)
		{
			if (pawn.MapHeld == null)
			{
				return false;
			}
			if (pawn.SpawnedParentOrMe is PawnFlyer)
			{
				return false;
			}
			GameCondition_BloodRain activeCondition = pawn.MapHeld.gameConditionManager.GetActiveCondition<GameCondition_BloodRain>();
			return activeCondition != null && !activeCondition.HiddenByOtherCondition(pawn.MapHeld) && (activeCondition.TicksPassed > 2000 && pawn.Map != null) && !pawn.Position.Roofed(pawn.Map);
		}

		// Token: 0x0600191A RID: 6426 RVA: 0x0007E8DC File Offset: 0x0007CADC
		public static void BloodRainTick(Pawn pawn)
		{
			if (!ModsConfig.AnomalyActive || !BloodRainUtility.ExposedToBloodRain(pawn))
			{
				return;
			}
			if ((pawn.RaceProps.Humanlike || pawn.IsAnimal) && !pawn.RaceProps.Dryad && !pawn.health.hediffSet.HasHediff(HediffDefOf.BloodRage, false))
			{
				pawn.health.AddHediff(HediffDefOf.BloodRage, null, null, null);
			}
		}

		// Token: 0x0600191B RID: 6427 RVA: 0x0007E950 File Offset: 0x0007CB50
		public static bool TryTriggerBerserkShort(Pawn pawn)
		{
			if (ModsConfig.AnomalyActive && pawn.health.hediffSet.HasHediff(HediffDefOf.BloodRage, false))
			{
				bool flag = MentalBreakDefOf.BerserkShort.Worker.TryStart(pawn, "MentalBreakReason_BloodRage".Translate(), false);
				if (flag && MessagesRepeatAvoider.MessageShowAllowed("MessageBerserkBloodrain-" + pawn.LabelShort, 240f))
				{
					Messages.Message("Berserk_BloodRain".Translate(pawn), pawn, MessageTypeDefOf.NeutralEvent, false);
				}
				return flag;
			}
			return false;
		}
	}
}
