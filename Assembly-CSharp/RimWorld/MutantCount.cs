using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020028A5 RID: 10405
	public class MutantCount : StartingPawnCount
	{
		// Token: 0x170022A4 RID: 8868
		// (get) Token: 0x0600E3BF RID: 58303 RVA: 0x00428FC0 File Offset: 0x004271C0
		public override string Summary
		{
			get
			{
				string key = "PawnCount";
				NamedArgument arg = this.count.Named("COUNT");
				string arg2;
				if ((arg2 = this.description) == null)
				{
					MutantDef mutantDef = this.mutant;
					arg2 = (((mutantDef != null) ? mutantDef.label : null) ?? Find.ActiveLanguageWorker.Pluralize("Colonist".Translate(), this.count));
				}
				return key.Translate(arg, arg2.Named("KINDLABEL"));
			}
		}

		// Token: 0x0600E3C0 RID: 58304 RVA: 0x0042903A File Offset: 0x0042723A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<MutantDef>(ref this.mutant, "mutant");
			Scribe_Values.Look<DevelopmentalStage>(ref this.allowedDevelopmentalStages, "allowedDevelopmentalStages", DevelopmentalStage.None, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
		}

		// Token: 0x0600E3C1 RID: 58305 RVA: 0x00429078 File Offset: 0x00427278
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			num ^= this.allowedDevelopmentalStages.GetHashCode();
			num ^= this.description.GetHashCodeSafe();
			if (this.mutant != null)
			{
				num ^= this.mutant.GetHashCode();
			}
			return num;
		}

		// Token: 0x0400987E RID: 39038
		public MutantDef mutant;

		// Token: 0x0400987F RID: 39039
		public DevelopmentalStage allowedDevelopmentalStages = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04009880 RID: 39040
		[MustTranslate]
		public string description;
	}
}
