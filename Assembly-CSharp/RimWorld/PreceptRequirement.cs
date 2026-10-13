using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200267A RID: 9850
	public abstract class PreceptRequirement : IExposable
	{
		// Token: 0x0600D846 RID: 55366
		public abstract bool Met(List<Precept> precepts);

		// Token: 0x0600D847 RID: 55367
		public abstract Precept MakePrecept(Ideo ideo);

		// Token: 0x0600D848 RID: 55368 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExposeData()
		{
		}
	}
}
