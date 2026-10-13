using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003104 RID: 12548
	public class Designator_ZoneAdd_Fishing : Designator_ZoneAdd
	{
		// Token: 0x17002DD3 RID: 11731
		// (get) Token: 0x06011A18 RID: 72216 RVA: 0x0050F3A7 File Offset: 0x0050D5A7
		protected override string NewZoneLabel
		{
			get
			{
				return "Zone_Fishing".Translate();
			}
		}

		// Token: 0x17002DD4 RID: 11732
		// (get) Token: 0x06011A19 RID: 72217 RVA: 0x0050F3B8 File Offset: 0x0050D5B8
		public override bool Visible
		{
			get
			{
				return ModsConfig.OdysseyActive && (ResearchProjectDefOf.Fishing.IsFinished || DebugSettings.godMode);
			}
		}

		// Token: 0x17002DD5 RID: 11733
		// (get) Token: 0x06011A1A RID: 72218 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool ShowRightClickHideOptions
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06011A1B RID: 72219 RVA: 0x0050F3D8 File Offset: 0x0050D5D8
		public Designator_ZoneAdd_Fishing()
		{
			this.zoneTypeToPlace = typeof(Zone_Fishing);
			this.defaultLabel = "Zone_Fishing".Translate();
			this.defaultDesc = "DesignatorFishingZoneDesc".Translate();
			this.tutorTag = "ZoneAdd_Fishing";
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_Fishing", true);
		}

		// Token: 0x06011A1C RID: 72220 RVA: 0x0050F464 File Offset: 0x0050D664
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.GetWaterBodyType(base.Map) == WaterBodyType.None || c.GetTerrain(base.Map).passability == Traversability.Impassable || !base.Map.waterBodyTracker.AnyFishPopulationAt(c))
			{
				return "CannotPlaceFishingZone".Translate();
			}
			AcceptanceReport result = base.CanDesignateCell(c);
			if (!result.Accepted)
			{
				return result;
			}
			return true;
		}

		// Token: 0x06011A1D RID: 72221 RVA: 0x0050F4E4 File Offset: 0x0050D6E4
		protected override Zone MakeNewZone()
		{
			if (!ModLister.CheckOdyssey("Fishing"))
			{
				return null;
			}
			return new Zone_Fishing(Find.CurrentMap.zoneManager);
		}

		// Token: 0x17002DD6 RID: 11734
		// (get) Token: 0x06011A1E RID: 72222 RVA: 0x0050F503 File Offset: 0x0050D703
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				if (!this.ShowRightClickHideOptions)
				{
					yield break;
				}
				foreach (FloatMenuOption floatMenuOption in Command_Hide_ZoneFishing.GetHideOptions())
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x06011A1F RID: 72223 RVA: 0x0050F514 File Offset: 0x0050D714
		public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			if (!ModLister.CheckOdyssey("Fishing"))
			{
				return;
			}
			try
			{
				bool flag = false;
				foreach (IntVec3 intVec in cells)
				{
					if (!flag && this.CanDesignateCell(intVec))
					{
						flag = true;
					}
					WaterBodyType waterBodyType = intVec.GetWaterBodyType(base.Map);
					if (waterBodyType == WaterBodyType.Saltwater)
					{
						this.saltWaterCells.Add(intVec);
					}
					else if (waterBodyType == WaterBodyType.Freshwater)
					{
						this.freshWaterCells.Add(intVec);
					}
				}
				if (!flag)
				{
					base.Finalize(false);
				}
				else
				{
					Zone_Fishing zone_Fishing = Find.Selector.SelectedZone as Zone_Fishing;
					WaterBodyType? waterBodyType2 = (zone_Fishing != null) ? new WaterBodyType?(zone_Fishing.Cells[0].GetWaterBodyType(base.Map)) : null;
					if (zone_Fishing != null)
					{
						this.selectedZones.Add(zone_Fishing);
					}
					WaterBodyType? waterBodyType4;
					WaterBodyType? waterBodyType3 = waterBodyType4 = waterBodyType2;
					WaterBodyType waterBodyType5 = WaterBodyType.Saltwater;
					if (!(waterBodyType4.GetValueOrDefault() == waterBodyType5 & waterBodyType4 != null))
					{
						Find.Selector.ClearSelection();
					}
					if (this.saltWaterCells.Count > 0)
					{
						base.DesignateMultiCell(this.saltWaterCells);
					}
					Zone_Fishing zone_Fishing2 = Find.Selector.SelectedZone as Zone_Fishing;
					if (zone_Fishing2 != null && zone_Fishing2 != zone_Fishing)
					{
						this.selectedZones.Add(zone_Fishing2);
					}
					waterBodyType4 = waterBodyType3;
					waterBodyType5 = WaterBodyType.Freshwater;
					if (waterBodyType4.GetValueOrDefault() == waterBodyType5 & waterBodyType4 != null)
					{
						Find.Selector.Select(zone_Fishing, false, false);
					}
					else
					{
						Find.Selector.ClearSelection();
					}
					if (this.freshWaterCells.Count > 0)
					{
						base.DesignateMultiCell(this.freshWaterCells);
					}
					Zone_Fishing zone_Fishing3 = Find.Selector.SelectedZone as Zone_Fishing;
					if (zone_Fishing3 != null && zone_Fishing3 != zone_Fishing)
					{
						this.selectedZones.Add(zone_Fishing3);
					}
					Find.Selector.ClearSelection();
					foreach (Zone obj in this.selectedZones)
					{
						Find.Selector.Select(obj, false, false);
					}
				}
			}
			finally
			{
				this.saltWaterCells.Clear();
				this.freshWaterCells.Clear();
				this.selectedZones.Clear();
			}
		}

		// Token: 0x06011A20 RID: 72224 RVA: 0x0050F788 File Offset: 0x0050D988
		protected override void FinalizeDesignationFailed()
		{
			base.FinalizeDesignationFailed();
			Messages.Message("CannotPlaceFishingZone".Translate(), MessageTypeDefOf.RejectInput, false);
		}

		// Token: 0x0400B59C RID: 46492
		private readonly List<IntVec3> freshWaterCells = new List<IntVec3>();

		// Token: 0x0400B59D RID: 46493
		private readonly List<IntVec3> saltWaterCells = new List<IntVec3>();

		// Token: 0x0400B59E RID: 46494
		private readonly List<Zone> selectedZones = new List<Zone>();
	}
}
