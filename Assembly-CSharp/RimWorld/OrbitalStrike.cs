using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AB5 RID: 10933
	public class OrbitalStrike : ThingWithComps
	{
		// Token: 0x17002617 RID: 9751
		// (get) Token: 0x0600F39E RID: 62366 RVA: 0x0046C96E File Offset: 0x0046AB6E
		protected int TicksPassed
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17002618 RID: 9752
		// (get) Token: 0x0600F39F RID: 62367 RVA: 0x0046C981 File Offset: 0x0046AB81
		protected int TicksLeft
		{
			get
			{
				return this.duration - this.TicksPassed;
			}
		}

		// Token: 0x0600F3A0 RID: 62368 RVA: 0x0046C990 File Offset: 0x0046AB90
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.instigator, "instigator", false);
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", 0, false);
		}

		// Token: 0x0600F3A1 RID: 62369 RVA: 0x00127514 File Offset: 0x00125714
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.Comps_PostDraw();
		}

		// Token: 0x0600F3A2 RID: 62370 RVA: 0x0046CA00 File Offset: 0x0046AC00
		public virtual void StartStrike()
		{
			if (!base.Spawned)
			{
				Log.Error("Called StartStrike() on unspawned thing.");
				return;
			}
			this.angle = OrbitalStrike.AngleRange.RandomInRange;
			this.startTick = Find.TickManager.TicksGame;
			CompAffectsSky comp = base.GetComp<CompAffectsSky>();
			if (comp != null)
			{
				comp.StartFadeInHoldFadeOut(30, this.duration - 30 - 15, 15, 1f);
			}
			base.GetComp<CompOrbitalBeam>().StartAnimation(this.duration, 10, this.angle);
		}

		// Token: 0x0600F3A3 RID: 62371 RVA: 0x0046CA81 File Offset: 0x0046AC81
		protected override void Tick()
		{
			base.Tick();
			if (this.TicksPassed >= this.duration)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0400A077 RID: 41079
		public int duration;

		// Token: 0x0400A078 RID: 41080
		public Thing instigator;

		// Token: 0x0400A079 RID: 41081
		public ThingDef weaponDef;

		// Token: 0x0400A07A RID: 41082
		private float angle;

		// Token: 0x0400A07B RID: 41083
		private int startTick;

		// Token: 0x0400A07C RID: 41084
		private static readonly FloatRange AngleRange = new FloatRange(-12f, 12f);

		// Token: 0x0400A07D RID: 41085
		private const int SkyColorFadeInTicks = 30;

		// Token: 0x0400A07E RID: 41086
		private const int SkyColorFadeOutTicks = 15;

		// Token: 0x0400A07F RID: 41087
		private const int OrbitalBeamFadeOutTicks = 10;
	}
}
