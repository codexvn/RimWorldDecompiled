using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02001062 RID: 4194
	public class PsychicRitualRoleAssignments : ILordJobAssignmentsManager<PsychicRitualRoleDef>, IExposable
	{
		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x0600665A RID: 26202 RVA: 0x00206420 File Offset: 0x00204620
		public IEnumerable<Pawn> AllAssignedPawns
		{
			get
			{
				return this.pawnAssignments.Keys;
			}
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x0600665B RID: 26203 RVA: 0x0020642D File Offset: 0x0020462D
		public int AssignedPawnCount
		{
			get
			{
				return this.pawnAssignments.Count;
			}
		}

		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x0600665C RID: 26204 RVA: 0x0020643A File Offset: 0x0020463A
		public List<Pawn> SpectatorsForReading
		{
			get
			{
				return this.spectators;
			}
		}

		// Token: 0x0600665D RID: 26205 RVA: 0x00206442 File Offset: 0x00204642
		public bool PawnParticipating(Pawn pawn)
		{
			return this.pawnAssignments.ContainsKey(pawn);
		}

		// Token: 0x0600665E RID: 26206 RVA: 0x00206450 File Offset: 0x00204650
		public bool Required(Pawn pawn)
		{
			return this.ForcedRole(pawn) != null;
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x0600665F RID: 26207 RVA: 0x00002501 File Offset: 0x00000701
		public bool SpectatorsAllowed
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x06006660 RID: 26208 RVA: 0x0020645C File Offset: 0x0020465C
		public IReadOnlyDictionary<PsychicRitualRoleDef, List<Pawn>> RoleAssignments
		{
			get
			{
				return this.roleAssignments;
			}
		}

		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x06006661 RID: 26209 RVA: 0x00206464 File Offset: 0x00204664
		public virtual TargetInfo Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x06006662 RID: 26210 RVA: 0x000024DF File Offset: 0x000006DF
		protected PsychicRitualRoleAssignments()
		{
		}

		// Token: 0x06006663 RID: 26211 RVA: 0x0020646C File Offset: 0x0020466C
		public PsychicRitualRoleAssignments(List<PsychicRitualRoleDef> roles, TargetInfo target, Dictionary<Pawn, PsychicRitualRoleDef> forcedAssignments = null)
		{
			this.target = target;
			int num = 0;
			this.roleAssignments = new Dictionary<PsychicRitualRoleDef, List<Pawn>>(roles.Count);
			foreach (PsychicRitualRoleDef psychicRitualRoleDef in roles)
			{
				num += psychicRitualRoleDef.MaxCount;
				this.roleAssignments[psychicRitualRoleDef] = new List<Pawn>(psychicRitualRoleDef.MaxCount);
			}
			this.pawnAssignments = new Dictionary<Pawn, PsychicRitualRoleDef>(num);
			this.spectators = new List<Pawn>();
			if (forcedAssignments != null)
			{
				this.forcedAssignments = new Dictionary<Pawn, PsychicRitualRoleDef>(forcedAssignments);
				foreach (KeyValuePair<Pawn, PsychicRitualRoleDef> keyValuePair in forcedAssignments)
				{
					Pawn pawn;
					PsychicRitualRoleDef psychicRitualRoleDef2;
					keyValuePair.Deconstruct(out pawn, out psychicRitualRoleDef2);
					Pawn pawn2 = pawn;
					PsychicRitualRoleDef psychicRitualRoleDef3 = psychicRitualRoleDef2;
					PsychicRitualRoleDef.Reason reason;
					if (!this.TryAssign(pawn2, psychicRitualRoleDef3, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, null))
					{
						throw new Exception(string.Format("Cannot assign pawn {0} to forced role {1}. Reason: {2}", pawn2.ToStringSafe<Pawn>(), psychicRitualRoleDef3.ToStringSafe<PsychicRitualRoleDef>(), reason.ToPlayerReadable()));
					}
				}
			}
		}

		// Token: 0x06006664 RID: 26212 RVA: 0x002065A0 File Offset: 0x002047A0
		public void AddForcedRole(Pawn pawn, PsychicRitualRoleDef role, PsychicRitualRoleDef.Context context = PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual)
		{
			if (this.forcedAssignments == null)
			{
				this.forcedAssignments = new Dictionary<Pawn, PsychicRitualRoleDef>();
			}
			this.forcedAssignments[pawn] = role;
			PsychicRitualRoleDef.Reason reason;
			if (!this.TryAssign(pawn, role, out reason, context, null))
			{
				throw new InvalidOperationException(string.Format("Cannot force pawn {0} to have role {1}. Reason: {2}", pawn.ToStringSafe<Pawn>(), role.ToStringSafe<PsychicRitualRoleDef>(), reason.ToPlayerReadable()));
			}
		}

		// Token: 0x06006665 RID: 26213 RVA: 0x00206604 File Offset: 0x00204804
		public virtual bool CanParticipate(Pawn pawn, out TaggedString reason)
		{
			reason = TaggedString.Empty;
			using (Dictionary<PsychicRitualRoleDef, List<Pawn>>.KeyCollection.Enumerator enumerator = this.roleAssignments.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PsychicRitualRoleDef.Reason reason2;
					if (enumerator.Current.PawnCanDo(PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, pawn, this.target, out reason2))
					{
						reason = TaggedString.Empty;
						return true;
					}
					if (reason.NullOrEmpty())
					{
						reason = reason2.ToPlayerReadable().CapitalizeFirst().EndWithPeriod();
					}
				}
			}
			return false;
		}

		// Token: 0x06006666 RID: 26214 RVA: 0x002066A8 File Offset: 0x002048A8
		public virtual string PawnNotAssignableReason(Pawn pawn, PsychicRitualRoleDef role)
		{
			if (role != null)
			{
				Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.forcedAssignments;
				PsychicRitualRoleDef psychicRitualRoleDef = (dictionary != null) ? dictionary.TryGetValue(pawn, null) : null;
				if (psychicRitualRoleDef != null && role != psychicRitualRoleDef)
				{
					return "RoleIsLocked".Translate(role.label);
				}
				PsychicRitualRoleDef.Reason reason;
				if (!role.PawnCanDo(PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, pawn, this.target, out reason))
				{
					return reason.ToPlayerReadable().CapitalizeFirst().EndWithPeriod();
				}
				return null;
			}
			else
			{
				TaggedString taggedString;
				if (this.CanParticipate(pawn, out taggedString))
				{
					return null;
				}
				return taggedString;
			}
		}

		// Token: 0x06006667 RID: 26215 RVA: 0x00206734 File Offset: 0x00204934
		public int RoleAssignedCount(PsychicRitualRoleDef role)
		{
			List<Pawn> list;
			if (!this.roleAssignments.TryGetValue(role, out list))
			{
				return 0;
			}
			return list.Count;
		}

		// Token: 0x06006668 RID: 26216 RVA: 0x00002501 File Offset: 0x00000701
		public bool PawnSpectating(Pawn pawn)
		{
			return false;
		}

		// Token: 0x06006669 RID: 26217 RVA: 0x0020675C File Offset: 0x0020495C
		public bool TryAssignSpectate(Pawn pawn, Pawn insertBefore = null)
		{
			PsychicRitualRoleDef.Reason reason;
			return !this.IsChanter(pawn) && (this.TryAssign(pawn, PsychicRitualRoleDefOf.ChanterAdvanced, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, insertBefore) || this.TryAssign(pawn, PsychicRitualRoleDefOf.Chanter, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, insertBefore));
		}

		// Token: 0x0600666A RID: 26218 RVA: 0x00206798 File Offset: 0x00204998
		public PsychicRitualRoleDef ForcedRole(Pawn pawn)
		{
			Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.forcedAssignments;
			if (dictionary == null)
			{
				return null;
			}
			return dictionary.TryGetValue(pawn, null);
		}

		// Token: 0x0600666B RID: 26219 RVA: 0x002067B0 File Offset: 0x002049B0
		public bool IsChanter(Pawn pawn)
		{
			return (this.roleAssignments.ContainsKey(PsychicRitualRoleDefOf.ChanterAdvanced) && this.roleAssignments[PsychicRitualRoleDefOf.ChanterAdvanced].Contains(pawn)) || (this.roleAssignments.ContainsKey(PsychicRitualRoleDefOf.Chanter) && this.roleAssignments[PsychicRitualRoleDefOf.Chanter].Contains(pawn));
		}

		// Token: 0x0600666C RID: 26220 RVA: 0x00206816 File Offset: 0x00204A16
		public IEnumerable<IGrouping<string, PsychicRitualRoleDef>> RoleGroups()
		{
			return from role in this.roleAssignments.Keys
			group role by role.Label;
		}

		// Token: 0x0600666D RID: 26221 RVA: 0x00206848 File Offset: 0x00204A48
		public IEnumerable<Pawn> AssignedPawns(PsychicRitualRoleDef role)
		{
			IEnumerable<Pawn> enumerable = this.roleAssignments.TryGetValue(role, null);
			return enumerable ?? Enumerable.Empty<Pawn>();
		}

		// Token: 0x0600666E RID: 26222 RVA: 0x00206870 File Offset: 0x00204A70
		public PsychicRitualRoleDef RoleForPawn(Pawn pawn, bool includeForced = true)
		{
			PsychicRitualRoleDef result;
			if (this.pawnAssignments.TryGetValue(pawn, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600666F RID: 26223 RVA: 0x00206890 File Offset: 0x00204A90
		public Pawn FirstAssignedPawn(PsychicRitualRoleDef role)
		{
			if (role == null)
			{
				Log.Error("Tried to get first assigned pawn for null role.");
				return null;
			}
			List<Pawn> list;
			if (!this.roleAssignments.TryGetValue(role, out list))
			{
				return null;
			}
			if (list.Count == 0)
			{
				return null;
			}
			return list[0];
		}

		// Token: 0x06006670 RID: 26224 RVA: 0x002068D0 File Offset: 0x00204AD0
		public bool TryUnassignAnyRole(Pawn pawn)
		{
			PsychicRitualRoleDef psychicRitualRoleDef;
			if (!this.pawnAssignments.TryGetValue(pawn, out psychicRitualRoleDef) || !psychicRitualRoleDef.removeOnLost)
			{
				return false;
			}
			Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.forcedAssignments;
			PsychicRitualRoleDef psychicRitualRoleDef2 = (dictionary != null) ? dictionary.TryGetValue(pawn, null) : null;
			if (psychicRitualRoleDef == psychicRitualRoleDef2)
			{
				return false;
			}
			this.pawnAssignments.Remove(pawn);
			this.roleAssignments[psychicRitualRoleDef].Remove(pawn);
			return true;
		}

		// Token: 0x06006671 RID: 26225 RVA: 0x00206933 File Offset: 0x00204B33
		public void RemoveParticipant(Pawn pawn)
		{
			this.TryUnassignAnyRole(pawn);
		}

		// Token: 0x06006672 RID: 26226 RVA: 0x0020693D File Offset: 0x00204B3D
		public void RemoveAllParticipantsAndTargets()
		{
			this.pawnAssignments.Clear();
			Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.forcedAssignments;
			if (dictionary != null)
			{
				dictionary.Clear();
			}
			this.roleAssignments.ClearValueLists<PsychicRitualRoleDef, Pawn>();
			this.spectators.Clear();
			this.target = TargetInfo.Invalid;
		}

		// Token: 0x06006673 RID: 26227 RVA: 0x0020697C File Offset: 0x00204B7C
		public void TryAssignAnyUnassigned(List<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				if (!this.PawnParticipating(pawn))
				{
					Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.forcedAssignments;
					PsychicRitualRoleDef psychicRitualRoleDef = (dictionary != null) ? dictionary.TryGetValue(pawn, null) : null;
					PsychicRitualRoleDef.Reason reason;
					if (psychicRitualRoleDef != null && this.TryAssign(pawn, psychicRitualRoleDef, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, null))
					{
						break;
					}
					foreach (PsychicRitualRoleDef role in this.roleAssignments.Keys)
					{
						if (this.TryAssign(pawn, role, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, null))
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x06006674 RID: 26228 RVA: 0x00206A50 File Offset: 0x00204C50
		public bool TryAssign(Pawn pawn, PsychicRitualRoleDef role, out PsychicRitualRoleDef.Reason reason, PsychicRitualRoleDef.Context context = PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, Pawn insertBefore = null)
		{
			reason = PsychicRitualRoleDef.Reason.None;
			if (role == null)
			{
				return false;
			}
			if (pawn == null)
			{
				return false;
			}
			Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.forcedAssignments;
			PsychicRitualRoleDef psychicRitualRoleDef = (dictionary != null) ? dictionary.TryGetValue(pawn, null) : null;
			if (psychicRitualRoleDef != null && role != psychicRitualRoleDef)
			{
				return false;
			}
			List<Pawn> list;
			if (!this.roleAssignments.TryGetValue(role, out list))
			{
				return false;
			}
			if (!role.PawnCanDo(context, pawn, this.target, out reason))
			{
				return false;
			}
			if (list.Count >= role.MaxCount)
			{
				return false;
			}
			this.TryUnassignAnyRole(pawn);
			if (insertBefore != null)
			{
				list.Insert(list.IndexOf(insertBefore), pawn);
			}
			else
			{
				list.Add(pawn);
			}
			this.pawnAssignments[pawn] = role;
			return true;
		}

		// Token: 0x06006675 RID: 26229 RVA: 0x00206AF8 File Offset: 0x00204CF8
		public void SetTarget(TargetInfo target)
		{
			this.target = target;
		}

		// Token: 0x06006676 RID: 26230 RVA: 0x00206B01 File Offset: 0x00204D01
		public IEnumerable<string> BlockingIssues()
		{
			foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in this.roleAssignments)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<Pawn> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef role = psychicRitualRoleDef;
				List<Pawn> pawns = list;
				if (role.MinCount > 0 && pawns.Count < role.MinCount)
				{
					if (role.MinCount == 1)
					{
						yield return "MessageLordJobNeedsAtLeastOneRolePawn".Translate(role.label.CapitalizeFirst());
					}
					else
					{
						yield return "MessageLordJobNeedsAtLeastNumRolePawn".Translate(role.LabelCap, role.MinCount);
					}
				}
				foreach (Pawn pawn in pawns)
				{
					foreach (string text in role.BlockingIssues(pawn, this.Target))
					{
						yield return text;
					}
					IEnumerator<string> enumerator3 = null;
					Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.forcedAssignments;
					PsychicRitualRoleDef psychicRitualRoleDef2 = (dictionary != null) ? dictionary.TryGetValue(pawn, null) : null;
					if (psychicRitualRoleDef2 != null && psychicRitualRoleDef2 != role)
					{
						yield return "RoleIsLocked".Translate(role.label);
					}
					pawn = null;
				}
				List<Pawn>.Enumerator enumerator2 = default(List<Pawn>.Enumerator);
				role = null;
				pawns = null;
			}
			Dictionary<PsychicRitualRoleDef, List<Pawn>>.Enumerator enumerator = default(Dictionary<PsychicRitualRoleDef, List<Pawn>>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06006677 RID: 26231 RVA: 0x00206B14 File Offset: 0x00204D14
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (this.roleAssignments != null)
				{
					foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in this.roleAssignments)
					{
						List<Pawn> value = keyValuePair.Value;
						if (value != null)
						{
							value.RemoveAll((Pawn pawn) => pawn == null || pawn.Discarded);
						}
					}
				}
				Dictionary<Pawn, PsychicRitualRoleDef> dictionary = this.pawnAssignments;
				if (dictionary != null)
				{
					dictionary.RemoveAll((KeyValuePair<Pawn, PsychicRitualRoleDef> kvp) => kvp.Key == null || kvp.Key.Discarded);
				}
				Dictionary<Pawn, PsychicRitualRoleDef> dictionary2 = this.forcedAssignments;
				if (dictionary2 != null)
				{
					dictionary2.RemoveAll((KeyValuePair<Pawn, PsychicRitualRoleDef> kvp) => kvp.Key == null || kvp.Key.Discarded);
				}
			}
			Scribe_Collections.Look<PsychicRitualRoleDef, List<Pawn>>(ref this.roleAssignments, "roleAssignments", LookMode.Def, LookMode.Reference, ref this.roleAssignmentsKeys, ref this.roleAssignmentsValues, true, false, true);
			Scribe_Collections.Look<Pawn, PsychicRitualRoleDef>(ref this.pawnAssignments, "pawnAssignments", LookMode.Reference, LookMode.Def, ref this.pawnAssignmentsKeys, ref this.pawnAssignmentsValues, false, true, false);
			Scribe_Collections.Look<Pawn, PsychicRitualRoleDef>(ref this.forcedAssignments, "forcedAssignments", LookMode.Reference, LookMode.Def, ref this.forcedAssignmentKeys, ref this.forcedAssignmentValues, false, true, false);
			Scribe_Collections.Look<Pawn>(ref this.spectators, "spectators", true, LookMode.Reference, Array.Empty<object>());
			Scribe_TargetInfo.Look(ref this.target, true, "target");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.roleAssignments != null)
				{
					foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair2 in this.roleAssignments)
					{
						List<Pawn> value2 = keyValuePair2.Value;
						if (value2 != null)
						{
							value2.RemoveAll((Pawn pawn) => pawn == null);
						}
					}
				}
				Dictionary<Pawn, PsychicRitualRoleDef> dictionary3 = this.pawnAssignments;
				if (dictionary3 != null)
				{
					dictionary3.RemoveAll((KeyValuePair<Pawn, PsychicRitualRoleDef> kvp) => kvp.Key == null);
				}
				Dictionary<Pawn, PsychicRitualRoleDef> dictionary4 = this.forcedAssignments;
				if (dictionary4 == null)
				{
					return;
				}
				dictionary4.RemoveAll((KeyValuePair<Pawn, PsychicRitualRoleDef> kvp) => kvp.Key == null);
			}
		}

		// Token: 0x04004927 RID: 18727
		protected Dictionary<PsychicRitualRoleDef, List<Pawn>> roleAssignments;

		// Token: 0x04004928 RID: 18728
		protected Dictionary<Pawn, PsychicRitualRoleDef> pawnAssignments;

		// Token: 0x04004929 RID: 18729
		protected List<Pawn> spectators;

		// Token: 0x0400492A RID: 18730
		protected TargetInfo target;

		// Token: 0x0400492B RID: 18731
		protected Dictionary<Pawn, PsychicRitualRoleDef> forcedAssignments;

		// Token: 0x0400492C RID: 18732
		private List<PsychicRitualRoleDef> roleAssignmentsKeys;

		// Token: 0x0400492D RID: 18733
		private List<List<Pawn>> roleAssignmentsValues;

		// Token: 0x0400492E RID: 18734
		private List<Pawn> pawnAssignmentsKeys;

		// Token: 0x0400492F RID: 18735
		private List<PsychicRitualRoleDef> pawnAssignmentsValues;

		// Token: 0x04004930 RID: 18736
		private List<Pawn> forcedAssignmentKeys;

		// Token: 0x04004931 RID: 18737
		private List<PsychicRitualRoleDef> forcedAssignmentValues;
	}
}
