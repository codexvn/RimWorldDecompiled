using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002D06 RID: 11526
	public class CompLabyrinthDoor : CompInteractable, IJammedDoorDrawer
	{
		// Token: 0x17002912 RID: 10514
		// (get) Token: 0x06010318 RID: 66328 RVA: 0x004AED02 File Offset: 0x004ACF02
		public Building_JammedDoor Door
		{
			get
			{
				return (Building_JammedDoor)this.parent;
			}
		}

		// Token: 0x17002913 RID: 10515
		// (get) Token: 0x06010319 RID: 66329 RVA: 0x004AED0F File Offset: 0x004ACF0F
		public new CompProperties_LabyrinthDoor Props
		{
			get
			{
				return (CompProperties_LabyrinthDoor)this.props;
			}
		}

		// Token: 0x0601031A RID: 66330 RVA: 0x004AED1C File Offset: 0x004ACF1C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckAnomaly("Labyrinth door"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && Rand.Chance(this.Props.unlockedChance))
			{
				this.Door.UnlockDoor();
			}
		}

		// Token: 0x0601031B RID: 66331 RVA: 0x004AED69 File Offset: 0x004ACF69
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.OrderActivation(target.Pawn);
		}

		// Token: 0x0601031C RID: 66332 RVA: 0x004AED78 File Offset: 0x004ACF78
		public override string CompInspectStringExtra()
		{
			if (!this.Door.Jammed)
			{
				return null;
			}
			return "GrayDoorJammed".Translate();
		}

		// Token: 0x0601031D RID: 66333 RVA: 0x004AED98 File Offset: 0x004ACF98
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!this.Door.Jammed)
			{
				yield break;
			}
			AcceptanceReport acceptanceReport = this.CanInteract(selPawn, true);
			FloatMenuOption floatMenuOption = new FloatMenuOption(this.Props.jobString.CapitalizeFirst(), delegate()
			{
				this.OrderActivation(selPawn);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			if (!acceptanceReport.Accepted)
			{
				floatMenuOption.Disabled = true;
				floatMenuOption.Label = floatMenuOption.Label + " (" + acceptanceReport.Reason + ")";
			}
			yield return floatMenuOption;
			yield break;
		}

		// Token: 0x0601031E RID: 66334 RVA: 0x004AEDAF File Offset: 0x004ACFAF
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!this.Door.Jammed)
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

		// Token: 0x0601031F RID: 66335 RVA: 0x004AEDBF File Offset: 0x004ACFBF
		protected override void OnInteracted(Pawn caster)
		{
			this.Door.UnlockAndOpenDoor();
			if (caster.IsColonist)
			{
				this.parent.Map.fogGrid.FloodUnfogAdjacent(this.parent.Position, false);
			}
		}

		// Token: 0x06010320 RID: 66336 RVA: 0x004AEDF8 File Offset: 0x004ACFF8
		private void OrderActivation(Pawn pawn)
		{
			Job job = JobMaker.MakeJob(JobDefOf.InteractThing, this.parent);
			job.count = 1;
			job.playerForced = true;
			pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x06010321 RID: 66337 RVA: 0x004AEE40 File Offset: 0x004AD040
		public void DrawJammed(Rot4 rotation)
		{
			Vector3 drawPos = this.Door.DrawPos;
			drawPos.y = AltitudeLayer.DoorMoveable.AltitudeFor();
			this.Props.jammed.Graphic.Draw(drawPos, rotation, this.Door, 0f);
		}
	}
}
