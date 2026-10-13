using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002985 RID: 10629
	public static class TurretGunUtility
	{
		// Token: 0x0600E9BA RID: 59834 RVA: 0x0044037F File Offset: 0x0043E57F
		public static bool NeedsShells(ThingDef turret)
		{
			return turret.category == ThingCategory.Building && turret.building.IsTurret && turret.building.turretGunDef.HasComp(typeof(CompChangeableProjectile));
		}

		// Token: 0x0600E9BB RID: 59835 RVA: 0x004403B4 File Offset: 0x0043E5B4
		public static ThingDef TryFindRandomShellDef(ThingDef turret, bool allowEMP = true, bool allowToxGas = false, bool mustHarmHealth = true, TechLevel techLevel = TechLevel.Undefined, bool allowAntigrainWarhead = false, float maxMarketValue = -1f, Faction faction = null)
		{
			if (!TurretGunUtility.NeedsShells(turret))
			{
				return null;
			}
			ThingFilter fixedFilter = turret.building.turretGunDef.building.fixedStorageSettings.filter;
			if (ModsConfig.BiotechActive && faction != null && faction.def == FactionDefOf.PirateWaster && fixedFilter.Allows(ThingDefOf.Shell_Toxic) && Rand.Chance(0.5f))
			{
				return ThingDefOf.Shell_Toxic;
			}
			ThingDef result;
			if ((from x in DefDatabase<ThingDef>.AllDefsListForReading
			where fixedFilter.Allows(x) && (allowEMP || x.projectileWhenLoaded.projectile.damageDef != DamageDefOf.EMP) && (allowToxGas || !ModsConfig.BiotechActive || x.projectileWhenLoaded.projectile.damageDef != DamageDefOf.ToxGas) && (!mustHarmHealth || x.projectileWhenLoaded.projectile.damageDef.harmsHealth) && (techLevel == TechLevel.Undefined || x.techLevel <= techLevel) && (allowAntigrainWarhead || x != ThingDefOf.Shell_AntigrainWarhead) && (maxMarketValue < 0f || x.BaseMarketValue <= maxMarketValue)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x04009B82 RID: 39810
		private const float WasterToxicShellChance = 0.5f;
	}
}
