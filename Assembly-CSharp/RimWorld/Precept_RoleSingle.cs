using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Precept_RoleSingle : Precept_Role
{
	[CompilerGenerated]
	private sealed class _003CChosenPawns_003Ed__7 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Precept_RoleSingle _003C_003E4__this;

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
		public _003CChosenPawns_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Precept_RoleSingle precept_RoleSingle = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (precept_RoleSingle.ChosenPawnValue != null)
				{
					_003C_003E2__current = precept_RoleSingle.chosenPawn.pawn;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003CChosenPawns_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CChosenPawns_003Ed__7(0)
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

	public IdeoRoleInstance chosenPawn;

	public Pawn ChosenPawnValue => chosenPawn?.pawn;

	public List<Ability> AbilitiesForReading => chosenPawn?.abilities;

	public override void Init(Ideo ideo, FactionDef generatingFor = null)
	{
		chosenPawn = new IdeoRoleInstance(this);
		base.Init(ideo, generatingFor);
	}

	public override void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
	{
		base.Notify_MemberChangedFaction(p, oldFaction, newFaction);
		if (p == ChosenPawnValue && oldFaction.IsPlayer)
		{
			Assign(null, addThoughts: false);
		}
	}

	[IteratorStateMachine(typeof(_003CChosenPawns_003Ed__7))]
	public override IEnumerable<Pawn> ChosenPawns()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CChosenPawns_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void RecacheActivity()
	{
		int colonistBelieverCountCached = ideo.ColonistBelieverCountCached;
		bool flag = Faction.OfPlayer.ideos.Has(ideo);
		if (active && def.deactivationBelieverCount >= 0 && colonistBelieverCountCached <= def.deactivationBelieverCount && !def.leaderRole)
		{
			active = false;
			if (flag)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelRoleInactive".Translate(name).CapitalizeFirst(), "LetterLabelRoleInactiveDesc".Translate(ideo.memberName, def.deactivationBelieverCount, def.activationBelieverCount, this.Named("ROLE")).CapitalizeFirst(), LetterDefOf.NeutralEvent);
			}
			if (ChosenPawnValue != null)
			{
				if (flag)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRoleLost".Translate(ChosenPawnValue.Named("PAWN"), this.Named("ROLE")), "LetterRoleLostDesc".Translate(ChosenPawnValue.Named("PAWN"), this.Named("ROLE")) + " " + "LetterRoleLostReasonLowBelieversDesc".Translate(ideo.memberName).CapitalizeFirst(), LetterDefOf.NeutralEvent, ChosenPawnValue);
				}
				Notify_PawnUnassigned(ChosenPawnValue);
				chosenPawn.pawn = null;
			}
		}
		if (!active && def.activationBelieverCount >= 0 && (colonistBelieverCountCached >= def.activationBelieverCount || def.leaderRole))
		{
			active = true;
			if (flag && !def.leaderRole && def.activationBelieverCount > 0 && Find.TickManager.TicksGame > 1)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelRoleActive".Translate(name).CapitalizeFirst(), "LetterLabelRoleActiveDesc".Translate(ideo.memberName, def.activationBelieverCount, this.Named("ROLE")).CapitalizeFirst(), LetterDefOf.NeutralEvent);
			}
		}
		if (ChosenPawnValue != null && !ValidatePawn(ChosenPawnValue))
		{
			Notify_PawnUnassigned(ChosenPawnValue);
			chosenPawn.pawn = null;
		}
	}

	public override void Assign(Pawn p, bool addThoughts)
	{
		if (p == ChosenPawnValue)
		{
			return;
		}
		if (p != null && !ValidatePawn(p))
		{
			Log.Error("Invalid pawn assigned to " + base.LabelCap + " role. pawn=" + p.GetUniqueLoadID());
		}
		if (ChosenPawnValue != null && addThoughts)
		{
			if (p != null)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelRoleLost".Translate(ChosenPawnValue.Named("PAWN"), this.Named("ROLE")), "LetterRoleLostDesc".Translate(ChosenPawnValue.Named("PAWN"), this.Named("ROLE")) + " " + "LetterRoleLostReasonUnassignedDesc".Translate(ChosenPawnValue.Named("PAWN")).CapitalizeFirst(), LetterDefOf.NeutralEvent, ChosenPawnValue);
			}
			ChosenPawnValue.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.IdeoRoleLost, this));
		}
		Pawn chosenPawnValue = ChosenPawnValue;
		chosenPawn.pawn = p;
		Notify_PawnUnassigned(chosenPawnValue);
		Notify_PawnAssigned(p);
		if (!def.leaderRole || Current.ProgramState != ProgramState.Playing)
		{
			return;
		}
		Faction ofPlayer = Faction.OfPlayer;
		if (ofPlayer == null)
		{
			return;
		}
		ofPlayer.leader = p;
		foreach (Ideo allIdeo in ofPlayer.ideos.AllIdeos)
		{
			foreach (Precept item in allIdeo.PreceptsListForReading)
			{
				if (item != this && item is Precept_Role precept_Role && precept_Role.def.leaderRole)
				{
					precept_Role.Assign(null, addThoughts: true);
				}
			}
		}
	}

	public override void FillOrUpdateAbilities()
	{
		if (!def.grantedAbilities.NullOrEmpty())
		{
			chosenPawn.abilities = FillOrUpdateAbilityList(ChosenPawnValue, chosenPawn?.abilities);
		}
	}

	public override List<Ability> AbilitiesFor(Pawn p)
	{
		return chosenPawn.abilities;
	}

	public override Pawn ChosenPawnSingle()
	{
		return chosenPawn?.pawn;
	}

	public override bool IsAssigned(Pawn p)
	{
		return chosenPawn?.pawn == p;
	}

	public override void Unassign(Pawn p, bool generateThoughts)
	{
		Assign(null, generateThoughts);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref chosenPawn, "chosenPawn", this);
		if (GameDataSaveLoader.IsSavingOrLoadingExternalIdeo && Scribe.mode == LoadSaveMode.LoadingVars)
		{
			chosenPawn = new IdeoRoleInstance(this);
		}
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			if (ChosenPawnValue != null && !ValidatePawn(ChosenPawnValue))
			{
				Pawn pawn = chosenPawn.pawn;
				chosenPawn.pawn = null;
				Notify_PawnUnassigned(pawn);
			}
			chosenPawn.sourceRole = this;
			FillOrUpdateAbilities();
		}
	}

	public override void CopyTo(Precept precept)
	{
		base.CopyTo(precept);
		((Precept_RoleSingle)precept).chosenPawn = chosenPawn;
	}
}
