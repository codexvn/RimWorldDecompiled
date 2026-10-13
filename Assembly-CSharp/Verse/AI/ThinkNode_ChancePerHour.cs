using System;

namespace Verse.AI
{
	// Token: 0x02000FD6 RID: 4054
	public abstract class ThinkNode_ChancePerHour : ThinkNode_Priority
	{
		// Token: 0x0600642B RID: 25643 RVA: 0x001FCFA4 File Offset: 0x001FB1A4
		public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
		{
			if (Find.TickManager.TicksGame < this.GetLastTryTick(pawn) + 2500)
			{
				return ThinkResult.NoJob;
			}
			this.SetLastTryTick(pawn, Find.TickManager.TicksGame);
			float num = this.MtbHours(pawn);
			if (num <= 0f)
			{
				return ThinkResult.NoJob;
			}
			Rand.PushState();
			int salt = Gen.HashCombineInt(base.UniqueSaveKey, 26504059);
			Rand.Seed = pawn.RandSeedForHour(salt);
			bool flag = Rand.MTBEventOccurs(num, 2500f, 2500f);
			Rand.PopState();
			if (flag)
			{
				return base.TryIssueJobPackage(pawn, jobParams);
			}
			return ThinkResult.NoJob;
		}

		// Token: 0x0600642C RID: 25644
		protected abstract float MtbHours(Pawn pawn);

		// Token: 0x0600642D RID: 25645 RVA: 0x001FD040 File Offset: 0x001FB240
		private int GetLastTryTick(Pawn pawn)
		{
			int result;
			if (pawn.mindState.thinkData.TryGetValue(base.UniqueSaveKey, out result))
			{
				return result;
			}
			return -99999;
		}

		// Token: 0x0600642E RID: 25646 RVA: 0x001FD06E File Offset: 0x001FB26E
		private void SetLastTryTick(Pawn pawn, int val)
		{
			pawn.mindState.thinkData[base.UniqueSaveKey] = val;
		}
	}
}
