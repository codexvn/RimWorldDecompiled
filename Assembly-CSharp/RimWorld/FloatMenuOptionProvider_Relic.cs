using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld;

public class FloatMenuOptionProvider_Relic : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003CGetOptionsFor_003Ed__9 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing clickedThing;

		public Thing _003C_003E3__clickedThing;

		public FloatMenuOptionProvider_Relic _003C_003E4__this;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		private CompRelicContainer _003Ccontainer_003E5__2;

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
			_003Ccontainer_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			FloatMenuOptionProvider_Relic floatMenuOptionProvider_Relic = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (clickedThing.TryGetComp(out _003Ccontainer_003E5__2))
				{
					if (_003Ccontainer_003E5__2.Full)
					{
						_003C_003E2__current = floatMenuOptionProvider_Relic.ExtractRelic(_003Ccontainer_003E5__2, context.FirstSelectedPawn);
						_003C_003E1__state = 1;
						return true;
					}
					if (floatMenuOptionProvider_Relic.InstallRelicFromContainer(_003Ccontainer_003E5__2.parent, context.FirstSelectedPawn, out var option))
					{
						_003C_003E2__current = option;
						_003C_003E1__state = 3;
						return true;
					}
				}
				goto IL_0113;
			case 1:
				_003C_003E1__state = -1;
				if (!context.FirstSelectedPawn.Map.IsPlayerHome && !context.FirstSelectedPawn.IsFormingCaravan())
				{
					_003C_003E2__current = floatMenuOptionProvider_Relic.ExtractToInventory(_003Ccontainer_003E5__2, context.FirstSelectedPawn);
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0113;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0113;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0113;
			case 4:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0113:
				if (CompRelicContainer.IsRelic(clickedThing) && clickedThing.Spawned)
				{
					_003C_003E2__current = floatMenuOptionProvider_Relic.InstallRelic(clickedThing, context.FirstSelectedPawn);
					_003C_003E1__state = 4;
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
			_003CGetOptionsFor_003Ed__9 _003CGetOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__9(0)
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

	protected override bool Drafted => true;

	protected override bool Undrafted => true;

	protected override bool Multiselect => false;

	protected override bool RequiresManipulation => true;

	protected override bool AppliesInt(FloatMenuContext context)
	{
		return ModsConfig.IdeologyActive;
	}

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__9))]
	public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__9(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__clickedThing = clickedThing,
			_003C_003E3__context = context
		};
	}

	private FloatMenuOption InstallRelic(Thing containerThing, Pawn pawn)
	{
		IEnumerable<Thing> searchSet = from x in containerThing.Map.listerThings.ThingsOfDef(ThingDefOf.Reliquary)
			where x.TryGetComp<CompRelicContainer>().ContainedThing == null
			select x;
		Thing thing = GenClosest.ClosestThing_Global_Reachable(containerThing.Position, containerThing.Map, searchSet, PathEndMode.Touch, TraverseParms.For(pawn), 9999f, (Thing t) => pawn.CanReserve(t));
		if (thing == null)
		{
			return new FloatMenuOption("InstallInReliquary".Translate() + " (" + "NoEmptyReliquary".Translate() + ")", null);
		}
		Job job = JobMaker.MakeJob(JobDefOf.InstallRelic, containerThing, thing, thing.InteractionCell);
		job.count = 1;
		return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("InstallInReliquary".Translate(), delegate
		{
			pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}), pawn, new LocalTargetInfo(containerThing));
	}

	private FloatMenuOption ExtractRelic(CompRelicContainer container, Pawn pawn)
	{
		TaggedString taggedString = "ExtractRelic".Translate(container.ContainedThing.Label);
		if (!StoreUtility.TryFindBestBetterStorageFor(container.ContainedThing, pawn, pawn.Map, StoragePriority.Unstored, pawn.Faction, out var foundCell, out var _))
		{
			return new FloatMenuOption(taggedString + " (" + HaulAIUtility.NoEmptyPlaceLowerTrans + ")", null);
		}
		Job job = JobMaker.MakeJob(JobDefOf.ExtractRelic, container.parent, container.ContainedThing, foundCell);
		job.count = 1;
		return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate
		{
			pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}), pawn, new LocalTargetInfo(container.parent));
	}

	private bool InstallRelicFromContainer(Thing containerThing, Pawn pawn, out FloatMenuOption option)
	{
		option = null;
		IEnumerable<Thing> enumerable = pawn.Map.listerThings.AllThings.Where((Thing x) => CompRelicContainer.IsRelic(x) && pawn.CanReach(x, PathEndMode.ClosestTouch, Danger.Deadly));
		if (!enumerable.Any())
		{
			option = new FloatMenuOption("NoRelicToInstall".Translate(), null);
			return true;
		}
		using (IEnumerator<Thing> enumerator = enumerable.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				Thing current = enumerator.Current;
				Job job = JobMaker.MakeJob(JobDefOf.InstallRelic, current, containerThing, containerThing.InteractionCell);
				job.count = 1;
				option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("InstallRelic".Translate(current.Label), delegate
				{
					pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				}), pawn, new LocalTargetInfo(containerThing));
				return true;
			}
		}
		return false;
	}

	private FloatMenuOption ExtractToInventory(CompRelicContainer container, Pawn pawn)
	{
		return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ExtractRelicToInventory".Translate(container.ContainedThing.Label, 300.ToStringTicksToPeriod()), delegate
		{
			Job job = JobMaker.MakeJob(JobDefOf.ExtractToInventory, container.parent, container.ContainedThing, container.parent.InteractionCell);
			job.count = 1;
			pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}), pawn, new LocalTargetInfo(container.parent));
	}
}
