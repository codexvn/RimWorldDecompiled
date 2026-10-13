using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200087A RID: 2170
	public class MechShield : ThingWithComps
	{
		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x0600370A RID: 14090 RVA: 0x001274E4 File Offset: 0x001256E4
		public override Vector3 DrawPos
		{
			get
			{
				if (this.target == null)
				{
					return base.DrawPos;
				}
				return this.target.DrawPos;
			}
		}

		// Token: 0x0600370B RID: 14091 RVA: 0x00127500 File Offset: 0x00125700
		public void SetTarget(Thing target)
		{
			this.target = target;
		}

		// Token: 0x0600370C RID: 14092 RVA: 0x00127509 File Offset: 0x00125709
		public bool IsTargeting(Thing target)
		{
			return this.target == target;
		}

		// Token: 0x0600370D RID: 14093 RVA: 0x00127514 File Offset: 0x00125714
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.Comps_PostDraw();
		}

		// Token: 0x0600370E RID: 14094 RVA: 0x0012751C File Offset: 0x0012571C
		protected override void Tick()
		{
			base.Tick();
			if (this.target != null)
			{
				base.Position = this.target.Position;
			}
		}

		// Token: 0x0600370F RID: 14095 RVA: 0x0012753D File Offset: 0x0012573D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.target, "target", false);
		}

		// Token: 0x0400294F RID: 10575
		private Thing target;
	}
}
