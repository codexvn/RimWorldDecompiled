using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003592 RID: 13714
	[DefOf]
	public static class PawnKindDefOf
	{
		// Token: 0x06013380 RID: 78720 RVA: 0x005B5A0B File Offset: 0x005B3C0B
		static PawnKindDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnKindDefOf));
		}

		// Token: 0x0400CD3E RID: 52542
		public static PawnKindDef Colonist;

		// Token: 0x0400CD3F RID: 52543
		public static PawnKindDef Slave;

		// Token: 0x0400CD40 RID: 52544
		public static PawnKindDef Villager;

		// Token: 0x0400CD41 RID: 52545
		public static PawnKindDef Drifter;

		// Token: 0x0400CD42 RID: 52546
		public static PawnKindDef SpaceRefugee;

		// Token: 0x0400CD43 RID: 52547
		public static PawnKindDef AncientSoldier;

		// Token: 0x0400CD44 RID: 52548
		public static PawnKindDef WildMan;

		// Token: 0x0400CD45 RID: 52549
		public static PawnKindDef Pirate;

		// Token: 0x0400CD46 RID: 52550
		public static PawnKindDef PirateBoss;

		// Token: 0x0400CD47 RID: 52551
		[MayRequireBiotech]
		public static PawnKindDef Sanguophage;

		// Token: 0x0400CD48 RID: 52552
		[MayRequireBiotech]
		public static PawnKindDef SanguophageThrall;

		// Token: 0x0400CD49 RID: 52553
		[MayRequireBiotech]
		public static PawnKindDef Mechanitor_Basic;

		// Token: 0x0400CD4A RID: 52554
		[MayRequireOdyssey]
		public static PawnKindDef AncientSoldier_Leader;

		// Token: 0x0400CD4B RID: 52555
		[MayRequireOdyssey]
		public static PawnKindDef Salvager_Elite;

		// Token: 0x0400CD4C RID: 52556
		public static PawnKindDef Thrumbo;

		// Token: 0x0400CD4D RID: 52557
		public static PawnKindDef Alphabeaver;

		// Token: 0x0400CD4E RID: 52558
		public static PawnKindDef Muffalo;

		// Token: 0x0400CD4F RID: 52559
		[MayRequireOdyssey]
		public static PawnKindDef AlphaThrumbo;

		// Token: 0x0400CD50 RID: 52560
		public static PawnKindDef Megascarab;

		// Token: 0x0400CD51 RID: 52561
		public static PawnKindDef Spelopede;

		// Token: 0x0400CD52 RID: 52562
		public static PawnKindDef Megaspider;

		// Token: 0x0400CD53 RID: 52563
		[MayRequireOdyssey]
		public static PawnKindDef Locust;

		// Token: 0x0400CD54 RID: 52564
		[MayRequireOdyssey]
		public static PawnKindDef HiveQueen;

		// Token: 0x0400CD55 RID: 52565
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Royal_Bestower;

		// Token: 0x0400CD56 RID: 52566
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Royal_NobleWimp;

		// Token: 0x0400CD57 RID: 52567
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Fighter_Janissary;

		// Token: 0x0400CD58 RID: 52568
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Fighter_Trooper;

		// Token: 0x0400CD59 RID: 52569
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Fighter_Cataphract;

		// Token: 0x0400CD5A RID: 52570
		[MayRequireRoyalty]
		public static PawnKindDef Empire_Common_Lodger;

		// Token: 0x0400CD5B RID: 52571
		[MayRequireRoyalty]
		public static PawnKindDef Refugee;

		// Token: 0x0400CD5C RID: 52572
		[MayRequireIdeology]
		public static PawnKindDef Beggar;

		// Token: 0x0400CD5D RID: 52573
		[MayRequireIdeology]
		public static PawnKindDef PovertyPilgrim;

		// Token: 0x0400CD5E RID: 52574
		[MayRequireIdeology]
		public static PawnKindDef WellEquippedTraveler;

		// Token: 0x0400CD5F RID: 52575
		[MayRequireIdeology]
		public static PawnKindDef Dryad_Basic;

		// Token: 0x0400CD60 RID: 52576
		[MayRequireIdeology]
		public static PawnKindDef Dryad_Gaumaker;

		// Token: 0x0400CD61 RID: 52577
		public static PawnKindDef Mech_Pikeman;

		// Token: 0x0400CD62 RID: 52578
		public static PawnKindDef Mech_Scyther;

		// Token: 0x0400CD63 RID: 52579
		[MayRequireBiotech]
		public static PawnKindDef Mech_Warqueen;

		// Token: 0x0400CD64 RID: 52580
		[MayRequireAnomaly]
		public static PawnKindDef ShamblerSwarmer;

		// Token: 0x0400CD65 RID: 52581
		[MayRequireAnomaly]
		public static PawnKindDef ShamblerGorehulk;

		// Token: 0x0400CD66 RID: 52582
		[MayRequireAnomaly]
		public static PawnKindDef Revenant;

		// Token: 0x0400CD67 RID: 52583
		[MayRequireAnomaly]
		public static PawnKindDef Sightstealer;

		// Token: 0x0400CD68 RID: 52584
		[MayRequireAnomaly]
		public static PawnKindDef Bulbfreak;

		// Token: 0x0400CD69 RID: 52585
		[MayRequireAnomaly]
		public static PawnKindDef Trispike;

		// Token: 0x0400CD6A RID: 52586
		[MayRequireAnomaly]
		public static PawnKindDef Toughspike;

		// Token: 0x0400CD6B RID: 52587
		[MayRequireAnomaly]
		public static PawnKindDef Fingerspike;

		// Token: 0x0400CD6C RID: 52588
		[MayRequireAnomaly]
		public static PawnKindDef Dreadmeld;

		// Token: 0x0400CD6D RID: 52589
		[MayRequireAnomaly]
		public static PawnKindDef Nociosphere;

		// Token: 0x0400CD6E RID: 52590
		[MayRequireAnomaly]
		public static PawnKindDef Metalhorror;

		// Token: 0x0400CD6F RID: 52591
		[MayRequireAnomaly]
		public static PawnKindDef Ghoul;

		// Token: 0x0400CD70 RID: 52592
		[MayRequireAnomaly]
		public static PawnKindDef FleshmassNucleus;

		// Token: 0x0400CD71 RID: 52593
		[MayRequireAnomaly]
		public static PawnKindDef Chimera;

		// Token: 0x0400CD72 RID: 52594
		[MayRequireOdyssey]
		public static PawnKindDef Drone_Hunter;

		// Token: 0x0400CD73 RID: 52595
		[MayRequireOdyssey]
		public static PawnKindDef Drone_Wasp;

		// Token: 0x0400CD74 RID: 52596
		[MayRequireOdyssey]
		public static PawnKindDef Drone_Sentry;
	}
}
