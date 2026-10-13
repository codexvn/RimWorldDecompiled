using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026D4 RID: 9940
	public class RitualBehaviorWorker_Conversion : RitualBehaviorWorker
	{
		// Token: 0x0600D9E2 RID: 55778 RVA: 0x00406F23 File Offset: 0x00405123
		public RitualBehaviorWorker_Conversion()
		{
		}

		// Token: 0x0600D9E3 RID: 55779 RVA: 0x00406F2B File Offset: 0x0040512B
		public RitualBehaviorWorker_Conversion(RitualBehaviorDef def) : base(def)
		{
		}

		// Token: 0x0600D9E4 RID: 55780 RVA: 0x0040790C File Offset: 0x00405B0C
		public override string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			Precept_Role precept_Role = ritual.ideo.RolesListForReading.FirstOrDefault((Precept_Role r) => r.def == PreceptDefOf.IdeoRole_Moralist);
			if (precept_Role == null)
			{
				return null;
			}
			if (precept_Role.ChosenPawnSingle() == null)
			{
				return "CantStartRitualRoleNotAssigned".Translate(precept_Role.LabelCap);
			}
			bool flag = false;
			using (List<Pawn>.Enumerator enumerator = target.Map.mapPawns.FreeColonistsAndPrisonersSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (RitualBehaviorWorker_Conversion.ValidateConvertee(enumerator.Current, precept_Role.ChosenPawnSingle(), false))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return "CantStartRitualNoConvertee".Translate(precept_Role.ChosenPawnSingle().Ideo.name);
			}
			return base.CanStartRitualNow(target, ritual, selectedPawn, forcedForRole);
		}

		// Token: 0x0600D9E5 RID: 55781 RVA: 0x00407A04 File Offset: 0x00405C04
		public static bool ValidateConvertee(Pawn convertee, Pawn leader, bool throwMessages)
		{
			return convertee != null && AbilityUtility.ValidateNoMentalState(convertee, throwMessages, null) && AbilityUtility.ValidateCanWalk(convertee, throwMessages, null) && AbilityUtility.ValidateNotSameIdeo(leader, convertee, throwMessages, null);
		}

		// Token: 0x0600D9E6 RID: 55782 RVA: 0x00407A34 File Offset: 0x00405C34
		public override void PostCleanup(LordJob_Ritual ritual)
		{
			Pawn warden = ritual.PawnWithRole("moralist");
			Pawn pawn = ritual.PawnWithRole("convertee");
			if (pawn.IsPrisonerOfColony)
			{
				WorkGiver_Warden_TakeToBed.TryTakePrisonerToBed(pawn, warden);
				pawn.guest.WaitInsteadOfEscapingFor(1250);
			}
		}
	}
}
