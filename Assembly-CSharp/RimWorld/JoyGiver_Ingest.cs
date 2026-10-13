using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014EF RID: 5359
	public class JoyGiver_Ingest : JoyGiver
	{
		// Token: 0x06007FDD RID: 32733 RVA: 0x00267DAF File Offset: 0x00265FAF
		public override Job TryGiveJob(Pawn pawn)
		{
			return this.TryGiveJobInternal(pawn, null);
		}

		// Token: 0x06007FDE RID: 32734 RVA: 0x00267DBC File Offset: 0x00265FBC
		public override Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatheringSpot, float maxRadius = -1f)
		{
			return this.TryGiveJobInternal(pawn, (Thing x) => !x.Spawned || (GatheringsUtility.InGatheringArea(x.Position, gatheringSpot, pawn.Map) && (maxRadius < 0f || x.Position.InHorDistOf(gatheringSpot, maxRadius))));
		}

		// Token: 0x06007FDF RID: 32735 RVA: 0x00267DFC File Offset: 0x00265FFC
		private Job TryGiveJobInternal(Pawn pawn, Predicate<Thing> extraValidator)
		{
			Thing thing = this.BestIngestItem(pawn, extraValidator);
			if (thing == null)
			{
				return null;
			}
			return this.CreateIngestJob(thing, pawn);
		}

		// Token: 0x06007FE0 RID: 32736 RVA: 0x00267E20 File Offset: 0x00266020
		protected virtual Thing BestIngestItem(Pawn pawn, Predicate<Thing> extraValidator)
		{
			JoyGiver_Ingest.<>c__DisplayClass4_0 CS$<>8__locals1 = new JoyGiver_Ingest.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.extraValidator = extraValidator;
			ThingOwner<Thing> innerContainer = CS$<>8__locals1.pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (this.SearchSetWouldInclude(innerContainer[i]) && CS$<>8__locals1.<BestIngestItem>g__Validator|0(innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			JoyGiver_Ingest.tmpCandidates.Clear();
			this.GetSearchSet(CS$<>8__locals1.pawn, JoyGiver_Ingest.tmpCandidates);
			if (JoyGiver_Ingest.tmpCandidates.Count == 0)
			{
				return null;
			}
			Thing result = GenClosest.ClosestThing_Global_Reachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, JoyGiver_Ingest.tmpCandidates, PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<BestIngestItem>g__Validator|0), null, false);
			JoyGiver_Ingest.tmpCandidates.Clear();
			return result;
		}

		// Token: 0x06007FE1 RID: 32737 RVA: 0x00267F08 File Offset: 0x00266108
		protected virtual bool CanIngestForJoy(Pawn pawn, Thing t)
		{
			if (!t.def.IsIngestible || t.def.ingestible.joyKind == null || t.def.ingestible.joy <= 0f || !pawn.WillEat(t, null, true, false))
			{
				return false;
			}
			if (t.Spawned)
			{
				if (t.Fogged())
				{
					return false;
				}
				if (!pawn.CanReserve(t, 1, -1, null, false))
				{
					return false;
				}
				if (t.IsForbidden(pawn))
				{
					return false;
				}
				if (!t.IsSociallyProper(pawn))
				{
					return false;
				}
				if (!t.IsPoliticallyProper(pawn))
				{
					return false;
				}
				if (t.Position.VacuumConcernTo(pawn))
				{
					return false;
				}
			}
			return !t.def.IsDrug || pawn.drugs == null || pawn.drugs.CurrentPolicy[t.def].allowedForJoy || pawn.story == null || pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0 || pawn.InMentalState;
		}

		// Token: 0x06007FE2 RID: 32738 RVA: 0x0026800D File Offset: 0x0026620D
		protected virtual bool SearchSetWouldInclude(Thing thing)
		{
			return this.def.thingDefs != null && this.def.thingDefs.Contains(thing.def);
		}

		// Token: 0x06007FE3 RID: 32739 RVA: 0x00268034 File Offset: 0x00266234
		protected virtual Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, ingestible);
			job.count = ingestible.stackCount;
			if (ingestible.def.ingestible.maxNumToIngestAtOnce > 0)
			{
				job.count = Math.Min(job.count, ingestible.def.ingestible.maxNumToIngestAtOnce);
			}
			return job;
		}

		// Token: 0x04005586 RID: 21894
		private static readonly List<Thing> tmpCandidates = new List<Thing>();
	}
}
