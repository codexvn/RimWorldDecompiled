using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B48 RID: 11080
	[StaticConstructorOnStartup]
	public class DropPodIncoming : Skyfaller, IActiveTransporter, IThingHolder
	{
		// Token: 0x170026DF RID: 9951
		// (get) Token: 0x0600F7B2 RID: 63410 RVA: 0x00480442 File Offset: 0x0047E642
		// (set) Token: 0x0600F7B3 RID: 63411 RVA: 0x0048045A File Offset: 0x0047E65A
		public ActiveTransporterInfo Contents
		{
			get
			{
				return ((ActiveTransporter)this.innerContainer[0]).Contents;
			}
			set
			{
				((ActiveTransporter)this.innerContainer[0]).Contents = value;
			}
		}

		// Token: 0x0600F7B4 RID: 63412 RVA: 0x00480474 File Offset: 0x0047E674
		protected override void SpawnThings()
		{
			if (this.Contents.spawnWipeMode == null)
			{
				base.SpawnThings();
				return;
			}
			for (int i = this.innerContainer.Count - 1; i >= 0; i--)
			{
				GenSpawn.Spawn(this.innerContainer[i], base.Position, base.Map, this.Contents.spawnWipeMode.Value);
			}
		}

		// Token: 0x0600F7B5 RID: 63413 RVA: 0x004804E0 File Offset: 0x0047E6E0
		protected override void Impact()
		{
			for (int i = 0; i < 6; i++)
			{
				FleckMaker.ThrowDustPuff(base.Position.ToVector3Shifted() + Gen.RandomHorizontalVector(1f), base.Map, 1.2f);
			}
			FleckMaker.ThrowLightningGlow(base.Position.ToVector3Shifted(), base.Map, 2f);
			GenClamor.DoClamor(this, 15f, ClamorDefOf.Impact);
			base.Impact();
		}
	}
}
