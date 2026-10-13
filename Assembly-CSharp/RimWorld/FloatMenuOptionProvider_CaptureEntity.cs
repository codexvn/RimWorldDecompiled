using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032BF RID: 12991
	public class FloatMenuOptionProvider_CaptureEntity : FloatMenuOptionProvider
	{
		// Token: 0x17002F3A RID: 12090
		// (get) Token: 0x0601249B RID: 74907 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F3B RID: 12091
		// (get) Token: 0x0601249C RID: 74908 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F3C RID: 12092
		// (get) Token: 0x0601249D RID: 74909 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F3D RID: 12093
		// (get) Token: 0x0601249E RID: 74910 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0601249F RID: 74911 RVA: 0x005631A8 File Offset: 0x005613A8
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.AnomalyActive;
		}

		// Token: 0x060124A0 RID: 74912 RVA: 0x005631AF File Offset: 0x005613AF
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			CompHoldingPlatformTarget holdComp;
			if (!clickedThing.TryGetComp(out holdComp) || !holdComp.CanBeCaptured || !holdComp.StudiedAtHoldingPlatform)
			{
				yield break;
			}
			if (!context.FirstSelectedPawn.CanReserveAndReach(clickedThing, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true))
			{
				yield return new FloatMenuOption("CannotGenericWorkCustom".Translate("CaptureLower".Translate(clickedThing)) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			IEnumerable<Building_HoldingPlatform> buildings = from x in context.FirstSelectedPawn.Map.listerBuildings.AllBuildingsColonistOfClass<Building_HoldingPlatform>()
			where !x.Occupied && context.FirstSelectedPawn.CanReserveAndReach(x, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false)
			select x;
			Thing building = GenClosest.ClosestThing_Global_Reachable(context.FirstSelectedPawn.Position, context.FirstSelectedPawn.Map, buildings, PathEndMode.ClosestTouch, TraverseParms.For(context.FirstSelectedPawn, Danger.Some, TraverseMode.ByPawn, false, false, false, true), 9999f, null, delegate(Thing t)
			{
				CompEntityHolder compEntityHolder = t.TryGetComp<CompEntityHolder>();
				if (compEntityHolder != null && compEntityHolder.ContainmentStrength >= clickedThing.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1))
				{
					return compEntityHolder.ContainmentStrength / Mathf.Max(clickedThing.PositionHeld.DistanceTo(t.Position), 1f);
				}
				return 0f;
			}, false);
			if (building == null)
			{
				yield return new FloatMenuOption("CannotGenericWorkCustom".Translate("CaptureLower".Translate(clickedThing)) + ": " + "NoHoldingPlatformsAvailable".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Capture".Translate(clickedThing.Label, clickedThing), delegate()
			{
				if (!ContainmentUtility.SafeContainerExistsFor(clickedThing))
				{
					Messages.Message("MessageNoRoomWithMinimumContainmentStrength".Translate(clickedThing.Label), MessageTypeDefOf.ThreatSmall, true);
				}
				holdComp.targetHolder = building;
				Job job = JobMaker.MakeJob(JobDefOf.CarryToEntityHolder, building, clickedThing);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			if (buildings.Count<Building_HoldingPlatform>() > 1)
			{
				yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Capture".Translate(clickedThing.Label, clickedThing) + " (" + "ChooseEntityHolder".Translate() + "...)", delegate()
				{
					StudyUtility.TargetHoldingPlatformForEntity(context.FirstSelectedPawn, clickedThing, false, null);
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			}
			yield break;
		}
	}
}
