using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011B2 RID: 4530
	public class JobDriver_PrepareSkylantern : JobDriver_GotoAndStandSociallyActive
	{
		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x06006ECC RID: 28364 RVA: 0x00228810 File Offset: 0x00226A10
		public override Toil StandToil
		{
			get
			{
				if (!ModLister.CheckIdeology("Skylantern job"))
				{
					return null;
				}
				Toil toil = base.StandToil.WithEffect(EffecterDefOf.MakingSkylantern, () => this.pawn.Position + this.pawn.Rotation.FacingCell, null);
				toil.AddPreInitAction(delegate
				{
					Thing thing = this.pawn.inventory.innerContainer.FirstOrDefault((Thing t) => t.def == this.job.thingDefToCarry && t.stackCount >= this.job.count);
					if (thing != null)
					{
						this.pawn.carryTracker.TryStartCarry(thing, this.job.count, true);
					}
				});
				return toil;
			}
		}
	}
}
