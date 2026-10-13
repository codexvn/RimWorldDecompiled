using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C95 RID: 11413
	public class CompFleshmassHeartChild : ThingComp
	{
		// Token: 0x17002872 RID: 10354
		// (get) Token: 0x0601002F RID: 65583 RVA: 0x004A3857 File Offset: 0x004A1A57
		public CompFleshmassHeart HeartComp
		{
			get
			{
				Building_FleshmassHeart building_FleshmassHeart = this.heart;
				if (building_FleshmassHeart == null)
				{
					return null;
				}
				return building_FleshmassHeart.GetComp<CompFleshmassHeart>();
			}
		}

		// Token: 0x06010030 RID: 65584 RVA: 0x004A386A File Offset: 0x004A1A6A
		public override void PostExposeData()
		{
			Scribe_References.Look<Building_FleshmassHeart>(ref this.heart, "heart", false);
		}

		// Token: 0x06010031 RID: 65585 RVA: 0x004A387D File Offset: 0x004A1A7D
		public override void CompTick()
		{
			if (this.destroyTick > 0 && Find.TickManager.TicksGame >= this.destroyTick)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06010032 RID: 65586 RVA: 0x004A38A6 File Offset: 0x004A1AA6
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			CompFleshmassHeart heartComp = this.HeartComp;
			if (heartComp == null)
			{
				return;
			}
			heartComp.Notify_ChildDied(this.parent);
		}

		// Token: 0x06010033 RID: 65587 RVA: 0x004A38C0 File Offset: 0x004A1AC0
		public void Notify_HeartDied()
		{
			this.destroyTick = Find.TickManager.TicksGame + CompFleshmassHeartChild.DestroyDelayRangeTicks.RandomInRange;
		}

		// Token: 0x0400A780 RID: 42880
		private static readonly IntRange DestroyDelayRangeTicks = new IntRange(180, 480);

		// Token: 0x0400A781 RID: 42881
		public Building_FleshmassHeart heart;

		// Token: 0x0400A782 RID: 42882
		private int destroyTick = -99999;
	}
}
