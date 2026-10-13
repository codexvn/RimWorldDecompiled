using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200263C RID: 9788
	public class PreceptComp_KnowsMemoryThought : PreceptComp_Thought
	{
		// Token: 0x170020CA RID: 8394
		// (get) Token: 0x0600D678 RID: 54904 RVA: 0x003FA1CB File Offset: 0x003F83CB
		public override IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				return ThoughtUtility.GetNullifyingTraits(this.thought);
			}
		}

		// Token: 0x0600D679 RID: 54905 RVA: 0x003FA1D8 File Offset: 0x003F83D8
		public override void Notify_MemberWitnessedAction(HistoryEvent ev, Precept precept, Pawn member)
		{
			if (ev.def != this.eventDef)
			{
				return;
			}
			if (!precept.def.enabledForNPCFactions && !member.CountsAsNonNPCForPrecepts())
			{
				return;
			}
			Pawn pawn;
			bool flag = ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out pawn);
			bool flag2 = false;
			if (this.doerMustBeMyFaction != null)
			{
				flag2 = this.doerMustBeMyFaction.Value;
			}
			else
			{
				if (flag)
				{
					using (List<ThoughtStage>.Enumerator enumerator = this.thought.stages.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.baseMoodEffect != 0f)
							{
								flag2 = true;
								break;
							}
						}
						goto IL_9E;
					}
				}
				flag2 = false;
			}
			IL_9E:
			if (member.needs != null && member.needs.mood != null && (!flag2 || (flag && pawn.Faction == member.Faction)) && (!this.doerMustBeMyIdeo || pawn.Ideo == member.Ideo) && (!this.onlyForNonSlaves || !member.IsSlave) && (!typeof(Thought_MemorySocial).IsAssignableFrom(this.thought.thoughtClass) || flag))
			{
				Thought_Memory thought_Memory = ThoughtMaker.MakeThought(this.thought, precept);
				int val;
				if (ev.args.TryGetArg<int>(HistoryEventArgsNames.ExecutionThoughtStage, out val))
				{
					thought_Memory.SetForcedStage(Math.Min(val, this.thought.stages.Count - 1));
				}
				Thought_KilledInnocentAnimal thought_KilledInnocentAnimal = thought_Memory as Thought_KilledInnocentAnimal;
				Pawn animal;
				if (thought_KilledInnocentAnimal != null && ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Victim, out animal))
				{
					thought_KilledInnocentAnimal.SetAnimal(animal);
				}
				member.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, pawn);
				if (this.removesThought != null)
				{
					member.needs.mood.thoughts.memories.RemoveMemoriesOfDef(this.removesThought);
				}
			}
		}

		// Token: 0x040091C6 RID: 37318
		public HistoryEventDef eventDef;

		// Token: 0x040091C7 RID: 37319
		public bool? doerMustBeMyFaction;

		// Token: 0x040091C8 RID: 37320
		public ThoughtDef removesThought;

		// Token: 0x040091C9 RID: 37321
		public bool onlyForNonSlaves;

		// Token: 0x040091CA RID: 37322
		public bool doerMustBeMyIdeo;
	}
}
