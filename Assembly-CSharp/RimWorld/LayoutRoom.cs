using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EB7 RID: 7863
	public class LayoutRoom : IExposable, ILoadReferenceable
	{
		// Token: 0x17001AA8 RID: 6824
		// (get) Token: 0x0600AE52 RID: 44626 RVA: 0x0031BA11 File Offset: 0x00319C11
		public int Area
		{
			get
			{
				return this.rects.Sum((CellRect r) => r.Area);
			}
		}

		// Token: 0x17001AA9 RID: 6825
		// (get) Token: 0x0600AE53 RID: 44627 RVA: 0x0031BA40 File Offset: 0x00319C40
		public CellRect Boundary
		{
			get
			{
				CellRect result = this.rects[0];
				for (int i = 1; i < this.rects.Count; i++)
				{
					result.Encapsulate(this.rects[i]);
				}
				return result;
			}
		}

		// Token: 0x17001AAA RID: 6826
		// (get) Token: 0x0600AE54 RID: 44628 RVA: 0x0031BA85 File Offset: 0x00319C85
		public bool DontDestroyWallsDoors
		{
			get
			{
				if (!this.defs.NullOrEmpty<LayoutRoomDef>())
				{
					return this.defs.Any((LayoutRoomDef d) => d.dontDestroyWallsDoors);
				}
				return false;
			}
		}

		// Token: 0x17001AAB RID: 6827
		// (get) Token: 0x0600AE55 RID: 44629 RVA: 0x0031BAC0 File Offset: 0x00319CC0
		public IEnumerable<IntVec3> Corners
		{
			get
			{
				return this.rects.SelectMany((CellRect r) => r.Corners);
			}
		}

		// Token: 0x17001AAC RID: 6828
		// (get) Token: 0x0600AE56 RID: 44630 RVA: 0x0031BAEC File Offset: 0x00319CEC
		public string ThreatSignal
		{
			get
			{
				return this.threatSignal;
			}
		}

		// Token: 0x0600AE57 RID: 44631 RVA: 0x0031BAF4 File Offset: 0x00319CF4
		public LayoutRoom()
		{
		}

		// Token: 0x0600AE58 RID: 44632 RVA: 0x0031BB20 File Offset: 0x00319D20
		public LayoutRoom(LayoutStructureSketch sketch, List<CellRect> rects)
		{
			this.rects = rects;
			this.sketch = sketch;
			this.threatSignal = "RoomThreat" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			this.loadId = Find.UniqueIDsManager.GetNextRoomID();
		}

		// Token: 0x17001AAD RID: 6829
		// (get) Token: 0x0600AE59 RID: 44633 RVA: 0x0031BB94 File Offset: 0x00319D94
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.rects.Count; i = num + 1)
				{
					foreach (IntVec3 intVec in this.rects[i].Cells)
					{
						yield return intVec;
					}
					IEnumerator<IntVec3> enumerator = null;
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600AE5A RID: 44634 RVA: 0x0031BBA4 File Offset: 0x00319DA4
		public bool TryGetRectOfSize(int minWidth, int minHeight, out CellRect rect)
		{
			foreach (CellRect cellRect in this.rects)
			{
				if (cellRect.AreSidesEqualOrGreater(minWidth, minHeight))
				{
					rect = cellRect;
					return true;
				}
			}
			rect = default(CellRect);
			return false;
		}

		// Token: 0x0600AE5B RID: 44635 RVA: 0x0031BC10 File Offset: 0x00319E10
		public bool IsCorner(IntVec3 position)
		{
			for (int i = 0; i < this.rects.Count; i++)
			{
				if (this.rects[i].IsCorner(position))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600AE5C RID: 44636 RVA: 0x0031BC4D File Offset: 0x00319E4D
		public bool HasLayoutDef(LayoutRoomDef def)
		{
			return this.defs.Contains(def);
		}

		// Token: 0x0600AE5D RID: 44637 RVA: 0x0031BC5C File Offset: 0x00319E5C
		public bool IsAdjacentTo(LayoutRoom room, int minAdjacencyScore = 1)
		{
			foreach (CellRect cellRect in this.rects)
			{
				foreach (CellRect other in room.rects)
				{
					if (cellRect.ContractedBy(1).GetAdjacencyScore(other) >= minAdjacencyScore)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600AE5E RID: 44638 RVA: 0x0031BD00 File Offset: 0x00319F00
		public bool Contains(IntVec3 position, int contractedBy = 0)
		{
			foreach (CellRect cellRect in this.rects)
			{
				if (cellRect.ContractedBy(contractedBy).Contains(position))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600AE5F RID: 44639 RVA: 0x0031BD68 File Offset: 0x00319F68
		public void SpawnRectTriggersForAction(SignalAction action, Map map)
		{
			foreach (CellRect rect in this.rects)
			{
				RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
				rectTrigger.signalTag = action.signalTag;
				rectTrigger.Rect = rect;
				GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
			}
		}

		// Token: 0x0600AE60 RID: 44640 RVA: 0x0031BDE8 File Offset: 0x00319FE8
		public bool TryGetRandomCellInRoom(ThingDef def, Map map, out IntVec3 cell, Rot4? rot = null, int contractedBy = 0, int padding = 0, Func<IntVec3, bool> validator = null, bool ignoreBuildings = false)
		{
			Rot4 rot2 = rot ?? Rot4.North;
			foreach (CellRect cellRect in this.rects.InRandomOrder(null))
			{
				foreach (IntVec3 intVec in cellRect.ContractedBy(contractedBy).Cells.InRandomOrder(null))
				{
					CellRect cellRect2 = intVec.RectAbout(def.size, rot2);
					if (GenSpawn.CanSpawnAt(def, intVec, map, rot, ignoreBuildings) && (!def.hasInteractionCell || ThingUtility.InteractionCellWhenAt(def, intVec, rot2, map).Standable(map)))
					{
						bool flag = false;
						foreach (IntVec3 intVec2 in cellRect2.ExpandedBy(padding))
						{
							flag = false;
							if (!intVec2.InBounds(map))
							{
								break;
							}
							if (!this.Contains(intVec2, 0))
							{
								break;
							}
							if (!ignoreBuildings && intVec2.GetEdifice(map) != null)
							{
								break;
							}
							if (!ignoreBuildings && intVec2.GetFirstBuilding(map) != null)
							{
								break;
							}
							if (!ignoreBuildings && def.passability != Traversability.Standable && RoomGenUtility.IsDoorAdjacentTo(intVec2, map, true))
							{
								break;
							}
							if (intVec2.GetFirstPawn(map) != null)
							{
								break;
							}
							if (validator != null && !validator(intVec2))
							{
								break;
							}
							flag = true;
						}
						if (flag)
						{
							cell = intVec;
							return true;
						}
					}
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600AE61 RID: 44641 RVA: 0x0031BFF0 File Offset: 0x0031A1F0
		public bool TryGetRandomCellInRoom(Map map, out IntVec3 cell, int contractedBy = 0, int padding = 0, Func<IntVec3, bool> validator = null, bool ignoreBuildings = false)
		{
			foreach (CellRect cellRect in this.rects.InRandomOrder(null))
			{
				foreach (IntVec3 intVec in cellRect.ContractedBy(contractedBy).Cells.InRandomOrder(null))
				{
					bool flag = false;
					foreach (IntVec3 intVec2 in CellRect.FromCell(intVec).ExpandedBy(padding).ExpandedBy(padding))
					{
						flag = false;
						if (!intVec2.InBounds(map))
						{
							break;
						}
						if (!this.Contains(intVec2, 0))
						{
							break;
						}
						if (!ignoreBuildings && intVec2.GetEdifice(map) != null)
						{
							break;
						}
						if (!ignoreBuildings && intVec2.GetFirstBuilding(map) != null)
						{
							break;
						}
						if (intVec2.GetFirstPawn(map) != null)
						{
							break;
						}
						if (validator != null && !validator(intVec2))
						{
							break;
						}
						flag = true;
					}
					if (flag)
					{
						cell = intVec;
						return true;
					}
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600AE62 RID: 44642 RVA: 0x0031C188 File Offset: 0x0031A388
		public bool TryGetRectContainingCell(IntVec3 cell, out CellRect rect)
		{
			foreach (CellRect cellRect in this.rects)
			{
				if (cellRect.Contains(cell))
				{
					rect = cellRect;
					return true;
				}
			}
			rect = CellRect.Empty;
			return false;
		}

		// Token: 0x0600AE63 RID: 44643 RVA: 0x0031C1F8 File Offset: 0x0031A3F8
		public bool TryGetRandomCellInRoom(out IntVec3 cell, int contractedBy = 0, Func<IntVec3, bool> validator = null)
		{
			for (int i = 0; i < this.rects.Count; i++)
			{
				foreach (IntVec3 intVec in this.rects[i].ContractedBy(contractedBy).Cells)
				{
					if (validator == null || validator(intVec))
					{
						LayoutRoom.cells.Add(intVec);
					}
				}
			}
			if (LayoutRoom.cells.Count == 0)
			{
				cell = IntVec3.Invalid;
				return false;
			}
			cell = LayoutRoom.cells.RandomElement<IntVec3>();
			LayoutRoom.cells.Clear();
			return true;
		}

		// Token: 0x0600AE64 RID: 44644 RVA: 0x0031C2B8 File Offset: 0x0031A4B8
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.noExteriorDoors, "noExteriorDoors", false, false);
			Scribe_Values.Look<int>(ref this.loadId, "loadId", 0, false);
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<string>(ref this.threatSignal, "threatSignal", null, false);
			Scribe_Defs.Look<LayoutRoomDef>(ref this.requiredDef, "requiredDef");
			Scribe_References.Look<LayoutStructureSketch>(ref this.sketch, "sketch", false);
			Scribe_Collections.Look<LayoutRoomDef>(ref this.defs, "defs", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<LayoutRoom>(ref this.merged, "merged", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<CellRect>(ref this.rects, "rects", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.entryCells, "entryCells", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.loadId == 0)
			{
				this.loadId = Rand.Int;
			}
		}

		// Token: 0x0600AE65 RID: 44645 RVA: 0x0031C3A1 File Offset: 0x0031A5A1
		public string GetUniqueLoadID()
		{
			return string.Format("LayoutRoom_{0}", this.loadId);
		}

		// Token: 0x040077DF RID: 30687
		public List<CellRect> rects;

		// Token: 0x040077E0 RID: 30688
		public List<LayoutRoomDef> defs = new List<LayoutRoomDef>();

		// Token: 0x040077E1 RID: 30689
		public List<LayoutRoom> merged = new List<LayoutRoom>();

		// Token: 0x040077E2 RID: 30690
		public List<IntVec3> entryCells;

		// Token: 0x040077E3 RID: 30691
		public LayoutRoomDef requiredDef;

		// Token: 0x040077E4 RID: 30692
		public LayoutStructureSketch sketch;

		// Token: 0x040077E5 RID: 30693
		public bool noExteriorDoors;

		// Token: 0x040077E6 RID: 30694
		private string threatSignal;

		// Token: 0x040077E7 RID: 30695
		public int loadId;

		// Token: 0x040077E8 RID: 30696
		public int id;

		// Token: 0x040077E9 RID: 30697
		public readonly List<LayoutRoom> connections = new List<LayoutRoom>();

		// Token: 0x040077EA RID: 30698
		private static readonly HashSet<IntVec3> cells = new HashSet<IntVec3>();
	}
}
