using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020008C5 RID: 2245
	[StaticConstructorOnStartup]
	public abstract class Projectile : ThingWithComps
	{
		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x060038C1 RID: 14529 RVA: 0x0012E441 File Offset: 0x0012C641
		public DamageDef DamageDef
		{
			get
			{
				return this.damageDefOverride ?? this.def.projectile.damageDef;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x060038C2 RID: 14530 RVA: 0x0012E460 File Offset: 0x0012C660
		public IEnumerable<ExtraDamage> ExtraDamages
		{
			get
			{
				IEnumerable<ExtraDamage> first = this.extraDamages;
				IEnumerable<ExtraDamage> enumerable = this.def.projectile.extraDamages;
				return first.Concat(enumerable ?? Enumerable.Empty<ExtraDamage>());
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x060038C3 RID: 14531 RVA: 0x0012E493 File Offset: 0x0012C693
		// (set) Token: 0x060038C4 RID: 14532 RVA: 0x0012E4C3 File Offset: 0x0012C6C3
		public ProjectileHitFlags HitFlags
		{
			get
			{
				if (this.def.projectile.alwaysFreeIntercept)
				{
					return ProjectileHitFlags.All;
				}
				if (this.def.projectile.flyOverhead)
				{
					return ProjectileHitFlags.None;
				}
				return this.desiredHitFlags;
			}
			set
			{
				this.desiredHitFlags = value;
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x060038C5 RID: 14533 RVA: 0x0012E4CC File Offset: 0x0012C6CC
		protected float StartingTicksToImpact
		{
			get
			{
				float num = (this.origin - this.destination).magnitude / this.def.projectile.SpeedTilesPerTick;
				if (num <= 0f)
				{
					num = 0.001f;
				}
				return num;
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x060038C6 RID: 14534 RVA: 0x0012E513 File Offset: 0x0012C713
		protected IntVec3 DestinationCell
		{
			get
			{
				return new IntVec3(this.destination);
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x060038C7 RID: 14535 RVA: 0x0012E520 File Offset: 0x0012C720
		public virtual Vector3 ExactPosition
		{
			get
			{
				Vector3 b = (this.destination - this.origin).Yto0() * this.DistanceCoveredFraction;
				return this.origin.Yto0() + b + Vector3.up * this.def.Altitude;
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x060038C8 RID: 14536 RVA: 0x0012E57A File Offset: 0x0012C77A
		protected float DistanceCoveredFraction
		{
			get
			{
				return Mathf.Clamp01(1f - (float)this.ticksToImpact / this.StartingTicksToImpact);
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x060038C9 RID: 14537 RVA: 0x0012E595 File Offset: 0x0012C795
		protected float DistanceCoveredFractionArc
		{
			get
			{
				return Mathf.Clamp01(1f - (float)(this.landed ? this.lifetime : this.ticksToImpact) / this.StartingTicksToImpact);
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x060038CA RID: 14538 RVA: 0x0012E5C0 File Offset: 0x0012C7C0
		public virtual Quaternion ExactRotation
		{
			get
			{
				return Quaternion.LookRotation((this.destination - this.origin).Yto0());
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x060038CB RID: 14539 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool AnimalsFleeImpact
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x060038CC RID: 14540 RVA: 0x0012E5DD File Offset: 0x0012C7DD
		public override Vector3 DrawPos
		{
			get
			{
				return this.ExactPosition;
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x060038CD RID: 14541 RVA: 0x0012E5E5 File Offset: 0x0012C7E5
		public virtual Material DrawMat
		{
			get
			{
				return this.def.graphic.MatSingleFor(this);
			}
		}

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x060038CE RID: 14542 RVA: 0x0012E5F8 File Offset: 0x0012C7F8
		public virtual int DamageAmount
		{
			get
			{
				return this.def.projectile.GetDamageAmount(this.equipment, null);
			}
		}

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x060038CF RID: 14543 RVA: 0x0012E611 File Offset: 0x0012C811
		public virtual float ArmorPenetration
		{
			get
			{
				return this.def.projectile.GetArmorPenetration(this.equipment, null);
			}
		}

		// Token: 0x17000A78 RID: 2680
		// (get) Token: 0x060038D0 RID: 14544 RVA: 0x0012E62A File Offset: 0x0012C82A
		public ThingDef EquipmentDef
		{
			get
			{
				return this.equipmentDef;
			}
		}

		// Token: 0x17000A79 RID: 2681
		// (get) Token: 0x060038D1 RID: 14545 RVA: 0x0012E632 File Offset: 0x0012C832
		public Thing Launcher
		{
			get
			{
				return this.launcher;
			}
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x060038D2 RID: 14546 RVA: 0x0012E63A File Offset: 0x0012C83A
		public override int UpdateRateTicks
		{
			get
			{
				if (base.Spawned && Find.CurrentMap == base.Map && Find.CameraDriver.InViewOf(this))
				{
					return 1;
				}
				return 15;
			}
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x0012E664 File Offset: 0x0012C864
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Vector3>(ref this.origin, "origin", default(Vector3), false);
			Scribe_Values.Look<Vector3>(ref this.destination, "destination", default(Vector3), false);
			Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
			Scribe_TargetInfo.Look(ref this.usedTarget, "usedTarget");
			Scribe_TargetInfo.Look(ref this.intendedTarget, "intendedTarget");
			Scribe_References.Look<Thing>(ref this.launcher, "launcher", false);
			Scribe_References.Look<Thing>(ref this.equipment, "equipment", false);
			Scribe_Defs.Look<ThingDef>(ref this.equipmentDef, "equipmentDef");
			Scribe_Defs.Look<ThingDef>(ref this.targetCoverDef, "targetCoverDef");
			Scribe_Values.Look<ProjectileHitFlags>(ref this.desiredHitFlags, "desiredHitFlags", ProjectileHitFlags.All, false);
			Scribe_Values.Look<bool>(ref this.preventFriendlyFire, "preventFriendlyFire", false, false);
			Scribe_Values.Look<bool>(ref this.landed, "landed", false, false);
			Scribe_Values.Look<int>(ref this.lifetime, "lifetime", 0, false);
			Scribe_Values.Look<QualityCategory>(ref this.equipmentQuality, "equipmentQuality", QualityCategory.Normal, false);
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x0012E77C File Offset: 0x0012C97C
		public void Launch(Thing launcher, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, bool preventFriendlyFire = false, Thing equipment = null)
		{
			this.Launch(launcher, base.Position.ToVector3Shifted(), usedTarget, intendedTarget, hitFlags, preventFriendlyFire, equipment, null);
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x0012E7A8 File Offset: 0x0012C9A8
		public virtual void Launch(Thing launcher, Vector3 origin, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, bool preventFriendlyFire = false, Thing equipment = null, ThingDef targetCoverDef = null)
		{
			this.launcher = launcher;
			this.origin = origin;
			this.usedTarget = usedTarget;
			this.intendedTarget = intendedTarget;
			this.targetCoverDef = targetCoverDef;
			this.preventFriendlyFire = preventFriendlyFire;
			this.HitFlags = hitFlags;
			this.stoppingPower = this.def.projectile.stoppingPower;
			if (this.stoppingPower == 0f && this.def.projectile.damageDef != null)
			{
				this.stoppingPower = this.def.projectile.damageDef.defaultStoppingPower;
			}
			if (equipment != null)
			{
				this.equipment = equipment;
				this.equipmentDef = equipment.def;
				equipment.TryGetQuality(out this.equipmentQuality);
				CompUniqueWeapon compUniqueWeapon;
				if (!equipment.TryGetComp(out compUniqueWeapon))
				{
					goto IL_115;
				}
				using (List<WeaponTraitDef>.Enumerator enumerator = compUniqueWeapon.TraitsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						WeaponTraitDef weaponTraitDef = enumerator.Current;
						if (!Mathf.Approximately(weaponTraitDef.additionalStoppingPower, 0f))
						{
							this.stoppingPower += weaponTraitDef.additionalStoppingPower;
						}
					}
					goto IL_115;
				}
			}
			this.equipmentDef = null;
			IL_115:
			this.destination = usedTarget.Cell.ToVector3Shifted() + Gen.RandomHorizontalVector(0.3f);
			this.ticksToImpact = Mathf.CeilToInt(this.StartingTicksToImpact);
			if (this.ticksToImpact < 1)
			{
				this.ticksToImpact = 1;
			}
			this.lifetime = this.ticksToImpact;
			if (!this.def.projectile.soundAmbient.NullOrUndefined())
			{
				this.ambientSustainer = this.def.projectile.soundAmbient.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
			}
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x0012E96C File Offset: 0x0012CB6C
		protected override void Tick()
		{
			base.Tick();
			if (this.ticksToImpact == 60 && Find.TickManager.CurTimeSpeed == TimeSpeed.Normal && this.def.projectile.soundImpactAnticipate != null)
			{
				this.def.projectile.soundImpactAnticipate.PlayOneShot(this);
			}
			if (this.ambientSustainer != null)
			{
				this.ambientSustainer.Maintain();
			}
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x0012E9D8 File Offset: 0x0012CBD8
		protected override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			this.lifetime -= delta;
			if (this.landed)
			{
				return;
			}
			Vector3 exactPosition = this.ExactPosition;
			this.ticksToImpact -= delta;
			if (!this.ExactPosition.InBounds(base.Map))
			{
				this.ticksToImpact += delta;
				base.Position = this.ExactPosition.ToIntVec3();
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Vector3 exactPosition2 = this.ExactPosition;
			if (this.CheckForFreeInterceptBetween(exactPosition, exactPosition2))
			{
				return;
			}
			base.Position = this.ExactPosition.ToIntVec3();
			if (this.ticksToImpact <= 0)
			{
				if (this.DestinationCell.InBounds(base.Map))
				{
					base.Position = this.DestinationCell;
				}
				this.ImpactSomething();
				return;
			}
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x0012EAA4 File Offset: 0x0012CCA4
		private bool CheckForFreeInterceptBetween(Vector3 lastExactPos, Vector3 newExactPos)
		{
			if (lastExactPos == newExactPos)
			{
				return false;
			}
			List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ProjectileInterceptor);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].TryGetComp<CompProjectileInterceptor>().CheckIntercept(this, lastExactPos, newExactPos))
				{
					this.Impact(null, true);
					return true;
				}
			}
			IntVec3 intVec = lastExactPos.ToIntVec3();
			IntVec3 intVec2 = newExactPos.ToIntVec3();
			if (intVec2 == intVec)
			{
				return false;
			}
			if (!intVec.InBounds(base.Map) || !intVec2.InBounds(base.Map))
			{
				return false;
			}
			if (intVec2.AdjacentToCardinal(intVec))
			{
				return this.CheckForFreeIntercept(intVec2);
			}
			if (VerbUtility.InterceptChanceFactorFromDistance(this.origin, intVec2) <= 0f)
			{
				return false;
			}
			Vector3 vector = lastExactPos;
			Vector3 v = newExactPos - lastExactPos;
			Vector3 b = v.normalized * 0.2f;
			int num = (int)(v.MagnitudeHorizontal() / 0.2f);
			Projectile.checkedCells.Clear();
			int num2 = 0;
			for (;;)
			{
				vector += b;
				IntVec3 intVec3 = vector.ToIntVec3();
				if (!Projectile.checkedCells.Contains(intVec3))
				{
					if (this.CheckForFreeIntercept(intVec3))
					{
						break;
					}
					Projectile.checkedCells.Add(intVec3);
				}
				num2++;
				if (num2 > num)
				{
					return false;
				}
				if (intVec3 == intVec2)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x0012EBF0 File Offset: 0x0012CDF0
		private bool CheckForFreeIntercept(IntVec3 c)
		{
			if (this.destination.ToIntVec3() == c)
			{
				return false;
			}
			float num = VerbUtility.InterceptChanceFactorFromDistance(this.origin, c);
			if (num <= 0f)
			{
				return false;
			}
			bool flag = false;
			List<Thing> thingList = c.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (this.CanHit(thing))
				{
					bool flag2 = false;
					if (thing.def.Fillage == FillCategory.Full)
					{
						Building_Door building_Door = thing as Building_Door;
						if (building_Door == null || !building_Door.Open)
						{
							this.ThrowDebugText("int-wall", c);
							this.Impact(thing, false);
							return true;
						}
						flag2 = true;
					}
					float num2 = 0f;
					Pawn pawn = thing as Pawn;
					if (pawn != null)
					{
						num2 = 0.4f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
						if (pawn.GetPosture() != PawnPosture.Standing)
						{
							num2 *= 0.1f;
						}
						if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
						{
							if (this.preventFriendlyFire)
							{
								num2 = 0f;
								this.ThrowDebugText("ff-miss", c);
							}
							else
							{
								num2 *= Find.Storyteller.difficulty.friendlyFireChanceFactor;
							}
						}
					}
					else if (thing.def.fillPercent > 0.2f)
					{
						if (flag2)
						{
							num2 = 0.05f;
						}
						else if (this.DestinationCell.AdjacentTo8Way(c))
						{
							num2 = thing.def.fillPercent * 1f;
						}
						else
						{
							num2 = thing.def.fillPercent * 0.15f;
						}
					}
					num2 *= num;
					if (num2 > 1E-05f)
					{
						if (Rand.Chance(num2))
						{
							this.ThrowDebugText("int-" + num2.ToStringPercent(), c);
							this.Impact(thing, false);
							return true;
						}
						flag = true;
						this.ThrowDebugText(num2.ToStringPercent(), c);
					}
				}
			}
			if (!flag)
			{
				this.ThrowDebugText("o", c);
			}
			return false;
		}

		// Token: 0x060038DA RID: 14554 RVA: 0x0012EE1A File Offset: 0x0012D01A
		private void ThrowDebugText(string text, IntVec3 c)
		{
			if (DebugViewSettings.drawShooting)
			{
				MoteMaker.ThrowText(c.ToVector3Shifted(), base.Map, text, -1f);
			}
		}

		// Token: 0x060038DB RID: 14555 RVA: 0x0012EE3C File Offset: 0x0012D03C
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			float num = this.ArcHeightFactor * GenMath.InverseParabola(this.DistanceCoveredFractionArc);
			Vector3 vector = drawLoc + new Vector3(0f, 0f, 1f) * num;
			if (this.def.projectile.shadowSize > 0f)
			{
				this.DrawShadow(drawLoc, num);
			}
			Quaternion rotation = this.ExactRotation;
			if (this.def.projectile.spinRate != 0f)
			{
				float num2 = 60f / this.def.projectile.spinRate;
				rotation = Quaternion.AngleAxis((float)Find.TickManager.TicksGame % num2 / num2 * 360f, Vector3.up);
			}
			if (this.def.projectile.useGraphicClass)
			{
				this.Graphic.Draw(vector, base.Rotation, this, rotation.eulerAngles.y);
			}
			else
			{
				Graphics.DrawMesh(MeshPool.GridPlane(this.def.graphicData.drawSize), vector, rotation, this.DrawMat, 0);
			}
			base.Comps_PostDraw();
		}

		// Token: 0x17000A7B RID: 2683
		// (get) Token: 0x060038DC RID: 14556 RVA: 0x0012EF54 File Offset: 0x0012D154
		private float ArcHeightFactor
		{
			get
			{
				float num = this.def.projectile.arcHeightFactor;
				float num2 = (this.destination - this.origin).MagnitudeHorizontalSquared();
				if (num * num > num2 * 0.2f * 0.2f)
				{
					num = Mathf.Sqrt(num2) * 0.2f;
				}
				return num;
			}
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x0012EFAC File Offset: 0x0012D1AC
		protected bool CanHit(Thing thing)
		{
			if (!thing.Spawned)
			{
				return false;
			}
			if (thing == this.launcher)
			{
				return false;
			}
			ProjectileHitFlags hitFlags = this.HitFlags;
			if (hitFlags == ProjectileHitFlags.None)
			{
				return false;
			}
			if (thing.Map != base.Map)
			{
				return false;
			}
			if (CoverUtility.ThingCovered(thing, base.Map))
			{
				return false;
			}
			if (thing == this.intendedTarget && (hitFlags & ProjectileHitFlags.IntendedTarget) != ProjectileHitFlags.None)
			{
				return true;
			}
			if (thing != this.intendedTarget)
			{
				if (thing is Pawn)
				{
					if ((hitFlags & ProjectileHitFlags.NonTargetPawns) != ProjectileHitFlags.None)
					{
						return true;
					}
				}
				else if ((hitFlags & ProjectileHitFlags.NonTargetWorld) != ProjectileHitFlags.None)
				{
					return true;
				}
			}
			return thing == this.intendedTarget && thing.def.Fillage == FillCategory.Full;
		}

		// Token: 0x060038DE RID: 14558 RVA: 0x0012F064 File Offset: 0x0012D264
		protected virtual void ImpactSomething()
		{
			if (this.def.projectile.flyOverhead)
			{
				RoofDef roofDef = base.Map.roofGrid.RoofAt(base.Position);
				if (roofDef != null)
				{
					if (roofDef.isThickRoof)
					{
						this.ThrowDebugText("hit-thick-roof", base.Position);
						if (!this.def.projectile.soundHitThickRoof.NullOrUndefined())
						{
							this.def.projectile.soundHitThickRoof.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
						}
						this.Destroy(DestroyMode.Vanish);
						return;
					}
					if (base.Position.GetEdifice(base.Map) == null || base.Position.GetEdifice(base.Map).def.Fillage != FillCategory.Full)
					{
						RoofCollapserImmediate.DropRoofInCells(base.Position, base.Map, null);
					}
				}
			}
			if (!this.usedTarget.HasThing || !this.CanHit(this.usedTarget.Thing))
			{
				List<Thing> list = VerbUtility.ThingsToHit(base.Position, base.Map, new Func<Thing, bool>(this.CanHit));
				list.Shuffle<Thing>();
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					Pawn pawn = thing as Pawn;
					float num;
					if (pawn != null)
					{
						num = 0.5f * Mathf.Clamp(pawn.BodySize, 0.1f, 2f);
						if (pawn.GetPosture() != PawnPosture.Standing && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25f)
						{
							num *= 0.5f;
						}
						if (this.launcher != null && pawn.Faction != null && this.launcher.Faction != null && !pawn.Faction.HostileTo(this.launcher.Faction))
						{
							num *= VerbUtility.InterceptChanceFactorFromDistance(this.origin, base.Position);
						}
					}
					else
					{
						num = 1.5f * thing.def.fillPercent;
					}
					if (Rand.Chance(num))
					{
						this.ThrowDebugText("hit-" + num.ToStringPercent(), base.Position);
						this.Impact(thing, false);
						return;
					}
					this.ThrowDebugText("miss-" + num.ToStringPercent(), base.Position);
				}
				this.Impact(null, false);
				return;
			}
			Pawn pawn2 = this.usedTarget.Thing as Pawn;
			if (pawn2 != null && pawn2.GetPosture() != PawnPosture.Standing && (this.origin - this.destination).MagnitudeHorizontalSquared() >= 20.25f && !Rand.Chance(0.5f))
			{
				this.ThrowDebugText("miss-laying", base.Position);
				this.Impact(null, false);
				return;
			}
			this.Impact(this.usedTarget.Thing, false);
		}

		// Token: 0x060038DF RID: 14559 RVA: 0x0012F33C File Offset: 0x0012D53C
		protected virtual void Impact(Thing hitThing, bool blockedByShield = false)
		{
			GenClamor.DoClamor(this, 12f, ClamorDefOf.Impact);
			if (!blockedByShield && this.def.projectile.landedEffecter != null)
			{
				this.def.projectile.landedEffecter.Spawn(base.Position, base.Map, 1f).Cleanup();
			}
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x060038E0 RID: 14560 RVA: 0x0012F3A0 File Offset: 0x0012D5A0
		private void DrawShadow(Vector3 drawLoc, float height)
		{
			if (Projectile.shadowMaterial == null)
			{
				return;
			}
			float num = this.def.projectile.shadowSize * Mathf.Lerp(1f, 0.6f, height);
			Vector3 s = new Vector3(num, 1f, num);
			Vector3 b = new Vector3(0f, -0.01f, 0f);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawLoc + b, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, Projectile.shadowMaterial, 0);
		}

		// Token: 0x040029FC RID: 10748
		private static readonly Material shadowMaterial = MaterialPool.MatFrom("Things/Skyfaller/SkyfallerShadowCircle", ShaderDatabase.Transparent);

		// Token: 0x040029FD RID: 10749
		protected Vector3 origin;

		// Token: 0x040029FE RID: 10750
		protected Vector3 destination;

		// Token: 0x040029FF RID: 10751
		public LocalTargetInfo usedTarget;

		// Token: 0x04002A00 RID: 10752
		public LocalTargetInfo intendedTarget;

		// Token: 0x04002A01 RID: 10753
		protected Thing equipment;

		// Token: 0x04002A02 RID: 10754
		protected ThingDef equipmentDef;

		// Token: 0x04002A03 RID: 10755
		protected Thing launcher;

		// Token: 0x04002A04 RID: 10756
		protected ThingDef targetCoverDef;

		// Token: 0x04002A05 RID: 10757
		private ProjectileHitFlags desiredHitFlags = ProjectileHitFlags.All;

		// Token: 0x04002A06 RID: 10758
		protected bool preventFriendlyFire;

		// Token: 0x04002A07 RID: 10759
		protected int lifetime;

		// Token: 0x04002A08 RID: 10760
		protected QualityCategory equipmentQuality = QualityCategory.Normal;

		// Token: 0x04002A09 RID: 10761
		public float stoppingPower;

		// Token: 0x04002A0A RID: 10762
		public DamageDef damageDefOverride;

		// Token: 0x04002A0B RID: 10763
		public List<ExtraDamage> extraDamages = new List<ExtraDamage>();

		// Token: 0x04002A0C RID: 10764
		protected bool landed;

		// Token: 0x04002A0D RID: 10765
		protected int ticksToImpact;

		// Token: 0x04002A0E RID: 10766
		private Sustainer ambientSustainer;

		// Token: 0x04002A0F RID: 10767
		private static List<IntVec3> checkedCells = new List<IntVec3>();
	}
}
