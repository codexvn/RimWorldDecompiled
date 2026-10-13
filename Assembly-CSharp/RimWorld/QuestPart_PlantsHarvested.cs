using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C76 RID: 7286
	public class QuestPart_PlantsHarvested : QuestPartActivable
	{
		// Token: 0x170018F5 RID: 6389
		// (get) Token: 0x0600A264 RID: 41572 RVA: 0x002EAD7C File Offset: 0x002E8F7C
		public override string DescriptionPart
		{
			get
			{
				return "PlantsHarvested".Translate().CapitalizeFirst() + ": " + this.harvested.ToString() + " / " + this.count.ToString();
			}
		}

		// Token: 0x170018F6 RID: 6390
		// (get) Token: 0x0600A265 RID: 41573 RVA: 0x002EADCA File Offset: 0x002E8FCA
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in base.Hyperlinks)
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				yield return new Dialog_InfoCard.Hyperlink(this.plant, -1);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A266 RID: 41574 RVA: 0x002EADDA File Offset: 0x002E8FDA
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.harvested = 0;
		}

		// Token: 0x0600A267 RID: 41575 RVA: 0x002EADEC File Offset: 0x002E8FEC
		public override void Notify_PlantHarvested(Pawn actor, Thing harvested)
		{
			base.Notify_PlantHarvested(actor, harvested);
			if (base.State == QuestPartState.Enabled && harvested.def == this.plant)
			{
				this.harvested += harvested.stackCount;
				if (this.harvested >= this.count)
				{
					this.harvested = this.count;
					base.Complete();
				}
			}
		}

		// Token: 0x0600A268 RID: 41576 RVA: 0x002EAE4B File Offset: 0x002E904B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plant, "plant");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.harvested, "harvested", 0, false);
		}

		// Token: 0x0600A269 RID: 41577 RVA: 0x002EAE87 File Offset: 0x002E9087
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.plant = ThingDefOf.RawPotatoes;
			this.count = 10;
		}

		// Token: 0x04007038 RID: 28728
		public ThingDef plant;

		// Token: 0x04007039 RID: 28729
		public int count;

		// Token: 0x0400703A RID: 28730
		private int harvested;
	}
}
