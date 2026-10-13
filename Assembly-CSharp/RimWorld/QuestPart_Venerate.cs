using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BFA RID: 7162
	public class QuestPart_Venerate : QuestPart_MakeLord
	{
		// Token: 0x06009F76 RID: 40822 RVA: 0x002E08D7 File Offset: 0x002DEAD7
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_Venerate(this.target, this.venerateDurationTicks, this.outSignalVenerationCompleted, this.inSignalForceExit), base.Map, null);
		}

		// Token: 0x06009F77 RID: 40823 RVA: 0x002E0908 File Offset: 0x002DEB08
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
			Scribe_Values.Look<int>(ref this.venerateDurationTicks, "venerateDurationTicks", 0, false);
			Scribe_Values.Look<string>(ref this.inSignalForceExit, "inSignalForceExit", null, false);
		}

		// Token: 0x04006E50 RID: 28240
		public Thing target;

		// Token: 0x04006E51 RID: 28241
		public int venerateDurationTicks;

		// Token: 0x04006E52 RID: 28242
		public string outSignalVenerationCompleted;

		// Token: 0x04006E53 RID: 28243
		public string inSignalForceExit;
	}
}
