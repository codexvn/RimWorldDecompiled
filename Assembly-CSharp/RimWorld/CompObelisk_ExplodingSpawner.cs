using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002D89 RID: 11657
	public abstract class CompObelisk_ExplodingSpawner : CompObelisk
	{
		// Token: 0x170029BD RID: 10685
		// (get) Token: 0x0601063B RID: 67131 RVA: 0x004BAA9F File Offset: 0x004B8C9F
		protected virtual IntRange ExplodeDelayTicks
		{
			get
			{
				return new IntRange(120, 180);
			}
		}

		// Token: 0x170029BE RID: 10686
		// (get) Token: 0x0601063C RID: 67132 RVA: 0x004BAAAD File Offset: 0x004B8CAD
		protected virtual IntRange SpawnIntervalTicks
		{
			get
			{
				return new IntRange(6, 60);
			}
		}

		// Token: 0x170029BF RID: 10687
		// (get) Token: 0x0601063D RID: 67133 RVA: 0x004BAAB8 File Offset: 0x004B8CB8
		protected Lord Lord
		{
			get
			{
				return this.lord ?? LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_AssaultColony(Faction.OfEntities, false, false, false, false, false, false, false), this.parent.Map, null);
			}
		}

		// Token: 0x0601063E RID: 67134 RVA: 0x004BAAF8 File Offset: 0x004B8CF8
		protected void Explode()
		{
			Map map = this.parent.Map;
			this.parent.Destroy(DestroyMode.KillFinalize);
			IntVec3 position = this.parent.Position;
			Map map2 = map;
			float radius = 4.9f;
			DamageDef vaporize = DamageDefOf.Vaporize;
			Thing parent = this.parent;
			int damAmount = -1;
			float armorPenetration = -1f;
			SoundDef explosionSound = null;
			ThingDef weapon = null;
			ThingDef projectile = null;
			Thing intendedTarget = null;
			ThingDef postExplosionSpawnThingDef = null;
			float postExplosionSpawnChance = 0f;
			int postExplosionSpawnThingCount = 1;
			List<Thing> ignoredThings = this.thingsIgnoredByExplosion;
			GenExplosion.DoExplosion(position, map2, radius, vaporize, parent, damAmount, armorPenetration, explosionSound, weapon, projectile, intendedTarget, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, null, null, 255, false, null, 0f, 1, 0f, false, null, ignoredThings, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}

		// Token: 0x0601063F RID: 67135 RVA: 0x004BABA5 File Offset: 0x004B8DA5
		public override void Notify_KilledLeavingsLeft(List<Thing> leavings)
		{
			base.Notify_KilledLeavingsLeft(leavings);
			if (this.thingsIgnoredByExplosion == null)
			{
				this.thingsIgnoredByExplosion = new List<Thing>();
			}
			else
			{
				this.thingsIgnoredByExplosion.Clear();
			}
			this.thingsIgnoredByExplosion.AddRange(leavings);
		}

		// Token: 0x06010640 RID: 67136 RVA: 0x004BABDC File Offset: 0x004B8DDC
		public override void CompTick()
		{
			base.CompTick();
			if (this.activated && this.explodeTick > 0)
			{
				Effecter effecter = this.effecterExplodeWarmup;
				if (effecter != null)
				{
					effecter.EffectTick(this.parent, new TargetInfo(this.parent.Position, this.parent.Map, false));
				}
				if (Find.TickManager.TicksGame > this.explodeTick)
				{
					Effecter effecter2 = this.effecterExplodeWarmup;
					if (effecter2 != null)
					{
						effecter2.Cleanup();
					}
					this.effecterExplodeWarmup = null;
					this.Explode();
				}
			}
		}

		// Token: 0x06010641 RID: 67137 RVA: 0x004BAC68 File Offset: 0x004B8E68
		protected void PrepareExplosion()
		{
			this.effecterExplodeWarmup = EffecterDefOf.ObeliskExplosionWarmup.Spawn();
			this.effecterExplodeWarmup.Trigger(this.parent, new TargetInfo(this.parent.Position, this.parent.Map, false), -1);
			this.explodeTick = Find.TickManager.TicksGame + this.ExplodeDelayTicks.RandomInRange;
		}

		// Token: 0x06010642 RID: 67138 RVA: 0x004BACD8 File Offset: 0x004B8ED8
		public override void OnActivityActivated()
		{
			base.OnActivityActivated();
			if (this.parent.Map != null)
			{
				this.pointsRemaining = (this.totalCombatPoints = StorytellerUtility.DefaultThreatPointsNow(this.parent.Map));
			}
		}

		// Token: 0x06010643 RID: 67139 RVA: 0x004BAD18 File Offset: 0x004B8F18
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.pointsRemaining, "pointsRemaining", 0f, false);
			Scribe_Values.Look<float>(ref this.totalCombatPoints, "totalCombatPoints", 0f, false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_Values.Look<int>(ref this.nextSpawnTick, "nextSpawnTick", 0, false);
			Scribe_Values.Look<int>(ref this.explodeTick, "explodeTick", 0, false);
		}

		// Token: 0x0400AB06 RID: 43782
		protected int explodeTick = -99999;

		// Token: 0x0400AB07 RID: 43783
		protected float pointsRemaining;

		// Token: 0x0400AB08 RID: 43784
		protected float totalCombatPoints;

		// Token: 0x0400AB09 RID: 43785
		private Lord lord;

		// Token: 0x0400AB0A RID: 43786
		protected int nextSpawnTick = -99999;

		// Token: 0x0400AB0B RID: 43787
		private Effecter effecterExplodeWarmup;

		// Token: 0x0400AB0C RID: 43788
		private List<Thing> thingsIgnoredByExplosion;
	}
}
