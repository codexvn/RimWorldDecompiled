using System;
using RimWorld;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000195 RID: 405
	public class DeathActionWorker_Vanish : DeathActionWorker
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x00036188 File Offset: 0x00034388
		public DeathActionProperties_Vanish Props
		{
			get
			{
				return (DeathActionProperties_Vanish)this.props;
			}
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00036198 File Offset: 0x00034398
		public override void PawnDied(Corpse corpse, Lord prevLord)
		{
			if (this.Props.fleck != null)
			{
				FleckMaker.Static(corpse.PositionHeld, corpse.MapHeld, this.Props.fleck, 1f);
			}
			if (this.Props.filth != null)
			{
				int randomInRange = this.Props.filthCountRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					FilthMaker.TryMakeFilth(corpse.PositionHeld, corpse.MapHeld, this.Props.filth, 1, FilthSourceFlags.None, true);
				}
			}
			if (this.Props.meatExplosionSize != null && ModsConfig.AnomalyActive)
			{
				FleshbeastUtility.MeatSplatter(0, corpse.PositionHeld, corpse.MapHeld, this.Props.meatExplosionSize.Value);
			}
			corpse.Destroy(DestroyMode.Vanish);
		}
	}
}
