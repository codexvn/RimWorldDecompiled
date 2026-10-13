using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CEA RID: 7402
	public class QuestPart_GiveRoyalFavor : QuestPart
	{
		// Token: 0x1700197C RID: 6524
		// (get) Token: 0x0600A53B RID: 42299 RVA: 0x002F540B File Offset: 0x002F360B
		public override bool RequiresAccepter
		{
			get
			{
				return this.giveToAccepter;
			}
		}

		// Token: 0x1700197D RID: 6525
		// (get) Token: 0x0600A53C RID: 42300 RVA: 0x002F5413 File Offset: 0x002F3613
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction != null)
				{
					yield return this.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A53D RID: 42301 RVA: 0x002F5424 File Offset: 0x002F3624
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				Pawn pawn = this.giveToAccepter ? this.quest.AccepterPawn : this.giveTo;
				if (pawn == null)
				{
					signal.args.TryGetArg<Pawn>("CHOSEN", out pawn);
				}
				if (pawn != null && pawn.royalty != null)
				{
					pawn.royalty.GainFavor(this.faction, this.amount);
				}
			}
		}

		// Token: 0x0600A53E RID: 42302 RVA: 0x002F54A0 File Offset: 0x002F36A0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.giveTo = PawnsFinder.AllMaps_FreeColonists.RandomElement<Pawn>();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			this.amount = 10;
		}

		// Token: 0x0600A53F RID: 42303 RVA: 0x002F54FC File Offset: 0x002F36FC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.giveTo, "giveTo", false);
			Scribe_Values.Look<bool>(ref this.giveToAccepter, "giveToAccepter", false, false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<int>(ref this.amount, "amount", 0, false);
		}

		// Token: 0x0600A540 RID: 42304 RVA: 0x002F5567 File Offset: 0x002F3767
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.giveTo == replace)
			{
				this.giveTo = with;
			}
		}

		// Token: 0x04007228 RID: 29224
		public Pawn giveTo;

		// Token: 0x04007229 RID: 29225
		public bool giveToAccepter;

		// Token: 0x0400722A RID: 29226
		public string inSignal;

		// Token: 0x0400722B RID: 29227
		public int amount;

		// Token: 0x0400722C RID: 29228
		public Faction faction;
	}
}
