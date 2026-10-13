using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002544 RID: 9540
	public class Thought_MemoryObservation_Chimera : Thought_MemoryObservation
	{
		// Token: 0x17002022 RID: 8226
		// (get) Token: 0x0600D0C0 RID: 53440 RVA: 0x003D6124 File Offset: 0x003D4324
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.animalLabel.Named("ANIMAL")).CapitalizeFirst();
			}
		}

		// Token: 0x17002023 RID: 8227
		// (get) Token: 0x0600D0C1 RID: 53441 RVA: 0x003D615E File Offset: 0x003D435E
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.animalLabel.Named("ANIMAL"));
			}
		}

		// Token: 0x17002024 RID: 8228
		// (get) Token: 0x0600D0C2 RID: 53442 RVA: 0x003D6185 File Offset: 0x003D4385
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || this.animalLabel.NullOrEmpty();
			}
		}

		// Token: 0x17002025 RID: 8229
		// (set) Token: 0x0600D0C3 RID: 53443 RVA: 0x003D619C File Offset: 0x003D439C
		public override Thing Target
		{
			set
			{
				this.targetThingID = value.thingIDNumber;
				CompChimera compChimera = value.TryGetComp<CompChimera>();
				string text;
				if (compChimera == null)
				{
					text = null;
				}
				else
				{
					CompProperties_Chimera props = compChimera.Props;
					text = ((props != null) ? props.simpleAnimalLabel : null);
				}
				this.animalLabel = text;
			}
		}

		// Token: 0x0600D0C4 RID: 53444 RVA: 0x003D61CE File Offset: 0x003D43CE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.animalLabel, "animalLabel", null, false);
		}

		// Token: 0x04008DE6 RID: 36326
		private string animalLabel;
	}
}
