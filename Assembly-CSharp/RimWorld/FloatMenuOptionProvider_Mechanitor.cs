using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class FloatMenuOptionProvider_Mechanitor : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public Pawn clickedPawn;

		public FloatMenuContext context;

		public Action _003C_003E9__5;

		internal void _003CGetOptionsFor_003Eb__0()
		{
			Job job = JobMaker.MakeJob(JobDefOf.ControlMech, clickedPawn);
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		internal void _003CGetOptionsFor_003Eb__1()
		{
			MechanitorUtility.ForceDisconnectMechFromOverseer(clickedPawn);
		}

		internal void _003CGetOptionsFor_003Eb__2()
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDisassemblingMech".Translate(clickedPawn.LabelCap) + ":\n" + (from x in MechanitorUtility.IngredientsFromDisassembly(clickedPawn.def)
				select x.Summary).ToLineList("  - "), delegate
			{
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.DisassembleMech, clickedPawn), JobTag.Misc);
			}, destructive: true));
		}

		internal void _003CGetOptionsFor_003Eb__5()
		{
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.DisassembleMech, clickedPawn), JobTag.Misc);
		}

		internal void _003CGetOptionsFor_003Eb__3()
		{
			Job job = JobMaker.MakeJob(JobDefOf.RepairMech, clickedPawn);
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
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
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass9_0();
				_003C_003E8__1.clickedPawn = clickedPawn;
				_003C_003E8__1.context = context;
				if (!_003C_003E8__1.clickedPawn.IsColonyMech)
				{
					return false;
				}
				if (_003C_003E8__1.clickedPawn.GetOverseer() != _003C_003E8__1.context.FirstSelectedPawn)
				{
					if (!_003C_003E8__1.context.FirstSelectedPawn.CanReach(_003C_003E8__1.clickedPawn, PathEndMode.Touch, Danger.Deadly))
					{
						_003C_003E2__current = new FloatMenuOption("CannotControlMech".Translate(_003C_003E8__1.clickedPawn.LabelShort) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
						_003C_003E1__state = 1;
						return true;
					}
					if (!MechanitorUtility.CanControlMech(_003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedPawn))
					{
						AcceptanceReport acceptanceReport = MechanitorUtility.CanControlMech(_003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedPawn);
						if (!acceptanceReport.Reason.NullOrEmpty())
						{
							_003C_003E2__current = new FloatMenuOption("CannotControlMech".Translate(_003C_003E8__1.clickedPawn.LabelShort) + ": " + acceptanceReport.Reason, null);
							_003C_003E1__state = 2;
							return true;
						}
						goto IL_0288;
					}
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ControlMech".Translate(_003C_003E8__1.clickedPawn.LabelShort), delegate
					{
						Job job2 = JobMaker.MakeJob(JobDefOf.ControlMech, _003C_003E8__1.clickedPawn);
						_003C_003E8__1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job2, JobTag.Misc);
					}), _003C_003E8__1.context.FirstSelectedPawn, new LocalTargetInfo(_003C_003E8__1.clickedPawn));
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("DisconnectMech".Translate(_003C_003E8__1.clickedPawn.LabelShort), delegate
				{
					MechanitorUtility.ForceDisconnectMechFromOverseer(_003C_003E8__1.clickedPawn);
				}, MenuOptionPriority.Low, null, null, 0f, null, null, playSelectionSound: true, -10), _003C_003E8__1.context.FirstSelectedPawn, new LocalTargetInfo(_003C_003E8__1.clickedPawn));
				_003C_003E1__state = 5;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0288;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0288;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0288;
			case 4:
				_003C_003E1__state = -1;
				goto IL_041f;
			case 5:
				_003C_003E1__state = -1;
				if (!_003C_003E8__1.clickedPawn.IsFighting())
				{
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("DisassembleMech".Translate(_003C_003E8__1.clickedPawn.LabelCap), delegate
					{
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmDisassemblingMech".Translate(_003C_003E8__1.clickedPawn.LabelCap) + ":\n" + (from x in MechanitorUtility.IngredientsFromDisassembly(_003C_003E8__1.clickedPawn.def)
							select x.Summary).ToLineList("  - "), delegate
						{
							_003C_003E8__1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.DisassembleMech, _003C_003E8__1.clickedPawn), JobTag.Misc);
						}, destructive: true));
					}, MenuOptionPriority.Low, null, null, 0f, null, null, playSelectionSound: true, -20), _003C_003E8__1.context.FirstSelectedPawn, new LocalTargetInfo(_003C_003E8__1.clickedPawn));
					_003C_003E1__state = 6;
					return true;
				}
				goto IL_041f;
			case 6:
				_003C_003E1__state = -1;
				goto IL_041f;
			case 7:
				_003C_003E1__state = -1;
				break;
			case 8:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_041f:
				if (_003C_003E8__1.context.FirstSelectedPawn.Drafted && MechRepairUtility.CanRepair(_003C_003E8__1.clickedPawn))
				{
					if (!_003C_003E8__1.context.FirstSelectedPawn.CanReach(_003C_003E8__1.clickedPawn, PathEndMode.Touch, Danger.Deadly))
					{
						_003C_003E2__current = new FloatMenuOption("CannotRepairMech".Translate(_003C_003E8__1.clickedPawn.LabelShort) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
						_003C_003E1__state = 7;
						return true;
					}
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("RepairThing".Translate(_003C_003E8__1.clickedPawn.LabelShort), delegate
					{
						Job job = JobMaker.MakeJob(JobDefOf.RepairMech, _003C_003E8__1.clickedPawn);
						_003C_003E8__1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					}), _003C_003E8__1.context.FirstSelectedPawn, new LocalTargetInfo(_003C_003E8__1.clickedPawn));
					_003C_003E1__state = 8;
					return true;
				}
				break;
				IL_0288:
				_003C_003E2__current = new FloatMenuOption("CannotDisassembleMech".Translate(_003C_003E8__1.clickedPawn.LabelCap) + ": " + "MustBeOverseer".Translate().CapitalizeFirst(), null);
				_003C_003E1__state = 4;
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
		if (!ModsConfig.BiotechActive)
		{
			return false;
		}
		return MechanitorUtility.IsMechanitor(context.FirstSelectedPawn);
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
