using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E9F RID: 7839
	public class EffecterMaintainer
	{
		// Token: 0x0600ADF8 RID: 44536 RVA: 0x00319C39 File Offset: 0x00317E39
		public EffecterMaintainer(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600ADF9 RID: 44537 RVA: 0x00319C54 File Offset: 0x00317E54
		public void AddEffecterToMaintain(Effecter eff, IntVec3 pos, int ticks)
		{
			eff.ticksLeft = ticks;
			TargetInfo a = new TargetInfo(pos, this.map, false);
			this.maintainedEffecters.Add(new EffecterMaintainer.MaintainedEffecter(eff, a, TargetInfo.Invalid));
		}

		// Token: 0x0600ADFA RID: 44538 RVA: 0x00319C8E File Offset: 0x00317E8E
		public void AddEffecterToMaintain(Effecter eff, Thing target, int ticks)
		{
			eff.ticksLeft = ticks;
			this.maintainedEffecters.Add(new EffecterMaintainer.MaintainedEffecter(eff, target, TargetInfo.Invalid));
		}

		// Token: 0x0600ADFB RID: 44539 RVA: 0x00319CB3 File Offset: 0x00317EB3
		public void AddEffecterToMaintain(Effecter eff, TargetInfo A, TargetInfo B, int ticks)
		{
			eff.ticksLeft = ticks;
			this.maintainedEffecters.Add(new EffecterMaintainer.MaintainedEffecter(eff, A, B));
		}

		// Token: 0x0600ADFC RID: 44540 RVA: 0x00319CD0 File Offset: 0x00317ED0
		public void EffecterMaintainerTick()
		{
			for (int i = this.maintainedEffecters.Count - 1; i >= 0; i--)
			{
				EffecterMaintainer.MaintainedEffecter maintainedEffecter = this.maintainedEffecters[i];
				if (maintainedEffecter.Effecter.ticksLeft > 0)
				{
					maintainedEffecter.Effecter.EffectTick(maintainedEffecter.A, maintainedEffecter.B);
					maintainedEffecter.Effecter.ticksLeft--;
				}
				else
				{
					maintainedEffecter.Effecter.Cleanup();
					this.maintainedEffecters.RemoveAt(i);
				}
			}
		}

		// Token: 0x040077A7 RID: 30631
		private Map map;

		// Token: 0x040077A8 RID: 30632
		private List<EffecterMaintainer.MaintainedEffecter> maintainedEffecters = new List<EffecterMaintainer.MaintainedEffecter>();

		// Token: 0x02001EA0 RID: 7840
		public struct MaintainedEffecter
		{
			// Token: 0x0600ADFD RID: 44541 RVA: 0x00319D53 File Offset: 0x00317F53
			public MaintainedEffecter(Effecter effecter, TargetInfo A, TargetInfo B)
			{
				this.Effecter = effecter;
				this.A = A;
				this.B = B;
			}

			// Token: 0x040077A9 RID: 30633
			public Effecter Effecter;

			// Token: 0x040077AA RID: 30634
			public TargetInfo A;

			// Token: 0x040077AB RID: 30635
			public TargetInfo B;
		}
	}
}
