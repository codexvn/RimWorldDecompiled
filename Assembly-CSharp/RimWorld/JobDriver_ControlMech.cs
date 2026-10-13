using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001246 RID: 4678
	public class JobDriver_ControlMech : JobDriver
	{
		// Token: 0x170011D1 RID: 4561
		// (get) Token: 0x0600726D RID: 29293 RVA: 0x00234A00 File Offset: 0x00232C00
		private Pawn Mech
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170011D2 RID: 4562
		// (get) Token: 0x0600726E RID: 29294 RVA: 0x00234A26 File Offset: 0x00232C26
		private int MechControlTime
		{
			get
			{
				return Mathf.RoundToInt(this.Mech.GetStatValue(StatDefOf.ControlTakingTime, true, -1) * 60f);
			}
		}

		// Token: 0x0600726F RID: 29295 RVA: 0x00234A45 File Offset: 0x00232C45
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Mech, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007270 RID: 29296 RVA: 0x00234A68 File Offset: 0x00232C68
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckBiotech("Control mech"))
			{
				yield break;
			}
			this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => !MechanitorUtility.CanControlMech(this.pawn, this.Mech));
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			Toil toil = Toils_General.WaitWith(TargetIndex.A, this.MechControlTime, true, true, false, TargetIndex.A, PathEndMode.Touch).WithEffect(EffecterDefOf.ControlMech, TargetIndex.A, null);
			yield return toil;
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.initAction = delegate()
			{
				Pawn overseer = this.Mech.GetOverseer();
				if (overseer != null)
				{
					overseer.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, this.Mech);
				}
				this.pawn.relations.AddDirectRelation(PawnRelationDefOf.Overseer, this.Mech);
			};
			toil2.PlaySoundAtEnd(SoundDefOf.ControlMech_Complete);
			yield return toil2;
			yield break;
		}

		// Token: 0x04004F1E RID: 20254
		private const TargetIndex MechInd = TargetIndex.A;
	}
}
