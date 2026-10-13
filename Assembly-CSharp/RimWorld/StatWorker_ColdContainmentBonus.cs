using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200373C RID: 14140
	public class StatWorker_ColdContainmentBonus : StatWorker
	{
		// Token: 0x060139BD RID: 80317 RVA: 0x005D2990 File Offset: 0x005D0B90
		public override bool ShouldShowFor(StatRequest req)
		{
			float num;
			return ContainmentUtility.ShowContainmentStats(req.Thing) && ContainmentUtility.TryGetColdContainmentBonus(req.Thing, out num);
		}

		// Token: 0x060139BE RID: 80318 RVA: 0x005D29BC File Offset: 0x005D0BBC
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			float num;
			ContainmentUtility.TryGetColdContainmentBonus(req.Thing, out num);
			return num - 1f;
		}
	}
}
