using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D84 RID: 3460
	public class Verb_SpewFire : Verb
	{
		// Token: 0x06005853 RID: 22611 RVA: 0x001C7A58 File Offset: 0x001C5C58
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			if (base.EquipmentSource != null)
			{
				CompChangeableProjectile comp = base.EquipmentSource.GetComp<CompChangeableProjectile>();
				if (comp != null)
				{
					comp.Notify_ProjectileLaunched();
				}
				CompApparelReloadable comp2 = base.EquipmentSource.GetComp<CompApparelReloadable>();
				if (comp2 != null)
				{
					comp2.UsedOnce();
				}
			}
			IntVec3 position = this.caster.Position;
			float num = Mathf.Atan2((float)(-(float)(this.currentTarget.Cell.z - position.z)), (float)(this.currentTarget.Cell.x - position.x)) * 57.29578f;
			FloatRange value = new FloatRange(num - 13f, num + 13f);
			IntVec3 center = position;
			Map mapHeld = this.caster.MapHeld;
			float effectiveRange = this.EffectiveRange;
			DamageDef flame = DamageDefOf.Flame;
			Thing caster = this.caster;
			int damAmount = -1;
			float armorPenetration = -1f;
			SoundDef explosionSound = null;
			ThingDef weapon = null;
			ThingDef projectile = null;
			Thing intendedTarget = null;
			ThingDef filth_FlammableBile = ThingDefOf.Filth_FlammableBile;
			float postExplosionSpawnChance = 1f;
			int postExplosionSpawnThingCount = 1;
			FloatRange? affectedAngle = new FloatRange?(value);
			GenExplosion.DoExplosion(center, mapHeld, effectiveRange, flame, caster, damAmount, armorPenetration, explosionSound, weapon, projectile, intendedTarget, filth_FlammableBile, postExplosionSpawnChance, postExplosionSpawnThingCount, null, null, 255, false, null, 0f, 1, 1f, false, null, null, affectedAngle, false, 0.6f, 0f, false, null, 1f, null, null, null, null);
			base.AddEffecterToMaintain(EffecterDefOf.Fire_SpewShort.Spawn(this.caster.Position, this.currentTarget.Cell, this.caster.Map, 1f), this.caster.Position, this.currentTarget.Cell, 14, this.caster.Map);
			this.lastShotTick = Find.TickManager.TicksGame;
			return true;
		}

		// Token: 0x06005854 RID: 22612 RVA: 0x001C7C18 File Offset: 0x001C5E18
		public override bool Available()
		{
			if (!base.Available())
			{
				return false;
			}
			if (this.CasterIsPawn)
			{
				Pawn casterPawn = this.CasterPawn;
				if (casterPawn.Faction != Faction.OfPlayer && casterPawn.mindState.MeleeThreatStillThreat && casterPawn.mindState.meleeThreat.Position.AdjacentTo8WayOrInside(casterPawn.Position))
				{
					return false;
				}
			}
			return true;
		}
	}
}
