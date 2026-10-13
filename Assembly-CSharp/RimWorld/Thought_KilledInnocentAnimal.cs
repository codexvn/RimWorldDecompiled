using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002540 RID: 9536
	public class Thought_KilledInnocentAnimal : Thought_Memory
	{
		// Token: 0x17002016 RID: 8214
		// (get) Token: 0x0600D0A1 RID: 53409 RVA: 0x003D5A44 File Offset: 0x003D3C44
		public override string LabelCap
		{
			get
			{
				string text = GenLabel.BestKindLabel(this.animal, this.gender, this.plural, -1);
				if (this.otherAnimals)
				{
					text += ", " + "Etc".Translate();
				}
				return base.CurStage.label.Formatted(text).CapitalizeFirst();
			}
		}

		// Token: 0x0600D0A2 RID: 53410 RVA: 0x003D5AB5 File Offset: 0x003D3CB5
		public void SetAnimal(Pawn animal)
		{
			this.animal = animal.kindDef;
			this.gender = animal.gender;
		}

		// Token: 0x0600D0A3 RID: 53411 RVA: 0x003D5AD0 File Offset: 0x003D3CD0
		public override void Notify_NewThoughtInGroupAdded(Thought_Memory memory)
		{
			base.Notify_NewThoughtInGroupAdded(memory);
			Thought_KilledInnocentAnimal thought_KilledInnocentAnimal = memory as Thought_KilledInnocentAnimal;
			if (thought_KilledInnocentAnimal != null)
			{
				if (thought_KilledInnocentAnimal.animal == this.animal)
				{
					this.plural = true;
					return;
				}
				this.otherAnimals = true;
			}
		}

		// Token: 0x0600D0A4 RID: 53412 RVA: 0x003D5B0C File Offset: 0x003D3D0C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.animal, "animal");
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<bool>(ref this.plural, "plural", false, false);
			Scribe_Values.Look<bool>(ref this.otherAnimals, "otherAnimals", false, false);
		}

		// Token: 0x04008DD5 RID: 36309
		public PawnKindDef animal;

		// Token: 0x04008DD6 RID: 36310
		public Gender gender;

		// Token: 0x04008DD7 RID: 36311
		public bool plural;

		// Token: 0x04008DD8 RID: 36312
		public bool otherAnimals;
	}
}
