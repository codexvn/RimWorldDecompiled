using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E37 RID: 15927
	public class TimeoutComp : WorldObjectComp
	{
		// Token: 0x1700365B RID: 13915
		// (get) Token: 0x06015C34 RID: 89140 RVA: 0x00681363 File Offset: 0x0067F563
		public bool Active
		{
			get
			{
				return this.timeoutEndTick != -1;
			}
		}

		// Token: 0x1700365C RID: 13916
		// (get) Token: 0x06015C35 RID: 89141 RVA: 0x00681371 File Offset: 0x0067F571
		public bool Passed
		{
			get
			{
				return this.Active && Find.TickManager.TicksGame >= this.timeoutEndTick;
			}
		}

		// Token: 0x1700365D RID: 13917
		// (get) Token: 0x06015C36 RID: 89142 RVA: 0x00681392 File Offset: 0x0067F592
		private bool ShouldRemoveWorldObjectNow
		{
			get
			{
				return this.Passed && !base.ParentHasMap;
			}
		}

		// Token: 0x1700365E RID: 13918
		// (get) Token: 0x06015C37 RID: 89143 RVA: 0x006813A7 File Offset: 0x0067F5A7
		public int TicksLeft
		{
			get
			{
				if (!this.Active)
				{
					return 0;
				}
				return this.timeoutEndTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x06015C38 RID: 89144 RVA: 0x006813C4 File Offset: 0x0067F5C4
		public void StartTimeout(int ticks)
		{
			this.timeoutEndTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06015C39 RID: 89145 RVA: 0x006813D8 File Offset: 0x0067F5D8
		public void StopTimeout()
		{
			this.timeoutEndTick = -1;
		}

		// Token: 0x06015C3A RID: 89146 RVA: 0x006813E1 File Offset: 0x0067F5E1
		public override void CompTickInterval(int delta)
		{
			base.CompTickInterval(delta);
			if (this.ShouldRemoveWorldObjectNow)
			{
				this.parent.Destroy();
			}
		}

		// Token: 0x06015C3B RID: 89147 RVA: 0x006813FD File Offset: 0x0067F5FD
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timeoutEndTick, "timeoutEndTick", 0, false);
		}

		// Token: 0x06015C3C RID: 89148 RVA: 0x00681417 File Offset: 0x0067F617
		public override string CompInspectStringExtra()
		{
			if (this.Active && !base.ParentHasMap)
			{
				return "WorldObjectTimeout".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
			return null;
		}

		// Token: 0x0400F168 RID: 61800
		private int timeoutEndTick = -1;
	}
}
