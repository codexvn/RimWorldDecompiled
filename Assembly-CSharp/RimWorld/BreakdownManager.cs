using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C2F RID: 11311
	public class BreakdownManager : MapComponent
	{
		// Token: 0x0600FDA5 RID: 64933 RVA: 0x004991BB File Offset: 0x004973BB
		public BreakdownManager(Map map) : base(map)
		{
		}

		// Token: 0x0600FDA6 RID: 64934 RVA: 0x004991DA File Offset: 0x004973DA
		public void Register(CompBreakdownable c)
		{
			this.comps.Add(c);
			if (c.BrokenDown)
			{
				this.brokenDownThings.Add(c.parent);
			}
		}

		// Token: 0x0600FDA7 RID: 64935 RVA: 0x00499202 File Offset: 0x00497402
		public void Deregister(CompBreakdownable c)
		{
			this.comps.Remove(c);
			this.brokenDownThings.Remove(c.parent);
		}

		// Token: 0x0600FDA8 RID: 64936 RVA: 0x00499224 File Offset: 0x00497424
		public override void MapComponentTick()
		{
			if (Find.TickManager.TicksGame % 1041 == 0)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CheckForBreakdown();
				}
			}
		}

		// Token: 0x0600FDA9 RID: 64937 RVA: 0x0049926A File Offset: 0x0049746A
		public void Notify_BrokenDown(Thing thing)
		{
			this.brokenDownThings.Add(thing);
		}

		// Token: 0x0600FDAA RID: 64938 RVA: 0x00499279 File Offset: 0x00497479
		public void Notify_Repaired(Thing thing)
		{
			this.brokenDownThings.Remove(thing);
		}

		// Token: 0x0400A62E RID: 42542
		private List<CompBreakdownable> comps = new List<CompBreakdownable>();

		// Token: 0x0400A62F RID: 42543
		public HashSet<Thing> brokenDownThings = new HashSet<Thing>();

		// Token: 0x0400A630 RID: 42544
		public const int CheckIntervalTicks = 1041;
	}
}
