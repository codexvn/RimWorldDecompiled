using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020027A1 RID: 10145
	public interface ILordJobAssignmentsManager<RoleType> where RoleType : ILordJobRole
	{
		// Token: 0x0600DE2A RID: 56874
		IEnumerable<IGrouping<string, RoleType>> RoleGroups();

		// Token: 0x170021F3 RID: 8691
		// (get) Token: 0x0600DE2B RID: 56875
		bool SpectatorsAllowed { get; }

		// Token: 0x0600DE2C RID: 56876
		IEnumerable<Pawn> AssignedPawns(RoleType role);

		// Token: 0x0600DE2D RID: 56877
		RoleType ForcedRole(Pawn pawn);

		// Token: 0x0600DE2E RID: 56878
		RoleType RoleForPawn(Pawn pawn, bool includeForced = true);

		// Token: 0x0600DE2F RID: 56879
		Pawn FirstAssignedPawn(RoleType role);

		// Token: 0x0600DE30 RID: 56880
		bool Required(Pawn pawn);

		// Token: 0x0600DE31 RID: 56881
		bool PawnParticipating(Pawn pawn);

		// Token: 0x0600DE32 RID: 56882
		bool PawnSpectating(Pawn pawn);

		// Token: 0x0600DE33 RID: 56883
		bool CanParticipate(Pawn pawn, out TaggedString reason);

		// Token: 0x0600DE34 RID: 56884
		bool TryAssignSpectate(Pawn pawn, Pawn insertBefore = null);

		// Token: 0x0600DE35 RID: 56885
		bool TryAssign(Pawn pawn, RoleType role, out PsychicRitualRoleDef.Reason reason, PsychicRitualRoleDef.Context context = PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, Pawn insertBefore = null);

		// Token: 0x0600DE36 RID: 56886
		bool TryUnassignAnyRole(Pawn pawn);

		// Token: 0x0600DE37 RID: 56887
		void RemoveParticipant(Pawn pawn);

		// Token: 0x0600DE38 RID: 56888
		string PawnNotAssignableReason(Pawn p, RoleType role);

		// Token: 0x170021F4 RID: 8692
		// (get) Token: 0x0600DE39 RID: 56889
		List<Pawn> SpectatorsForReading { get; }
	}
}
