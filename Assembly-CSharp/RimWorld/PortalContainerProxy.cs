using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002963 RID: 10595
	public class PortalContainerProxy : ThingOwner
	{
		// Token: 0x17002372 RID: 9074
		// (get) Token: 0x0600E88D RID: 59533 RVA: 0x00002501 File Offset: 0x00000701
		public override int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600E88E RID: 59534 RVA: 0x0043B4D0 File Offset: 0x004396D0
		public override int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true)
		{
			if (this.TryAdd(item, canMergeWithExistingStacks))
			{
				return count;
			}
			return 0;
		}

		// Token: 0x0600E88F RID: 59535 RVA: 0x0043B4E0 File Offset: 0x004396E0
		public override bool TryAdd(Thing item, bool canMergeWithExistingStacks = true)
		{
			Map otherMap = this.portal.GetOtherMap();
			IntVec3 destinationLocation = this.portal.GetDestinationLocation();
			this.portal.Notify_ThingAdded(item);
			Thing thing;
			GenDrop.TryDropSpawn(item, destinationLocation, otherMap, ThingPlaceMode.Near, out thing, null, null, true);
			return true;
		}

		// Token: 0x0600E890 RID: 59536 RVA: 0x0000F9E0 File Offset: 0x0000DBE0
		public override int IndexOf(Thing item)
		{
			return -1;
		}

		// Token: 0x0600E891 RID: 59537 RVA: 0x00002501 File Offset: 0x00000701
		public override bool Remove(Thing item)
		{
			return false;
		}

		// Token: 0x0600E892 RID: 59538 RVA: 0x00002C42 File Offset: 0x00000E42
		protected override Thing GetAt(int index)
		{
			return null;
		}

		// Token: 0x04009AF0 RID: 39664
		public MapPortal portal;
	}
}
