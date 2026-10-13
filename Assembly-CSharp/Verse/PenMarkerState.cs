using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020003AA RID: 938
	public class PenMarkerState
	{
		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001808 RID: 6152 RVA: 0x00079E74 File Offset: 0x00078074
		public bool Enclosed
		{
			get
			{
				return this.Calc().Enclosed;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x06001809 RID: 6153 RVA: 0x00079E81 File Offset: 0x00078081
		public bool Unenclosed
		{
			get
			{
				return !this.Enclosed;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x0600180A RID: 6154 RVA: 0x00079E8C File Offset: 0x0007808C
		public bool PassableDoors
		{
			get
			{
				return this.Calc().PassableDoors;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x0600180B RID: 6155 RVA: 0x00079E99 File Offset: 0x00078099
		public bool HasOutsideAccess
		{
			get
			{
				return !this.Enclosed || this.Calc().ImpassableDoors;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x0600180C RID: 6156 RVA: 0x00079EB0 File Offset: 0x000780B0
		public List<Region> DirectlyConnectedRegions
		{
			get
			{
				return this.Calc().DirectlyConnectedRegions;
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x0600180D RID: 6157 RVA: 0x00079EBD File Offset: 0x000780BD
		public HashSet<Region> ConnectedRegions
		{
			get
			{
				return this.Calc().ConnectedRegions;
			}
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x00079ECA File Offset: 0x000780CA
		public bool ContainsConnectedRegion(Region r)
		{
			return this.Calc().ContainsConnectedRegion(r);
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x00079ED8 File Offset: 0x000780D8
		public PenMarkerState(CompAnimalPenMarker marker)
		{
			this.marker = marker;
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x00079EE8 File Offset: 0x000780E8
		private AnimalPenEnclosureStateCalculator Calc()
		{
			if (this.state == null)
			{
				this.state = new AnimalPenEnclosureStateCalculator();
				this.state.Recalulate(this.marker.parent.Position, this.marker.parent.Map);
			}
			else if (this.state.NeedsRecalculation())
			{
				this.state.Recalulate(this.marker.parent.Position, this.marker.parent.Map);
			}
			return this.state;
		}

		// Token: 0x04001585 RID: 5509
		private readonly CompAnimalPenMarker marker;

		// Token: 0x04001586 RID: 5510
		private AnimalPenEnclosureStateCalculator state;
	}
}
