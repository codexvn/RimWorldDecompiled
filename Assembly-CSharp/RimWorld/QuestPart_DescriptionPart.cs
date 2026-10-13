using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C23 RID: 7203
	public class QuestPart_DescriptionPart : QuestPartActivable
	{
		// Token: 0x170018A8 RID: 6312
		// (get) Token: 0x0600A06B RID: 41067 RVA: 0x002E3A64 File Offset: 0x002E1C64
		public override string DescriptionPart
		{
			get
			{
				return this.resolvedDescriptionPart;
			}
		}

		// Token: 0x0600A06C RID: 41068 RVA: 0x002E3A6C File Offset: 0x002E1C6C
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			this.resolvedDescriptionPart = receivedArgs.GetFormattedText(this.descriptionPart).Resolve();
		}

		// Token: 0x0600A06D RID: 41069 RVA: 0x002E3AA0 File Offset: 0x002E1CA0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.descriptionPart, "descriptionPart", null, false);
			Scribe_Values.Look<string>(ref this.resolvedDescriptionPart, "resolvedDescriptionPart", null, false);
		}

		// Token: 0x0600A06E RID: 41070 RVA: 0x002E3ACC File Offset: 0x002E1CCC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.descriptionPart = "Debug description part.";
		}

		// Token: 0x04006EED RID: 28397
		public string descriptionPart;

		// Token: 0x04006EEE RID: 28398
		protected string resolvedDescriptionPart;
	}
}
