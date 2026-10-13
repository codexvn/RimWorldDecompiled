using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200378A RID: 14218
	public class Verb_ShootOneUse : Verb_Shoot
	{
		// Token: 0x06013B24 RID: 80676 RVA: 0x005DA4C7 File Offset: 0x005D86C7
		protected override bool TryCastShot()
		{
			if (base.TryCastShot())
			{
				if (this.burstShotsLeft <= 1)
				{
					this.SelfConsume();
				}
				return true;
			}
			if (this.burstShotsLeft < base.BurstShotCount)
			{
				this.SelfConsume();
			}
			return false;
		}

		// Token: 0x06013B25 RID: 80677 RVA: 0x005DA4F7 File Offset: 0x005D86F7
		public override void Notify_EquipmentLost()
		{
			base.Notify_EquipmentLost();
			if (this.state == VerbState.Bursting && this.burstShotsLeft < base.BurstShotCount)
			{
				this.SelfConsume();
			}
		}

		// Token: 0x06013B26 RID: 80678 RVA: 0x0003A743 File Offset: 0x00038943
		private void SelfConsume()
		{
			if (base.EquipmentSource != null && !base.EquipmentSource.Destroyed)
			{
				base.EquipmentSource.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
