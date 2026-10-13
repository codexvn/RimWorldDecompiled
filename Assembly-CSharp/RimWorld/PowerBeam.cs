using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AB7 RID: 10935
	public class PowerBeam : OrbitalStrike
	{
		// Token: 0x0600F3AD RID: 62381 RVA: 0x0046CB8B File Offset: 0x0046AD8B
		public override void StartStrike()
		{
			base.StartStrike();
			MoteMaker.MakePowerBeamMote(base.Position, base.Map);
		}

		// Token: 0x0600F3AE RID: 62382 RVA: 0x0046CBA4 File Offset: 0x0046ADA4
		protected override void Tick()
		{
			base.Tick();
			if (base.Destroyed)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				this.StartRandomFireAndDoFlameDamage();
			}
		}

		// Token: 0x0600F3AF RID: 62383 RVA: 0x0046CBD4 File Offset: 0x0046ADD4
		private void StartRandomFireAndDoFlameDamage()
		{
			IntVec3 c = (from x in GenRadial.RadialCellsAround(base.Position, 15f, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => 1f - Mathf.Min(x.DistanceTo(base.Position) / 15f, 1f) + 0.05f);
			FireUtility.TryStartFireIn(c, base.Map, Rand.Range(0.1f, 0.925f), this.instigator, null);
			PowerBeam.tmpThings.Clear();
			PowerBeam.tmpThings.AddRange(c.GetThingList(base.Map));
			for (int i = 0; i < PowerBeam.tmpThings.Count; i++)
			{
				int num = (PowerBeam.tmpThings[i] is Corpse) ? PowerBeam.CorpseFlameDamageAmountRange.RandomInRange : PowerBeam.FlameDamageAmountRange.RandomInRange;
				Pawn pawn = PowerBeam.tmpThings[i] as Pawn;
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
				if (pawn != null)
				{
					battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_PowerBeam, this.instigator as Pawn);
					Find.BattleLog.Add(battleLogEntry_DamageTaken);
				}
				PowerBeam.tmpThings[i].TakeDamage(new DamageInfo(DamageDefOf.Flame, (float)num, 0f, -1f, this.instigator, null, this.weaponDef, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false)).AssociateWithLog(battleLogEntry_DamageTaken);
			}
			PowerBeam.tmpThings.Clear();
		}

		// Token: 0x0400A082 RID: 41090
		public const float Radius = 15f;

		// Token: 0x0400A083 RID: 41091
		private const int FiresStartedPerTick = 4;

		// Token: 0x0400A084 RID: 41092
		private static readonly IntRange FlameDamageAmountRange = new IntRange(65, 100);

		// Token: 0x0400A085 RID: 41093
		private static readonly IntRange CorpseFlameDamageAmountRange = new IntRange(5, 10);

		// Token: 0x0400A086 RID: 41094
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
