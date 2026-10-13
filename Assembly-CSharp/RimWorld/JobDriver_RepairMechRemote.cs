using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012FB RID: 4859
	public class JobDriver_RepairMechRemote : JobDriver_RepairMech
	{
		// Token: 0x170012BE RID: 4798
		// (get) Token: 0x06007735 RID: 30517 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Remote
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06007736 RID: 30518 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06007737 RID: 30519 RVA: 0x00243CCF File Offset: 0x00241ECF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => base.Mech.Position.DistanceTo(this.pawn.Position) > this.pawn.GetStatValue(StatDefOf.MechRemoteRepairDistance, true, -1));
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}
	}
}
