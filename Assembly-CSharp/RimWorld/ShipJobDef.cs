using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AE2 RID: 6882
	public class ShipJobDef : Def
	{
		// Token: 0x060099CC RID: 39372 RVA: 0x002CC407 File Offset: 0x002CA607
		public override IEnumerable<string> ConfigErrors()
		{
			IEnumerable<string> enumerable = base.ConfigErrors();
			foreach (string text in enumerable)
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(ShipJob).IsAssignableFrom(this.jobClass))
			{
				yield return this.jobClass.Name + " does not inherit from ShipJob";
			}
			yield break;
			yield break;
		}

		// Token: 0x04006976 RID: 26998
		public Type jobClass;

		// Token: 0x04006977 RID: 26999
		public bool blocksDisposalIfQueuedUnspawned = true;
	}
}
