using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006E8 RID: 1768
	public class HediffComp_MessageStageIncreased : HediffComp
	{
		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06002BA8 RID: 11176 RVA: 0x000E3CD5 File Offset: 0x000E1ED5
		protected HediffCompProperties_MessageStageIncreased Props
		{
			get
			{
				return (HediffCompProperties_MessageStageIncreased)this.props;
			}
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x000E3CE4 File Offset: 0x000E1EE4
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			if (this.parent.CurStageIndex > this.lastStageMessaged)
			{
				this.lastStageMessaged = this.parent.CurStageIndex;
				if (this.sendMessages)
				{
					Messages.Message(this.Props.message.Formatted(base.Pawn), base.Pawn, this.Props.messageType ?? MessageTypeDefOf.NeutralEvent, true);
				}
			}
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x000E3D62 File Offset: 0x000E1F62
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<bool>(ref this.sendMessages, "sendMessages", false, false);
			Scribe_Values.Look<int>(ref this.lastStageMessaged, "lastStageMessaged", 0, false);
		}

		// Token: 0x04002321 RID: 8993
		private int lastStageMessaged;

		// Token: 0x04002322 RID: 8994
		public bool sendMessages = true;
	}
}
