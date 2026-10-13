using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200054F RID: 1359
	public abstract class Zone : IExposable, ISelectable, ILoadReferenceable, IRenameable, IHideable
	{
		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06002336 RID: 9014 RVA: 0x000B7257 File Offset: 0x000B5457
		// (set) Token: 0x06002337 RID: 9015 RVA: 0x000B7269 File Offset: 0x000B5469
		public string RenamableLabel
		{
			get
			{
				return this.label ?? this.baseLabel;
			}
			set
			{
				this.label = value;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002338 RID: 9016 RVA: 0x000B7272 File Offset: 0x000B5472
		public string BaseLabel
		{
			get
			{
				return this.baseLabel;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06002339 RID: 9017 RVA: 0x000B727A File Offset: 0x000B547A
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x0600233A RID: 9018 RVA: 0x000B7282 File Offset: 0x000B5482
		public Map Map
		{
			get
			{
				return this.zoneManager.map;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x0600233B RID: 9019 RVA: 0x000B728F File Offset: 0x000B548F
		public IntVec3 Position
		{
			get
			{
				if (this.cells.Count == 0)
				{
					return IntVec3.Invalid;
				}
				return this.cells[0];
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x0600233C RID: 9020 RVA: 0x000B72B0 File Offset: 0x000B54B0
		// (set) Token: 0x0600233D RID: 9021 RVA: 0x000B72B8 File Offset: 0x000B54B8
		public bool Hidden
		{
			get
			{
				return this.hidden;
			}
			set
			{
				this.hidden = value;
				foreach (IntVec3 loc in this.Cells)
				{
					this.Map.mapDrawer.MapMeshDirty(loc, MapMeshFlagDefOf.Zone);
				}
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x0600233E RID: 9022 RVA: 0x000B7328 File Offset: 0x000B5528
		public Material Material
		{
			get
			{
				if (this.materialInt == null)
				{
					this.materialInt = this.CreateMaterial();
				}
				return this.materialInt;
			}
		}

		// Token: 0x0600233F RID: 9023 RVA: 0x000B7344 File Offset: 0x000B5544
		protected virtual Material CreateMaterial()
		{
			Material material = SolidColorMaterials.SimpleSolidColorMaterial(this.color, false);
			material.renderQueue = 3600;
			return material;
		}

		// Token: 0x06002340 RID: 9024 RVA: 0x000B735D File Offset: 0x000B555D
		public IEnumerator<IntVec3> GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.cells.Count; i = num + 1)
			{
				yield return this.cells[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06002341 RID: 9025 RVA: 0x000B736C File Offset: 0x000B556C
		public List<IntVec3> Cells
		{
			get
			{
				if (!this.cellsShuffled)
				{
					this.cells.Shuffle<IntVec3>();
					this.cellsShuffled = true;
				}
				return this.cells;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06002342 RID: 9026 RVA: 0x000B738E File Offset: 0x000B558E
		public int CellCount
		{
			get
			{
				return this.cells.Count;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06002343 RID: 9027 RVA: 0x000B739C File Offset: 0x000B559C
		public int HeldThingsCount
		{
			get
			{
				int num = 0;
				ThingGrid thingGrid = this.Map.thingGrid;
				for (int i = 0; i < this.cells.Count; i++)
				{
					List<Thing> list = thingGrid.ThingsListAt(this.cells[i]);
					num += list.Count;
				}
				return num;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06002344 RID: 9028 RVA: 0x000B73EA File Offset: 0x000B55EA
		public IEnumerable<Thing> AllContainedThings
		{
			get
			{
				ThingGrid grids = this.Map.thingGrid;
				int num;
				for (int i = 0; i < this.cells.Count; i = num + 1)
				{
					List<Thing> thingList = grids.ThingsListAt(this.cells[i]);
					for (int j = 0; j < thingList.Count; j = num + 1)
					{
						yield return thingList[j];
						num = j;
					}
					thingList = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000641 RID: 1601
		// (get) Token: 0x06002345 RID: 9029 RVA: 0x000B73FC File Offset: 0x000B55FC
		public bool ContainsStaticFire
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastStaticFireCheckTick + 1000)
				{
					this.lastStaticFireCheckResult = false;
					for (int i = 0; i < this.cells.Count; i++)
					{
						if (this.cells[i].ContainsStaticFire(this.Map))
						{
							this.lastStaticFireCheckResult = true;
							break;
						}
					}
				}
				return this.lastStaticFireCheckResult;
			}
		}

		// Token: 0x17000642 RID: 1602
		// (get) Token: 0x06002346 RID: 9030 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool IsMultiselectable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x06002347 RID: 9031
		protected abstract Color NextZoneColor { get; }

		// Token: 0x06002348 RID: 9032 RVA: 0x000B7466 File Offset: 0x000B5666
		public Zone()
		{
		}

		// Token: 0x06002349 RID: 9033 RVA: 0x000B7498 File Offset: 0x000B5698
		public Zone(string baseName, ZoneManager zoneManager)
		{
			this.baseLabel = baseName;
			this.label = zoneManager.NewZoneName(baseName);
			this.zoneManager = zoneManager;
			this.ID = Find.UniqueIDsManager.GetNextZoneID();
			this.color = this.NextZoneColor;
		}

		// Token: 0x0600234A RID: 9034 RVA: 0x000B750C File Offset: 0x000B570C
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.baseLabel, "baseLabel", null, false);
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Collections.Look<IntVec3>(ref this.cells, "cells", LookMode.Undefined, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.CheckAddHaulDestination();
			}
		}

		// Token: 0x0600234B RID: 9035 RVA: 0x000B75A8 File Offset: 0x000B57A8
		public virtual void AddCell(IntVec3 c)
		{
			if (this.cells.Contains(c))
			{
				string str = "Adding cell to zone which already has it. c=";
				IntVec3 intVec = c;
				Log.Error(str + intVec.ToString() + ", zone=" + ((this != null) ? this.ToString() : null));
				return;
			}
			List<Thing> list = this.Map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (!thing.def.CanOverlapZones)
				{
					string str2 = "Added zone over zone-incompatible thing ";
					Thing thing2 = thing;
					Log.Error(str2 + ((thing2 != null) ? thing2.ToString() : null));
					return;
				}
			}
			this.cells.Add(c);
			this.zoneManager.AddZoneGridCell(this, c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Zone);
			AutoHomeAreaMaker.Notify_ZoneCellAdded(c, this);
			this.cellsShuffled = false;
		}

		// Token: 0x0600234C RID: 9036 RVA: 0x000B768C File Offset: 0x000B588C
		public virtual void RemoveCell(IntVec3 c)
		{
			if (!this.cells.Contains(c))
			{
				string str = "Cannot remove cell from zone which doesn't have it. c=";
				IntVec3 intVec = c;
				Log.Error(str + intVec.ToString() + ", zone=" + ((this != null) ? this.ToString() : null));
				return;
			}
			this.cells.Remove(c);
			this.zoneManager.ClearZoneGridCell(c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlagDefOf.Zone);
			this.cellsShuffled = false;
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
		}

		// Token: 0x0600234D RID: 9037 RVA: 0x000B7726 File Offset: 0x000B5926
		public void Delete()
		{
			this.Delete(true);
		}

		// Token: 0x0600234E RID: 9038 RVA: 0x000B7730 File Offset: 0x000B5930
		public virtual void Delete(bool playSound)
		{
			if (playSound)
			{
				SoundDefOf.Designate_ZoneDelete.PlayOneShotOnCamera(this.Map);
			}
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
			else
			{
				while (this.cells.Count > 0)
				{
					this.RemoveCell(this.cells[this.cells.Count - 1]);
				}
			}
			Find.Selector.Deselect(this);
		}

		// Token: 0x0600234F RID: 9039 RVA: 0x000B779C File Offset: 0x000B599C
		public void Deregister()
		{
			this.zoneManager.DeregisterZone(this);
		}

		// Token: 0x06002350 RID: 9040 RVA: 0x000B77AA File Offset: 0x000B59AA
		public virtual void PostRegister()
		{
			this.CheckAddHaulDestination();
		}

		// Token: 0x06002351 RID: 9041 RVA: 0x000B77B4 File Offset: 0x000B59B4
		public virtual void PostDeregister()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
		}

		// Token: 0x06002352 RID: 9042 RVA: 0x000B77DC File Offset: 0x000B59DC
		public bool ContainsCell(IntVec3 c)
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				if (this.cells[i] == c)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002353 RID: 9043 RVA: 0x000B7818 File Offset: 0x000B5A18
		public virtual string GetInspectString()
		{
			return string.Format("{0}: {1}", "Size".Translate().CapitalizeFirst(), this.CellCount);
		}

		// Token: 0x06002354 RID: 9044 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return null;
		}

		// Token: 0x06002355 RID: 9045 RVA: 0x000B7851 File Offset: 0x000B5A51
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.GetZoneAddGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Designator_ZoneDelete_Shrink designator_ZoneDelete_Shrink = DesignatorUtility.FindAllowedDesignator<Designator_ZoneDelete_Shrink>();
			if (designator_ZoneDelete_Shrink != null)
			{
				yield return designator_ZoneDelete_Shrink;
			}
			yield return new Command_Action
			{
				icon = TexButton.Delete,
				defaultLabel = "CommandDeleteZoneLabel".Translate(),
				defaultDesc = "CommandDeleteZoneDesc".Translate(),
				action = new Action(this.Delete),
				hotKey = KeyBindingDefOf.Designator_Deconstruct
			};
			yield break;
			yield break;
		}

		// Token: 0x06002356 RID: 9046 RVA: 0x000B7861 File Offset: 0x000B5A61
		public virtual IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06002357 RID: 9047 RVA: 0x000B7868 File Offset: 0x000B5A68
		public void CheckContiguous()
		{
			if (this.cells.Count == 0)
			{
				return;
			}
			if (Zone.extantGrid == null)
			{
				Zone.extantGrid = new BoolGrid(this.Map);
			}
			else
			{
				Zone.extantGrid.ClearAndResizeTo(this.Map);
			}
			if (Zone.foundGrid == null)
			{
				Zone.foundGrid = new BoolGrid(this.Map);
			}
			else
			{
				Zone.foundGrid.ClearAndResizeTo(this.Map);
			}
			for (int i = 0; i < this.cells.Count; i++)
			{
				Zone.extantGrid.Set(this.cells[i], true);
			}
			Predicate<IntVec3> passCheck = (IntVec3 c) => Zone.extantGrid[c] && !Zone.foundGrid[c];
			int numFound = 0;
			Action<IntVec3> processor = delegate(IntVec3 c)
			{
				Zone.foundGrid.Set(c, true);
				int numFound = numFound;
				numFound++;
			};
			this.Map.floodFiller.FloodFill(this.cells[0], passCheck, processor, int.MaxValue, false, null);
			if (numFound < this.cells.Count)
			{
				foreach (IntVec3 c2 in this.Map.AllCells)
				{
					if (Zone.extantGrid[c2] && !Zone.foundGrid[c2])
					{
						this.RemoveCell(c2);
					}
				}
			}
		}

		// Token: 0x06002358 RID: 9048 RVA: 0x000B79DC File Offset: 0x000B5BDC
		public virtual IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		// Token: 0x06002359 RID: 9049 RVA: 0x000B79E8 File Offset: 0x000B5BE8
		private void CheckAddHaulDestination()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x000B7A10 File Offset: 0x000B5C10
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x000B7A18 File Offset: 0x000B5C18
		public string GetUniqueLoadID()
		{
			return "Zone_" + this.ID.ToString();
		}

		// Token: 0x04001D61 RID: 7521
		public ZoneManager zoneManager;

		// Token: 0x04001D62 RID: 7522
		public int ID = -1;

		// Token: 0x04001D63 RID: 7523
		public string label;

		// Token: 0x04001D64 RID: 7524
		private string baseLabel;

		// Token: 0x04001D65 RID: 7525
		public List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x04001D66 RID: 7526
		private bool cellsShuffled;

		// Token: 0x04001D67 RID: 7527
		public Color color = Color.white;

		// Token: 0x04001D68 RID: 7528
		private Material materialInt;

		// Token: 0x04001D69 RID: 7529
		private bool hidden;

		// Token: 0x04001D6A RID: 7530
		private int lastStaticFireCheckTick = -9999;

		// Token: 0x04001D6B RID: 7531
		private bool lastStaticFireCheckResult;

		// Token: 0x04001D6C RID: 7532
		private const int StaticFireCheckInterval = 1000;

		// Token: 0x04001D6D RID: 7533
		private static BoolGrid extantGrid;

		// Token: 0x04001D6E RID: 7534
		private static BoolGrid foundGrid;
	}
}
