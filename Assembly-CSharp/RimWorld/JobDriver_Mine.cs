using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012BF RID: 4799
	public class JobDriver_Mine : JobDriver
	{
		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06007592 RID: 30098 RVA: 0x0023E84C File Offset: 0x0023CA4C
		private Thing MineTarget
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06007593 RID: 30099 RVA: 0x0023E86D File Offset: 0x0023CA6D
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.MineTarget, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007594 RID: 30100 RVA: 0x0023E890 File Offset: 0x0023CA90
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => this.pawn.IsPlayerControlled && !this.job.ignoreDesignations && this.pawn.Map.designationManager.DesignationAt(this.TargetA.Cell, DesignationDefOf.Mine) == null && this.pawn.Map.designationManager.DesignationAt(this.TargetA.Cell, DesignationDefOf.MineVein) == null);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			Toil mine = ToilMaker.MakeToil("MakeNewToils");
			mine.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = mine.actor;
				Thing mineTarget = this.MineTarget;
				if (this.ticksToPickHit < -100)
				{
					this.ResetTicksToPickHit();
				}
				if (actor.skills != null && (mineTarget.Faction != actor.Faction || actor.Faction == null))
				{
					actor.skills.Learn(SkillDefOf.Mining, 0.07f * (float)delta, false, false);
				}
				this.ticksToPickHit -= delta;
				if (this.ticksToPickHit <= 0)
				{
					IntVec3 position = mineTarget.Position;
					if (this.effecter == null)
					{
						this.effecter = EffecterDefOf.Mine.Spawn();
					}
					this.effecter.Trigger(actor, mineTarget, -1);
					this.DoDamage(mineTarget, mine, actor, position);
					if (mineTarget.Destroyed)
					{
						actor.Map.mineStrikeManager.CheckStruckOre(position, mineTarget.def, actor);
						actor.records.Increment(RecordDefOf.CellsMined);
						if (this.pawn.Faction != Faction.OfPlayer)
						{
							List<Thing> thingList = position.GetThingList(this.Map);
							for (int i = 0; i < thingList.Count; i++)
							{
								thingList[i].SetForbidden(true, false);
							}
						}
						if (this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsVeryValuable(mineTarget.def))
						{
							TaleRecorder.RecordTale(TaleDefOf.MinedValuable, new object[]
							{
								this.pawn,
								mineTarget.def.building.mineableThing
							});
						}
						if (this.pawn.Faction == Faction.OfPlayer && MineStrikeManager.MineableIsValuable(mineTarget.def) && !this.pawn.Map.IsPlayerHome)
						{
							TaleRecorder.RecordTale(TaleDefOf.CaravanRemoteMining, new object[]
							{
								this.pawn,
								mineTarget.def.building.mineableThing
							});
						}
						this.ReadyForNextToil();
						return;
					}
					this.ResetTicksToPickHit();
				}
			};
			mine.defaultCompleteMode = ToilCompleteMode.Never;
			mine.WithProgressBar(TargetIndex.A, () => 1f - (float)this.MineTarget.HitPoints / (float)this.MineTarget.MaxHitPoints, false, -0.5f, false);
			mine.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
			mine.activeSkill = (() => SkillDefOf.Mining);
			yield return mine;
			yield break;
		}

		// Token: 0x06007595 RID: 30101 RVA: 0x0023E8A0 File Offset: 0x0023CAA0
		private void DoDamage(Thing target, Toil mine, Pawn actor, IntVec3 mineablePos)
		{
			int num = target.def.building.isNaturalRock ? 80 : 40;
			Mineable mineable = target as Mineable;
			if (mineable == null || target.HitPoints > num)
			{
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Mining, (float)num, 0f, -1f, mine.actor, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				target.TakeDamage(dinfo);
				return;
			}
			bool flag = base.Map.designationManager.DesignationAt(mineable.Position, DesignationDefOf.MineVein) != null;
			mineable.Notify_TookMiningDamage(target.HitPoints, mine.actor);
			mineable.HitPoints = 0;
			mineable.DestroyMined(actor);
			if (flag)
			{
				foreach (IntVec3 b in GenAdj.AdjacentCells)
				{
					Designator_MineVein.FloodFillDesignations(mineablePos + b, base.Map, mineable.def);
				}
			}
		}

		// Token: 0x06007596 RID: 30102 RVA: 0x0023E988 File Offset: 0x0023CB88
		private void ResetTicksToPickHit()
		{
			float num = this.pawn.GetStatValue(StatDefOf.MiningSpeed, true, -1);
			if (num < 0.6f && this.pawn.Faction != Faction.OfPlayer)
			{
				num = 0.6f;
			}
			this.ticksToPickHit = (int)Math.Round((double)(100f / num));
		}

		// Token: 0x06007597 RID: 30103 RVA: 0x0023E9DC File Offset: 0x0023CBDC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksToPickHit, "ticksToPickHit", 0, false);
		}

		// Token: 0x0400507E RID: 20606
		private int ticksToPickHit = -1000;

		// Token: 0x0400507F RID: 20607
		private Effecter effecter;

		// Token: 0x04005080 RID: 20608
		public const int BaseTicksBetweenPickHits = 100;

		// Token: 0x04005081 RID: 20609
		private const int BaseDamagePerPickHit_NaturalRock = 80;

		// Token: 0x04005082 RID: 20610
		private const int BaseDamagePerPickHit_NotNaturalRock = 40;

		// Token: 0x04005083 RID: 20611
		private const float MinMiningSpeedFactorForNPCs = 0.6f;
	}
}
