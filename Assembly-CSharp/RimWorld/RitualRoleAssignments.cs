using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020027A2 RID: 10146
	public class RitualRoleAssignments : ILordJobAssignmentsManager<RitualRole>, ILordJobCandidatePool, IExposable
	{
		// Token: 0x170021F5 RID: 8693
		// (get) Token: 0x0600DE3A RID: 56890 RVA: 0x00415B68 File Offset: 0x00413D68
		public List<Pawn> SpectatorsForReading
		{
			get
			{
				return this.spectators;
			}
		}

		// Token: 0x170021F6 RID: 8694
		// (get) Token: 0x0600DE3B RID: 56891 RVA: 0x00415B70 File Offset: 0x00413D70
		public Precept_Ritual Ritual
		{
			get
			{
				return this.ritual;
			}
		}

		// Token: 0x170021F7 RID: 8695
		// (get) Token: 0x0600DE3C RID: 56892 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool SpectatorsAllowed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021F8 RID: 8696
		// (get) Token: 0x0600DE3D RID: 56893 RVA: 0x00415B78 File Offset: 0x00413D78
		public List<Pawn> Participants
		{
			get
			{
				this.tmpParticipants.Clear();
				foreach (Pawn pawn in this.allPawns)
				{
					if (this.PawnParticipating(pawn))
					{
						this.tmpParticipants.Add(pawn);
					}
				}
				return this.tmpParticipants;
			}
		}

		// Token: 0x170021F9 RID: 8697
		// (get) Token: 0x0600DE3E RID: 56894 RVA: 0x00415BEC File Offset: 0x00413DEC
		public List<Pawn> AllCandidatePawns
		{
			get
			{
				return this.allPawns;
			}
		}

		// Token: 0x170021FA RID: 8698
		// (get) Token: 0x0600DE3F RID: 56895 RVA: 0x00415BF4 File Offset: 0x00413DF4
		public List<Pawn> NonAssignablePawns
		{
			get
			{
				return this.nonAssignablePawns;
			}
		}

		// Token: 0x170021FB RID: 8699
		// (get) Token: 0x0600DE40 RID: 56896 RVA: 0x00415BFC File Offset: 0x00413DFC
		public List<RitualRole> AllRolesForReading
		{
			get
			{
				return this.allRoles;
			}
		}

		// Token: 0x170021FC RID: 8700
		// (get) Token: 0x0600DE41 RID: 56897 RVA: 0x00415C04 File Offset: 0x00413E04
		public List<Pawn> ExtraRequiredPawnsForReading
		{
			get
			{
				return this.requiredPawns;
			}
		}

		// Token: 0x170021FD RID: 8701
		// (get) Token: 0x0600DE42 RID: 56898 RVA: 0x00415C0C File Offset: 0x00413E0C
		public Dictionary<string, Pawn> ForcedRolesForReading
		{
			get
			{
				return this.forcedRoles;
			}
		}

		// Token: 0x170021FE RID: 8702
		// (get) Token: 0x0600DE43 RID: 56899 RVA: 0x00415C14 File Offset: 0x00413E14
		public Precept_Role RoleChangeSelection
		{
			get
			{
				return this.roleChangeSelection;
			}
		}

		// Token: 0x0600DE44 RID: 56900 RVA: 0x00415C1C File Offset: 0x00413E1C
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.allPawns, "allPawns", true, LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.nonAssignablePawns, "nonAssignablePawns", true, LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.spectators, "spectators", true, LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.requiredPawns, "requiredPawns", true, LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<string, Pawn>(ref this.forcedRoles, "forcedRoles", LookMode.Value, LookMode.Reference, ref this.tmpForcedRoleIds, ref this.tmpForcedRolePawns, true, false, true);
			Scribe_Collections.Look<string, List<Pawn>>(ref this.assignedRoles, "assignedRoles", LookMode.Value, LookMode.Reference, ref this.tmpAssignedRoleIds, ref this.tmpAssignedRolePawns, true, false, true);
			Scribe_References.Look<Precept_Ritual>(ref this.ritual, "ritual", false);
			Scribe_TargetInfo.Look(ref this.ritualTarget, true, "ritualTarget");
			Scribe_References.Look<Pawn>(ref this.selectedPawn, "selectedPawn", true);
			Scribe_References.Look<Precept_Role>(ref this.roleChangeSelection, "roleChangeSelection", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.allRoles = ((this.ritual != null) ? new List<RitualRole>(this.ritual.behavior.def.roles) : new List<RitualRole>());
			}
		}

		// Token: 0x0600DE45 RID: 56901 RVA: 0x00415D42 File Offset: 0x00413F42
		protected RitualRoleAssignments()
		{
		}

		// Token: 0x0600DE46 RID: 56902 RVA: 0x00415D6B File Offset: 0x00413F6B
		public RitualRoleAssignments(Precept_Ritual ritual, TargetInfo ritualTarget)
		{
			this.ritual = ritual;
			this.ritualTarget = ritualTarget;
		}

		// Token: 0x0600DE47 RID: 56903 RVA: 0x00415DA4 File Offset: 0x00413FA4
		public void Setup(List<Pawn> allPawns, List<Pawn> nonAssignablePawns, Dictionary<string, Pawn> forcedRoles = null, List<Pawn> requiredPawns = null, Pawn selectedPawn = null)
		{
			this.allPawns = allPawns;
			this.nonAssignablePawns = nonAssignablePawns;
			this.forcedRoles = forcedRoles;
			this.allRoles = ((this.ritual != null) ? new List<RitualRole>(this.ritual.behavior.def.roles) : new List<RitualRole>());
			this.selectedPawn = selectedPawn;
			this.requiredPawns = new List<Pawn>();
			if (forcedRoles != null)
			{
				this.requiredPawns.AddRange(forcedRoles.Values);
			}
			if (requiredPawns != null)
			{
				this.requiredPawns.AddRange(requiredPawns);
			}
			allPawns.SortBy((Pawn p) => p.Faction == null || !p.Faction.IsPlayer, (Pawn p) => !Faction.OfPlayer.ideos.Has(p.Ideo), (Pawn p) => !p.IsFreeNonSlaveColonist);
		}

		// Token: 0x0600DE48 RID: 56904 RVA: 0x00415E90 File Offset: 0x00414090
		public bool Forced(Pawn pawn)
		{
			return this.forcedRoles != null && this.forcedRoles.ContainsValue(pawn);
		}

		// Token: 0x0600DE49 RID: 56905 RVA: 0x00415EA8 File Offset: 0x004140A8
		public RitualRole ForcedRole(Pawn pawn)
		{
			if (this.forcedRoles == null)
			{
				return null;
			}
			string text = null;
			foreach (KeyValuePair<string, Pawn> keyValuePair in this.forcedRoles)
			{
				if (keyValuePair.Value == pawn)
				{
					text = keyValuePair.Key;
				}
			}
			if (text == null)
			{
				return null;
			}
			foreach (RitualRole ritualRole in this.allRoles)
			{
				if (ritualRole.id == text)
				{
					return ritualRole;
				}
			}
			return null;
		}

		// Token: 0x0600DE4A RID: 56906 RVA: 0x00415F6C File Offset: 0x0041416C
		public void RemoveParticipant(Pawn pawn)
		{
			this.TryUnassignAnyRole(pawn);
			this.spectators.Remove(pawn);
			this.allPawns.Remove(pawn);
			this.allPawns.Add(pawn);
		}

		// Token: 0x0600DE4B RID: 56907 RVA: 0x00415F9C File Offset: 0x0041419C
		public bool TryUnassignAnyRole(Pawn pawn)
		{
			foreach (KeyValuePair<string, List<Pawn>> keyValuePair in this.assignedRoles)
			{
				if (keyValuePair.Value.Remove(pawn))
				{
					if (this.CanEverSpectate(pawn))
					{
						this.spectators.Add(pawn);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600DE4C RID: 56908 RVA: 0x00416014 File Offset: 0x00414214
		public bool TryAssign(Pawn pawn, RitualRole role, out PsychicRitualRoleDef.Reason reason, PsychicRitualRoleDef.Context context = PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, Pawn insertBefore = null)
		{
			reason = PsychicRitualRoleDef.Reason.None;
			if (this.forcedRoles != null && this.forcedRoles.ContainsValue(pawn))
			{
				return false;
			}
			bool flag;
			if (this.PawnNotAssignableReason(pawn, role, out flag) != null)
			{
				return false;
			}
			if (role == null)
			{
				return this.TryAssignSpectate(pawn, null);
			}
			string text;
			if (role.AppliesToPawn(pawn, out text, this.ritualTarget, null, this, null, true) && (role.maxCount <= 0 || this.AssignedPawns(role).Count<Pawn>() < role.maxCount))
			{
				this.TryUnassignAnyRole(pawn);
				this.spectators.Remove(pawn);
				List<Pawn> list;
				if (!this.assignedRoles.TryGetValue(role.id, out list))
				{
					list = new List<Pawn>();
					this.assignedRoles.Add(role.id, list);
				}
				if (insertBefore != null && list.Contains(insertBefore))
				{
					list.Insert(list.IndexOf(insertBefore), pawn);
				}
				else
				{
					list.Add(pawn);
				}
				if (role is RitualRoleIdeoRoleChanger)
				{
					this.UpdateRoleChangeTargetRole(pawn);
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600DE4D RID: 56909 RVA: 0x00416110 File Offset: 0x00414310
		public bool TryAssignSpectate(Pawn pawn, Pawn insertBefore = null)
		{
			bool flag;
			if (this.spectators.Contains(pawn) || !this.CanEverSpectate(pawn) || this.PawnNotAssignableReason(pawn, null, out flag) != null)
			{
				return false;
			}
			this.TryUnassignAnyRole(pawn);
			if (!this.spectators.Contains(pawn))
			{
				if (insertBefore != null && this.spectators.Contains(insertBefore))
				{
					this.spectators.Insert(this.spectators.IndexOf(insertBefore), pawn);
				}
				else
				{
					this.spectators.Add(pawn);
				}
			}
			return this.RoleForPawn(pawn, true) == null;
		}

		// Token: 0x0600DE4E RID: 56910 RVA: 0x0041619C File Offset: 0x0041439C
		public RitualRole GetRole(string roleId)
		{
			if (!this.AllRolesForReading.NullOrEmpty<RitualRole>())
			{
				foreach (RitualRole ritualRole in this.AllRolesForReading)
				{
					if (ritualRole.id == roleId)
					{
						return ritualRole;
					}
				}
			}
			return null;
		}

		// Token: 0x0600DE4F RID: 56911 RVA: 0x0041620C File Offset: 0x0041440C
		public bool CanParticipate(Pawn pawn, out TaggedString reason)
		{
			if (this.forcedRoles != null && this.forcedRoles.ContainsValue(pawn))
			{
				reason = TaggedString.Empty;
				return true;
			}
			if (this.Required(pawn))
			{
				reason = TaggedString.Empty;
				return true;
			}
			if (pawn == this.selectedPawn)
			{
				reason = TaggedString.Empty;
				return true;
			}
			foreach (RitualRole role in this.AllRolesForReading)
			{
				bool flag;
				if (this.PawnNotAssignableReason(pawn, role, out flag) == null)
				{
					reason = TaggedString.Empty;
					return true;
				}
			}
			if (this.CanEverSpectate(pawn))
			{
				reason = TaggedString.Empty;
				return true;
			}
			reason = this.PawnNotAssignableReason(pawn, null);
			return false;
		}

		// Token: 0x0600DE50 RID: 56912 RVA: 0x004162F0 File Offset: 0x004144F0
		public Pawn FirstAssignedPawn(RitualRole role)
		{
			return this.FirstAssignedPawn(role.id);
		}

		// Token: 0x0600DE51 RID: 56913 RVA: 0x00416300 File Offset: 0x00414500
		public Pawn FirstAssignedPawn(string roleId)
		{
			Pawn result;
			if (this.forcedRoles != null && this.forcedRoles.TryGetValue(roleId, out result))
			{
				return result;
			}
			List<Pawn> list;
			if (this.assignedRoles.TryGetValue(roleId, out list) && list.Count > 0)
			{
				return list[0];
			}
			return null;
		}

		// Token: 0x0600DE52 RID: 56914 RVA: 0x0041634C File Offset: 0x0041454C
		public bool CanEverSpectate(Pawn pawn)
		{
			if (this.ritual != null && this.ritual.ritualOnlyForIdeoMembers && pawn.Ideo != this.ritual.ideo && !this.ritual.def.allowSpectatorsFromOtherIdeos)
			{
				return false;
			}
			if (this.ritual != null && this.ritual.behavior.def.spectatorFilter != null && !this.ritual.behavior.def.spectatorFilter.Allowed(pawn))
			{
				return false;
			}
			string text;
			if (this.ritual != null && !this.ritual.behavior.PawnCanFillRole(pawn, null, out text, this.ritualTarget))
			{
				return false;
			}
			if (pawn.RaceProps.Humanlike && !pawn.IsPrisoner)
			{
				Precept_Ritual precept_Ritual = this.ritual;
				Precept_Ritual precept_Ritual2 = this.ritual;
				return GatheringsUtility.ShouldPawnKeepAttendingRitual(pawn, precept_Ritual, precept_Ritual2 != null && precept_Ritual2.behavior.def.spectatorsIgnoreBleeding);
			}
			return false;
		}

		// Token: 0x0600DE53 RID: 56915 RVA: 0x00416438 File Offset: 0x00414638
		public IEnumerable<Pawn> SpectatorCandidates()
		{
			foreach (Pawn pawn in this.allPawns)
			{
				if (this.CanEverSpectate(pawn) && this.RoleForPawn(pawn, true) == null)
				{
					Precept_Ritual precept_Ritual = this.ritual;
					bool flag;
					if (precept_Ritual == null)
					{
						flag = true;
					}
					else
					{
						RitualBehaviorWorker behavior = precept_Ritual.behavior;
						bool? flag2 = (behavior != null) ? new bool?(behavior.ShouldInitAsSpectator(pawn, this)) : null;
						bool flag3 = false;
						flag = !(flag2.GetValueOrDefault() == flag3 & flag2 != null);
					}
					if (flag)
					{
						yield return pawn;
					}
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600DE54 RID: 56916 RVA: 0x00416448 File Offset: 0x00414648
		public IEnumerable<Pawn> CandidatesForRole(string roleId, TargetInfo ritualTarget, bool includeAssigned = false, bool includeAssignedForSameRole = false, bool includeForced = true)
		{
			return this.CandidatesForRole(this.GetRole(roleId), ritualTarget, includeAssigned, includeAssignedForSameRole, includeForced);
		}

		// Token: 0x0600DE55 RID: 56917 RVA: 0x0041645D File Offset: 0x0041465D
		public IEnumerable<Pawn> CandidatesForRole(RitualRole role, TargetInfo ritualTarget, bool includeAssigned = false, bool includeAssignedForSameRole = false, bool includeForced = true)
		{
			RitualRoleAssignments.<>c__DisplayClass53_0 CS$<>8__locals1;
			CS$<>8__locals1.includeAssigned = includeAssigned;
			CS$<>8__locals1.includeAssignedForSameRole = includeAssignedForSameRole;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.role = role;
			Pawn pawn;
			if (this.forcedRoles != null && this.forcedRoles.TryGetValue(CS$<>8__locals1.role.id, out pawn))
			{
				yield return pawn;
				yield break;
			}
			foreach (Pawn pawn2 in this.allPawns)
			{
				string text;
				if (CS$<>8__locals1.role.AppliesToPawn(pawn2, out text, ritualTarget, null, this, null, true) && this.<CandidatesForRole>g__ShouldIncludePawn|53_0(pawn2, ref CS$<>8__locals1))
				{
					yield return pawn2;
				}
			}
			List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600DE56 RID: 56918 RVA: 0x0041648A File Offset: 0x0041468A
		public IEnumerable<Pawn> AssignedPawns(RitualRole role)
		{
			if (this.forcedRoles != null)
			{
				foreach (KeyValuePair<string, Pawn> keyValuePair in this.forcedRoles)
				{
					if (keyValuePair.Key == role.id)
					{
						yield return keyValuePair.Value;
					}
				}
				Dictionary<string, Pawn>.Enumerator enumerator = default(Dictionary<string, Pawn>.Enumerator);
			}
			List<Pawn> list;
			if (this.assignedRoles.TryGetValue(role.id, out list))
			{
				foreach (Pawn pawn in list)
				{
					yield return pawn;
				}
				List<Pawn>.Enumerator enumerator2 = default(List<Pawn>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0600DE57 RID: 56919 RVA: 0x004164A1 File Offset: 0x004146A1
		public bool AnyPawnAssigned(string roleId)
		{
			return (this.forcedRoles != null && this.forcedRoles.ContainsKey(roleId)) || (this.assignedRoles.ContainsKey(roleId) && this.assignedRoles[roleId].Any<Pawn>());
		}

		// Token: 0x0600DE58 RID: 56920 RVA: 0x004164DC File Offset: 0x004146DC
		public bool AnyPawnAssigned(RitualRole role)
		{
			return this.AnyPawnAssigned(role.id);
		}

		// Token: 0x0600DE59 RID: 56921 RVA: 0x004164EA File Offset: 0x004146EA
		public IEnumerable<Pawn> AssignedPawns(string roleId)
		{
			return this.AssignedPawns(this.GetRole(roleId));
		}

		// Token: 0x0600DE5A RID: 56922 RVA: 0x004164FC File Offset: 0x004146FC
		public RitualRole RoleForPawn(Pawn pawn, bool includeForced = true)
		{
			if (this.spectators.Contains(pawn))
			{
				return null;
			}
			if (includeForced && this.forcedRoles != null)
			{
				foreach (KeyValuePair<string, Pawn> keyValuePair in this.forcedRoles)
				{
					if (keyValuePair.Value == pawn)
					{
						return this.GetRole(keyValuePair.Key);
					}
				}
			}
			foreach (KeyValuePair<string, List<Pawn>> keyValuePair2 in this.assignedRoles)
			{
				if (!keyValuePair2.Value.NullOrEmpty<Pawn>() && keyValuePair2.Value.Contains(pawn))
				{
					return this.GetRole(keyValuePair2.Key);
				}
			}
			return null;
		}

		// Token: 0x0600DE5B RID: 56923 RVA: 0x004165EC File Offset: 0x004147EC
		public bool PawnParticipating(Pawn pawn)
		{
			return this.RoleForPawn(pawn, true) != null || this.PawnSpectating(pawn);
		}

		// Token: 0x0600DE5C RID: 56924 RVA: 0x00416601 File Offset: 0x00414801
		public bool PawnSpectating(Pawn pawn)
		{
			return this.spectators.Contains(pawn);
		}

		// Token: 0x0600DE5D RID: 56925 RVA: 0x00416610 File Offset: 0x00414810
		public void FillPawns(Dialog_BeginRitual.PawnFilter filter, TargetInfo ritualTarget)
		{
			if (!this.requiredPawns.NullOrEmpty<Pawn>())
			{
				foreach (Pawn pawn5 in this.requiredPawns)
				{
					if (this.forcedRoles == null || !this.forcedRoles.ContainsValue(pawn5))
					{
						this.TryAssignSpectate(pawn5, null);
					}
				}
			}
			if (this.selectedPawn != null && this.RoleForPawn(this.selectedPawn, true) == null)
			{
				foreach (RitualRole ritualRole in this.AllRolesForReading)
				{
					string text;
					if (ritualRole.defaultForSelectedColonist && ritualRole.AppliesToPawn(this.selectedPawn, out text, ritualTarget, null, this, null, false))
					{
						PsychicRitualRoleDef.Reason reason;
						this.TryAssign(this.selectedPawn, ritualRole, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, null);
						break;
					}
				}
			}
			using (List<RitualRole>.Enumerator enumerator2 = this.AllRolesForReading.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RitualRole role = enumerator2.Current;
					RitualRoleAssignments.tmpOrderedPawns.Clear();
					RitualRoleAssignments.tmpOrderedPawns.AddRange(from pawn in this.allPawns
					where filter == null || filter(pawn, !(role is RitualRoleForced), role.allowOtherIdeos)
					select pawn);
					role.OrderByDesirability(RitualRoleAssignments.tmpOrderedPawns);
					foreach (Pawn pawn2 in RitualRoleAssignments.tmpOrderedPawns)
					{
						if (this.RoleForPawn(pawn2, true) == null)
						{
							if (role.maxCount > 0 && this.AssignedPawns(role).Count<Pawn>() >= role.maxCount)
							{
								break;
							}
							string text;
							if (role.AppliesToPawn(pawn2, out text, ritualTarget, null, this, null, true))
							{
								PsychicRitualRoleDef.Reason reason;
								this.TryAssign(pawn2, role, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, null);
							}
						}
					}
				}
			}
			foreach (Pawn pawn3 in this.SpectatorCandidates())
			{
				this.TryAssignSpectate(pawn3, null);
			}
			List<Pawn> pawnsToRemove = new List<Pawn>();
			foreach (Pawn pawn4 in this.allPawns)
			{
				RitualRole ritualRole2 = this.RoleForPawn(pawn4, true);
				bool flag;
				if (ritualRole2 != null && ritualRole2.required && ritualRole2.substitutable && this.PawnNotAssignableReason(pawn4, ritualRole2, out flag) != null)
				{
					this.RemoveParticipant(pawn4);
					pawnsToRemove.Add(pawn4);
				}
			}
			this.allPawns.RemoveAll((Pawn p) => pawnsToRemove.Contains(p));
		}

		// Token: 0x0600DE5E RID: 56926 RVA: 0x00416984 File Offset: 0x00414B84
		public bool Required(Pawn pawn)
		{
			return this.requiredPawns.NotNullAndContains(pawn);
		}

		// Token: 0x0600DE5F RID: 56927 RVA: 0x00416994 File Offset: 0x00414B94
		public bool RoleSubstituted(string roleId)
		{
			if (this.ritual.behavior.def.roles.NullOrEmpty<RitualRole>())
			{
				return false;
			}
			RitualRole role = this.ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == roleId);
			if (role == null)
			{
				return false;
			}
			if (!role.substitutable || role.precept == null)
			{
				return false;
			}
			Precept precept = this.ritual.ideo.PreceptsListForReading.FirstOrDefault((Precept p) => p.def == role.precept);
			if (precept == null)
			{
				return false;
			}
			bool result = false;
			foreach (Pawn pawn in this.AssignedPawns(roleId))
			{
				Ideo ideo = pawn.Ideo;
				if (((ideo != null) ? ideo.GetRole(pawn) : null) != precept)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600DE60 RID: 56928 RVA: 0x00416AA4 File Offset: 0x00414CA4
		public string PawnNotAssignableReason(Pawn p, RitualRole role)
		{
			bool flag;
			return this.PawnNotAssignableReason(p, role, out flag);
		}

		// Token: 0x0600DE61 RID: 56929 RVA: 0x00416ABB File Offset: 0x00414CBB
		public string PawnNotAssignableReason(Pawn p, RitualRole role, out bool stillAddToPawnList)
		{
			return RitualRoleAssignments.PawnNotAssignableReason(p, role, this.ritual, this, this.ritualTarget, out stillAddToPawnList);
		}

		// Token: 0x0600DE62 RID: 56930 RVA: 0x00416AD4 File Offset: 0x00414CD4
		public static string PawnNotAssignableReason(Pawn p, RitualRole role, Precept_Ritual ritual, RitualRoleAssignments assignments, TargetInfo ritualTarget, out bool stillAddToPawnList)
		{
			stillAddToPawnList = false;
			if (p.Downed && (role == null || !role.allowDowned))
			{
				return "MessageRitualPawnDowned".Translate(p);
			}
			bool flag = (role != null) ? role.ignoreBleeding : (ritual != null && ritual.behavior.def.spectatorsIgnoreBleeding);
			if (p.health.hediffSet.BleedRateTotal > 0f && !flag)
			{
				return "MessageRitualPawnInjured".Translate(p);
			}
			if (p.InMentalState && (role == null || !role.allowNonAggroMentalState || p.InAggroMentalState))
			{
				return "MessageRitualPawnMentalState".Translate(p);
			}
			if (p.IsPrisoner && role == null)
			{
				string key = "MessageRitualRoleMustNotBePrisonerToSpectate";
				string text;
				if (ritual == null)
				{
					text = null;
				}
				else
				{
					RitualBehaviorWorker behavior = ritual.behavior;
					text = ((behavior != null) ? behavior.def.spectatorGerund : null);
				}
				return key.Translate(text ?? "Spectate".Translate()).CapitalizeFirst();
			}
			if (ModsConfig.BiotechActive && role != null && !role.allowBaby && (p.DevelopmentalStage == DevelopmentalStage.Baby || p.DevelopmentalStage == DevelopmentalStage.Newborn))
			{
				return "MessageRitualRoleCannotBeBaby".Translate(role.LabelCap);
			}
			if (ModsConfig.AnomalyActive && p.IsSubhuman)
			{
				return "MessageRitualCannotBeMutant".Translate();
			}
			Lord lord = p.GetLord();
			LordJob lordJob = (lord != null) ? lord.LordJob : null;
			if (lordJob != null && !(lordJob is LordJob_VoluntarilyJoinable) && assignments != null)
			{
				List<Pawn> list = assignments.requiredPawns;
				bool? flag2 = (list != null) ? new bool?(list.Contains(p)) : null;
				bool flag3 = false;
				if (flag2.GetValueOrDefault() == flag3 & flag2 != null)
				{
					stillAddToPawnList = true;
					return "MessageRitualRoleBusy".Translate(p);
				}
			}
			if (p.IsPrisoner)
			{
				if (p.guest.Released)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnReleased".Translate(p);
				}
				if (!p.guest.PrisonerIsSecure)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnPrisonerNotSecured".Translate(p);
				}
			}
			else if (ritualTarget.IsValid && (ritual == null || !ritual.ignoreExtremeTemperatures) && !p.SafeTemperatureRange().IncludesEpsilon(ritualTarget.Cell.GetTemperature(ritualTarget.Map)))
			{
				return "MessageRitualWontAttendExtremeTemperature".Translate(p);
			}
			if (p.IsSlave)
			{
				if (p.guest.Released)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnReleased".Translate(p);
				}
				if (p.InMentalState && p.MentalStateDef != MentalStateDefOf.Rebellion)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnMentalState".Translate(p);
				}
				if (!p.guest.SlaveIsSecure)
				{
					stillAddToPawnList = true;
					return "MessageRitualPawnSlaveNotSecured".Translate(p);
				}
			}
			if (role == null && !p.RaceProps.Humanlike)
			{
				return "MessageRitualRoleMustBeHumanlike".Translate("Spectators".Translate());
			}
			if (role == null && ritual != null && !ritual.def.allowSpectatorsFromOtherIdeos && ritual.ritualOnlyForIdeoMembers && p.Ideo != ritual.ideo)
			{
				string key2 = "MessageRitualRoleMustHaveIdeoToSpectate";
				NamedArgument arg = ritual.ideo.MemberNamePlural;
				string text2;
				if (ritual == null)
				{
					text2 = null;
				}
				else
				{
					RitualBehaviorWorker behavior2 = ritual.behavior;
					text2 = ((behavior2 != null) ? behavior2.def.spectatorGerund : null);
				}
				return key2.Translate(arg, text2 ?? "Spectate".Translate());
			}
			string result;
			if (ritual != null && !ritual.behavior.PawnCanFillRole(p, role, out result, ritualTarget))
			{
				return result;
			}
			if (role != null && !role.AppliesToPawn(p, out result, ritualTarget, null, assignments, null, false))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600DE63 RID: 56931 RVA: 0x00416EC8 File Offset: 0x004150C8
		public void SetRoleChangeSelection(Precept_Role role)
		{
			this.roleChangeSelection = role;
		}

		// Token: 0x0600DE64 RID: 56932 RVA: 0x00416ED4 File Offset: 0x004150D4
		public void UpdateRoleChangeTargetRole(Pawn pawn)
		{
			Precept_Role precept_Role = null;
			Ideo ideo = pawn.Ideo;
			if (((ideo != null) ? ideo.GetRole(pawn) : null) == null)
			{
				precept_Role = RitualUtility.AllRolesForPawn(pawn).FirstOrDefault((Precept_Role r) => r.Active && r.RequirementsMet(pawn));
			}
			this.SetRoleChangeSelection(precept_Role);
		}

		// Token: 0x0600DE65 RID: 56933 RVA: 0x00416F33 File Offset: 0x00415133
		public IEnumerable<IGrouping<string, RitualRole>> RoleGroups()
		{
			return from r in this.AllRolesForReading
			group r by r.mergeId ?? r.id;
		}

		// Token: 0x0600DE67 RID: 56935 RVA: 0x00416F70 File Offset: 0x00415170
		[CompilerGenerated]
		private bool <CandidatesForRole>g__ShouldIncludePawn|53_0(Pawn pawn, ref RitualRoleAssignments.<>c__DisplayClass53_0 A_2)
		{
			return (A_2.includeAssigned || (A_2.includeAssignedForSameRole && this.RoleForPawn(pawn, true) == A_2.role) || this.RoleForPawn(pawn, true) == null) && GatheringsUtility.ShouldPawnKeepAttendingRitual(pawn, this.ritual, A_2.role != null && A_2.role.ignoreBleeding);
		}

		// Token: 0x040095BA RID: 38330
		private List<Pawn> allPawns;

		// Token: 0x040095BB RID: 38331
		private List<Pawn> nonAssignablePawns;

		// Token: 0x040095BC RID: 38332
		private List<RitualRole> allRoles;

		// Token: 0x040095BD RID: 38333
		private Dictionary<string, Pawn> forcedRoles;

		// Token: 0x040095BE RID: 38334
		private Dictionary<string, List<Pawn>> assignedRoles = new Dictionary<string, List<Pawn>>();

		// Token: 0x040095BF RID: 38335
		private Precept_Role roleChangeSelection;

		// Token: 0x040095C0 RID: 38336
		private List<Pawn> spectators = new List<Pawn>();

		// Token: 0x040095C1 RID: 38337
		private List<Pawn> requiredPawns;

		// Token: 0x040095C2 RID: 38338
		private Precept_Ritual ritual;

		// Token: 0x040095C3 RID: 38339
		private TargetInfo ritualTarget;

		// Token: 0x040095C4 RID: 38340
		private Pawn selectedPawn;

		// Token: 0x040095C5 RID: 38341
		private List<Pawn> tmpParticipants = new List<Pawn>();

		// Token: 0x040095C6 RID: 38342
		private List<Pawn> tmpForcedRolePawns;

		// Token: 0x040095C7 RID: 38343
		private List<string> tmpForcedRoleIds;

		// Token: 0x040095C8 RID: 38344
		private List<string> tmpAssignedRoleIds;

		// Token: 0x040095C9 RID: 38345
		private List<List<Pawn>> tmpAssignedRolePawns;

		// Token: 0x040095CA RID: 38346
		private static List<Pawn> tmpOrderedPawns = new List<Pawn>(32);
	}
}
