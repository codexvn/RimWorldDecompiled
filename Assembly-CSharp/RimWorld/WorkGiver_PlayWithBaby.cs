using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001602 RID: 5634
	public class WorkGiver_PlayWithBaby : WorkGiver_Scanner
	{
		// Token: 0x17001461 RID: 5217
		// (get) Token: 0x060084F6 RID: 34038 RVA: 0x000E9647 File Offset: 0x000E7847
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.InteractionCell;
			}
		}

		// Token: 0x17001462 RID: 5218
		// (get) Token: 0x060084F7 RID: 34039 RVA: 0x002735F9 File Offset: 0x002717F9
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
			}
		}

		// Token: 0x060084F8 RID: 34040 RVA: 0x0027D0FC File Offset: 0x0027B2FC
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedBabiesInFaction(pawn.Faction);
		}

		// Token: 0x060084F9 RID: 34041 RVA: 0x0027D114 File Offset: 0x0027B314
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return ModsConfig.BiotechActive && pawn.IsBurning();
		}

		// Token: 0x060084FA RID: 34042 RVA: 0x0027D12C File Offset: 0x0027B32C
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null)
			{
				return false;
			}
			if (!pawn2.DevelopmentalStage.Baby())
			{
				return false;
			}
			if (pawn2.needs.play == null)
			{
				return false;
			}
			if (forced)
			{
				if (pawn2.needs.play.CurLevelPercentage >= 0.95f)
				{
					JobFailReason.Is("CannotInteractBabyPlayFull".Translate(), null);
					return false;
				}
			}
			else
			{
				if (!pawn2.Awake())
				{
					return false;
				}
				if (!pawn2.needs.play.IsLow)
				{
					return false;
				}
			}
			using (IEnumerator<BabyPlayDef> enumerator = DefDatabase<BabyPlayDef>.AllDefs.InRandomOrder(null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Worker.CanDo(pawn, pawn2))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060084FB RID: 34043 RVA: 0x0027D204 File Offset: 0x0027B404
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn baby = (Pawn)t;
			foreach (BabyPlayDef babyPlayDef in DefDatabase<BabyPlayDef>.AllDefs.InRandomOrder(null))
			{
				if (babyPlayDef.Worker.CanDo(pawn, baby))
				{
					return babyPlayDef.Worker.TryGiveJob(pawn, baby);
				}
			}
			return null;
		}

		// Token: 0x0400574F RID: 22351
		private const float BabyPlayNeedFullThreshold = 0.95f;
	}
}
