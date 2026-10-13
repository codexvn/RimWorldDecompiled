using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020003A8 RID: 936
	public class AnimalPenEnclosureStateCalculator : AnimalPenEnclosureCalculator
	{
		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x060017F5 RID: 6133 RVA: 0x00079A90 File Offset: 0x00077C90
		public bool Enclosed
		{
			get
			{
				return this.enclosed;
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x060017F6 RID: 6134 RVA: 0x00079A98 File Offset: 0x00077C98
		public bool IndirectlyConnected
		{
			get
			{
				return this.indirectlyConnected;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x060017F7 RID: 6135 RVA: 0x00079AA0 File Offset: 0x00077CA0
		public bool PassableDoors
		{
			get
			{
				return this.passableDoors.Any<Building_Door>();
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x060017F8 RID: 6136 RVA: 0x00079AAD File Offset: 0x00077CAD
		public bool ImpassableDoors
		{
			get
			{
				return this.impassableDoors.Any<Building_Door>();
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x060017F9 RID: 6137 RVA: 0x00079ABA File Offset: 0x00077CBA
		public List<Region> DirectlyConnectedRegions
		{
			get
			{
				return this.directlyConnectedRegions;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x00079AC2 File Offset: 0x00077CC2
		public HashSet<Region> ConnectedRegions
		{
			get
			{
				return this.connectedRegions;
			}
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00079ACA File Offset: 0x00077CCA
		public bool ContainsConnectedRegion(Region r)
		{
			return this.connectedRegions.Contains(r);
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00079AD8 File Offset: 0x00077CD8
		public bool NeedsRecalculation()
		{
			using (List<Building_Door>.Enumerator enumerator = this.passableDoors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!AnimalPenEnclosureCalculator.RoamerCanPass(enumerator.Current))
					{
						return true;
					}
				}
			}
			using (List<Building_Door>.Enumerator enumerator = this.impassableDoors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (AnimalPenEnclosureCalculator.RoamerCanPass(enumerator.Current))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x00079B78 File Offset: 0x00077D78
		public void Recalulate(IntVec3 position, Map map)
		{
			this.indirectlyConnected = false;
			this.passableDoors.Clear();
			this.impassableDoors.Clear();
			this.connectedRegions.Clear();
			this.directlyConnectedRegions.Clear();
			this.enclosed = base.VisitPen(position, map);
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x00079BC6 File Offset: 0x00077DC6
		protected override void VisitDirectlyConnectedRegion(Region r)
		{
			this.connectedRegions.Add(r);
			this.directlyConnectedRegions.Add(r);
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x00079BE1 File Offset: 0x00077DE1
		protected override void VisitIndirectlyDirectlyConnectedRegion(Region r)
		{
			this.indirectlyConnected = true;
			this.connectedRegions.Add(r);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x00079BF7 File Offset: 0x00077DF7
		protected override void VisitPassableDoorway(Region r)
		{
			this.connectedRegions.Add(r);
			this.passableDoors.Add(r.door);
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x00079C17 File Offset: 0x00077E17
		protected override void VisitImpassableDoorway(Region r)
		{
			this.impassableDoors.Add(r.door);
		}

		// Token: 0x04001578 RID: 5496
		private bool enclosed;

		// Token: 0x04001579 RID: 5497
		private bool indirectlyConnected;

		// Token: 0x0400157A RID: 5498
		private List<Building_Door> passableDoors = new List<Building_Door>();

		// Token: 0x0400157B RID: 5499
		private List<Building_Door> impassableDoors = new List<Building_Door>();

		// Token: 0x0400157C RID: 5500
		private List<Region> directlyConnectedRegions = new List<Region>();

		// Token: 0x0400157D RID: 5501
		private HashSet<Region> connectedRegions = new HashSet<Region>();
	}
}
