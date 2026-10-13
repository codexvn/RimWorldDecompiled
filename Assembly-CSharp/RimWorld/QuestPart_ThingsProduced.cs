using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CC1 RID: 7361
	public class QuestPart_ThingsProduced : QuestPartActivable
	{
		// Token: 0x1700193D RID: 6461
		// (get) Token: 0x0600A404 RID: 41988 RVA: 0x002F09C8 File Offset: 0x002EEBC8
		public override string DescriptionPart
		{
			get
			{
				return "ThingsProduced".Translate().CapitalizeFirst() + ": " + this.produced.ToString() + " / " + this.count.ToString();
			}
		}

		// Token: 0x1700193E RID: 6462
		// (get) Token: 0x0600A405 RID: 41989 RVA: 0x002F0A16 File Offset: 0x002EEC16
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in base.Hyperlinks)
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				yield return new Dialog_InfoCard.Hyperlink(this.def, -1);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A406 RID: 41990 RVA: 0x002F0A26 File Offset: 0x002EEC26
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.produced = 0;
		}

		// Token: 0x0600A407 RID: 41991 RVA: 0x002F0A38 File Offset: 0x002EEC38
		public override void Notify_ThingsProduced(Pawn actor, List<Thing> things)
		{
			base.Notify_ThingsProduced(actor, things);
			if (base.State == QuestPartState.Enabled)
			{
				for (int i = 0; i < things.Count; i++)
				{
					Thing innerIfMinified = things[i].GetInnerIfMinified();
					if (innerIfMinified.def == this.def && innerIfMinified.Stuff == this.stuff)
					{
						this.produced += things[i].stackCount;
					}
				}
				if (this.produced >= this.count)
				{
					this.produced = this.count;
					base.Complete();
				}
			}
		}

		// Token: 0x0600A408 RID: 41992 RVA: 0x002F0ACC File Offset: 0x002EECCC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.count, "count", 0, false);
			Scribe_Values.Look<int>(ref this.produced, "produced", 0, false);
		}

		// Token: 0x0600A409 RID: 41993 RVA: 0x002F0B23 File Offset: 0x002EED23
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = ThingDefOf.MealSimple;
			this.count = 10;
		}

		// Token: 0x0400714C RID: 29004
		public ThingDef def;

		// Token: 0x0400714D RID: 29005
		public ThingDef stuff;

		// Token: 0x0400714E RID: 29006
		public int count;

		// Token: 0x0400714F RID: 29007
		private int produced;
	}
}
