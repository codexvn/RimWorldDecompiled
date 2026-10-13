using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014B2 RID: 5298
	public class JobGiver_OptimizeApparel : ThinkNode_JobGiver
	{
		// Token: 0x06007F04 RID: 32516 RVA: 0x00263537 File Offset: 0x00261737
		private static void SetNextOptimizeTick(Pawn pawn)
		{
			pawn.mindState.nextApparelOptimizeTick = Find.TickManager.TicksGame + Rand.Range(6000, 9000);
		}

		// Token: 0x06007F05 RID: 32517 RVA: 0x00263560 File Offset: 0x00261760
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.outfits == null)
			{
				Log.ErrorOnce(string.Format("{0} tried to run JobGiver_OptimizeApparel without an OutfitTracker", pawn), 5643897);
				return null;
			}
			if (pawn.Faction != Faction.OfPlayer)
			{
				Log.ErrorOnce(string.Format("Non-colonist {0} tried to optimize apparel.", pawn), 764323);
				return null;
			}
			if (pawn.IsMutant && pawn.mutant.Def.disableApparel)
			{
				return null;
			}
			if (pawn.IsQuestLodger())
			{
				return null;
			}
			if (!DebugViewSettings.debugApparelOptimize)
			{
				if (Find.TickManager.TicksGame < pawn.mindState.nextApparelOptimizeTick)
				{
					return null;
				}
			}
			else
			{
				JobGiver_OptimizeApparel.debugSb = new StringBuilder();
				JobGiver_OptimizeApparel.debugSb.AppendLine(string.Format("Scanning for {0} at {1}", pawn, pawn.Position));
			}
			Job result;
			if (ModsConfig.IdeologyActive && JobGiver_OptimizeApparel.TryCreateRecolorJob(pawn, out result, false))
			{
				return result;
			}
			ApparelPolicy currentApparelPolicy = pawn.outfits.CurrentApparelPolicy;
			List<Apparel> wornApparel = pawn.apparel.WornApparel;
			for (int i = wornApparel.Count - 1; i >= 0; i--)
			{
				if (!currentApparelPolicy.filter.Allows(wornApparel[i]) && pawn.outfits.forcedHandler.AllowedToAutomaticallyDrop(wornApparel[i]) && !pawn.apparel.IsLocked(wornApparel[i]))
				{
					Job job = JobMaker.MakeJob(JobDefOf.RemoveApparel, wornApparel[i]);
					job.haulDroppedApparel = true;
					return job;
				}
			}
			Thing thing = null;
			float num = 0f;
			JobGiver_OptimizeApparel.tmpApparelList.Clear();
			pawn.Map.listerThings.GetAllThings(JobGiver_OptimizeApparel.tmpApparelList, ThingRequestGroup.Apparel, null, true);
			foreach (IHaulSource haulSource in pawn.Map.haulDestinationManager.AllHaulSourcesListForReading)
			{
				foreach (Thing thing2 in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
				{
					Apparel apparel = thing2 as Apparel;
					if (apparel != null)
					{
						JobGiver_OptimizeApparel.tmpApparelList.Add(apparel);
					}
				}
			}
			if (JobGiver_OptimizeApparel.tmpApparelList.Count == 0)
			{
				JobGiver_OptimizeApparel.SetNextOptimizeTick(pawn);
				return null;
			}
			JobGiver_OptimizeApparel.neededWarmth = PawnApparelGenerator.CalculateNeededWarmth(pawn, pawn.Map.TileInfo.tile, GenLocalDate.Twelfth(pawn));
			JobGiver_OptimizeApparel.wornApparelScores.Clear();
			for (int j = 0; j < wornApparel.Count; j++)
			{
				JobGiver_OptimizeApparel.wornApparelScores.Add(JobGiver_OptimizeApparel.ApparelScoreRaw(pawn, wornApparel[j]));
			}
			for (int k = 0; k < JobGiver_OptimizeApparel.tmpApparelList.Count; k++)
			{
				Apparel apparel2 = (Apparel)JobGiver_OptimizeApparel.tmpApparelList[k];
				if (currentApparelPolicy.filter.Allows(apparel2) && apparel2.IsInAnyStorage() && !apparel2.IsForbidden(pawn) && !apparel2.IsBurning() && (apparel2.def.apparel.gender == Gender.None || apparel2.def.apparel.gender == pawn.gender))
				{
					float num2 = JobGiver_OptimizeApparel.ApparelScoreGain(pawn, apparel2, JobGiver_OptimizeApparel.wornApparelScores);
					if (DebugViewSettings.debugApparelOptimize)
					{
						JobGiver_OptimizeApparel.debugSb.AppendLine(string.Format("{0}: {1:F2}", apparel2.LabelCap, num2));
					}
					if (num2 >= 0.05f && num2 >= num && (!CompBiocodable.IsBiocoded(apparel2) || CompBiocodable.IsBiocodedFor(apparel2, pawn)) && ApparelUtility.HasPartsToWear(pawn, apparel2.def))
					{
						LocalTargetInfo target = apparel2;
						IApparelSource apparelSource = apparel2.ParentHolder as IApparelSource;
						if (apparelSource != null)
						{
							Thing thing3 = apparelSource as Thing;
							if (thing3 != null)
							{
								if (!apparelSource.ApparelSourceEnabled)
								{
									goto IL_3D6;
								}
								target = thing3;
							}
						}
						if (pawn.CanReserveAndReach(target, PathEndMode.OnCell, pawn.NormalMaxDanger(), 1, -1, null, false) && apparel2.def.apparel.developmentalStageFilter.Has(pawn.DevelopmentalStage))
						{
							thing = apparel2;
							num = num2;
						}
					}
				}
				IL_3D6:;
			}
			JobGiver_OptimizeApparel.tmpApparelList.Clear();
			if (DebugViewSettings.debugApparelOptimize)
			{
				JobGiver_OptimizeApparel.debugSb.AppendLine(string.Format("BEST: {0}", thing));
				Log.Message(JobGiver_OptimizeApparel.debugSb.ToString());
				JobGiver_OptimizeApparel.debugSb = null;
			}
			if (thing == null)
			{
				JobGiver_OptimizeApparel.SetNextOptimizeTick(pawn);
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Wear, thing);
		}

		// Token: 0x06007F06 RID: 32518 RVA: 0x002639D0 File Offset: 0x00261BD0
		public static bool TryCreateRecolorJob(Pawn pawn, out Job job, bool dryRun = false)
		{
			if (!ModLister.CheckIdeology("Apparel recoloring"))
			{
				job = null;
				return false;
			}
			if (pawn.apparel.AnyApparelNeedsRecoloring)
			{
				Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.StylingStation), PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, (Thing t) => !t.IsForbidden(pawn) && pawn.CanReserve(t, 1, -1, null, false) && pawn.CanReserveSittableOrSpot(t.InteractionCell, false), null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (thing != null)
				{
					try
					{
						foreach (Apparel apparel in pawn.apparel.WornApparel)
						{
							if (apparel.DesiredColor != null)
							{
								JobGiver_OptimizeApparel.tmpApparelToRecolor.Add(apparel);
							}
						}
						List<Thing> list = pawn.Map.listerThings.ThingsOfDef(ThingDefOf.Dye);
						if (JobGiver_OptimizeApparel.tmpApparelToRecolor.Count > 0)
						{
							list.SortBy((Thing t) => t.Position.DistanceToSquared(pawn.Position));
							foreach (Thing thing2 in list)
							{
								if (pawn.CanReach(thing2, PathEndMode.Touch, Danger.Some, false, false, TraverseMode.ByPawn) && !thing2.IsForbidden(pawn))
								{
									int num = 0;
									while (num < thing2.stackCount && pawn.CanReserve(thing2, 1, num + 1, null, false))
									{
										JobGiver_OptimizeApparel.tmpQueueApparel.Add(JobGiver_OptimizeApparel.tmpApparelToRecolor[JobGiver_OptimizeApparel.tmpApparelToRecolor.Count - 1]);
										if (!JobGiver_OptimizeApparel.tmpQueueDye.Contains(thing2))
										{
											JobGiver_OptimizeApparel.tmpQueueDye.Add(thing2);
										}
										JobGiver_OptimizeApparel.tmpApparelToRecolor.RemoveAt(JobGiver_OptimizeApparel.tmpApparelToRecolor.Count - 1);
										if (JobGiver_OptimizeApparel.tmpApparelToRecolor.Count == 0)
										{
											break;
										}
										num++;
									}
									if (JobGiver_OptimizeApparel.tmpApparelToRecolor.Count == 0)
									{
										break;
									}
								}
							}
							if (JobGiver_OptimizeApparel.tmpQueueApparel.Count > 0)
							{
								if (dryRun)
								{
									job = null;
								}
								else
								{
									job = JobMaker.MakeJob(JobDefOf.RecolorApparel);
									List<LocalTargetInfo> targetQueue = job.GetTargetQueue(TargetIndex.A);
									List<LocalTargetInfo> targetQueue2 = job.GetTargetQueue(TargetIndex.B);
									targetQueue.AddRange(JobGiver_OptimizeApparel.tmpQueueDye);
									targetQueue2.AddRange(JobGiver_OptimizeApparel.tmpQueueApparel);
									job.SetTarget(TargetIndex.C, thing);
									job.count = JobGiver_OptimizeApparel.tmpQueueApparel.Count;
								}
								return true;
							}
						}
					}
					finally
					{
						JobGiver_OptimizeApparel.tmpApparelToRecolor.Clear();
						JobGiver_OptimizeApparel.tmpQueueApparel.Clear();
						JobGiver_OptimizeApparel.tmpQueueDye.Clear();
					}
				}
			}
			job = null;
			return false;
		}

		// Token: 0x06007F07 RID: 32519 RVA: 0x00263CF0 File Offset: 0x00261EF0
		public static float ApparelScoreGain(Pawn pawn, Apparel ap, List<float> wornScoresCache)
		{
			if (ap.def == ThingDefOf.Apparel_ShieldBelt && pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsWeaponUsingProjectiles)
			{
				return -1000f;
			}
			if (ap.def.apparel.ignoredByNonViolent && pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return -1000f;
			}
			float num = JobGiver_OptimizeApparel.ApparelScoreRaw(pawn, ap);
			List<Apparel> wornApparel = pawn.apparel.WornApparel;
			bool flag = false;
			for (int i = 0; i < wornApparel.Count; i++)
			{
				if (!ApparelUtility.CanWearTogether(wornApparel[i].def, ap.def, pawn.RaceProps.body))
				{
					if (!pawn.outfits.forcedHandler.AllowedToAutomaticallyDrop(wornApparel[i]) || pawn.apparel.IsLocked(wornApparel[i]))
					{
						return -1000f;
					}
					num -= wornScoresCache[i];
					flag = true;
				}
			}
			if (!flag)
			{
				num *= 10f;
			}
			return num;
		}

		// Token: 0x06007F08 RID: 32520 RVA: 0x00263DEC File Offset: 0x00261FEC
		public static float ApparelScoreRaw(Pawn pawn, Apparel ap)
		{
			if (!ap.PawnCanWear(pawn, true))
			{
				return -10f;
			}
			if (ap.def.apparel.blocksVision)
			{
				return -10f;
			}
			if (ap.def.apparel.slaveApparel && !pawn.IsSlave)
			{
				return -10f;
			}
			if (ap.def.apparel.mechanitorApparel && pawn.mechanitor == null)
			{
				return -10f;
			}
			float num = 0.1f + ap.def.apparel.scoreOffset;
			float num2 = ap.GetStatValue(StatDefOf.ArmorRating_Sharp, true, -1) + ap.GetStatValue(StatDefOf.ArmorRating_Blunt, true, -1);
			num += num2;
			if (ap.def.useHitPoints)
			{
				float x = (float)ap.HitPoints / (float)ap.MaxHitPoints;
				num *= JobGiver_OptimizeApparel.HitPointsPercentScoreFactorCurve.Evaluate(x);
			}
			num += ap.GetSpecialApparelScoreOffset();
			float num3 = 1f;
			if (JobGiver_OptimizeApparel.neededWarmth == NeededWarmth.Warm)
			{
				float statValue = ap.GetStatValue(StatDefOf.Insulation_Cold, true, -1);
				num3 *= JobGiver_OptimizeApparel.InsulationColdScoreFactorCurve_NeedWarm.Evaluate(statValue);
			}
			num *= num3;
			if (ap.WornByCorpse && (pawn == null || ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.DeadMansApparel, true)))
			{
				num -= 0.5f;
				if (num > 0f)
				{
					num *= 0.1f;
				}
			}
			if (ap.Stuff == ThingDefOf.Human.race.leatherDef)
			{
				if (pawn.Ideo != null && pawn.Ideo.LikesHumanLeatherApparel)
				{
					num += 0.12f;
				}
				else
				{
					if (pawn == null || ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.HumanLeatherApparelSad, true))
					{
						num -= 0.5f;
						if (num > 0f)
						{
							num *= 0.1f;
						}
					}
					if (pawn != null && ThoughtUtility.CanGetThought(pawn, ThoughtDefOf.HumanLeatherApparelHappy, true))
					{
						num += 0.12f;
					}
				}
			}
			if (pawn != null && !ap.def.apparel.CorrectGenderForWearing(pawn.gender))
			{
				num *= 0.01f;
			}
			bool flag = false;
			if (pawn != null)
			{
				foreach (ApparelRequirementWithSource apparelRequirementWithSource in pawn.apparel.AllRequirements)
				{
					foreach (BodyPartGroupDef item in apparelRequirementWithSource.requirement.bodyPartGroupsMatchAny)
					{
						if (ap.def.apparel.bodyPartGroups.Contains(item))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				bool flag2 = false;
				bool flag3 = false;
				foreach (ApparelRequirementWithSource apparelRequirementWithSource2 in pawn.apparel.AllRequirements)
				{
					if (apparelRequirementWithSource2.requirement.RequiredForPawn(pawn, ap.def, false))
					{
						flag2 = true;
					}
					if (apparelRequirementWithSource2.requirement.AllowedForPawn(pawn, ap.def, false))
					{
						flag3 = true;
					}
				}
				if (flag2)
				{
					num *= 25f;
				}
				else if (flag3)
				{
					num *= 10f;
				}
			}
			if (pawn != null && pawn.royalty != null && pawn.royalty.AllTitlesInEffectForReading.Count > 0)
			{
				QualityCategory qualityCategory = QualityCategory.Awful;
				foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesInEffectForReading)
				{
					if (royalTitle.def.requiredMinimumApparelQuality > qualityCategory)
					{
						qualityCategory = royalTitle.def.requiredMinimumApparelQuality;
					}
				}
				QualityCategory qualityCategory2;
				if (ap.TryGetQuality(out qualityCategory2) && qualityCategory2 < qualityCategory)
				{
					num *= 0.25f;
				}
			}
			return num;
		}

		// Token: 0x04005512 RID: 21778
		private static NeededWarmth neededWarmth;

		// Token: 0x04005513 RID: 21779
		private static StringBuilder debugSb;

		// Token: 0x04005514 RID: 21780
		private static readonly List<float> wornApparelScores = new List<float>();

		// Token: 0x04005515 RID: 21781
		private static readonly List<Thing> tmpApparelList = new List<Thing>();

		// Token: 0x04005516 RID: 21782
		private const int ApparelOptimizeCheckIntervalMin = 6000;

		// Token: 0x04005517 RID: 21783
		private const int ApparelOptimizeCheckIntervalMax = 9000;

		// Token: 0x04005518 RID: 21784
		private const float MinScoreGainToCare = 0.05f;

		// Token: 0x04005519 RID: 21785
		private const float ScoreFactorIfNotReplacing = 10f;

		// Token: 0x0400551A RID: 21786
		private static readonly SimpleCurve InsulationColdScoreFactorCurve_NeedWarm = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(30f, 8f),
				true
			}
		};

		// Token: 0x0400551B RID: 21787
		private static readonly SimpleCurve HitPointsPercentScoreFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(0.2f, 0.2f),
				true
			},
			{
				new CurvePoint(0.22f, 0.3f),
				true
			},
			{
				new CurvePoint(0.5f, 0.3f),
				true
			},
			{
				new CurvePoint(0.52f, 1f),
				true
			}
		};

		// Token: 0x0400551C RID: 21788
		private static readonly List<LocalTargetInfo> tmpQueueDye = new List<LocalTargetInfo>();

		// Token: 0x0400551D RID: 21789
		private static readonly List<LocalTargetInfo> tmpQueueApparel = new List<LocalTargetInfo>();

		// Token: 0x0400551E RID: 21790
		private static readonly List<Apparel> tmpApparelToRecolor = new List<Apparel>();
	}
}
