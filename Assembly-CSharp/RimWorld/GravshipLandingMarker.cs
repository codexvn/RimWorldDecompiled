using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A9D RID: 10909
	public class GravshipLandingMarker : Thing
	{
		// Token: 0x170025F6 RID: 9718
		// (get) Token: 0x0600F2F1 RID: 62193 RVA: 0x00468ABB File Offset: 0x00466CBB
		public IEnumerable<IntVec3> GravshipCells
		{
			get
			{
				return this.gravshipCells;
			}
		}

		// Token: 0x170025F7 RID: 9719
		// (get) Token: 0x0600F2F2 RID: 62194 RVA: 0x00468AC3 File Offset: 0x00466CC3
		public IEnumerable<IntVec3> ThrusterCells
		{
			get
			{
				return this.thrusterCells;
			}
		}

		// Token: 0x170025F8 RID: 9720
		// (get) Token: 0x0600F2F3 RID: 62195 RVA: 0x00468ACB File Offset: 0x00466CCB
		public IEnumerable<IntVec3> DoorCells
		{
			get
			{
				return this.doorCells;
			}
		}

		// Token: 0x170025F9 RID: 9721
		// (get) Token: 0x0600F2F4 RID: 62196 RVA: 0x00468AD3 File Offset: 0x00466CD3
		protected bool Visible
		{
			get
			{
				return base.Spawned && this.gravship != null && !(Find.DesignatorManager.SelectedDesignator is Designator_MoveGravship);
			}
		}

		// Token: 0x170025FA RID: 9722
		// (get) Token: 0x0600F2F5 RID: 62197 RVA: 0x00468AFC File Offset: 0x00466CFC
		// (set) Token: 0x0600F2F6 RID: 62198 RVA: 0x00468B09 File Offset: 0x00466D09
		public Rot4 GravshipRotation
		{
			get
			{
				return this.gravship.Rotation;
			}
			set
			{
				if (this.gravship.Rotation == value)
				{
					return;
				}
				this.gravship.Rotation = value;
				this.CacheCells();
				this.Notify_Moved();
			}
		}

		// Token: 0x170025FB RID: 9723
		// (get) Token: 0x0600F2F7 RID: 62199 RVA: 0x00468B38 File Offset: 0x00466D38
		public override CellRect? CustomRectForSelector
		{
			get
			{
				if (this.Visible)
				{
					return new CellRect?(this.selectorRect);
				}
				return null;
			}
		}

		// Token: 0x0600F2F8 RID: 62200 RVA: 0x00468B64 File Offset: 0x00466D64
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<Gravship>(ref this.gravship, "gravship", Array.Empty<object>());
			Scribe_Values.Look<CellRect>(ref this.selectorRect, "selectorRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Current.Game.Gravship = this.gravship;
				this.CacheCells();
			}
		}

		// Token: 0x0600F2F9 RID: 62201 RVA: 0x00468BC4 File Offset: 0x00466DC4
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (!ModLister.CheckOdyssey("Gravship"))
			{
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (Find.CurrentMap != map || Find.World.renderer.wantedMode == WorldRenderMode.Planet)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					CameraJumper.TryJump(this, CameraJumper.MovementMode.Cut);
				});
			}
			this.CacheCells();
			this.Notify_Moved();
			if (!respawningAfterLoad)
			{
				Find.GravshipController.Notify_LandingAreaConfirmationStarted(this);
			}
		}

		// Token: 0x0600F2FA RID: 62202 RVA: 0x00468C2C File Offset: 0x00466E2C
		public void CacheCells()
		{
			this.gravshipCells.Clear();
			this.gravshipCells.AddRange(this.gravship.Foundations.Keys);
			this.doorCells.Clear();
			this.thrusterCells.Clear();
			this.tmpExclusionZoneCells.Clear();
			foreach (KeyValuePair<Thing, PositionData.Data> keyValuePair in this.gravship.ThrusterPlacements)
			{
				Thing thing;
				PositionData.Data data;
				keyValuePair.Deconstruct(out thing, out data);
				Thing thing2 = thing;
				PositionData.Data data2 = data;
				thing2.def.GetCompProperties<CompProperties_GravshipThruster>().GetExclusionZone(data2.local, data2.rotation, ref this.tmpExclusionZoneCells);
				this.thrusterCells.AddRange(this.tmpExclusionZoneCells);
			}
		}

		// Token: 0x0600F2FB RID: 62203 RVA: 0x00468D08 File Offset: 0x00466F08
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (!this.Visible)
			{
				return;
			}
			this.tmpGravshipCells.Clear();
			this.tmpGravshipCells.AddRange(this.GravshipCells);
			IntVec3 position = base.Position;
			for (int i = 0; i < this.tmpGravshipCells.Count; i++)
			{
				List<IntVec3> list = this.tmpGravshipCells;
				int index = i;
				list[index] += position;
			}
			this.tmpThrusterCells.Clear();
			this.tmpThrusterCells.AddRange(this.ThrusterCells);
			for (int j = 0; j < this.tmpThrusterCells.Count; j++)
			{
				List<IntVec3> list = this.tmpThrusterCells;
				int index = j;
				list[index] += position;
			}
			GenDraw.DrawFieldEdges(this.tmpGravshipCells, 2900);
			GenDraw.DrawDiagonalStripes(this.tmpGravshipCells, new Color?(GravshipLandingMarker.diagonalsColor), null, 2900);
			GenDraw.DrawFieldEdges(this.tmpThrusterCells, ColorLibrary.Orange, null, null, 2900);
			foreach (KeyValuePair<Thing, PositionData.Data> keyValuePair in this.gravship.ExteriorDoorPlacements)
			{
				Thing thing;
				PositionData.Data data;
				keyValuePair.Deconstruct(out thing, out data);
				Thing thing2 = thing;
				PositionData.Data data2 = data;
				GhostUtility.GhostGraphicFor(thing2.Graphic, thing2.def, Color.white, null).DrawFromDef(GenThing.TrueCenter(position + data2.local, data2.rotation, thing2.def.Size, AltitudeLayer.MetaOverlays.AltitudeFor()), data2.rotation, thing2.def, 0f);
			}
		}

		// Token: 0x0600F2FC RID: 62204 RVA: 0x00468ED8 File Offset: 0x004670D8
		public void Notify_Moved()
		{
			if (this.gravship == null)
			{
				return;
			}
			this.selectorRect = CellRect.CenteredOn(base.Position, 1);
			foreach (IntVec3 a in this.gravship.Foundations.Keys)
			{
				this.selectorRect = this.selectorRect.Encapsulate(a + base.Position);
			}
		}

		// Token: 0x0600F2FD RID: 62205 RVA: 0x00468F68 File Offset: 0x00467168
		public void BeginLanding(WorldComponent_GravshipController gravshipController)
		{
			Map map = base.Map;
			CameraJumper.TryJump(this, CameraJumper.MovementMode.Pan);
			this.Destroy(DestroyMode.Vanish);
			gravshipController.InitiateLanding(this.gravship, map, base.Position, this.GravshipRotation);
		}

		// Token: 0x0600F2FE RID: 62206 RVA: 0x00468FA8 File Offset: 0x004671A8
		public override string GetInspectString()
		{
			return "ConfirmLandingToContinue".Translate();
		}

		// Token: 0x0400A008 RID: 40968
		public Gravship gravship;

		// Token: 0x0400A009 RID: 40969
		private CellRect selectorRect;

		// Token: 0x0400A00A RID: 40970
		private readonly List<IntVec3> gravshipCells = new List<IntVec3>();

		// Token: 0x0400A00B RID: 40971
		private readonly List<IntVec3> thrusterCells = new List<IntVec3>();

		// Token: 0x0400A00C RID: 40972
		private readonly List<IntVec3> doorCells = new List<IntVec3>();

		// Token: 0x0400A00D RID: 40973
		private static readonly Color diagonalsColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x0400A00E RID: 40974
		private List<IntVec3> tmpExclusionZoneCells = new List<IntVec3>();

		// Token: 0x0400A00F RID: 40975
		private readonly List<IntVec3> tmpGravshipCells = new List<IntVec3>();

		// Token: 0x0400A010 RID: 40976
		private readonly List<IntVec3> tmpThrusterCells = new List<IntVec3>();
	}
}
