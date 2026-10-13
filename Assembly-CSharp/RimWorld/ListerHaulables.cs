using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F73 RID: 8051
	public class ListerHaulables
	{
		// Token: 0x0600B1D4 RID: 45524 RVA: 0x0032D214 File Offset: 0x0032B414
		public ListerHaulables(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600B1D5 RID: 45525 RVA: 0x0032D239 File Offset: 0x0032B439
		public ICollection<Thing> ThingsPotentiallyNeedingHauling()
		{
			return this.haulables;
		}

		// Token: 0x0600B1D6 RID: 45526 RVA: 0x0032D241 File Offset: 0x0032B441
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x0600B1D7 RID: 45527 RVA: 0x0032D24A File Offset: 0x0032B44A
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x0600B1D8 RID: 45528 RVA: 0x0032D241 File Offset: 0x0032B441
		public void HaulDesignationAdded(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x0600B1D9 RID: 45529 RVA: 0x0032D24A File Offset: 0x0032B44A
		public void HaulDesignationRemoved(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x0600B1DA RID: 45530 RVA: 0x0032D241 File Offset: 0x0032B441
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x0600B1DB RID: 45531 RVA: 0x0032D24A File Offset: 0x0032B44A
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x0600B1DC RID: 45532 RVA: 0x0032D241 File Offset: 0x0032B441
		public void Notify_AddedThing(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x0600B1DD RID: 45533 RVA: 0x0032D254 File Offset: 0x0032B454
		public void Notify_SlotGroupChanged(SlotGroup sg)
		{
			List<IntVec3> cellsList = sg.CellsList;
			if (cellsList != null)
			{
				sg.RemoveHaulDesignationOnStoredThings();
				for (int i = 0; i < cellsList.Count; i++)
				{
					this.RecalcAllInCell(cellsList[i]);
				}
			}
		}

		// Token: 0x0600B1DE RID: 45534 RVA: 0x0032D290 File Offset: 0x0032B490
		public void Notify_HaulSourceChanged(IHaulSource holder)
		{
			foreach (Thing t in ((IEnumerable<Thing>)holder.GetDirectlyHeldThings()))
			{
				this.Check(t);
			}
		}

		// Token: 0x0600B1DF RID: 45535 RVA: 0x0032D2E0 File Offset: 0x0032B4E0
		public void ListerHaulablesTick()
		{
			ListerHaulables.groupCycleIndex++;
			if (ListerHaulables.groupCycleIndex >= 2147473647)
			{
				ListerHaulables.groupCycleIndex = 0;
			}
			this.CellsCheckTick(this.map.haulDestinationManager.AllGroupsListForReading);
			this.HaulSourcesCheckTick(this.map.haulDestinationManager.AllHaulSourcesListForReading);
		}

		// Token: 0x0600B1E0 RID: 45536 RVA: 0x0032D338 File Offset: 0x0032B538
		private void CellsCheckTick(List<SlotGroup> sgList)
		{
			if (sgList.Count == 0)
			{
				return;
			}
			int num = ListerHaulables.groupCycleIndex % sgList.Count;
			SlotGroup slotGroup = sgList[ListerHaulables.groupCycleIndex % sgList.Count];
			if (slotGroup.CellsList.Count == 0)
			{
				return;
			}
			while (ListerHaulables.cellCycleIndices.Count <= num)
			{
				ListerHaulables.cellCycleIndices.Add(0);
			}
			if (ListerHaulables.cellCycleIndices[num] >= 2147473647)
			{
				ListerHaulables.cellCycleIndices[num] = 0;
			}
			for (int i = 0; i < 4; i++)
			{
				List<int> list = ListerHaulables.cellCycleIndices;
				int index = num;
				int num2 = list[index];
				list[index] = num2 + 1;
				this.RecalcAllInCell(slotGroup.CellsList[ListerHaulables.cellCycleIndices[num] % slotGroup.CellsList.Count]);
			}
		}

		// Token: 0x0600B1E1 RID: 45537 RVA: 0x0032D400 File Offset: 0x0032B600
		private void HaulSourcesCheckTick(List<IHaulSource> haulList)
		{
			if (haulList.Count == 0)
			{
				return;
			}
			int num = Mathf.CeilToInt((float)haulList.Count / 4f);
			int num2 = ListerHaulables.groupCycleIndex % num;
			int num3 = 0;
			while (num3 < 4 && num3 < haulList.Count)
			{
				int index = num2 + num3;
				IHaulSource haulSource = haulList[index];
				if (haulSource.GetDirectlyHeldThings().Count != 0)
				{
					this.RecalculateAllInHaulSource(haulSource);
				}
				num3++;
			}
		}

		// Token: 0x0600B1E2 RID: 45538 RVA: 0x0032D46C File Offset: 0x0032B66C
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x0600B1E3 RID: 45539 RVA: 0x0032D4A4 File Offset: 0x0032B6A4
		public void RecalcAllInCells(IEnumerable<IntVec3> cells)
		{
			foreach (IntVec3 c in cells)
			{
				this.RecalcAllInCell(c);
			}
		}

		// Token: 0x0600B1E4 RID: 45540 RVA: 0x0032D4EC File Offset: 0x0032B6EC
		public void RecalculateAllInHaulSources(IList<IHaulSource> sources)
		{
			foreach (IHaulSource haulSource in sources)
			{
				foreach (Thing t in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
				{
					this.Check(t);
				}
			}
		}

		// Token: 0x0600B1E5 RID: 45541 RVA: 0x0032D568 File Offset: 0x0032B768
		public void RecalculateAllInHaulSource(IHaulSource source)
		{
			foreach (Thing t in ((IEnumerable<Thing>)source.GetDirectlyHeldThings()))
			{
				this.Check(t);
			}
		}

		// Token: 0x0600B1E6 RID: 45542 RVA: 0x0032D5B8 File Offset: 0x0032B7B8
		private void Check(Thing t)
		{
			if (this.ShouldBeHaulable(t))
			{
				this.haulables.Add(t);
				return;
			}
			this.haulables.Remove(t);
		}

		// Token: 0x0600B1E7 RID: 45543 RVA: 0x0032D5E0 File Offset: 0x0032B7E0
		private bool ShouldBeHaulable(Thing t)
		{
			if (t.IsForbidden(Faction.OfPlayer))
			{
				return false;
			}
			if (!t.def.alwaysHaulable)
			{
				if (!t.def.EverHaulable)
				{
					return false;
				}
				if (this.map.designationManager.DesignationOn(t, DesignationDefOf.Haul) == null && !t.IsInAnyStorage())
				{
					return false;
				}
			}
			if (t.IsInValidBestStorage())
			{
				return false;
			}
			IHaulSource haulSource = t.ParentHolder as IHaulSource;
			return haulSource == null || haulSource.HaulSourceEnabled;
		}

		// Token: 0x0600B1E8 RID: 45544 RVA: 0x0032D65E File Offset: 0x0032B85E
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeHaulable(t))
			{
				this.haulables.Add(t);
			}
		}

		// Token: 0x0600B1E9 RID: 45545 RVA: 0x0032D676 File Offset: 0x0032B876
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item && this.haulables.Contains(t))
			{
				this.haulables.Remove(t);
			}
		}

		// Token: 0x0600B1EA RID: 45546 RVA: 0x0032D6A4 File Offset: 0x0032B8A4
		internal string DebugString()
		{
			if (Time.frameCount % 10 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("======= All haulables (Count " + this.haulables.Count.ToString() + ")");
				int num = 0;
				foreach (Thing thing in this.haulables)
				{
					stringBuilder.AppendLine(thing.ThingID);
					num++;
					if (num > 200)
					{
						break;
					}
				}
				this.debugOutput = stringBuilder.ToString();
			}
			return this.debugOutput;
		}

		// Token: 0x040079A0 RID: 31136
		private Map map;

		// Token: 0x040079A1 RID: 31137
		private HashSet<Thing> haulables = new HashSet<Thing>();

		// Token: 0x040079A2 RID: 31138
		private const int CellsPerTick = 4;

		// Token: 0x040079A3 RID: 31139
		private const int HaulSourcesPerTick = 4;

		// Token: 0x040079A4 RID: 31140
		private static int groupCycleIndex = 0;

		// Token: 0x040079A5 RID: 31141
		private static readonly List<int> cellCycleIndices = new List<int>();

		// Token: 0x040079A6 RID: 31142
		private string debugOutput = "uninitialized";
	}
}
