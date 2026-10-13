using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032F9 RID: 13049
	public class FloatMenuOptionProvider_EnterMapPortal : FloatMenuOptionProvider
	{
		// Token: 0x17002F95 RID: 12181
		// (get) Token: 0x060125B2 RID: 75186 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F96 RID: 12182
		// (get) Token: 0x060125B3 RID: 75187 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F97 RID: 12183
		// (get) Token: 0x060125B4 RID: 75188 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Multiselect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F98 RID: 12184
		// (get) Token: 0x060125B5 RID: 75189 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool MechanoidCanDo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060125B6 RID: 75190 RVA: 0x00568EF4 File Offset: 0x005670F4
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			MapPortal portal = clickedThing as MapPortal;
			if (portal == null)
			{
				return null;
			}
			string t;
			if (!portal.IsEnterable(out t))
			{
				return new FloatMenuOption("CannotEnterPortal".Translate(portal.Label) + ": " + t, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.IsMultiselect)
			{
				AcceptanceReport acceptanceReport = FloatMenuOptionProvider_EnterMapPortal.CanEnterPortal(context.FirstSelectedPawn, portal);
				if (!acceptanceReport.Accepted)
				{
					return new FloatMenuOption("CannotEnterPortal".Translate(portal.Label) + ": " + acceptanceReport.Reason, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
			}
			FloatMenuOptionProvider_EnterMapPortal.tmpPortalEnteringPawns.Clear();
			foreach (Pawn item in context.ValidSelectedPawns)
			{
				if (FloatMenuOptionProvider_EnterMapPortal.CanEnterPortal(context.FirstSelectedPawn, portal))
				{
					FloatMenuOptionProvider_EnterMapPortal.tmpPortalEnteringPawns.Add(item);
				}
			}
			if (FloatMenuOptionProvider_EnterMapPortal.tmpPortalEnteringPawns.NullOrEmpty<Pawn>())
			{
				return null;
			}
			return new FloatMenuOption(portal.EnterString, delegate()
			{
				foreach (Pawn pawn in FloatMenuOptionProvider_EnterMapPortal.tmpPortalEnteringPawns)
				{
					Job job = JobMaker.MakeJob(JobDefOf.EnterPortal, portal);
					job.playerForced = true;
					pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				}
			}, MenuOptionPriority.High, null, null, 0f, null, null, true, 0);
		}

		// Token: 0x060125B7 RID: 75191 RVA: 0x00569078 File Offset: 0x00567278
		private static AcceptanceReport CanEnterPortal(Pawn pawn, MapPortal portal)
		{
			if (!pawn.CanReach(portal, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate();
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return "Incapable".Translate();
			}
			return true;
		}

		// Token: 0x0400BEDD RID: 48861
		private static List<Pawn> tmpPortalEnteringPawns = new List<Pawn>();
	}
}
