using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC7 RID: 7111
	public class QuestPart_FactionRelationKind : QuestPartActivable
	{
		// Token: 0x1700184F RID: 6223
		// (get) Token: 0x06009E50 RID: 40528 RVA: 0x002DCD0B File Offset: 0x002DAF0B
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction1 != null)
				{
					yield return this.faction1;
				}
				if (this.faction2 != null)
				{
					yield return this.faction2;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06009E51 RID: 40529 RVA: 0x002DCD1C File Offset: 0x002DAF1C
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.faction1 != null && this.faction2 != null && this.faction1.RelationKindWith(this.faction2) == this.relationKind)
			{
				base.Complete(this.faction1.Named("SUBJECT"));
			}
		}

		// Token: 0x06009E52 RID: 40530 RVA: 0x002DCD6E File Offset: 0x002DAF6E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction1, "faction1", false);
			Scribe_References.Look<Faction>(ref this.faction2, "faction2", false);
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
		}

		// Token: 0x06009E53 RID: 40531 RVA: 0x002DCDAA File Offset: 0x002DAFAA
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.faction1 = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			this.faction2 = Faction.OfPlayer;
			this.relationKind = FactionRelationKind.Neutral;
		}

		// Token: 0x04006D99 RID: 28057
		public Faction faction1;

		// Token: 0x04006D9A RID: 28058
		public Faction faction2;

		// Token: 0x04006D9B RID: 28059
		public FactionRelationKind relationKind;
	}
}
