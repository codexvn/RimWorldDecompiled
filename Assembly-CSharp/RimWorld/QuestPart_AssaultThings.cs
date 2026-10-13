using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BEC RID: 7148
	public class QuestPart_AssaultThings : QuestPart_MakeLord
	{
		// Token: 0x06009F3B RID: 40763 RVA: 0x002DFACE File Offset: 0x002DDCCE
		protected override Lord MakeLord()
		{
			return LordMaker.MakeNewLord(this.faction, new LordJob_AssaultThings(this.faction, this.things, 1f, false), base.Map, null);
		}

		// Token: 0x06009F3C RID: 40764 RVA: 0x002DFAFC File Offset: 0x002DDCFC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04006E19 RID: 28185
		public List<Thing> things = new List<Thing>();
	}
}
