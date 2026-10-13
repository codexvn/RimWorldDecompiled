using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02001067 RID: 4199
	public class PsychicRitualToil_Brainwipe : PsychicRitualToil
	{
		// Token: 0x060066AE RID: 26286 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_Brainwipe()
		{
		}

		// Token: 0x060066AF RID: 26287 RVA: 0x002074F1 File Offset: 0x002056F1
		public PsychicRitualToil_Brainwipe(PsychicRitualRoleDef invokerRole, PsychicRitualRoleDef targetRole)
		{
			this.invokerRole = invokerRole;
			this.targetRole = targetRole;
		}

		// Token: 0x060066B0 RID: 26288 RVA: 0x00207508 File Offset: 0x00205708
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			PsychicRitualDef_Brainwipe psychicRitualDef_Brainwipe = (PsychicRitualDef_Brainwipe)psychicRitual.def;
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(this.targetRole);
			this.comaDurationTicks = Mathf.RoundToInt(psychicRitualDef_Brainwipe.comaDurationDaysFromQualityCurve.Evaluate(psychicRitual.PowerPercent) * 60000f);
			if (pawn != null && pawn2 != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, pawn2);
			}
		}

		// Token: 0x060066B1 RID: 26289 RVA: 0x00207580 File Offset: 0x00205780
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, Pawn target)
		{
			Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BrainwipeComa, target, null);
			HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null)
			{
				hediffComp_Disappears.SetDuration(this.comaDurationTicks);
			}
			target.health.AddHediff(hediff, null, null, null);
			bool flag = target.Inhumanized();
			if (flag)
			{
				target.Rehumanize();
			}
			bool recruitable = target.guest.Recruitable;
			target.guest.Recruitable = true;
			MemoryThoughtHandler memories = target.needs.mood.thoughts.memories;
			List<Thought_Memory> list = new List<Thought_Memory>();
			List<Thought_Memory> list2 = new List<Thought_Memory>();
			foreach (Thought_Memory thought_Memory in memories.Memories)
			{
				if (thought_Memory.MoodOffset() != 0f && thought_Memory.DurationTicks > 0)
				{
					list.Add(thought_Memory);
				}
				if (thought_Memory is ISocialThought)
				{
					list2.Add(thought_Memory);
				}
			}
			foreach (Thought_Memory th in list)
			{
				memories.RemoveMemory(th);
			}
			foreach (Thought_Memory th2 in list2)
			{
				memories.RemoveMemory(th2);
			}
			target.guest.resistance = 0f;
			target.guest.will = 0f;
			if (ModsConfig.IdeologyActive)
			{
				target.ideo.OffsetCertainty(-target.ideo.Certainty);
			}
			TaggedString taggedString = "BrainwipeCompleteText".Translate(invoker.Named("INVOKER"), psychicRitual.def.Named("RITUAL"), target.Named("TARGET"));
			taggedString += "\n\n" + "BrainwipeTargetComa".Translate(target.Named("TARGET"), this.comaDurationTicks.ToStringTicksToDays("F1")).CapitalizeFirst();
			if (!recruitable)
			{
				taggedString += "\n\n" + "BrainwipeUnwaveringlyLoyal".Translate(target.Named("TARGET"));
			}
			if (flag)
			{
				taggedString += "\n\n" + "BrainwipeRehumanized".Translate(target.Named("TARGET"));
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), taggedString, LetterDefOf.NeutralEvent, new LookTargets(new TargetInfo[]
			{
				invoker,
				target
			}), null, null, null, null, 0, true);
		}

		// Token: 0x060066B2 RID: 26290 RVA: 0x00207870 File Offset: 0x00205A70
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.targetRole, "targetRole");
			Scribe_Values.Look<int>(ref this.comaDurationTicks, "comaDurationTicks", 0, false);
		}

		// Token: 0x04004947 RID: 18759
		public PsychicRitualRoleDef targetRole;

		// Token: 0x04004948 RID: 18760
		public PsychicRitualRoleDef invokerRole;

		// Token: 0x04004949 RID: 18761
		public int comaDurationTicks;
	}
}
