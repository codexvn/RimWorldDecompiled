using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001349 RID: 4937
	public class JobDriver_Vomit : JobDriver
	{
		// Token: 0x06007944 RID: 31044 RVA: 0x000026EA File Offset: 0x000008EA
		public override void SetInitialPosture()
		{
		}

		// Token: 0x06007945 RID: 31045 RVA: 0x0024A47B File Offset: 0x0024867B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x06007946 RID: 31046 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06007947 RID: 31047 RVA: 0x0024A495 File Offset: 0x00248695
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.ticksLeft = Rand.Range(300, 900);
				int num = 0;
				IntVec3 c;
				for (;;)
				{
					c = this.pawn.Position + GenAdj.AdjacentCellsAndInside[Rand.Range(0, 9)];
					num++;
					if (num > 12)
					{
						break;
					}
					if (c.InBounds(this.pawn.Map) && c.Standable(this.pawn.Map))
					{
						goto IL_77;
					}
				}
				c = this.pawn.Position;
				IL_77:
				this.job.targetA = c;
				this.pawn.pather.StopDead();
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				if (this.pawn.IsHashIntervalTick(150, delta))
				{
					FilthMaker.TryMakeFilth(this.job.targetA.Cell, base.Map, ThingDefOf.Filth_Vomit, this.pawn.LabelIndefinite(), 1, FilthSourceFlags.None);
					Need_Food need_Food;
					if (this.pawn.needs != null && this.pawn.needs.TryGetNeed<Need_Food>(out need_Food) && need_Food.CurLevelPercentage > 0.1f)
					{
						need_Food.CurLevel -= need_Food.MaxLevel * 0.04f;
					}
				}
				this.ticksLeft -= delta;
				if (this.ticksLeft <= 0)
				{
					base.ReadyForNextToil();
					TaleRecorder.RecordTale(TaleDefOf.Vomited, new object[]
					{
						this.pawn
					});
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.WithEffect(EffecterDefOf.Vomit, TargetIndex.A, null);
			toil.PlaySustainerOrSound(() => SoundDefOf.Vomit, 1f);
			yield return toil;
			yield break;
		}

		// Token: 0x04005248 RID: 21064
		private int ticksLeft;
	}
}
