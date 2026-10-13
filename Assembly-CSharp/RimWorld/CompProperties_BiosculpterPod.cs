using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C0D RID: 11277
	public class CompProperties_BiosculpterPod : CompProperties
	{
		// Token: 0x0600FCB9 RID: 64697 RVA: 0x004946C3 File Offset: 0x004928C3
		public CompProperties_BiosculpterPod()
		{
			this.compClass = typeof(CompBiosculpterPod);
		}

		// Token: 0x0600FCBA RID: 64698 RVA: 0x004946DB File Offset: 0x004928DB
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return base.GetType().Name + " requires parent ticker type Normal";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400A59E RID: 42398
		public SoundDef enterSound;

		// Token: 0x0400A59F RID: 42399
		public SoundDef exitSound;

		// Token: 0x0400A5A0 RID: 42400
		public EffecterDef operatingEffecter;

		// Token: 0x0400A5A1 RID: 42401
		public EffecterDef readyEffecter;

		// Token: 0x0400A5A2 RID: 42402
		public Color selectCycleColor;

		// Token: 0x0400A5A3 RID: 42403
		public float biotunedCycleSpeedFactor;
	}
}
