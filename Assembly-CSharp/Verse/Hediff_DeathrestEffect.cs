using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000743 RID: 1859
	public class Hediff_DeathrestEffect : Hediff
	{
		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06002D67 RID: 11623 RVA: 0x000EA2B8 File Offset: 0x000E84B8
		public override string LabelBase
		{
			get
			{
				return base.LabelBase + " x" + Mathf.FloorToInt(this.Severity).ToString();
			}
		}
	}
}
