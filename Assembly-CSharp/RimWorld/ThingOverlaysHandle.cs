using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002970 RID: 10608
	public class ThingOverlaysHandle
	{
		// Token: 0x17002380 RID: 9088
		// (get) Token: 0x0600E8E2 RID: 59618 RVA: 0x0043CBDC File Offset: 0x0043ADDC
		public OverlayTypes OverlayTypes
		{
			get
			{
				if (this.overlayTypesDirty)
				{
					this.overlayTypes = OverlayTypes.None;
					for (int i = 0; i < this.overlayHandles.Count; i++)
					{
						this.overlayTypes |= this.overlayHandles[i].overlayType;
					}
					this.overlayTypesDirty = false;
				}
				return this.overlayTypes;
			}
		}

		// Token: 0x0600E8E3 RID: 59619 RVA: 0x0043CC39 File Offset: 0x0043AE39
		public ThingOverlaysHandle(OverlayDrawer drawer, Thing thing)
		{
			this.drawer = drawer;
			this.thing = thing;
		}

		// Token: 0x0600E8E4 RID: 59620 RVA: 0x0043CC5C File Offset: 0x0043AE5C
		public OverlayHandle Enable(OverlayTypes overlayType)
		{
			int num = this.handleIdCounter;
			this.handleIdCounter = num + 1;
			OverlayHandle overlayHandle = new OverlayHandle(this, overlayType, num);
			this.overlayHandles.Add(overlayHandle);
			this.overlayTypesDirty = true;
			return overlayHandle;
		}

		// Token: 0x0600E8E5 RID: 59621 RVA: 0x0043CC98 File Offset: 0x0043AE98
		public void Disable(OverlayHandle handle)
		{
			for (int i = this.overlayHandles.Count - 1; i >= 0; i--)
			{
				if (this.overlayHandles[i].handleId == handle.handleId)
				{
					this.overlayHandles.RemoveAt(i);
				}
			}
			this.overlayTypesDirty = true;
		}

		// Token: 0x0600E8E6 RID: 59622 RVA: 0x0043CCE9 File Offset: 0x0043AEE9
		public void Disable(ref OverlayHandle? handle)
		{
			if (handle != null)
			{
				this.Disable(handle.Value);
				handle = null;
			}
		}

		// Token: 0x0600E8E7 RID: 59623 RVA: 0x0043CD06 File Offset: 0x0043AF06
		public void Dispose()
		{
			if (this.disposed)
			{
				Log.Error("Tried disposing already disposed ThingOverlaysHandle!");
				return;
			}
			this.disposed = true;
		}

		// Token: 0x04009B20 RID: 39712
		public readonly OverlayDrawer drawer;

		// Token: 0x04009B21 RID: 39713
		public readonly Thing thing;

		// Token: 0x04009B22 RID: 39714
		private bool disposed;

		// Token: 0x04009B23 RID: 39715
		private int handleIdCounter;

		// Token: 0x04009B24 RID: 39716
		private bool overlayTypesDirty;

		// Token: 0x04009B25 RID: 39717
		private OverlayTypes overlayTypes;

		// Token: 0x04009B26 RID: 39718
		private List<OverlayHandle> overlayHandles = new List<OverlayHandle>();
	}
}
