using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A8D RID: 10893
	[StaticConstructorOnStartup]
	public class Bombardment : OrbitalStrike
	{
		// Token: 0x0600F27F RID: 62079 RVA: 0x00466AA1 File Offset: 0x00464CA1
		public override void SpawnSetup(Map map, bool respawningAfterReload)
		{
			base.SpawnSetup(map, respawningAfterReload);
			if (!respawningAfterReload)
			{
				this.GetNextExplosionCell();
			}
		}

		// Token: 0x0600F280 RID: 62080 RVA: 0x00466AB4 File Offset: 0x00464CB4
		public override void StartStrike()
		{
			this.duration = this.bombIntervalTicks * this.explosionCount;
			base.StartStrike();
		}

		// Token: 0x0600F281 RID: 62081 RVA: 0x00466AD0 File Offset: 0x00464CD0
		protected override void Tick()
		{
			if (base.Destroyed)
			{
				return;
			}
			if (this.warmupTicks > 0)
			{
				this.warmupTicks--;
				if (this.warmupTicks <= 0)
				{
					this.StartStrike();
				}
			}
			else
			{
				base.Tick();
				if (base.TicksLeft > 0 && this.IsHashIntervalTick(20))
				{
					this.StartRandomFire();
				}
			}
			this.EffectTick();
		}

		// Token: 0x0600F282 RID: 62082 RVA: 0x00466B34 File Offset: 0x00464D34
		private void EffectTick()
		{
			if (!this.nextExplosionCell.IsValid)
			{
				this.ticksToNextEffect = this.warmupTicks - this.bombIntervalTicks;
				this.GetNextExplosionCell();
			}
			this.ticksToNextEffect--;
			if (this.ticksToNextEffect <= 0 && base.TicksLeft >= this.bombIntervalTicks)
			{
				SoundDefOf.Bombardment_PreImpact.PlayOneShot(new TargetInfo(this.nextExplosionCell, base.Map, false));
				this.projectiles.Add(new Bombardment.BombardmentProjectile(60, this.nextExplosionCell));
				this.ticksToNextEffect = this.bombIntervalTicks;
				this.GetNextExplosionCell();
			}
			for (int i = this.projectiles.Count - 1; i >= 0; i--)
			{
				this.projectiles[i].Tick();
				if (this.projectiles[i].LifeTime <= 0)
				{
					this.TryDoExplosion(this.projectiles[i]);
					this.projectiles.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600F283 RID: 62083 RVA: 0x00466C34 File Offset: 0x00464E34
		private void TryDoExplosion(Bombardment.BombardmentProjectile proj)
		{
			List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ProjectileInterceptor);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].TryGetComp<CompProjectileInterceptor>().CheckBombardmentIntercept(this, proj))
				{
					return;
				}
			}
			IntVec3 targetCell = proj.targetCell;
			Map map = base.Map;
			float randomInRange = this.explosionRadiusRange.RandomInRange;
			DamageDef bomb = DamageDefOf.Bomb;
			Thing instigator = this.instigator;
			int damAmount = -1;
			float armorPenetration = -1f;
			SoundDef explosionSound = null;
			ThingDef def = this.def;
			GenExplosion.DoExplosion(targetCell, map, randomInRange, bomb, instigator, damAmount, armorPenetration, explosionSound, this.weaponDef, def, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
		}

		// Token: 0x0600F284 RID: 62084 RVA: 0x00466D10 File Offset: 0x00464F10
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			if (this.projectiles.NullOrEmpty<Bombardment.BombardmentProjectile>())
			{
				return;
			}
			for (int i = 0; i < this.projectiles.Count; i++)
			{
				this.projectiles[i].Draw(Bombardment.ProjectileMaterial);
			}
		}

		// Token: 0x0600F285 RID: 62085 RVA: 0x00466D60 File Offset: 0x00464F60
		private void StartRandomFire()
		{
			IntVec3 intVec = (from x in GenRadial.RadialCellsAround(base.Position, (float)this.randomFireRadius, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => Bombardment.DistanceChanceFactor.Evaluate(x.DistanceTo(base.Position)));
			List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ProjectileInterceptor);
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].TryGetComp<CompProjectileInterceptor>().BombardmentCanStartFireAt(this, intVec))
				{
					return;
				}
			}
			RoofDef roof = intVec.GetRoof(base.Map);
			if (roof != null && roof.isThickRoof)
			{
				return;
			}
			FireUtility.TryStartFireIn(intVec, base.Map, Rand.Range(0.1f, 0.925f), this.instigator, null);
		}

		// Token: 0x0600F286 RID: 62086 RVA: 0x00466E19 File Offset: 0x00465019
		private void GetNextExplosionCell()
		{
			this.nextExplosionCell = (from x in GenRadial.RadialCellsAround(base.Position, this.impactAreaRadius, true)
			where x.InBounds(base.Map)
			select x).RandomElementByWeight((IntVec3 x) => Bombardment.DistanceChanceFactor.Evaluate(x.DistanceTo(base.Position) / this.impactAreaRadius));
		}

		// Token: 0x0600F287 RID: 62087 RVA: 0x00466E58 File Offset: 0x00465058
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.impactAreaRadius, "impactAreaRadius", 15f, false);
			Scribe_Values.Look<FloatRange>(ref this.explosionRadiusRange, "explosionRadiusRange", new FloatRange(6f, 8f), false);
			Scribe_Values.Look<int>(ref this.randomFireRadius, "randomFireRadius", 25, false);
			Scribe_Values.Look<int>(ref this.bombIntervalTicks, "bombIntervalTicks", 18, false);
			Scribe_Values.Look<int>(ref this.warmupTicks, "warmupTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToNextEffect, "ticksToNextEffect", 0, false);
			Scribe_Values.Look<IntVec3>(ref this.nextExplosionCell, "nextExplosionCell", default(IntVec3), false);
			Scribe_Collections.Look<Bombardment.BombardmentProjectile>(ref this.projectiles, "projectiles", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (!this.nextExplosionCell.IsValid)
				{
					this.GetNextExplosionCell();
				}
				if (this.projectiles == null)
				{
					this.projectiles = new List<Bombardment.BombardmentProjectile>();
				}
			}
		}

		// Token: 0x04009FC2 RID: 40898
		public float impactAreaRadius = 15f;

		// Token: 0x04009FC3 RID: 40899
		public FloatRange explosionRadiusRange = new FloatRange(6f, 8f);

		// Token: 0x04009FC4 RID: 40900
		public int randomFireRadius = 25;

		// Token: 0x04009FC5 RID: 40901
		public int bombIntervalTicks = 18;

		// Token: 0x04009FC6 RID: 40902
		public int warmupTicks = 60;

		// Token: 0x04009FC7 RID: 40903
		public int explosionCount = 30;

		// Token: 0x04009FC8 RID: 40904
		private int ticksToNextEffect;

		// Token: 0x04009FC9 RID: 40905
		private IntVec3 nextExplosionCell = IntVec3.Invalid;

		// Token: 0x04009FCA RID: 40906
		private List<Bombardment.BombardmentProjectile> projectiles = new List<Bombardment.BombardmentProjectile>();

		// Token: 0x04009FCB RID: 40907
		public const int EffectiveAreaRadius = 23;

		// Token: 0x04009FCC RID: 40908
		private const int StartRandomFireEveryTicks = 20;

		// Token: 0x04009FCD RID: 40909
		private const int EffectDuration = 60;

		// Token: 0x04009FCE RID: 40910
		private static readonly Material ProjectileMaterial = MaterialPool.MatFrom("Things/Projectile/Bullet_Big", ShaderDatabase.Transparent, Color.white);

		// Token: 0x04009FCF RID: 40911
		public static readonly SimpleCurve DistanceChanceFactor = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.1f),
				true
			}
		};

		// Token: 0x02002A8E RID: 10894
		public class BombardmentProjectile : IExposable
		{
			// Token: 0x170025E3 RID: 9699
			// (get) Token: 0x0600F28E RID: 62094 RVA: 0x0046704B File Offset: 0x0046524B
			public int LifeTime
			{
				get
				{
					return this.lifeTime;
				}
			}

			// Token: 0x0600F28F RID: 62095 RVA: 0x000024DF File Offset: 0x000006DF
			public BombardmentProjectile()
			{
			}

			// Token: 0x0600F290 RID: 62096 RVA: 0x00467053 File Offset: 0x00465253
			public BombardmentProjectile(int lifeTime, IntVec3 targetCell)
			{
				this.lifeTime = lifeTime;
				this.maxLifeTime = lifeTime;
				this.targetCell = targetCell;
			}

			// Token: 0x0600F291 RID: 62097 RVA: 0x00467070 File Offset: 0x00465270
			public void Tick()
			{
				this.lifeTime--;
			}

			// Token: 0x0600F292 RID: 62098 RVA: 0x00467080 File Offset: 0x00465280
			public void Draw(Material material)
			{
				if (this.lifeTime > 0)
				{
					Vector3 pos = this.targetCell.ToVector3() + Vector3.forward * Mathf.Lerp(60f, 0f, 1f - (float)this.lifeTime / (float)this.maxLifeTime);
					pos.z += 1.25f;
					pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
					Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0f, 180f, 0f), new Vector3(2.5f, 1f, 2.5f));
					Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
				}
			}

			// Token: 0x0600F293 RID: 62099 RVA: 0x00467134 File Offset: 0x00465334
			public void ExposeData()
			{
				Scribe_Values.Look<int>(ref this.lifeTime, "lifeTime", 0, false);
				Scribe_Values.Look<int>(ref this.maxLifeTime, "maxLifeTime", 0, false);
				Scribe_Values.Look<IntVec3>(ref this.targetCell, "targetCell", default(IntVec3), false);
			}

			// Token: 0x04009FD0 RID: 40912
			private int lifeTime;

			// Token: 0x04009FD1 RID: 40913
			private int maxLifeTime;

			// Token: 0x04009FD2 RID: 40914
			public IntVec3 targetCell;

			// Token: 0x04009FD3 RID: 40915
			private const float StartZ = 60f;

			// Token: 0x04009FD4 RID: 40916
			private const float Scale = 2.5f;

			// Token: 0x04009FD5 RID: 40917
			private const float Angle = 180f;
		}
	}
}
