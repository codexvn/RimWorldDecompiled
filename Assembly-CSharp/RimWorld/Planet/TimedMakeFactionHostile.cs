using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E36 RID: 15926
	public class TimedMakeFactionHostile : WorldObjectComp
	{
		// Token: 0x1700365A RID: 13914
		// (get) Token: 0x06015C2D RID: 89133 RVA: 0x006811A4 File Offset: 0x0067F3A4
		public int? TicksLeft
		{
			get
			{
				if (this.ticksLeftMakeFactionHostile != -1)
				{
					return new int?(this.ticksLeftMakeFactionHostile);
				}
				return null;
			}
		}

		// Token: 0x06015C2E RID: 89134 RVA: 0x006811CF File Offset: 0x0067F3CF
		public void SetupTimer(int ticks, string message = null, HistoryEventDef reason = null)
		{
			this.timerMakeFactionHostile = ticks;
			this.ticksLeftMakeFactionHostile = -1;
			this.messageBecameHostile = message;
			this.reasonBecameHostile = reason;
		}

		// Token: 0x06015C2F RID: 89135 RVA: 0x006811ED File Offset: 0x0067F3ED
		public override void PostMyMapRemoved()
		{
			this.ticksLeftMakeFactionHostile = -1;
		}

		// Token: 0x06015C30 RID: 89136 RVA: 0x006811F6 File Offset: 0x0067F3F6
		public override void PostMapGenerate()
		{
			this.ticksLeftMakeFactionHostile = this.timerMakeFactionHostile;
		}

		// Token: 0x06015C31 RID: 89137 RVA: 0x00681204 File Offset: 0x0067F404
		public override void CompTickInterval(int delta)
		{
			if (this.ticksLeftMakeFactionHostile != -1 && this.parent.Faction.HostileTo(Faction.OfPlayer))
			{
				this.ticksLeftMakeFactionHostile = -1;
			}
			if (this.ticksLeftMakeFactionHostile != -1)
			{
				this.ticksLeftMakeFactionHostile = Mathf.Max(this.ticksLeftMakeFactionHostile - delta, 0);
				if (this.ticksLeftMakeFactionHostile == 0)
				{
					if (this.parent.Faction.temporary)
					{
						this.parent.Faction.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
					}
					else
					{
						this.parent.Faction.TryAffectGoodwillWith(Faction.OfPlayer, this.parent.Faction.GoodwillToMakeHostile(Faction.OfPlayer), true, true, this.reasonBecameHostile, null);
					}
					if (this.messageBecameHostile != null)
					{
						Messages.Message(this.messageBecameHostile, MessageTypeDefOf.NegativeEvent, true);
					}
					this.ticksLeftMakeFactionHostile = -1;
				}
			}
		}

		// Token: 0x06015C32 RID: 89138 RVA: 0x006812F4 File Offset: 0x0067F4F4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timerMakeFactionHostile, "timerMakeFactionHostile", -1, false);
			Scribe_Values.Look<int>(ref this.ticksLeftMakeFactionHostile, "ticksLeftMakeFactionHostile", -1, false);
			Scribe_Values.Look<string>(ref this.messageBecameHostile, "messageBecameHostile", null, false);
			Scribe_Defs.Look<HistoryEventDef>(ref this.reasonBecameHostile, "reasonBecameHostile");
		}

		// Token: 0x0400F164 RID: 61796
		private int timerMakeFactionHostile = -1;

		// Token: 0x0400F165 RID: 61797
		private int ticksLeftMakeFactionHostile = -1;

		// Token: 0x0400F166 RID: 61798
		private string messageBecameHostile;

		// Token: 0x0400F167 RID: 61799
		public HistoryEventDef reasonBecameHostile;
	}
}
