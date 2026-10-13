using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002497 RID: 9367
	public class PawnRecentMemory : IExposable
	{
		// Token: 0x17001EF0 RID: 7920
		// (get) Token: 0x0600CB1A RID: 51994 RVA: 0x003B7FC4 File Offset: 0x003B61C4
		public int TicksSinceLastLight
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastLightTick;
			}
		}

		// Token: 0x17001EF1 RID: 7921
		// (get) Token: 0x0600CB1B RID: 51995 RVA: 0x003B7FD7 File Offset: 0x003B61D7
		public int TicksSinceOutdoors
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastOutdoorTick;
			}
		}

		// Token: 0x0600CB1C RID: 51996 RVA: 0x003B7FEA File Offset: 0x003B61EA
		public PawnRecentMemory(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CB1D RID: 51997 RVA: 0x003B800F File Offset: 0x003B620F
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLightTick, "lastLightTick", 999999, false);
			Scribe_Values.Look<int>(ref this.lastOutdoorTick, "lastOutdoorTick", 999999, false);
		}

		// Token: 0x0600CB1E RID: 51998 RVA: 0x003B8040 File Offset: 0x003B6240
		public void RecentMemoryInterval()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			if (this.pawn.Map.glowGrid.PsychGlowAt(this.pawn.Position) != PsychGlow.Dark)
			{
				this.lastLightTick = Find.TickManager.TicksGame;
			}
			if (this.Outdoors())
			{
				this.lastOutdoorTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x0600CB1F RID: 51999 RVA: 0x003B80A8 File Offset: 0x003B62A8
		private bool Outdoors()
		{
			Room room = this.pawn.GetRoom(RegionType.Set_All);
			return room != null && room.PsychologicallyOutdoors;
		}

		// Token: 0x0600CB20 RID: 52000 RVA: 0x003B80CE File Offset: 0x003B62CE
		public void Notify_Spawned(bool respawningAfterLoad)
		{
			this.lastLightTick = Find.TickManager.TicksGame;
			if (!respawningAfterLoad && this.Outdoors())
			{
				this.lastOutdoorTick = Find.TickManager.TicksGame;
			}
		}

		// Token: 0x04008A8C RID: 35468
		private Pawn pawn;

		// Token: 0x04008A8D RID: 35469
		private int lastLightTick = 999999;

		// Token: 0x04008A8E RID: 35470
		private int lastOutdoorTick = 999999;
	}
}
