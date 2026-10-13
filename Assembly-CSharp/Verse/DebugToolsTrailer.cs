using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000A68 RID: 2664
	public static class DebugToolsTrailer
	{
		// Token: 0x060041F1 RID: 16881 RVA: 0x00157604 File Offset: 0x00155804
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000, requiresOdyssey = true)]
		private static void SwimAt(Pawn pawn)
		{
			DebugTools.curTool = new DebugTool("Select destination...", delegate()
			{
				List<IntVec3> list;
				if (!SwimPathFinder.TryFindSwimPath(pawn, UI.MouseCell(), out list))
				{
					return;
				}
				pawn.needs.joy.CurLevelPercentage = 0.3f;
				Job job = JobMaker.MakeJob(JobDefOf.GoSwimming, list[0]);
				job.locomotionUrgency = LocomotionUrgency.Walk;
				job.targetQueueA = new List<LocalTargetInfo>();
				for (int i = 1; i < list.Count; i++)
				{
					job.targetQueueA.Add(list[i]);
				}
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				DebugTools.curTool = null;
			}, null);
		}

		// Token: 0x060041F2 RID: 16882 RVA: 0x0015763C File Offset: 0x0015583C
		[DebugAction("Pawns", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = 1000)]
		private static void MeleeAttackTarget(Pawn p)
		{
			DebugTools.curTool = new DebugTool("Select target...", delegate()
			{
				Pawn t = DebugToolsTrailer.<MeleeAttackTarget>g__PawnAt|1_0(UI.MouseCell());
				Job job = JobMaker.MakeJob(JobDefOf.AttackMelee);
				job.targetA = t;
				p.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, null);
		}

		// Token: 0x060041F3 RID: 16883 RVA: 0x00157672 File Offset: 0x00155872
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000, requiresOdyssey = true)]
		private static void VolcanicDebris()
		{
			SkyfallerMaker.SpawnSkyfaller(ThingDefOf.LavaRockIncoming, UI.MouseCell(), Find.CurrentMap);
		}

		// Token: 0x060041F4 RID: 16884 RVA: 0x0015768C File Offset: 0x0015588C
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMap, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000, requiresOdyssey = true)]
		private static List<DebugActionNode> VolcanicDebrisDelayed()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			for (int i = 0; i < 41; i++)
			{
				int delay = i * 30;
				list.Add(new DebugActionNode(((float)delay / 60f).ToString("F1") + " seconds", DebugActionType.ToolMap, delegate()
				{
					SkyfallerMaker.SpawnSkyfaller(ThingDefOf.LavaRockIncoming, UI.MouseCell(), Find.CurrentMap).ticksToImpact += delay;
				}, null));
			}
			return list;
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x001576FC File Offset: 0x001558FC
		[DebugAction("General", null, false, false, false, false, false, 0, false, actionType = DebugActionType.ToolMapForPawns, allowedGameStates = AllowedGameStates.PlayingOnMap, displayPriority = -1000, requiresOdyssey = true)]
		private static void NuzzlePawn(Pawn animal)
		{
			if (!animal.IsAnimal)
			{
				Messages.Message("Must select an animal", MessageTypeDefOf.RejectInput, true);
				return;
			}
			DebugTools.curTool = new DebugTool("Select target...", delegate()
			{
				Pawn pawn = animal.Map.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>().FirstOrDefault<Pawn>();
				if (pawn == null || !pawn.RaceProps.Humanlike)
				{
					return;
				}
				pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Wait, 3000, false), new JobTag?(JobTag.Misc), false);
				Job job = JobMaker.MakeJob(JobDefOf.Nuzzle, pawn);
				job.locomotionUrgency = LocomotionUrgency.Walk;
				job.expiryInterval = 3000;
				animal.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				DebugTools.curTool = null;
			}, null);
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x00157750 File Offset: 0x00155950
		[CompilerGenerated]
		internal static Pawn <MeleeAttackTarget>g__PawnAt|1_0(IntVec3 c)
		{
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(c))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					return pawn;
				}
			}
			return null;
		}
	}
}
