using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200277A RID: 10106
	public class RitualOutcomeEffectWorker_Skylantern : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x0600DD94 RID: 56724 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_Skylantern()
		{
		}

		// Token: 0x0600DD95 RID: 56725 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_Skylantern(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD96 RID: 56726 RVA: 0x0041396C File Offset: 0x00411B6C
		protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			extraOutcomeDesc = null;
			if (outcome.Positive && Rand.Chance(0.2f))
			{
				List<Thing> list = new List<Thing>();
				Ideo ideo = jobRitual.Ritual.ideo;
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					Precept_Building precept_Building = precept as Precept_Building;
					if (precept_Building != null && precept_Building.ThingDef.ritualFocus != null && precept_Building.ThingDef.ritualFocus.consumable)
					{
						list.Add(ThingMaker.MakeThing(precept_Building.ThingDef, null).MakeMinified(DestroyMode.Vanish));
					}
				}
				if (list.Count == 0)
				{
					list.AddRange(ThingSetMakerDefOf.VisitorGift.root.Generate(new ThingSetMakerParams
					{
						totalMarketValueRange = new FloatRange?(RitualOutcomeEffectWorker_Skylantern.RewardMarketValueRange)
					}));
				}
				IncidentParms incidentParms = new IncidentParms
				{
					target = jobRitual.Map,
					points = 160f,
					gifts = list,
					pawnIdeo = ideo,
					pawnCount = 3,
					storeGeneratedNeutralPawns = new List<Pawn>()
				};
				if (IncidentDefOf.WanderersSkylantern.Worker.TryExecute(incidentParms))
				{
					List<GlobalTargetInfo> list2 = new List<GlobalTargetInfo>();
					list2.AddRange(letterLookTargets.targets);
					list2.AddRange(from p in incidentParms.storeGeneratedNeutralPawns
					select new GlobalTargetInfo(p));
					letterLookTargets = new LookTargets(list2);
					extraOutcomeDesc = "RitualOutcomeExtraDesc_SkylanternWanderers".Translate();
				}
			}
		}

		// Token: 0x04009554 RID: 38228
		public const float WanderersChance = 0.2f;

		// Token: 0x04009555 RID: 38229
		public const float WanderersPoints = 160f;

		// Token: 0x04009556 RID: 38230
		public static readonly FloatRange RewardMarketValueRange = new FloatRange(200f, 500f);
	}
}
