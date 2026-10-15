using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class CompTechprint : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public Pawn selPawn;

		public Job job;

		internal bool _003CCompFloatMenuOptions_003Eb__0(Thing thing)
		{
			if (thing is Building_ResearchBench && !thing.IsForbidden(selPawn))
			{
				return selPawn.CanReserve(thing);
			}
			return false;
		}

		internal void _003CCompFloatMenuOptions_003Eb__1()
		{
			if (job == null)
			{
				Messages.Message("MessageNoResearchBenchForTechprint".Translate(), MessageTypeDefOf.RejectInput);
			}
			else
			{
				selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__2 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		public CompTechprint _003C_003E4__this;

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
		public _003CCompFloatMenuOptions_003Ed__2(int _003C_003E1__state)
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
			CompTechprint compTechprint = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals18 = new _003C_003Ec__DisplayClass2_0
				{
					selPawn = selPawn
				};
				if (!ModLister.CheckRoyalty("Techprint"))
				{
					return false;
				}
				JobFailReason.Clear();
				if (CS_0024_003C_003E8__locals18.selPawn.WorkTypeIsDisabled(WorkTypeDefOf.Research) || CS_0024_003C_003E8__locals18.selPawn.WorkTagIsDisabled(WorkTags.Intellectual))
				{
					JobFailReason.Is("WillNever".Translate("Research".TranslateSimple().UncapitalizeFirst()));
				}
				else if (!CS_0024_003C_003E8__locals18.selPawn.CanReach(compTechprint.parent, PathEndMode.ClosestTouch, Danger.Some))
				{
					JobFailReason.Is("CannotReach".Translate());
				}
				HaulAIUtility.PawnCanAutomaticallyHaul(CS_0024_003C_003E8__locals18.selPawn, compTechprint.parent, forced: true);
				Thing thing2 = GenClosest.ClosestThingReachable(CS_0024_003C_003E8__locals18.selPawn.Position, CS_0024_003C_003E8__locals18.selPawn.Map, ThingRequest.ForGroup(ThingRequestGroup.ResearchBench), PathEndMode.InteractionCell, TraverseParms.For(CS_0024_003C_003E8__locals18.selPawn, Danger.Some), 9999f, (Thing thing) => thing is Building_ResearchBench && !thing.IsForbidden(CS_0024_003C_003E8__locals18.selPawn) && CS_0024_003C_003E8__locals18.selPawn.CanReserve(thing));
				CS_0024_003C_003E8__locals18.job = null;
				if (thing2 != null)
				{
					CS_0024_003C_003E8__locals18.job = JobMaker.MakeJob(JobDefOf.ApplyTechprint);
					CS_0024_003C_003E8__locals18.job.targetA = thing2;
					CS_0024_003C_003E8__locals18.job.targetB = compTechprint.parent;
					CS_0024_003C_003E8__locals18.job.targetC = thing2.Position;
				}
				if (JobFailReason.HaveReason)
				{
					_003C_003E2__current = new FloatMenuOption("CannotGenericWorkCustom".Translate("ApplyTechprint".Translate(compTechprint.parent.Label)) + ": " + JobFailReason.Reason.CapitalizeFirst(), null);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ApplyTechprint".Translate(compTechprint.parent.Label).CapitalizeFirst(), delegate
				{
					if (CS_0024_003C_003E8__locals18.job == null)
					{
						Messages.Message("MessageNoResearchBenchForTechprint".Translate(), MessageTypeDefOf.RejectInput);
					}
					else
					{
						CS_0024_003C_003E8__locals18.selPawn.jobs.TryTakeOrderedJob(CS_0024_003C_003E8__locals18.job, JobTag.Misc);
					}
				}), CS_0024_003C_003E8__locals18.selPawn, compTechprint.parent);
				_003C_003E1__state = 2;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				JobFailReason.Clear();
				break;
			case 2:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompFloatMenuOptions_003Ed__2 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CCompFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	public CompProperties_Techprint Props => (CompProperties_Techprint)props;

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__2))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}
}
