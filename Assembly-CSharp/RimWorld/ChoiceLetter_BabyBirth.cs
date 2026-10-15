using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ChoiceLetter_BabyBirth : ChoiceLetter
{
	[CompilerGenerated]
	private sealed class _003Cget_Choices_003Ed__6 : IEnumerable<DiaOption>, IEnumerable, IEnumerator<DiaOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DiaOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ChoiceLetter_BabyBirth _003C_003E4__this;

		DiaOption IEnumerator<DiaOption>.Current
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
		public _003Cget_Choices_003Ed__6(int _003C_003E1__state)
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
			ChoiceLetter_BabyBirth choiceLetter_BabyBirth = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!choiceLetter_BabyBirth.ArchivedOnly)
				{
					Faction faction = choiceLetter_BabyBirth.pawn.Faction;
					if (faction != null && faction.IsPlayer)
					{
						_003C_003E2__current = new DiaOption("NameBaby".Translate().CapitalizeFirst())
						{
							action = choiceLetter_BabyBirth.Rename,
							disabled = (choiceLetter_BabyBirth.pawn.babyNamingDeadline < Find.TickManager.TicksGame),
							disabledReason = "BabyAlreadyNamed".Translate(choiceLetter_BabyBirth.pawn),
							resolveTree = true
						};
						_003C_003E1__state = 1;
						return true;
					}
				}
				goto IL_00f0;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00f0;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0114;
			case 3:
				_003C_003E1__state = -1;
				break;
			case 4:
				_003C_003E1__state = -1;
				break;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = choiceLetter_BabyBirth.Option_Close;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				break;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = choiceLetter_BabyBirth.Option_Postpone;
				_003C_003E1__state = 8;
				return true;
			case 8:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00f0:
				if (choiceLetter_BabyBirth.ArchivedOnly)
				{
					_003C_003E2__current = choiceLetter_BabyBirth.Option_JumpToLocation;
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0114;
				IL_0114:
				if (choiceLetter_BabyBirth.ArchivedOnly)
				{
					_003C_003E2__current = choiceLetter_BabyBirth.Option_Close;
					_003C_003E1__state = 3;
					return true;
				}
				if (choiceLetter_BabyBirth.LastTickBeforeTimeout)
				{
					if (choiceLetter_BabyBirth.pawn.babyNamingDeadline < 0 || choiceLetter_BabyBirth.pawn.Dead)
					{
						_003C_003E2__current = choiceLetter_BabyBirth.Option_Close;
						_003C_003E1__state = 4;
						return true;
					}
					break;
				}
				if (choiceLetter_BabyBirth.pawn.babyNamingDeadline < Find.TickManager.TicksGame)
				{
					_003C_003E2__current = choiceLetter_BabyBirth.Option_JumpToLocationAndPostpone;
					_003C_003E1__state = 5;
					return true;
				}
				_003C_003E2__current = choiceLetter_BabyBirth.Option_JumpToLocationAndPostpone;
				_003C_003E1__state = 7;
				return true;
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
		IEnumerator<DiaOption> IEnumerable<DiaOption>.GetEnumerator()
		{
			_003Cget_Choices_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Choices_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<DiaOption>)this).GetEnumerator();
		}
	}

	private Pawn pawn;

	public override bool CanShowInLetterStack
	{
		get
		{
			Pawn obj = pawn;
			if (obj != null && obj.Faction?.IsPlayer == true)
			{
				Pawn obj2 = pawn;
				if (obj2 != null && obj2.babyNamingDeadline >= 0)
				{
					return base.CanShowInLetterStack;
				}
			}
			return false;
		}
	}

	public override bool ShouldAutomaticallyOpenLetter
	{
		get
		{
			Pawn obj = pawn;
			if (obj != null && obj.babyNamingDeadline >= 0)
			{
				return base.ShouldAutomaticallyOpenLetter;
			}
			return false;
		}
	}

	public override IEnumerable<DiaOption> Choices
	{
		[IteratorStateMachine(typeof(_003Cget_Choices_003Ed__6))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Choices_003Ed__6(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public void Start()
	{
		pawn = lookTargets.TryGetPrimaryTarget().Thing as Pawn;
		if (pawn.babyNamingDeadline >= Find.TickManager.TicksGame)
		{
			StartTimeout(pawn.babyNamingDeadline - Find.TickManager.TicksGame);
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref pawn, "pawn", saveDestroyedThings: true);
		if (Scribe.mode != LoadSaveMode.Saving && pawn == null)
		{
			pawn = lookTargets.TryGetPrimaryTarget().Thing as Pawn;
		}
	}

	private void Rename()
	{
		string initialFirstNameOverride = null;
		if (pawn.Name is NameTriple nameTriple && nameTriple.First == "Baby".Translate().CapitalizeFirst())
		{
			Rand.PushState();
			Name name;
			try
			{
				Rand.Seed = pawn.thingIDNumber;
				name = PawnBioAndNameGenerator.GeneratePawnName(pawn, NameStyle.Full, null, forceNoNick: false, pawn.genes?.Xenotype);
			}
			finally
			{
				Rand.PopState();
			}
			initialFirstNameOverride = ((name is NameTriple nameTriple2) ? nameTriple2.First : ((NameSingle)name).Name);
		}
		Find.WindowStack.Add(pawn.NamePawnDialog(initialFirstNameOverride));
	}
}
