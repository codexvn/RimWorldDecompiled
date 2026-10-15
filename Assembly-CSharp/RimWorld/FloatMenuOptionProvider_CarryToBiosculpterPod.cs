using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class FloatMenuOptionProvider_CarryToBiosculpterPod : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public CompBiosculpterPod podComp;

		public Pawn clickedPawn;

		public FloatMenuContext context;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_1
	{
		public CompBiosculpterPod_Cycle cycle;

		public _003C_003Ec__DisplayClass9_0 CS_0024_003C_003E8__locals1;

		internal void _003CGetOptionsFor_003Eb__0()
		{
			if (!CS_0024_003C_003E8__locals1.podComp.CanAcceptOnceCycleChosen(CS_0024_003C_003E8__locals1.clickedPawn))
			{
				Messages.Message("CannotCarryToBiosculpterPod".Translate() + ": " + "NoBiosculpterPod".Translate(), CS_0024_003C_003E8__locals1.clickedPawn, MessageTypeDefOf.RejectInput, historical: false);
			}
			else
			{
				CS_0024_003C_003E8__locals1.podComp.PrepareCycleJob(CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, CS_0024_003C_003E8__locals1.clickedPawn, cycle, CS_0024_003C_003E8__locals1.podComp.MakeCarryToBiosculpterJob(CS_0024_003C_003E8__locals1.clickedPawn));
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetOptionsFor_003Ed__9 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn clickedPawn;

		public Pawn _003C_003E3__clickedPawn;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		private _003C_003Ec__DisplayClass9_0 _003C_003E8__1;

		private List<CompBiosculpterPod_Cycle>.Enumerator _003C_003E7__wrap1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetOptionsFor_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 1) <= 3u)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = default(List<CompBiosculpterPod_Cycle>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			bool result;
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					result = false;
					goto end_IL_0000;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass9_0();
					_003C_003E8__1.clickedPawn = clickedPawn;
					_003C_003E8__1.context = context;
					if (!_003C_003E8__1.clickedPawn.IsColonistPlayerControlled && !_003C_003E8__1.clickedPawn.IsPrisonerOfColony)
					{
						result = false;
					}
					else if (!_003C_003E8__1.clickedPawn.IsPrisoner && !_003C_003E8__1.clickedPawn.Downed)
					{
						result = false;
					}
					else if (_003C_003E8__1.clickedPawn.IsPrisoner && PrisonBreakUtility.IsPrisonBreaking(_003C_003E8__1.clickedPawn))
					{
						result = false;
					}
					else if (!_003C_003E8__1.context.FirstSelectedPawn.CanReserveAndReach(_003C_003E8__1.clickedPawn, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, ignoreOtherReservations: true))
					{
						result = false;
					}
					else
					{
						Thing thing = CompBiosculpterPod.FindPodFor(_003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedPawn, biotuned: true) ?? CompBiosculpterPod.FindPodFor(_003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedPawn, biotuned: false);
						if (thing != null && thing.TryGetComp(out _003C_003E8__1.podComp))
						{
							_003C_003E7__wrap1 = _003C_003E8__1.podComp.AvailableCycles.GetEnumerator();
							_003C_003E1__state = -3;
							goto IL_048d;
						}
						result = false;
					}
					goto end_IL_0000;
				case 1:
					_003C_003E1__state = -3;
					result = false;
					break;
				case 2:
					_003C_003E1__state = -3;
					result = false;
					break;
				case 3:
					_003C_003E1__state = -3;
					result = false;
					break;
				case 4:
					{
						_003C_003E1__state = -3;
						goto IL_048d;
					}
					IL_048d:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003C_003Ec__DisplayClass9_1 CS_0024_003C_003E8__locals32 = new _003C_003Ec__DisplayClass9_1
						{
							CS_0024_003C_003E8__locals1 = _003C_003E8__1,
							cycle = _003C_003E7__wrap1.Current
						};
						TaggedString taggedString = "CarryToBiosculpterPod".Translate(CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn.Named("PAWN"), CS_0024_003C_003E8__locals32.cycle.Props.label.Named("CYCLE"));
						if (CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn.IsQuestLodger())
						{
							_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString + " (" + "CryptosleepCasketGuestsNotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn), CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn);
							_003C_003E1__state = 1;
							result = true;
						}
						else if (CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn.GetExtraHostFaction() != null)
						{
							_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString + " (" + "CryptosleepCasketGuestPrisonersNotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn), CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn);
							_003C_003E1__state = 2;
							result = true;
						}
						else
						{
							string text = CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.podComp.CannotUseNowCycleReason(CS_0024_003C_003E8__locals32.cycle) ?? CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.podComp.CannotUseNowPawnCycleReason(CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn, CS_0024_003C_003E8__locals32.cycle);
							if (text != null)
							{
								_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(CompBiosculpterPod.CannotStartText(CS_0024_003C_003E8__locals32.cycle, text), null, MenuOptionPriority.Default, null, CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn), CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn);
								_003C_003E1__state = 3;
								result = true;
							}
							else
							{
								Action action = delegate
								{
									if (!CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.podComp.CanAcceptOnceCycleChosen(CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn))
									{
										Messages.Message("CannotCarryToBiosculpterPod".Translate() + ": " + "NoBiosculpterPod".Translate(), CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn, MessageTypeDefOf.RejectInput, historical: false);
									}
									else
									{
										CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.podComp.PrepareCycleJob(CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn, CS_0024_003C_003E8__locals32.cycle, CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.podComp.MakeCarryToBiosculpterJob(CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn));
									}
								};
								_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, action, MenuOptionPriority.Default, null, CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn), CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, CS_0024_003C_003E8__locals32.CS_0024_003C_003E8__locals1.clickedPawn);
								_003C_003E1__state = 4;
								result = true;
							}
						}
					}
					else
					{
						_003C_003Em__Finally1();
						_003C_003E7__wrap1 = default(List<CompBiosculpterPod_Cycle>.Enumerator);
						result = false;
					}
					goto end_IL_0000;
				}
				_003C_003Em__Finally1();
				end_IL_0000:;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			return result;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetOptionsFor_003Ed__9 _003CGetOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__9(0);
			}
			_003CGetOptionsFor_003Ed__.clickedPawn = _003C_003E3__clickedPawn;
			_003CGetOptionsFor_003Ed__.context = _003C_003E3__context;
			return _003CGetOptionsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	protected override bool Drafted => true;

	protected override bool Undrafted => true;

	protected override bool Multiselect => false;

	protected override bool RequiresManipulation => true;

	protected override bool AppliesInt(FloatMenuContext context)
	{
		return ModsConfig.IdeologyActive;
	}

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__9))]
	public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__9(-2)
		{
			_003C_003E3__clickedPawn = clickedPawn,
			_003C_003E3__context = context
		};
	}
}
