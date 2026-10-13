using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003790 RID: 14224
	public class TreeSighting : IExposable
	{
		// Token: 0x17003259 RID: 12889
		// (get) Token: 0x06013B2F RID: 80687 RVA: 0x005DA6B4 File Offset: 0x005D88B4
		public Thing Tree
		{
			get
			{
				return this.tree;
			}
		}

		// Token: 0x1700325A RID: 12890
		// (get) Token: 0x06013B30 RID: 80688 RVA: 0x005DA6BC File Offset: 0x005D88BC
		public int TicksSinceSighting
		{
			get
			{
				return Find.TickManager.TicksGame - this.tickSighted;
			}
		}

		// Token: 0x06013B31 RID: 80689 RVA: 0x000024DF File Offset: 0x000006DF
		public TreeSighting()
		{
		}

		// Token: 0x06013B32 RID: 80690 RVA: 0x005DA6CF File Offset: 0x005D88CF
		public TreeSighting(Thing tree, int tickSighted)
		{
			this.tree = tree;
			this.tickSighted = tickSighted;
		}

		// Token: 0x06013B33 RID: 80691 RVA: 0x005DA6E5 File Offset: 0x005D88E5
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.tickSighted, "tickSighted", 0, false);
			Scribe_References.Look<Thing>(ref this.tree, "tree", false);
		}

		// Token: 0x0400DA31 RID: 55857
		private int tickSighted;

		// Token: 0x0400DA32 RID: 55858
		public Thing tree;
	}
}
