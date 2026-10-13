using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02002495 RID: 9365
	public class PawnObserver
	{
		// Token: 0x0600CB10 RID: 51984 RVA: 0x003B7C0C File Offset: 0x003B5E0C
		public PawnObserver(Pawn pawn)
		{
			this.pawn = pawn;
			this.intervalsUntilObserve = 0;
		}

		// Token: 0x0600CB11 RID: 51985 RVA: 0x003B7C2D File Offset: 0x003B5E2D
		public void ObserverInterval()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			this.intervalsUntilObserve--;
			if (this.intervalsUntilObserve <= 0)
			{
				this.ObserveSurroundingThings();
				this.intervalsUntilObserve = 4 + Rand.RangeInclusive(-1, 1);
			}
		}

		// Token: 0x0600CB12 RID: 51986 RVA: 0x003B7C6C File Offset: 0x003B5E6C
		private void ObserveSurroundingThings()
		{
			TerrorUtility.RemoveAllTerrorThoughts(this.pawn);
			this.terrorThoughts.Clear();
			if (PawnUtility.IsBiologicallyOrArtificiallyBlind(this.pawn) || !this.pawn.Awake() || this.pawn.needs.mood == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(this.pawn.Position, this.pawn.Map, (Region from, Region to) => this.pawn.Position.InHorDistOf(to.extentsClose.ClosestCellTo(this.pawn.Position), 5f), delegate(Region reg)
			{
				foreach (Thing thing in reg.ListerThings.ThingsInGroup(ThingRequestGroup.Corpse))
				{
					if (this.PossibleToObserve(thing))
					{
						this.<ObserveSurroundingThings>g__TryCreateObservedThought|7_0(thing);
						this.<ObserveSurroundingThings>g__TryCreateObservedHistoryEvent|7_1(thing);
					}
				}
				foreach (Thing thing2 in reg.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn))
				{
					if (this.PossibleToObserve(thing2))
					{
						this.<ObserveSurroundingThings>g__TryCreateObservedThought|7_0(thing2);
						this.<ObserveSurroundingThings>g__TryCreateObservedHistoryEvent|7_1(thing2);
					}
				}
				foreach (Thing thing3 in reg.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
				{
					if (this.PossibleToObserve(thing3))
					{
						this.<ObserveSurroundingThings>g__TryCreateObservedThought|7_0(thing3);
						this.<ObserveSurroundingThings>g__TryCreateObservedHistoryEvent|7_1(thing3);
					}
				}
				return true;
			}, 999999, RegionType.Set_Passable);
			foreach (Thought_MemoryObservationTerror newThought in TerrorUtility.TakeTopTerrorThoughts(this.terrorThoughts))
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
			}
			this.terrorThoughts.Clear();
		}

		// Token: 0x0600CB13 RID: 51987 RVA: 0x003B7D60 File Offset: 0x003B5F60
		private bool PossibleToObserve(Thing thing)
		{
			return thing.Position.InHorDistOf(this.pawn.Position, 5f) && GenSight.LineOfSight(thing.Position, this.pawn.Position, this.pawn.Map, true, null, 0, 0);
		}

		// Token: 0x0600CB14 RID: 51988 RVA: 0x003B7DB4 File Offset: 0x003B5FB4
		[CompilerGenerated]
		private void <ObserveSurroundingThings>g__TryCreateObservedThought|7_0(Thing thing)
		{
			Thought_MemoryObservationTerror item;
			if (TerrorUtility.TryCreateTerrorThought(thing, out item))
			{
				this.terrorThoughts.Add(item);
			}
			IObservedThoughtGiver observedThoughtGiver = thing as IObservedThoughtGiver;
			if (observedThoughtGiver != null)
			{
				Thought_Memory thought_Memory = observedThoughtGiver.GiveObservedThought(this.pawn);
				if (thought_Memory != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
				}
			}
		}

		// Token: 0x0600CB15 RID: 51989 RVA: 0x003B7E14 File Offset: 0x003B6014
		[CompilerGenerated]
		private void <ObserveSurroundingThings>g__TryCreateObservedHistoryEvent|7_1(Thing thing)
		{
			IObservedThoughtGiver observedThoughtGiver = thing as IObservedThoughtGiver;
			if (observedThoughtGiver != null)
			{
				HistoryEventDef historyEventDef = observedThoughtGiver.GiveObservedHistoryEvent(this.pawn);
				if (historyEventDef != null)
				{
					HistoryEvent historyEvent = new HistoryEvent(historyEventDef, this.pawn.Named(HistoryEventArgsNames.Doer), thing.Named(HistoryEventArgsNames.Subject));
					Find.HistoryEventsManager.RecordEvent(historyEvent, true);
				}
			}
		}

		// Token: 0x04008A87 RID: 35463
		private Pawn pawn;

		// Token: 0x04008A88 RID: 35464
		private int intervalsUntilObserve;

		// Token: 0x04008A89 RID: 35465
		private List<Thought_MemoryObservationTerror> terrorThoughts = new List<Thought_MemoryObservationTerror>();

		// Token: 0x04008A8A RID: 35466
		private const int IntervalsBetweenObservations = 4;

		// Token: 0x04008A8B RID: 35467
		private const float ObservationRadius = 5f;
	}
}
