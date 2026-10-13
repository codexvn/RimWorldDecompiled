using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200124D RID: 4685
	public class JobDriver_DisassembleMech : JobDriver
	{
		// Token: 0x170011DC RID: 4572
		// (get) Token: 0x060072A0 RID: 29344 RVA: 0x002352F8 File Offset: 0x002334F8
		private Pawn Mech
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x060072A1 RID: 29345 RVA: 0x0023531E File Offset: 0x0023351E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Mech, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x060072A2 RID: 29346 RVA: 0x00235341 File Offset: 0x00233541
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckBiotech("Disassemble mech"))
			{
				yield break;
			}
			this.FailOnDestroyedNullOrForbidden(TargetIndex.A);
			this.FailOn(() => this.Mech.IsFighting());
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false);
			yield return Toils_General.WaitWith(TargetIndex.A, 300, true, true, false, TargetIndex.A, PathEndMode.Touch).WithEffect(EffecterDefOf.ControlMech, TargetIndex.A, null);
			Toil toil = Toils_General.Do(delegate
			{
				foreach (ThingDefCountClass thingDefCountClass in MechanitorUtility.IngredientsFromDisassembly(this.Mech.def))
				{
					Thing thing = ThingMaker.MakeThing(thingDefCountClass.thingDef, null);
					thing.stackCount = thingDefCountClass.count;
					GenPlace.TryPlaceThing(thing, this.Mech.Position, this.Mech.Map, ThingPlaceMode.Near, null, null, null, 1);
				}
				this.Mech.forceNoDeathNotification = true;
				this.Mech.Kill(null, null);
				this.Mech.forceNoDeathNotification = false;
				this.Mech.Corpse.Destroy(DestroyMode.Vanish);
			}).WithEffect(EffecterDefOf.ButcherMechanoid, TargetIndex.A, null).PlaySustainerOrSound(SoundDefOf.Recipe_ButcherCorpseMechanoid, 1f);
			yield return toil;
			yield break;
		}

		// Token: 0x04004F31 RID: 20273
		private const TargetIndex MechInd = TargetIndex.A;

		// Token: 0x04004F32 RID: 20274
		private const int DisassemblyTime = 300;
	}
}
