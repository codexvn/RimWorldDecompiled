using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002D7C RID: 11644
	public class CompObeliskDeactivationInteractor : CompInteractable
	{
		// Token: 0x170029AE RID: 10670
		// (get) Token: 0x060105DB RID: 67035 RVA: 0x004B91A8 File Offset: 0x004B73A8
		private CompObelisk ObeliskComp
		{
			get
			{
				CompObelisk result;
				if ((result = this.obeliskComp) == null)
				{
					result = (this.obeliskComp = this.parent.GetComp<CompObelisk>());
				}
				return result;
			}
		}

		// Token: 0x170029AF RID: 10671
		// (get) Token: 0x060105DC RID: 67036 RVA: 0x004B91D3 File Offset: 0x004B73D3
		private new CompProperties_ObeliskDeactivationInteractor Props
		{
			get
			{
				return (CompProperties_ObeliskDeactivationInteractor)this.props;
			}
		}

		// Token: 0x170029B0 RID: 10672
		// (get) Token: 0x060105DD RID: 67037 RVA: 0x004B91E0 File Offset: 0x004B73E0
		public override string ExposeKey
		{
			get
			{
				return "Deactivation";
			}
		}

		// Token: 0x170029B1 RID: 10673
		// (get) Token: 0x060105DE RID: 67038 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanCooldown
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060105DF RID: 67039 RVA: 0x004B91E8 File Offset: 0x004B73E8
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (!this.ObeliskComp.StudyFinished || this.ObeliskComp.Activated || this.ObeliskComp.ActivityComp.Deactivated)
			{
				return false;
			}
			if (activateBy != null)
			{
				if (checkOptionalItems && !activateBy.HasReserved(ThingDefOf.Shard) && !ReservationUtility.ExistsUnreservedAmountOfDef(this.parent.MapHeld, ThingDefOf.Shard, Faction.OfPlayer, this.Props.shardsRequired, (Thing t) => activateBy.CanReserveAndReach(t, PathEndMode.Touch, Danger.None, 1, -1, null, false)))
				{
					return "ObeliskDeactivateMissingShards".Translate(this.Props.shardsRequired);
				}
			}
			else if (checkOptionalItems && !ReservationUtility.ExistsUnreservedAmountOfDef(this.parent.MapHeld, ThingDefOf.Shard, Faction.OfPlayer, this.Props.shardsRequired, null))
			{
				return "ObeliskDeactivateMissingShards".Translate(this.Props.shardsRequired);
			}
			return base.CanInteract(activateBy, checkOptionalItems);
		}

		// Token: 0x060105E0 RID: 67040 RVA: 0x004B9303 File Offset: 0x004B7503
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!this.ObeliskComp.StudyFinished || this.ObeliskComp.Activated || this.ObeliskComp.ActivityComp.Deactivated)
			{
				yield break;
			}
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060105E1 RID: 67041 RVA: 0x004B9313 File Offset: 0x004B7513
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!this.ObeliskComp.StudyFinished || this.ObeliskComp.Activated || this.ObeliskComp.ActivityComp.Deactivated)
			{
				yield break;
			}
			foreach (FloatMenuOption floatMenuOption in base.CompFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060105E2 RID: 67042 RVA: 0x004B932A File Offset: 0x004B752A
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			if (base.ValidateTarget(target, false))
			{
				this.OrderDeactivation(target.Pawn);
			}
		}

		// Token: 0x060105E3 RID: 67043 RVA: 0x004B9344 File Offset: 0x004B7544
		private void OrderDeactivation(Pawn pawn)
		{
			List<Thing> list = HaulAIUtility.FindFixedIngredientCount(pawn, ThingDefOf.Shard, this.Props.shardsRequired);
			if (list.NullOrEmpty<Thing>())
			{
				return;
			}
			Job job = JobMaker.MakeJob(JobDefOf.InteractThing, this.parent, list[0]);
			job.targetQueueB = (from i in list.Skip(1)
			select new LocalTargetInfo(i)).ToList<LocalTargetInfo>();
			job.count = this.Props.shardsRequired;
			job.playerForced = true;
			job.interactableIndex = 1;
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x060105E4 RID: 67044 RVA: 0x004B93FC File Offset: 0x004B75FC
		protected override void OnInteracted(Pawn caster)
		{
			if (this.obeliskComp.Activated)
			{
				return;
			}
			this.obeliskComp.ActivityComp.Deactivate();
			CompObeliskTriggerInteractor comp = this.parent.GetComp<CompObeliskTriggerInteractor>();
			if (comp == null)
			{
				return;
			}
			comp.ResetCooldown(true);
		}

		// Token: 0x0400AAC2 RID: 43714
		private CompObelisk obeliskComp;
	}
}
