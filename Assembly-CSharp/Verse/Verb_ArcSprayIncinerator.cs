using System;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D7C RID: 3452
	public class Verb_ArcSprayIncinerator : Verb_ShootBeam
	{
		// Token: 0x06005811 RID: 22545 RVA: 0x001C5BD8 File Offset: 0x001C3DD8
		public override void WarmupComplete()
		{
			this.sprayer = (GenSpawn.Spawn(ThingDefOf.IncineratorSpray, this.caster.Position, this.caster.Map, WipeMode.Vanish) as IncineratorSpray);
			base.WarmupComplete();
			BattleLog battleLog = Find.BattleLog;
			Thing caster = this.caster;
			Thing target = this.currentTarget.HasThing ? this.currentTarget.Thing : null;
			ThingWithComps equipmentSource = base.EquipmentSource;
			battleLog.Add(new BattleLogEntry_RangedFire(caster, target, (equipmentSource != null) ? equipmentSource.def : null, null, false));
		}

		// Token: 0x06005812 RID: 22546 RVA: 0x001C5C5C File Offset: 0x001C3E5C
		protected override bool TryCastShot()
		{
			bool result = base.TryCastShot();
			Vector3 vector = base.InterpolatedPosition.Yto0();
			IntVec3 intVec = vector.ToIntVec3();
			Vector3 vector2 = this.caster.DrawPos;
			Vector3 normalized = (vector - vector2).normalized;
			vector2 += normalized * Verb_ArcSprayIncinerator.BarrelOffset;
			IntVec3 position = this.caster.Position;
			MoteDualAttached mote = MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_IncineratorBurst, new TargetInfo(position, this.caster.Map, false), new TargetInfo(intVec, this.caster.Map, false));
			float num = Vector3.Distance(vector, vector2);
			float num2 = (num < Verb_ArcSprayIncinerator.BarrelOffset) ? 0.5f : 1f;
			IncineratorSpray incineratorSpray = this.sprayer;
			if (incineratorSpray == null)
			{
				return result;
			}
			incineratorSpray.Add(new IncineratorProjectileMotion
			{
				mote = mote,
				targetDest = intVec,
				worldSource = vector2,
				worldTarget = vector,
				moveVector = (vector - vector2).normalized,
				startScale = 1f * num2,
				endScale = (1f + Rand.Range(0.1f, 0.4f)) * num2,
				lifespanTicks = Mathf.FloorToInt(num * Verb_ArcSprayIncinerator.DistanceToLifetimeScalar)
			});
			return result;
		}

		// Token: 0x04004008 RID: 16392
		[TweakValue("Incinerator", 0f, 10f)]
		public static float DistanceToLifetimeScalar = 5f;

		// Token: 0x04004009 RID: 16393
		[TweakValue("Incinerator", -2f, 7f)]
		public static float BarrelOffset = 5f;

		// Token: 0x0400400A RID: 16394
		private IncineratorSpray sprayer;
	}
}
