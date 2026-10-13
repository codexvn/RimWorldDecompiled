using System;

namespace Verse
{
	// Token: 0x02000D65 RID: 3429
	public abstract class SpecialThingFilterWorker
	{
		// Token: 0x06005738 RID: 22328
		public abstract bool Matches(Thing t);

		// Token: 0x06005739 RID: 22329 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool AlwaysMatches(ThingDef def)
		{
			return false;
		}

		// Token: 0x0600573A RID: 22330 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanEverMatch(ThingDef def)
		{
			return true;
		}
	}
}
