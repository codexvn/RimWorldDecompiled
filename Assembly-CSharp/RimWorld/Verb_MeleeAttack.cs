using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003785 RID: 14213
	public abstract class Verb_MeleeAttack : Verb
	{
		// Token: 0x06013B02 RID: 80642 RVA: 0x005D9304 File Offset: 0x005D7504
		protected override bool TryCastShot()
		{
			Pawn casterPawn = this.CasterPawn;
			if (!casterPawn.Spawned)
			{
				return false;
			}
			if (casterPawn.stances.FullBodyBusy)
			{
				return false;
			}
			Thing thing = this.currentTarget.Thing;
			if (!this.CanHitTarget(thing))
			{
				Pawn pawn = casterPawn;
				string str = (pawn != null) ? pawn.ToString() : null;
				string str2 = " meleed ";
				Thing thing2 = thing;
				Log.Warning(str + str2 + ((thing2 != null) ? thing2.ToString() : null) + " from out of melee position.");
			}
			casterPawn.rotationTracker.Face(thing.DrawPos);
			if (!this.IsTargetImmobile(this.currentTarget) && casterPawn.skills != null && (this.currentTarget.Pawn == null || !this.currentTarget.Pawn.IsColonyMech))
			{
				casterPawn.skills.Learn(SkillDefOf.Melee, 200f * this.verbProps.AdjustedFullCycleTime(this, casterPawn), false, false);
			}
			Pawn pawn2 = thing as Pawn;
			if (pawn2 != null && !pawn2.Dead && (casterPawn.MentalStateDef != MentalStateDefOf.SocialFighting || pawn2.MentalStateDef != MentalStateDefOf.SocialFighting) && (casterPawn.story == null || !casterPawn.story.traits.DisableHostilityFrom(pawn2)))
			{
				pawn2.mindState.meleeThreat = casterPawn;
				pawn2.mindState.lastMeleeThreatHarmTick = Find.TickManager.TicksGame;
			}
			Map map = thing.Map;
			Vector3 drawPos = thing.DrawPos;
			SoundDef soundDef;
			bool result;
			if (Rand.Chance(this.GetNonMissChance(thing)))
			{
				if (!Rand.Chance(this.GetDodgeChance(thing)))
				{
					if (thing.def.category == ThingCategory.Building)
					{
						soundDef = this.SoundHitBuilding();
					}
					else
					{
						soundDef = this.SoundHitPawn();
					}
					if (this.verbProps.impactMote != null)
					{
						MoteMaker.MakeStaticMote(drawPos, map, this.verbProps.impactMote, 1f, false, 0f);
					}
					if (this.verbProps.impactFleck != null)
					{
						FleckMaker.Static(drawPos, map, this.verbProps.impactFleck, 1f);
					}
					BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = this.CreateCombatLog((ManeuverDef maneuver) => maneuver.combatLogRulesHit, true);
					result = true;
					DamageWorker.DamageResult damageResult = this.ApplyMeleeDamageToTarget(this.currentTarget);
					if (pawn2 != null && damageResult.totalDamageDealt > 0f)
					{
						this.ApplyMeleeSlaveSuppression(pawn2, damageResult.totalDamageDealt);
					}
					if (damageResult.stunned && damageResult.parts.NullOrEmpty<BodyPartRecord>())
					{
						Find.BattleLog.RemoveEntry(battleLogEntry_MeleeCombat);
					}
					else
					{
						damageResult.AssociateWithLog(battleLogEntry_MeleeCombat);
						if (damageResult.deflected)
						{
							battleLogEntry_MeleeCombat.RuleDef = this.maneuver.combatLogRulesDeflect;
							battleLogEntry_MeleeCombat.alwaysShowInCompact = false;
						}
					}
				}
				else
				{
					result = false;
					soundDef = this.SoundDodge(thing);
					MoteMaker.ThrowText(drawPos, map, "TextMote_Dodge".Translate(), 1.9f);
					this.CreateCombatLog((ManeuverDef maneuver) => maneuver.combatLogRulesDodge, false);
				}
			}
			else
			{
				result = false;
				soundDef = this.SoundMiss();
				this.CreateCombatLog((ManeuverDef maneuver) => maneuver.combatLogRulesMiss, false);
			}
			if (soundDef != null)
			{
				soundDef.PlayOneShot(new TargetInfo(thing.Position, map, false));
			}
			if (casterPawn.Spawned)
			{
				casterPawn.Drawer.Notify_MeleeAttackOn(thing);
			}
			if (pawn2 != null && !pawn2.Dead && pawn2.Spawned)
			{
				pawn2.stances.stagger.StaggerFor(95, 0.17f);
			}
			if (casterPawn.Spawned)
			{
				casterPawn.rotationTracker.FaceCell(thing.Position);
			}
			if (casterPawn.caller != null)
			{
				casterPawn.caller.Notify_DidMeleeAttack();
			}
			return result;
		}

		// Token: 0x06013B03 RID: 80643 RVA: 0x005D96BC File Offset: 0x005D78BC
		public BattleLogEntry_MeleeCombat CreateCombatLog(Func<ManeuverDef, RulePackDef> rulePackGetter, bool alwaysShow)
		{
			if (this.maneuver == null)
			{
				return null;
			}
			if (this.tool == null)
			{
				return null;
			}
			BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = new BattleLogEntry_MeleeCombat(rulePackGetter(this.maneuver), alwaysShow, this.CasterPawn, this.currentTarget.Thing, base.ImplementOwnerType, this.tool.labelUsedInLogging ? this.tool.label : "", (base.EquipmentSource == null) ? null : base.EquipmentSource.def, (base.HediffCompSource == null) ? null : base.HediffCompSource.Def, this.maneuver.logEntryDef);
			Find.BattleLog.Add(battleLogEntry_MeleeCombat);
			return battleLogEntry_MeleeCombat;
		}

		// Token: 0x06013B04 RID: 80644 RVA: 0x005D976C File Offset: 0x005D796C
		private float GetNonMissChance(LocalTargetInfo target)
		{
			if (this.surpriseAttack)
			{
				return 1f;
			}
			if (this.IsTargetImmobile(target))
			{
				return 1f;
			}
			float num = this.CasterPawn.GetStatValue(StatDefOf.MeleeHitChance, true, -1);
			if (ModsConfig.IdeologyActive && target.HasThing)
			{
				if (DarknessCombatUtility.IsOutdoorsAndLit(target.Thing))
				{
					num += this.caster.GetStatValue(StatDefOf.MeleeHitChanceOutdoorsLitOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsOutdoorsAndDark(target.Thing))
				{
					num += this.caster.GetStatValue(StatDefOf.MeleeHitChanceOutdoorsDarkOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsIndoorsAndDark(target.Thing))
				{
					num += this.caster.GetStatValue(StatDefOf.MeleeHitChanceIndoorsDarkOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsIndoorsAndLit(target.Thing))
				{
					num += this.caster.GetStatValue(StatDefOf.MeleeHitChanceIndoorsLitOffset, true, -1);
				}
			}
			return num;
		}

		// Token: 0x06013B05 RID: 80645 RVA: 0x005D9854 File Offset: 0x005D7A54
		private float GetDodgeChance(LocalTargetInfo target)
		{
			if (this.surpriseAttack)
			{
				return 0f;
			}
			if (this.IsTargetImmobile(target))
			{
				return 0f;
			}
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				return 0f;
			}
			Stance_Busy stance_Busy = pawn.stances.curStance as Stance_Busy;
			if (stance_Busy != null && stance_Busy.verb != null && !stance_Busy.verb.verbProps.IsMeleeAttack)
			{
				return 0f;
			}
			float num = pawn.GetStatValue(StatDefOf.MeleeDodgeChance, true, -1);
			if (ModsConfig.IdeologyActive)
			{
				if (DarknessCombatUtility.IsOutdoorsAndLit(target.Thing))
				{
					num += pawn.GetStatValue(StatDefOf.MeleeDodgeChanceOutdoorsLitOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsOutdoorsAndDark(target.Thing))
				{
					num += pawn.GetStatValue(StatDefOf.MeleeDodgeChanceOutdoorsDarkOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsIndoorsAndDark(target.Thing))
				{
					num += pawn.GetStatValue(StatDefOf.MeleeDodgeChanceIndoorsDarkOffset, true, -1);
				}
				else if (DarknessCombatUtility.IsIndoorsAndLit(target.Thing))
				{
					num += pawn.GetStatValue(StatDefOf.MeleeDodgeChanceIndoorsLitOffset, true, -1);
				}
			}
			return num;
		}

		// Token: 0x06013B06 RID: 80646 RVA: 0x005D995C File Offset: 0x005D7B5C
		private bool IsTargetImmobile(LocalTargetInfo target)
		{
			Thing thing = target.Thing;
			Pawn pawn = thing as Pawn;
			return thing.def.category != ThingCategory.Pawn || pawn.Downed || pawn.GetPosture() > PawnPosture.Standing;
		}

		// Token: 0x06013B07 RID: 80647
		protected abstract DamageWorker.DamageResult ApplyMeleeDamageToTarget(LocalTargetInfo target);

		// Token: 0x06013B08 RID: 80648 RVA: 0x005D9998 File Offset: 0x005D7B98
		private bool CanApplyMeleeSlaveSuppression(Pawn targetPawn)
		{
			return this.CasterPawn != null && this.CasterPawn.IsColonist && !this.CasterPawn.IsSlave && targetPawn != null && targetPawn.IsSlaveOfColony && targetPawn.health.capacities.CanBeAwake && !SlaveRebellionUtility.IsRebelling(targetPawn);
		}

		// Token: 0x06013B09 RID: 80649 RVA: 0x005D99EF File Offset: 0x005D7BEF
		private void ApplyMeleeSlaveSuppression(Pawn targetPawn, float damageDealt)
		{
			if (this.CanApplyMeleeSlaveSuppression(targetPawn))
			{
				SlaveRebellionUtility.IncrementMeleeSuppression(this.CasterPawn, targetPawn, damageDealt);
			}
		}

		// Token: 0x06013B0A RID: 80650 RVA: 0x005D9A08 File Offset: 0x005D7C08
		private SoundDef SoundHitPawn()
		{
			if (base.EquipmentSource != null && !base.EquipmentSource.def.meleeHitSound.NullOrUndefined())
			{
				return base.EquipmentSource.def.meleeHitSound;
			}
			if (this.tool != null && !this.tool.soundMeleeHit.NullOrUndefined())
			{
				return this.tool.soundMeleeHit;
			}
			if (base.EquipmentSource != null && base.EquipmentSource.Stuff != null)
			{
				if (this.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp;
					}
				}
				else if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt;
				}
			}
			if (this.CasterPawn != null && !this.CasterPawn.def.race.soundMeleeHitPawn.NullOrUndefined())
			{
				return this.CasterPawn.def.race.soundMeleeHitPawn;
			}
			return SoundDefOf.Pawn_Melee_Punch_HitPawn;
		}

		// Token: 0x06013B0B RID: 80651 RVA: 0x005D9B40 File Offset: 0x005D7D40
		private SoundDef SoundHitBuilding()
		{
			Building building = this.currentTarget.Thing as Building;
			if (building != null && !building.def.building.soundMeleeHitOverride.NullOrUndefined())
			{
				return building.def.building.soundMeleeHitOverride;
			}
			if (base.EquipmentSource != null && !base.EquipmentSource.def.meleeHitSound.NullOrUndefined())
			{
				return base.EquipmentSource.def.meleeHitSound;
			}
			if (this.tool != null && !this.tool.soundMeleeHit.NullOrUndefined())
			{
				return this.tool.soundMeleeHit;
			}
			if (base.EquipmentSource != null && base.EquipmentSource.Stuff != null)
			{
				if (this.verbProps.meleeDamageDef.armorCategory == DamageArmorCategoryDefOf.Sharp)
				{
					if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp.NullOrUndefined())
					{
						return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitSharp;
					}
				}
				else if (!base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt.NullOrUndefined())
				{
					return base.EquipmentSource.Stuff.stuffProps.soundMeleeHitBlunt;
				}
			}
			if (this.CasterPawn != null && !this.CasterPawn.def.race.soundMeleeHitBuilding.NullOrUndefined())
			{
				return this.CasterPawn.def.race.soundMeleeHitBuilding;
			}
			return SoundDefOf.MeleeHit_Unarmed;
		}

		// Token: 0x06013B0C RID: 80652 RVA: 0x005D9CB4 File Offset: 0x005D7EB4
		private SoundDef SoundMiss()
		{
			if (this.CasterPawn != null)
			{
				if (this.tool != null && !this.tool.soundMeleeMiss.NullOrUndefined())
				{
					return this.tool.soundMeleeMiss;
				}
				if (!this.CasterPawn.def.race.soundMeleeMiss.NullOrUndefined())
				{
					return this.CasterPawn.def.race.soundMeleeMiss;
				}
			}
			return SoundDefOf.Pawn_Melee_Punch_Miss;
		}

		// Token: 0x06013B0D RID: 80653 RVA: 0x005D9D26 File Offset: 0x005D7F26
		private SoundDef SoundDodge(Thing target)
		{
			if (target.def.race != null && target.def.race.soundMeleeDodge != null)
			{
				return target.def.race.soundMeleeDodge;
			}
			return this.SoundMiss();
		}

		// Token: 0x0400DA09 RID: 55817
		private const int TargetCooldown = 50;
	}
}
