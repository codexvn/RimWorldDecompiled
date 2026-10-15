using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class FloatMenuOptionProvider_WorkGivers : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003CGetOptions_003Ed__13 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public FloatMenuOptionProvider_WorkGivers _003C_003E4__this;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

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
		public _003CGetOptions_003Ed__13(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				FloatMenuOptionProvider_WorkGivers floatMenuOptionProvider_WorkGivers = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					shouldProcessEquivalenceGroups = false;
					if (equivalenceGroupTempStorage == null || equivalenceGroupTempStorage.Length != DefDatabase<WorkGiverEquivalenceGroupDef>.DefCount)
					{
						equivalenceGroupTempStorage = new FloatMenuOption[DefDatabase<WorkGiverEquivalenceGroupDef>.DefCount];
					}
					tmpUsedLabels.Clear();
					_003C_003E7__wrap1 = floatMenuOptionProvider_WorkGivers.GetWorkGiversOptionsFor(context.FirstSelectedPawn, context.ClickedCell, context).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetOptions_003Ed__13 _003CGetOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptions_003Ed__ = this;
			}
			else
			{
				_003CGetOptions_003Ed__ = new _003CGetOptions_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetOptions_003Ed__.context = _003C_003E3__context;
			return _003CGetOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetOptionsFor_003Ed__14 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public FloatMenuOptionProvider_WorkGivers _003C_003E4__this;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		private Thing clickedThing;

		public Thing _003C_003E3__clickedThing;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

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
		public _003CGetOptionsFor_003Ed__14(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				FloatMenuOptionProvider_WorkGivers floatMenuOptionProvider_WorkGivers = _003C_003E4__this;
				int num2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					shouldProcessEquivalenceGroups = false;
					_003C_003E7__wrap1 = floatMenuOptionProvider_WorkGivers.GetWorkGiversOptionsFor(context.FirstSelectedPawn, clickedThing, context).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0094;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0094;
				case 2:
					{
						_003C_003E1__state = -1;
						equivalenceGroupTempStorage[_003Ci_003E5__3] = null;
						goto IL_011c;
					}
					IL_0094:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!shouldProcessEquivalenceGroups)
					{
						break;
					}
					_003Ci_003E5__3 = 0;
					goto IL_012e;
					IL_011c:
					num2 = _003Ci_003E5__3 + 1;
					_003Ci_003E5__3 = num2;
					goto IL_012e;
					IL_012e:
					if (_003Ci_003E5__3 >= equivalenceGroupTempStorage.Length)
					{
						break;
					}
					if (equivalenceGroupTempStorage[_003Ci_003E5__3] != null)
					{
						tmpUsedLabels.Add(equivalenceGroupTempStorage[_003Ci_003E5__3].Label);
						_003C_003E2__current = equivalenceGroupTempStorage[_003Ci_003E5__3];
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_011c;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetOptionsFor_003Ed__14 _003CGetOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__14(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetOptionsFor_003Ed__.clickedThing = _003C_003E3__clickedThing;
			_003CGetOptionsFor_003Ed__.context = _003C_003E3__context;
			return _003CGetOptionsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetWorkGiversOptionsFor_003Ed__16 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		public FloatMenuOptionProvider_WorkGivers _003C_003E4__this;

		private LocalTargetInfo target;

		public LocalTargetInfo _003C_003E3__target;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		private List<FloatMenuOption>.Enumerator _003C_003E7__wrap1;

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
		public _003CGetWorkGiversOptionsFor_003Ed__16(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<FloatMenuOption>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				FloatMenuOptionProvider_WorkGivers floatMenuOptionProvider_WorkGivers = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (pawn.thinker.TryGetMainTreeThinkNode<JobGiver_Work>() == null)
					{
						return false;
					}
					tmpFloatMenuOptions.Clear();
					foreach (WorkTypeDef item in DefDatabase<WorkTypeDef>.AllDefsListForReading)
					{
						foreach (WorkGiverDef item2 in item.workGiversByPriority)
						{
							try
							{
								FloatMenuOption workGiverOption = floatMenuOptionProvider_WorkGivers.GetWorkGiverOption(pawn, item2, target, context);
								if (workGiverOption != null)
								{
									tmpUsedLabels.Add(workGiverOption.Label);
									tmpFloatMenuOptions.Add(workGiverOption);
								}
							}
							catch (Exception ex)
							{
								Log.Error("Exception in WorkGiverOptionProvider_WorkGivers.GetWorkGiversOptionsFor for " + item2.defName + ": " + ex);
							}
						}
					}
					_003C_003E7__wrap1 = tmpFloatMenuOptions.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FloatMenuOption current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<FloatMenuOption>.Enumerator);
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetWorkGiversOptionsFor_003Ed__16 _003CGetWorkGiversOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetWorkGiversOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetWorkGiversOptionsFor_003Ed__ = new _003CGetWorkGiversOptionsFor_003Ed__16(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetWorkGiversOptionsFor_003Ed__.pawn = _003C_003E3__pawn;
			_003CGetWorkGiversOptionsFor_003Ed__.target = _003C_003E3__target;
			_003CGetWorkGiversOptionsFor_003Ed__.context = _003C_003E3__context;
			return _003CGetWorkGiversOptionsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private static bool shouldProcessEquivalenceGroups = false;

	private static FloatMenuOption[] equivalenceGroupTempStorage;

	private static HashSet<string> tmpUsedLabels = new HashSet<string>();

	private static List<FloatMenuOption> tmpFloatMenuOptions = new List<FloatMenuOption>();

	protected override bool Drafted => true;

	protected override bool Undrafted => true;

	protected override bool Multiselect => false;

	protected override bool MechanoidCanDo => false;

	protected override bool CanSelfTarget => true;

	[IteratorStateMachine(typeof(_003CGetOptions_003Ed__13))]
	public override IEnumerable<FloatMenuOption> GetOptions(FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptions_003Ed__13(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__context = context
		};
	}

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__14))]
	public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__14(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__clickedThing = clickedThing,
			_003C_003E3__context = context
		};
	}

	[IteratorStateMachine(typeof(_003CGetWorkGiversOptionsFor_003Ed__16))]
	private IEnumerable<FloatMenuOption> GetWorkGiversOptionsFor(Pawn pawn, LocalTargetInfo target, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetWorkGiversOptionsFor_003Ed__16(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pawn = pawn,
			_003C_003E3__target = target,
			_003C_003E3__context = context
		};
	}

	private FloatMenuOption GetWorkGiverOption(Pawn pawn, WorkGiverDef workGiver, LocalTargetInfo target, FloatMenuContext context)
	{
		if (pawn.Drafted && !workGiver.canBeDoneWhileDrafted)
		{
			return null;
		}
		if (!(workGiver.Worker is WorkGiver_Scanner workGiver_Scanner) || !workGiver_Scanner.def.directOrderable)
		{
			return null;
		}
		JobFailReason.Clear();
		if (target.HasThing)
		{
			if (ScannerShouldSkip(pawn, workGiver_Scanner, target.Thing))
			{
				return null;
			}
		}
		else if (!workGiver_Scanner.PotentialWorkCellsGlobal(pawn).Contains(target.Cell) || workGiver_Scanner.ShouldSkip(pawn, forced: true))
		{
			return null;
		}
		Action action = null;
		Job job = (target.HasThing ? (workGiver_Scanner.HasJobOnThing(pawn, target.Thing, forced: true) ? workGiver_Scanner.JobOnThing(pawn, target.Thing, forced: true) : null) : (workGiver_Scanner.HasJobOnCell(pawn, target.Cell, forced: true) ? workGiver_Scanner.JobOnCell(pawn, target.Cell, forced: true) : null));
		string text = ((!JobFailReason.CustomJobString.NullOrEmpty()) ? ((string)"CannotGenericWorkCustom".Translate(JobFailReason.CustomJobString).CapitalizeFirst()) : ((!target.HasThing) ? ((string)"CannotGenericWork".Translate(workGiver.verb, "AreaLower".Translate())) : ((string)"CannotGenericWork".Translate(workGiver.verb, target.Thing.Label))));
		if (job == null)
		{
			if (!JobFailReason.HaveReason)
			{
				return null;
			}
			text = text + ": " + JobFailReason.Reason.CapitalizeFirst();
		}
		else
		{
			WorkTypeDef workType = workGiver_Scanner.def.workType;
			PawnCapacityDef pawnCapacityDef = workGiver_Scanner.MissingRequiredCapacity(pawn);
			if (pawnCapacityDef != null)
			{
				text += ": " + "CannotMissingHealthActivities".Translate(pawnCapacityDef.label);
			}
			else if (pawn.WorkTagIsDisabled(workGiver_Scanner.def.workTags))
			{
				text += ": " + "CannotPrioritizeWorkGiverDisabled".Translate(workGiver_Scanner.def.label);
			}
			else if (pawn.jobs.curJob != null && pawn.jobs.curJob.JobIsSameAs(pawn, job))
			{
				text = ((!target.HasThing) ? ((string)"CannotGenericAlreadyAmCustom".Translate(workGiver_Scanner.PostProcessedGerund(job))) : ((string)"CannotGenericAlreadyAm".Translate(workGiver_Scanner.PostProcessedGerund(job), target.Thing.LabelShort, target.Thing)));
			}
			else if (pawn.workSettings.GetPriority(workType) == 0)
			{
				text = (pawn.WorkTypeIsDisabled(workType) ? ((string)(text + (": " + "CannotPrioritizeWorkTypeDisabled".Translate(workType.gerundLabel)))) : ((!"CannotPrioritizeNotAssignedToWorkType".CanTranslate()) ? ((string)(text + (": " + "CannotPrioritizeWorkTypeDisabled".Translate(workType.pawnLabel)))) : ((string)(text + (": " + "CannotPrioritizeNotAssignedToWorkType".Translate(workType.gerundLabel))))));
			}
			else if (job.def == JobDefOf.Research && target.Thing is Building_ResearchBench)
			{
				text += ": " + "CannotPrioritizeResearch".Translate();
			}
			else if (target.HasThing && target.Thing.IsForbidden(pawn))
			{
				text = (target.Thing.Position.InAllowedArea(pawn) ? ((string)(text + (": " + "CannotPrioritizeForbidden".Translate(target.Thing.Label, target.Thing)))) : ((string)(text + (": " + "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + (" (" + pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label + ")")))));
			}
			else if (!target.HasThing && target.Cell.IsForbidden(pawn))
			{
				text = (target.Cell.InAllowedArea(pawn) ? ((string)(text + (": " + "CannotPrioritizeCellForbidden".Translate()))) : ((string)(text + (": " + "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + (" (" + pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label + ")")))));
			}
			else if ((target.HasThing && !pawn.CanReach(target.Thing, workGiver_Scanner.PathEndMode, Danger.Deadly)) || (!target.HasThing && !pawn.CanReach(target.Cell, PathEndMode.Touch, Danger.Deadly)))
			{
				text += ": " + "NoPath".Translate().CapitalizeFirst();
			}
			else
			{
				text = (target.HasThing ? ((string)"PrioritizeGeneric".Translate(workGiver_Scanner.PostProcessedGerund(job), target.Thing.Label).CapitalizeFirst()) : ((!job.def.displayAsAreaInFloatMenu) ? ((string)"PrioritizeGenericSimple".Translate(workGiver_Scanner.PostProcessedGerund(job)).CapitalizeFirst()) : ((string)"PrioritizeGeneric".Translate(workGiver_Scanner.PostProcessedGerund(job), "AreaLower".Translate()).CapitalizeFirst())));
				Job localJob = job;
				WorkGiver_Scanner localScanner = workGiver_Scanner;
				job.workGiverDef = workGiver_Scanner.def;
				WorkGiverDef giver = workGiver;
				action = delegate
				{
					if (pawn.jobs.TryTakeOrderedJobPrioritizedWork(localJob, localScanner, context.ClickedCell))
					{
						if (giver.forceMote != null)
						{
							MoteMaker.MakeStaticMote(context.ClickedCell, pawn.Map, giver.forceMote);
						}
						if (giver.forceFleck != null)
						{
							FleckMaker.Static(context.ClickedCell, pawn.Map, giver.forceFleck);
						}
					}
				};
			}
		}
		if (DebugViewSettings.showFloatMenuWorkGivers)
		{
			text += $" (from {workGiver.defName})";
		}
		FloatMenuOption menuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, action), pawn, target);
		if (tmpUsedLabels.Any((string used) => used == menuOption.Label))
		{
			return null;
		}
		if (pawn.Drafted && workGiver.autoTakeablePriorityDrafted != -1)
		{
			menuOption.autoTakeable = true;
			menuOption.autoTakeablePriority = workGiver.autoTakeablePriorityDrafted;
		}
		if (target.HasThing && workGiver.equivalenceGroup != null)
		{
			if (equivalenceGroupTempStorage[workGiver.equivalenceGroup.index] == null || (equivalenceGroupTempStorage[workGiver.equivalenceGroup.index].Disabled && !menuOption.Disabled))
			{
				equivalenceGroupTempStorage[workGiver.equivalenceGroup.index] = menuOption;
				shouldProcessEquivalenceGroups = true;
			}
			return null;
		}
		return menuOption;
	}

	private static bool ScannerShouldSkip(Pawn pawn, WorkGiver_Scanner scanner, Thing t)
	{
		if (scanner.PotentialWorkThingRequest.Accepts(t) || (scanner.PotentialWorkThingsGlobal(pawn) != null && scanner.PotentialWorkThingsGlobal(pawn).Contains(t)))
		{
			return scanner.ShouldSkip(pawn, forced: true);
		}
		return true;
	}
}
