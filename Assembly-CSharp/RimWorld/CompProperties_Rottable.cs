using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001980 RID: 6528
	public class CompProperties_Rottable : CompProperties
	{
		// Token: 0x1700167D RID: 5757
		// (get) Token: 0x0600941E RID: 37918 RVA: 0x002B867B File Offset: 0x002B687B
		public int TicksToRotStart
		{
			get
			{
				return Mathf.RoundToInt(this.daysToRotStart * 60000f);
			}
		}

		// Token: 0x1700167E RID: 5758
		// (get) Token: 0x0600941F RID: 37919 RVA: 0x002B868E File Offset: 0x002B688E
		public int TicksToDessicated
		{
			get
			{
				return Mathf.RoundToInt(this.daysToDessicated * 60000f);
			}
		}

		// Token: 0x06009420 RID: 37920 RVA: 0x002B86A1 File Offset: 0x002B68A1
		public CompProperties_Rottable()
		{
			this.compClass = typeof(CompRottable);
		}

		// Token: 0x06009421 RID: 37921 RVA: 0x002B86DA File Offset: 0x002B68DA
		public CompProperties_Rottable(float daysToRotStart)
		{
			this.daysToRotStart = daysToRotStart;
		}

		// Token: 0x06009422 RID: 37922 RVA: 0x002B870A File Offset: 0x002B690A
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal && parentDef.tickerType != TickerType.Rare)
			{
				yield return string.Concat(new string[]
				{
					"CompRottable needs tickerType ",
					TickerType.Rare.ToString(),
					" or ",
					TickerType.Normal.ToString(),
					", has ",
					parentDef.tickerType.ToString()
				});
			}
			yield break;
			yield break;
		}

		// Token: 0x04005FA5 RID: 24485
		public float daysToRotStart = 2f;

		// Token: 0x04005FA6 RID: 24486
		public bool rotDestroys;

		// Token: 0x04005FA7 RID: 24487
		public float rotDamagePerDay = 40f;

		// Token: 0x04005FA8 RID: 24488
		public float daysToDessicated = 999f;

		// Token: 0x04005FA9 RID: 24489
		public float dessicatedDamagePerDay;

		// Token: 0x04005FAA RID: 24490
		public bool disableIfHatcher;
	}
}
