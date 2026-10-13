using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200027B RID: 635
	public class SubEffecterDef
	{
		// Token: 0x0600100B RID: 4107 RVA: 0x0004BB36 File Offset: 0x00049D36
		public SubEffecter Spawn(Effecter parent)
		{
			return (SubEffecter)Activator.CreateInstance(this.subEffecterClass, new object[]
			{
				this,
				parent
			});
		}

		// Token: 0x04000C5D RID: 3165
		public Type subEffecterClass;

		// Token: 0x04000C5E RID: 3166
		public IntRange burstCount = new IntRange(1, 1);

		// Token: 0x04000C5F RID: 3167
		public int ticksBetweenMotes = 40;

		// Token: 0x04000C60 RID: 3168
		public int maxMoteCount = int.MaxValue;

		// Token: 0x04000C61 RID: 3169
		public int initialDelayTicks;

		// Token: 0x04000C62 RID: 3170
		public int lifespanMaxTicks = 999999;

		// Token: 0x04000C63 RID: 3171
		public float chancePerTick = 0.1f;

		// Token: 0x04000C64 RID: 3172
		public int chancePeriodTicks;

		// Token: 0x04000C65 RID: 3173
		public MoteSpawnLocType spawnLocType = MoteSpawnLocType.BetweenPositions;

		// Token: 0x04000C66 RID: 3174
		public float positionLerpFactor = 0.5f;

		// Token: 0x04000C67 RID: 3175
		public Vector3 positionOffset = Vector3.zero;

		// Token: 0x04000C68 RID: 3176
		public float positionRadius;

		// Token: 0x04000C69 RID: 3177
		public float positionRadiusMin;

		// Token: 0x04000C6A RID: 3178
		public List<Vector3> perRotationOffsets;

		// Token: 0x04000C6B RID: 3179
		public Vector3? positionDimensions;

		// Token: 0x04000C6C RID: 3180
		public bool attachToSpawnThing;

		// Token: 0x04000C6D RID: 3181
		public float avoidLastPositionRadius;

		// Token: 0x04000C6E RID: 3182
		public AttachPointType attachPoint;

		// Token: 0x04000C6F RID: 3183
		public ThingDef moteDef;

		// Token: 0x04000C70 RID: 3184
		public FleckDef fleckDef;

		// Token: 0x04000C71 RID: 3185
		public Color color = Color.white;

		// Token: 0x04000C72 RID: 3186
		public FloatRange angle = new FloatRange(0f, 360f);

		// Token: 0x04000C73 RID: 3187
		public bool absoluteAngle;

		// Token: 0x04000C74 RID: 3188
		public bool useTargetAInitialRotation;

		// Token: 0x04000C75 RID: 3189
		public bool useTargetBInitialRotation;

		// Token: 0x04000C76 RID: 3190
		public bool fleckUsesAngleForVelocity;

		// Token: 0x04000C77 RID: 3191
		public bool rotateTowardsTargetCenter;

		// Token: 0x04000C78 RID: 3192
		public bool useTargetABodyAngle;

		// Token: 0x04000C79 RID: 3193
		public bool useTargetBBodyAngle;

		// Token: 0x04000C7A RID: 3194
		public FloatRange speed = new FloatRange(0f, 0f);

		// Token: 0x04000C7B RID: 3195
		public FloatRange rotation = new FloatRange(0f, 360f);

		// Token: 0x04000C7C RID: 3196
		public FloatRange rotationRate = new FloatRange(0f, 0f);

		// Token: 0x04000C7D RID: 3197
		public FloatRange scale = new FloatRange(1f, 1f);

		// Token: 0x04000C7E RID: 3198
		public FloatRange airTime = new FloatRange(999999f, 999999f);

		// Token: 0x04000C7F RID: 3199
		public SoundDef soundDef;

		// Token: 0x04000C80 RID: 3200
		public IntRange intermittentSoundInterval = new IntRange(300, 600);

		// Token: 0x04000C81 RID: 3201
		public int ticksBeforeSustainerStart;

		// Token: 0x04000C82 RID: 3202
		public bool orbitOrigin;

		// Token: 0x04000C83 RID: 3203
		public FloatRange orbitSpeed;

		// Token: 0x04000C84 RID: 3204
		public float orbitSnapStrength;

		// Token: 0x04000C85 RID: 3205
		public bool makeMoteOnSubtrigger;

		// Token: 0x04000C86 RID: 3206
		public bool destroyMoteOnCleanup;

		// Token: 0x04000C87 RID: 3207
		public FloatRange cameraShake;

		// Token: 0x04000C88 RID: 3208
		public float distanceAttenuationScale;

		// Token: 0x04000C89 RID: 3209
		public float distanceAttenuationMax = 100f;

		// Token: 0x04000C8A RID: 3210
		public float randomWeight = 1f;

		// Token: 0x04000C8B RID: 3211
		public bool subTriggerOnSpawn = true;

		// Token: 0x04000C8C RID: 3212
		public List<SubEffecterDef> children;
	}
}
