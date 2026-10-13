using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030AC RID: 12460
	public class Designator_MoveGravship : Designator
	{
		// Token: 0x17002D49 RID: 11593
		// (get) Token: 0x060117D9 RID: 71641 RVA: 0x00505DE8 File Offset: 0x00503FE8
		public override string Label
		{
			get
			{
				return "DesignatorMoveGravship".Translate();
			}
		}

		// Token: 0x17002D4A RID: 11594
		// (get) Token: 0x060117DA RID: 71642 RVA: 0x00505DF9 File Offset: 0x00503FF9
		public override string Desc
		{
			get
			{
				return "DesignatorMoveGravshipDesc".Translate();
			}
		}

		// Token: 0x17002D4B RID: 11595
		// (get) Token: 0x060117DB RID: 71643 RVA: 0x00182697 File Offset: 0x00180897
		public override float Order
		{
			get
			{
				return -100f;
			}
		}

		// Token: 0x17002D4C RID: 11596
		// (get) Token: 0x060117DC RID: 71644 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AlwaysDoGuiControls
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002D4D RID: 11597
		// (get) Token: 0x060117DD RID: 71645 RVA: 0x00505E0A File Offset: 0x0050400A
		public float PaneTopY
		{
			get
			{
				return (float)(UI.screenHeight - 35);
			}
		}

		// Token: 0x17002D4E RID: 11598
		// (get) Token: 0x060117DE RID: 71646 RVA: 0x00505E15 File Offset: 0x00504015
		public IntVec3 AdjustedMouseCell
		{
			get
			{
				return this.GetSizeRotAdjustedCell(UI.MouseCell());
			}
		}

		// Token: 0x060117DF RID: 71647 RVA: 0x00505E24 File Offset: 0x00504024
		public Designator_MoveGravship(Map map, GravshipLandingMarker marker)
		{
			this.marker = marker;
			this.map = (map ?? marker.Map);
		}

		// Token: 0x060117E0 RID: 71648 RVA: 0x00505E70 File Offset: 0x00504070
		private IntVec3 GetSizeRotAdjustedCell(IntVec3 cell)
		{
			IntVec3 a = new IntVec3(this.marker.gravship.Bounds.Size.x / 2, 0, 0);
			IntVec3 b = new IntVec3(0, 0, this.marker.gravship.Bounds.Size.z / 2);
			if (this.marker.GravshipRotation == Rot4.North)
			{
				cell += a + b;
			}
			else if (this.marker.GravshipRotation == Rot4.East)
			{
				cell += a - b;
			}
			else if (this.marker.GravshipRotation == Rot4.South)
			{
				cell += -a - b;
			}
			else if (this.marker.GravshipRotation == Rot4.West)
			{
				cell += -a + b;
			}
			return cell;
		}

		// Token: 0x060117E1 RID: 71649 RVA: 0x00505F79 File Offset: 0x00504179
		public override AcceptanceReport CanDesignateCell(IntVec3 loc)
		{
			return this.ValidGravshipLocation(loc, null, null);
		}

		// Token: 0x060117E2 RID: 71650 RVA: 0x00505F84 File Offset: 0x00504184
		private AcceptanceReport ValidGravshipLocation(IntVec3 loc, List<IntVec3> validCells = null, List<IntVec3> invalidCells = null)
		{
			Map currentMap = Find.CurrentMap;
			IntVec3 root = PrefabUtility.GetRoot(this.GetSizeRotAdjustedCell(loc), this.marker.gravship.Bounds.Size, this.marker.GravshipRotation);
			bool flag = false;
			AcceptanceReport result = true;
			foreach (IntVec3 b in this.marker.GravshipCells)
			{
				IntVec3 intVec = root + b;
				if (!flag && intVec.InBounds(currentMap) && intVec.GetTerrain(currentMap) != TerrainDefOf.Space)
				{
					flag = true;
				}
				AcceptanceReport acceptanceReport = Designator_MoveGravship.IsValidCell(intVec, currentMap);
				if (acceptanceReport.Accepted)
				{
					if (validCells != null)
					{
						validCells.Add(intVec);
					}
				}
				else
				{
					result = acceptanceReport;
					if (invalidCells != null)
					{
						invalidCells.Add(intVec);
					}
					if (validCells == null && invalidCells == null)
					{
						return acceptanceReport;
					}
				}
			}
			if (!flag)
			{
				if (invalidCells != null)
				{
					invalidCells.AddRange(validCells);
				}
				if (validCells != null)
				{
					validCells.Clear();
				}
				return "GravshipMustBeConnectedToLand".Translate();
			}
			return result;
		}

		// Token: 0x060117E3 RID: 71651 RVA: 0x005060A4 File Offset: 0x005042A4
		private static AcceptanceReport IsValidCell(IntVec3 cell, Map map)
		{
			if (!cell.InBounds(map))
			{
				return "GravshipOutOfBounds".Translate();
			}
			if (!cell.InBounds(map, 1) || cell.InNoBuildEdgeArea(map))
			{
				return "GravshipInNoBuildArea".Translate();
			}
			if (map.landingBlockers != null)
			{
				foreach (CellRect cellRect in map.landingBlockers)
				{
					if (cellRect.Contains(cell))
					{
						return "GravshipInBlockedArea".Translate();
					}
				}
			}
			if (cell.Roofed(map))
			{
				return "GravshipBlockedByRoof".Translate();
			}
			if (cell.Fogged(map))
			{
				return "GravshipBlockedByFog".Translate();
			}
			foreach (Thing thing in cell.GetThingList(map))
			{
				if (!thing.def.preventGravshipLandingOn)
				{
					BuildingProperties building = thing.def.building;
					if (building == null || building.canLandGravshipOn)
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null && (pawn.RaceProps.Humanlike || pawn.HostileTo(Faction.OfPlayer)))
						{
							return "GravshipBlockedBy".Translate(pawn);
						}
						continue;
					}
				}
				return "GravshipBlockedBy".Translate(thing);
			}
			if (!GenConstruct.CanBuildOnTerrain(TerrainDefOf.Substructure, cell, map, Rot4.North, null, null))
			{
				return "GravshipBlockedByTerrain".Translate(cell.GetTerrain(map));
			}
			return true;
		}

		// Token: 0x060117E4 RID: 71652 RVA: 0x00506288 File Offset: 0x00504488
		public override void Selected()
		{
			if ((this.marker.Spawned && Find.CurrentMap != this.marker.Map) || Find.World.renderer.wantedMode == WorldRenderMode.Planet)
			{
				CameraJumper.TryJump(this.marker, CameraJumper.MovementMode.Pan);
			}
			this.deselectedRotation = this.marker.GravshipRotation;
		}

		// Token: 0x060117E5 RID: 71653 RVA: 0x005062E8 File Offset: 0x005044E8
		public override void DesignateSingleCell(IntVec3 c)
		{
			this.marker.Position = PrefabUtility.GetRoot(this.GetSizeRotAdjustedCell(c), this.marker.gravship.Bounds.Size, this.marker.GravshipRotation);
			if (!this.marker.Spawned)
			{
				GenSpawn.Spawn(this.marker, this.marker.Position, this.map, WipeMode.Vanish);
			}
			this.marker.Notify_Moved();
			this.deselectedRotation = this.marker.GravshipRotation;
			Find.DesignatorManager.Deselect();
		}

		// Token: 0x060117E6 RID: 71654 RVA: 0x00506380 File Offset: 0x00504580
		public override void Deselected()
		{
			this.marker.GravshipRotation = this.deselectedRotation;
		}

		// Token: 0x060117E7 RID: 71655 RVA: 0x00506394 File Offset: 0x00504594
		public override void DrawMouseAttachments()
		{
			if (!this.lastAcceptanceReport.Accepted)
			{
				Texture iconTex = null;
				string reason = this.lastAcceptanceReport.Reason;
				float angle = 0f;
				Color? textColor = new Color?(ColorLibrary.RedReadable);
				Color textBgColor = new Color(0f, 0f, 0f, 0.5f);
				GenUI.DrawMouseAttachment(iconTex, reason, angle, default(Vector2), null, textColor, true, textBgColor, null, null);
			}
		}

		// Token: 0x060117E8 RID: 71656 RVA: 0x0050640C File Offset: 0x0050460C
		public override void SelectedUpdate()
		{
			this.tmpValidGravshipCells.Clear();
			this.tmpInvalidGravshipCells.Clear();
			this.lastAcceptanceReport = this.ValidGravshipLocation(UI.MouseCell(), this.tmpValidGravshipCells, this.tmpInvalidGravshipCells);
			this.tmpThrusterCells.Clear();
			this.tmpThrusterCells.AddRange(this.marker.ThrusterCells);
			IntVec3 root = PrefabUtility.GetRoot(this.GetSizeRotAdjustedCell(UI.MouseCell()), this.marker.gravship.Bounds.Size, this.marker.GravshipRotation);
			for (int i = 0; i < this.tmpThrusterCells.Count; i++)
			{
				List<IntVec3> list = this.tmpThrusterCells;
				int index = i;
				list[index] += root;
			}
			GenDraw.DrawFieldEdges(this.tmpValidGravshipCells, ColorLibrary.White, null, null, 2900);
			GenDraw.DrawFieldEdges(this.tmpInvalidGravshipCells, ColorLibrary.Red, null, null, 2900);
			GenDraw.DrawDiagonalStripes(this.tmpValidGravshipCells, new Color?(Designator_MoveGravship.diagonalsColorValid), null, 2900);
			GenDraw.DrawDiagonalStripes(this.tmpInvalidGravshipCells, new Color?(Designator_MoveGravship.diagonalsColorInvalid), null, 2900);
			GenDraw.DrawFieldEdges(this.tmpThrusterCells, ColorLibrary.Orange, null, null, 2900);
			foreach (KeyValuePair<Thing, PositionData.Data> keyValuePair in this.marker.gravship.ExteriorDoorPlacements)
			{
				Thing thing;
				PositionData.Data data;
				keyValuePair.Deconstruct(out thing, out data);
				Thing thing2 = thing;
				PositionData.Data data2 = data;
				GhostUtility.GhostGraphicFor(thing2.Graphic, thing2.def, Color.white, null).DrawFromDef(GenThing.TrueCenter(root + data2.local, data2.rotation, thing2.def.Size, AltitudeLayer.MetaOverlays.AltitudeFor()), data2.rotation, thing2.def, 0f);
			}
		}

		// Token: 0x060117E9 RID: 71657 RVA: 0x00506640 File Offset: 0x00504840
		public override void DoExtraGuiControls(float leftX, float bottomY)
		{
			DesignatorUtility.GUIDoRotationControls(leftX, bottomY, this.marker.GravshipRotation, delegate(Rot4 rot)
			{
				this.marker.GravshipRotation = rot;
			});
		}

		// Token: 0x060117EA RID: 71658 RVA: 0x00506660 File Offset: 0x00504860
		public override void SelectedProcessInput(Event ev)
		{
			RotationDirection rotationDirection = RotationDirection.None;
			if (Event.current.button == 2)
			{
				if (Event.current.type == EventType.MouseDown)
				{
					Event.current.Use();
					Designator_MoveGravship.middleMouseDownTime = Time.realtimeSinceStartup;
				}
				if (Event.current.type == EventType.MouseUp && Time.realtimeSinceStartup - Designator_MoveGravship.middleMouseDownTime < 0.15f)
				{
					rotationDirection = RotationDirection.Clockwise;
				}
			}
			if (KeyBindingDefOf.Designator_RotateRight.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Clockwise;
			}
			else if (KeyBindingDefOf.Designator_RotateLeft.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Counterclockwise;
			}
			if (rotationDirection != RotationDirection.None)
			{
				this.marker.GravshipRotation = this.marker.GravshipRotation.Rotated(rotationDirection);
			}
			DesignatorUtility.GUIDoRotationControls(0f, this.PaneTopY, this.marker.GravshipRotation, delegate(Rot4 rot)
			{
				this.marker.GravshipRotation = rot;
			});
		}

		// Token: 0x0400B4CC RID: 46284
		public GravshipLandingMarker marker;

		// Token: 0x0400B4CD RID: 46285
		public Map map;

		// Token: 0x0400B4CE RID: 46286
		private Rot4 deselectedRotation;

		// Token: 0x0400B4CF RID: 46287
		private AcceptanceReport lastAcceptanceReport;

		// Token: 0x0400B4D0 RID: 46288
		private static readonly Color diagonalsColorValid = ColorLibrary.White.WithAlpha(0.5f);

		// Token: 0x0400B4D1 RID: 46289
		private static readonly Color diagonalsColorInvalid = ColorLibrary.Red.WithAlpha(0.5f);

		// Token: 0x0400B4D2 RID: 46290
		private readonly List<IntVec3> tmpValidGravshipCells = new List<IntVec3>();

		// Token: 0x0400B4D3 RID: 46291
		private readonly List<IntVec3> tmpInvalidGravshipCells = new List<IntVec3>();

		// Token: 0x0400B4D4 RID: 46292
		private readonly List<IntVec3> tmpThrusterCells = new List<IntVec3>();

		// Token: 0x0400B4D5 RID: 46293
		private static float middleMouseDownTime;
	}
}
