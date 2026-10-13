using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AA1 RID: 10913
	public class IncineratorSpray : Thing
	{
		// Token: 0x17002602 RID: 9730
		// (get) Token: 0x0600F31F RID: 62239 RVA: 0x00469653 File Offset: 0x00467853
		private int NumAlive
		{
			get
			{
				return this.projectiles.Count;
			}
		}

		// Token: 0x17002603 RID: 9731
		// (get) Token: 0x0600F320 RID: 62240 RVA: 0x00469660 File Offset: 0x00467860
		private bool AllComplete
		{
			get
			{
				return this.numComplete >= this.NumAlive;
			}
		}

		// Token: 0x0600F321 RID: 62241 RVA: 0x00469674 File Offset: 0x00467874
		protected override void Tick()
		{
			int i = 0;
			int count = this.projectiles.Count;
			while (i < count)
			{
				IncineratorProjectileMotion value = this.projectiles[i];
				int num = Mathf.Approximately(value.Alpha, 1f) ? 1 : 0;
				value.Tick(base.MapHeld);
				bool flag = Mathf.Approximately(value.Alpha, 1f);
				this.projectiles[i] = value;
				if (num == 0 && flag)
				{
					this.numComplete++;
				}
				i++;
			}
			if (this.AllComplete)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600F322 RID: 62242 RVA: 0x00469707 File Offset: 0x00467907
		public void Add(IncineratorProjectileMotion proj)
		{
			this.projectiles.Add(proj);
		}

		// Token: 0x0400A020 RID: 40992
		private List<IncineratorProjectileMotion> projectiles = new List<IncineratorProjectileMotion>();

		// Token: 0x0400A021 RID: 40993
		private int numComplete;
	}
}
