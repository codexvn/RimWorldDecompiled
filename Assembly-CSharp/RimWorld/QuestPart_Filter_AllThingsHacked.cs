using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B9E RID: 7070
	public class QuestPart_Filter_AllThingsHacked : QuestPart_Filter
	{
		// Token: 0x06009DB5 RID: 40373 RVA: 0x002DB08C File Offset: 0x002D928C
		protected override bool Pass(SignalArgs args)
		{
			if (this.things.NullOrEmpty<Thing>())
			{
				return false;
			}
			foreach (Thing thing in this.things)
			{
				if (!thing.IsHackable() || !thing.IsHacked())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009DB6 RID: 40374 RVA: 0x002DB100 File Offset: 0x002D9300
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04006D44 RID: 27972
		public List<Thing> things = new List<Thing>();
	}
}
