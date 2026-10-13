using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020037CC RID: 14284
	public class SurgeryOutcome_Failure : SurgeryOutcome
	{
		// Token: 0x06013C86 RID: 81030 RVA: 0x005DE978 File Offset: 0x005DCB78
		protected virtual bool CanApply(RecipeDef recipe)
		{
			return Rand.Chance(this.chance);
		}

		// Token: 0x06013C87 RID: 81031 RVA: 0x005DE985 File Offset: 0x005DCB85
		public override bool Apply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part)
		{
			if (this.CanApply(recipe))
			{
				base.ApplyDamage(patient, part);
				this.PostDamagedApplied(patient);
				if (!patient.Dead)
				{
					this.TryGainBotchedSurgeryThought(patient, surgeon);
				}
				base.SendLetter(surgeon, patient, recipe);
				return true;
			}
			return false;
		}

		// Token: 0x06013C88 RID: 81032 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void PostDamagedApplied(Pawn patient)
		{
		}

		// Token: 0x06013C89 RID: 81033 RVA: 0x005DE9C1 File Offset: 0x005DCBC1
		protected void TryGainBotchedSurgeryThought(Pawn patient, Pawn surgeon)
		{
			if (!patient.RaceProps.Humanlike || patient.needs.mood == null)
			{
				return;
			}
			patient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BotchedMySurgery, surgeon, null);
		}
	}
}
