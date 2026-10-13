using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003891 RID: 14481
	public static class StudyUtility
	{
		// Token: 0x0601402D RID: 81965 RVA: 0x005F1230 File Offset: 0x005EF430
		public static void TargetHoldingPlatformForEntity(Pawn carrier, Thing entity, bool transferBetweenPlatforms = false, Thing sourcePlatform = null)
		{
			StudyUtility.<>c__DisplayClass0_0 CS$<>8__locals1 = new StudyUtility.<>c__DisplayClass0_0();
			CS$<>8__locals1.carrier = carrier;
			CS$<>8__locals1.transferBetweenPlatforms = transferBetweenPlatforms;
			CS$<>8__locals1.entity = entity;
			CS$<>8__locals1.sourcePlatform = sourcePlatform;
			Find.Targeter.BeginTargeting(TargetingParameters.ForBuilding(null), delegate(LocalTargetInfo t)
			{
				if (CS$<>8__locals1.carrier != null && !base.<TargetHoldingPlatformForEntity>g__CanReserveForTransfer|1(t))
				{
					Messages.Message("MessageHolderReserved".Translate(t.Thing.Label), MessageTypeDefOf.RejectInput, true);
					return;
				}
				foreach (Thing thing in Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.EntityHolder))
				{
					Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
					if (building_HoldingPlatform != null && CS$<>8__locals1.entity != building_HoldingPlatform.HeldPawn)
					{
						Pawn heldPawn = building_HoldingPlatform.HeldPawn;
						CompHoldingPlatformTarget compHoldingPlatformTarget = (heldPawn != null) ? heldPawn.TryGetComp<CompHoldingPlatformTarget>() : null;
						if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.targetHolder == t.Thing)
						{
							Messages.Message("MessageHolderReserved".Translate(t.Thing.Label), MessageTypeDefOf.RejectInput, true);
							return;
						}
					}
				}
				CompHoldingPlatformTarget compHoldingPlatformTarget2 = CS$<>8__locals1.entity.TryGetComp<CompHoldingPlatformTarget>();
				if (compHoldingPlatformTarget2 != null)
				{
					compHoldingPlatformTarget2.targetHolder = t.Thing;
				}
				if (CS$<>8__locals1.carrier != null)
				{
					Job job = CS$<>8__locals1.transferBetweenPlatforms ? JobMaker.MakeJob(JobDefOf.TransferBetweenEntityHolders, CS$<>8__locals1.sourcePlatform, t, CS$<>8__locals1.entity) : JobMaker.MakeJob(JobDefOf.CarryToEntityHolder, t, CS$<>8__locals1.entity);
					job.count = 1;
					CS$<>8__locals1.carrier.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}
				if (t.Thing != null && !t.Thing.SafelyContains(CS$<>8__locals1.entity))
				{
					Messages.Message("MessageTargetBelowMinimumContainmentStrength".Translate(t.Thing.Label, CS$<>8__locals1.entity.Label), MessageTypeDefOf.ThreatSmall, true);
				}
			}, delegate(LocalTargetInfo t)
			{
				if (base.<TargetHoldingPlatformForEntity>g__ValidateTarget|0(t))
				{
					GenDraw.DrawTargetHighlight(t);
				}
			}, new Func<LocalTargetInfo, bool>(CS$<>8__locals1.<TargetHoldingPlatformForEntity>g__ValidateTarget|0), null, null, BaseContent.ClearTex, true, delegate(LocalTargetInfo t)
			{
				Thing thing = t.Thing;
				CompEntityHolder compEntityHolder = (thing != null) ? thing.TryGetComp<CompEntityHolder>() : null;
				TaggedString taggedString;
				if (compEntityHolder == null)
				{
					taggedString = "ChooseEntityHolder".Translate().CapitalizeFirst() + "...";
					Widgets.MouseAttachedLabel(taggedString, 0f, 0f, null);
					return;
				}
				Pawn pawn = null;
				if (CS$<>8__locals1.carrier != null)
				{
					pawn = t.Thing.Map.reservationManager.FirstRespectedReserver(t.Thing, CS$<>8__locals1.carrier, null);
				}
				else
				{
					Building_HoldingPlatform building_HoldingPlatform = t.Thing as Building_HoldingPlatform;
					Pawn pawn2;
					if (building_HoldingPlatform != null && StudyUtility.AlreadyReserved(building_HoldingPlatform, out pawn2))
					{
						pawn = pawn2;
					}
				}
				if (pawn != null)
				{
					taggedString = string.Format("{0}: {1}", "EntityHolderReservedBy".Translate(), pawn.LabelShortCap);
				}
				else
				{
					taggedString = "FloatMenuContainmentStrength".Translate() + ": " + StatDefOf.ContainmentStrength.Worker.ValueToString(compEntityHolder.ContainmentStrength, false, ToStringNumberSense.Absolute);
					taggedString += "\n" + ("FloatMenuContainmentRequires".Translate(CS$<>8__locals1.entity).CapitalizeFirst() + ": " + StatDefOf.MinimumContainmentStrength.Worker.ValueToString(CS$<>8__locals1.entity.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1), false, ToStringNumberSense.Absolute)).Colorize(t.Thing.SafelyContains(CS$<>8__locals1.entity) ? Color.white : Color.red);
				}
				Widgets.MouseAttachedLabel(taggedString, 0f, 0f, null);
			}, delegate(LocalTargetInfo t)
			{
				foreach (Building building in CS$<>8__locals1.entity.MapHeld.listerBuildings.AllBuildingsColonistOfGroup(ThingRequestGroup.EntityHolder))
				{
					if (base.<TargetHoldingPlatformForEntity>g__ValidateTarget|0(building) && (CS$<>8__locals1.carrier == null || base.<TargetHoldingPlatformForEntity>g__CanReserveForTransfer|1(building)))
					{
						GenDraw.DrawArrowPointingAt(building.DrawPos, false);
					}
				}
			});
		}

		// Token: 0x0601402E RID: 81966 RVA: 0x005F12B4 File Offset: 0x005EF4B4
		public static bool TryFindResearchBench(Pawn pawn, out Building_ResearchBench bench)
		{
			bench = (Building_ResearchBench)GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.ResearchBench), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Some, TraverseMode.ByPawn, false, false, false, true), 9999f, delegate(Thing t)
			{
				if (pawn.CanReserve(t, 1, -1, null, false))
				{
					CompPowerTrader compPowerTrader = t.TryGetComp<CompPowerTrader>();
					return compPowerTrader == null || compPowerTrader.PowerOn;
				}
				return false;
			}, null, 0, -1, false, RegionType.Set_Passable, false, false);
			return bench != null;
		}

		// Token: 0x0601402F RID: 81967 RVA: 0x005F1328 File Offset: 0x005EF528
		public static bool HoldingPlatformAvailableOnCurrentMap()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return false;
			}
			foreach (Building building in currentMap.listerBuildings.allBuildingsColonist)
			{
				CompEntityHolder compEntityHolder;
				Pawn pawn;
				if (building.TryGetComp(out compEntityHolder) && compEntityHolder.Available && !StudyUtility.AlreadyReserved(building, out pawn))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06014030 RID: 81968 RVA: 0x005F13AC File Offset: 0x005EF5AC
		public static bool IsAnomalyResearchCategory(this KnowledgeCategoryDef def)
		{
			return ModsConfig.AnomalyActive && def != null && (def == KnowledgeCategoryDefOf.Basic || def == KnowledgeCategoryDefOf.Advanced);
		}

		// Token: 0x06014031 RID: 81969 RVA: 0x005F13CC File Offset: 0x005EF5CC
		public static bool AlreadyReserved(Thing p, out Pawn reserver)
		{
			StudyUtility.tmpReservers.Clear();
			p.Map.reservationManager.ReserversOf(p, StudyUtility.tmpReservers);
			reserver = StudyUtility.tmpReservers.FirstOrDefault<Pawn>();
			if (reserver != null)
			{
				return true;
			}
			foreach (Thing thing in p.Map.listerThings.ThingsInGroup(ThingRequestGroup.HoldingPlatformTarget))
			{
				if (thing.TryGetComp<CompHoldingPlatformTarget>().targetHolder == p)
				{
					reserver = (thing as Pawn);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400DD1A RID: 56602
		private static readonly HashSet<Pawn> tmpReservers = new HashSet<Pawn>();
	}
}
