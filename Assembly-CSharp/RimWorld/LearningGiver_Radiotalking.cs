using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200151D RID: 5405
	public class LearningGiver_Radiotalking : LearningGiver
	{
		// Token: 0x170013BC RID: 5052
		// (get) Token: 0x060080BA RID: 32954 RVA: 0x0026C78D File Offset: 0x0026A98D
		public override bool CanGiveDesire
		{
			get
			{
				return ResearchProjectDefOf.MicroelectronicsBasics.IsFinished;
			}
		}

		// Token: 0x060080BB RID: 32955 RVA: 0x0026C79C File Offset: 0x0026A99C
		private bool TryFindCommsConsole(Pawn pawn, out Thing commsConsole)
		{
			commsConsole = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForDef(ThingDefOf.CommsConsole), PathEndMode.InteractionCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, delegate(Thing t)
			{
				Building_CommsConsole building_CommsConsole = t as Building_CommsConsole;
				return building_CommsConsole != null && building_CommsConsole.CanUseCommsNow && pawn.CanReserve(building_CommsConsole, 1, -1, null, false) && !building_CommsConsole.IsForbidden(pawn);
			}, null, 0, -1, false, RegionType.Set_Passable, false, false);
			return commsConsole != null;
		}

		// Token: 0x060080BC RID: 32956 RVA: 0x0026C810 File Offset: 0x0026AA10
		public override bool CanDo(Pawn pawn)
		{
			Thing thing;
			return base.CanDo(pawn) && this.TryFindCommsConsole(pawn, out thing);
		}

		// Token: 0x060080BD RID: 32957 RVA: 0x0026C834 File Offset: 0x0026AA34
		public override Job TryGiveJob(Pawn pawn)
		{
			Thing t;
			if (!this.TryFindCommsConsole(pawn, out t))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, t);
		}
	}
}
