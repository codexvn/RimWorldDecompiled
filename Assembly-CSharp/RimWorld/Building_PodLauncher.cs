using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A44 RID: 10820
	[StaticConstructorOnStartup]
	public class Building_PodLauncher : Building, INotifyLaunchableLaunch
	{
		// Token: 0x0600F042 RID: 61506 RVA: 0x0045AF03 File Offset: 0x00459103
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Designator_Build designator_Build = BuildCopyCommandUtility.FindAllowedDesignator(ThingDefOf.TransportPod, true);
			if (designator_Build != null)
			{
				AcceptanceReport acceptanceReport = GenConstruct.CanPlaceBlueprintAt(ThingDefOf.TransportPod, FuelingPortUtility.GetFuelingPortCell(this), ThingDefOf.TransportPod.defaultPlacingRot, base.Map, false, null, null, null, false, false, false);
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "BuildThing".Translate(ThingDefOf.TransportPod.label),
					icon = designator_Build.icon,
					defaultDesc = designator_Build.Desc,
					action = delegate()
					{
						IntVec3 fuelingPortCell = FuelingPortUtility.GetFuelingPortCell(this);
						GenConstruct.PlaceBlueprintForBuild(ThingDefOf.TransportPod, fuelingPortCell, base.Map, ThingDefOf.TransportPod.defaultPlacingRot, Faction.OfPlayer, null, null, null, true);
					}
				};
				if (!acceptanceReport.Accepted)
				{
					command_Action.Disable(acceptanceReport.Reason);
				}
				yield return command_Action;
			}
			yield return new Command_Toggle
			{
				icon = Building_PodLauncher.AutoBuildIcon,
				defaultLabel = "AutoBuildTransportPod".Translate(),
				defaultDesc = "AutoBuildTransportPodDesc".Translate(),
				isActive = (() => this.autoPlacePods),
				toggleAction = new Action(this.ToggleAutoBuildTransportPods)
			};
			yield break;
			yield break;
		}

		// Token: 0x0600F043 RID: 61507 RVA: 0x0045AF13 File Offset: 0x00459113
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.autoPlacePods, "autoPlacePods", false, false);
		}

		// Token: 0x0600F044 RID: 61508 RVA: 0x0045AF2D File Offset: 0x0045912D
		private void ToggleAutoBuildTransportPods()
		{
			this.autoPlacePods = !this.autoPlacePods;
			if (this.autoPlacePods)
			{
				this.CheckPlacePod();
			}
		}

		// Token: 0x0600F045 RID: 61509 RVA: 0x0045AF4C File Offset: 0x0045914C
		private void CheckPlacePod()
		{
			if (!this.autoPlacePods)
			{
				return;
			}
			IntVec3 fuelingPortCell = FuelingPortUtility.GetFuelingPortCell(this);
			if (!GenConstruct.CanPlaceBlueprintAt(ThingDefOf.TransportPod, fuelingPortCell, ThingDefOf.TransportPod.defaultPlacingRot, base.Map, false, null, null, null, false, false, false).Accepted)
			{
				return;
			}
			GenConstruct.PlaceBlueprintForBuild(ThingDefOf.TransportPod, fuelingPortCell, base.Map, ThingDefOf.TransportPod.defaultPlacingRot, Faction.OfPlayer, null, null, null, true);
		}

		// Token: 0x0600F046 RID: 61510 RVA: 0x0045AFBB File Offset: 0x004591BB
		public void Notify_LaunchableLaunched(CompLaunchable launchable)
		{
			this.CheckPlacePod();
		}

		// Token: 0x04009E9D RID: 40605
		public bool autoPlacePods;

		// Token: 0x04009E9E RID: 40606
		private static readonly Texture2D AutoBuildIcon = ContentFinder<Texture2D>.Get("UI/Commands/AutoBuildTransportPod", true);
	}
}
