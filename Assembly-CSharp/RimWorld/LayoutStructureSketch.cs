using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EBC RID: 7868
	public class LayoutStructureSketch : IExposable, ILoadReferenceable
	{
		// Token: 0x0600AE81 RID: 44673 RVA: 0x0031C833 File Offset: 0x0031AA33
		public string GetUniqueLoadID()
		{
			return this.uniqueId;
		}

		// Token: 0x0600AE82 RID: 44674 RVA: 0x0031C83C File Offset: 0x0031AA3C
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.thingDiscoveredMessage, "thingDiscoveredMessage", null, false);
			Scribe_Values.Look<bool>(ref this.spawned, "spawned", false, false);
			Scribe_Values.Look<IntVec3>(ref this.center, "center", default(IntVec3), false);
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<string>(ref this.uniqueId, "uniqueId", null, false);
			Scribe_Defs.Look<LayoutDef>(ref this.layoutDef, "layoutDef");
			Scribe_Deep.Look<LayoutSketch>(ref this.layoutSketch, "layoutSketch", Array.Empty<object>());
			Scribe_Deep.Look<StructureLayout>(ref this.structureLayout, "structureLayout", new object[]
			{
				this
			});
			Scribe_Collections.Look<Thing>(ref this.thingsToSpawn, "thingsToSpawn", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars && string.IsNullOrEmpty(this.uniqueId))
			{
				this.uniqueId = string.Format("LayoutStructureSketch_{0}", Find.UniqueIDsManager.GetNextStructureSketchID());
			}
		}

		// Token: 0x0600AE83 RID: 44675 RVA: 0x0031C934 File Offset: 0x0031AB34
		public bool AnyRoomContains(IntVec3 c, LayoutRoom exclude = null)
		{
			foreach (LayoutRoom layoutRoom in this.structureLayout.Rooms)
			{
				if (layoutRoom != exclude && layoutRoom.Contains(c, 0))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600AE84 RID: 44676 RVA: 0x0031C99C File Offset: 0x0031AB9C
		public float ClosestDistTo(IntVec3 c)
		{
			float num = float.MaxValue;
			foreach (LayoutRoom layoutRoom in this.structureLayout.Rooms)
			{
				foreach (CellRect cellRect in layoutRoom.rects)
				{
					num = Mathf.Min(cellRect.ClosestDistanceTo(c), num);
				}
			}
			return num;
		}

		// Token: 0x040077FD RID: 30717
		public LayoutSketch layoutSketch;

		// Token: 0x040077FE RID: 30718
		public StructureLayout structureLayout;

		// Token: 0x040077FF RID: 30719
		public List<Thing> thingsToSpawn = new List<Thing>();

		// Token: 0x04007800 RID: 30720
		public string thingDiscoveredMessage;

		// Token: 0x04007801 RID: 30721
		public LayoutDef layoutDef;

		// Token: 0x04007802 RID: 30722
		public bool spawned;

		// Token: 0x04007803 RID: 30723
		public IntVec3 center;

		// Token: 0x04007804 RID: 30724
		public int id;

		// Token: 0x04007805 RID: 30725
		public string uniqueId;
	}
}
