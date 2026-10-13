using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002951 RID: 10577
	public class Blueprint_Install : Blueprint
	{
		// Token: 0x17002338 RID: 9016
		// (get) Token: 0x0600E7CD RID: 59341 RVA: 0x00438884 File Offset: 0x00436A84
		public Thing MiniToInstallOrBuildingToReinstall
		{
			get
			{
				if (this.miniToInstall != null)
				{
					return this.miniToInstall;
				}
				if (this.buildingToReinstall != null)
				{
					return this.buildingToReinstall;
				}
				throw new InvalidOperationException("Nothing to install.");
			}
		}

		// Token: 0x17002339 RID: 9017
		// (get) Token: 0x0600E7CE RID: 59342 RVA: 0x004388AE File Offset: 0x00436AAE
		public Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		// Token: 0x1700233A RID: 9018
		// (get) Token: 0x0600E7CF RID: 59343 RVA: 0x004388BB File Offset: 0x00436ABB
		// (set) Token: 0x0600E7D0 RID: 59344 RVA: 0x004388CE File Offset: 0x00436ACE
		public override ThingStyleDef StyleDef
		{
			get
			{
				Thing thingToInstall = this.ThingToInstall;
				if (thingToInstall == null)
				{
					return null;
				}
				return thingToInstall.StyleDef;
			}
			set
			{
				this.ThingToInstall.StyleDef = value;
			}
		}

		// Token: 0x1700233B RID: 9019
		// (get) Token: 0x0600E7D1 RID: 59345 RVA: 0x004388DC File Offset: 0x00436ADC
		protected override float WorkTotal
		{
			get
			{
				return (float)((this.ThingToInstall.def.plant != null) ? -1 : 150);
			}
		}

		// Token: 0x0600E7D2 RID: 59346 RVA: 0x004388F9 File Offset: 0x00436AF9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MinifiedThing>(ref this.miniToInstall, "miniToInstall", false);
			Scribe_References.Look<Building>(ref this.buildingToReinstall, "buildingToReinstall", false);
		}

		// Token: 0x0600E7D3 RID: 59347 RVA: 0x00438923 File Offset: 0x00436B23
		public override BuildableDef EntityToBuild()
		{
			Thing thingToInstall = this.ThingToInstall;
			if (thingToInstall == null)
			{
				return null;
			}
			return thingToInstall.def;
		}

		// Token: 0x0600E7D4 RID: 59348 RVA: 0x004388BB File Offset: 0x00436ABB
		public override ThingStyleDef EntityToBuildStyle()
		{
			Thing thingToInstall = this.ThingToInstall;
			if (thingToInstall == null)
			{
				return null;
			}
			return thingToInstall.StyleDef;
		}

		// Token: 0x0600E7D5 RID: 59349 RVA: 0x00438936 File Offset: 0x00436B36
		public override ThingDef EntityToBuildStuff()
		{
			Thing thingToInstall = this.ThingToInstall;
			if (thingToInstall == null)
			{
				return null;
			}
			return thingToInstall.Stuff;
		}

		// Token: 0x0600E7D6 RID: 59350 RVA: 0x00438949 File Offset: 0x00436B49
		public override List<ThingDefCountClass> TotalMaterialCost()
		{
			Log.Error("Called MaterialsNeededTotal on a Blueprint_Install.");
			return new List<ThingDefCountClass>();
		}

		// Token: 0x0600E7D7 RID: 59351 RVA: 0x0043895C File Offset: 0x00436B5C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			map.listerBuildings.RegisterInstallBlueprint(this);
			if (this.buildingToReinstall != null)
			{
				this.overrideGraphicIndex = new int?(this.buildingToReinstall.overrideGraphicIndex ?? this.buildingToReinstall.thingIDNumber);
			}
			else if (this.miniToInstall != null)
			{
				this.overrideGraphicIndex = new int?(this.miniToInstall.InnerThing.overrideGraphicIndex ?? this.miniToInstall.InnerThing.thingIDNumber);
			}
			if (!respawningAfterLoad && !this.MiniToInstallOrBuildingToReinstall.BeingTransportedOnGravship && (this.MiniToInstallOrBuildingToReinstall.Destroyed || !this.MiniToInstallOrBuildingToReinstall.SpawnedOrAnyParentSpawned))
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600E7D8 RID: 59352 RVA: 0x00438A32 File Offset: 0x00436C32
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Map.listerBuildings.DeregisterInstallBlueprint(this);
			base.DeSpawn(mode);
		}

		// Token: 0x0600E7D9 RID: 59353 RVA: 0x00438A4C File Offset: 0x00436C4C
		protected override Thing MakeSolidThing(out bool shouldSelect)
		{
			Thing thingToInstall = this.ThingToInstall;
			if (this.miniToInstall != null)
			{
				foreach (Designation designation in base.Map.designationManager.AllDesignationsOn(this.miniToInstall))
				{
					Designation newDes = new Designation(thingToInstall, designation.def, designation.colorDef);
					base.Map.designationManager.AddDesignation(newDes);
				}
				shouldSelect = Find.Selector.IsSelected(this.miniToInstall);
				this.miniToInstall.InnerThing = null;
				this.miniToInstall.Destroy(DestroyMode.Vanish);
			}
			else
			{
				shouldSelect = false;
			}
			return thingToInstall;
		}

		// Token: 0x0600E7DA RID: 59354 RVA: 0x00438B14 File Offset: 0x00436D14
		public override bool TryReplaceWithSolidThing(Pawn workerPawn, out Thing createdThing, out bool jobEnded)
		{
			Map map = base.Map;
			bool flag = base.TryReplaceWithSolidThing(workerPawn, out createdThing, out jobEnded);
			if (flag)
			{
				if (this.overrideGraphicIndex != null)
				{
					createdThing.overrideGraphicIndex = this.overrideGraphicIndex;
				}
				if (createdThing is Plant)
				{
					SoundDefOf.Replant_Complete.PlayOneShot(new TargetInfo(base.Position, map, false));
				}
				else
				{
					SoundDefOf.Building_Complete.PlayOneShot(new TargetInfo(base.Position, map, false));
				}
				workerPawn.records.Increment(RecordDefOf.ThingsInstalled);
			}
			return flag;
		}

		// Token: 0x0600E7DB RID: 59355 RVA: 0x00438BA1 File Offset: 0x00436DA1
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Command command = BuildCopyCommandUtility.BuildCopyCommand(this.ThingToInstall.def, this.ThingToInstall.Stuff, this.ThingToInstall.StyleSourcePrecept as Precept_Building, this.StyleDef, true, null);
			if (command != null)
			{
				yield return command;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				foreach (Command command2 in BuildRelatedCommandUtility.RelatedBuildCommands(this.ThingToInstall.def))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600E7DC RID: 59356 RVA: 0x00438BB4 File Offset: 0x00436DB4
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.buildingToReinstall != null)
			{
				GenDraw.DrawLineBetween(this.buildingToReinstall.TrueCenter(), this.TrueCenter());
			}
			if (this.ThingToInstall.def.drawPlaceWorkersWhileInstallBlueprintSelected && this.ThingToInstall.def.PlaceWorkers != null)
			{
				List<PlaceWorker> placeWorkers = this.ThingToInstall.def.PlaceWorkers;
				for (int i = 0; i < placeWorkers.Count; i++)
				{
					placeWorkers[i].DrawGhost(this.ThingToInstall.def, base.Position, base.Rotation, Color.white, this.ThingToInstall);
				}
			}
		}

		// Token: 0x0600E7DD RID: 59357 RVA: 0x00438C59 File Offset: 0x00436E59
		internal void SetThingToInstallFromMinified(MinifiedThing itemToInstall)
		{
			this.miniToInstall = itemToInstall;
			this.buildingToReinstall = null;
		}

		// Token: 0x0600E7DE RID: 59358 RVA: 0x00438C69 File Offset: 0x00436E69
		internal void SetBuildingToReinstall(Building buildingToReinstall)
		{
			if (!buildingToReinstall.def.Minifiable)
			{
				Log.Error("Tried to reinstall non-minifiable building.");
				return;
			}
			this.miniToInstall = null;
			this.buildingToReinstall = buildingToReinstall;
		}

		// Token: 0x04009AB3 RID: 39603
		private MinifiedThing miniToInstall;

		// Token: 0x04009AB4 RID: 39604
		private Building buildingToReinstall;
	}
}
