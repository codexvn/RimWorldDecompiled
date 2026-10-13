using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200119B RID: 4507
	public class JobDriver_Blind : JobDriver
	{
		// Token: 0x17001113 RID: 4371
		// (get) Token: 0x06006E38 RID: 28216 RVA: 0x00226BD8 File Offset: 0x00224DD8
		protected Pawn Target
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06006E39 RID: 28217 RVA: 0x00226C00 File Offset: 0x00224E00
		public static void Blind(Pawn pawn, Pawn doer)
		{
			Lord lord = pawn.GetLord();
			IEnumerable<BodyPartRecord> enumerable = from p in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where p.def == BodyPartDefOf.Eye
			select p;
			if (lord != null)
			{
				LordJob_Ritual_Mutilation lordJob_Ritual_Mutilation = lord.LordJob as LordJob_Ritual_Mutilation;
				if (lordJob_Ritual_Mutilation != null && enumerable.Count<BodyPartRecord>() == 1)
				{
					lordJob_Ritual_Mutilation.mutilatedPawns.Add(pawn);
				}
			}
			foreach (BodyPartRecord bodyPartRecord in enumerable)
			{
				if (bodyPartRecord.def == BodyPartDefOf.Eye)
				{
					pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
					break;
				}
			}
			if (pawn.Dead)
			{
				ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, doer, PawnExecutionKind.GenericBrutal);
			}
		}

		// Token: 0x06006E3A RID: 28218 RVA: 0x00226CF4 File Offset: 0x00224EF4
		public static void CreateHistoryEventDef(Pawn pawn)
		{
			if (PawnUtility.IsBiologicallyBlind(pawn))
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.GotBlinded, pawn.Named(HistoryEventArgsNames.Doer)), true);
			}
		}

		// Token: 0x06006E3B RID: 28219 RVA: 0x00226D1E File Offset: 0x00224F1E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006E3C RID: 28220 RVA: 0x00226D41 File Offset: 0x00224F41
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Scarify"))
			{
				yield break;
			}
			this.FailOnDespawnedOrNull(TargetIndex.A);
			Pawn target = this.Target;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false);
			yield return Toils_General.Wait(35, TargetIndex.None);
			Toil scarify = ToilMaker.MakeToil("MakeNewToils");
			scarify.initAction = delegate()
			{
				JobDriver_Blind.Blind(target, this.pawn);
				JobDriver_Blind.CreateHistoryEventDef(target);
				SoundDefOf.Execute_Cut.PlayOneShot(target);
				if (target.RaceProps.BloodDef != null)
				{
					CellRect cellRect = new CellRect(target.PositionHeld.x - 1, target.PositionHeld.z - 1, 3, 3);
					for (int i = 0; i < 3; i++)
					{
						IntVec3 randomCell = cellRect.RandomCell;
						if (randomCell.InBounds(this.Map) && GenSight.LineOfSight(randomCell, target.PositionHeld, this.Map))
						{
							FilthMaker.TryMakeFilth(randomCell, target.MapHeld, target.RaceProps.BloodDef, target.LabelIndefinite(), 1, FilthSourceFlags.None);
						}
					}
				}
			};
			scarify.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return Toils_General.Wait(120, TargetIndex.None);
			yield return scarify;
			yield break;
		}

		// Token: 0x04004D00 RID: 19712
		private const TargetIndex TargetPawnIndex = TargetIndex.A;
	}
}
