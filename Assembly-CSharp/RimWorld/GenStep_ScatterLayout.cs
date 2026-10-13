using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037E2 RID: 14306
	public class GenStep_ScatterLayout : GenStep_Scatterer
	{
		// Token: 0x1700329F RID: 12959
		// (get) Token: 0x06013D3F RID: 81215 RVA: 0x005E3318 File Offset: 0x005E1518
		public override int SeedPart
		{
			get
			{
				return 947153967;
			}
		}

		// Token: 0x06013D40 RID: 81216 RVA: 0x005E3320 File Offset: 0x005E1520
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			for (int i = 0; i < this.layout.Count; i++)
			{
				Thing t = GenSpawn.Spawn(ThingMaker.MakeThing(this.layout[i].thing, null), loc + this.layout[i].offset, map, this.layout[i].rotation, WipeMode.Vanish, false, false);
				if (this.layout[i].filthDef != null)
				{
					foreach (IntVec3 c in t.OccupiedRect().ExpandedBy(this.layout[i].filthExpandBy))
					{
						if (Rand.Chance(this.layout[i].filthChance) && c.InBounds(map))
						{
							FilthMaker.TryMakeFilth(c, map, this.layout[i].filthDef, 1, FilthSourceFlags.None, true);
						}
					}
				}
			}
		}

		// Token: 0x06013D41 RID: 81217 RVA: 0x005E3444 File Offset: 0x005E1644
		protected override bool CanScatterAt(IntVec3 loc, Map map)
		{
			if (!base.CanScatterAt(loc, map))
			{
				return false;
			}
			for (int i = 0; i < this.layout.Count; i++)
			{
				if (!this.CanSpawn(this.layout[i].thing, map, loc + this.layout[i].offset, this.layout[i].rotation))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06013D42 RID: 81218 RVA: 0x005E34B8 File Offset: 0x005E16B8
		private bool CanSpawn(ThingDef def, Map map, IntVec3 cell, Rot4 rot)
		{
			CellRect cellRect = GenAdj.OccupiedRect(cell, rot, def.size);
			if (!cellRect.InBounds(map))
			{
				return false;
			}
			foreach (IntVec3 intVec in cellRect)
			{
				if (this.checkEveryLayoutCell && !base.CanScatterAt(intVec, map))
				{
					return false;
				}
				List<Thing> thingList = intVec.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (!thingList[i].def.destroyable || !thingList[i].def.canScatterOver)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0400DBA4 RID: 56228
		public List<LayoutThing> layout;

		// Token: 0x0400DBA5 RID: 56229
		public bool checkEveryLayoutCell = true;
	}
}
