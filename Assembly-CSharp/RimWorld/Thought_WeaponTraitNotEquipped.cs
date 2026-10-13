using System;

namespace RimWorld
{
	// Token: 0x020018B2 RID: 6322
	public class Thought_WeaponTraitNotEquipped : Thought_WeaponTrait
	{
		// Token: 0x170015EA RID: 5610
		// (get) Token: 0x06009078 RID: 36984 RVA: 0x002A9657 File Offset: 0x002A7857
		public override bool ShouldDiscard
		{
			get
			{
				return base.ShouldDiscard || this.pawn.equipment.Primary == this.weapon;
			}
		}
	}
}
