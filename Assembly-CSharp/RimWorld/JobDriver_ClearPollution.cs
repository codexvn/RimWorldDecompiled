using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200123E RID: 4670
	public class JobDriver_ClearPollution : JobDriver
	{
		// Token: 0x0600723E RID: 29246 RVA: 0x00234077 File Offset: 0x00232277
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
		}

		// Token: 0x0600723F RID: 29247 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007240 RID: 29248 RVA: 0x00234095 File Offset: 0x00232295
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.TargetLocA.IsPolluted(base.Map));
			this.FailOn(() => !base.Map.areaManager.PollutionClear[base.TargetLocA]);
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				this.workDone += this.pawn.GetStatValue(StatDefOf.GeneralLaborSpeed, true, -1) * (float)delta;
				if (this.workDone >= 5600f)
				{
					this.ClearPollutionAt(base.TargetLocA, this.pawn.Map);
					IntVec3 intVec = CellFinder.FindNoWipeSpawnLocNear(base.TargetLocA, this.pawn.Map, ThingDefOf.Wastepack, Rot4.North, 2, null);
					GenSpawn.Spawn(ThingDefOf.Wastepack, intVec.IsValid ? intVec : base.TargetLocA, this.pawn.Map, WipeMode.Vanish);
					base.ReadyForNextToil();
					return;
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.PlaySustainerOrSound(() => SoundDefOf.Interact_CleanFilth, 1f);
			toil.WithProgressBar(TargetIndex.A, () => this.workDone / 5600f, true, -0.5f, false);
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			toil.WithEffect(EffecterDefOf.Clean, TargetIndex.A, null);
			yield return toil;
			yield break;
		}

		// Token: 0x06007241 RID: 29249 RVA: 0x002340A8 File Offset: 0x002322A8
		private void ClearPollutionAt(IntVec3 root, Map map)
		{
			JobDriver_ClearPollution.tmpCellsToClear.Clear();
			JobDriver_ClearPollution.tmpCellsToClear.AddRange(from c in map.areaManager.PollutionClear.ActiveCells
			where c.DistanceToSquared(root) < 100
			select c);
			JobDriver_ClearPollution.tmpCellsToClear.SortBy((IntVec3 c) => c.DistanceToSquared(root));
			int num = 6;
			for (int i = 0; i < JobDriver_ClearPollution.tmpCellsToClear.Count; i++)
			{
				if (this.CanUnpollute(root, map, JobDriver_ClearPollution.tmpCellsToClear[i], false))
				{
					JobDriver_ClearPollution.tmpCellsToClear[i].Unpollute(map);
					num--;
					if (num <= 0)
					{
						break;
					}
				}
			}
			JobDriver_ClearPollution.tmpCellsToClear.Clear();
			if (num > 0)
			{
				num -= this.UnpolluteRadially(root, map, num, false);
				if (num > 0)
				{
					this.UnpolluteRadially(root, map, num, true);
				}
			}
		}

		// Token: 0x06007242 RID: 29250 RVA: 0x0023418C File Offset: 0x0023238C
		private int UnpolluteRadially(IntVec3 root, Map map, int maxToUnpollute = 5, bool ignoreOtherPawnsCleaningCell = false)
		{
			int num = 0;
			foreach (IntVec3 c in GenRadial.RadialCellsAround(root, 10f, true))
			{
				if (this.CanUnpollute(root, map, c, ignoreOtherPawnsCleaningCell))
				{
					c.Unpollute(map);
					num++;
					if (num >= maxToUnpollute)
					{
						break;
					}
				}
			}
			return num;
		}

		// Token: 0x06007243 RID: 29251 RVA: 0x002341FC File Offset: 0x002323FC
		private bool CanUnpollute(IntVec3 root, Map map, IntVec3 c, bool ignoreOtherPawnsCleaningCell = false)
		{
			return c.IsPolluted(map) && (ignoreOtherPawnsCleaningCell || !this.AnyOtherPawnCleaning(this.pawn, c)) && (c.GetRoom(map) == null || root.GetRoom(map) == c.GetRoom(map)) && c.DistanceToSquared(root) <= 100;
		}

		// Token: 0x06007244 RID: 29252 RVA: 0x00234254 File Offset: 0x00232454
		private bool AnyOtherPawnCleaning(Pawn pawn, IntVec3 cell)
		{
			List<Pawn> freeColonistsSpawned = pawn.Map.mapPawns.FreeColonistsSpawned;
			for (int i = 0; i < freeColonistsSpawned.Count; i++)
			{
				if (freeColonistsSpawned[i] != pawn && freeColonistsSpawned[i].CurJobDef == JobDefOf.ClearPollution)
				{
					LocalTargetInfo target = freeColonistsSpawned[i].CurJob.GetTarget(TargetIndex.A);
					if (target.IsValid && target.Cell == cell)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04004F05 RID: 20229
		private const float WorkAmount = 5600f;

		// Token: 0x04004F06 RID: 20230
		private const int MaxDistanceToClear = 10;

		// Token: 0x04004F07 RID: 20231
		private const int MaxDistanceToClearSquared = 100;

		// Token: 0x04004F08 RID: 20232
		private const int PollutionCellsToClearPerJob = 6;

		// Token: 0x04004F09 RID: 20233
		private const TargetIndex CleanCellIndex = TargetIndex.A;

		// Token: 0x04004F0A RID: 20234
		private float workDone;

		// Token: 0x04004F0B RID: 20235
		private static List<IntVec3> tmpCellsToClear = new List<IntVec3>();
	}
}
