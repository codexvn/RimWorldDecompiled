using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI.Group;

public class PsychicRitualRoleAssignments : ILordJobAssignmentsManager<PsychicRitualRoleDef>, IExposable
{
	[CompilerGenerated]
	private sealed class _003CBlockingIssues_003Ed__39 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PsychicRitualRoleAssignments _003C_003E4__this;

		private Dictionary<PsychicRitualRoleDef, List<Pawn>>.Enumerator _003C_003E7__wrap1;

		private PsychicRitualRoleDef _003Crole_003E5__3;

		private List<Pawn> _003Cpawns_003E5__4;

		private List<Pawn>.Enumerator _003C_003E7__wrap4;

		private Pawn _003Cpawn_003E5__6;

		private IEnumerator<string> _003C_003E7__wrap6;

		string IEnumerator<string>.Current
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
		public _003CBlockingIssues_003Ed__39(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -5) <= 2u || (uint)(num - 1) <= 3u)
			{
				try
				{
					if ((uint)(num - -5) <= 1u || (uint)(num - 3) <= 1u)
					{
						try
						{
							if (num == -5 || num == 3)
							{
								try
								{
								}
								finally
								{
									_003C_003Em__Finally3();
								}
							}
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(Dictionary<PsychicRitualRoleDef, List<Pawn>>.Enumerator);
			_003Crole_003E5__3 = null;
			_003Cpawns_003E5__4 = null;
			_003C_003E7__wrap4 = default(List<Pawn>.Enumerator);
			_003Cpawn_003E5__6 = null;
			_003C_003E7__wrap6 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PsychicRitualRoleAssignments psychicRitualRoleAssignments = _003C_003E4__this;
				PsychicRitualRoleDef psychicRitualRoleDef2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = psychicRitualRoleAssignments.roleAssignments.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_027a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0142;
				case 2:
					_003C_003E1__state = -3;
					goto IL_0142;
				case 3:
					_003C_003E1__state = -5;
					goto IL_01c8;
				case 4:
					{
						_003C_003E1__state = -4;
						goto IL_0243;
					}
					IL_027a:
					if (!_003C_003E7__wrap1.MoveNext())
					{
						_003C_003Em__Finally1();
						_003C_003E7__wrap1 = default(Dictionary<PsychicRitualRoleDef, List<Pawn>>.Enumerator);
						return false;
					}
					(_003Crole_003E5__3, _003Cpawns_003E5__4) = (KeyValuePair<PsychicRitualRoleDef, List<Pawn>>)(ref _003C_003E7__wrap1.Current);
					if (_003Crole_003E5__3.MinCount > 0 && _003Cpawns_003E5__4.Count < _003Crole_003E5__3.MinCount)
					{
						if (_003Crole_003E5__3.MinCount == 1)
						{
							_003C_003E2__current = "MessageLordJobNeedsAtLeastOneRolePawn".Translate(_003Crole_003E5__3.label.CapitalizeFirst());
							_003C_003E1__state = 1;
							return true;
						}
						_003C_003E2__current = "MessageLordJobNeedsAtLeastNumRolePawn".Translate(_003Crole_003E5__3.LabelCap, _003Crole_003E5__3.MinCount);
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0142;
					IL_01c8:
					if (_003C_003E7__wrap6.MoveNext())
					{
						string current = _003C_003E7__wrap6.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap6 = null;
					psychicRitualRoleDef2 = psychicRitualRoleAssignments.forcedAssignments?.TryGetValue(_003Cpawn_003E5__6);
					if (psychicRitualRoleDef2 != null && psychicRitualRoleDef2 != _003Crole_003E5__3)
					{
						_003C_003E2__current = "RoleIsLocked".Translate(_003Crole_003E5__3.label);
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0243;
					IL_0142:
					_003C_003E7__wrap4 = _003Cpawns_003E5__4.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_024a;
					IL_024a:
					if (_003C_003E7__wrap4.MoveNext())
					{
						_003Cpawn_003E5__6 = _003C_003E7__wrap4.Current;
						_003C_003E7__wrap6 = _003Crole_003E5__3.BlockingIssues(_003Cpawn_003E5__6, psychicRitualRoleAssignments.Target).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_01c8;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = default(List<Pawn>.Enumerator);
					_003Crole_003E5__3 = null;
					_003Cpawns_003E5__4 = null;
					goto IL_027a;
					IL_0243:
					_003Cpawn_003E5__6 = null;
					goto IL_024a;
				}
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
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap4/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			if (_003C_003E7__wrap6 != null)
			{
				_003C_003E7__wrap6.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CBlockingIssues_003Ed__39 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CBlockingIssues_003Ed__39(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	protected Dictionary<PsychicRitualRoleDef, List<Pawn>> roleAssignments;

	protected Dictionary<Pawn, PsychicRitualRoleDef> pawnAssignments;

	protected List<Pawn> spectators;

	protected TargetInfo target;

	protected Dictionary<Pawn, PsychicRitualRoleDef> forcedAssignments;

	private List<PsychicRitualRoleDef> roleAssignmentsKeys;

	private List<List<Pawn>> roleAssignmentsValues;

	private List<Pawn> pawnAssignmentsKeys;

	private List<PsychicRitualRoleDef> pawnAssignmentsValues;

	private List<Pawn> forcedAssignmentKeys;

	private List<PsychicRitualRoleDef> forcedAssignmentValues;

	public IEnumerable<Pawn> AllAssignedPawns => pawnAssignments.Keys;

	public int AssignedPawnCount => pawnAssignments.Count;

	public List<Pawn> SpectatorsForReading => spectators;

	public bool SpectatorsAllowed => false;

	public IReadOnlyDictionary<PsychicRitualRoleDef, List<Pawn>> RoleAssignments => roleAssignments;

	public virtual TargetInfo Target => target;

	public bool PawnParticipating(Pawn pawn)
	{
		return pawnAssignments.ContainsKey(pawn);
	}

	public bool Required(Pawn pawn)
	{
		return ForcedRole(pawn) != null;
	}

	protected PsychicRitualRoleAssignments()
	{
	}

	public PsychicRitualRoleAssignments(List<PsychicRitualRoleDef> roles, TargetInfo target, Dictionary<Pawn, PsychicRitualRoleDef> forcedAssignments = null)
	{
		this.target = target;
		int num = 0;
		roleAssignments = new Dictionary<PsychicRitualRoleDef, List<Pawn>>(roles.Count);
		foreach (PsychicRitualRoleDef role in roles)
		{
			num += role.MaxCount;
			roleAssignments[role] = new List<Pawn>(role.MaxCount);
		}
		pawnAssignments = new Dictionary<Pawn, PsychicRitualRoleDef>(num);
		spectators = new List<Pawn>();
		if (forcedAssignments == null)
		{
			return;
		}
		this.forcedAssignments = new Dictionary<Pawn, PsychicRitualRoleDef>(forcedAssignments);
		foreach (var (pawn2, psychicRitualRoleDef2) in forcedAssignments)
		{
			if (!TryAssign(pawn2, psychicRitualRoleDef2, out var reason))
			{
				throw new Exception($"Cannot assign pawn {pawn2.ToStringSafe()} to forced role {psychicRitualRoleDef2.ToStringSafe()}. Reason: {reason.ToPlayerReadable()}");
			}
		}
	}

	public void AddForcedRole(Pawn pawn, PsychicRitualRoleDef role, PsychicRitualRoleDef.Context context = PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual)
	{
		if (forcedAssignments == null)
		{
			forcedAssignments = new Dictionary<Pawn, PsychicRitualRoleDef>();
		}
		forcedAssignments[pawn] = role;
		if (!TryAssign(pawn, role, out var reason, context))
		{
			throw new InvalidOperationException($"Cannot force pawn {pawn.ToStringSafe()} to have role {role.ToStringSafe()}. Reason: {reason.ToPlayerReadable()}");
		}
	}

	public virtual bool CanParticipate(Pawn pawn, out TaggedString reason)
	{
		reason = TaggedString.Empty;
		foreach (PsychicRitualRoleDef key in roleAssignments.Keys)
		{
			if (key.PawnCanDo(PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, pawn, target, out var reason2))
			{
				reason = TaggedString.Empty;
				return true;
			}
			if (reason.NullOrEmpty())
			{
				reason = reason2.ToPlayerReadable().CapitalizeFirst().EndWithPeriod();
			}
		}
		return false;
	}

	public virtual string PawnNotAssignableReason(Pawn pawn, PsychicRitualRoleDef role)
	{
		if (role != null)
		{
			PsychicRitualRoleDef psychicRitualRoleDef = forcedAssignments?.TryGetValue(pawn);
			if (psychicRitualRoleDef != null && role != psychicRitualRoleDef)
			{
				return "RoleIsLocked".Translate(role.label);
			}
			if (!role.PawnCanDo(PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, pawn, target, out var reason))
			{
				return reason.ToPlayerReadable().CapitalizeFirst().EndWithPeriod();
			}
			return null;
		}
		if (CanParticipate(pawn, out var reason2))
		{
			return null;
		}
		return reason2;
	}

	public int RoleAssignedCount(PsychicRitualRoleDef role)
	{
		if (!roleAssignments.TryGetValue(role, out var value))
		{
			return 0;
		}
		return value.Count;
	}

	public bool PawnSpectating(Pawn pawn)
	{
		return false;
	}

	public bool TryAssignSpectate(Pawn pawn, Pawn insertBefore = null)
	{
		if (IsChanter(pawn))
		{
			return false;
		}
		if (TryAssign(pawn, PsychicRitualRoleDefOf.ChanterAdvanced, out var reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, insertBefore))
		{
			return true;
		}
		return TryAssign(pawn, PsychicRitualRoleDefOf.Chanter, out reason, PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual, insertBefore);
	}

	public PsychicRitualRoleDef ForcedRole(Pawn pawn)
	{
		return forcedAssignments?.TryGetValue(pawn);
	}

	public bool IsChanter(Pawn pawn)
	{
		if (roleAssignments.ContainsKey(PsychicRitualRoleDefOf.ChanterAdvanced) && roleAssignments[PsychicRitualRoleDefOf.ChanterAdvanced].Contains(pawn))
		{
			return true;
		}
		if (roleAssignments.ContainsKey(PsychicRitualRoleDefOf.Chanter) && roleAssignments[PsychicRitualRoleDefOf.Chanter].Contains(pawn))
		{
			return true;
		}
		return false;
	}

	public IEnumerable<IGrouping<string, PsychicRitualRoleDef>> RoleGroups()
	{
		return ((IEnumerable<PsychicRitualRoleDef>)roleAssignments.Keys).GroupBy((Func<PsychicRitualRoleDef, string>)((PsychicRitualRoleDef role) => role.Label));
	}

	public IEnumerable<Pawn> AssignedPawns(PsychicRitualRoleDef role)
	{
		IEnumerable<Pawn> enumerable = roleAssignments.TryGetValue(role);
		return enumerable ?? Enumerable.Empty<Pawn>();
	}

	public PsychicRitualRoleDef RoleForPawn(Pawn pawn, bool includeForced = true)
	{
		if (pawnAssignments.TryGetValue(pawn, out var value))
		{
			return value;
		}
		return null;
	}

	public Pawn FirstAssignedPawn(PsychicRitualRoleDef role)
	{
		if (role == null)
		{
			Log.Error("Tried to get first assigned pawn for null role.");
			return null;
		}
		if (!roleAssignments.TryGetValue(role, out var value))
		{
			return null;
		}
		if (value.Count == 0)
		{
			return null;
		}
		return value[0];
	}

	public bool TryUnassignAnyRole(Pawn pawn)
	{
		if (!pawnAssignments.TryGetValue(pawn, out var value))
		{
			return false;
		}
		PsychicRitualRoleDef psychicRitualRoleDef = forcedAssignments?.TryGetValue(pawn);
		if (value == psychicRitualRoleDef)
		{
			return false;
		}
		pawnAssignments.Remove(pawn);
		roleAssignments[value].Remove(pawn);
		return true;
	}

	public void RemoveParticipant(Pawn pawn)
	{
		TryUnassignAnyRole(pawn);
	}

	public void RemoveAllParticipantsAndTargets()
	{
		pawnAssignments.Clear();
		forcedAssignments?.Clear();
		roleAssignments.ClearValueLists();
		spectators.Clear();
		target = TargetInfo.Invalid;
	}

	public void TryAssignAnyUnassigned(List<Pawn> pawns)
	{
		foreach (Pawn pawn in pawns)
		{
			if (PawnParticipating(pawn))
			{
				continue;
			}
			PsychicRitualRoleDef psychicRitualRoleDef = forcedAssignments?.TryGetValue(pawn);
			if (psychicRitualRoleDef != null && TryAssign(pawn, psychicRitualRoleDef, out var reason))
			{
				break;
			}
			foreach (PsychicRitualRoleDef key in roleAssignments.Keys)
			{
				if (TryAssign(pawn, key, out reason))
				{
					break;
				}
			}
		}
	}

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
		PsychicRitualRoleDef psychicRitualRoleDef = forcedAssignments?.TryGetValue(pawn);
		if (psychicRitualRoleDef != null && role != psychicRitualRoleDef)
		{
			return false;
		}
		if (!roleAssignments.TryGetValue(role, out var value))
		{
			return false;
		}
		if (!role.PawnCanDo(context, pawn, target, out reason))
		{
			return false;
		}
		if (value.Count >= role.MaxCount)
		{
			return false;
		}
		TryUnassignAnyRole(pawn);
		if (insertBefore != null)
		{
			value.Insert(value.IndexOf(insertBefore), pawn);
		}
		else
		{
			value.Add(pawn);
		}
		pawnAssignments[pawn] = role;
		return true;
	}

	public void SetTarget(TargetInfo target)
	{
		this.target = target;
	}

	[IteratorStateMachine(typeof(_003CBlockingIssues_003Ed__39))]
	public IEnumerable<string> BlockingIssues()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CBlockingIssues_003Ed__39(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void ExposeData()
	{
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			if (roleAssignments != null)
			{
				foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> roleAssignment in roleAssignments)
				{
					roleAssignment.Value?.RemoveAll((Pawn pawn) => pawn?.Discarded ?? true);
				}
			}
			pawnAssignments?.RemoveAll((KeyValuePair<Pawn, PsychicRitualRoleDef> kvp) => kvp.Key == null || kvp.Key.Discarded);
			forcedAssignments?.RemoveAll((KeyValuePair<Pawn, PsychicRitualRoleDef> kvp) => kvp.Key == null || kvp.Key.Discarded);
		}
		Scribe_Collections.Look(ref roleAssignments, "roleAssignments", LookMode.Def, LookMode.Reference, ref roleAssignmentsKeys, ref roleAssignmentsValues, logNullErrors: true, saveDestroyedKeys: false, saveDestroyedValues: true);
		Scribe_Collections.Look(ref pawnAssignments, "pawnAssignments", LookMode.Reference, LookMode.Def, ref pawnAssignmentsKeys, ref pawnAssignmentsValues, logNullErrors: false, saveDestroyedKeys: true);
		Scribe_Collections.Look(ref forcedAssignments, "forcedAssignments", LookMode.Reference, LookMode.Def, ref forcedAssignmentKeys, ref forcedAssignmentValues, logNullErrors: false, saveDestroyedKeys: true);
		Scribe_Collections.Look(ref spectators, "spectators", true, LookMode.Reference);
		Scribe_TargetInfo.Look(ref target, saveDestroyedThings: true, "target");
		if (Scribe.mode != LoadSaveMode.PostLoadInit)
		{
			return;
		}
		if (roleAssignments != null)
		{
			foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> roleAssignment2 in roleAssignments)
			{
				roleAssignment2.Value?.RemoveAll((Pawn pawn) => pawn == null);
			}
		}
		pawnAssignments?.RemoveAll((KeyValuePair<Pawn, PsychicRitualRoleDef> kvp) => kvp.Key == null);
		forcedAssignments?.RemoveAll((KeyValuePair<Pawn, PsychicRitualRoleDef> kvp) => kvp.Key == null);
	}
}
