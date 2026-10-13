using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001816 RID: 6166
	public class ThoughtWorker_VeneratedAnimalOnMapOrCaravan : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06008EE1 RID: 36577 RVA: 0x002A4548 File Offset: 0x002A2748
		public override string PostProcessLabel(Pawn p, string label)
		{
			Pawn pawn = PawnUtility.FirstVeneratedAnimalOnMapOrCaravan(p);
			if (pawn == null)
			{
				return label;
			}
			return label.Formatted(pawn.kindDef.Named("ANIMAL"));
		}

		// Token: 0x06008EE2 RID: 36578 RVA: 0x002A457C File Offset: 0x002A277C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.IsSlave)
			{
				return false;
			}
			int num = this.ThoughtStageFromAnimalDensity(PawnUtility.PlayerVeneratedAnimalBodySizePerCapitaOnMapOrCaravan(p));
			if (num >= 0)
			{
				return ThoughtState.ActiveAtStage(num);
			}
			return false;
		}

		// Token: 0x06008EE3 RID: 36579 RVA: 0x002A45B6 File Offset: 0x002A27B6
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return ("(" + "VeneratedAnimalsBodySizePerColonist".Translate() + ": " + 1f.ToString() + ")").Named("STAGE1");
			yield return ("(" + "VeneratedAnimalsBodySizePerColonist".Translate() + ": " + 2f.ToString() + ")").Named("STAGE2");
			yield return ("(" + "VeneratedAnimalsBodySizePerColonist".Translate() + ": " + 4f.ToString() + ")").Named("STAGE3");
			yield return ("(" + "VeneratedAnimalsBodySizePerColonist".Translate() + ": " + 6f.ToString() + ")").Named("STAGE4");
			yield break;
		}

		// Token: 0x06008EE4 RID: 36580 RVA: 0x002A45BF File Offset: 0x002A27BF
		private int ThoughtStageFromAnimalDensity(float density)
		{
			if (density <= 0f)
			{
				return -1;
			}
			if (density < 1f)
			{
				return 0;
			}
			if (density < 2f)
			{
				return 1;
			}
			if (density < 4f)
			{
				return 2;
			}
			if (density < 6f)
			{
				return 3;
			}
			return 4;
		}

		// Token: 0x04005C1A RID: 23578
		private const float FewAnimals = 1f;

		// Token: 0x04005C1B RID: 23579
		private const float SomeAnimals = 2f;

		// Token: 0x04005C1C RID: 23580
		private const float ManyAnimals = 4f;

		// Token: 0x04005C1D RID: 23581
		private const float LotsOfAnimals = 6f;
	}
}
