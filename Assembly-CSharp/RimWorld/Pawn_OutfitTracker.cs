using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200250C RID: 9484
	public class Pawn_OutfitTracker : IExposable
	{
		// Token: 0x17001FC9 RID: 8137
		// (get) Token: 0x0600CF30 RID: 53040 RVA: 0x003CD214 File Offset: 0x003CB414
		// (set) Token: 0x0600CF31 RID: 53041 RVA: 0x003CD281 File Offset: 0x003CB481
		public ApparelPolicy CurrentApparelPolicy
		{
			get
			{
				if (this.pawn.IsMutant && (this.pawn.mutant.Def.disableApparel || this.pawn.mutant.Def.disablePolicies))
				{
					return null;
				}
				if (this.curApparelPolicy == null)
				{
					this.curApparelPolicy = Current.Game.outfitDatabase.DefaultOutfit();
				}
				return this.curApparelPolicy;
			}
			set
			{
				if (this.curApparelPolicy == value)
				{
					return;
				}
				this.curApparelPolicy = value;
				if (this.pawn.mindState != null)
				{
					this.pawn.mindState.Notify_OutfitChanged();
				}
			}
		}

		// Token: 0x0600CF32 RID: 53042 RVA: 0x003CD2B1 File Offset: 0x003CB4B1
		public Pawn_OutfitTracker()
		{
		}

		// Token: 0x0600CF33 RID: 53043 RVA: 0x003CD2C4 File Offset: 0x003CB4C4
		public Pawn_OutfitTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CF34 RID: 53044 RVA: 0x003CD2DE File Offset: 0x003CB4DE
		public void ExposeData()
		{
			Scribe_References.Look<ApparelPolicy>(ref this.curApparelPolicy, "curOutfit", false);
			Scribe_Deep.Look<OutfitForcedHandler>(ref this.forcedHandler, "overrideHandler", Array.Empty<object>());
		}

		// Token: 0x04008CF4 RID: 36084
		public Pawn pawn;

		// Token: 0x04008CF5 RID: 36085
		private ApparelPolicy curApparelPolicy;

		// Token: 0x04008CF6 RID: 36086
		public OutfitForcedHandler forcedHandler = new OutfitForcedHandler();
	}
}
