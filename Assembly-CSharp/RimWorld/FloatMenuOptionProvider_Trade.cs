using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class FloatMenuOptionProvider_Trade : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public Pawn clickedPawn;

		public FloatMenuContext context;

		internal void _003CGetOptionsFor_003Eb__0()
		{
			Job job = JobMaker.MakeJob(JobDefOf.TradeWithPawn, clickedPawn);
			job.playerForced = true;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InteractingWithTraders, KnowledgeAmount.Total);
		}

		internal void _003CGetOptionsFor_003Eb__1()
		{
			Job job = JobMaker.MakeJob(JobDefOf.DismissTrader, clickedPawn);
			job.playerForced = true;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetOptionsFor_003Ed__6 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn clickedPawn;

		public Pawn _003C_003E3__clickedPawn;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		private _003C_003Ec__DisplayClass6_0 _003C_003E8__1;

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
		public _003CGetOptionsFor_003Ed__6(int _003C_003E1__state)
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
			{
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass6_0();
				_003C_003E8__1.clickedPawn = clickedPawn;
				_003C_003E8__1.context = context;
				if (_003C_003E8__1.clickedPawn == null || !((ITrader)_003C_003E8__1.clickedPawn).CanTradeNow)
				{
					return false;
				}
				if (!_003C_003E8__1.context.FirstSelectedPawn.CanReach(_003C_003E8__1.clickedPawn, PathEndMode.OnCell, Danger.Deadly))
				{
					_003C_003E2__current = new FloatMenuOption("CannotTrade".Translate() + ": " + "NoPath".Translate().CapitalizeFirst(), null);
					_003C_003E1__state = 1;
					return true;
				}
				if (_003C_003E8__1.context.FirstSelectedPawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
				{
					_003C_003E2__current = new FloatMenuOption("CannotPrioritizeWorkTypeDisabled".Translate(SkillDefOf.Social.LabelCap), null);
					_003C_003E1__state = 2;
					return true;
				}
				if (_003C_003E8__1.clickedPawn.mindState.traderDismissed)
				{
					_003C_003E2__current = new FloatMenuOption("TraderDismissed".Translate(), null);
					_003C_003E1__state = 3;
					return true;
				}
				if (!_003C_003E8__1.context.FirstSelectedPawn.CanTradeWith(_003C_003E8__1.clickedPawn.Faction, _003C_003E8__1.clickedPawn.TraderKind).Accepted)
				{
					_003C_003E2__current = new FloatMenuOption("CannotTrade".Translate() + ": " + "MissingTitleAbility".Translate().CapitalizeFirst(), null);
					_003C_003E1__state = 4;
					return true;
				}
				Action action2 = delegate
				{
					Job job2 = JobMaker.MakeJob(JobDefOf.TradeWithPawn, _003C_003E8__1.clickedPawn);
					job2.playerForced = true;
					_003C_003E8__1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job2, JobTag.Misc);
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InteractingWithTraders, KnowledgeAmount.Total);
				};
				string text = "";
				if (_003C_003E8__1.clickedPawn.Faction != null)
				{
					text = " (" + _003C_003E8__1.clickedPawn.Faction.Name + ")";
				}
				_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TradeWith".Translate(_003C_003E8__1.clickedPawn.LabelShort + ", " + _003C_003E8__1.clickedPawn.TraderKind.label) + text, action2, MenuOptionPriority.InitiateSocial, null, _003C_003E8__1.clickedPawn), _003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedPawn);
				_003C_003E1__state = 5;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				return false;
			case 2:
				_003C_003E1__state = -1;
				return false;
			case 3:
				_003C_003E1__state = -1;
				return false;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0358;
			case 5:
				_003C_003E1__state = -1;
				goto IL_0358;
			case 6:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0358:
				if (_003C_003E8__1.clickedPawn.GetLord().LordJob is LordJob_TradeWithColony && !_003C_003E8__1.clickedPawn.mindState.traderDismissed)
				{
					Action action = delegate
					{
						Job job = JobMaker.MakeJob(JobDefOf.DismissTrader, _003C_003E8__1.clickedPawn);
						job.playerForced = true;
						_003C_003E8__1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
					};
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("DismissTrader".Translate(), action, MenuOptionPriority.InitiateSocial, null, _003C_003E8__1.clickedPawn), _003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.clickedPawn);
					_003C_003E1__state = 6;
					return true;
				}
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetOptionsFor_003Ed__6 _003CGetOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__6(0);
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

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__6))]
	public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__6(-2)
		{
			_003C_003E3__clickedPawn = clickedPawn,
			_003C_003E3__context = context
		};
	}
}
