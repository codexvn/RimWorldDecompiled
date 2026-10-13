using System;

namespace Verse
{
	// Token: 0x02000D5E RID: 3422
	public static class ShootTuning
	{
		// Token: 0x04003F59 RID: 16217
		public const float DistTouch = 3f;

		// Token: 0x04003F5A RID: 16218
		public const float DistShort = 12f;

		// Token: 0x04003F5B RID: 16219
		public const float DistMedium = 25f;

		// Token: 0x04003F5C RID: 16220
		public const float DistLong = 40f;

		// Token: 0x04003F5D RID: 16221
		public const float MeleeRange = 1.42f;

		// Token: 0x04003F5E RID: 16222
		public const float HitChanceFactorFromEquipmentMin = 0.01f;

		// Token: 0x04003F5F RID: 16223
		public const float MinAccuracyFactorFromShooterAndDistance = 0.0201f;

		// Token: 0x04003F60 RID: 16224
		public const float LayingDownHitChanceFactorMinDistance = 4.5f;

		// Token: 0x04003F61 RID: 16225
		public const float HitChanceFactorIfLayingDown = 0.5f;

		// Token: 0x04003F62 RID: 16226
		public const float ExecutionMaxDistance = 3.9f;

		// Token: 0x04003F63 RID: 16227
		public const float ExecutionAccuracyFactor = 7.5f;

		// Token: 0x04003F64 RID: 16228
		public const float TargetSizeFactorFromFillPercentFactor = 2.5f;

		// Token: 0x04003F65 RID: 16229
		public const float TargetSizeFactorMin = 0.5f;

		// Token: 0x04003F66 RID: 16230
		public const float TargetSizeFactorMax = 2f;

		// Token: 0x04003F67 RID: 16231
		public const float MinAimOnChance_StandardTarget = 0.0201f;

		// Token: 0x04003F68 RID: 16232
		public static readonly SimpleSurface MissDistanceFromAimOnChanceCurves = new SimpleSurface
		{
			new SurfaceColumn(0.02f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 10f),
					true
				}
			}),
			new SurfaceColumn(0.04f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 8f),
					true
				}
			}),
			new SurfaceColumn(0.07f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 6f),
					true
				}
			}),
			new SurfaceColumn(0.11f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 4f),
					true
				}
			}),
			new SurfaceColumn(0.22f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 2f),
					true
				}
			}),
			new SurfaceColumn(1f, new SimpleCurve
			{
				{
					new CurvePoint(0f, 1f),
					true
				},
				{
					new CurvePoint(1f, 1f),
					true
				}
			})
		};

		// Token: 0x04003F69 RID: 16233
		public const float CanInterceptPawnsChanceOnWildOrForcedMissRadius = 0.5f;

		// Token: 0x04003F6A RID: 16234
		public const float InterceptDistMin = 5f;

		// Token: 0x04003F6B RID: 16235
		public const float InterceptDistMax = 12f;

		// Token: 0x04003F6C RID: 16236
		public const float Intercept_Pawn_HitChancePerBodySize = 0.4f;

		// Token: 0x04003F6D RID: 16237
		public const float Intercept_Pawn_HitChanceFactor_LayingDown = 0.1f;

		// Token: 0x04003F6E RID: 16238
		public const float Intercept_Object_HitChancePerFillPercent = 0.15f;

		// Token: 0x04003F6F RID: 16239
		public const float Intercept_Object_AdjToTarget_HitChancePerFillPercent = 1f;

		// Token: 0x04003F70 RID: 16240
		public const float Intercept_OpenDoor_HitChance = 0.05f;

		// Token: 0x04003F71 RID: 16241
		public const float ImpactCell_Pawn_HitChancePerBodySize = 0.5f;

		// Token: 0x04003F72 RID: 16242
		public const float ImpactCell_Object_HitChancePerFillPercent = 1.5f;

		// Token: 0x04003F73 RID: 16243
		public const float BodySizeClampMin = 0.1f;

		// Token: 0x04003F74 RID: 16244
		public const float BodySizeClampMax = 2f;
	}
}
