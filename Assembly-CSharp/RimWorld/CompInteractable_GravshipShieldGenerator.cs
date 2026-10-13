using System;
using System.Collections.Generic;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002CC3 RID: 11459
	public class CompInteractable_GravshipShieldGenerator : CompInteractable
	{
		// Token: 0x170028B4 RID: 10420
		// (get) Token: 0x0601015C RID: 65884 RVA: 0x004A7551 File Offset: 0x004A5751
		private CompProjectileInterceptor Shield
		{
			get
			{
				return this.shield ?? this.parent.TryGetComp<CompProjectileInterceptor>();
			}
		}

		// Token: 0x170028B5 RID: 10421
		// (get) Token: 0x0601015D RID: 65885 RVA: 0x004A7568 File Offset: 0x004A5768
		private CompGravshipFacility Facility
		{
			get
			{
				return this.facility ?? this.parent.TryGetComp<CompGravshipFacility>();
			}
		}

		// Token: 0x170028B6 RID: 10422
		// (get) Token: 0x0601015E RID: 65886 RVA: 0x004A757F File Offset: 0x004A577F
		protected override string ActivateOptionLabel
		{
			get
			{
				return this.Shield.Active ? "Deactivate".Translate() : "Activate".Translate();
			}
		}

		// Token: 0x0601015F RID: 65887 RVA: 0x004A75AC File Offset: 0x004A57AC
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (this.Facility.LinkedBuildings.NullOrEmpty<Thing>())
			{
				return "ShieldNotConnectedToGravship".Translate();
			}
			if (this.Shield.Charging)
			{
				return "ShieldOnCooldown".Translate();
			}
			if (!this.Shield.Active && this.Shield.currentHitPoints < 100)
			{
				return "ShieldBelowMinHitpoints".Translate();
			}
			return base.CanInteract(activateBy, checkOptionalItems);
		}

		// Token: 0x06010160 RID: 65888 RVA: 0x004A763A File Offset: 0x004A583A
		protected override void OnInteracted(Pawn caster)
		{
			if (!this.Shield.Active)
			{
				this.Shield.Activate();
				return;
			}
			this.Shield.Deactivate();
		}

		// Token: 0x06010161 RID: 65889 RVA: 0x004A7660 File Offset: 0x004A5860
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!this.parent.SpawnedOrAnyParentSpawned)
			{
				yield break;
			}
			string defaultLabel;
			string defaultDesc;
			if (!this.Shield.Active)
			{
				defaultLabel = "GravshipShieldOrderActivation".Translate() + "...";
				defaultDesc = "GravshipShieldOrderActivationDesc".Translate(this.parent.Named("THING"));
			}
			else
			{
				defaultLabel = "GravshipShieldOrderDeactivation".Translate() + "...";
				defaultDesc = "GravshipShieldOrderDeactivationDesc".Translate(this.parent.Named("THING"));
			}
			Command_Action command_Action = new Command_Action
			{
				defaultLabel = defaultLabel,
				defaultDesc = defaultDesc,
				icon = base.UIIcon,
				groupable = false,
				action = delegate()
				{
					SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
					Find.Targeter.BeginTargeting(this, null, false, null, null, true);
				}
			};
			AcceptanceReport acceptanceReport = this.CanInteract(null, true);
			if (!acceptanceReport.Accepted)
			{
				command_Action.Disable(acceptanceReport.Reason.CapitalizeFirst());
			}
			yield return command_Action;
			yield break;
		}

		// Token: 0x0400A80C RID: 43020
		private const int MinHitPointsToActivate = 100;

		// Token: 0x0400A80D RID: 43021
		private CompProjectileInterceptor shield;

		// Token: 0x0400A80E RID: 43022
		private CompGravshipFacility facility;
	}
}
