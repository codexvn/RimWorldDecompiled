using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002DBD RID: 11709
	[StaticConstructorOnStartup]
	public class CompProjectileInterceptor : ThingComp
	{
		// Token: 0x170029F9 RID: 10745
		// (get) Token: 0x0601075A RID: 67418 RVA: 0x004BF417 File Offset: 0x004BD617
		private static Material ShieldDotMat
		{
			get
			{
				return MaterialPool.MatFrom("Things/Mote/ShieldDownDot", ShaderDatabase.MoteGlow);
			}
		}

		// Token: 0x170029FA RID: 10746
		// (get) Token: 0x0601075B RID: 67419 RVA: 0x002A994D File Offset: 0x002A7B4D
		protected virtual int NumInactiveDots
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x170029FB RID: 10747
		// (get) Token: 0x0601075C RID: 67420 RVA: 0x004BF428 File Offset: 0x004BD628
		public CompProperties_ProjectileInterceptor Props
		{
			get
			{
				return (CompProperties_ProjectileInterceptor)this.props;
			}
		}

		// Token: 0x170029FC RID: 10748
		// (get) Token: 0x0601075D RID: 67421 RVA: 0x004BF438 File Offset: 0x004BD638
		public bool Active
		{
			get
			{
				if (this.OnCooldown || this.Charging || this.stunner.Stunned || this.shutDown || this.currentHitPoints == 0 || !this.parent.Spawned)
				{
					return false;
				}
				if (this.Props.activated && Find.TickManager.TicksGame > this.activatedTick + this.Props.activeDuration)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && (pawn.IsCharging() || pawn.IsSelfShutdown()))
				{
					return false;
				}
				CompCanBeDormant comp = this.parent.GetComp<CompCanBeDormant>();
				return comp == null || comp.Awake;
			}
		}

		// Token: 0x170029FD RID: 10749
		// (get) Token: 0x0601075E RID: 67422 RVA: 0x004BF4E9 File Offset: 0x004BD6E9
		public bool OnCooldown
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastInterceptTicks + this.Props.cooldownTicks;
			}
		}

		// Token: 0x170029FE RID: 10750
		// (get) Token: 0x0601075F RID: 67423 RVA: 0x004BF509 File Offset: 0x004BD709
		public bool Charging
		{
			get
			{
				return this.startedChargingTick >= 0 && Find.TickManager.TicksGame > this.startedChargingTick;
			}
		}

		// Token: 0x170029FF RID: 10751
		// (get) Token: 0x06010760 RID: 67424 RVA: 0x004BF528 File Offset: 0x004BD728
		public int ChargeCycleStartTick
		{
			get
			{
				if (this.startedChargingTick < 0)
				{
					return 0;
				}
				return this.startedChargingTick;
			}
		}

		// Token: 0x17002A00 RID: 10752
		// (get) Token: 0x06010761 RID: 67425 RVA: 0x004BF53B File Offset: 0x004BD73B
		public int ChargingTicksLeft
		{
			get
			{
				if (this.startedChargingTick < 0)
				{
					return 0;
				}
				return Mathf.Max(this.startedChargingTick + this.Props.chargeDurationTicks - Find.TickManager.TicksGame, 0);
			}
		}

		// Token: 0x17002A01 RID: 10753
		// (get) Token: 0x06010762 RID: 67426 RVA: 0x004BF56B File Offset: 0x004BD76B
		public int CooldownTicksLeft
		{
			get
			{
				if (!this.OnCooldown)
				{
					return 0;
				}
				return this.Props.cooldownTicks - (Find.TickManager.TicksGame - this.lastInterceptTicks);
			}
		}

		// Token: 0x17002A02 RID: 10754
		// (get) Token: 0x06010763 RID: 67427 RVA: 0x004BF594 File Offset: 0x004BD794
		public bool ReactivatedThisTick
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastInterceptTicks == this.Props.cooldownTicks;
			}
		}

		// Token: 0x17002A03 RID: 10755
		// (get) Token: 0x06010764 RID: 67428 RVA: 0x004BF5B4 File Offset: 0x004BD7B4
		public bool ShouldDisplayHitpointsGizmo
		{
			get
			{
				if (this.Props.alwaysShowHitpointsGizmo)
				{
					return true;
				}
				Pawn pawn = this.parent as Pawn;
				return pawn != null && (pawn.IsColonistPlayerControlled || pawn.RaceProps.IsMechanoid) && this.HitPointsMax > 0;
			}
		}

		// Token: 0x17002A04 RID: 10756
		// (get) Token: 0x06010765 RID: 67429 RVA: 0x004BF600 File Offset: 0x004BD800
		public int HitPointsMax
		{
			get
			{
				int? num = this.maxHitPointsOverride;
				if (num == null)
				{
					return this.Props.hitPoints;
				}
				return num.GetValueOrDefault();
			}
		}

		// Token: 0x17002A05 RID: 10757
		// (get) Token: 0x06010766 RID: 67430 RVA: 0x004BF630 File Offset: 0x004BD830
		private int RemainingTicks
		{
			get
			{
				return this.activatedTick + this.Props.activeDuration - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17002A06 RID: 10758
		// (get) Token: 0x06010767 RID: 67431 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual int HitPointsPerInterval
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06010768 RID: 67432 RVA: 0x004BF650 File Offset: 0x004BD850
		public override void PostPostMake()
		{
			base.PostPostMake();
			if (this.Props.chargeIntervalTicks > 0)
			{
				this.startedChargingTick = Find.TickManager.TicksGame + Rand.Range(0, this.Props.chargeIntervalTicks);
			}
			if (this.HitPointsMax > 0)
			{
				if (this.Props.startWithMaxHitPoints)
				{
					this.currentHitPoints = this.HitPointsMax;
				}
				else
				{
					this.currentHitPoints = 0;
				}
			}
			this.stunner = new StunHandler(this.parent);
		}

		// Token: 0x06010769 RID: 67433 RVA: 0x004BF6CF File Offset: 0x004BD8CF
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			Sustainer sustainer = this.sustainer;
			if (sustainer == null)
			{
				return;
			}
			sustainer.End();
		}

		// Token: 0x0601076A RID: 67434 RVA: 0x004BF6E4 File Offset: 0x004BD8E4
		public bool CheckIntercept(Projectile projectile, Vector3 lastExactPos, Vector3 newExactPos)
		{
			Vector3 vector = this.parent.Position.ToVector3Shifted();
			float num = this.Props.radius + projectile.def.projectile.SpeedTilesPerTick + 0.1f;
			if ((newExactPos.x - vector.x) * (newExactPos.x - vector.x) + (newExactPos.z - vector.z) * (newExactPos.z - vector.z) > num * num)
			{
				return false;
			}
			if (!this.Active)
			{
				return false;
			}
			if (!CompProjectileInterceptor.InterceptsProjectile(this.Props, projectile))
			{
				return false;
			}
			if (projectile.Launcher == null && !this.debugInterceptNonHostileProjectiles && !this.Props.interceptNonHostileProjectiles)
			{
				return false;
			}
			if (this.parent.Faction != null)
			{
				if (projectile.Launcher != null && projectile.Launcher.Spawned && !projectile.Launcher.HostileTo(this.parent.Faction))
				{
					return false;
				}
				if (projectile.Launcher != null && !projectile.Launcher.Spawned && !projectile.Launcher.Faction.HostileTo(this.parent.Faction))
				{
					return false;
				}
			}
			if (!this.Props.interceptOutgoingProjectiles && (new Vector2(vector.x, vector.z) - new Vector2(lastExactPos.x, lastExactPos.z)).sqrMagnitude <= this.Props.radius * this.Props.radius)
			{
				return false;
			}
			if (!GenGeo.IntersectLineCircleOutline(new Vector2(vector.x, vector.z), this.Props.radius, new Vector2(lastExactPos.x, lastExactPos.z), new Vector2(newExactPos.x, newExactPos.z)))
			{
				return false;
			}
			this.lastInterceptAngle = lastExactPos.AngleToFlat(this.parent.TrueCenter());
			this.lastInterceptTicks = Find.TickManager.TicksGame;
			this.drawInterceptCone = true;
			this.TriggerEffecter(newExactPos.ToIntVec3());
			if (projectile.DamageDef == DamageDefOf.EMP && this.Props.disarmedByEmpForTicks > 0)
			{
				this.BreakShieldEmp(new DamageInfo(projectile.DamageDef, (float)projectile.DamageDef.defaultDamage, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			if (this.currentHitPoints > 0)
			{
				this.currentHitPoints -= projectile.DamageAmount;
				if (this.currentHitPoints < 0)
				{
					this.currentHitPoints = 0;
				}
				if (this.currentHitPoints == 0)
				{
					this.startedChargingTick = Find.TickManager.TicksGame;
					this.BreakShieldHitpoints(new DamageInfo(projectile.DamageDef, (float)projectile.DamageDef.defaultDamage, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
					return true;
				}
			}
			return true;
		}

		// Token: 0x0601076B RID: 67435 RVA: 0x004BF9B0 File Offset: 0x004BDBB0
		public bool CheckBombardmentIntercept(Bombardment bombardment, Bombardment.BombardmentProjectile projectile)
		{
			if (!this.Active || !this.Props.interceptAirProjectiles)
			{
				return false;
			}
			if (!projectile.targetCell.InHorDistOf(this.parent.Position, this.Props.radius))
			{
				return false;
			}
			if ((bombardment.instigator == null || !bombardment.instigator.HostileTo(this.parent)) && !this.debugInterceptNonHostileProjectiles && !this.Props.interceptNonHostileProjectiles)
			{
				return false;
			}
			this.lastInterceptTicks = Find.TickManager.TicksGame;
			this.drawInterceptCone = false;
			this.TriggerEffecter(projectile.targetCell);
			return true;
		}

		// Token: 0x0601076C RID: 67436 RVA: 0x004BFA50 File Offset: 0x004BDC50
		public bool BombardmentCanStartFireAt(Bombardment bombardment, IntVec3 cell)
		{
			return !this.Active || !this.Props.interceptAirProjectiles || ((bombardment.instigator == null || !bombardment.instigator.HostileTo(this.parent)) && !this.debugInterceptNonHostileProjectiles && !this.Props.interceptNonHostileProjectiles) || !cell.InHorDistOf(this.parent.Position, this.Props.radius);
		}

		// Token: 0x0601076D RID: 67437 RVA: 0x004BFAC6 File Offset: 0x004BDCC6
		private void TriggerEffecter(IntVec3 pos)
		{
			Effecter effecter = new Effecter(this.Props.interceptEffect ?? EffecterDefOf.Interceptor_BlockedProjectile);
			effecter.Trigger(new TargetInfo(pos, this.parent.Map, false), TargetInfo.Invalid, -1);
			effecter.Cleanup();
		}

		// Token: 0x0601076E RID: 67438 RVA: 0x004BFB08 File Offset: 0x004BDD08
		public static bool InterceptsProjectile(CompProperties_ProjectileInterceptor props, Projectile projectile)
		{
			if (props.interceptGroundProjectiles && props.interceptAirProjectiles)
			{
				return true;
			}
			if (props.interceptGroundProjectiles)
			{
				return !projectile.def.projectile.flyOverhead;
			}
			return props.interceptAirProjectiles && projectile.def.projectile.flyOverhead;
		}

		// Token: 0x0601076F RID: 67439 RVA: 0x004BFB60 File Offset: 0x004BDD60
		public override void CompTick()
		{
			if (this.HitPointsMax > 0 && this.ChargingTicksLeft == 0)
			{
				if (this.currentHitPoints == 0 && this.Props.hitPointsRestoreInstantlyAfterCharge)
				{
					this.currentHitPoints = this.HitPointsMax;
				}
				if (this.parent.IsHashIntervalTick(this.Props.rechargeHitPointsIntervalTicks))
				{
					this.currentHitPoints = Mathf.Clamp(this.currentHitPoints + this.HitPointsPerInterval, 0, this.HitPointsMax);
				}
				if (this.startedChargingTick > 0)
				{
					this.startedChargingTick = -1;
				}
			}
			if (this.Props.activated && !this.Charging && Find.TickManager.TicksGame == this.activatedTick + this.Props.activeDuration)
			{
				this.startedChargingTick = Find.TickManager.TicksGame;
			}
			if (this.ReactivatedThisTick && this.Props.reactivateEffect != null)
			{
				this.Props.reactivateEffect.Spawn(this.parent, this.parent.MapHeld, 1f).Cleanup();
			}
			if (this.Props.chargeIntervalTicks > 0 && Find.TickManager.TicksGame >= this.startedChargingTick + this.Props.chargeDurationTicks)
			{
				this.startedChargingTick += this.Props.chargeIntervalTicks;
			}
			this.stunner.StunHandlerTick();
			if (!this.Props.activeSound.NullOrUndefined())
			{
				if (this.Active)
				{
					if (this.sustainer == null || this.sustainer.Ended)
					{
						this.sustainer = this.Props.activeSound.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
					}
					this.sustainer.Maintain();
					return;
				}
				if (this.sustainer != null && !this.sustainer.Ended)
				{
					this.sustainer.End();
				}
			}
		}

		// Token: 0x06010770 RID: 67440 RVA: 0x004BFD3C File Offset: 0x004BDF3C
		public override void PostDrawExtraSelectionOverlays()
		{
			base.PostDrawExtraSelectionOverlays();
			if (!this.Active && !WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				for (int i = 0; i < this.NumInactiveDots; i++)
				{
					Vector3 b = new Vector3(0f, 0f, 1f).RotatedBy((float)i / (float)this.NumInactiveDots * 360f) * (this.Props.radius * 0.966f);
					Vector3 vector = this.parent.DrawPos + b;
					Graphics.DrawMesh(MeshPool.plane10, new Vector3(vector.x, AltitudeLayer.MoteOverhead.AltitudeFor(), vector.z), Quaternion.identity, CompProjectileInterceptor.ShieldDotMat, 0);
				}
			}
		}

		// Token: 0x06010771 RID: 67441 RVA: 0x004BFDFA File Offset: 0x004BDFFA
		public override void Notify_LordDestroyed()
		{
			base.Notify_LordDestroyed();
			this.shutDown = true;
		}

		// Token: 0x06010772 RID: 67442 RVA: 0x004BFE0C File Offset: 0x004BE00C
		public override void PostDraw()
		{
			base.PostDraw();
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			float currentAlpha = this.GetCurrentAlpha();
			if (currentAlpha > 0f)
			{
				Color value;
				if (this.Active || !Find.Selector.IsSelected(this.parent))
				{
					value = this.Props.color;
				}
				else
				{
					value = CompProjectileInterceptor.InactiveColor;
				}
				value.a *= currentAlpha;
				CompProjectileInterceptor.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, value);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(drawPos, Quaternion.identity, new Vector3(this.Props.radius * 2f * 1.1601562f, 1f, this.Props.radius * 2f * 1.1601562f));
				Graphics.DrawMesh(MeshPool.plane10, matrix, CompProjectileInterceptor.ForceFieldMat, 0, null, 0, CompProjectileInterceptor.MatPropertyBlock);
			}
			float currentConeAlpha_RecentlyIntercepted = this.GetCurrentConeAlpha_RecentlyIntercepted();
			if (currentConeAlpha_RecentlyIntercepted > 0f)
			{
				Color color = this.Props.color;
				color.a *= currentConeAlpha_RecentlyIntercepted;
				CompProjectileInterceptor.MatPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
				Matrix4x4 matrix2 = default(Matrix4x4);
				matrix2.SetTRS(drawPos, Quaternion.Euler(0f, this.lastInterceptAngle - 90f, 0f), new Vector3(this.Props.radius * 2f * 1.1601562f, 1f, this.Props.radius * 2f * 1.1601562f));
				Graphics.DrawMesh(MeshPool.plane10, matrix2, CompProjectileInterceptor.ForceFieldConeMat, 0, null, 0, CompProjectileInterceptor.MatPropertyBlock);
			}
		}

		// Token: 0x06010773 RID: 67443 RVA: 0x004BFFB3 File Offset: 0x004BE1B3
		private float GetCurrentAlpha()
		{
			return Mathf.Max(Mathf.Max(Mathf.Max(Mathf.Max(this.GetCurrentAlpha_Idle(), this.GetCurrentAlpha_Selected()), this.GetCurrentAlpha_RecentlyIntercepted()), this.GetCurrentAlpha_RecentlyActivated()), this.Props.minAlpha);
		}

		// Token: 0x06010774 RID: 67444 RVA: 0x004BFFEC File Offset: 0x004BE1EC
		private float GetCurrentAlpha_Idle()
		{
			float idlePulseSpeed = this.Props.idlePulseSpeed;
			float minIdleAlpha = this.Props.minIdleAlpha;
			if (!this.Active)
			{
				return 0f;
			}
			if (this.parent.Faction == Faction.OfPlayer && !this.debugInterceptNonHostileProjectiles)
			{
				return 0f;
			}
			if (Find.Selector.IsSelected(this.parent))
			{
				return 0f;
			}
			return Mathf.Lerp(minIdleAlpha, 0.11f, (Mathf.Sin((float)(Gen.HashCombineInt(this.parent.thingIDNumber, 96804938) % 100) + Time.realtimeSinceStartup * idlePulseSpeed) + 1f) / 2f);
		}

		// Token: 0x06010775 RID: 67445 RVA: 0x004C0094 File Offset: 0x004BE294
		private float GetCurrentAlpha_Selected()
		{
			float num = Mathf.Max(2f, this.Props.idlePulseSpeed);
			if ((!Find.Selector.IsSelected(this.parent) && !this.Props.drawWithNoSelection) || !this.Active)
			{
				return 0f;
			}
			return Mathf.Lerp(0.2f, 0.62f, (Mathf.Sin((float)(Gen.HashCombineInt(this.parent.thingIDNumber, 35990913) % 100) + Time.realtimeSinceStartup * num) + 1f) / 2f);
		}

		// Token: 0x06010776 RID: 67446 RVA: 0x004C0128 File Offset: 0x004BE328
		private float GetCurrentAlpha_RecentlyIntercepted()
		{
			int num = Find.TickManager.TicksGame - this.lastInterceptTicks;
			return Mathf.Clamp01(1f - (float)num / 40f) * 0.09f;
		}

		// Token: 0x06010777 RID: 67447 RVA: 0x004C0160 File Offset: 0x004BE360
		private float GetCurrentAlpha_RecentlyActivated()
		{
			if (!this.Active)
			{
				return 0f;
			}
			int num = Find.TickManager.TicksGame - (this.lastInterceptTicks + this.Props.cooldownTicks);
			return Mathf.Clamp01(1f - (float)num / 50f) * 0.09f;
		}

		// Token: 0x06010778 RID: 67448 RVA: 0x004C01B4 File Offset: 0x004BE3B4
		private float GetCurrentConeAlpha_RecentlyIntercepted()
		{
			if (!this.drawInterceptCone)
			{
				return 0f;
			}
			int num = Find.TickManager.TicksGame - this.lastInterceptTicks;
			return Mathf.Clamp01(1f - (float)num / 40f) * 0.82f;
		}

		// Token: 0x06010779 RID: 67449 RVA: 0x004C01FA File Offset: 0x004BE3FA
		public void Activate()
		{
			this.activatedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0601077A RID: 67450 RVA: 0x004C020C File Offset: 0x004BE40C
		public void Deactivate()
		{
			this.activatedTick = Find.TickManager.TicksGame - this.Props.activeDuration;
			this.startedChargingTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0601077B RID: 67451 RVA: 0x004C023A File Offset: 0x004BE43A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.ShouldDisplayHitpointsGizmo)
			{
				yield return new Gizmo_ProjectileInterceptorHitPoints
				{
					interceptor = this
				};
			}
			if (DebugSettings.ShowDevGizmos)
			{
				if (this.OnCooldown)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Reset cooldown",
						action = delegate()
						{
							this.lastInterceptTicks = Find.TickManager.TicksGame - this.Props.cooldownTicks;
						}
					};
				}
				yield return new Command_Toggle
				{
					defaultLabel = "DEV: Intercept non-hostile",
					isActive = (() => this.debugInterceptNonHostileProjectiles),
					toggleAction = delegate()
					{
						this.debugInterceptNonHostileProjectiles = !this.debugInterceptNonHostileProjectiles;
					}
				};
			}
			yield break;
		}

		// Token: 0x0601077C RID: 67452 RVA: 0x004C024C File Offset: 0x004BE44C
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.Props.interceptGroundProjectiles || this.Props.interceptAirProjectiles)
			{
				string value;
				if (this.Props.interceptGroundProjectiles && this.Props.interceptAirProjectiles)
				{
					value = "InterceptsProjectiles_BothProjectiles".Translate();
				}
				else if (this.Props.interceptGroundProjectiles)
				{
					value = "InterceptsProjectiles_GroundProjectiles".Translate();
				}
				else
				{
					value = "InterceptsProjectiles_AerialProjectiles".Translate();
				}
				if (this.Props.cooldownTicks > 0)
				{
					stringBuilder.AppendTagged("InterceptsProjectilesEvery".Translate(value, this.Props.cooldownTicks.ToStringTicksToPeriod(true, false, true, true, false)));
				}
				else
				{
					stringBuilder.Append("InterceptsProjectiles".Translate(value));
				}
			}
			if (this.Props.activated && this.Active)
			{
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.AppendTagged("ShieldDuration".Translate() + ": " + this.RemainingTicks.ToStringSecondsFromTicks("F0"));
			}
			if (this.OnCooldown)
			{
				stringBuilder.AppendLineIfNotEmpty();
				stringBuilder.AppendTagged("CooldownTime".Translate() + ": " + this.CooldownTicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
			if (this.stunner.Stunned)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendTagged("DisarmedTime".Translate() + ": " + this.stunner.StunTicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
			if (this.shutDown)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("ShutDown".Translate());
			}
			else if (this.Props.chargeIntervalTicks > 0)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				if (this.Charging)
				{
					stringBuilder.AppendTagged("ChargingTime".Translate() + ": " + this.ChargingTicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
				}
				else
				{
					stringBuilder.AppendTagged("ChargingNext".Translate((this.ChargeCycleStartTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false), this.Props.chargeDurationTicks.ToStringTicksToPeriod(true, false, true, true, false), this.Props.chargeIntervalTicks.ToStringTicksToPeriod(true, false, true, true, false)));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0601077D RID: 67453 RVA: 0x004C0507 File Offset: 0x004BE707
		public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PostPreApplyDamage(ref dinfo, out absorbed);
			if (dinfo.Def == DamageDefOf.EMP && this.Props.disarmedByEmpForTicks > 0)
			{
				this.BreakShieldEmp(dinfo);
			}
		}

		// Token: 0x0601077E RID: 67454 RVA: 0x004C0538 File Offset: 0x004BE738
		private void BreakShieldEmp(DamageInfo dinfo)
		{
			if (this.Active)
			{
				EffecterDefOf.Shield_Break.SpawnAttached(this.parent, this.parent.MapHeld, this.Props.radius);
				int num = Mathf.CeilToInt(this.Props.radius * 2f);
				CompProjectileInterceptor.<>c__DisplayClass66_0 CS$<>8__locals1;
				CS$<>8__locals1.fTheta = 6.2831855f / (float)num;
				CS$<>8__locals1.center = this.parent.TrueCenter();
				for (int i = 0; i < num; i++)
				{
					FleckMaker.ConnectingLine(this.<BreakShieldEmp>g__PosAtIndex|66_0(i, ref CS$<>8__locals1), this.<BreakShieldEmp>g__PosAtIndex|66_0((i + 1) % num, ref CS$<>8__locals1), FleckDefOf.LineEMP, this.parent.Map, 1.5f);
				}
			}
			dinfo.SetAmount((float)this.Props.disarmedByEmpForTicks / 30f);
			this.stunner.Notify_DamageApplied(dinfo);
		}

		// Token: 0x0601077F RID: 67455 RVA: 0x004C0610 File Offset: 0x004BE810
		private void BreakShieldHitpoints(DamageInfo dinfo)
		{
			EffecterDefOf.Shield_Break.SpawnAttached(this.parent, this.parent.MapHeld, this.Props.radius);
			this.stunner.Notify_DamageApplied(dinfo);
		}

		// Token: 0x06010780 RID: 67456 RVA: 0x004C0648 File Offset: 0x004BE848
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.lastInterceptTicks, "lastInterceptTicks", -999999, false);
			Scribe_Values.Look<bool>(ref this.shutDown, "shutDown", false, false);
			Scribe_Values.Look<int>(ref this.startedChargingTick, "nextChargeTick", -1, false);
			Scribe_Deep.Look<StunHandler>(ref this.stunner, "stunner", new object[]
			{
				this.parent
			});
			Scribe_Values.Look<int>(ref this.currentHitPoints, "currentHitPoints", -1, false);
			Scribe_Values.Look<int?>(ref this.maxHitPointsOverride, "maxHitPointsOverride", null, false);
			Scribe_Values.Look<int>(ref this.activatedTick, "activatedTick", -999999, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.Props.chargeIntervalTicks > 0 && this.startedChargingTick <= 0)
				{
					this.startedChargingTick = Find.TickManager.TicksGame + Rand.Range(0, this.Props.chargeIntervalTicks);
				}
				if (this.stunner == null)
				{
					this.stunner = new StunHandler(this.parent);
				}
			}
		}

		// Token: 0x06010786 RID: 67462 RVA: 0x004C080C File Offset: 0x004BEA0C
		[CompilerGenerated]
		private Vector3 <BreakShieldEmp>g__PosAtIndex|66_0(int index, ref CompProjectileInterceptor.<>c__DisplayClass66_0 A_2)
		{
			return new Vector3(this.Props.radius * Mathf.Cos(A_2.fTheta * (float)index) + A_2.center.x, 0f, this.Props.radius * Mathf.Sin(A_2.fTheta * (float)index) + A_2.center.z);
		}

		// Token: 0x0400ABBD RID: 43965
		private int lastInterceptTicks = -999999;

		// Token: 0x0400ABBE RID: 43966
		private int startedChargingTick = -1;

		// Token: 0x0400ABBF RID: 43967
		private bool shutDown;

		// Token: 0x0400ABC0 RID: 43968
		private StunHandler stunner;

		// Token: 0x0400ABC1 RID: 43969
		private Sustainer sustainer;

		// Token: 0x0400ABC2 RID: 43970
		public int currentHitPoints = -1;

		// Token: 0x0400ABC3 RID: 43971
		public int? maxHitPointsOverride;

		// Token: 0x0400ABC4 RID: 43972
		private int activatedTick = -999999;

		// Token: 0x0400ABC5 RID: 43973
		private float lastInterceptAngle;

		// Token: 0x0400ABC6 RID: 43974
		private bool drawInterceptCone;

		// Token: 0x0400ABC7 RID: 43975
		private bool debugInterceptNonHostileProjectiles;

		// Token: 0x0400ABC8 RID: 43976
		private static readonly Material ForceFieldMat = MaterialPool.MatFrom("Other/ForceField", ShaderDatabase.MoteGlow);

		// Token: 0x0400ABC9 RID: 43977
		private static readonly Material ForceFieldConeMat = MaterialPool.MatFrom("Other/ForceFieldCone", ShaderDatabase.MoteGlow);

		// Token: 0x0400ABCA RID: 43978
		private static readonly MaterialPropertyBlock MatPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x0400ABCB RID: 43979
		private const float TextureActualRingSizeFactor = 1.1601562f;

		// Token: 0x0400ABCC RID: 43980
		private static readonly Color InactiveColor = new Color(0.2f, 0.2f, 0.2f);
	}
}
