using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020037CF RID: 14287
	public class WorkGiverDef : Def
	{
		// Token: 0x17003287 RID: 12935
		// (get) Token: 0x06013C90 RID: 81040 RVA: 0x005DEADA File Offset: 0x005DCCDA
		public WorkGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (WorkGiver)Activator.CreateInstance(this.giverClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06013C91 RID: 81041 RVA: 0x005DEB0C File Offset: 0x005DCD0C
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.verb.NullOrEmpty())
			{
				yield return this.defName + " lacks a verb.";
			}
			if (this.gerund.NullOrEmpty())
			{
				yield return this.defName + " lacks a gerund.";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400DB0C RID: 56076
		public Type giverClass;

		// Token: 0x0400DB0D RID: 56077
		public WorkTypeDef workType;

		// Token: 0x0400DB0E RID: 56078
		public WorkTags workTags;

		// Token: 0x0400DB0F RID: 56079
		public int priorityInType;

		// Token: 0x0400DB10 RID: 56080
		[MustTranslate]
		public string verb;

		// Token: 0x0400DB11 RID: 56081
		[MustTranslate]
		public string gerund;

		// Token: 0x0400DB12 RID: 56082
		public bool scanThings = true;

		// Token: 0x0400DB13 RID: 56083
		public bool scanCells;

		// Token: 0x0400DB14 RID: 56084
		public bool emergency;

		// Token: 0x0400DB15 RID: 56085
		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		// Token: 0x0400DB16 RID: 56086
		public bool directOrderable = true;

		// Token: 0x0400DB17 RID: 56087
		public bool prioritizeSustains;

		// Token: 0x0400DB18 RID: 56088
		public bool nonColonistsCanDo;

		// Token: 0x0400DB19 RID: 56089
		public JobTag tagToGive = JobTag.MiscWork;

		// Token: 0x0400DB1A RID: 56090
		public WorkGiverEquivalenceGroupDef equivalenceGroup;

		// Token: 0x0400DB1B RID: 56091
		public bool canBeDoneWhileDrafted;

		// Token: 0x0400DB1C RID: 56092
		public int autoTakeablePriorityDrafted = -1;

		// Token: 0x0400DB1D RID: 56093
		public ThingDef forceMote;

		// Token: 0x0400DB1E RID: 56094
		public FleckDef forceFleck;

		// Token: 0x0400DB1F RID: 56095
		public bool canBeDoneByMechs = true;

		// Token: 0x0400DB20 RID: 56096
		public List<ThingDef> fixedBillGiverDefs;

		// Token: 0x0400DB21 RID: 56097
		public bool billGiversAllHumanlikes;

		// Token: 0x0400DB22 RID: 56098
		public bool billGiversAllHumanlikesCorpses;

		// Token: 0x0400DB23 RID: 56099
		public bool billGiversAllMechanoids;

		// Token: 0x0400DB24 RID: 56100
		public bool billGiversAllMechanoidsCorpses;

		// Token: 0x0400DB25 RID: 56101
		public bool billGiversAllAnimals;

		// Token: 0x0400DB26 RID: 56102
		public bool billGiversAllAnimalsCorpses;

		// Token: 0x0400DB27 RID: 56103
		public bool feedHumanlikesOnly;

		// Token: 0x0400DB28 RID: 56104
		public bool feedAnimalsOnly;

		// Token: 0x0400DB29 RID: 56105
		public bool doesSmoothing;

		// Token: 0x0400DB2A RID: 56106
		public ThingDef scannerDef;

		// Token: 0x0400DB2B RID: 56107
		[Unsaved(false)]
		private WorkGiver workerInt;
	}
}
