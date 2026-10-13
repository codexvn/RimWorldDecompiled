using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CFF RID: 11519
	public class CompInteractableFoamTurret : CompInteractable
	{
		// Token: 0x1700290D RID: 10509
		// (get) Token: 0x060102FC RID: 66300 RVA: 0x004AE907 File Offset: 0x004ACB07
		private Building_TurretGun ParentGun
		{
			get
			{
				return (Building_TurretGun)this.parent;
			}
		}

		// Token: 0x1700290E RID: 10510
		// (get) Token: 0x060102FD RID: 66301 RVA: 0x004AE914 File Offset: 0x004ACB14
		public override bool CanCooldown
		{
			get
			{
				return this.power == null || this.power.PowerOn;
			}
		}

		// Token: 0x060102FE RID: 66302 RVA: 0x004AE92C File Offset: 0x004ACB2C
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
			if (!result.Accepted)
			{
				return result;
			}
			if (!this.ParentGun.TryFindNewTarget().IsValid)
			{
				return "NoNearbyFire".Translate();
			}
			return true;
		}

		// Token: 0x060102FF RID: 66303 RVA: 0x004AE978 File Offset: 0x004ACB78
		protected override void SendDeactivateMessage()
		{
			Messages.Message("MessageActivationCanceled".Translate(this.parent) + ": " + "NoNearbyFireNoFuelUsed".Translate(), this.parent, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06010300 RID: 66304 RVA: 0x0049C8A8 File Offset: 0x0049AAA8
		protected override bool ShouldDeactivate()
		{
			return !this.CanInteract(null, true);
		}

		// Token: 0x06010301 RID: 66305 RVA: 0x004AE9D0 File Offset: 0x004ACBD0
		protected override bool TryInteractTick()
		{
			this.ParentGun.TryActivateBurst();
			if (this.ParentGun.CurrentTarget.IsValid)
			{
				this.ParentGun.Top.ForceFaceTarget(this.ParentGun.CurrentTarget);
				return true;
			}
			return false;
		}
	}
}
