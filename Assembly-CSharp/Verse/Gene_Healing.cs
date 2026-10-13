using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000658 RID: 1624
	public class Gene_Healing : Gene
	{
		// Token: 0x060028F3 RID: 10483 RVA: 0x000D7EFB File Offset: 0x000D60FB
		public override void PostAdd()
		{
			base.PostAdd();
			this.ResetInterval();
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x000D7F09 File Offset: 0x000D6109
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.ticksToHeal -= delta;
			if (this.ticksToHeal <= 0)
			{
				HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(this.pawn, this.LabelCap);
				this.ResetInterval();
			}
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x000D7F40 File Offset: 0x000D6140
		private void ResetInterval()
		{
			this.ticksToHeal = Gene_Healing.HealingIntervalTicksRange.RandomInRange;
		}

		// Token: 0x060028F6 RID: 10486 RVA: 0x000D7F60 File Offset: 0x000D6160
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Heal permanent wound",
					action = delegate()
					{
						HediffComp_HealPermanentWounds.TryHealRandomPermanentWound(this.pawn, this.LabelCap);
						this.ResetInterval();
					}
				};
			}
			yield break;
		}

		// Token: 0x060028F7 RID: 10487 RVA: 0x000D7F70 File Offset: 0x000D6170
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToHeal, "ticksToHeal", 0, false);
		}

		// Token: 0x04002198 RID: 8600
		private int ticksToHeal;

		// Token: 0x04002199 RID: 8601
		private static readonly IntRange HealingIntervalTicksRange = new IntRange(900000, 1800000);
	}
}
