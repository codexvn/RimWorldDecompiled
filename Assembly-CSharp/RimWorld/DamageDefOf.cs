using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003591 RID: 13713
	[DefOf]
	public static class DamageDefOf
	{
		// Token: 0x0601337F RID: 78719 RVA: 0x005B59FA File Offset: 0x005B3BFA
		static DamageDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(DamageDefOf));
		}

		// Token: 0x0400CD20 RID: 52512
		public static DamageDef Cut;

		// Token: 0x0400CD21 RID: 52513
		public static DamageDef Crush;

		// Token: 0x0400CD22 RID: 52514
		public static DamageDef Blunt;

		// Token: 0x0400CD23 RID: 52515
		public static DamageDef Stab;

		// Token: 0x0400CD24 RID: 52516
		public static DamageDef Bullet;

		// Token: 0x0400CD25 RID: 52517
		public static DamageDef Bomb;

		// Token: 0x0400CD26 RID: 52518
		public static DamageDef Scratch;

		// Token: 0x0400CD27 RID: 52519
		public static DamageDef ScratchToxic;

		// Token: 0x0400CD28 RID: 52520
		public static DamageDef TornadoScratch;

		// Token: 0x0400CD29 RID: 52521
		public static DamageDef Bite;

		// Token: 0x0400CD2A RID: 52522
		public static DamageDef Flame;

		// Token: 0x0400CD2B RID: 52523
		public static DamageDef Burn;

		// Token: 0x0400CD2C RID: 52524
		public static DamageDef AcidBurn;

		// Token: 0x0400CD2D RID: 52525
		public static DamageDef Vaporize;

		// Token: 0x0400CD2E RID: 52526
		[MayRequireAnomaly]
		public static DamageDef ElectricalBurn;

		// Token: 0x0400CD2F RID: 52527
		[MayRequireAnomaly]
		public static DamageDef Psychic;

		// Token: 0x0400CD30 RID: 52528
		public static DamageDef SurgicalCut;

		// Token: 0x0400CD31 RID: 52529
		public static DamageDef ExecutionCut;

		// Token: 0x0400CD32 RID: 52530
		public static DamageDef Frostbite;

		// Token: 0x0400CD33 RID: 52531
		[MayRequireOdyssey]
		public static DamageDef VacuumBurn;

		// Token: 0x0400CD34 RID: 52532
		public static DamageDef Stun;

		// Token: 0x0400CD35 RID: 52533
		public static DamageDef EMP;

		// Token: 0x0400CD36 RID: 52534
		public static DamageDef NerveStun;

		// Token: 0x0400CD37 RID: 52535
		[MayRequireBiotech]
		public static DamageDef MechBandShockwave;

		// Token: 0x0400CD38 RID: 52536
		public static DamageDef Extinguish;

		// Token: 0x0400CD39 RID: 52537
		public static DamageDef Smoke;

		// Token: 0x0400CD3A RID: 52538
		public static DamageDef Deterioration;

		// Token: 0x0400CD3B RID: 52539
		public static DamageDef Mining;

		// Token: 0x0400CD3C RID: 52540
		public static DamageDef Rotting;

		// Token: 0x0400CD3D RID: 52541
		[MayRequireBiotech]
		public static DamageDef ToxGas;
	}
}
