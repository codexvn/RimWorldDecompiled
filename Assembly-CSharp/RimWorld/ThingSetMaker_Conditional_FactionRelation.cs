using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B5C RID: 11100
	public class ThingSetMaker_Conditional_FactionRelation : ThingSetMaker_Conditional
	{
		// Token: 0x0600F83E RID: 63550 RVA: 0x004833AC File Offset: 0x004815AC
		protected override bool Condition(ThingSetMakerParams parms)
		{
			Faction faction = Find.FactionManager.FirstFactionOfDef(this.factionDef);
			if (faction == null)
			{
				return false;
			}
			switch (faction.RelationKindWith(Faction.OfPlayer))
			{
			case FactionRelationKind.Hostile:
				return this.allowHostile;
			case FactionRelationKind.Neutral:
				return this.allowNeutral;
			case FactionRelationKind.Ally:
				return this.allowAlly;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0400A338 RID: 41784
		public FactionDef factionDef;

		// Token: 0x0400A339 RID: 41785
		public bool allowHostile;

		// Token: 0x0400A33A RID: 41786
		public bool allowNeutral;

		// Token: 0x0400A33B RID: 41787
		public bool allowAlly;
	}
}
