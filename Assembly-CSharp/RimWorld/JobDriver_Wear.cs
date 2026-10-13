using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013C7 RID: 5063
	public class JobDriver_Wear : JobDriver
	{
		// Token: 0x17001388 RID: 5000
		// (get) Token: 0x06007BFC RID: 31740 RVA: 0x00255718 File Offset: 0x00253918
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17001389 RID: 5001
		// (get) Token: 0x06007BFD RID: 31741 RVA: 0x00255740 File Offset: 0x00253940
		private bool TargetIsOnApparelSource
		{
			get
			{
				Apparel apparel = this.Apparel;
				if (apparel != null && !apparel.Spawned)
				{
					IApparelSource apparelSource = apparel.ParentHolder as IApparelSource;
					if (apparelSource != null)
					{
						return apparelSource is Thing;
					}
				}
				return false;
			}
		}

		// Token: 0x1700138A RID: 5002
		// (get) Token: 0x06007BFE RID: 31742 RVA: 0x0025577C File Offset: 0x0025397C
		private IApparelSource ApparelSource
		{
			get
			{
				return (IApparelSource)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x06007BFF RID: 31743 RVA: 0x002557A2 File Offset: 0x002539A2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<int>(ref this.unequipBuffer, "unequipBuffer", 0, false);
		}

		// Token: 0x06007C00 RID: 31744 RVA: 0x002557D0 File Offset: 0x002539D0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.TargetIsOnApparelSource)
			{
				return this.pawn.Reserve((Thing)this.Apparel.ParentHolder, this.job, 1, -1, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(this.Apparel, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007C01 RID: 31745 RVA: 0x00255834 File Offset: 0x00253A34
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.TargetIsOnApparelSource)
			{
				this.job.targetB = (Thing)this.Apparel.ParentHolder;
			}
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			for (int i = wornApparel.Count - 1; i >= 0; i--)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					this.duration += (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
				}
			}
		}

		// Token: 0x06007C02 RID: 31746 RVA: 0x00255909 File Offset: 0x00253B09
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnBurningImmobile(TargetIndex.A);
			bool usingSource = this.TargetIsOnApparelSource;
			if (usingSource)
			{
				yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.B);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			}
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.tickIntervalAction = delegate(int delta)
			{
				this.unequipBuffer += delta;
				this.TryUnequipSomething();
				this.pawn.rotationTracker.FaceTarget(this.Apparel.PositionHeld);
			};
			toil.WithProgressBarToilDelay(usingSource ? TargetIndex.B : TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedNullOrForbidden(usingSource ? TargetIndex.B : TargetIndex.A);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.duration;
			toil.handlingFacing = true;
			toil.PlaySustainerOrSound(new Func<SoundDef>(this.GetCurrentWearSound), 1f);
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				Apparel apparel = this.Apparel;
				if (usingSource)
				{
					this.ApparelSource.RemoveApparel(apparel);
				}
				this.pawn.apparel.Wear(apparel, true, false);
				if (this.pawn.outfits != null && this.job.playerForced)
				{
					this.pawn.outfits.forcedHandler.SetForced(apparel, true);
				}
			});
			yield break;
		}

		// Token: 0x06007C03 RID: 31747 RVA: 0x0025591C File Offset: 0x00253B1C
		private SoundDef GetCurrentWearSound()
		{
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			int i = wornApparel.Count - 1;
			while (i >= 0)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					if (this.unequipBuffer < (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f))
					{
						return wornApparel[i].def.apparel.soundRemove;
					}
					break;
				}
				else
				{
					i--;
				}
			}
			return apparel.def.apparel.soundWear;
		}

		// Token: 0x06007C04 RID: 31748 RVA: 0x002559C8 File Offset: 0x00253BC8
		private void TryUnequipSomething()
		{
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			int i = wornApparel.Count - 1;
			while (i >= 0)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					int num = (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
					if (this.unequipBuffer < num)
					{
						break;
					}
					bool forbid = this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer);
					Apparel apparel2;
					if (!this.pawn.apparel.TryDrop(wornApparel[i], out apparel2, this.pawn.PositionHeld, forbid))
					{
						Pawn pawn = this.pawn;
						Log.Error(((pawn != null) ? pawn.ToString() : null) + " could not drop " + wornApparel[i].ToStringSafe<Apparel>());
						base.EndJobWith(JobCondition.Errored);
						return;
					}
					break;
				}
				else
				{
					i--;
				}
			}
		}

		// Token: 0x040053BB RID: 21435
		private int duration;

		// Token: 0x040053BC RID: 21436
		private int unequipBuffer;

		// Token: 0x040053BD RID: 21437
		private const TargetIndex ApparelInd = TargetIndex.A;

		// Token: 0x040053BE RID: 21438
		private const TargetIndex ApparelSourceIndex = TargetIndex.B;
	}
}
