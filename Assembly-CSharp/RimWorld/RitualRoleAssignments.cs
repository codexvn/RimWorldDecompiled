using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Verse;
using Verse.AI.Group;

namespace RimWorld;

public class RitualRoleAssignments : ILordJobAssignmentsManager<RitualRole>, ILordJobCandidatePool, IExposable
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003C_003Ec__DisplayClass53_0
	{
		public bool includeAssigned;

		public bool includeAssignedForSameRole;

		public RitualRoleAssignments _003C_003E4__this;

		public RitualRole role;
	}

	[CompilerGenerated]
	private sealed class _003CAssignedPawns_003Ed__54 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RitualRoleAssignments _003C_003E4__this;

		private RitualRole role;

		public RitualRole _003C_003E3__role;

		private Dictionary<string, Pawn>.Enumerator _003C_003E7__wrap1;

		private List<Pawn>.Enumerator _003C_003E7__wrap2;

		Pawn IEnumerator<Pawn>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CAssignedPawns_003Ed__54(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = default(Dictionary<string, Pawn>.Enumerator);
			_003C_003E7__wrap2 = default(List<Pawn>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RitualRoleAssignments ritualRoleAssignments = _003C_003E4__this;
				List<Pawn> value;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (ritualRoleAssignments.forcedRoles != null)
					{
						_003C_003E7__wrap1 = ritualRoleAssignments.forcedRoles.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_009d;
					}
					goto IL_00bc;
				case 1:
					_003C_003E1__state = -3;
					goto IL_009d;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_0114;
					}
					IL_009d:
					while (_003C_003E7__wrap1.MoveNext())
					{
						KeyValuePair<string, Pawn> current = _003C_003E7__wrap1.Current;
						if (current.Key == role.id)
						{
							_003C_003E2__current = current.Value;
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(Dictionary<string, Pawn>.Enumerator);
					goto IL_00bc;
					IL_0114:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Pawn current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<Pawn>.Enumerator);
					break;
					IL_00bc:
					if (!ritualRoleAssignments.assignedRoles.TryGetValue(role.id, out value))
					{
						break;
					}
					_003C_003E7__wrap2 = value.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0114;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003CAssignedPawns_003Ed__54 _003CAssignedPawns_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAssignedPawns_003Ed__ = this;
			}
			else
			{
				_003CAssignedPawns_003Ed__ = new _003CAssignedPawns_003Ed__54(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAssignedPawns_003Ed__.role = _003C_003E3__role;
			return _003CAssignedPawns_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCandidatesForRole_003Ed__53 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool includeAssigned;

		public bool _003C_003E3__includeAssigned;

		private bool includeAssignedForSameRole;

		public bool _003C_003E3__includeAssignedForSameRole;

		public RitualRoleAssignments _003C_003E4__this;

		private RitualRole role;

		public RitualRole _003C_003E3__role;

		private _003C_003Ec__DisplayClass53_0 _003C_003E8__1;

		private TargetInfo ritualTarget;

		public TargetInfo _003C_003E3__ritualTarget;

		private List<Pawn>.Enumerator _003C_003E7__wrap1;

		Pawn IEnumerator<Pawn>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CCandidatesForRole_003Ed__53(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = default(_003C_003Ec__DisplayClass53_0);
			_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RitualRoleAssignments ritualRoleAssignments = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					_003C_003E8__1.includeAssigned = includeAssigned;
					_003C_003E8__1.includeAssignedForSameRole = includeAssignedForSameRole;
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E8__1.role = role;
					if (ritualRoleAssignments.forcedRoles != null && ritualRoleAssignments.forcedRoles.TryGetValue(_003C_003E8__1.role.id, out var value))
					{
						_003C_003E2__current = value;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E7__wrap1 = ritualRoleAssignments.allPawns.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -1;
					return false;
				case 2:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Pawn current = _003C_003E7__wrap1.Current;
					if (_003C_003E8__1.role.AppliesToPawn(current, out var _, ritualTarget, null, ritualRoleAssignments, null, skipReason: true) && ritualRoleAssignments._003CCandidatesForRole_003Eg__ShouldIncludePawn_007C53_0(current, ref _003C_003E8__1))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003CCandidatesForRole_003Ed__53 _003CCandidatesForRole_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCandidatesForRole_003Ed__ = this;
			}
			else
			{
				_003CCandidatesForRole_003Ed__ = new _003CCandidatesForRole_003Ed__53(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCandidatesForRole_003Ed__.role = _003C_003E3__role;
			_003CCandidatesForRole_003Ed__.ritualTarget = _003C_003E3__ritualTarget;
			_003CCandidatesForRole_003Ed__.includeAssigned = _003C_003E3__includeAssigned;
			_003CCandidatesForRole_003Ed__.includeAssignedForSameRole = _003C_003E3__includeAssignedForSameRole;
			return _003CCandidatesForRole_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpectatorCandidates_003Ed__51 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RitualRoleAssignments _003C_003E4__this;

		private List<Pawn>.Enumerator _003C_003E7__wrap1;

		Pawn IEnumerator<Pawn>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CSpectatorCandidates_003Ed__51(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RitualRoleAssignments ritualRoleAssignments = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ritualRoleAssignments.allPawns.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Pawn current = _003C_003E7__wrap1.Current;
					if (ritualRoleAssignments.CanEverSpectate(current) && ritualRoleAssignments.RoleForPawn(current) == null)
					{
						Precept_Ritual ritual = ritualRoleAssignments.ritual;
						if (ritual == null || ritual.behavior?.ShouldInitAsSpectator(current, ritualRoleAssignments) != false)
						{
							_003C_003E2__current = current;
							_003C_003E1__state = 1;
							return true;
						}
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003CSpectatorCandidates_003Ed__51 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpectatorCandidates_003Ed__51(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	private List<Pawn> allPawns;

	private List<Pawn> nonAssignablePawns;

	private List<RitualRole> allRoles;

	private Dictionary<string, Pawn> forcedRoles;

	private Dictionary<string, List<Pawn>> assignedRoles = new Dictionary<string, List<Pawn>>();

	private Precept_Role roleChangeSelection;

	private List<Pawn> spectators = new List<Pawn>();

	private List<Pawn> requiredPawns;

	private Precept_Ritual ritual;

	private TargetInfo ritualTarget;

	private Pawn selectedPawn;

	private List<Pawn> tmpParticipants = new List<Pawn>();

	private List<Pawn> tmpForcedRolePawns;

	private List<string> tmpForcedRoleIds;

	private List<string> tmpAssignedRoleIds;

	private List<List<Pawn>> tmpAssignedRolePawns;

	private static List<Pawn> tmpOrderedPawns = new List<Pawn>(32);

	public List<Pawn> SpectatorsForReading => spectators;

	public Precept_Ritual Ritual => ritual;

	public bool SpectatorsAllowed => true;

	public List<Pawn> Participants
	{
		get
		{
			tmpParticipants.Clear();
			foreach (Pawn allPawn in allPawns)
			{
				if (PawnParticipating(allPawn))
				{
					tmpParticipants.Add(allPawn);
				}
			}
			return tmpParticipants;
		}
	}

	public List<Pawn> AllCandidatePawns => allPawns;

	public List<Pawn> NonAssignablePawns => nonAssignablePawns;

	public List<RitualRole> AllRolesForReading => allRoles;

	public List<Pawn> ExtraRequiredPawnsForReading => requiredPawns;

	public Dictionary<string, Pawn> ForcedRolesForReading => forcedRoles;

	public Precept_Role RoleChangeSelection => roleChangeSelection;

	public void ExposeData()
	{
		Scribe_Collections.Look(ref allPawns, "allPawns", true, LookMode.Reference);
		Scribe_Collections.Look(ref nonAssignablePawns, "nonAssignablePawns", true, LookMode.Reference);
		Scribe_Collections.Look(ref spectators, "spectators", true, LookMode.Reference);
		Scribe_Collections.Look(ref requiredPawns, "requiredPawns", true, LookMode.Reference);
		Scribe_Collections.Look(ref forcedRoles, "forcedRoles", LookMode.Value, LookMode.Reference, ref tmpForcedRoleIds, ref tmpForcedRolePawns, logNullErrors: true, saveDestroyedKeys: false, saveDestroyedValues: true);
		Scribe_Collections.Look(ref assignedRoles, "assignedRoles", LookMode.Value, LookMode.Reference, ref tmpAssignedRoleIds, ref tmpAssignedRolePawns, logNullErrors: true, saveDestroyedKeys: false, saveDestroyedValues: true);
		Scribe_References.Look(ref ritual, "ritual");
		Scribe_TargetInfo.Look(ref ritualTarget, saveDestroyedThings: true, "ritualTarget");
		Scribe_References.Look(ref selectedPawn, "selectedPawn", saveDestroyedThings: true);
		Scribe_References.Look(ref roleChangeSelection, "roleChangeSelection");
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			allRoles = ((ritual != null) ? new List<RitualRole>(ritual.behavior.def.roles) : new List<RitualRole>());
		}
	}

	protected RitualRoleAssignments()
	{
	}

	public RitualRoleAssignments(Precept_Ritual ritual, TargetInfo ritualTarget)
	{
		this.ritual = ritual;
		this.ritualTarget = ritualTarget;
	}

	public void Setup(List<Pawn> allPawns, List<Pawn> nonAssignablePawns, Dictionary<string, Pawn> forcedRoles = null, List<Pawn> requiredPawns = null, Pawn selectedPawn = null)
	{
		this.allPawns = allPawns;
		this.nonAssignablePawns = nonAssignablePawns;
		this.forcedRoles = forcedRoles;
		allRoles = ((ritual != null) ? new List<RitualRole>(ritual.behavior.def.roles) : new List<RitualRole>());
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

	public bool Forced(Pawn pawn)
	{
		if (forcedRoles != null)
		{
			return forcedRoles.ContainsValue(pawn);
		}
		return false;
	}

	public RitualRole ForcedRole(Pawn pawn)
	{
		if (forcedRoles == null)
		{
			return null;
		}
		string text = null;
		foreach (KeyValuePair<string, Pawn> forcedRole in forcedRoles)
		{
			if (forcedRole.Value == pawn)
			{
				text = forcedRole.Key;
			}
		}
		if (text == null)
		{
			return null;
		}
		foreach (RitualRole allRole in allRoles)
		{
			if (allRole.id == text)
			{
				return allRole;
			}
		}
		return null;
	}

	public void RemoveParticipant(Pawn pawn)
	{
		TryUnassignAnyRole(pawn);
		spectators.Remove(pawn);
		allPawns.Remove(pawn);
		allPawns.Add(pawn);
	}

	public bool TryUnassignAnyRole(Pawn pawn)
	{
		foreach (KeyValuePair<string, List<Pawn>> assignedRole in assignedRoles)
		{
			if (assignedRole.Value.Remove(pawn))
			{
				if (CanEverSpectate(pawn))
				{
					spectators.Add(pawn);
				}
				return true;
			}
		}
		return false;
	}

	public bool TryAssign(Pawn pawn, RitualRole role, out PsychicRitualRoleDef.Reason reason, PsychicRitualRoleDef.Context context = PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, Pawn insertBefore = null)
	{
		reason = PsychicRitualRoleDef.Reason.None;
		if (forcedRoles != null && forcedRoles.ContainsValue(pawn))
		{
			return false;
		}
		if (PawnNotAssignableReason(pawn, role, out var _) != null)
		{
			return false;
		}
		if (role == null)
		{
			return TryAssignSpectate(pawn);
		}
		if (role.AppliesToPawn(pawn, out var _, ritualTarget, null, this, null, skipReason: true) && (role.maxCount <= 0 || AssignedPawns(role).Count() < role.maxCount))
		{
			TryUnassignAnyRole(pawn);
			spectators.Remove(pawn);
			if (!assignedRoles.TryGetValue(role.id, out var value))
			{
				value = new List<Pawn>();
				assignedRoles.Add(role.id, value);
			}
			if (insertBefore != null && value.Contains(insertBefore))
			{
				value.Insert(value.IndexOf(insertBefore), pawn);
			}
			else
			{
				value.Add(pawn);
			}
			if (role is RitualRoleIdeoRoleChanger)
			{
				UpdateRoleChangeTargetRole(pawn);
			}
			return true;
		}
		return false;
	}

	public bool TryAssignSpectate(Pawn pawn, Pawn insertBefore = null)
	{
		if (spectators.Contains(pawn) || !CanEverSpectate(pawn) || PawnNotAssignableReason(pawn, null, out var _) != null)
		{
			return false;
		}
		TryUnassignAnyRole(pawn);
		if (!spectators.Contains(pawn))
		{
			if (insertBefore != null && spectators.Contains(insertBefore))
			{
				spectators.Insert(spectators.IndexOf(insertBefore), pawn);
			}
			else
			{
				spectators.Add(pawn);
			}
		}
		return RoleForPawn(pawn) == null;
	}

	public RitualRole GetRole(string roleId)
	{
		if (!AllRolesForReading.NullOrEmpty())
		{
			foreach (RitualRole item in AllRolesForReading)
			{
				if (item.id == roleId)
				{
					return item;
				}
			}
		}
		return null;
	}

	public bool CanParticipate(Pawn pawn, out TaggedString reason)
	{
		if (forcedRoles != null && forcedRoles.ContainsValue(pawn))
		{
			reason = TaggedString.Empty;
			return true;
		}
		if (Required(pawn))
		{
			reason = TaggedString.Empty;
			return true;
		}
		if (pawn == selectedPawn)
		{
			reason = TaggedString.Empty;
			return true;
		}
		foreach (RitualRole item in AllRolesForReading)
		{
			if (PawnNotAssignableReason(pawn, item, out var _) == null)
			{
				reason = TaggedString.Empty;
				return true;
			}
		}
		if (CanEverSpectate(pawn))
		{
			reason = TaggedString.Empty;
			return true;
		}
		reason = PawnNotAssignableReason(pawn, null);
		return false;
	}

	public Pawn FirstAssignedPawn(RitualRole role)
	{
		return FirstAssignedPawn(role.id);
	}

	public Pawn FirstAssignedPawn(string roleId)
	{
		if (forcedRoles != null && forcedRoles.TryGetValue(roleId, out var value))
		{
			return value;
		}
		if (assignedRoles.TryGetValue(roleId, out var value2) && value2.Count > 0)
		{
			return value2[0];
		}
		return null;
	}

	public bool CanEverSpectate(Pawn pawn)
	{
		if (ritual != null && ritual.ritualOnlyForIdeoMembers && pawn.Ideo != ritual.ideo && !ritual.def.allowSpectatorsFromOtherIdeos)
		{
			return false;
		}
		if (ritual != null && ritual.behavior.def.spectatorFilter != null && !ritual.behavior.def.spectatorFilter.Allowed(pawn))
		{
			return false;
		}
		if (ritual != null && !ritual.behavior.PawnCanFillRole(pawn, null, out var _, ritualTarget))
		{
			return false;
		}
		if (pawn.RaceProps.Humanlike && !pawn.IsPrisoner)
		{
			return GatheringsUtility.ShouldPawnKeepAttendingRitual(pawn, ritual, ritual?.behavior.def.spectatorsIgnoreBleeding ?? false);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CSpectatorCandidates_003Ed__51))]
	public IEnumerable<Pawn> SpectatorCandidates()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpectatorCandidates_003Ed__51(-2)
		{
			_003C_003E4__this = this
		};
	}

	public IEnumerable<Pawn> CandidatesForRole(string roleId, TargetInfo ritualTarget, bool includeAssigned = false, bool includeAssignedForSameRole = false, bool includeForced = true)
	{
		return CandidatesForRole(GetRole(roleId), ritualTarget, includeAssigned, includeAssignedForSameRole, includeForced);
	}

	[IteratorStateMachine(typeof(_003CCandidatesForRole_003Ed__53))]
	public IEnumerable<Pawn> CandidatesForRole(RitualRole role, TargetInfo ritualTarget, bool includeAssigned = false, bool includeAssignedForSameRole = false, bool includeForced = true)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCandidatesForRole_003Ed__53(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__role = role,
			_003C_003E3__ritualTarget = ritualTarget,
			_003C_003E3__includeAssigned = includeAssigned,
			_003C_003E3__includeAssignedForSameRole = includeAssignedForSameRole
		};
	}

	[IteratorStateMachine(typeof(_003CAssignedPawns_003Ed__54))]
	public IEnumerable<Pawn> AssignedPawns(RitualRole role)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAssignedPawns_003Ed__54(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__role = role
		};
	}

	public bool AnyPawnAssigned(string roleId)
	{
		if (forcedRoles == null || !forcedRoles.ContainsKey(roleId))
		{
			if (assignedRoles.ContainsKey(roleId))
			{
				return assignedRoles[roleId].Any();
			}
			return false;
		}
		return true;
	}

	public bool AnyPawnAssigned(RitualRole role)
	{
		return AnyPawnAssigned(role.id);
	}

	public IEnumerable<Pawn> AssignedPawns(string roleId)
	{
		return AssignedPawns(GetRole(roleId));
	}

	public RitualRole RoleForPawn(Pawn pawn, bool includeForced = true)
	{
		if (spectators.Contains(pawn))
		{
			return null;
		}
		if (includeForced && forcedRoles != null)
		{
			foreach (KeyValuePair<string, Pawn> forcedRole in forcedRoles)
			{
				if (forcedRole.Value == pawn)
				{
					return GetRole(forcedRole.Key);
				}
			}
		}
		foreach (KeyValuePair<string, List<Pawn>> assignedRole in assignedRoles)
		{
			if (!assignedRole.Value.NullOrEmpty() && assignedRole.Value.Contains(pawn))
			{
				return GetRole(assignedRole.Key);
			}
		}
		return null;
	}

	public bool PawnParticipating(Pawn pawn)
	{
		if (RoleForPawn(pawn) == null)
		{
			return PawnSpectating(pawn);
		}
		return true;
	}

	public bool PawnSpectating(Pawn pawn)
	{
		return spectators.Contains(pawn);
	}

	public void FillPawns(Dialog_BeginRitual.PawnFilter filter, TargetInfo ritualTarget)
	{
		if (!requiredPawns.NullOrEmpty())
		{
			foreach (Pawn requiredPawn in requiredPawns)
			{
				if (forcedRoles == null || !forcedRoles.ContainsValue(requiredPawn))
				{
					TryAssignSpectate(requiredPawn);
				}
			}
		}
		string reason;
		PsychicRitualRoleDef.Reason reason2;
		if (selectedPawn != null && RoleForPawn(selectedPawn) == null)
		{
			foreach (RitualRole item in AllRolesForReading)
			{
				if (item.defaultForSelectedColonist && item.AppliesToPawn(selectedPawn, out reason, ritualTarget, null, this))
				{
					TryAssign(selectedPawn, item, out reason2);
					break;
				}
			}
		}
		foreach (RitualRole role in AllRolesForReading)
		{
			tmpOrderedPawns.Clear();
			tmpOrderedPawns.AddRange(allPawns.Where((Pawn pawn) => filter == null || filter(pawn, !(role is RitualRoleForced), role.allowOtherIdeos)));
			role.OrderByDesirability(tmpOrderedPawns);
			foreach (Pawn tmpOrderedPawn in tmpOrderedPawns)
			{
				if (RoleForPawn(tmpOrderedPawn) == null)
				{
					if (role.maxCount > 0 && AssignedPawns(role).Count() >= role.maxCount)
					{
						break;
					}
					if (role.AppliesToPawn(tmpOrderedPawn, out reason, ritualTarget, null, this, null, skipReason: true))
					{
						TryAssign(tmpOrderedPawn, role, out reason2);
					}
				}
			}
		}
		foreach (Pawn item2 in SpectatorCandidates())
		{
			TryAssignSpectate(item2);
		}
		List<Pawn> pawnsToRemove = new List<Pawn>();
		foreach (Pawn allPawn in allPawns)
		{
			RitualRole ritualRole = RoleForPawn(allPawn);
			if (ritualRole != null && ritualRole.required && ritualRole.substitutable && PawnNotAssignableReason(allPawn, ritualRole, out var _) != null)
			{
				RemoveParticipant(allPawn);
				pawnsToRemove.Add(allPawn);
			}
		}
		allPawns.RemoveAll((Pawn p) => pawnsToRemove.Contains(p));
	}

	public bool Required(Pawn pawn)
	{
		return requiredPawns.NotNullAndContains(pawn);
	}

	public bool RoleSubstituted(string roleId)
	{
		if (ritual.behavior.def.roles.NullOrEmpty())
		{
			return false;
		}
		RitualRole role = ritual.behavior.def.roles.FirstOrDefault((RitualRole r) => r.id == roleId);
		if (role == null)
		{
			return false;
		}
		if (!role.substitutable || role.precept == null)
		{
			return false;
		}
		Precept precept = ritual.ideo.PreceptsListForReading.FirstOrDefault((Precept p) => p.def == role.precept);
		if (precept == null)
		{
			return false;
		}
		bool result = false;
		foreach (Pawn item in AssignedPawns(roleId))
		{
			if (item.Ideo?.GetRole(item) != precept)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public string PawnNotAssignableReason(Pawn p, RitualRole role)
	{
		bool stillAddToPawnList;
		return PawnNotAssignableReason(p, role, out stillAddToPawnList);
	}

	public string PawnNotAssignableReason(Pawn p, RitualRole role, out bool stillAddToPawnList)
	{
		return PawnNotAssignableReason(p, role, ritual, this, ritualTarget, out stillAddToPawnList);
	}

	public static string PawnNotAssignableReason(Pawn p, RitualRole role, Precept_Ritual ritual, RitualRoleAssignments assignments, TargetInfo ritualTarget, out bool stillAddToPawnList)
	{
		stillAddToPawnList = false;
		if (p.Downed && (role == null || !role.allowDowned))
		{
			return "MessageRitualPawnDowned".Translate(p);
		}
		bool flag = role?.ignoreBleeding ?? ritual?.behavior.def.spectatorsIgnoreBleeding ?? false;
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
			return "MessageRitualRoleMustNotBePrisonerToSpectate".Translate(ritual?.behavior?.def.spectatorGerund ?? ((string)"Spectate".Translate())).CapitalizeFirst();
		}
		if (ModsConfig.BiotechActive && role != null && !role.allowBaby && (p.DevelopmentalStage == DevelopmentalStage.Baby || p.DevelopmentalStage == DevelopmentalStage.Newborn))
		{
			return "MessageRitualRoleCannotBeBaby".Translate(role.LabelCap);
		}
		if (ModsConfig.AnomalyActive && p.IsSubhuman)
		{
			return "MessageRitualCannotBeMutant".Translate();
		}
		LordJob lordJob = p.GetLord()?.LordJob;
		if (lordJob != null && !(lordJob is LordJob_VoluntarilyJoinable) && assignments != null && assignments.requiredPawns?.Contains(p) == false)
		{
			stillAddToPawnList = true;
			return "MessageRitualRoleBusy".Translate(p);
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
			return "MessageRitualRoleMustHaveIdeoToSpectate".Translate(ritual.ideo.MemberNamePlural, ritual?.behavior?.def.spectatorGerund ?? ((string)"Spectate".Translate()));
		}
		if (ritual != null && !ritual.behavior.PawnCanFillRole(p, role, out var s, ritualTarget))
		{
			return s;
		}
		if (role != null && !role.AppliesToPawn(p, out s, ritualTarget, null, assignments))
		{
			return s;
		}
		return null;
	}

	public void SetRoleChangeSelection(Precept_Role role)
	{
		roleChangeSelection = role;
	}

	public void UpdateRoleChangeTargetRole(Pawn pawn)
	{
		Precept_Role precept_Role = null;
		if (pawn.Ideo?.GetRole(pawn) == null)
		{
			precept_Role = RitualUtility.AllRolesForPawn(pawn).FirstOrDefault((Precept_Role r) => r.Active && r.RequirementsMet(pawn));
		}
		SetRoleChangeSelection(precept_Role);
	}

	public IEnumerable<IGrouping<string, RitualRole>> RoleGroups()
	{
		return from r in AllRolesForReading
			group r by r.mergeId ?? r.id;
	}

	[CompilerGenerated]
	private bool _003CCandidatesForRole_003Eg__ShouldIncludePawn_007C53_0(Pawn pawn, ref _003C_003Ec__DisplayClass53_0 P_1)
	{
		if (P_1.includeAssigned || (P_1.includeAssignedForSameRole && RoleForPawn(pawn) == P_1.role) || RoleForPawn(pawn) == null)
		{
			return GatheringsUtility.ShouldPawnKeepAttendingRitual(pawn, ritual, P_1.role != null && P_1.role.ignoreBleeding);
		}
		return false;
	}
}
