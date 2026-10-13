using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008B8 RID: 2232
	public class JitterHandler
	{
		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x0600386D RID: 14445 RVA: 0x0012CBE0 File Offset: 0x0012ADE0
		public Vector3 CurrentOffset
		{
			get
			{
				return this.curOffset;
			}
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x0012CBE8 File Offset: 0x0012ADE8
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			float num = (float)ticksPassed * this.JitterDropPerTick;
			if (this.curOffset.sqrMagnitude < num * num)
			{
				this.curOffset = new Vector3(0f, 0f, 0f);
				return;
			}
			this.curOffset -= this.curOffset.normalized * num;
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x0012CC4C File Offset: 0x0012AE4C
		public void Notify_DamageApplied(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DamageJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x0012CC6F File Offset: 0x0012AE6F
		public void Notify_DamageDeflected(DamageInfo dinfo)
		{
			if (dinfo.Def.hasForcefulImpact)
			{
				this.AddOffset(this.DeflectJitterDistance, dinfo.Angle);
			}
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x0012CC94 File Offset: 0x0012AE94
		public void AddOffset(float dist, float dir)
		{
			this.curOffset += Quaternion.AngleAxis(dir, Vector3.up) * Vector3.forward * dist;
			if (this.curOffset.sqrMagnitude > this.JitterMax * this.JitterMax)
			{
				this.curOffset *= this.JitterMax / this.curOffset.magnitude;
			}
		}

		// Token: 0x040029C7 RID: 10695
		private Vector3 curOffset = new Vector3(0f, 0f, 0f);

		// Token: 0x040029C8 RID: 10696
		private float DamageJitterDistance = 0.17f;

		// Token: 0x040029C9 RID: 10697
		private float DeflectJitterDistance = 0.1f;

		// Token: 0x040029CA RID: 10698
		private float JitterDropPerTick = 0.018f;

		// Token: 0x040029CB RID: 10699
		private float JitterMax = 0.35f;
	}
}
