using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001811 RID: 6161
	public class Thought_RelicAtRitual : Thought_Memory
	{
		// Token: 0x170015C6 RID: 5574
		// (get) Token: 0x06008ECB RID: 36555 RVA: 0x002A4018 File Offset: 0x002A2218
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.relicName.Named("RELICNAME")).CapitalizeFirst();
			}
		}

		// Token: 0x170015C7 RID: 5575
		// (get) Token: 0x06008ECC RID: 36556 RVA: 0x002A4054 File Offset: 0x002A2254
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.relicName.Named("RELICNAME")).CapitalizeFirst();
			}
		}

		// Token: 0x06008ECD RID: 36557 RVA: 0x002A408E File Offset: 0x002A228E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.relicName, "relicName", null, false);
		}

		// Token: 0x04005C15 RID: 23573
		public string relicName;
	}
}
