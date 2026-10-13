using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017DF RID: 6111
	public abstract class ThoughtWorker
	{
		// Token: 0x06008E23 RID: 36387 RVA: 0x002A23DE File Offset: 0x002A05DE
		public virtual string PostProcessLabel(Pawn p, string label)
		{
			return label.Formatted(p.Named("PAWN"));
		}

		// Token: 0x06008E24 RID: 36388 RVA: 0x002A23DE File Offset: 0x002A05DE
		public virtual string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(p.Named("PAWN"));
		}

		// Token: 0x06008E25 RID: 36389 RVA: 0x002A23F8 File Offset: 0x002A05F8
		private ThoughtState? FailState(Pawn p)
		{
			if (!ThoughtUtility.CanGetThought(p, this.def, false))
			{
				return new ThoughtState?(ThoughtState.Inactive);
			}
			return null;
		}

		// Token: 0x06008E26 RID: 36390 RVA: 0x002A2428 File Offset: 0x002A0628
		public ThoughtState CurrentState(Pawn p)
		{
			return this.PostProcessedState(this.FailState(p) ?? this.CurrentStateInternal(p));
		}

		// Token: 0x06008E27 RID: 36391 RVA: 0x002A245C File Offset: 0x002A065C
		public ThoughtState CurrentSocialState(Pawn p, Pawn otherPawn)
		{
			return this.PostProcessedState(this.FailState(p) ?? this.CurrentSocialStateInternal(p, otherPawn));
		}

		// Token: 0x06008E28 RID: 36392 RVA: 0x002A2491 File Offset: 0x002A0691
		private ThoughtState PostProcessedState(ThoughtState state)
		{
			if (this.def.invert)
			{
				if (state.Active)
				{
					state = ThoughtState.Inactive;
				}
				else
				{
					state = ThoughtState.ActiveAtStage(0);
				}
			}
			return state;
		}

		// Token: 0x06008E29 RID: 36393 RVA: 0x002A24BB File Offset: 0x002A06BB
		protected virtual ThoughtState CurrentStateInternal(Pawn p)
		{
			throw new NotImplementedException(this.def.defName + " (normal)");
		}

		// Token: 0x06008E2A RID: 36394 RVA: 0x002A24D7 File Offset: 0x002A06D7
		protected virtual ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			throw new NotImplementedException(this.def.defName + " (social)");
		}

		// Token: 0x06008E2B RID: 36395 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public virtual float MoodMultiplier(Pawn p)
		{
			return 1f;
		}

		// Token: 0x04005BF0 RID: 23536
		public ThoughtDef def;
	}
}
