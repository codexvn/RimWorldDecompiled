using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032E1 RID: 13025
	public class FloatMenuOptionProvider_CleanRoom : FloatMenuOptionProvider
	{
		// Token: 0x17002F6D RID: 12141
		// (get) Token: 0x06012535 RID: 75061 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Drafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F6E RID: 12142
		// (get) Token: 0x06012536 RID: 75062 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F6F RID: 12143
		// (get) Token: 0x06012537 RID: 75063 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F70 RID: 12144
		// (get) Token: 0x06012538 RID: 75064 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06012539 RID: 75065 RVA: 0x00566618 File Offset: 0x00564818
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return context.ClickedRoom != null && !context.ClickedRoom.PsychologicallyOutdoors && context.ClickedRoom.ProperRoom && !context.ClickedRoom.TouchesMapEdge && !context.ClickedRoom.IsHuge && (!context.map.IsPocketMap || context.map.generatorDef.pocketMapProperties.canBeCleaned) && !context.FirstSelectedPawn.WorkTypeIsDisabled(WorkTypeDefOf.Cleaning);
		}

		// Token: 0x0601253A RID: 75066 RVA: 0x005666A0 File Offset: 0x005648A0
		protected override FloatMenuOption GetSingleOption(FloatMenuContext context)
		{
			List<Filth> filth = FloatMenuOptionProvider_CleanRoom.GetRoomFilthCleanableByPawn(context.ClickedRoom, context.FirstSelectedPawn);
			if (filth.Count == 0)
			{
				return null;
			}
			string roomRoleLabel = context.ClickedRoom.GetRoomRoleLabel();
			return new FloatMenuOption("CleanRoom".Translate(roomRoleLabel), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.Clean);
				foreach (Filth t in filth)
				{
					job.AddQueuedTarget(TargetIndex.A, t);
				}
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.VeryLow, null, null, 0f, null, null, true, 0);
		}

		// Token: 0x0601253B RID: 75067 RVA: 0x00566730 File Offset: 0x00564930
		private static List<Filth> GetRoomFilthCleanableByPawn(Room room, Pawn pawn)
		{
			if (pawn.Map.IsPocketMap && !pawn.Map.generatorDef.pocketMapProperties.canBeCleaned)
			{
				return null;
			}
			if (room == null || room.Dereferenced || room.Fogged)
			{
				return null;
			}
			if (room.IsHuge || room.TouchesMapEdge)
			{
				return null;
			}
			FloatMenuOptionProvider_CleanRoom.tmpFilth.Clear();
			foreach (Filth filth in room.ContainedAndAdjacentThings.OfType<Filth>())
			{
				if (!filth.IsForbidden(pawn) && filth.Map.areaManager.Home[filth.Position] && pawn.CanReach(filth, PathEndMode.Touch, pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn) && pawn.CanReserve(filth, 1, -1, null, false))
				{
					FloatMenuOptionProvider_CleanRoom.tmpFilth.Add(filth);
				}
			}
			return FloatMenuOptionProvider_CleanRoom.tmpFilth;
		}

		// Token: 0x0400BEA0 RID: 48800
		private static List<Filth> tmpFilth = new List<Filth>();
	}
}
