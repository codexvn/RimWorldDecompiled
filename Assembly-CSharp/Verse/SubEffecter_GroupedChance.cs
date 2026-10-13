using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CB9 RID: 3257
	public class SubEffecter_GroupedChance : SubEffecter
	{
		// Token: 0x06005364 RID: 21348 RVA: 0x001ADA84 File Offset: 0x001ABC84
		public SubEffecter_GroupedChance(SubEffecterDef subDef, Effecter parent) : base(subDef, parent)
		{
			this.ticksUntilEffect = this.def.initialDelayTicks;
			this.lifespanMaxTicks = Find.TickManager.TicksGame + this.def.lifespanMaxTicks + this.ticksUntilEffect;
			if (this.def.children != null)
			{
				for (int i = 0; i < this.def.children.Count; i++)
				{
					this.children.Add(this.def.children[i].Spawn(parent));
				}
			}
		}

		// Token: 0x06005365 RID: 21349 RVA: 0x001ADB24 File Offset: 0x001ABD24
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			this.ticksUntilEffect--;
			if (Find.TickManager.TicksGame >= this.lastEffectTicks + this.def.chancePeriodTicks && Find.TickManager.TicksGame < this.lifespanMaxTicks && this.ticksUntilEffect <= 0 && Rand.Chance(base.EffectiveChancePerTick))
			{
				this.lastEffectTicks = Find.TickManager.TicksGame;
				this.SubTrigger(A, B, -1, true);
			}
		}

		// Token: 0x06005366 RID: 21350 RVA: 0x001ADBA8 File Offset: 0x001ABDA8
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			if (!this.def.subTriggerOnSpawn && Find.TickManager.TicksGame == this.parent.spawnTick)
			{
				return;
			}
			foreach (SubEffecter subEffecter in this.children)
			{
				subEffecter.SubTrigger(A, B, overrideSpawnTick, force);
			}
		}

		// Token: 0x04003C34 RID: 15412
		private int lastEffectTicks;

		// Token: 0x04003C35 RID: 15413
		private int ticksUntilEffect;

		// Token: 0x04003C36 RID: 15414
		private int lifespanMaxTicks;

		// Token: 0x04003C37 RID: 15415
		public List<SubEffecter> children = new List<SubEffecter>();
	}
}
