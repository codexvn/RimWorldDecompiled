using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200250D RID: 9485
	public class OutfitForcedHandler : IExposable
	{
		// Token: 0x17001FCA RID: 8138
		// (get) Token: 0x0600CF35 RID: 53045 RVA: 0x003CD306 File Offset: 0x003CB506
		public bool SomethingIsForced
		{
			get
			{
				return this.forcedAps.Count > 0;
			}
		}

		// Token: 0x17001FCB RID: 8139
		// (get) Token: 0x0600CF36 RID: 53046 RVA: 0x003CD316 File Offset: 0x003CB516
		public List<Apparel> ForcedApparel
		{
			get
			{
				return this.forcedAps;
			}
		}

		// Token: 0x0600CF37 RID: 53047 RVA: 0x003CD31E File Offset: 0x003CB51E
		public void Reset()
		{
			this.forcedAps.Clear();
		}

		// Token: 0x0600CF38 RID: 53048 RVA: 0x003CD32B File Offset: 0x003CB52B
		public bool AllowedToAutomaticallyDrop(Apparel ap)
		{
			return !this.forcedAps.Contains(ap);
		}

		// Token: 0x0600CF39 RID: 53049 RVA: 0x003CD33C File Offset: 0x003CB53C
		public void SetForced(Apparel ap, bool forced)
		{
			if (forced)
			{
				if (!this.forcedAps.Contains(ap))
				{
					this.forcedAps.Add(ap);
					return;
				}
			}
			else if (this.forcedAps.Contains(ap))
			{
				this.forcedAps.Remove(ap);
			}
		}

		// Token: 0x0600CF3A RID: 53050 RVA: 0x003CD377 File Offset: 0x003CB577
		public void ExposeData()
		{
			Scribe_Collections.Look<Apparel>(ref this.forcedAps, "forcedAps", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x0600CF3B RID: 53051 RVA: 0x003CD390 File Offset: 0x003CB590
		public bool IsForced(Apparel ap)
		{
			if (ap.Destroyed)
			{
				Log.Error("Apparel was forced while Destroyed: " + ((ap != null) ? ap.ToString() : null));
				if (this.forcedAps.Contains(ap))
				{
					this.forcedAps.Remove(ap);
				}
				return false;
			}
			return this.forcedAps.Contains(ap);
		}

		// Token: 0x04008CF7 RID: 36087
		private List<Apparel> forcedAps = new List<Apparel>();
	}
}
