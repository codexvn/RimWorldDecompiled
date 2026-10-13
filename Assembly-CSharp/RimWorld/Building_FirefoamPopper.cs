using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002975 RID: 10613
	public class Building_FirefoamPopper : Building
	{
		// Token: 0x17002385 RID: 9093
		// (get) Token: 0x0600E912 RID: 59666 RVA: 0x0043DA9B File Offset: 0x0043BC9B
		private bool CanSetAutoRebuild
		{
			get
			{
				return base.Faction == Faction.OfPlayer && this.def.blueprintDef != null && this.def.IsResearchFinished;
			}
		}

		// Token: 0x0600E913 RID: 59667 RVA: 0x0043DAC4 File Offset: 0x0043BCC4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				this.autoRebuild = (this.CanSetAutoRebuild && map.areaManager.Home[base.Position]);
			}
		}

		// Token: 0x0600E914 RID: 59668 RVA: 0x0043DB00 File Offset: 0x0043BD00
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (mode != DestroyMode.Deconstruct && this.autoRebuild && map != null && this.CanSetAutoRebuild && GenConstruct.CanPlaceBlueprintAt(this.def, base.Position, base.Rotation, map, false, null, null, null, false, false, false).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(this.def, base.Position, map, base.Rotation, Faction.OfPlayer, base.Stuff, null, null, true);
			}
		}

		// Token: 0x0600E915 RID: 59669 RVA: 0x0043DB82 File Offset: 0x0043BD82
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.CanSetAutoRebuild)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandAutoRebuild_Building".Translate(),
					defaultDesc = "CommandAutoRebuild_BuildingDesc".Translate(this.Named("BUILDING")),
					hotKey = KeyBindingDefOf.Misc3,
					icon = Building_FirefoamPopper.RebuildCommandTex.Texture,
					isActive = (() => this.autoRebuild),
					toggleAction = delegate()
					{
						this.autoRebuild = !this.autoRebuild;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600E916 RID: 59670 RVA: 0x0043DB92 File Offset: 0x0043BD92
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.autoRebuild, "autoRebuild", false, false);
		}

		// Token: 0x04009B42 RID: 39746
		private bool autoRebuild;

		// Token: 0x04009B43 RID: 39747
		private static readonly CachedTexture RebuildCommandTex = new CachedTexture("UI/Commands/RearmFirefoamPopper");
	}
}
