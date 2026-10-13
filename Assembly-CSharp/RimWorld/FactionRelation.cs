using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002594 RID: 9620
	public class FactionRelation : IExposable
	{
		// Token: 0x0600D2D9 RID: 53977 RVA: 0x003E17F2 File Offset: 0x003DF9F2
		public FactionRelation()
		{
		}

		// Token: 0x0600D2DA RID: 53978 RVA: 0x003E1809 File Offset: 0x003DFA09
		public FactionRelation(Faction other, FactionRelationKind kind)
		{
			this.other = other;
			this.kind = kind;
		}

		// Token: 0x0600D2DB RID: 53979 RVA: 0x003E1830 File Offset: 0x003DFA30
		public void CheckKindThresholds(Faction faction, bool canSendLetter, string reason, GlobalTargetInfo lookTarget, out bool sentLetter)
		{
			int num = faction.GoodwillWith(this.other);
			FactionRelationKind previousKind = this.kind;
			sentLetter = false;
			if (this.kind != FactionRelationKind.Hostile && num <= -75)
			{
				this.kind = FactionRelationKind.Hostile;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind != FactionRelationKind.Ally && num >= 75)
			{
				this.kind = FactionRelationKind.Ally;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind == FactionRelationKind.Hostile && num >= 0)
			{
				this.kind = FactionRelationKind.Neutral;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
			if (this.kind == FactionRelationKind.Ally && num <= 0)
			{
				this.kind = FactionRelationKind.Neutral;
				faction.Notify_RelationKindChanged(this.other, previousKind, canSendLetter, reason, lookTarget, out sentLetter);
			}
		}

		// Token: 0x0600D2DC RID: 53980 RVA: 0x003E18F1 File Offset: 0x003DFAF1
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.other, "other", false);
			Scribe_Values.Look<FactionRelationKind>(ref this.kind, "kind", FactionRelationKind.Neutral, false);
			Scribe_Values.Look<int>(ref this.baseGoodwill, "goodwill", 0, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600D2DD RID: 53981 RVA: 0x003E1930 File Offset: 0x003DFB30
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			Faction faction = this.other;
			array[num] = ((faction != null) ? faction.ToString() : null);
			array[2] = ", kind=";
			array[3] = this.kind.ToString();
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x04008F32 RID: 36658
		public Faction other;

		// Token: 0x04008F33 RID: 36659
		public int baseGoodwill = 100;

		// Token: 0x04008F34 RID: 36660
		public FactionRelationKind kind = FactionRelationKind.Neutral;
	}
}
