using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001969 RID: 6505
	public class CompProperties_Explosive : CompProperties
	{
		// Token: 0x060093B7 RID: 37815 RVA: 0x002B715C File Offset: 0x002B535C
		public CompProperties_Explosive()
		{
			this.compClass = typeof(CompExplosive);
		}

		// Token: 0x060093B8 RID: 37816 RVA: 0x002B7200 File Offset: 0x002B5400
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			if (this.explosiveDamageType == null)
			{
				this.explosiveDamageType = DamageDefOf.Bomb;
			}
		}

		// Token: 0x060093B9 RID: 37817 RVA: 0x002B721C File Offset: 0x002B541C
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.tickerType != TickerType.Normal)
			{
				yield return "CompExplosive requires Normal ticker type";
			}
			yield break;
			yield break;
		}

		// Token: 0x04005F13 RID: 24339
		public float explosiveRadius = 1.9f;

		// Token: 0x04005F14 RID: 24340
		public DamageDef explosiveDamageType;

		// Token: 0x04005F15 RID: 24341
		public int damageAmountBase = -1;

		// Token: 0x04005F16 RID: 24342
		public float armorPenetrationBase = -1f;

		// Token: 0x04005F17 RID: 24343
		public ThingDef postExplosionSpawnThingDef;

		// Token: 0x04005F18 RID: 24344
		public float postExplosionSpawnChance;

		// Token: 0x04005F19 RID: 24345
		public int postExplosionSpawnThingCount = 1;

		// Token: 0x04005F1A RID: 24346
		public bool applyDamageToExplosionCellsNeighbors;

		// Token: 0x04005F1B RID: 24347
		public ThingDef preExplosionSpawnThingDef;

		// Token: 0x04005F1C RID: 24348
		public float preExplosionSpawnChance;

		// Token: 0x04005F1D RID: 24349
		public int preExplosionSpawnThingCount = 1;

		// Token: 0x04005F1E RID: 24350
		public float chanceToStartFire;

		// Token: 0x04005F1F RID: 24351
		public bool damageFalloff;

		// Token: 0x04005F20 RID: 24352
		public bool explodeOnKilled;

		// Token: 0x04005F21 RID: 24353
		public bool explodeOnDestroyed;

		// Token: 0x04005F22 RID: 24354
		public GasType? postExplosionGasType;

		// Token: 0x04005F23 RID: 24355
		public float? postExplosionGasRadiusOverride;

		// Token: 0x04005F24 RID: 24356
		public int postExplosionGasAmount = 255;

		// Token: 0x04005F25 RID: 24357
		public bool doVisualEffects = true;

		// Token: 0x04005F26 RID: 24358
		public bool doSoundEffects = true;

		// Token: 0x04005F27 RID: 24359
		public float propagationSpeed = 1f;

		// Token: 0x04005F28 RID: 24360
		public ThingDef postExplosionSpawnSingleThingDef;

		// Token: 0x04005F29 RID: 24361
		public ThingDef preExplosionSpawnSingleThingDef;

		// Token: 0x04005F2A RID: 24362
		public float explosiveExpandPerStackcount;

		// Token: 0x04005F2B RID: 24363
		public float explosiveExpandPerFuel;

		// Token: 0x04005F2C RID: 24364
		public EffecterDef explosionEffect;

		// Token: 0x04005F2D RID: 24365
		public SoundDef explosionSound;

		// Token: 0x04005F2E RID: 24366
		public List<DamageDef> startWickOnDamageTaken;

		// Token: 0x04005F2F RID: 24367
		public float startWickHitPointsPercent = 0.2f;

		// Token: 0x04005F30 RID: 24368
		public IntRange wickTicks = new IntRange(140, 150);

		// Token: 0x04005F31 RID: 24369
		public float wickScale = 1f;

		// Token: 0x04005F32 RID: 24370
		public List<DamageDef> startWickOnInternalDamageTaken;

		// Token: 0x04005F33 RID: 24371
		public bool drawWick = true;

		// Token: 0x04005F34 RID: 24372
		public float chanceNeverExplodeFromDamage;

		// Token: 0x04005F35 RID: 24373
		public float destroyThingOnExplosionSize;

		// Token: 0x04005F36 RID: 24374
		public DamageDef requiredDamageTypeToExplode;

		// Token: 0x04005F37 RID: 24375
		public IntRange? countdownTicks;

		// Token: 0x04005F38 RID: 24376
		public string extraInspectStringKey;

		// Token: 0x04005F39 RID: 24377
		public List<WickMessage> wickMessages;
	}
}
