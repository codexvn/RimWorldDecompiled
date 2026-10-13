using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x02002380 RID: 9088
	public class PawnGroupMakerParms
	{
		// Token: 0x0600C4FD RID: 50429 RVA: 0x00394DF8 File Offset: 0x00392FF8
		public override string ToString()
		{
			string[] array = new string[26];
			array[0] = "groupKind=";
			int num = 1;
			PawnGroupKindDef pawnGroupKindDef = this.groupKind;
			array[num] = ((pawnGroupKindDef != null) ? pawnGroupKindDef.ToString() : null);
			array[2] = ", tile=";
			array[3] = this.tile.ToString();
			array[4] = ", inhabitants=";
			array[5] = this.inhabitants.ToString();
			array[6] = ", points=";
			array[7] = this.points.ToString();
			array[8] = ", faction=";
			int num2 = 9;
			Faction faction = this.faction;
			array[num2] = ((faction != null) ? faction.ToString() : null);
			array[10] = ", ideo=";
			int num3 = 11;
			Ideo ideo = this.ideo;
			array[num3] = ((ideo != null) ? ideo.name : null);
			array[12] = ", traderKind=";
			int num4 = 13;
			TraderKindDef traderKindDef = this.traderKind;
			array[num4] = ((traderKindDef != null) ? traderKindDef.ToString() : null);
			array[14] = ", generateFightersOnly=";
			array[15] = this.generateFightersOnly.ToString();
			array[16] = ", dontUseSingleUseRocketLaunchers=";
			array[17] = this.dontUseSingleUseRocketLaunchers.ToString();
			array[18] = ", raidStrategy=";
			int num5 = 19;
			RaidStrategyDef raidStrategyDef = this.raidStrategy;
			array[num5] = ((raidStrategyDef != null) ? raidStrategyDef.ToString() : null);
			array[20] = ", forceOneDowned=";
			array[21] = this.forceOneDowned.ToString();
			array[22] = ", seed=";
			int num6 = 23;
			int? num7 = this.seed;
			array[num6] = num7.ToString();
			array[24] = ", raidAgeRestriction=";
			int num8 = 25;
			RaidAgeRestrictionDef raidAgeRestrictionDef = this.raidAgeRestriction;
			array[num8] = ((raidAgeRestrictionDef != null) ? raidAgeRestrictionDef.ToString() : null);
			return string.Concat(array);
		}

		// Token: 0x04008660 RID: 34400
		public PawnGroupKindDef groupKind;

		// Token: 0x04008661 RID: 34401
		public PlanetTile tile = PlanetTile.Invalid;

		// Token: 0x04008662 RID: 34402
		public bool inhabitants;

		// Token: 0x04008663 RID: 34403
		public float points;

		// Token: 0x04008664 RID: 34404
		public Faction faction;

		// Token: 0x04008665 RID: 34405
		public Ideo ideo;

		// Token: 0x04008666 RID: 34406
		public TraderKindDef traderKind;

		// Token: 0x04008667 RID: 34407
		public bool generateFightersOnly;

		// Token: 0x04008668 RID: 34408
		public bool dontUseSingleUseRocketLaunchers;

		// Token: 0x04008669 RID: 34409
		public RaidStrategyDef raidStrategy;

		// Token: 0x0400866A RID: 34410
		public bool forceOneDowned;

		// Token: 0x0400866B RID: 34411
		public int? seed;

		// Token: 0x0400866C RID: 34412
		public RaidAgeRestrictionDef raidAgeRestriction;

		// Token: 0x0400866D RID: 34413
		public bool ignoreGroupCommonality;
	}
}
