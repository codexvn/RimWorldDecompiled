using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200117B RID: 4475
	public class JobDriver_RemoveRoof : JobDriver_AffectRoof
	{
		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x06006D79 RID: 28025 RVA: 0x00075162 File Offset: 0x00073362
		protected override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x06006D7A RID: 28026 RVA: 0x00224533 File Offset: 0x00222733
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => !base.Map.areaManager.NoRoof[base.Cell]);
			foreach (Toil toil in base.MakeNewToils())
			{
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006D7B RID: 28027 RVA: 0x00224544 File Offset: 0x00222744
		protected override void DoEffect()
		{
			JobDriver_RemoveRoof.removedRoofs.Clear();
			base.Map.roofGrid.SetRoof(base.Cell, null);
			JobDriver_RemoveRoof.removedRoofs.Add(base.Cell);
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(JobDriver_RemoveRoof.removedRoofs, base.Map, true, false);
			JobDriver_RemoveRoof.removedRoofs.Clear();
		}

		// Token: 0x06006D7C RID: 28028 RVA: 0x0022459E File Offset: 0x0022279E
		protected override bool DoWorkFailOn()
		{
			return !base.Cell.Roofed(base.Map);
		}

		// Token: 0x04004CA2 RID: 19618
		private static List<IntVec3> removedRoofs = new List<IntVec3>();
	}
}
