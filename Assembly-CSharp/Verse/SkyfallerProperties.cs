using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000261 RID: 609
	public class SkyfallerProperties
	{
		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000F7A RID: 3962 RVA: 0x000493A4 File Offset: 0x000475A4
		public bool MakesShrapnel
		{
			get
			{
				return this.metalShrapnelCountRange.max > 0 || this.rubbleShrapnelCountRange.max > 0;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x000493C4 File Offset: 0x000475C4
		public bool CausesExplosion
		{
			get
			{
				return this.explosionDamage != null && this.explosionRadius > 0f;
			}
		}

		// Token: 0x04000B44 RID: 2884
		public bool hitRoof = true;

		// Token: 0x04000B45 RID: 2885
		public IntRange ticksToImpactRange = new IntRange(120, 200);

		// Token: 0x04000B46 RID: 2886
		public IntRange ticksToDiscardInReverse = IntRange.Zero;

		// Token: 0x04000B47 RID: 2887
		public bool reversed;

		// Token: 0x04000B48 RID: 2888
		public bool flightFlippedHorizontally;

		// Token: 0x04000B49 RID: 2889
		public float explosionRadius = 3f;

		// Token: 0x04000B4A RID: 2890
		public DamageDef explosionDamage;

		// Token: 0x04000B4B RID: 2891
		public bool damageSpawnedThings;

		// Token: 0x04000B4C RID: 2892
		public float explosionDamageFactor = 1f;

		// Token: 0x04000B4D RID: 2893
		public IntRange metalShrapnelCountRange = IntRange.Zero;

		// Token: 0x04000B4E RID: 2894
		public IntRange rubbleShrapnelCountRange = IntRange.Zero;

		// Token: 0x04000B4F RID: 2895
		public float shrapnelDistanceFactor = 1f;

		// Token: 0x04000B50 RID: 2896
		public ThingDef spawnThing;

		// Token: 0x04000B51 RID: 2897
		public SkyfallerMovementType movementType;

		// Token: 0x04000B52 RID: 2898
		public float speed = 1f;

		// Token: 0x04000B53 RID: 2899
		public string shadow = "Things/Skyfaller/SkyfallerShadowCircle";

		// Token: 0x04000B54 RID: 2900
		public Vector2 shadowSize = Vector2.one;

		// Token: 0x04000B55 RID: 2901
		public float cameraShake;

		// Token: 0x04000B56 RID: 2902
		public SoundDef impactSound;

		// Token: 0x04000B57 RID: 2903
		public bool rotateGraphicTowardsDirection;

		// Token: 0x04000B58 RID: 2904
		public SoundDef anticipationSound;

		// Token: 0x04000B59 RID: 2905
		public SoundDef floatingSound;

		// Token: 0x04000B5A RID: 2906
		public int anticipationSoundTicks = 100;

		// Token: 0x04000B5B RID: 2907
		public int motesPerCell = 3;

		// Token: 0x04000B5C RID: 2908
		public float moteSpawnTime = float.MinValue;

		// Token: 0x04000B5D RID: 2909
		public SimpleCurve xPositionCurve;

		// Token: 0x04000B5E RID: 2910
		public SimpleCurve zPositionCurve;

		// Token: 0x04000B5F RID: 2911
		public SimpleCurve angleCurve;

		// Token: 0x04000B60 RID: 2912
		public SimpleCurve rotationCurve;

		// Token: 0x04000B61 RID: 2913
		public SimpleCurve speedCurve;

		// Token: 0x04000B62 RID: 2914
		public int fadeInTicks;

		// Token: 0x04000B63 RID: 2915
		public int fadeOutTicks;
	}
}
