using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EDB RID: 11995
	public abstract class CompDissolutionEffect : ThingComp
	{
		// Token: 0x06010F08 RID: 69384 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DoDissolutionEffectMap(int amount)
		{
		}

		// Token: 0x06010F09 RID: 69385 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DoDissolutionEffectWorld(int amount, PlanetTile tile)
		{
		}

		// Token: 0x06010F0A RID: 69386 RVA: 0x004DFE9F File Offset: 0x004DE09F
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Dissolution effect"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}
	}
}
