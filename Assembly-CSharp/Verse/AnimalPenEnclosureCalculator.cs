using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003A6 RID: 934
	public abstract class AnimalPenEnclosureCalculator
	{
		// Token: 0x060017E2 RID: 6114 RVA: 0x0007983F File Offset: 0x00077A3F
		protected AnimalPenEnclosureCalculator()
		{
			this.regionProcessor = new RegionProcessor(this.ProcessRegion);
			this.regionEntryPredicate = new RegionEntryPredicate(this.EnterRegion);
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void VisitDirectlyConnectedRegion(Region r)
		{
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void VisitIndirectlyDirectlyConnectedRegion(Region r)
		{
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void VisitPassableDoorway(Region r)
		{
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void VisitImpassableDoorway(Region r)
		{
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x0007986C File Offset: 0x00077A6C
		protected bool VisitPen(IntVec3 position, Map map)
		{
			this.rootDistrict = position.GetDistrict(map, RegionType.Set_Passable);
			if (this.rootDistrict == null || this.rootDistrict.TouchesMapEdge)
			{
				return false;
			}
			this.isEnclosed = true;
			RegionTraverser.BreadthFirstTraverse(position, map, this.regionEntryPredicate, this.regionProcessor, 999999, RegionType.Set_Passable);
			return this.isEnclosed;
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x000798C6 File Offset: 0x00077AC6
		public static bool RoamerCanPass(Building_Door door)
		{
			return door.FreePassage || AnimalPenEnclosureCalculator.RoamerCanPass(door.def);
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x000798DD File Offset: 0x00077ADD
		public static bool RoamerCanPass(ThingDef thingDef)
		{
			return thingDef.building.roamerCanOpen;
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x000798EA File Offset: 0x00077AEA
		private bool EnterRegion(Region from, Region to)
		{
			return (!from.IsDoorway || AnimalPenEnclosureCalculator.RoamerCanPass(from.door)) && (to.type == RegionType.Normal || to.IsDoorway);
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x00079914 File Offset: 0x00077B14
		private bool ProcessRegion(Region reg)
		{
			if (reg.touchesMapEdge)
			{
				this.isEnclosed = false;
				return true;
			}
			if (reg.type == RegionType.Normal)
			{
				if (reg.District == this.rootDistrict)
				{
					this.VisitDirectlyConnectedRegion(reg);
				}
				else
				{
					this.VisitIndirectlyDirectlyConnectedRegion(reg);
				}
			}
			else if (reg.IsDoorway)
			{
				if (AnimalPenEnclosureCalculator.RoamerCanPass(reg.door))
				{
					this.VisitPassableDoorway(reg);
				}
				else
				{
					this.VisitImpassableDoorway(reg);
				}
			}
			return false;
		}

		// Token: 0x04001571 RID: 5489
		private District rootDistrict;

		// Token: 0x04001572 RID: 5490
		private bool isEnclosed;

		// Token: 0x04001573 RID: 5491
		private readonly RegionProcessor regionProcessor;

		// Token: 0x04001574 RID: 5492
		private readonly RegionEntryPredicate regionEntryPredicate;
	}
}
