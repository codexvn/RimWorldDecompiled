using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BF5 RID: 7157
	public class QuestPart_ExitOnShuttle : QuestPart_MakeLord
	{
		// Token: 0x06009F59 RID: 40793 RVA: 0x002E0231 File Offset: 0x002DE431
		public QuestPart_ExitOnShuttle()
		{
			ModLister.CheckRoyaltyOrIdeology("Shuttle");
		}

		// Token: 0x06009F5A RID: 40794 RVA: 0x002E024B File Offset: 0x002DE44B
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_ExitOnShuttle(this.shuttle, this.addFleeToil), base.Map, null);
		}

		// Token: 0x06009F5B RID: 40795 RVA: 0x002E0270 File Offset: 0x002DE470
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04006E35 RID: 28213
		public Thing shuttle;

		// Token: 0x04006E36 RID: 28214
		public bool addFleeToil = true;
	}
}
