using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026AD RID: 9901
	public abstract class StageFailTrigger : IExposable
	{
		// Token: 0x0600D931 RID: 55601 RVA: 0x00405264 File Offset: 0x00403464
		public string Reason(LordJob_Ritual ritual, TargetInfo spot)
		{
			return this.desc;
		}

		// Token: 0x0600D932 RID: 55602
		public abstract bool Failed(LordJob_Ritual ritual, TargetInfo spot, TargetInfo focus);

		// Token: 0x0600D933 RID: 55603 RVA: 0x0040526C File Offset: 0x0040346C
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.desc, "desc", null, false);
			Scribe_Values.Look<int>(ref this.allowanceTicks, "allowanceTicks", 0, false);
		}

		// Token: 0x0400932C RID: 37676
		[MustTranslate]
		public string desc;

		// Token: 0x0400932D RID: 37677
		public int allowanceTicks;
	}
}
