using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Precept_RoleMulti : Precept_Role
{
	[CompilerGenerated]
	private sealed class _003CChosenPawns_003Ed__2 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Precept_RoleMulti _003C_003E4__this;

		private List<IdeoRoleInstance>.Enumerator _003C_003E7__wrap1;

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
		public _003CChosenPawns_003Ed__2(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<IdeoRoleInstance>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Precept_RoleMulti precept_RoleMulti = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = precept_RoleMulti.chosenPawns.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					IdeoRoleInstance current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current.pawn;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<IdeoRoleInstance>.Enumerator);
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
			_003CChosenPawns_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CChosenPawns_003Ed__2(0)
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

	public List<IdeoRoleInstance> chosenPawns = new List<IdeoRoleInstance>();

	private List<IdeoRoleInstance> chosenPawnsCache = new List<IdeoRoleInstance>();

	private List<IdeoRoleInstance> pawnsToRemove = new List<IdeoRoleInstance>();

	[IteratorStateMachine(typeof(_003CChosenPawns_003Ed__2))]
	public override IEnumerable<Pawn> ChosenPawns()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CChosenPawns_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override Pawn ChosenPawnSingle()
	{
		return null;
	}

	public override void Init(Ideo ideo, FactionDef generatingFor = null)
	{
		base.Init(ideo, generatingFor);
		active = true;
	}

	public override void Assign(Pawn p, bool addThoughts)
	{
		if (!IsAssigned(p))
		{
			IdeoRoleInstance ideoRoleInstance = chosenPawnsCache.FirstOrDefault((IdeoRoleInstance c) => c.pawn == p);
			if (ideoRoleInstance != null)
			{
				chosenPawnsCache.Remove(ideoRoleInstance);
				chosenPawns.Add(ideoRoleInstance);
			}
			else
			{
				chosenPawns.Add(new IdeoRoleInstance(this)
				{
					pawn = p
				});
				FillOrUpdateAbilities();
			}
			Notify_PawnAssigned(p);
		}
	}

	public override void FillOrUpdateAbilities()
	{
		foreach (IdeoRoleInstance chosenPawn in chosenPawns)
		{
			chosenPawn.abilities = FillOrUpdateAbilityList(chosenPawn.pawn, chosenPawn.abilities);
		}
	}

	public override List<Ability> AbilitiesFor(Pawn p)
	{
		for (int i = 0; i < chosenPawns.Count; i++)
		{
			if (chosenPawns[i].pawn == p)
			{
				return chosenPawns[i].abilities;
			}
		}
		return null;
	}

	public override bool IsAssigned(Pawn p)
	{
		for (int i = 0; i < chosenPawns.Count; i++)
		{
			if (chosenPawns[i].pawn == p)
			{
				return true;
			}
		}
		return false;
	}

	public override void Unassign(Pawn p, bool generateThoughts)
	{
		if (IsAssigned(p))
		{
			IdeoRoleInstance ideoRoleInstance = chosenPawns.FirstOrDefault((IdeoRoleInstance c) => c.pawn == p);
			if (ideoRoleInstance != null)
			{
				chosenPawns.Remove(ideoRoleInstance);
				chosenPawnsCache.Add(ideoRoleInstance);
				p.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleLost, this));
				Notify_PawnUnassigned(p);
			}
		}
	}

	public override void RecacheActivity()
	{
		pawnsToRemove.Clear();
		foreach (IdeoRoleInstance chosenPawn in chosenPawns)
		{
			if (chosenPawn.pawn == null || !ValidatePawn(chosenPawn.pawn))
			{
				pawnsToRemove.Add(chosenPawn);
			}
		}
		foreach (IdeoRoleInstance item in pawnsToRemove)
		{
			Unassign(item.pawn, generateThoughts: false);
			if (chosenPawns.Contains(item))
			{
				chosenPawns.Remove(item);
			}
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		if (!GameDataSaveLoader.IsSavingOrLoadingExternalIdeo)
		{
			Scribe_Collections.Look(ref chosenPawns, "chosenPawns", LookMode.Deep, this);
			Scribe_Collections.Look(ref chosenPawnsCache, "chosenPawnsCache", LookMode.Deep, this);
		}
		if (Scribe.mode != LoadSaveMode.PostLoadInit)
		{
			return;
		}
		chosenPawns.RemoveAll((IdeoRoleInstance c) => c.pawn == null || !ValidatePawn(c.pawn));
		chosenPawnsCache.RemoveAll((IdeoRoleInstance c) => c.pawn == null);
		foreach (IdeoRoleInstance chosenPawn in chosenPawns)
		{
			chosenPawn.sourceRole = this;
		}
	}

	public override void CopyTo(Precept precept)
	{
		base.CopyTo(precept);
		Precept_RoleMulti obj = (Precept_RoleMulti)precept;
		obj.chosenPawns.Clear();
		obj.chosenPawns.AddRange(chosenPawns);
		obj.chosenPawnsCache.Clear();
		obj.chosenPawnsCache.AddRange(chosenPawnsCache);
	}
}
