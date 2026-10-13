using System;

namespace RimWorld
{
	// Token: 0x0200299F RID: 10655
	public class Building_KidOutfitStand : Building_OutfitStand
	{
		// Token: 0x170023F4 RID: 9204
		// (get) Token: 0x0600EAA8 RID: 60072 RVA: 0x004449E7 File Offset: 0x00442BE7
		protected override BodyTypeDef BodyTypeDefForRendering { get; } = BodyTypeDefOf.Child;

		// Token: 0x170023F5 RID: 9205
		// (get) Token: 0x0600EAA9 RID: 60073 RVA: 0x00352976 File Offset: 0x00350B76
		protected override float WeaponDrawDistanceFactor
		{
			get
			{
				return 0.55f;
			}
		}
	}
}
