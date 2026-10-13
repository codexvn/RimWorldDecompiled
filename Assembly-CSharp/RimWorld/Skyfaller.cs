using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002B51 RID: 11089
	[StaticConstructorOnStartup]
	public class Skyfaller : ThingWithComps, IThingHolder, IRoofCollapseAlert
	{
		// Token: 0x170026E7 RID: 9959
		// (get) Token: 0x0600F7E9 RID: 63465 RVA: 0x00481612 File Offset: 0x0047F812
		public int LeaveMapAfterTicks
		{
			get
			{
				if (this.ticksToDiscard <= 0)
				{
					return 220;
				}
				return this.ticksToDiscard;
			}
		}

		// Token: 0x170026E8 RID: 9960
		// (get) Token: 0x0600F7EA RID: 63466 RVA: 0x00481629 File Offset: 0x0047F829
		// (set) Token: 0x0600F7EB RID: 63467 RVA: 0x00481631 File Offset: 0x0047F831
		public bool? OverrideFlightFlippedHorizontal { get; set; }

		// Token: 0x170026E9 RID: 9961
		// (get) Token: 0x0600F7EC RID: 63468 RVA: 0x0048163A File Offset: 0x0047F83A
		public CompSkyfallerRandomizeDirection RandomizeDirectionComp
		{
			get
			{
				return this.randomizeDirectionComp;
			}
		}

		// Token: 0x0600F7ED RID: 63469 RVA: 0x00481642 File Offset: 0x0047F842
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.randomizeDirectionComp = base.GetComp<CompSkyfallerRandomizeDirection>();
		}

		// Token: 0x170026EA RID: 9962
		// (get) Token: 0x0600F7EE RID: 63470 RVA: 0x00481658 File Offset: 0x0047F858
		public override Graphic Graphic
		{
			get
			{
				Thing thingForGraphic = this.GetThingForGraphic();
				if (this.def.skyfaller.fadeInTicks > 0 || this.def.skyfaller.fadeOutTicks > 0)
				{
					return this.def.graphicData.GraphicColoredFor(thingForGraphic);
				}
				if (thingForGraphic == this)
				{
					return base.Graphic;
				}
				return thingForGraphic.Graphic.ExtractInnerGraphicFor(thingForGraphic, null).GetShadowlessGraphic();
			}
		}

		// Token: 0x170026EB RID: 9963
		// (get) Token: 0x0600F7EF RID: 63471 RVA: 0x004816CC File Offset: 0x0047F8CC
		public override Vector3 DrawPos
		{
			get
			{
				switch (this.def.skyfaller.movementType)
				{
				case SkyfallerMovementType.Accelerate:
					return SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed, this.OverrideFlightFlippedHorizontal ?? this.def.skyfaller.flightFlippedHorizontally, this.randomizeDirectionComp);
				case SkyfallerMovementType.ConstantSpeed:
					return SkyfallerDrawPosUtility.DrawPos_ConstantSpeed(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed, this.OverrideFlightFlippedHorizontal ?? this.def.skyfaller.flightFlippedHorizontally, this.randomizeDirectionComp);
				case SkyfallerMovementType.Decelerate:
					return SkyfallerDrawPosUtility.DrawPos_Decelerate(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed, this.OverrideFlightFlippedHorizontal ?? this.def.skyfaller.flightFlippedHorizontally, this.randomizeDirectionComp);
				default:
					Log.ErrorOnce("SkyfallerMovementType not handled: " + this.def.skyfaller.movementType.ToString(), this.thingIDNumber ^ 1948576711);
					return SkyfallerDrawPosUtility.DrawPos_Accelerate(base.DrawPos, this.ticksToImpact, this.angle, this.CurrentSpeed, this.OverrideFlightFlippedHorizontal ?? this.def.skyfaller.flightFlippedHorizontally, this.randomizeDirectionComp);
				}
			}
		}

		// Token: 0x170026EC RID: 9964
		// (get) Token: 0x0600F7F0 RID: 63472 RVA: 0x0048186C File Offset: 0x0047FA6C
		// (set) Token: 0x0600F7F1 RID: 63473 RVA: 0x00481957 File Offset: 0x0047FB57
		public override Color DrawColor
		{
			get
			{
				if (this.def.skyfaller.fadeInTicks > 0 && this.ageTicks < this.def.skyfaller.fadeInTicks)
				{
					Color drawColor = base.DrawColor;
					drawColor.a *= Mathf.Lerp(0f, 1f, Mathf.Min((float)this.ageTicks / (float)this.def.skyfaller.fadeInTicks, 1f));
					return drawColor;
				}
				if (this.FadingOut)
				{
					Color drawColor2 = base.DrawColor;
					drawColor2.a *= Mathf.Lerp(1f, 0f, Mathf.Max((float)this.ageTicks - (float)(this.LeaveMapAfterTicks - this.def.skyfaller.fadeOutTicks), 0f) / (float)this.def.skyfaller.fadeOutTicks);
					return drawColor2;
				}
				return base.DrawColor;
			}
			set
			{
				base.DrawColor = value;
			}
		}

		// Token: 0x170026ED RID: 9965
		// (get) Token: 0x0600F7F2 RID: 63474 RVA: 0x00481960 File Offset: 0x0047FB60
		public bool FadingOut
		{
			get
			{
				return this.def.skyfaller.fadeOutTicks > 0 && this.ageTicks >= this.LeaveMapAfterTicks - this.def.skyfaller.fadeOutTicks;
			}
		}

		// Token: 0x170026EE RID: 9966
		// (get) Token: 0x0600F7F3 RID: 63475 RVA: 0x0048199C File Offset: 0x0047FB9C
		protected Material ShadowMaterial
		{
			get
			{
				if (this.cachedShadowMaterial == null && !this.def.skyfaller.shadow.NullOrEmpty())
				{
					this.cachedShadowMaterial = MaterialPool.MatFrom(this.def.skyfaller.shadow, ShaderDatabase.Transparent);
				}
				return this.cachedShadowMaterial;
			}
		}

		// Token: 0x170026EF RID: 9967
		// (get) Token: 0x0600F7F4 RID: 63476 RVA: 0x004819F4 File Offset: 0x0047FBF4
		protected float TimeInAnimation
		{
			get
			{
				if (this.def.skyfaller.reversed)
				{
					return (float)this.ticksToImpact / (float)this.LeaveMapAfterTicks;
				}
				return 1f - (float)this.ticksToImpact / (float)this.ticksToImpactMax;
			}
		}

		// Token: 0x170026F0 RID: 9968
		// (get) Token: 0x0600F7F5 RID: 63477 RVA: 0x00481A30 File Offset: 0x0047FC30
		private float CurrentSpeed
		{
			get
			{
				if (this.def.skyfaller.speedCurve == null)
				{
					return this.def.skyfaller.speed;
				}
				return this.def.skyfaller.speedCurve.Evaluate(this.TimeInAnimation) * this.def.skyfaller.speed;
			}
		}

		// Token: 0x170026F1 RID: 9969
		// (get) Token: 0x0600F7F6 RID: 63478 RVA: 0x00481A8C File Offset: 0x0047FC8C
		private bool SpawnTimedMotes
		{
			get
			{
				return this.def.skyfaller.moteSpawnTime != float.MinValue && Mathf.Approximately(this.def.skyfaller.moteSpawnTime, this.TimeInAnimation);
			}
		}

		// Token: 0x0600F7F7 RID: 63479 RVA: 0x00481AC2 File Offset: 0x0047FCC2
		public Skyfaller()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600F7F8 RID: 63480 RVA: 0x00481AE8 File Offset: 0x0047FCE8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_Deep.Look<Letter>(ref this.impactLetter, "impactLetter", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToDiscard, "ticksToDiscard", 0, false);
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToImpactMax, "ticksToImpactMax", this.LeaveMapAfterTicks, false);
			Scribe_Values.Look<float>(ref this.angle, "angle", 0f, false);
			Scribe_Values.Look<float>(ref this.shrapnelDirection, "shrapnelDirection", 0f, false);
			Scribe_Values.Look<bool>(ref this.hasHitRoof, "hasHitRoof", false, false);
			Scribe_Values.Look<bool>(ref this.hasImpacted, "hasImpacted", false, false);
			Scribe_Values.Look<bool>(ref this.hasLeftMap, "hasLeftMap", false, false);
			Scribe_Values.Look<bool>(ref this.contentsCanOverlap, "contentsCanOverlap", true, false);
			Scribe_Values.Look<bool>(ref this.moveAside, "moveAside", false, false);
		}

		// Token: 0x0600F7F9 RID: 63481 RVA: 0x00481BFD File Offset: 0x0047FDFD
		public override void PostMake()
		{
			base.PostMake();
			if (this.def.skyfaller.MakesShrapnel)
			{
				this.shrapnelDirection = Rand.Range(0f, 360f);
			}
		}

		// Token: 0x0600F7FA RID: 63482 RVA: 0x00481C2C File Offset: 0x0047FE2C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (respawningAfterLoad || base.BeingTransportedOnGravship)
			{
				return;
			}
			this.ticksToImpact = (this.ticksToImpactMax = this.def.skyfaller.ticksToImpactRange.RandomInRange);
			this.ticksToDiscard = ((this.def.skyfaller.ticksToDiscardInReverse != IntRange.Zero) ? this.def.skyfaller.ticksToDiscardInReverse.RandomInRange : -1);
			if (this.def.skyfaller.MakesShrapnel)
			{
				float num = GenMath.PositiveMod(this.shrapnelDirection, 360f);
				if (num < 270f && num >= 90f)
				{
					this.angle = Rand.Range(0f, 33f);
				}
				else
				{
					this.angle = Rand.Range(-33f, 0f);
				}
			}
			else if (this.def.skyfaller.angleCurve != null)
			{
				this.angle = this.def.skyfaller.angleCurve.Evaluate(0f);
			}
			else
			{
				this.angle = -33.7f;
			}
			if (this.def.rotatable && this.innerContainer.Any)
			{
				base.Rotation = this.innerContainer[0].Rotation;
			}
		}

		// Token: 0x0600F7FB RID: 63483 RVA: 0x00481D7C File Offset: 0x0047FF7C
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Destroy(mode);
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			if (this.anticipationSoundPlaying != null)
			{
				this.anticipationSoundPlaying.End();
				this.anticipationSoundPlaying = null;
			}
		}

		// Token: 0x0600F7FC RID: 63484 RVA: 0x00481DAC File Offset: 0x0047FFAC
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			float extraRotation;
			this.GetDrawPositionAndRotation(ref drawLoc, out extraRotation);
			Thing thingForGraphic = this.GetThingForGraphic();
			if (!WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				this.Graphic.Draw(drawLoc, flip ? thingForGraphic.Rotation.Opposite : thingForGraphic.Rotation, thingForGraphic, extraRotation);
			}
			this.DrawDropSpotShadow();
		}

		// Token: 0x0600F7FD RID: 63485 RVA: 0x00481E00 File Offset: 0x00480000
		protected virtual void GetDrawPositionAndRotation(ref Vector3 drawLoc, out float extraRotation)
		{
			extraRotation = 0f;
			if (this.def.skyfaller.rotateGraphicTowardsDirection)
			{
				extraRotation = this.angle;
			}
			if (this.randomizeDirectionComp != null)
			{
				extraRotation += this.randomizeDirectionComp.ExtraDrawAngle;
			}
			if (this.def.skyfaller.angleCurve != null)
			{
				this.angle = this.def.skyfaller.angleCurve.Evaluate(this.TimeInAnimation);
			}
			if (this.def.skyfaller.rotationCurve != null)
			{
				extraRotation += this.def.skyfaller.rotationCurve.Evaluate(this.TimeInAnimation);
			}
			if (this.def.skyfaller.xPositionCurve != null)
			{
				drawLoc.x += this.def.skyfaller.xPositionCurve.Evaluate(this.TimeInAnimation);
			}
			if (this.def.skyfaller.zPositionCurve != null)
			{
				drawLoc.z += this.def.skyfaller.zPositionCurve.Evaluate(this.TimeInAnimation);
			}
		}

		// Token: 0x0600F7FE RID: 63486 RVA: 0x00481F1C File Offset: 0x0048011C
		public virtual float DrawAngle()
		{
			float num = 0f;
			if (this.def.skyfaller.rotateGraphicTowardsDirection)
			{
				num = this.angle;
			}
			num += this.def.skyfaller.rotationCurve.Evaluate(this.TimeInAnimation);
			if (this.randomizeDirectionComp != null)
			{
				num += this.randomizeDirectionComp.ExtraDrawAngle;
			}
			return num;
		}

		// Token: 0x0600F7FF RID: 63487 RVA: 0x00481F80 File Offset: 0x00480180
		protected override void Tick()
		{
			base.Tick();
			if (this.SpawnTimedMotes)
			{
				CellRect cellRect = this.OccupiedRect();
				for (int i = 0; i < cellRect.Area * this.def.skyfaller.motesPerCell; i++)
				{
					FleckMaker.ThrowDustPuff(cellRect.RandomVector3, base.Map, 2f);
				}
			}
			if (this.def.skyfaller.floatingSound != null && (this.floatingSoundPlaying == null || this.floatingSoundPlaying.Ended))
			{
				this.floatingSoundPlaying = this.def.skyfaller.floatingSound.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this), MaintenanceType.PerTick));
			}
			Sustainer sustainer = this.floatingSoundPlaying;
			if (sustainer != null)
			{
				sustainer.Maintain();
			}
			if (this.def.skyfaller.reversed)
			{
				this.ticksToImpact++;
				if (!this.anticipationSoundPlayed && this.def.skyfaller.anticipationSound != null && this.ticksToImpact > this.def.skyfaller.anticipationSoundTicks)
				{
					this.anticipationSoundPlayed = true;
					TargetInfo source = new TargetInfo(base.Position, base.Map, false);
					if (this.def.skyfaller.anticipationSound.sustain)
					{
						this.anticipationSoundPlaying = this.def.skyfaller.anticipationSound.TrySpawnSustainer(source);
					}
					else
					{
						this.def.skyfaller.anticipationSound.PlayOneShot(source);
					}
				}
				if (this.ticksToImpact >= this.LeaveMapAfterTicks && !this.hasLeftMap)
				{
					this.LeaveMap();
				}
			}
			else
			{
				this.ticksToImpact--;
				if (this.ticksToImpact <= 15 && !this.hasHitRoof)
				{
					this.HitRoof();
				}
				if (!this.anticipationSoundPlayed && this.def.skyfaller.anticipationSound != null && this.ticksToImpact < this.def.skyfaller.anticipationSoundTicks)
				{
					this.anticipationSoundPlayed = true;
					TargetInfo source2 = new TargetInfo(base.Position, base.Map, false);
					if (this.def.skyfaller.anticipationSound.sustain)
					{
						this.anticipationSoundPlaying = this.def.skyfaller.anticipationSound.TrySpawnSustainer(source2);
					}
					else
					{
						this.def.skyfaller.anticipationSound.PlayOneShot(source2);
					}
				}
				Sustainer sustainer2 = this.anticipationSoundPlaying;
				if (sustainer2 != null)
				{
					sustainer2.Maintain();
				}
				if (this.ticksToImpact <= 0 && !this.hasImpacted)
				{
					this.Impact();
				}
			}
			this.ageTicks++;
		}

		// Token: 0x0600F800 RID: 63488 RVA: 0x00482234 File Offset: 0x00480434
		protected virtual void HitRoof()
		{
			if (!this.def.skyfaller.hitRoof)
			{
				return;
			}
			CellRect cr = this.OccupiedRect();
			this.hasHitRoof = true;
			if (!cr.Cells.Any((IntVec3 x) => x.InBounds(this.Map) && x.Roofed(this.Map)))
			{
				return;
			}
			RoofDef roof = cr.Cells.First((IntVec3 x) => x.InBounds(this.Map) && x.Roofed(this.Map)).GetRoof(base.Map);
			if (!roof.soundPunchThrough.NullOrUndefined())
			{
				roof.soundPunchThrough.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			CellRect cellRect = cr.ExpandedBy(1).ClipInsideMap(base.Map);
			Map map = base.Map;
			RoofCollapserImmediate.DropRoofInCells(cellRect.Cells.Where(delegate(IntVec3 c)
			{
				if (!c.InBounds(map))
				{
					return false;
				}
				if (cr.Contains(c))
				{
					return true;
				}
				if (c.GetFirstPawn(map) != null)
				{
					return false;
				}
				Building edifice = c.GetEdifice(map);
				return edifice == null || !edifice.def.holdsRoof;
			}), map, null);
		}

		// Token: 0x0600F801 RID: 63489 RVA: 0x00482338 File Offset: 0x00480538
		protected virtual void SpawnThings()
		{
			Skyfaller.usedCells.Clear();
			int i;
			int j;
			for (i = this.innerContainer.Count - 1; i >= 0; i = j - 1)
			{
				GenPlace.TryPlaceThing(this.innerContainer[i], base.Position, base.Map, ThingPlaceMode.Near, delegate(Thing thing, int count)
				{
					PawnUtility.RecoverFromUnwalkablePositionOrKill(thing.Position, thing.Map);
					if (thing.def.Fillage == FillCategory.Full && this.def.skyfaller.CausesExplosion && this.def.skyfaller.explosionDamage.isExplosive && thing.Position.InHorDistOf(base.Position, this.def.skyfaller.explosionRadius))
					{
						base.Map.terrainGrid.Notify_TerrainDestroyed(thing.Position);
					}
					if (this.moveAside)
					{
						GenSpawn.CheckMoveItemsAside(thing.Position, thing.Rotation, thing.def, thing.Map);
					}
					if (!this.contentsCanOverlap)
					{
						foreach (IntVec3 item in thing.OccupiedRect())
						{
							Skyfaller.usedCells.Add(item);
						}
					}
				}, delegate(IntVec3 c)
				{
					if (!this.contentsCanOverlap)
					{
						foreach (IntVec3 item in GenAdj.OccupiedRect(c, this.innerContainer[i].def.defaultPlacingRot, this.innerContainer[i].def.size))
						{
							if (Skyfaller.usedCells.Contains(item))
							{
								return false;
							}
						}
						return true;
					}
					return true;
				}, new Rot4?(this.innerContainer[i].Rotation), 1);
				j = i;
			}
		}

		// Token: 0x0600F802 RID: 63490 RVA: 0x004823E4 File Offset: 0x004805E4
		protected virtual void Impact()
		{
			this.hasImpacted = true;
			if (this.def.skyfaller.CausesExplosion)
			{
				IntVec3 position = base.Position;
				Map map = base.Map;
				float explosionRadius = this.def.skyfaller.explosionRadius;
				DamageDef explosionDamage = this.def.skyfaller.explosionDamage;
				Thing instigator = null;
				int damAmount = GenMath.RoundRandom((float)this.def.skyfaller.explosionDamage.defaultDamage * this.def.skyfaller.explosionDamageFactor);
				float armorPenetration = -1f;
				SoundDef explosionSound = null;
				ThingDef weapon = null;
				ThingDef projectile = null;
				Thing intendedTarget = null;
				ThingDef postExplosionSpawnThingDef = null;
				float postExplosionSpawnChance = 0f;
				int postExplosionSpawnThingCount = 1;
				List<Thing> ignoredThings = (!this.def.skyfaller.damageSpawnedThings) ? this.innerContainer.ToList<Thing>() : null;
				GenExplosion.DoExplosion(position, map, explosionRadius, explosionDamage, instigator, damAmount, armorPenetration, explosionSound, weapon, projectile, intendedTarget, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, null, null, 255, false, null, 0f, 1, 0f, false, null, ignoredThings, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			}
			this.SpawnThings();
			this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			CellRect cellRect = this.OccupiedRect();
			for (int i = 0; i < cellRect.Area * this.def.skyfaller.motesPerCell; i++)
			{
				FleckMaker.ThrowDustPuff(cellRect.RandomVector3, base.Map, 2f);
			}
			if (this.def.skyfaller.MakesShrapnel)
			{
				SkyfallerShrapnelUtility.MakeShrapnel(base.Position, base.Map, this.shrapnelDirection, this.def.skyfaller.shrapnelDistanceFactor, this.def.skyfaller.metalShrapnelCountRange.RandomInRange, this.def.skyfaller.rubbleShrapnelCountRange.RandomInRange, true);
			}
			if (this.def.skyfaller.cameraShake > 0f && base.Map == Find.CurrentMap)
			{
				Find.CameraDriver.shaker.DoShake(this.def.skyfaller.cameraShake);
			}
			if (this.def.skyfaller.impactSound != null)
			{
				this.def.skyfaller.impactSound.PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			if (this.impactLetter != null)
			{
				Find.LetterStack.ReceiveLetter(this.impactLetter, null, 0, true);
			}
			Map map2 = base.Map;
			this.Destroy(DestroyMode.Vanish);
			if (this.def.skyfaller.spawnThing != null)
			{
				Thing thing;
				GenSpawn.TrySpawn(this.def.skyfaller.spawnThing, base.Position, map2, out thing, WipeMode.Vanish, true);
			}
		}

		// Token: 0x0600F803 RID: 63491 RVA: 0x00482686 File Offset: 0x00480886
		protected virtual void LeaveMap()
		{
			this.hasLeftMap = true;
			this.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0600F804 RID: 63492 RVA: 0x00482696 File Offset: 0x00480896
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600F805 RID: 63493 RVA: 0x0048269E File Offset: 0x0048089E
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600F806 RID: 63494 RVA: 0x004826AC File Offset: 0x004808AC
		private Thing GetThingForGraphic()
		{
			if (this.def.graphicData != null || !this.innerContainer.Any)
			{
				return this;
			}
			return this.innerContainer[0];
		}

		// Token: 0x0600F807 RID: 63495 RVA: 0x004826D8 File Offset: 0x004808D8
		protected virtual void DrawDropSpotShadow()
		{
			Material shadowMaterial = this.ShadowMaterial;
			if (shadowMaterial == null)
			{
				return;
			}
			Skyfaller.DrawDropSpotShadow(base.DrawPos, base.Rotation, shadowMaterial, this.def.skyfaller.shadowSize, this.ticksToImpact);
		}

		// Token: 0x0600F808 RID: 63496 RVA: 0x00482720 File Offset: 0x00480920
		public static void DrawDropSpotShadow(Vector3 center, Rot4 rot, Material material, Vector2 shadowSize, int ticksToImpact)
		{
			ticksToImpact = Mathf.Max(ticksToImpact, 0);
			Vector3 pos = center;
			pos.y = AltitudeLayer.Shadows.AltitudeFor();
			float num = 1f + (float)ticksToImpact / 100f;
			Vector3 s = new Vector3(num * shadowSize.x, 1f, num * shadowSize.y);
			Color white = Color.white;
			if (ticksToImpact > 150)
			{
				white.a = Mathf.InverseLerp(200f, 150f, (float)ticksToImpact);
			}
			Skyfaller.shadowPropertyBlock.SetColor(ShaderPropertyIDs.Color, white);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, rot.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10Back, matrix, material, 0, null, 0, Skyfaller.shadowPropertyBlock);
		}

		// Token: 0x0600F809 RID: 63497 RVA: 0x00002501 File Offset: 0x00000701
		public RoofCollapseResponse Notify_OnBeforeRoofCollapse()
		{
			return RoofCollapseResponse.None;
		}

		// Token: 0x0400A30E RID: 41742
		public ThingOwner innerContainer;

		// Token: 0x0400A30F RID: 41743
		public int ticksToImpact;

		// Token: 0x0400A310 RID: 41744
		public int ageTicks;

		// Token: 0x0400A311 RID: 41745
		public int ticksToDiscard;

		// Token: 0x0400A312 RID: 41746
		public float angle;

		// Token: 0x0400A313 RID: 41747
		public float shrapnelDirection;

		// Token: 0x0400A314 RID: 41748
		private int ticksToImpactMax = 220;

		// Token: 0x0400A315 RID: 41749
		public Letter impactLetter;

		// Token: 0x0400A316 RID: 41750
		public bool contentsCanOverlap = true;

		// Token: 0x0400A317 RID: 41751
		private bool hasHitRoof;

		// Token: 0x0400A318 RID: 41752
		protected bool hasImpacted;

		// Token: 0x0400A319 RID: 41753
		private bool hasLeftMap;

		// Token: 0x0400A31A RID: 41754
		public bool moveAside;

		// Token: 0x0400A31B RID: 41755
		private Material cachedShadowMaterial;

		// Token: 0x0400A31C RID: 41756
		private bool anticipationSoundPlayed;

		// Token: 0x0400A31D RID: 41757
		private Sustainer floatingSoundPlaying;

		// Token: 0x0400A31E RID: 41758
		private Sustainer anticipationSoundPlaying;

		// Token: 0x0400A31F RID: 41759
		private static MaterialPropertyBlock shadowPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400A320 RID: 41760
		public const float DefaultAngle = -33.7f;

		// Token: 0x0400A321 RID: 41761
		private const int RoofHitPreDelay = 15;

		// Token: 0x0400A322 RID: 41762
		private const int LeaveMapAfterTicksDefault = 220;

		// Token: 0x0400A324 RID: 41764
		protected CompSkyfallerRandomizeDirection randomizeDirectionComp;

		// Token: 0x0400A325 RID: 41765
		private static readonly List<IntVec3> usedCells = new List<IntVec3>();
	}
}
