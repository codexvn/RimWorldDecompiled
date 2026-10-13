using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C1D RID: 11293
	public abstract class CompBiosculpterPod_Cycle : ThingComp
	{
		// Token: 0x170027D7 RID: 10199
		// (get) Token: 0x0600FD49 RID: 64841 RVA: 0x00497AED File Offset: 0x00495CED
		public CompProperties_BiosculpterPod_BaseCycle Props
		{
			get
			{
				return (CompProperties_BiosculpterPod_BaseCycle)this.props;
			}
		}

		// Token: 0x0600FD4A RID: 64842
		public abstract void CycleCompleted(Pawn occupant);

		// Token: 0x0600FD4B RID: 64843 RVA: 0x00497AFA File Offset: 0x00495CFA
		public virtual string Description(Pawn tunedFor)
		{
			return this.Props.description;
		}

		// Token: 0x0600FD4C RID: 64844 RVA: 0x00497B08 File Offset: 0x00495D08
		public List<string> MissingResearchLabels()
		{
			this.tmpMissingResearchLabels.Clear();
			if (this.Props.requiredResearch.NullOrEmpty<ResearchProjectDef>())
			{
				return this.tmpMissingResearchLabels;
			}
			foreach (ResearchProjectDef researchProjectDef in this.Props.requiredResearch)
			{
				if (!researchProjectDef.IsFinished)
				{
					this.tmpMissingResearchLabels.Add(researchProjectDef.LabelCap);
				}
			}
			return this.tmpMissingResearchLabels;
		}

		// Token: 0x0400A60B RID: 42507
		private List<string> tmpMissingResearchLabels = new List<string>();
	}
}
