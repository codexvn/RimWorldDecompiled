using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C0C RID: 7180
	public class QuestPart_AddQuestRefugeeDelayedReward : QuestPart_AddQuest
	{
		// Token: 0x17001893 RID: 6291
		// (get) Token: 0x06009FEB RID: 40939 RVA: 0x002E1E75 File Offset: 0x002E0075
		public override QuestScriptDef QuestDef
		{
			get
			{
				return QuestScriptDefOf.RefugeeDelayedReward;
			}
		}

		// Token: 0x06009FEC RID: 40940 RVA: 0x002E1E7C File Offset: 0x002E007C
		public override Slate GetSlate()
		{
			Slate slate = new Slate();
			slate.Set<FloatRange>("marketValueRange", this.marketValueRange, false);
			slate.Set<Faction>("faction", this.faction, false);
			for (int i = 0; i < this.lodgers.Count; i++)
			{
				if (!this.lodgers[i].Dead && this.lodgers[i].Faction != Faction.OfPlayer && !this.lodgers[i].IsPrisoner)
				{
					slate.Set<Pawn>("rewardGiver", this.lodgers[i], false);
					break;
				}
			}
			return slate;
		}

		// Token: 0x06009FED RID: 40941 RVA: 0x002E1F22 File Offset: 0x002E0122
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06009FEE RID: 40942 RVA: 0x002E1F34 File Offset: 0x002E0134
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.lodgers.Contains(item))
			{
				this.lodgers.Remove(item);
			}
		}

		// Token: 0x06009FEF RID: 40943 RVA: 0x002E1F8C File Offset: 0x002E018C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Collections.Look<Pawn>(ref this.lodgers, "lodgers", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			Scribe_Values.Look<FloatRange>(ref this.marketValueRange, "marketValueRange", default(FloatRange), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lodgers.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04006E97 RID: 28311
		public List<Pawn> lodgers = new List<Pawn>();

		// Token: 0x04006E98 RID: 28312
		public FloatRange marketValueRange;

		// Token: 0x04006E99 RID: 28313
		public Faction faction;

		// Token: 0x04006E9A RID: 28314
		public string inSignalRemovePawn;
	}
}
