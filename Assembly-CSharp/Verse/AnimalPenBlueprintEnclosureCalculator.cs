using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020003A9 RID: 937
	public class AnimalPenBlueprintEnclosureCalculator
	{
		// Token: 0x06001803 RID: 6147 RVA: 0x00079C5E File Offset: 0x00077E5E
		public AnimalPenBlueprintEnclosureCalculator()
		{
			this.passCheck = new Predicate<IntVec3>(this.PassCheck);
			this.cellProcessor = new Func<IntVec3, bool>(this.CellProcessor);
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x00079C98 File Offset: 0x00077E98
		public void VisitPen(IntVec3 position, Map map)
		{
			int num = Gen.HashCombineInt(map.listerThings.StateHashOfGroup(ThingRequestGroup.Blueprint), map.listerThings.StateHashOfGroup(ThingRequestGroup.BuildingFrame), map.listerThings.StateHashOfGroup(ThingRequestGroup.BuildingArtificial), 42);
			if (this.map == null || this.map != map || !this.last_position.Equals(position) || this.last_stateHash != num)
			{
				this.map = map;
				this.last_position = position;
				this.last_stateHash = num;
				this.isEnclosed = true;
				this.cellsFound.Clear();
				this.FloodFill(position);
			}
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x00079D2F File Offset: 0x00077F2F
		private void FloodFill(IntVec3 position)
		{
			this.map.floodFiller.FloodFill(position, this.passCheck, this.cellProcessor, int.MaxValue, false, null);
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00079D55 File Offset: 0x00077F55
		private bool CellProcessor(IntVec3 c)
		{
			this.cellsFound.Add(c);
			if (c.OnEdge(this.map))
			{
				this.isEnclosed = false;
				return true;
			}
			return false;
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00079D7C File Offset: 0x00077F7C
		private bool PassCheck(IntVec3 c)
		{
			if (!c.WalkableByFenceBlocked(this.map))
			{
				return false;
			}
			foreach (Thing thing in c.GetThingList(this.map))
			{
				ThingDef def = thing.def;
				if (def.passability == Traversability.Impassable)
				{
					return false;
				}
				Building_Door building_Door = thing as Building_Door;
				if (building_Door != null)
				{
					if (AnimalPenEnclosureCalculator.RoamerCanPass(building_Door))
					{
						return true;
					}
					return false;
				}
				else if (def.IsBlueprint || def.IsFrame)
				{
					ThingDef thingDef = def.entityDefToBuild as ThingDef;
					if (thingDef != null)
					{
						if (thingDef.IsFence || thingDef.passability == Traversability.Impassable)
						{
							return false;
						}
						if (thingDef.IsDoor)
						{
							if (AnimalPenEnclosureCalculator.RoamerCanPass(thingDef))
							{
								return true;
							}
							return false;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0400157E RID: 5502
		private readonly Predicate<IntVec3> passCheck;

		// Token: 0x0400157F RID: 5503
		private readonly Func<IntVec3, bool> cellProcessor;

		// Token: 0x04001580 RID: 5504
		public bool isEnclosed;

		// Token: 0x04001581 RID: 5505
		public List<IntVec3> cellsFound = new List<IntVec3>();

		// Token: 0x04001582 RID: 5506
		private Map map;

		// Token: 0x04001583 RID: 5507
		private IntVec3 last_position;

		// Token: 0x04001584 RID: 5508
		private int last_stateHash;
	}
}
