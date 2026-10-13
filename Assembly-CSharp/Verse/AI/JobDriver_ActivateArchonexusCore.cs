using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E4E RID: 3662
	public class JobDriver_ActivateArchonexusCore : JobDriver_Goto
	{
		// Token: 0x06005C41 RID: 23617 RVA: 0x001D7C6F File Offset: 0x001D5E6F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Activate archonexus core"))
			{
				yield break;
			}
			yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.InteractionCell);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				((Building_ArchonexusCore)this.job.targetA.Thing).Activate();
			};
			toil.handlingFacing = true;
			toil.tickIntervalAction = delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(base.TargetA);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 120;
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			yield return toil;
			yield break;
		}

		// Token: 0x040042CD RID: 17101
		private const int DefaultDuration = 120;
	}
}
