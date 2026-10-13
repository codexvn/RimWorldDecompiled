using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001368 RID: 4968
	public class JobDriver_PruneGauranlenTre : JobDriver
	{
		// Token: 0x1700133A RID: 4922
		// (get) Token: 0x060079DC RID: 31196 RVA: 0x0024C4B8 File Offset: 0x0024A6B8
		private CompTreeConnection TreeConnection
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompTreeConnection>();
			}
		}

		// Token: 0x060079DD RID: 31197 RVA: 0x0024C4E0 File Offset: 0x0024A6E0
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			float num = this.TreeConnection.DesiredConnectionStrength - this.TreeConnection.ConnectionStrength;
			this.numPositions = Mathf.Min(8, Mathf.CeilToInt(num / this.TreeConnection.ConnectionStrengthGainPerHourOfPruning) + 1);
		}

		// Token: 0x060079DE RID: 31198 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060079DF RID: 31199 RVA: 0x0024C52B File Offset: 0x0024A72B
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			int ticks = Mathf.RoundToInt(2500f / this.pawn.GetStatValue(StatDefOf.PruningSpeed, true, -1));
			Toil findAdjacentCell = Toils_General.Do(delegate
			{
				this.job.targetB = this.GetAdjacentCell(this.job.GetTarget(TargetIndex.A).Thing);
			});
			Toil goToAdjacentCell = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell).FailOn(() => this.TreeConnection.ConnectionStrength >= this.TreeConnection.DesiredConnectionStrength);
			Toil prune = Toils_General.WaitWith(TargetIndex.A, ticks, true, false, false, TargetIndex.None, PathEndMode.Touch).WithEffect(EffecterDefOf.Harvest_MetaOnly, TargetIndex.A, null).WithEffect(EffecterDefOf.GauranlenDebris, TargetIndex.A, null).PlaySustainerOrSound(SoundDefOf.Interact_Prune, 1f);
			prune.AddPreTickIntervalAction(delegate(int delta)
			{
				this.TreeConnection.Prune(delta);
				Pawn_SkillTracker skills = this.pawn.skills;
				if (skills != null)
				{
					skills.Learn(SkillDefOf.Plants, 0.085f * (float)delta, false, false);
				}
				if (this.TreeConnection.ConnectionStrength >= this.TreeConnection.DesiredConnectionStrength)
				{
					base.ReadyForNextToil();
				}
			});
			prune.activeSkill = (() => SkillDefOf.Plants);
			int num;
			for (int i = 0; i < this.numPositions; i = num + 1)
			{
				yield return findAdjacentCell;
				yield return goToAdjacentCell;
				yield return prune;
				num = i;
			}
			yield break;
		}

		// Token: 0x060079E0 RID: 31200 RVA: 0x0024C53C File Offset: 0x0024A73C
		private IntVec3 GetAdjacentCell(Thing treeThing)
		{
			IntVec3 result;
			if ((from x in GenAdj.CellsAdjacent8Way(treeThing)
			where x.InBounds(this.pawn.Map) && !x.Fogged(this.pawn.Map) && !x.IsForbidden(this.pawn) && this.pawn.CanReserveAndReach(x, PathEndMode.OnCell, Danger.Some, 1, -1, null, false)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return treeThing.Position;
		}

		// Token: 0x060079E1 RID: 31201 RVA: 0x0024C571 File Offset: 0x0024A771
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.numPositions, "numPositions", 1, false);
		}

		// Token: 0x04005290 RID: 21136
		private int numPositions = 1;

		// Token: 0x04005291 RID: 21137
		private const TargetIndex TreeIndex = TargetIndex.A;

		// Token: 0x04005292 RID: 21138
		private const TargetIndex AdjacentCellIndex = TargetIndex.B;

		// Token: 0x04005293 RID: 21139
		private const int DurationTicks = 2500;

		// Token: 0x04005294 RID: 21140
		private const int MaxPositions = 8;
	}
}
