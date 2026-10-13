using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020029CF RID: 10703
	public class Building_ArchonexusCore : Building
	{
		// Token: 0x1700245A RID: 9306
		// (get) Token: 0x0600EBE2 RID: 60386 RVA: 0x00447F74 File Offset: 0x00446174
		public bool CanActivateNow
		{
			get
			{
				return !ArchonexusCountdown.CountdownActivated;
			}
		}

		// Token: 0x0600EBE3 RID: 60387 RVA: 0x00447F7E File Offset: 0x0044617E
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					action = new Action(this.Activate),
					Disabled = !this.CanActivateNow,
					defaultLabel = "DEV: Activate archonexus core"
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EBE4 RID: 60388 RVA: 0x00447F8E File Offset: 0x0044618E
		public void Activate()
		{
			if (!this.CanActivateNow)
			{
				return;
			}
			ArchonexusCountdown.InitiateCountdown(this);
		}

		// Token: 0x0600EBE5 RID: 60389 RVA: 0x00447F9F File Offset: 0x0044619F
		public override IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			if (!this.CanActivateNow)
			{
				yield return new FloatMenuOption("CannotInvoke".Translate("Power".Translate()) + ": " + "AlreadyInvoked".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			this.tmpPawnsCanReach.Clear();
			foreach (Pawn pawn in selPawns)
			{
				if (pawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					this.tmpPawnsCanReach.Add(pawn);
				}
			}
			if (this.tmpPawnsCanReach.NullOrEmpty<Pawn>())
			{
				yield return new FloatMenuOption("CannotInvoke".Translate("Power".Translate()) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			yield return new FloatMenuOption("Invoke".Translate("Power".Translate()), delegate()
			{
				this.tmpPawnsCanReach[0].jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.ActivateArchonexusCore, this), new JobTag?(JobTag.Misc), false);
				this.tmpObservationSpots.Clear();
				IntVec3 intVec = base.Position + ((this.InteractionCell - base.Position).ToVector3().normalized * 8f).ToIntVec3();
				for (int i = 1; i < this.tmpPawnsCanReach.Count; i++)
				{
					IntVec3 intVec2 = RCellFinder.BestOrderedGotoDestNear(intVec, this.tmpPawnsCanReach[i], (IntVec3 c) => !this.tmpObservationSpots.Contains(c), true);
					FloatMenuOptionProvider_DraftedMove.PawnGotoAction(intVec, this.tmpPawnsCanReach[i], intVec2);
					this.tmpObservationSpots.Add(intVec2);
				}
				this.tmpObservationSpots.Clear();
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield break;
		}

		// Token: 0x04009C7D RID: 40061
		private const int ObservationDistance = 8;

		// Token: 0x04009C7E RID: 40062
		private List<Pawn> tmpPawnsCanReach = new List<Pawn>();

		// Token: 0x04009C7F RID: 40063
		private List<IntVec3> tmpObservationSpots = new List<IntVec3>();
	}
}
