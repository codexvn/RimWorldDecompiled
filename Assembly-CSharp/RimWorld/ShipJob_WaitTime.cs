using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001781 RID: 6017
	public class ShipJob_WaitTime : ShipJob_Wait
	{
		// Token: 0x170015B1 RID: 5553
		// (get) Token: 0x06008D34 RID: 36148 RVA: 0x0029FFDF File Offset: 0x0029E1DF
		protected override bool ShouldEnd
		{
			get
			{
				return this.startTick >= 0 && Find.TickManager.TicksGame >= this.startTick + this.duration;
			}
		}

		// Token: 0x06008D35 RID: 36149 RVA: 0x002A0008 File Offset: 0x0029E208
		public override bool TryStart()
		{
			if (!this.transportShip.ShipExistsAndIsSpawned)
			{
				return false;
			}
			if (!base.TryStart())
			{
				return false;
			}
			if (this.startTick < 0)
			{
				this.startTick = Find.TickManager.TicksGame;
			}
			return true;
		}

		// Token: 0x06008D36 RID: 36150 RVA: 0x002A003D File Offset: 0x0029E23D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x04005BC3 RID: 23491
		public int duration;

		// Token: 0x04005BC4 RID: 23492
		private int startTick = -1;
	}
}
