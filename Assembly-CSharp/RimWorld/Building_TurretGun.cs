using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002982 RID: 10626
	[StaticConstructorOnStartup]
	public class Building_TurretGun : Building_Turret
	{
		// Token: 0x170023A5 RID: 9125
		// (get) Token: 0x0600E97E RID: 59774 RVA: 0x0043EEB0 File Offset: 0x0043D0B0
		public bool Active
		{
			get
			{
				return (this.powerComp == null || this.powerComp.PowerOn) && (this.dormantComp == null || this.dormantComp.Awake) && (this.initiatableComp == null || this.initiatableComp.Initiated) && (this.interactableComp == null || this.burstActivated) && (this.powerCellComp == null || !this.powerCellComp.depleted) && (this.hackableComp == null || !this.hackableComp.IsHacked);
			}
		}

		// Token: 0x170023A6 RID: 9126
		// (get) Token: 0x0600E97F RID: 59775 RVA: 0x0043EF3B File Offset: 0x0043D13B
		public CompEquippable GunCompEq
		{
			get
			{
				return this.gun.TryGetComp<CompEquippable>();
			}
		}

		// Token: 0x170023A7 RID: 9127
		// (get) Token: 0x0600E980 RID: 59776 RVA: 0x0043EF48 File Offset: 0x0043D148
		public override LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTargetInt;
			}
		}

		// Token: 0x170023A8 RID: 9128
		// (get) Token: 0x0600E981 RID: 59777 RVA: 0x0043EF50 File Offset: 0x0043D150
		private bool WarmingUp
		{
			get
			{
				return this.burstWarmupTicksLeft > 0;
			}
		}

		// Token: 0x170023A9 RID: 9129
		// (get) Token: 0x0600E982 RID: 59778 RVA: 0x0043EF5B File Offset: 0x0043D15B
		public override Verb AttackVerb
		{
			get
			{
				return this.GunCompEq.PrimaryVerb;
			}
		}

		// Token: 0x170023AA RID: 9130
		// (get) Token: 0x0600E983 RID: 59779 RVA: 0x0043EF68 File Offset: 0x0043D168
		public bool IsMannable
		{
			get
			{
				return this.mannableComp != null;
			}
		}

		// Token: 0x170023AB RID: 9131
		// (get) Token: 0x0600E984 RID: 59780 RVA: 0x0043EF73 File Offset: 0x0043D173
		private bool PlayerControlled
		{
			get
			{
				return (base.Faction == Faction.OfPlayer || this.MannedByColonist) && !this.MannedByNonColonist && !this.IsActivable;
			}
		}

		// Token: 0x170023AC RID: 9132
		// (get) Token: 0x0600E985 RID: 59781 RVA: 0x0043EF9D File Offset: 0x0043D19D
		protected virtual bool CanSetForcedTarget
		{
			get
			{
				return this.mannableComp != null && this.PlayerControlled;
			}
		}

		// Token: 0x170023AD RID: 9133
		// (get) Token: 0x0600E986 RID: 59782 RVA: 0x0043EFAF File Offset: 0x0043D1AF
		private bool CanToggleHoldFire
		{
			get
			{
				return this.PlayerControlled;
			}
		}

		// Token: 0x170023AE RID: 9134
		// (get) Token: 0x0600E987 RID: 59783 RVA: 0x0043EFB7 File Offset: 0x0043D1B7
		private bool IsMortar
		{
			get
			{
				return this.def.building.IsMortar;
			}
		}

		// Token: 0x170023AF RID: 9135
		// (get) Token: 0x0600E988 RID: 59784 RVA: 0x0043EFC9 File Offset: 0x0043D1C9
		private bool IsMortarOrProjectileFliesOverhead
		{
			get
			{
				return this.AttackVerb.ProjectileFliesOverhead() || this.IsMortar;
			}
		}

		// Token: 0x170023B0 RID: 9136
		// (get) Token: 0x0600E989 RID: 59785 RVA: 0x0043EFE0 File Offset: 0x0043D1E0
		private bool IsActivable
		{
			get
			{
				return this.interactableComp != null;
			}
		}

		// Token: 0x170023B1 RID: 9137
		// (get) Token: 0x0600E98A RID: 59786 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool HideForceTargetGizmo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170023B2 RID: 9138
		// (get) Token: 0x0600E98B RID: 59787 RVA: 0x0043EFEB File Offset: 0x0043D1EB
		public TurretTop Top
		{
			get
			{
				return this.top;
			}
		}

		// Token: 0x170023B3 RID: 9139
		// (get) Token: 0x0600E98C RID: 59788 RVA: 0x0043EFF4 File Offset: 0x0043D1F4
		private bool CanExtractShell
		{
			get
			{
				if (!this.PlayerControlled)
				{
					return false;
				}
				CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
				return compChangeableProjectile != null && compChangeableProjectile.Loaded;
			}
		}

		// Token: 0x170023B4 RID: 9140
		// (get) Token: 0x0600E98D RID: 59789 RVA: 0x0043F022 File Offset: 0x0043D222
		private bool MannedByColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x170023B5 RID: 9141
		// (get) Token: 0x0600E98E RID: 59790 RVA: 0x0043F052 File Offset: 0x0043D252
		private bool MannedByNonColonist
		{
			get
			{
				return this.mannableComp != null && this.mannableComp.ManningPawn != null && this.mannableComp.ManningPawn.Faction != Faction.OfPlayer;
			}
		}

		// Token: 0x0600E98F RID: 59791 RVA: 0x0043F085 File Offset: 0x0043D285
		public Building_TurretGun()
		{
			this.top = new TurretTop(this);
		}

		// Token: 0x0600E990 RID: 59792 RVA: 0x0043F0A4 File Offset: 0x0043D2A4
		public override void PostMake()
		{
			base.PostMake();
			this.burstCooldownTicksLeft = this.def.building.turretInitialCooldownTime.SecondsToTicks();
			this.MakeGun();
		}

		// Token: 0x0600E991 RID: 59793 RVA: 0x0043F0D0 File Offset: 0x0043D2D0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.dormantComp = base.GetComp<CompCanBeDormant>();
			this.initiatableComp = base.GetComp<CompInitiatable>();
			this.powerComp = base.GetComp<CompPowerTrader>();
			this.mannableComp = base.GetComp<CompMannable>();
			this.interactableComp = base.GetComp<CompInteractable>();
			this.refuelableComp = base.GetComp<CompRefuelable>();
			this.powerCellComp = base.GetComp<CompMechPowerCell>();
			this.hackableComp = base.GetComp<CompHackable>();
			if (!respawningAfterLoad)
			{
				this.top.SetRotationFromOrientation();
			}
		}

		// Token: 0x0600E992 RID: 59794 RVA: 0x0043F153 File Offset: 0x0043D353
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			this.ResetCurrentTarget();
			Effecter effecter = this.progressBarEffecter;
			if (effecter == null)
			{
				return;
			}
			effecter.Cleanup();
		}

		// Token: 0x0600E993 RID: 59795 RVA: 0x0043F174 File Offset: 0x0043D374
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.burstCooldownTicksLeft, "burstCooldownTicksLeft", 0, false);
			Scribe_Values.Look<int>(ref this.burstWarmupTicksLeft, "burstWarmupTicksLeft", 0, false);
			Scribe_TargetInfo.Look(ref this.currentTargetInt, "currentTarget");
			Scribe_Values.Look<bool>(ref this.holdFire, "holdFire", false, false);
			Scribe_Values.Look<bool>(ref this.burstActivated, "burstActivated", false, false);
			Scribe_Deep.Look<Thing>(ref this.gun, "gun", Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.gun == null)
				{
					Log.Error("Turret had null gun after loading. Recreating.");
					this.MakeGun();
					return;
				}
				this.UpdateGunVerbs();
			}
		}

		// Token: 0x0600E994 RID: 59796 RVA: 0x0043F224 File Offset: 0x0043D424
		public override AcceptanceReport ClaimableBy(Faction by)
		{
			AcceptanceReport result = base.ClaimableBy(by);
			if (!result.Accepted)
			{
				return result;
			}
			if (this.mannableComp != null && this.mannableComp.ManningPawn != null)
			{
				return false;
			}
			if (this.Active && this.mannableComp == null)
			{
				return false;
			}
			if (((this.dormantComp != null && !this.dormantComp.Awake) || (this.initiatableComp != null && !this.initiatableComp.Initiated)) && (this.powerComp == null || this.powerComp.PowerOn))
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600E995 RID: 59797 RVA: 0x0043F2C4 File Offset: 0x0043D4C4
		public override void OrderAttack(LocalTargetInfo targ)
		{
			if (!targ.IsValid)
			{
				if (this.forcedTarget.IsValid)
				{
					this.ResetForcedTarget();
				}
				return;
			}
			if ((targ.Cell - base.Position).LengthHorizontal < this.AttackVerb.verbProps.EffectiveMinRange(targ, this))
			{
				Messages.Message("MessageTargetBelowMinimumRange".Translate(), this, MessageTypeDefOf.RejectInput, false);
				return;
			}
			if ((targ.Cell - base.Position).LengthHorizontal > this.AttackVerb.EffectiveRange)
			{
				Messages.Message("MessageTargetBeyondMaximumRange".Translate(), this, MessageTypeDefOf.RejectInput, false);
				return;
			}
			if (this.forcedTarget != targ)
			{
				this.forcedTarget = targ;
				if (this.burstCooldownTicksLeft <= 0)
				{
					this.TryStartShootSomething(false);
				}
			}
			if (this.holdFire)
			{
				Messages.Message("MessageTurretWontFireBecauseHoldFire".Translate(this.def.label), this, MessageTypeDefOf.RejectInput, false);
			}
		}

		// Token: 0x0600E996 RID: 59798 RVA: 0x0043F3E4 File Offset: 0x0043D5E4
		protected override void Tick()
		{
			base.Tick();
			if (this.CanExtractShell && this.MannedByColonist)
			{
				CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
				if (!compChangeableProjectile.allowedShellsSettings.AllowedToAccept(compChangeableProjectile.LoadedShell))
				{
					this.ExtractShell();
				}
			}
			if (this.forcedTarget.IsValid && !this.CanSetForcedTarget)
			{
				this.ResetForcedTarget();
			}
			if (!this.CanToggleHoldFire)
			{
				this.holdFire = false;
			}
			if (this.forcedTarget.ThingDestroyed)
			{
				this.ResetForcedTarget();
			}
			if (this.Active && (this.mannableComp == null || this.mannableComp.MannedNow) && !base.IsStunned && base.Spawned)
			{
				this.GunCompEq.verbTracker.VerbsTick();
				if (this.AttackVerb.state != VerbState.Bursting)
				{
					this.burstActivated = false;
					if (this.WarmingUp)
					{
						this.burstWarmupTicksLeft--;
						if (this.burstWarmupTicksLeft <= 0)
						{
							this.BeginBurst();
						}
					}
					else
					{
						if (this.burstCooldownTicksLeft > 0)
						{
							this.burstCooldownTicksLeft--;
							if (this.IsMortar)
							{
								if (this.progressBarEffecter == null)
								{
									this.progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
								}
								this.progressBarEffecter.EffectTick(this, TargetInfo.Invalid);
								MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBarEffecter.children[0]).mote;
								mote.progress = 1f - (float)Math.Max(this.burstCooldownTicksLeft, 0) / (float)this.BurstCooldownTime().SecondsToTicks();
								mote.offsetZ = -0.8f;
							}
						}
						if (this.burstCooldownTicksLeft <= 0 && this.IsHashIntervalTick(15))
						{
							this.TryStartShootSomething(true);
						}
					}
					this.top.TurretTopTick();
					return;
				}
			}
			else
			{
				this.ResetCurrentTarget();
			}
		}

		// Token: 0x0600E997 RID: 59799 RVA: 0x0043F5BE File Offset: 0x0043D7BE
		public void TryActivateBurst()
		{
			this.burstActivated = true;
			this.TryStartShootSomething(true);
		}

		// Token: 0x0600E998 RID: 59800 RVA: 0x0043F5D0 File Offset: 0x0043D7D0
		public void TryStartShootSomething(bool canBeginBurstImmediately)
		{
			if (this.progressBarEffecter != null)
			{
				this.progressBarEffecter.Cleanup();
				this.progressBarEffecter = null;
			}
			if (!base.Spawned || (this.holdFire && this.CanToggleHoldFire) || (this.AttackVerb.ProjectileFliesOverhead() && base.Map.roofGrid.Roofed(base.Position)) || !this.AttackVerb.Available())
			{
				this.ResetCurrentTarget();
				return;
			}
			bool isValid = this.currentTargetInt.IsValid;
			if (this.forcedTarget.IsValid)
			{
				this.currentTargetInt = this.forcedTarget;
			}
			else
			{
				this.currentTargetInt = this.TryFindNewTarget();
			}
			if (!isValid && this.currentTargetInt.IsValid && this.def.building.playTargetAcquiredSound)
			{
				SoundDefOf.TurretAcquireTarget.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
			}
			if (!this.currentTargetInt.IsValid)
			{
				this.ResetCurrentTarget();
				return;
			}
			float randomInRange = this.def.building.turretBurstWarmupTime.RandomInRange;
			if (randomInRange > 0f)
			{
				this.burstWarmupTicksLeft = randomInRange.SecondsToTicks();
				return;
			}
			if (canBeginBurstImmediately)
			{
				this.BeginBurst();
				return;
			}
			this.burstWarmupTicksLeft = 1;
		}

		// Token: 0x0600E999 RID: 59801 RVA: 0x0043F70C File Offset: 0x0043D90C
		public virtual LocalTargetInfo TryFindNewTarget()
		{
			IAttackTargetSearcher attackTargetSearcher = this.TargSearcher();
			Faction faction = attackTargetSearcher.Thing.Faction;
			float range = this.AttackVerb.EffectiveRange;
			Building t;
			if (Rand.Value < 0.5f && this.AttackVerb.ProjectileFliesOverhead() && faction.HostileTo(Faction.OfPlayer) && base.Map.listerBuildings.allBuildingsColonist.Where(delegate(Building x)
			{
				float num = this.AttackVerb.verbProps.EffectiveMinRange(x, this);
				float num2 = (float)x.Position.DistanceToSquared(this.Position);
				return num2 > num * num && num2 < range * range;
			}).TryRandomElement(out t))
			{
				return t;
			}
			TargetScanFlags targetScanFlags = TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
			if (!this.AttackVerb.ProjectileFliesOverhead())
			{
				targetScanFlags |= TargetScanFlags.NeedLOSToAll;
				targetScanFlags |= TargetScanFlags.LOSBlockableByGas;
			}
			if (this.AttackVerb.IsIncendiary_Ranged())
			{
				targetScanFlags |= TargetScanFlags.NeedNonBurning;
			}
			if (this.IsMortar)
			{
				targetScanFlags |= TargetScanFlags.NeedNotUnderThickRoof;
			}
			return (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(attackTargetSearcher, targetScanFlags, new Predicate<Thing>(this.IsValidTarget), 0f, 9999f);
		}

		// Token: 0x0600E99A RID: 59802 RVA: 0x0043F80A File Offset: 0x0043DA0A
		private IAttackTargetSearcher TargSearcher()
		{
			if (this.mannableComp != null && this.mannableComp.MannedNow)
			{
				return this.mannableComp.ManningPawn;
			}
			return this;
		}

		// Token: 0x0600E99B RID: 59803 RVA: 0x0043F830 File Offset: 0x0043DA30
		private bool IsValidTarget(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (base.Faction == Faction.OfPlayer && pawn.IsPrisoner)
				{
					return false;
				}
				if (this.AttackVerb.ProjectileFliesOverhead())
				{
					RoofDef roofDef = base.Map.roofGrid.RoofAt(t.Position);
					if (roofDef != null && roofDef.isThickRoof)
					{
						return false;
					}
				}
				if (this.mannableComp == null)
				{
					return !GenAI.MachinesLike(base.Faction, pawn);
				}
				if (pawn.RaceProps.Animal && pawn.Faction == Faction.OfPlayer)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600E99C RID: 59804 RVA: 0x0043F8C4 File Offset: 0x0043DAC4
		protected virtual void BeginBurst()
		{
			this.AttackVerb.TryStartCastOn(this.CurrentTarget, false, true, false, false);
			base.OnAttackedTarget(this.CurrentTarget);
		}

		// Token: 0x0600E99D RID: 59805 RVA: 0x0043F8E8 File Offset: 0x0043DAE8
		protected virtual void BurstComplete()
		{
			this.burstCooldownTicksLeft = this.BurstCooldownTime().SecondsToTicks();
		}

		// Token: 0x0600E99E RID: 59806 RVA: 0x0043F8FB File Offset: 0x0043DAFB
		protected virtual float BurstCooldownTime()
		{
			if (this.def.building.turretBurstCooldownTime >= 0f)
			{
				return this.def.building.turretBurstCooldownTime;
			}
			return this.AttackVerb.verbProps.defaultCooldownTime;
		}

		// Token: 0x0600E99F RID: 59807 RVA: 0x0043F938 File Offset: 0x0043DB38
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string inspectString = base.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectString);
			}
			if (this.AttackVerb.verbProps.minRange > 0f)
			{
				stringBuilder.AppendLine("MinimumRange".Translate() + ": " + this.AttackVerb.verbProps.minRange.ToString("F0"));
			}
			if (base.Spawned && this.IsMortarOrProjectileFliesOverhead && base.Position.Roofed(base.Map))
			{
				stringBuilder.AppendLine("CannotFire".Translate() + ": " + "Roofed".Translate().CapitalizeFirst());
			}
			else if (base.Spawned && this.burstCooldownTicksLeft > 0 && this.BurstCooldownTime() > 5f)
			{
				stringBuilder.AppendLine("CanFireIn".Translate() + ": " + this.burstCooldownTicksLeft.ToStringSecondsFromTicks());
			}
			CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
			if (compChangeableProjectile != null)
			{
				if (compChangeableProjectile.Loaded)
				{
					stringBuilder.AppendLine("ShellLoaded".Translate(compChangeableProjectile.LoadedShell.LabelCap, compChangeableProjectile.LoadedShell));
				}
				else
				{
					stringBuilder.AppendLine("ShellNotLoaded".Translate());
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600E9A0 RID: 59808 RVA: 0x0043FAD0 File Offset: 0x0043DCD0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			Vector3 zero = Vector3.zero;
			float recoilAngleOffset = 0f;
			if (this.IsMortar)
			{
				EquipmentUtility.Recoil(this.def.building.turretGunDef, (Verb_LaunchProjectile)this.AttackVerb, out zero, out recoilAngleOffset, this.top.CurRotation);
			}
			this.top.DrawTurret(drawLoc, zero, recoilAngleOffset);
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x0600E9A1 RID: 59809 RVA: 0x0043FB38 File Offset: 0x0043DD38
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			float effectiveRange = this.AttackVerb.EffectiveRange;
			if (effectiveRange < 90f)
			{
				GenDraw.DrawRadiusRing(base.Position, effectiveRange);
			}
			float num = this.AttackVerb.verbProps.EffectiveMinRange(true);
			if (num < 90f && num > 0.1f)
			{
				GenDraw.DrawRadiusRing(base.Position, num);
			}
			if (this.WarmingUp)
			{
				int degreesWide = (int)((float)this.burstWarmupTicksLeft * 0.5f);
				GenDraw.DrawAimPie(this, this.CurrentTarget, degreesWide, (float)this.def.size.x * 0.5f);
			}
			if (this.forcedTarget.IsValid && (!this.forcedTarget.HasThing || this.forcedTarget.Thing.Spawned))
			{
				Vector3 vector;
				if (this.forcedTarget.HasThing)
				{
					vector = this.forcedTarget.Thing.TrueCenter();
				}
				else
				{
					vector = this.forcedTarget.Cell.ToVector3Shifted();
				}
				Vector3 a = this.TrueCenter();
				vector.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				a.y = vector.y;
				GenDraw.DrawLineBetween(a, vector, Building_TurretGun.ForcedTargetLineMat, 0.2f);
			}
		}

		// Token: 0x0600E9A2 RID: 59810 RVA: 0x0043FC6D File Offset: 0x0043DE6D
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.CanExtractShell)
			{
				CompChangeableProjectile compChangeableProjectile = this.gun.TryGetComp<CompChangeableProjectile>();
				yield return new Command_Action
				{
					defaultLabel = "CommandExtractShell".Translate(),
					defaultDesc = "CommandExtractShellDesc".Translate(),
					icon = compChangeableProjectile.LoadedShell.uiIcon,
					iconAngle = compChangeableProjectile.LoadedShell.uiIconAngle,
					iconOffset = compChangeableProjectile.LoadedShell.uiIconOffset,
					iconDrawScale = GenUI.IconDrawScale(compChangeableProjectile.LoadedShell),
					action = delegate()
					{
						this.ExtractShell();
					}
				};
			}
			CompChangeableProjectile compChangeableProjectile2 = this.gun.TryGetComp<CompChangeableProjectile>();
			if (compChangeableProjectile2 != null)
			{
				StorageSettings storeSettings = compChangeableProjectile2.GetStoreSettings();
				foreach (Gizmo gizmo2 in StorageSettingsClipboard.CopyPasteGizmosFor(storeSettings))
				{
					yield return gizmo2;
				}
				enumerator = null;
			}
			if (!this.HideForceTargetGizmo)
			{
				if (this.CanSetForcedTarget)
				{
					Command_VerbTarget command_VerbTarget = new Command_VerbTarget();
					command_VerbTarget.defaultLabel = "CommandSetForceAttackTarget".Translate();
					command_VerbTarget.defaultDesc = "CommandSetForceAttackTargetDesc".Translate();
					command_VerbTarget.icon = ContentFinder<Texture2D>.Get("UI/Commands/Attack", true);
					command_VerbTarget.verb = this.AttackVerb;
					command_VerbTarget.hotKey = KeyBindingDefOf.Misc4;
					command_VerbTarget.drawRadius = false;
					command_VerbTarget.requiresAvailableVerb = false;
					if (base.Spawned && this.IsMortarOrProjectileFliesOverhead && base.Position.Roofed(base.Map))
					{
						command_VerbTarget.Disable("CannotFire".Translate() + ": " + "Roofed".Translate().CapitalizeFirst());
					}
					yield return command_VerbTarget;
				}
				if (this.forcedTarget.IsValid)
				{
					Command_Action command_Action = new Command_Action();
					command_Action.defaultLabel = "CommandStopForceAttack".Translate();
					command_Action.defaultDesc = "CommandStopForceAttackDesc".Translate();
					command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/Halt", true);
					command_Action.action = delegate()
					{
						this.ResetForcedTarget();
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					};
					if (!this.forcedTarget.IsValid)
					{
						command_Action.Disable("CommandStopAttackFailNotForceAttacking".Translate());
					}
					command_Action.hotKey = KeyBindingDefOf.Misc5;
					yield return command_Action;
				}
			}
			if (this.CanToggleHoldFire)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandHoldFire".Translate(),
					defaultDesc = "CommandHoldFireDesc".Translate(),
					icon = ContentFinder<Texture2D>.Get("UI/Commands/HoldFire", true),
					hotKey = KeyBindingDefOf.Misc6,
					toggleAction = delegate()
					{
						this.holdFire = !this.holdFire;
						if (this.holdFire)
						{
							this.ResetForcedTarget();
						}
					},
					isActive = (() => this.holdFire)
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600E9A3 RID: 59811 RVA: 0x0043FC80 File Offset: 0x0043DE80
		private void ExtractShell()
		{
			GenPlace.TryPlaceThing(this.gun.TryGetComp<CompChangeableProjectile>().RemoveShell(), base.Position, base.Map, ThingPlaceMode.Near, null, null, null, 1);
		}

		// Token: 0x0600E9A4 RID: 59812 RVA: 0x0043FCBC File Offset: 0x0043DEBC
		private void ResetForcedTarget()
		{
			this.forcedTarget = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
			if (this.burstCooldownTicksLeft <= 0)
			{
				this.TryStartShootSomething(false);
			}
		}

		// Token: 0x0600E9A5 RID: 59813 RVA: 0x0043FCE0 File Offset: 0x0043DEE0
		private void ResetCurrentTarget()
		{
			this.currentTargetInt = LocalTargetInfo.Invalid;
			this.burstWarmupTicksLeft = 0;
		}

		// Token: 0x0600E9A6 RID: 59814 RVA: 0x0043FCF4 File Offset: 0x0043DEF4
		public void MakeGun()
		{
			this.gun = ThingMaker.MakeThing(this.def.building.turretGunDef, null);
			this.UpdateGunVerbs();
		}

		// Token: 0x0600E9A7 RID: 59815 RVA: 0x0043FD18 File Offset: 0x0043DF18
		private void UpdateGunVerbs()
		{
			List<Verb> allVerbs = this.gun.TryGetComp<CompEquippable>().AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				Verb verb = allVerbs[i];
				verb.caster = this;
				verb.castCompleteCallback = new Action(this.BurstComplete);
			}
		}

		// Token: 0x04009B69 RID: 39785
		protected int burstCooldownTicksLeft;

		// Token: 0x04009B6A RID: 39786
		protected int burstWarmupTicksLeft;

		// Token: 0x04009B6B RID: 39787
		protected LocalTargetInfo currentTargetInt = LocalTargetInfo.Invalid;

		// Token: 0x04009B6C RID: 39788
		private bool holdFire;

		// Token: 0x04009B6D RID: 39789
		private bool burstActivated;

		// Token: 0x04009B6E RID: 39790
		public Thing gun;

		// Token: 0x04009B6F RID: 39791
		protected TurretTop top;

		// Token: 0x04009B70 RID: 39792
		protected CompPowerTrader powerComp;

		// Token: 0x04009B71 RID: 39793
		protected CompCanBeDormant dormantComp;

		// Token: 0x04009B72 RID: 39794
		protected CompInitiatable initiatableComp;

		// Token: 0x04009B73 RID: 39795
		protected CompMannable mannableComp;

		// Token: 0x04009B74 RID: 39796
		protected CompInteractable interactableComp;

		// Token: 0x04009B75 RID: 39797
		public CompRefuelable refuelableComp;

		// Token: 0x04009B76 RID: 39798
		protected Effecter progressBarEffecter;

		// Token: 0x04009B77 RID: 39799
		protected CompMechPowerCell powerCellComp;

		// Token: 0x04009B78 RID: 39800
		protected CompHackable hackableComp;

		// Token: 0x04009B79 RID: 39801
		private const int TryStartShootSomethingIntervalTicks = 15;

		// Token: 0x04009B7A RID: 39802
		public static Material ForcedTargetLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));
	}
}
