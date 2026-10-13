using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020006AB RID: 1707
	public class HediffCompProperties_Disorientation : HediffCompProperties
	{
		// Token: 0x06002AD4 RID: 10964 RVA: 0x000E0BF5 File Offset: 0x000DEDF5
		public HediffCompProperties_Disorientation()
		{
			this.compClass = typeof(HediffComp_Disorientation);
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x000E0C1F File Offset: 0x000DEE1F
		public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.wanderMtbHours <= 0f)
			{
				yield return "wanderMtbHours must be greater than zero";
			}
			if (this.singleWanderDurationTicks <= 0)
			{
				yield return "singleWanderDurationTicks must be greater than zero";
			}
			if (this.wanderRadius <= 0f)
			{
				yield return "wanderRadius must be greater than zero";
			}
			yield break;
			yield break;
		}

		// Token: 0x0400229A RID: 8858
		public float wanderMtbHours = -1f;

		// Token: 0x0400229B RID: 8859
		public float wanderRadius;

		// Token: 0x0400229C RID: 8860
		public int singleWanderDurationTicks = -1;
	}
}
