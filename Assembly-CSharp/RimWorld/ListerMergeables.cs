using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F74 RID: 8052
	public class ListerMergeables
	{
		// Token: 0x0600B1EC RID: 45548 RVA: 0x0032D772 File Offset: 0x0032B972
		public ListerMergeables(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600B1ED RID: 45549 RVA: 0x0032D797 File Offset: 0x0032B997
		public List<Thing> ThingsPotentiallyNeedingMerging()
		{
			return this.mergeables;
		}

		// Token: 0x0600B1EE RID: 45550 RVA: 0x0032D79F File Offset: 0x0032B99F
		public void Notify_Spawned(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x0600B1EF RID: 45551 RVA: 0x0032D7A8 File Offset: 0x0032B9A8
		public void Notify_DeSpawned(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x0600B1F0 RID: 45552 RVA: 0x0032D79F File Offset: 0x0032B99F
		public void Notify_Unforbidden(Thing t)
		{
			this.CheckAdd(t);
		}

		// Token: 0x0600B1F1 RID: 45553 RVA: 0x0032D7A8 File Offset: 0x0032B9A8
		public void Notify_Forbidden(Thing t)
		{
			this.TryRemove(t);
		}

		// Token: 0x0600B1F2 RID: 45554 RVA: 0x0032D7B4 File Offset: 0x0032B9B4
		public void Notify_SlotGroupChanged(SlotGroup sg)
		{
			if (sg.CellsList != null)
			{
				for (int i = 0; i < sg.CellsList.Count; i++)
				{
					this.RecalcAllInCell(sg.CellsList[i]);
				}
			}
		}

		// Token: 0x0600B1F3 RID: 45555 RVA: 0x0032D7F1 File Offset: 0x0032B9F1
		public void Notify_ThingStackChanged(Thing t)
		{
			this.Check(t);
		}

		// Token: 0x0600B1F4 RID: 45556 RVA: 0x0032D7FC File Offset: 0x0032B9FC
		public void RecalcAllInCell(IntVec3 c)
		{
			List<Thing> thingList = c.GetThingList(this.map);
			for (int i = 0; i < thingList.Count; i++)
			{
				this.Check(thingList[i]);
			}
		}

		// Token: 0x0600B1F5 RID: 45557 RVA: 0x0032D834 File Offset: 0x0032BA34
		private void Check(Thing t)
		{
			if (this.ShouldBeMergeable(t))
			{
				if (!this.mergeables.Contains(t))
				{
					this.mergeables.Add(t);
					return;
				}
			}
			else
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x0600B1F6 RID: 45558 RVA: 0x0032D867 File Offset: 0x0032BA67
		private bool ShouldBeMergeable(Thing t)
		{
			return !t.IsForbidden(Faction.OfPlayer) && t.GetSlotGroup() != null && t.stackCount != t.def.stackLimit;
		}

		// Token: 0x0600B1F7 RID: 45559 RVA: 0x0032D898 File Offset: 0x0032BA98
		private void CheckAdd(Thing t)
		{
			if (this.ShouldBeMergeable(t) && !this.mergeables.Contains(t))
			{
				this.mergeables.Add(t);
			}
		}

		// Token: 0x0600B1F8 RID: 45560 RVA: 0x0032D8BD File Offset: 0x0032BABD
		private void TryRemove(Thing t)
		{
			if (t.def.category == ThingCategory.Item)
			{
				this.mergeables.Remove(t);
			}
		}

		// Token: 0x0600B1F9 RID: 45561 RVA: 0x0032D8DC File Offset: 0x0032BADC
		internal string DebugString()
		{
			if (Time.frameCount % 10 == 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("======= All mergeables (Count " + this.mergeables.Count.ToString() + ")");
				int num = 0;
				foreach (Thing thing in this.mergeables)
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

		// Token: 0x040079A7 RID: 31143
		private Map map;

		// Token: 0x040079A8 RID: 31144
		private List<Thing> mergeables = new List<Thing>();

		// Token: 0x040079A9 RID: 31145
		private string debugOutput = "uninitialized";
	}
}
