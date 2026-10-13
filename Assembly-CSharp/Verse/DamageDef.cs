using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000229 RID: 553
	public class DamageDef : Def
	{
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000E8F RID: 3727 RVA: 0x00043F33 File Offset: 0x00042133
		public DamageWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (DamageWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00043F68 File Offset: 0x00042168
		public bool ExternalViolenceFor(Thing thing)
		{
			if (this.externalViolence)
			{
				return true;
			}
			if (this.externalViolenceForMechanoids)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && pawn.RaceProps.IsMechanoid)
				{
					return true;
				}
				if (thing is Building_Turret)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000918 RID: 2328
		public Type workerClass = typeof(DamageWorker);

		// Token: 0x04000919 RID: 2329
		private bool externalViolence;

		// Token: 0x0400091A RID: 2330
		private bool externalViolenceForMechanoids;

		// Token: 0x0400091B RID: 2331
		public bool hasForcefulImpact = true;

		// Token: 0x0400091C RID: 2332
		public bool harmsHealth = true;

		// Token: 0x0400091D RID: 2333
		public bool makesBlood = true;

		// Token: 0x0400091E RID: 2334
		public bool canInterruptJobs = true;

		// Token: 0x0400091F RID: 2335
		public bool isRanged;

		// Token: 0x04000920 RID: 2336
		public bool makesAnimalsFlee;

		// Token: 0x04000921 RID: 2337
		public bool execution;

		// Token: 0x04000922 RID: 2338
		public RulePackDef combatLogRules;

		// Token: 0x04000923 RID: 2339
		public float buildingDamageFactor = 1f;

		// Token: 0x04000924 RID: 2340
		public float buildingDamageFactorPassable = 1f;

		// Token: 0x04000925 RID: 2341
		public float buildingDamageFactorImpassable = 1f;

		// Token: 0x04000926 RID: 2342
		public float plantDamageFactor = 1f;

		// Token: 0x04000927 RID: 2343
		public float corpseDamageFactor = 1f;

		// Token: 0x04000928 RID: 2344
		public bool causeStun;

		// Token: 0x04000929 RID: 2345
		public int stunAdaptationTicks;

		// Token: 0x0400092A RID: 2346
		public int? constantStunDurationTicks;

		// Token: 0x0400092B RID: 2347
		public StatDef stunResistStat;

		// Token: 0x0400092C RID: 2348
		public bool displayAdaptedTextMote = true;

		// Token: 0x0400092D RID: 2349
		[MustTranslate]
		public string adaptedText;

		// Token: 0x0400092E RID: 2350
		public bool canUseDeflectMetalEffect = true;

		// Token: 0x0400092F RID: 2351
		public ImpactSoundTypeDef impactSoundType;

		// Token: 0x04000930 RID: 2352
		[MustTranslate]
		public string deathMessage = "{0} has been killed.";

		// Token: 0x04000931 RID: 2353
		public EffecterDef damageEffecter;

		// Token: 0x04000932 RID: 2354
		public int defaultDamage = -1;

		// Token: 0x04000933 RID: 2355
		public float defaultArmorPenetration = -1f;

		// Token: 0x04000934 RID: 2356
		public float defaultStoppingPower;

		// Token: 0x04000935 RID: 2357
		public List<DamageDefAdditionalHediff> additionalHediffs;

		// Token: 0x04000936 RID: 2358
		public List<HediffDef> additionalHediffsThisPart;

		// Token: 0x04000937 RID: 2359
		public bool applyAdditionalHediffsIfHuntingForFood = true;

		// Token: 0x04000938 RID: 2360
		public DamageArmorCategoryDef armorCategory;

		// Token: 0x04000939 RID: 2361
		public int minDamageToFragment = 99999;

		// Token: 0x0400093A RID: 2362
		public FloatRange overkillPctToDestroyPart = new FloatRange(0f, 0.7f);

		// Token: 0x0400093B RID: 2363
		public bool consideredHelpful;

		// Token: 0x0400093C RID: 2364
		public SimpleCurve igniteChanceByTargetFlammability;

		// Token: 0x0400093D RID: 2365
		public float igniteCellChance;

		// Token: 0x0400093E RID: 2366
		public bool ignoreShields;

		// Token: 0x0400093F RID: 2367
		public bool harmAllLayersUntilOutside;

		// Token: 0x04000940 RID: 2368
		public HediffDef hediff;

		// Token: 0x04000941 RID: 2369
		public HediffDef hediffSkin;

		// Token: 0x04000942 RID: 2370
		public HediffDef hediffSolid;

		// Token: 0x04000943 RID: 2371
		public bool isExplosive;

		// Token: 0x04000944 RID: 2372
		public float explosionSnowMeltAmount = 1f;

		// Token: 0x04000945 RID: 2373
		public bool explosionAffectOutsidePartsOnly = true;

		// Token: 0x04000946 RID: 2374
		public ThingDef explosionCellMote;

		// Token: 0x04000947 RID: 2375
		public FleckDef explosionCellFleck;

		// Token: 0x04000948 RID: 2376
		public Color explosionColorCenter = Color.white;

		// Token: 0x04000949 RID: 2377
		public Color explosionColorEdge = Color.white;

		// Token: 0x0400094A RID: 2378
		public EffecterDef explosionInteriorEffecter;

		// Token: 0x0400094B RID: 2379
		public ThingDef explosionInteriorMote;

		// Token: 0x0400094C RID: 2380
		public FleckDef explosionInteriorFleck;

		// Token: 0x0400094D RID: 2381
		public ThingDef explosionCenterMote;

		// Token: 0x0400094E RID: 2382
		public FleckDef explosionCenterFleck;

		// Token: 0x0400094F RID: 2383
		public EffecterDef explosionCenterEffecter;

		// Token: 0x04000950 RID: 2384
		public EffecterDef explosionCellEffecter;

		// Token: 0x04000951 RID: 2385
		public float explosionCellEffecterChance;

		// Token: 0x04000952 RID: 2386
		public float explosionCellEffecterMaxRadius;

		// Token: 0x04000953 RID: 2387
		public float explosionHeatEnergyPerCell;

		// Token: 0x04000954 RID: 2388
		public float expolosionPropagationSpeed = 1f;

		// Token: 0x04000955 RID: 2389
		public SoundDef soundExplosion;

		// Token: 0x04000956 RID: 2390
		public float explosionInteriorCellCountMultiplier = 1f;

		// Token: 0x04000957 RID: 2391
		public float explosionInteriorCellDistanceMultiplier = 0.7f;

		// Token: 0x04000958 RID: 2392
		public float stabChanceOfForcedInternal;

		// Token: 0x04000959 RID: 2393
		public SimpleCurve cutExtraTargetsCurve;

		// Token: 0x0400095A RID: 2394
		public float cutCleaveBonus;

		// Token: 0x0400095B RID: 2395
		public float bluntInnerHitChance;

		// Token: 0x0400095C RID: 2396
		public FloatRange bluntInnerHitDamageFractionToConvert;

		// Token: 0x0400095D RID: 2397
		public FloatRange bluntInnerHitDamageFractionToAdd;

		// Token: 0x0400095E RID: 2398
		public float bluntStunDuration = 1f;

		// Token: 0x0400095F RID: 2399
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToHeadCurve;

		// Token: 0x04000960 RID: 2400
		public SimpleCurve bluntStunChancePerDamagePctOfCorePartToBodyCurve;

		// Token: 0x04000961 RID: 2401
		public float scratchSplitPercentage = 0.5f;

		// Token: 0x04000962 RID: 2402
		public bool scaleDamageToBuildingsBasedOnFlammability;

		// Token: 0x04000963 RID: 2403
		[Unsaved(false)]
		private DamageWorker workerInt;
	}
}
