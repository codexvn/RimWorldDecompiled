using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200297F RID: 10623
	public class Building_TrapDamager : Building_Trap
	{
		// Token: 0x0600E963 RID: 59747 RVA: 0x0043EAFD File Offset: 0x0043CCFD
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad && !base.BeingTransportedOnGravship)
			{
				SoundDefOf.TrapArm.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
		}

		// Token: 0x0600E964 RID: 59748 RVA: 0x0043EB30 File Offset: 0x0043CD30
		protected override void SpringSub(Pawn p)
		{
			if (base.Spawned)
			{
				SoundDefOf.TrapSpring.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			if (p == null)
			{
				return;
			}
			float num = this.GetStatValue(StatDefOf.TrapMeleeDamage, true, -1) * Building_TrapDamager.DamageRandomFactorRange.RandomInRange;
			float armorPenetration = num * 0.015f;
			int num2 = 0;
			while ((float)num2 < 5f)
			{
				DamageInfo dinfo = new DamageInfo(DamageDefOf.Stab, num, armorPenetration, -1f, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				DamageWorker.DamageResult damageResult = p.TakeDamage(dinfo);
				if (num2 == 0)
				{
					BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(p, RulePackDefOf.DamageEvent_TrapSpike, null);
					Find.BattleLog.Add(battleLogEntry_DamageTaken);
					damageResult.AssociateWithLog(battleLogEntry_DamageTaken);
				}
				num2++;
			}
		}

		// Token: 0x04009B61 RID: 39777
		private static readonly FloatRange DamageRandomFactorRange = new FloatRange(0.8f, 1.2f);

		// Token: 0x04009B62 RID: 39778
		public const float DamageCount = 5f;
	}
}
