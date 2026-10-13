using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002198 RID: 8600
	public class TreeDestructionTracker : IExposable
	{
		// Token: 0x17001C89 RID: 7305
		// (get) Token: 0x0600BBD5 RID: 48085 RVA: 0x00366B56 File Offset: 0x00364D56
		public int PlayerResponsibleTreeDestructionCount
		{
			get
			{
				this.DiscardOldEvents();
				return this.playerTreeDestructionTicks.Count;
			}
		}

		// Token: 0x0600BBD6 RID: 48086 RVA: 0x00366B69 File Offset: 0x00364D69
		public TreeDestructionTracker(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600BBD7 RID: 48087 RVA: 0x00366B83 File Offset: 0x00364D83
		public void Notify_TreeDestroyed(DamageInfo dInfo)
		{
			if (dInfo.Instigator != null && dInfo.Instigator.Faction == Faction.OfPlayer)
			{
				this.playerTreeDestructionTicks.Add(Find.TickManager.TicksGame);
			}
		}

		// Token: 0x0600BBD8 RID: 48088 RVA: 0x00366BB6 File Offset: 0x00364DB6
		public void Notify_TreeCut(Pawn by)
		{
			if (by.Faction == Faction.OfPlayer)
			{
				this.playerTreeDestructionTicks.Add(Find.TickManager.TicksGame);
			}
		}

		// Token: 0x0600BBD9 RID: 48089 RVA: 0x00366BDC File Offset: 0x00364DDC
		private void DiscardOldEvents()
		{
			int num = 0;
			while (num < this.playerTreeDestructionTicks.Count && Find.TickManager.TicksGame >= this.playerTreeDestructionTicks[num] + 900000)
			{
				this.playerTreeDestructionTicks.RemoveAt(num);
				num--;
				num++;
			}
		}

		// Token: 0x0600BBDA RID: 48090 RVA: 0x00366C2D File Offset: 0x00364E2D
		public void ExposeData()
		{
			Scribe_Collections.Look<int>(ref this.playerTreeDestructionTicks, "playerTreeDestructionTicks", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0400815B RID: 33115
		private Map map;

		// Token: 0x0400815C RID: 33116
		private List<int> playerTreeDestructionTicks = new List<int>();

		// Token: 0x0400815D RID: 33117
		private const int DestructionsExpireInTicks = 900000;
	}
}
