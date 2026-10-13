using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200065B RID: 1627
	public class Gene_ChemicalDependency : Gene
	{
		// Token: 0x1700072F RID: 1839
		// (get) Token: 0x06002906 RID: 10502 RVA: 0x000D8137 File Offset: 0x000D6337
		public override bool Active
		{
			get
			{
				return base.Active && this.pawn != null && !this.pawn.IsGhoul;
			}
		}

		// Token: 0x17000730 RID: 1840
		// (get) Token: 0x06002907 RID: 10503 RVA: 0x000D815C File Offset: 0x000D635C
		public Hediff_ChemicalDependency LinkedHediff
		{
			get
			{
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff_ChemicalDependency hediff_ChemicalDependency = hediffs[i] as Hediff_ChemicalDependency;
					if (hediff_ChemicalDependency != null && hediff_ChemicalDependency.chemical == this.def.chemical)
					{
						return hediff_ChemicalDependency;
					}
				}
				return null;
			}
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x000D81B8 File Offset: 0x000D63B8
		public override void PostAdd()
		{
			if (!ModLister.CheckBiotech("Chemical dependency"))
			{
				return;
			}
			base.PostAdd();
			if (this.def.chemical.addictionHediff != null)
			{
				Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(this.def.chemical.addictionHediff, false);
				if (firstHediffOfDef != null)
				{
					this.pawn.health.RemoveHediff(firstHediffOfDef);
				}
			}
			this.AddHediff();
			this.lastIngestedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x000D823C File Offset: 0x000D643C
		private void AddHediff()
		{
			if (this.Active)
			{
				Hediff_ChemicalDependency hediff_ChemicalDependency = (Hediff_ChemicalDependency)HediffMaker.MakeHediff(HediffDefOf.GeneticDrugNeed, this.pawn, null);
				hediff_ChemicalDependency.chemical = this.def.chemical;
				this.pawn.health.AddHediff(hediff_ChemicalDependency, null, null, null);
			}
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x000D8298 File Offset: 0x000D6498
		public override void PostRemove()
		{
			Hediff_ChemicalDependency linkedHediff = this.LinkedHediff;
			if (linkedHediff != null)
			{
				this.pawn.health.RemoveHediff(linkedHediff);
			}
			base.PostRemove();
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x000D82C8 File Offset: 0x000D64C8
		public override void Notify_IngestedThing(Thing thing, int numTaken)
		{
			if (!thing.def.thingCategories.NullOrEmpty<ThingCategoryDef>() && !thing.def.thingCategories.Contains(ThingCategoryDefOf.Drugs))
			{
				return;
			}
			CompDrug compDrug = thing.TryGetComp<CompDrug>();
			if (compDrug == null || compDrug.Props.chemical != this.def.chemical)
			{
				return;
			}
			this.Reset();
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x000D8328 File Offset: 0x000D6528
		public override void Reset()
		{
			Hediff_ChemicalDependency linkedHediff = this.LinkedHediff;
			if (linkedHediff != null)
			{
				linkedHediff.Severity = linkedHediff.def.initialSeverity;
			}
			else
			{
				this.AddHediff();
			}
			this.lastIngestedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x000D8368 File Offset: 0x000D6568
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.lastIngestedTick, "lastIngestedTick", 0, false);
		}

		// Token: 0x040021A0 RID: 8608
		public int lastIngestedTick;
	}
}
