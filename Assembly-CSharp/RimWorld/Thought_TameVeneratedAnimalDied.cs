using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001815 RID: 6165
	public class Thought_TameVeneratedAnimalDied : Thought_Memory
	{
		// Token: 0x170015CB RID: 5579
		// (get) Token: 0x06008EDD RID: 36573 RVA: 0x002A44AC File Offset: 0x002A26AC
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.animalKindLabel.Named("ANIMALKIND")).CapitalizeFirst();
			}
		}

		// Token: 0x170015CC RID: 5580
		// (get) Token: 0x06008EDE RID: 36574 RVA: 0x002A44E8 File Offset: 0x002A26E8
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.animalKindLabel.Named("ANIMALKIND")).CapitalizeFirst() + base.CausedByBeliefInPrecept;
			}
		}

		// Token: 0x06008EDF RID: 36575 RVA: 0x002A452D File Offset: 0x002A272D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.animalKindLabel, "animalKindLabel", null, false);
		}

		// Token: 0x04005C19 RID: 23577
		public string animalKindLabel;
	}
}
