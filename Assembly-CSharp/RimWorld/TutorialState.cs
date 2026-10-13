using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020034EF RID: 13551
	public class TutorialState : IExposable
	{
		// Token: 0x060130A0 RID: 77984 RVA: 0x005A47D8 File Offset: 0x005A29D8
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.startingItems != null)
			{
				this.startingItems.RemoveAll((Thing it) => it == null || it.Destroyed || (it.Map == null && it.MapHeld == null));
			}
			Scribe_Collections.Look<Thing>(ref this.startingItems, "startingItems", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<CellRect>(ref this.roomRect, "roomRect", default(CellRect), false);
			Scribe_Values.Look<CellRect>(ref this.sandbagsRect, "sandbagsRect", default(CellRect), false);
			Scribe_Values.Look<int>(ref this.endTick, "endTick", -1, false);
			Scribe_Values.Look<bool>(ref this.introDone, "introDone", false, false);
			if (this.startingItems != null)
			{
				this.startingItems.RemoveAll((Thing it) => it == null);
			}
		}

		// Token: 0x060130A1 RID: 77985 RVA: 0x005A48C1 File Offset: 0x005A2AC1
		public void Notify_TutorialEnding()
		{
			this.startingItems.Clear();
			this.roomRect = default(CellRect);
			this.sandbagsRect = default(CellRect);
			this.endTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060130A2 RID: 77986 RVA: 0x005A48F6 File Offset: 0x005A2AF6
		public void AddStartingItem(Thing t)
		{
			if (this.startingItems.Contains(t))
			{
				return;
			}
			this.startingItems.Add(t);
		}

		// Token: 0x0400C54D RID: 50509
		public List<Thing> startingItems = new List<Thing>();

		// Token: 0x0400C54E RID: 50510
		public CellRect roomRect;

		// Token: 0x0400C54F RID: 50511
		public CellRect sandbagsRect;

		// Token: 0x0400C550 RID: 50512
		public int endTick = -1;

		// Token: 0x0400C551 RID: 50513
		public bool introDone;
	}
}
