using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F5B RID: 12123
	public class CompUseEffect_DestroySelf : CompUseEffect
	{
		// Token: 0x17002C31 RID: 11313
		// (get) Token: 0x06011169 RID: 69993 RVA: 0x004E82EC File Offset: 0x004E64EC
		private CompProperties_UseEffectDestroySelf Props
		{
			get
			{
				return (CompProperties_UseEffectDestroySelf)this.props;
			}
		}

		// Token: 0x17002C32 RID: 11314
		// (get) Token: 0x0601116A RID: 69994 RVA: 0x004E82F9 File Offset: 0x004E64F9
		public override float OrderPriority
		{
			get
			{
				return this.Props.orderPriority;
			}
		}

		// Token: 0x0601116B RID: 69995 RVA: 0x004E8306 File Offset: 0x004E6506
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			if (this.Props.delayTicks <= 0)
			{
				this.DoDestroy();
				return;
			}
			this.delayTicks = this.Props.delayTicks;
		}

		// Token: 0x0601116C RID: 69996 RVA: 0x004E8338 File Offset: 0x004E6538
		private void DoDestroy()
		{
			if (this.Props.effecterDef != null)
			{
				Effecter effecter = new Effecter(this.Props.effecterDef);
				effecter.Trigger(new TargetInfo(this.parent.Position, this.parent.Map, false), TargetInfo.Invalid, -1);
				effecter.Cleanup();
			}
			if (this.Props.spawnKilledLeavings)
			{
				GenLeaving.DoLeavingsFor(this.parent, this.parent.MapHeld, DestroyMode.KillFinalizeLeavingsOnly, null);
			}
			if (!this.Props.leavings.NullOrEmpty<ThingDefCountClass>())
			{
				Rand.PushState(this.parent.thingIDNumber);
				for (int i = 0; i < this.Props.leavings.Count; i++)
				{
					ThingDefCountClass thingDefCountClass = this.Props.leavings[i];
					if (!thingDefCountClass.IsChanceBased || Rand.Chance(thingDefCountClass.DropChance))
					{
						Thing thing = ThingMaker.MakeThing(this.Props.leavings[i].thingDef, null);
						thing.stackCount = this.Props.leavings[i].count;
						Thing thing2;
						GenDrop.TryDropSpawn(thing, this.parent.PositionHeld, this.parent.MapHeld, ThingPlaceMode.Near, out thing2, null, null, true);
					}
				}
				Rand.PopState();
			}
			this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0601116D RID: 69997 RVA: 0x004E8493 File Offset: 0x004E6693
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", -1, false);
		}

		// Token: 0x0601116E RID: 69998 RVA: 0x004E84AD File Offset: 0x004E66AD
		public override void CompTick()
		{
			base.CompTick();
			if (this.delayTicks > 0)
			{
				this.delayTicks--;
			}
			if (this.delayTicks == 0)
			{
				this.DoDestroy();
				this.delayTicks = -1;
			}
		}

		// Token: 0x0400B16E RID: 45422
		private int delayTicks = -1;
	}
}
