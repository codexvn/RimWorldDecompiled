using System;
using System.Collections.Generic;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001139 RID: 4409
	public class JobDriver_Train : JobDriver_InteractAnimal
	{
		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x06006B96 RID: 27542 RVA: 0x0021D8EB File Offset: 0x0021BAEB
		protected override bool CanInteractNow
		{
			get
			{
				return !TrainableUtility.TrainedTooRecently(base.Animal);
			}
		}

		// Token: 0x06006B97 RID: 27543 RVA: 0x0021D8FB File Offset: 0x0021BAFB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Func<bool> noLongerTrainable = () => base.Animal.training.NextTrainableToTrain() == null;
			foreach (Toil toil in base.MakeNewToils())
			{
				toil.FailOn(noLongerTrainable);
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return Toils_Interpersonal.TryTrain(TargetIndex.A);
			yield break;
			yield break;
		}
	}
}
