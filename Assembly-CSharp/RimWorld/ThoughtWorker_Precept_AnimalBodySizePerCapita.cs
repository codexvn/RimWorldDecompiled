using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200180A RID: 6154
	public class ThoughtWorker_Precept_AnimalBodySizePerCapita : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06008EA6 RID: 36518 RVA: 0x002A3A14 File Offset: 0x002A1C14
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (ThoughtUtility.ThoughtNullified(p, this.def))
			{
				return false;
			}
			if (p.IsSlave)
			{
				return false;
			}
			float num = PawnUtility.PlayerAnimalBodySizePerCapita();
			if (num <= 2f && GenTicks.TicksAbs < 900000)
			{
				return false;
			}
			if (num < 4f && this.def.minExpectationForNegativeThought != null && p.MapHeld != null && ExpectationsUtility.CurrentExpectationFor(p.MapHeld).order < this.def.minExpectationForNegativeThought.order)
			{
				return false;
			}
			if (this.ThoughtStageFromAnimalDensity(num) < 0)
			{
				return false;
			}
			return ThoughtState.ActiveAtStage(this.ThoughtStageFromAnimalDensity(PawnUtility.PlayerAnimalBodySizePerCapita()));
		}

		// Token: 0x06008EA7 RID: 36519 RVA: 0x002A3AD0 File Offset: 0x002A1CD0
		public override string PostProcessDescription(Pawn p, string description)
		{
			return base.PostProcessDescription(p, description) + "\n\n" + "CurrentTotalAnimalBodySizePerColonist".Translate() + ": " + PawnUtility.PlayerAnimalBodySizePerCapita().ToString("F1") + "\n" + "MinAnimalBodySizePerColonist".Translate(4f.ToString("F1"));
		}

		// Token: 0x06008EA8 RID: 36520 RVA: 0x002A3B54 File Offset: 0x002A1D54
		private int ThoughtStageFromAnimalDensity(float density)
		{
			if (density < 0f)
			{
				return 0;
			}
			if (density < 1f)
			{
				return 1;
			}
			if (density < 2f)
			{
				return 2;
			}
			if (density < 4f)
			{
				return -1;
			}
			if (density < 6f)
			{
				return 3;
			}
			if (density < 8f)
			{
				return 4;
			}
			return 5;
		}

		// Token: 0x06008EA9 RID: 36521 RVA: 0x002A3B93 File Offset: 0x002A1D93
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return ("(" + "AnimalsBodySizePerColonist".Translate() + ": " + 1f.ToString() + ")").Named("STAGE1");
			yield return ("(" + "AnimalsBodySizePerColonist".Translate() + ": " + 2f.ToString() + ")").Named("STAGE2");
			yield return ("(" + "AnimalsBodySizePerColonist".Translate() + ": " + 6f.ToString() + ")").Named("STAGE4");
			yield return ("(" + "AnimalsBodySizePerColonist".Translate() + ": " + 8f.ToString() + ")").Named("STAGE5");
			yield break;
		}

		// Token: 0x04005C03 RID: 23555
		private const float NoAnimals = 0f;

		// Token: 0x04005C04 RID: 23556
		private const float ScarceAnimals = 1f;

		// Token: 0x04005C05 RID: 23557
		private const float FewAnimals = 2f;

		// Token: 0x04005C06 RID: 23558
		private const float NoThought = 4f;

		// Token: 0x04005C07 RID: 23559
		private const float SomeAnimals = 6f;

		// Token: 0x04005C08 RID: 23560
		private const float LotsOfAnimals = 8f;

		// Token: 0x04005C09 RID: 23561
		private const int MinimumTicksBeforeFewAnimals = 900000;
	}
}
