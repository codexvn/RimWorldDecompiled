using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002549 RID: 9545
	public class Thought_Situational : Thought
	{
		// Token: 0x1700202F RID: 8239
		// (get) Token: 0x0600D0E1 RID: 53473 RVA: 0x003D6638 File Offset: 0x003D4838
		public bool Active
		{
			get
			{
				return this.curStageIndex >= 0;
			}
		}

		// Token: 0x17002030 RID: 8240
		// (get) Token: 0x0600D0E2 RID: 53474 RVA: 0x003D6646 File Offset: 0x003D4846
		public override int CurStageIndex
		{
			get
			{
				return this.curStageIndex;
			}
		}

		// Token: 0x17002031 RID: 8241
		// (get) Token: 0x0600D0E3 RID: 53475 RVA: 0x003D6650 File Offset: 0x003D4850
		public override string LabelCap
		{
			get
			{
				if (!this.reason.NullOrEmpty())
				{
					string text = base.CurStage.label.Formatted(this.reason.Named("REASON"), this.pawn.Named("PAWN")).CapitalizeFirst();
					if (this.def.Worker != null)
					{
						text = this.def.Worker.PostProcessLabel(this.pawn, text);
					}
					return text;
				}
				return base.LabelCap;
			}
		}

		// Token: 0x0600D0E4 RID: 53476 RVA: 0x003D66D8 File Offset: 0x003D48D8
		public void RecalculateState()
		{
			bool active = this.Active;
			ThoughtState thoughtState = this.CurrentStateInternal();
			if (thoughtState.ActiveFor(this.def))
			{
				this.curStageIndex = thoughtState.StageIndexFor(this.def);
				this.reason = thoughtState.Reason;
			}
			else
			{
				this.curStageIndex = -1;
			}
			bool active2 = this.Active;
			if (active && !active2)
			{
				this.Notify_BecameInactive();
			}
			if (!active && active2)
			{
				this.Notify_BecameActive();
			}
		}

		// Token: 0x0600D0E5 RID: 53477 RVA: 0x003D674C File Offset: 0x003D494C
		protected virtual void Notify_BecameActive()
		{
			if (this.def.producesMemoryThought != null)
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs == null)
				{
					return;
				}
				Need_Mood mood = needs.mood;
				if (mood == null)
				{
					return;
				}
				mood.thoughts.memories.RemoveMemoriesOfDef(this.def.producesMemoryThought);
			}
		}

		// Token: 0x0600D0E6 RID: 53478 RVA: 0x003D679C File Offset: 0x003D499C
		protected virtual void Notify_BecameInactive()
		{
			if (this.def.producesMemoryThought != null)
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs == null)
				{
					return;
				}
				Need_Mood mood = needs.mood;
				if (mood == null)
				{
					return;
				}
				mood.thoughts.memories.TryGainMemory(this.def.producesMemoryThought, null, this.sourcePrecept);
			}
		}

		// Token: 0x0600D0E7 RID: 53479 RVA: 0x003D67F1 File Offset: 0x003D49F1
		protected virtual ThoughtState CurrentStateInternal()
		{
			return this.def.Worker.CurrentState(this.pawn);
		}

		// Token: 0x04008DEB RID: 36331
		private int curStageIndex = -1;

		// Token: 0x04008DEC RID: 36332
		protected string reason;
	}
}
