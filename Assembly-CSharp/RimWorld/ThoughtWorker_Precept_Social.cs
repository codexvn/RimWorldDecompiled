using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E1 RID: 6113
	public abstract class ThoughtWorker_Precept_Social : ThoughtWorker
	{
		// Token: 0x06008E30 RID: 36400
		protected abstract ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn);

		// Token: 0x06008E31 RID: 36401 RVA: 0x002A2564 File Offset: 0x002A0764
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			Ideo ideo = p.Ideo;
			if (ideo == null)
			{
				return ThoughtState.Inactive;
			}
			if (!ideo.cachedPossibleSituationalThoughts.Contains(this.def))
			{
				return ThoughtState.Inactive;
			}
			if (this.def.gender != Gender.None && otherPawn.gender != this.def.gender)
			{
				return ThoughtState.Inactive;
			}
			return this.ShouldHaveThought(p, otherPawn);
		}

		// Token: 0x06008E32 RID: 36402 RVA: 0x002A25C8 File Offset: 0x002A07C8
		public override string PostProcessLabel(Pawn p, string label)
		{
			return base.PostProcessLabel(p, label) + " (" + "Ideo".Translate() + ")";
		}

		// Token: 0x06008E33 RID: 36403 RVA: 0x002A25FC File Offset: 0x002A07FC
		public override string PostProcessDescription(Pawn p, string description)
		{
			return base.PostProcessDescription(p, description) + "\n\n" + "ComesFromIdeo".Translate() + ": " + p.Ideo.name;
		}
	}
}
