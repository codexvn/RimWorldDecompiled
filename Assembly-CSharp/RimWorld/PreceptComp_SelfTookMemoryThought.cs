using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200263F RID: 9791
	public class PreceptComp_SelfTookMemoryThought : PreceptComp_Thought
	{
		// Token: 0x170020CD RID: 8397
		// (get) Token: 0x0600D685 RID: 54917 RVA: 0x003FA1CB File Offset: 0x003F83CB
		public override IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				return ThoughtUtility.GetNullifyingTraits(this.thought);
			}
		}

		// Token: 0x0600D686 RID: 54918 RVA: 0x003FA4A0 File Offset: 0x003F86A0
		public override void Notify_MemberTookAction(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts)
		{
			if (ev.def != this.eventDef || !canApplySelfTookThoughts)
			{
				return;
			}
			Pawn arg = ev.args.GetArg<Pawn>(HistoryEventArgsNames.Doer);
			if (arg.needs != null && arg.needs.mood != null && (!this.onlyForNonSlaves || !arg.IsSlave))
			{
				if (this.thought.minExpectation != null && ExpectationsUtility.CurrentExpectationFor(arg).order < this.thought.minExpectation.order)
				{
					return;
				}
				Thought_Memory thought_Memory = ThoughtMaker.MakeThought(this.thought, precept);
				Thought_KilledInnocentAnimal thought_KilledInnocentAnimal = thought_Memory as Thought_KilledInnocentAnimal;
				Pawn animal;
				if (thought_KilledInnocentAnimal != null && ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Victim, out animal))
				{
					thought_KilledInnocentAnimal.SetAnimal(animal);
				}
				Thought_MemoryObservation thought_MemoryObservation = thought_Memory as Thought_MemoryObservation;
				Corpse target;
				if (thought_MemoryObservation != null && ev.args.TryGetArg<Corpse>(HistoryEventArgsNames.Subject, out target))
				{
					thought_MemoryObservation.Target = target;
				}
				arg.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
			}
		}

		// Token: 0x040091D0 RID: 37328
		public HistoryEventDef eventDef;

		// Token: 0x040091D1 RID: 37329
		public bool onlyForNonSlaves;
	}
}
