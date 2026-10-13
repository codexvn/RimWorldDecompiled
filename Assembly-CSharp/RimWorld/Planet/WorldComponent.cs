using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CD4 RID: 15572
	public abstract class WorldComponent : IExposable
	{
		// Token: 0x060152C3 RID: 86723 RVA: 0x006560BB File Offset: 0x006542BB
		public WorldComponent(World world)
		{
			this.world = world;
		}

		// Token: 0x060152C4 RID: 86724 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void WorldComponentUpdate()
		{
		}

		// Token: 0x060152C5 RID: 86725 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void WorldComponentTick()
		{
		}

		// Token: 0x060152C6 RID: 86726 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void WorldComponentOnGUI()
		{
		}

		// Token: 0x060152C7 RID: 86727 RVA: 0x006560CA File Offset: 0x006542CA
		public virtual void ExposeData()
		{
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060152C8 RID: 86728 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void FinalizeInit(bool fromLoad)
		{
		}

		// Token: 0x0400EC47 RID: 60487
		public World world;
	}
}
