using System;

namespace Verse.AI.Group
{
	// Token: 0x02001054 RID: 4180
	public class PsychicRitualLordToilData : LordToilData
	{
		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x06006603 RID: 26115 RVA: 0x00204FD0 File Offset: 0x002031D0
		public PsychicRitualToil CurPsychicRitualToil
		{
			get
			{
				PsychicRitualGraph psychicRitualGraph = this.psychicRitualToil as PsychicRitualGraph;
				if (psychicRitualGraph != null)
				{
					return psychicRitualGraph.CurrentToil;
				}
				return this.psychicRitualToil;
			}
		}

		// Token: 0x06006604 RID: 26116 RVA: 0x00204FFC File Offset: 0x002031FC
		public override void ExposeData()
		{
			Scribe_Deep.Look<PsychicRitualToil>(ref this.psychicRitualToil, "psychicRitualToil", Array.Empty<object>());
			Scribe_Deep.Look<PsychicRitual>(ref this.psychicRitual, "psychicRitual", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.done, "done", false, false);
			Scribe_Values.Look<int>(ref this.iteration, "iteration", 0, false);
			Scribe_Values.Look<bool>(ref this.playerRitual, "playerRitual", false, false);
			Scribe_Values.Look<bool>(ref this.removeLordOnCancel, "removeLordOnCancel", false, false);
		}

		// Token: 0x040048F8 RID: 18680
		public PsychicRitual psychicRitual;

		// Token: 0x040048F9 RID: 18681
		public PsychicRitualToil psychicRitualToil;

		// Token: 0x040048FA RID: 18682
		public bool done;

		// Token: 0x040048FB RID: 18683
		public int iteration;

		// Token: 0x040048FC RID: 18684
		public bool playerRitual;

		// Token: 0x040048FD RID: 18685
		public bool removeLordOnCancel;
	}
}
