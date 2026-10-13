using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020037C9 RID: 14281
	public abstract class SurgeryOutcome
	{
		// Token: 0x06013C7E RID: 81022
		public abstract bool Apply(float quality, RecipeDef recipe, Pawn surgeon, Pawn patient, BodyPartRecord part);

		// Token: 0x06013C7F RID: 81023 RVA: 0x005DE83E File Offset: 0x005DCA3E
		protected void ApplyDamage(Pawn patient, BodyPartRecord part)
		{
			if (this.totalDamage > 0)
			{
				HealthUtility.GiveRandomSurgeryInjuries(patient, this.totalDamage, this.applyEffectsToPart ? part : null);
			}
		}

		// Token: 0x06013C80 RID: 81024 RVA: 0x005DE864 File Offset: 0x005DCA64
		protected void SendLetter(Pawn surgeon, Pawn patient, RecipeDef recipe)
		{
			if (!this.letterLabel.NullOrEmpty() && !this.letterText.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(this.letterLabel.Formatted(patient.Named("PATIENT")), this.letterText.Formatted(surgeon.Named("SURGEON"), patient.Named("PATIENT"), recipe.Named("RECIPE")), LetterDefOf.NegativeEvent, patient, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0400DB02 RID: 56066
		public float chance;

		// Token: 0x0400DB03 RID: 56067
		public int totalDamage;

		// Token: 0x0400DB04 RID: 56068
		public bool applyEffectsToPart = true;

		// Token: 0x0400DB05 RID: 56069
		public bool failure;

		// Token: 0x0400DB06 RID: 56070
		[MustTranslate]
		public string letterLabel;

		// Token: 0x0400DB07 RID: 56071
		[MustTranslate]
		public string letterText;
	}
}
