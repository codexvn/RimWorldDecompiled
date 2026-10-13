using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030D6 RID: 12502
	public class Designator_Install : Designator_Place
	{
		// Token: 0x17002D82 RID: 11650
		// (get) Token: 0x060118E7 RID: 71911 RVA: 0x0050B1F0 File Offset: 0x005093F0
		protected Thing MiniToInstallOrBuildingToReinstall
		{
			get
			{
				Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
				if (singleSelectedThing is MinifiedThing)
				{
					return singleSelectedThing;
				}
				Building building = singleSelectedThing as Building;
				if (building != null && building.def.Minifiable)
				{
					return singleSelectedThing;
				}
				return null;
			}
		}

		// Token: 0x17002D83 RID: 11651
		// (get) Token: 0x060118E8 RID: 71912 RVA: 0x0050B22C File Offset: 0x0050942C
		protected Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		// Token: 0x17002D84 RID: 11652
		// (get) Token: 0x060118E9 RID: 71913 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool DoTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D85 RID: 11653
		// (get) Token: 0x060118EA RID: 71914 RVA: 0x0050B239 File Offset: 0x00509439
		public override BuildableDef PlacingDef
		{
			get
			{
				return this.ThingToInstall.def;
			}
		}

		// Token: 0x17002D86 RID: 11654
		// (get) Token: 0x060118EB RID: 71915 RVA: 0x0050B246 File Offset: 0x00509446
		public override string Label
		{
			get
			{
				if (this.MiniToInstallOrBuildingToReinstall is MinifiedThing)
				{
					return "CommandInstall".Translate();
				}
				return "CommandReinstall".Translate();
			}
		}

		// Token: 0x17002D87 RID: 11655
		// (get) Token: 0x060118EC RID: 71916 RVA: 0x0050B274 File Offset: 0x00509474
		public override string Desc
		{
			get
			{
				if (this.MiniToInstallOrBuildingToReinstall is MinifiedThing)
				{
					return "CommandInstallDesc".Translate();
				}
				return "CommandReinstallDesc".Translate();
			}
		}

		// Token: 0x17002D88 RID: 11656
		// (get) Token: 0x060118ED RID: 71917 RVA: 0x000440FD File Offset: 0x000422FD
		public override Color IconDrawColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17002D89 RID: 11657
		// (get) Token: 0x060118EE RID: 71918 RVA: 0x0050B2A2 File Offset: 0x005094A2
		public override bool Visible
		{
			get
			{
				return Find.Selector.SingleSelectedThing != null && base.Visible;
			}
		}

		// Token: 0x17002D8A RID: 11658
		// (get) Token: 0x060118EF RID: 71919 RVA: 0x00002C42 File Offset: 0x00000E42
		public override ThingDef StuffDef
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002D8B RID: 11659
		// (get) Token: 0x060118F0 RID: 71920 RVA: 0x0050B2B8 File Offset: 0x005094B8
		public override ThingStyleDef ThingStyleDefForPreview
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
		}

		// Token: 0x060118F1 RID: 71921 RVA: 0x0050B2CB File Offset: 0x005094CB
		public Designator_Install()
		{
			this.icon = TexCommand.Install;
			this.iconProportions = new Vector2(1f, 1f);
			this.Order = -10f;
		}

		// Token: 0x060118F2 RID: 71922 RVA: 0x0050B2FE File Offset: 0x005094FE
		public override bool CanRemainSelected()
		{
			return this.MiniToInstallOrBuildingToReinstall != null;
		}

		// Token: 0x060118F3 RID: 71923 RVA: 0x0050B30C File Offset: 0x0050950C
		public override void ProcessInput(Event ev)
		{
			Thing miniToInstallOrBuildingToReinstall = this.MiniToInstallOrBuildingToReinstall;
			if (miniToInstallOrBuildingToReinstall != null)
			{
				InstallBlueprintUtility.CancelBlueprintsFor(miniToInstallOrBuildingToReinstall);
				if (!((ThingDef)this.PlacingDef).rotatable)
				{
					this.placingRot = Rot4.North;
				}
				if (ModsConfig.AnomalyActive)
				{
					Building_HoldingPlatform building_HoldingPlatform = miniToInstallOrBuildingToReinstall as Building_HoldingPlatform;
					if (building_HoldingPlatform != null && building_HoldingPlatform.Occupied)
					{
						Messages.Message("MessageOccupiedHoldingPlatformReinstalled".Translate(), building_HoldingPlatform, MessageTypeDefOf.CautionInput, true);
					}
				}
			}
			base.ProcessInput(ev);
		}

		// Token: 0x060118F4 RID: 71924 RVA: 0x0050B388 File Offset: 0x00509588
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (!(this.MiniToInstallOrBuildingToReinstall is MinifiedThing) && c.GetThingList(base.Map).Find((Thing x) => x.Position == c && x.Rotation == this.placingRot && x.def == this.PlacingDef) != null)
			{
				return new AcceptanceReport("IdenticalThingExists".Translate());
			}
			return GenConstruct.CanPlaceBlueprintAt(this.PlacingDef, c, this.placingRot, base.Map, false, this.MiniToInstallOrBuildingToReinstall, this.ThingToInstall, null, false, false, false);
		}

		// Token: 0x060118F5 RID: 71925 RVA: 0x0050B438 File Offset: 0x00509638
		public override void DesignateSingleCell(IntVec3 c)
		{
			GenSpawn.WipeExistingThings(c, this.placingRot, this.PlacingDef.installBlueprintDef, base.Map, DestroyMode.Deconstruct);
			MinifiedThing minifiedThing = this.MiniToInstallOrBuildingToReinstall as MinifiedThing;
			if (minifiedThing != null)
			{
				GenConstruct.PlaceBlueprintForInstall(minifiedThing, c, base.Map, this.placingRot, Faction.OfPlayer, true);
			}
			else
			{
				GenConstruct.PlaceBlueprintForReinstall((Building)this.MiniToInstallOrBuildingToReinstall, c, base.Map, this.placingRot, Faction.OfPlayer, true);
			}
			FleckMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, this.placingRot, this.PlacingDef.Size), base.Map);
			Find.DesignatorManager.Deselect();
		}

		// Token: 0x060118F6 RID: 71926 RVA: 0x0050B4E0 File Offset: 0x005096E0
		protected override void DrawGhost(Color ghostCol)
		{
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null)
			{
				MeditationUtility.DrawMeditationFociAffectedByBuildingOverlay(base.Map, thingDef, Faction.OfPlayer, UI.MouseCell(), this.placingRot);
				GauranlenUtility.DrawConnectionsAffectedByBuildingOverlay(base.Map, thingDef, Faction.OfPlayer, UI.MouseCell(), this.placingRot);
				PsychicRitualUtility.DrawPsychicRitualSpotsAffectedByThingOverlay(base.Map, thingDef, UI.MouseCell(), this.placingRot);
			}
			Graphic baseGraphic = this.ThingToInstall.Graphic.ExtractInnerGraphicFor(this.ThingToInstall, null);
			GhostDrawer.DrawGhostThing(UI.MouseCell(), this.placingRot, (ThingDef)this.PlacingDef, baseGraphic, ghostCol, AltitudeLayer.Blueprint, this.ThingToInstall, true, this.StuffDef);
		}

		// Token: 0x060118F7 RID: 71927 RVA: 0x0050B597 File Offset: 0x00509797
		protected override bool CanDrawNumbersBetween(Thing thing, ThingDef def, IntVec3 a, IntVec3 b, Map map)
		{
			return this.ThingToInstall != thing && !GenThing.CloserThingBetween(def, a, b, map, this.ThingToInstall);
		}

		// Token: 0x060118F8 RID: 71928 RVA: 0x0050B5B8 File Offset: 0x005097B8
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.PlacingDef, this.placingRot);
		}
	}
}
