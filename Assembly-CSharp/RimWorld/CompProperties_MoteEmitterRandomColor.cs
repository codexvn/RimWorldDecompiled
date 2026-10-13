using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D6B RID: 11627
	public class CompProperties_MoteEmitterRandomColor : CompProperties_MoteEmitter
	{
		// Token: 0x0601055F RID: 66911 RVA: 0x004B7432 File Offset: 0x004B5632
		public CompProperties_MoteEmitterRandomColor()
		{
			this.compClass = typeof(CompMoteEmitterRandomColor);
		}

		// Token: 0x06010560 RID: 66912 RVA: 0x004B744A File Offset: 0x004B564A
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.colors.NullOrEmpty<Color>())
			{
				yield return "colors list is empty or null";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400AA6B RID: 43627
		public List<Color> colors;
	}
}
