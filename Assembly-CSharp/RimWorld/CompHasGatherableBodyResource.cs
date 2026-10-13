using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CD9 RID: 11481
	public abstract class CompHasGatherableBodyResource : ThingComp
	{
		// Token: 0x170028D1 RID: 10449
		// (get) Token: 0x06010209 RID: 66057
		protected abstract int GatherResourcesIntervalDays { get; }

		// Token: 0x170028D2 RID: 10450
		// (get) Token: 0x0601020A RID: 66058
		protected abstract int ResourceAmount { get; }

		// Token: 0x170028D3 RID: 10451
		// (get) Token: 0x0601020B RID: 66059
		protected abstract ThingDef ResourceDef { get; }

		// Token: 0x170028D4 RID: 10452
		// (get) Token: 0x0601020C RID: 66060
		protected abstract string SaveKey { get; }

		// Token: 0x170028D5 RID: 10453
		// (get) Token: 0x0601020D RID: 66061 RVA: 0x004AA9FC File Offset: 0x004A8BFC
		public float Fullness
		{
			get
			{
				return this.fullness;
			}
		}

		// Token: 0x170028D6 RID: 10454
		// (get) Token: 0x0601020E RID: 66062 RVA: 0x004AAA04 File Offset: 0x004A8C04
		protected virtual bool Active
		{
			get
			{
				return this.parent.Faction != null && !this.parent.Suspended;
			}
		}

		// Token: 0x170028D7 RID: 10455
		// (get) Token: 0x0601020F RID: 66063 RVA: 0x004AAA25 File Offset: 0x004A8C25
		public bool ActiveAndFull
		{
			get
			{
				return this.Active && this.fullness >= 1f;
			}
		}

		// Token: 0x06010210 RID: 66064 RVA: 0x004AAA41 File Offset: 0x004A8C41
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.fullness, this.SaveKey, 0f, false);
		}

		// Token: 0x06010211 RID: 66065 RVA: 0x004AAA60 File Offset: 0x004A8C60
		public override void CompTick()
		{
			if (this.Active)
			{
				float num = 1f / (float)(this.GatherResourcesIntervalDays * 60000);
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					num *= PawnUtility.BodyResourceGrowthSpeed(pawn);
				}
				this.fullness += num;
				if (this.fullness > 1f)
				{
					this.fullness = 1f;
				}
			}
		}

		// Token: 0x06010212 RID: 66066 RVA: 0x004AAAC8 File Offset: 0x004A8CC8
		public void Gathered(Pawn doer)
		{
			if (!this.Active)
			{
				string str = (doer != null) ? doer.ToString() : null;
				string str2 = " gathered body resources while not Active: ";
				ThingWithComps parent = this.parent;
				Log.Error(str + str2 + ((parent != null) ? parent.ToString() : null));
			}
			if (!Rand.Chance(doer.GetStatValue(StatDefOf.AnimalGatherYield, true, -1)))
			{
				MoteMaker.ThrowText((doer.DrawPos + this.parent.DrawPos) / 2f, this.parent.Map, "TextMote_ProductWasted".Translate(), 3.65f);
			}
			else
			{
				int i = GenMath.RoundRandom((float)this.ResourceAmount * this.fullness);
				while (i > 0)
				{
					int num = Mathf.Clamp(i, 1, this.ResourceDef.stackLimit);
					i -= num;
					Thing thing = ThingMaker.MakeThing(this.ResourceDef, null);
					thing.stackCount = num;
					GenPlace.TryPlaceThing(thing, doer.Position, doer.Map, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
			this.fullness = 0f;
		}

		// Token: 0x0400A885 RID: 43141
		protected float fullness;
	}
}
