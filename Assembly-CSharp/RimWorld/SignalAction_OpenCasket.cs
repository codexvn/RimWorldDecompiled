using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AC1 RID: 10945
	public class SignalAction_OpenCasket : SignalAction_Delay
	{
		// Token: 0x17002620 RID: 9760
		// (get) Token: 0x0600F3DB RID: 62427 RVA: 0x0046D9CD File Offset: 0x0046BBCD
		public override Alert_ActionDelay Alert
		{
			get
			{
				if (this.cachedAlert == null)
				{
					this.cachedAlert = new Alert_CasketOpening(this);
				}
				return this.cachedAlert;
			}
		}

		// Token: 0x17002621 RID: 9761
		// (get) Token: 0x0600F3DC RID: 62428 RVA: 0x0046D9EC File Offset: 0x0046BBEC
		public override bool ShouldRemoveNow
		{
			get
			{
				for (int i = 0; i < this.caskets.Count; i++)
				{
					if (this.caskets[i] != null && !this.caskets[i].Destroyed && ((Building_Casket)this.caskets[i]).HasAnyContents)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x0600F3DD RID: 62429 RVA: 0x0046DA4C File Offset: 0x0046BC4C
		protected override void Complete()
		{
			base.Complete();
			for (int i = 0; i < this.caskets.Count; i++)
			{
				Building_Casket building_Casket = (Building_Casket)this.caskets[i];
				if (building_Casket.CanOpen)
				{
					building_Casket.Open();
				}
			}
		}

		// Token: 0x0600F3DE RID: 62430 RVA: 0x0046DA95 File Offset: 0x0046BC95
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Thing>(ref this.caskets, "caskets", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x0400A0AF RID: 41135
		public List<Thing> caskets = new List<Thing>();

		// Token: 0x0400A0B0 RID: 41136
		private Alert_ActionDelay cachedAlert;
	}
}
