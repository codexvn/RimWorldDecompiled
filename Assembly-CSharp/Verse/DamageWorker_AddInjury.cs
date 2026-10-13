using System;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000663 RID: 1635
	public class DamageWorker_AddInjury : DamageWorker
	{
		// Token: 0x06002930 RID: 10544 RVA: 0x000D8BDC File Offset: 0x000D6DDC
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn == null)
			{
				return base.Apply(dinfo, thing);
			}
			return this.ApplyToPawn(dinfo, pawn);
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x000D8C04 File Offset: 0x000D6E04
		private DamageWorker.DamageResult ApplyToPawn(DamageInfo dinfo, Pawn pawn)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			if (dinfo.Amount <= 0f)
			{
				return damageResult;
			}
			if (!DebugSettings.enablePlayerDamage && pawn.Faction == Faction.OfPlayer)
			{
				return damageResult;
			}
			Map mapHeld = pawn.MapHeld;
			bool spawnedOrAnyParentSpawned = pawn.SpawnedOrAnyParentSpawned;
			if (dinfo.ApplyAllDamage)
			{
				float num = dinfo.Amount;
				int num2 = 25;
				float b = num / (float)dinfo.DamagePropagationPartsRange.RandomInRange;
				do
				{
					DamageInfo dinfo2 = dinfo;
					dinfo2.SetAmount(Mathf.Min(num, b));
					this.ApplyDamageToPart(dinfo2, pawn, damageResult);
					num -= damageResult.totalDamageDealt;
					if (num2-- <= 0)
					{
						break;
					}
				}
				while (num > 0f);
			}
			else if (dinfo.AllowDamagePropagation && dinfo.Amount >= (float)dinfo.Def.minDamageToFragment)
			{
				int randomInRange = dinfo.DamagePropagationPartsRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					DamageInfo dinfo3 = dinfo;
					dinfo3.SetAmount(dinfo.Amount / (float)randomInRange);
					this.ApplyDamageToPart(dinfo3, pawn, damageResult);
				}
			}
			else
			{
				this.ApplyDamageToPart(dinfo, pawn, damageResult);
				this.ApplySmallPawnDamagePropagation(dinfo, pawn, damageResult);
			}
			if (damageResult.wounded)
			{
				DamageWorker_AddInjury.PlayWoundedVoiceSound(dinfo, pawn);
				pawn.Drawer.Notify_DamageApplied(dinfo);
				EffecterDef damageEffecter = pawn.RaceProps.FleshType.damageEffecter;
				if (damageEffecter != null)
				{
					if (pawn.health.woundedEffecter != null && pawn.health.woundedEffecter.def != damageEffecter)
					{
						pawn.health.woundedEffecter.Cleanup();
					}
					pawn.health.woundedEffecter = damageEffecter.Spawn();
					pawn.health.woundedEffecter.Trigger(pawn, dinfo.Instigator ?? pawn, -1);
				}
				if (dinfo.Def.damageEffecter != null)
				{
					Effecter effecter = dinfo.Def.damageEffecter.Spawn();
					effecter.Trigger(pawn, pawn, -1);
					effecter.Cleanup();
				}
			}
			if (damageResult.headshot && pawn.Spawned)
			{
				MoteMaker.ThrowText(new Vector3((float)pawn.Position.x + 1f, (float)pawn.Position.y, (float)pawn.Position.z + 1f), pawn.Map, "Headshot".Translate(), Color.white, -1f);
				if (dinfo.Instigator != null)
				{
					Pawn pawn2 = dinfo.Instigator as Pawn;
					if (pawn2 != null)
					{
						pawn2.records.Increment(RecordDefOf.Headshots);
					}
				}
			}
			if ((damageResult.deflected || damageResult.diminished) && spawnedOrAnyParentSpawned)
			{
				EffecterDef effecterDef;
				if (damageResult.deflected)
				{
					if (damageResult.deflectedByMetalArmor && dinfo.Def.canUseDeflectMetalEffect)
					{
						if (dinfo.Def == DamageDefOf.Bullet)
						{
							effecterDef = EffecterDefOf.Deflect_Metal_Bullet;
						}
						else
						{
							effecterDef = EffecterDefOf.Deflect_Metal;
						}
					}
					else if (dinfo.Def == DamageDefOf.Bullet)
					{
						effecterDef = EffecterDefOf.Deflect_General_Bullet;
					}
					else
					{
						effecterDef = EffecterDefOf.Deflect_General;
					}
				}
				else if (damageResult.diminishedByMetalArmor)
				{
					effecterDef = EffecterDefOf.DamageDiminished_Metal;
				}
				else
				{
					effecterDef = EffecterDefOf.DamageDiminished_General;
				}
				if (pawn.health.deflectionEffecter == null || pawn.health.deflectionEffecter.def != effecterDef)
				{
					if (pawn.health.deflectionEffecter != null)
					{
						pawn.health.deflectionEffecter.Cleanup();
						pawn.health.deflectionEffecter = null;
					}
					pawn.health.deflectionEffecter = effecterDef.Spawn();
				}
				TargetInfo targetInfo = new TargetInfo(pawn.Position, mapHeld, false);
				Effecter deflectionEffecter = pawn.health.deflectionEffecter;
				TargetInfo a = targetInfo;
				Thing instigator = dinfo.Instigator;
				deflectionEffecter.Trigger(a, (instigator != null) ? instigator : targetInfo, -1);
				if (damageResult.deflected)
				{
					pawn.Drawer.Notify_DamageDeflected(dinfo);
				}
			}
			if (!damageResult.deflected && spawnedOrAnyParentSpawned)
			{
				ImpactSoundUtility.PlayImpactSound(pawn, dinfo.Def.impactSoundType, mapHeld);
			}
			return damageResult;
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x000D8FF0 File Offset: 0x000D71F0
		private void ApplySmallPawnDamagePropagation(DamageInfo dinfo, Pawn pawn, DamageWorker.DamageResult result)
		{
			if (!dinfo.AllowDamagePropagation)
			{
				return;
			}
			if (result.LastHitPart != null && dinfo.Def.harmsHealth && result.LastHitPart != pawn.RaceProps.body.corePart && result.LastHitPart.parent != null && pawn.health.hediffSet.GetPartHealth(result.LastHitPart.parent) > 0f && result.LastHitPart.parent.coverageAbs > 0f && dinfo.Amount >= 10f && pawn.HealthScale <= 0.5001f)
			{
				DamageInfo dinfo2 = dinfo;
				dinfo2.SetHitPart(result.LastHitPart.parent);
				this.ApplyDamageToPart(dinfo2, pawn, result);
			}
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x000D90BC File Offset: 0x000D72BC
		private void ApplyDamageToPart(DamageInfo dinfo, Pawn pawn, DamageWorker.DamageResult result)
		{
			BodyPartRecord exactPartFromDamageInfo = this.GetExactPartFromDamageInfo(dinfo, pawn);
			if (exactPartFromDamageInfo == null)
			{
				return;
			}
			dinfo.SetHitPart(exactPartFromDamageInfo);
			float num = dinfo.Amount;
			bool flag = !dinfo.InstantPermanentInjury && !dinfo.IgnoreArmor;
			bool deflectedByMetalArmor = false;
			if (flag)
			{
				DamageDef def = dinfo.Def;
				bool diminishedByMetalArmor;
				num = ArmorUtility.GetPostArmorDamage(pawn, num, dinfo.ArmorPenetrationInt, dinfo.HitPart, ref def, out deflectedByMetalArmor, out diminishedByMetalArmor);
				dinfo.Def = def;
				if (num < dinfo.Amount)
				{
					result.diminished = true;
					result.diminishedByMetalArmor = diminishedByMetalArmor;
				}
			}
			if (dinfo.Def.ExternalViolenceFor(pawn))
			{
				num *= pawn.GetStatValue(StatDefOf.IncomingDamageFactor, true, -1);
			}
			if (num <= 0f)
			{
				result.AddPart(pawn, dinfo.HitPart);
				result.deflected = true;
				result.deflectedByMetalArmor = deflectedByMetalArmor;
				return;
			}
			if (DamageWorker_AddInjury.IsHeadshot(dinfo, pawn))
			{
				result.headshot = true;
			}
			if (dinfo.InstantPermanentInjury && (HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, dinfo.HitPart).CompPropsFor(typeof(HediffComp_GetsPermanent)) == null || dinfo.HitPart.def.permanentInjuryChanceFactor == 0f || pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(dinfo.HitPart)))
			{
				return;
			}
			if (!dinfo.AllowDamagePropagation)
			{
				this.FinalizeAndAddInjury(pawn, num, dinfo, result);
				return;
			}
			this.ApplySpecialEffectsToPart(pawn, num, dinfo, result);
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x000D9219 File Offset: 0x000D7419
		protected virtual void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			totalDamage = this.ReduceDamageToPreserveOutsideParts(totalDamage, dinfo, pawn);
			this.FinalizeAndAddInjury(pawn, totalDamage, dinfo, result);
			this.CheckDuplicateDamageToOuterParts(dinfo, pawn, totalDamage, result);
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x000D9240 File Offset: 0x000D7440
		protected float FinalizeAndAddInjury(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			if (pawn.health.hediffSet.PartIsMissing(dinfo.HitPart))
			{
				return 0f;
			}
			Pawn pawn2 = dinfo.Instigator as Pawn;
			HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, dinfo.HitPart);
			Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(hediffDefFromDamage, pawn, null);
			hediff_Injury.Part = dinfo.HitPart;
			hediff_Injury.sourceDef = dinfo.Weapon;
			if (pawn2 != null && pawn2.IsMutant && dinfo.Weapon == ThingDefOf.Human)
			{
				hediff_Injury.sourceLabel = pawn2.mutant.Def.label;
			}
			else
			{
				Hediff hediff = hediff_Injury;
				ThingDef weapon = dinfo.Weapon;
				hediff.sourceLabel = (((weapon != null) ? weapon.label : null) ?? "");
			}
			hediff_Injury.sourceBodyPartGroup = dinfo.WeaponBodyPartGroup;
			hediff_Injury.sourceHediffDef = dinfo.WeaponLinkedHediff;
			Hediff hediff2 = hediff_Injury;
			Tool tool = dinfo.Tool;
			string sourceToolLabel;
			if ((sourceToolLabel = ((tool != null) ? tool.labelNoLocation : null)) == null)
			{
				Tool tool2 = dinfo.Tool;
				sourceToolLabel = ((tool2 != null) ? tool2.label : null);
			}
			hediff2.sourceToolLabel = sourceToolLabel;
			hediff_Injury.Severity = totalDamage;
			if (pawn2 != null && pawn2.CurJobDef == JobDefOf.SocialFight)
			{
				hediff_Injury.destroysBodyParts = false;
			}
			if (dinfo.InstantPermanentInjury)
			{
				HediffComp_GetsPermanent hediffComp_GetsPermanent = hediff_Injury.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null)
				{
					hediffComp_GetsPermanent.IsPermanent = true;
				}
				else
				{
					string str = "Tried to create instant permanent injury on Hediff without a GetsPermanent comp: ";
					HediffDef hediffDef = hediffDefFromDamage;
					Log.Error(str + ((hediffDef != null) ? hediffDef.ToString() : null) + " on " + ((pawn != null) ? pawn.ToString() : null));
				}
			}
			return this.FinalizeAndAddInjury(pawn, hediff_Injury, dinfo, result);
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x000D93C8 File Offset: 0x000D75C8
		protected float FinalizeAndAddInjury(Pawn pawn, Hediff_Injury injury, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			HediffComp_GetsPermanent hediffComp_GetsPermanent = injury.TryGetComp<HediffComp_GetsPermanent>();
			if (hediffComp_GetsPermanent != null)
			{
				hediffComp_GetsPermanent.PreFinalizeInjury();
			}
			float partHealth = pawn.health.hediffSet.GetPartHealth(injury.Part);
			if (pawn.IsColonist && !dinfo.IgnoreInstantKillProtection && dinfo.Def.ExternalViolenceFor(pawn) && !Rand.Chance(Find.Storyteller.difficulty.allowInstantKillChance))
			{
				float num = injury.IsLethal ? (injury.def.lethalSeverity * 1.1f) : 1f;
				float min = 1f;
				float max = Mathf.Min(injury.Severity, partHealth);
				int num2 = 0;
				while (num2 < 7 && pawn.health.WouldDieAfterAddingHediff(injury))
				{
					float num3 = Mathf.Clamp(partHealth - num, min, max);
					if (DebugViewSettings.logCauseOfDeath)
					{
						Log.Message(string.Format("CauseOfDeath: attempt to prevent death for {0} on {1} attempt:{2} severity:{3}->{4} part health:{5}", new object[]
						{
							pawn.Name,
							injury.Part.Label,
							num2 + 1,
							injury.Severity,
							num3,
							partHealth
						}));
					}
					injury.Severity = num3;
					num *= 2f;
					min = 0f;
					num2++;
				}
			}
			pawn.health.AddHediff(injury, null, new DamageInfo?(dinfo), result);
			float num4 = Mathf.Min(injury.Severity, partHealth);
			result.totalDamageDealt += num4;
			result.wounded = true;
			result.AddPart(pawn, injury.Part);
			result.AddHediff(injury);
			if (!this.def.additionalHediffsThisPart.NullOrEmpty<HediffDef>() && !pawn.health.hediffSet.PartIsMissing(injury.Part))
			{
				foreach (HediffDef def in this.def.additionalHediffsThisPart)
				{
					Hediff hediff = HediffMaker.MakeHediff(def, pawn, injury.Part);
					pawn.health.AddHediff(hediff, null, new DamageInfo?(dinfo), result);
					result.AddHediff(hediff);
				}
			}
			return num4;
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x000D960C File Offset: 0x000D780C
		private void CheckDuplicateDamageToOuterParts(DamageInfo dinfo, Pawn pawn, float totalDamage, DamageWorker.DamageResult result)
		{
			if (!dinfo.AllowDamagePropagation)
			{
				return;
			}
			if (dinfo.Def.harmAllLayersUntilOutside && dinfo.HitPart.depth == BodyPartDepth.Inside)
			{
				BodyPartRecord parent = dinfo.HitPart.parent;
				do
				{
					if (pawn.health.hediffSet.GetPartHealth(parent) != 0f && parent.coverageAbs > 0f)
					{
						Pawn pawn2 = dinfo.Instigator as Pawn;
						Hediff_Injury hediff_Injury = (Hediff_Injury)HediffMaker.MakeHediff(HealthUtility.GetHediffDefFromDamage(dinfo.Def, pawn, parent), pawn, null);
						hediff_Injury.Part = parent;
						hediff_Injury.sourceDef = dinfo.Weapon;
						if (pawn2 != null && pawn2.IsMutant && dinfo.Weapon == ThingDefOf.Human)
						{
							hediff_Injury.sourceLabel = pawn2.mutant.Def.label;
						}
						else
						{
							Hediff hediff = hediff_Injury;
							ThingDef weapon = dinfo.Weapon;
							hediff.sourceLabel = (((weapon != null) ? weapon.label : null) ?? "");
						}
						hediff_Injury.sourceBodyPartGroup = dinfo.WeaponBodyPartGroup;
						hediff_Injury.Severity = totalDamage;
						if (hediff_Injury.Severity <= 0f)
						{
							hediff_Injury.Severity = 1f;
						}
						this.FinalizeAndAddInjury(pawn, hediff_Injury, dinfo, result);
					}
					if (parent.depth == BodyPartDepth.Outside)
					{
						break;
					}
					parent = parent.parent;
				}
				while (parent != null);
			}
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x000D975B File Offset: 0x000D795B
		private static bool IsHeadshot(DamageInfo dinfo, Pawn pawn)
		{
			return !dinfo.InstantPermanentInjury && dinfo.HitPart.groups.Contains(BodyPartGroupDefOf.FullHead) && dinfo.Def.isRanged;
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x000D9790 File Offset: 0x000D7990
		private BodyPartRecord GetExactPartFromDamageInfo(DamageInfo dinfo, Pawn pawn)
		{
			if (dinfo.HitPart == null)
			{
				BodyPartRecord bodyPartRecord = this.ChooseHitPart(dinfo, pawn);
				if (bodyPartRecord == null)
				{
					Log.Warning("ChooseHitPart returned null (any part).");
				}
				return bodyPartRecord;
			}
			if (!pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Any((BodyPartRecord x) => x == dinfo.HitPart))
			{
				return null;
			}
			return dinfo.HitPart;
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x000D9806 File Offset: 0x000D7A06
		protected virtual BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, dinfo.Depth, null);
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x000D9830 File Offset: 0x000D7A30
		private static void PlayWoundedVoiceSound(DamageInfo dinfo, Pawn pawn)
		{
			if (pawn.Dead)
			{
				return;
			}
			if (dinfo.InstantPermanentInjury)
			{
				return;
			}
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			if (dinfo.Def.ExternalViolenceFor(pawn))
			{
				LifeStageUtility.PlayNearestLifestageSound(pawn, (LifeStageAge lifeStage) => lifeStage.soundWounded, (GeneDef gene) => gene.soundWounded, (MutantDef mutantDef) => mutantDef.soundWounded, 1f);
			}
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x000D98D0 File Offset: 0x000D7AD0
		protected float ReduceDamageToPreserveOutsideParts(float postArmorDamage, DamageInfo dinfo, Pawn pawn)
		{
			if (!DamageWorker_AddInjury.ShouldReduceDamageToPreservePart(dinfo.HitPart))
			{
				return postArmorDamage;
			}
			float partHealth = pawn.health.hediffSet.GetPartHealth(dinfo.HitPart);
			if (postArmorDamage < partHealth)
			{
				return postArmorDamage;
			}
			float maxHealth = dinfo.HitPart.def.GetMaxHealth(pawn);
			float f = (postArmorDamage - partHealth) / maxHealth;
			if (Rand.Chance(this.def.overkillPctToDestroyPart.InverseLerpThroughRange(f)))
			{
				return postArmorDamage;
			}
			return postArmorDamage = partHealth - 1f;
		}

		// Token: 0x0600293D RID: 10557 RVA: 0x000D9948 File Offset: 0x000D7B48
		public static bool ShouldReduceDamageToPreservePart(BodyPartRecord bodyPart)
		{
			return bodyPart.depth == BodyPartDepth.Outside && !bodyPart.IsCorePart;
		}
	}
}
