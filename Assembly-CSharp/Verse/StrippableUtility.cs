using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020007C7 RID: 1991
	public static class StrippableUtility
	{
		// Token: 0x0600325D RID: 12893 RVA: 0x00107730 File Offset: 0x00105930
		public static bool CanBeStrippedByColony(Thing th)
		{
			IStrippable strippable = th as IStrippable;
			if (strippable == null)
			{
				return false;
			}
			if (!strippable.AnythingToStrip())
			{
				return false;
			}
			Pawn pawn = th as Pawn;
			if (pawn == null)
			{
				return true;
			}
			if (!pawn.kindDef.canStrip)
			{
				return false;
			}
			if (pawn.CarriedBy != null)
			{
				return false;
			}
			if (pawn.Downed)
			{
				return true;
			}
			if (pawn.IsPrisonerOfColony && pawn.guest.PrisonerIsSecure)
			{
				return true;
			}
			pawn.IsQuestLodger();
			return false;
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x001077A4 File Offset: 0x001059A4
		public static void CheckSendStrippingImpactsGoodwillMessage(Thing th)
		{
			Pawn pawn = th as Pawn;
			if (pawn != null && !pawn.Dead && pawn.Faction != null && pawn.Faction != Faction.OfPlayer && !pawn.Faction.HostileTo(Faction.OfPlayer) && !pawn.Faction.Hidden)
			{
				Messages.Message("MessageStrippingWillAngerFaction".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
