using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000D70 RID: 3440
	public abstract class Verb : ITargetingSource, IExposable, ILoadReferenceable
	{
		// Token: 0x17000DC5 RID: 3525
		// (get) Token: 0x06005781 RID: 22401 RVA: 0x001C33C8 File Offset: 0x001C15C8
		public IVerbOwner DirectOwner
		{
			get
			{
				return this.verbTracker.directOwner;
			}
		}

		// Token: 0x17000DC6 RID: 3526
		// (get) Token: 0x06005782 RID: 22402 RVA: 0x001C33D5 File Offset: 0x001C15D5
		public ImplementOwnerTypeDef ImplementOwnerType
		{
			get
			{
				return this.verbTracker.directOwner.ImplementOwnerTypeDef;
			}
		}

		// Token: 0x17000DC7 RID: 3527
		// (get) Token: 0x06005783 RID: 22403 RVA: 0x001C33E7 File Offset: 0x001C15E7
		public CompEquippable EquipmentCompSource
		{
			get
			{
				return this.DirectOwner as CompEquippable;
			}
		}

		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06005784 RID: 22404 RVA: 0x001C33F4 File Offset: 0x001C15F4
		public CompApparelReloadable ReloadableCompSource
		{
			get
			{
				return this.DirectOwner as CompApparelReloadable;
			}
		}

		// Token: 0x17000DC9 RID: 3529
		// (get) Token: 0x06005785 RID: 22405 RVA: 0x001C3401 File Offset: 0x001C1601
		public CompApparelVerbOwner_Charged VerbOwner_ChargedCompSource
		{
			get
			{
				return this.DirectOwner as CompApparelVerbOwner_Charged;
			}
		}

		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06005786 RID: 22406 RVA: 0x001C340E File Offset: 0x001C160E
		public ThingWithComps EquipmentSource
		{
			get
			{
				if (this.EquipmentCompSource != null)
				{
					return this.EquipmentCompSource.parent;
				}
				if (this.ReloadableCompSource != null)
				{
					return this.ReloadableCompSource.parent;
				}
				if (this.VerbOwner_ChargedCompSource != null)
				{
					return this.VerbOwner_ChargedCompSource.parent;
				}
				return null;
			}
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06005787 RID: 22407 RVA: 0x001C344D File Offset: 0x001C164D
		public HediffComp_VerbGiver HediffCompSource
		{
			get
			{
				return this.DirectOwner as HediffComp_VerbGiver;
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06005788 RID: 22408 RVA: 0x001C345A File Offset: 0x001C165A
		public Hediff HediffSource
		{
			get
			{
				if (this.HediffCompSource == null)
				{
					return null;
				}
				return this.HediffCompSource.parent;
			}
		}

		// Token: 0x17000DCD RID: 3533
		// (get) Token: 0x06005789 RID: 22409 RVA: 0x001C3471 File Offset: 0x001C1671
		public Pawn_MeleeVerbs_TerrainSource TerrainSource
		{
			get
			{
				return this.DirectOwner as Pawn_MeleeVerbs_TerrainSource;
			}
		}

		// Token: 0x17000DCE RID: 3534
		// (get) Token: 0x0600578A RID: 22410 RVA: 0x001C347E File Offset: 0x001C167E
		public TerrainDef TerrainDefSource
		{
			get
			{
				if (this.TerrainSource == null)
				{
					return null;
				}
				return this.TerrainSource.def;
			}
		}

		// Token: 0x17000DCF RID: 3535
		// (get) Token: 0x0600578B RID: 22411 RVA: 0x001C3495 File Offset: 0x001C1695
		public virtual Thing Caster
		{
			get
			{
				return this.caster;
			}
		}

		// Token: 0x17000DD0 RID: 3536
		// (get) Token: 0x0600578C RID: 22412 RVA: 0x001C349D File Offset: 0x001C169D
		public virtual Pawn CasterPawn
		{
			get
			{
				return this.caster as Pawn;
			}
		}

		// Token: 0x17000DD1 RID: 3537
		// (get) Token: 0x0600578D RID: 22413 RVA: 0x000FD030 File Offset: 0x000FB230
		public virtual Verb GetVerb
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000DD2 RID: 3538
		// (get) Token: 0x0600578E RID: 22414 RVA: 0x001C34AA File Offset: 0x001C16AA
		public virtual bool CasterIsPawn
		{
			get
			{
				return this.caster is Pawn;
			}
		}

		// Token: 0x17000DD3 RID: 3539
		// (get) Token: 0x0600578F RID: 22415 RVA: 0x001C34BA File Offset: 0x001C16BA
		public virtual bool Targetable
		{
			get
			{
				return this.verbProps.targetable;
			}
		}

		// Token: 0x17000DD4 RID: 3540
		// (get) Token: 0x06005790 RID: 22416 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DD5 RID: 3541
		// (get) Token: 0x06005791 RID: 22417 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06005792 RID: 22418 RVA: 0x001C34C7 File Offset: 0x001C16C7
		public LocalTargetInfo CurrentTarget
		{
			get
			{
				return this.currentTarget;
			}
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x06005793 RID: 22419 RVA: 0x001C34CF File Offset: 0x001C16CF
		public LocalTargetInfo CurrentDestination
		{
			get
			{
				return this.currentDestination;
			}
		}

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x06005794 RID: 22420 RVA: 0x001C34D7 File Offset: 0x001C16D7
		public int LastShotTick
		{
			get
			{
				return this.lastShotTick;
			}
		}

		// Token: 0x17000DD9 RID: 3545
		// (get) Token: 0x06005795 RID: 22421 RVA: 0x001C34DF File Offset: 0x001C16DF
		public virtual TargetingParameters targetParams
		{
			get
			{
				return this.verbProps.targetParams;
			}
		}

		// Token: 0x17000DDA RID: 3546
		// (get) Token: 0x06005796 RID: 22422 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000DDB RID: 3547
		// (get) Token: 0x06005797 RID: 22423 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual int ShotsPerBurst
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000DDC RID: 3548
		// (get) Token: 0x06005798 RID: 22424 RVA: 0x001C34EC File Offset: 0x001C16EC
		public virtual Texture2D UIIcon
		{
			get
			{
				if (this.verbProps.commandIcon != null)
				{
					if (this.commandIconCached == null)
					{
						this.commandIconCached = ContentFinder<Texture2D>.Get(this.verbProps.commandIcon, true);
					}
					return this.commandIconCached;
				}
				if (this.EquipmentSource != null)
				{
					return this.EquipmentSource.def.uiIcon;
				}
				return BaseContent.BadTex;
			}
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x06005799 RID: 22425 RVA: 0x001C3550 File Offset: 0x001C1750
		public bool Bursting
		{
			get
			{
				return this.burstShotsLeft > 0;
			}
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x0600579A RID: 22426 RVA: 0x001C355B File Offset: 0x001C175B
		public virtual bool IsMeleeAttack
		{
			get
			{
				return this.verbProps.IsMeleeAttack;
			}
		}

		// Token: 0x17000DDF RID: 3551
		// (get) Token: 0x0600579B RID: 22427 RVA: 0x001C3568 File Offset: 0x001C1768
		public bool BuggedAfterLoading
		{
			get
			{
				return this.verbProps == null;
			}
		}

		// Token: 0x17000DE0 RID: 3552
		// (get) Token: 0x0600579C RID: 22428 RVA: 0x001C3573 File Offset: 0x001C1773
		public bool WarmingUp
		{
			get
			{
				return this.WarmupStance != null;
			}
		}

		// Token: 0x17000DE1 RID: 3553
		// (get) Token: 0x0600579D RID: 22429 RVA: 0x001C3580 File Offset: 0x001C1780
		public Stance_Warmup WarmupStance
		{
			get
			{
				if (this.CasterPawn == null || !this.CasterPawn.Spawned)
				{
					return null;
				}
				Stance_Warmup stance_Warmup = this.CasterPawn.stances.curStance as Stance_Warmup;
				if (stance_Warmup == null || stance_Warmup.verb != this)
				{
					return null;
				}
				return stance_Warmup;
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x0600579E RID: 22430 RVA: 0x001C35C9 File Offset: 0x001C17C9
		public int WarmupTicksLeft
		{
			get
			{
				if (this.WarmupStance == null)
				{
					return 0;
				}
				return this.WarmupStance.ticksLeft;
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x0600579F RID: 22431 RVA: 0x001C35E0 File Offset: 0x001C17E0
		public float WarmupProgress
		{
			get
			{
				return 1f - this.WarmupTicksLeft.TicksToSeconds() / this.verbProps.warmupTime;
			}
		}

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x060057A0 RID: 22432 RVA: 0x001C35FF File Offset: 0x001C17FF
		public virtual string ReportLabel
		{
			get
			{
				return this.verbProps.label;
			}
		}

		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x060057A1 RID: 22433 RVA: 0x001C360C File Offset: 0x001C180C
		public virtual float EffectiveRange
		{
			get
			{
				return this.verbProps.AdjustedRange(this, this.Caster);
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x060057A2 RID: 22434 RVA: 0x001C3620 File Offset: 0x001C1820
		public virtual float? AimAngleOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x060057A3 RID: 22435 RVA: 0x001C3636 File Offset: 0x001C1836
		public bool NonInterruptingSelfCast
		{
			get
			{
				return this.verbProps.nonInterruptingSelfCast || this.nonInterruptingSelfCast;
			}
		}

		// Token: 0x17000DE8 RID: 3560
		// (get) Token: 0x060057A4 RID: 22436 RVA: 0x001C3650 File Offset: 0x001C1850
		public int TicksBetweenBurstShots
		{
			get
			{
				if (this.cachedTicksBetweenBurstShots == null)
				{
					float num = (float)this.verbProps.ticksBetweenBurstShots;
					CompUniqueWeapon compUniqueWeapon;
					if (this.EquipmentSource != null && this.EquipmentSource.TryGetComp(out compUniqueWeapon))
					{
						foreach (WeaponTraitDef weaponTraitDef in compUniqueWeapon.TraitsListForReading)
						{
							num /= weaponTraitDef.burstShotSpeedMultiplier;
						}
					}
					this.cachedTicksBetweenBurstShots = new int?(Mathf.RoundToInt(num));
				}
				return this.cachedTicksBetweenBurstShots.Value;
			}
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x060057A5 RID: 22437 RVA: 0x001C36F4 File Offset: 0x001C18F4
		public int BurstShotCount
		{
			get
			{
				if (this.cachedBurstShotCount == null)
				{
					float num = (float)this.verbProps.burstShotCount;
					CompUniqueWeapon compUniqueWeapon;
					if (this.EquipmentSource != null && this.EquipmentSource.TryGetComp(out compUniqueWeapon))
					{
						foreach (WeaponTraitDef weaponTraitDef in compUniqueWeapon.TraitsListForReading)
						{
							num *= weaponTraitDef.burstShotCountMultiplier;
						}
					}
					this.cachedBurstShotCount = new int?(Mathf.CeilToInt(num));
				}
				return this.cachedBurstShotCount.Value;
			}
		}

		// Token: 0x060057A6 RID: 22438 RVA: 0x001C3798 File Offset: 0x001C1998
		public bool IsStillUsableBy(Pawn pawn)
		{
			return this.Available() && this.DirectOwner.VerbsStillUsableBy(pawn) && this.verbProps.GetDamageFactorFor(this, pawn) != 0f && (!pawn.IsSubhuman || this.verbProps.category != VerbCategory.Ignite);
		}

		// Token: 0x060057A7 RID: 22439 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool IsUsableOn(Thing target)
		{
			return true;
		}

		// Token: 0x060057A8 RID: 22440 RVA: 0x001C37F0 File Offset: 0x001C19F0
		public virtual void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.loadID, "loadID", null, false);
			Scribe_Values.Look<VerbState>(ref this.state, "state", VerbState.Idle, false);
			Scribe_TargetInfo.Look(ref this.currentTarget, "currentTarget");
			Scribe_TargetInfo.Look(ref this.currentDestination, "currentDestination");
			Scribe_Values.Look<int>(ref this.burstShotsLeft, "burstShotsLeft", 0, false);
			Scribe_Values.Look<int>(ref this.ticksToNextBurstShot, "ticksToNextBurstShot", 0, false);
			Scribe_Values.Look<int>(ref this.lastShotTick, "lastShotTick", 0, false);
			Scribe_Values.Look<bool>(ref this.surpriseAttack, "surpriseAttack", false, false);
			Scribe_Values.Look<bool>(ref this.canHitNonTargetPawnsNow, "canHitNonTargetPawnsNow", false, false);
			Scribe_Values.Look<bool>(ref this.preventFriendlyFire, "preventFriendlyFire", false, false);
			Scribe_Values.Look<bool>(ref this.nonInterruptingSelfCast, "nonInterruptingSelfCast", false, false);
		}

		// Token: 0x060057A9 RID: 22441 RVA: 0x001C38BF File Offset: 0x001C1ABF
		public string GetUniqueLoadID()
		{
			return "Verb_" + this.loadID;
		}

		// Token: 0x060057AA RID: 22442 RVA: 0x001C38D1 File Offset: 0x001C1AD1
		public static string CalculateUniqueLoadID(IVerbOwner owner, Tool tool, ManeuverDef maneuver)
		{
			return string.Format("{0}_{1}_{2}", owner.UniqueVerbOwnerID(), (tool != null) ? tool.id : "NT", (maneuver != null) ? maneuver.defName : "NM");
		}

		// Token: 0x060057AB RID: 22443 RVA: 0x001C3903 File Offset: 0x001C1B03
		public static string CalculateUniqueLoadID(IVerbOwner owner, int index)
		{
			return string.Format("{0}_{1}", owner.UniqueVerbOwnerID(), index);
		}

		// Token: 0x060057AC RID: 22444 RVA: 0x001C391B File Offset: 0x001C1B1B
		public bool TryStartCastOn(LocalTargetInfo castTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false, bool nonInterruptingSelfCast = false)
		{
			return this.TryStartCastOn(castTarg, LocalTargetInfo.Invalid, surpriseAttack, canHitNonTargetPawns, preventFriendlyFire, nonInterruptingSelfCast);
		}

		// Token: 0x060057AD RID: 22445 RVA: 0x001C3930 File Offset: 0x001C1B30
		public virtual bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false, bool nonInterruptingSelfCast = false)
		{
			if (this.caster == null)
			{
				Log.Error("Verb " + this.GetUniqueLoadID() + " needs caster to work (possibly lost during saving/loading).");
				return false;
			}
			if (!this.caster.Spawned)
			{
				return false;
			}
			if (this.state == VerbState.Bursting || !this.CanHitTarget(castTarg))
			{
				return false;
			}
			if (this.CausesTimeSlowdown(castTarg))
			{
				Find.TickManager.slower.SignalForceNormalSpeed();
			}
			this.surpriseAttack = surpriseAttack;
			this.canHitNonTargetPawnsNow = canHitNonTargetPawns;
			this.preventFriendlyFire = preventFriendlyFire;
			this.nonInterruptingSelfCast = nonInterruptingSelfCast;
			this.currentTarget = castTarg;
			this.currentDestination = destTarg;
			if (this.CasterIsPawn && this.verbProps.warmupTime > 0f)
			{
				ShootLine newShootLine;
				if (!this.TryFindShootLineFromTo(this.caster.Position, castTarg, out newShootLine, false))
				{
					return false;
				}
				this.CasterPawn.Drawer.Notify_WarmingCastAlongLine(newShootLine, this.caster.Position);
				float statValue = this.CasterPawn.GetStatValue(StatDefOf.AimingDelayFactor, true, -1);
				int ticks = (this.verbProps.warmupTime * statValue).SecondsToTicks();
				this.CasterPawn.stances.SetStance(new Stance_Warmup(ticks, castTarg, this));
				if (this.verbProps.stunTargetOnCastStart && castTarg.Pawn != null)
				{
					castTarg.Pawn.stances.stunner.StunFor(ticks, null, false, true, false);
				}
			}
			else
			{
				Ability ability = this.verbTracker.directOwner as Ability;
				if (ability != null)
				{
					ability.lastCastTick = Find.TickManager.TicksGame;
				}
				this.WarmupComplete();
			}
			return true;
		}

		// Token: 0x060057AE RID: 22446 RVA: 0x001C3ABD File Offset: 0x001C1CBD
		public virtual void WarmupComplete()
		{
			this.burstShotsLeft = this.ShotsPerBurst;
			this.state = VerbState.Bursting;
			this.TryCastNextBurstShot();
		}

		// Token: 0x060057AF RID: 22447 RVA: 0x001C3AD8 File Offset: 0x001C1CD8
		public void VerbTick()
		{
			if (this.state == VerbState.Bursting)
			{
				if (this.caster.Spawned)
				{
					Pawn pawn = this.caster as Pawn;
					if (pawn == null || !pawn.stances.stunner.Stunned)
					{
						this.ticksToNextBurstShot--;
						if (this.ticksToNextBurstShot <= 0)
						{
							this.TryCastNextBurstShot();
						}
						this.BurstingTick();
						goto IL_62;
					}
				}
				this.Reset();
			}
			IL_62:
			for (int i = this.maintainedEffecters.Count - 1; i >= 0; i--)
			{
				Effecter item = this.maintainedEffecters[i].Item1;
				if (item.ticksLeft > 0)
				{
					TargetInfo item2 = this.maintainedEffecters[i].Item2;
					TargetInfo item3 = this.maintainedEffecters[i].Item3;
					item.EffectTick(item2, item3);
					item.ticksLeft--;
				}
				else
				{
					item.Cleanup();
					this.maintainedEffecters.RemoveAt(i);
				}
			}
		}

		// Token: 0x060057B0 RID: 22448 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void BurstingTick()
		{
		}

		// Token: 0x060057B1 RID: 22449 RVA: 0x001C3BCC File Offset: 0x001C1DCC
		public void AddEffecterToMaintain(Effecter eff, IntVec3 pos, int ticks, Map map = null)
		{
			eff.ticksLeft = ticks;
			TargetInfo targetInfo = new TargetInfo(pos, map ?? this.caster.Map, false);
			this.maintainedEffecters.Add(new Tuple<Effecter, TargetInfo, TargetInfo>(eff, targetInfo, targetInfo));
		}

		// Token: 0x060057B2 RID: 22450 RVA: 0x001C3C10 File Offset: 0x001C1E10
		public void AddEffecterToMaintain(Effecter eff, IntVec3 posA, IntVec3 posB, int ticks, Map map = null)
		{
			eff.ticksLeft = ticks;
			TargetInfo item = new TargetInfo(posA, map ?? this.caster.Map, false);
			TargetInfo item2 = new TargetInfo(posB, map ?? this.caster.Map, false);
			this.maintainedEffecters.Add(new Tuple<Effecter, TargetInfo, TargetInfo>(eff, item, item2));
		}

		// Token: 0x060057B3 RID: 22451 RVA: 0x001C3C6C File Offset: 0x001C1E6C
		public virtual bool Available()
		{
			if (this.verbProps.consumeFuelPerShot > 0f)
			{
				CompRefuelable compRefuelable = this.caster.TryGetComp<CompRefuelable>();
				if (compRefuelable != null && compRefuelable.Fuel < this.verbProps.consumeFuelPerShot)
				{
					return false;
				}
			}
			ThingWithComps equipmentSource = this.EquipmentSource;
			CompApparelVerbOwner compApparelVerbOwner = (equipmentSource != null) ? equipmentSource.GetComp<CompApparelVerbOwner>() : null;
			string text;
			return (compApparelVerbOwner == null || compApparelVerbOwner.CanBeUsed(out text)) && (!this.CasterIsPawn || this.EquipmentSource == null || !EquipmentUtility.RolePreventsFromUsing(this.CasterPawn, this.EquipmentSource, out text));
		}

		// Token: 0x060057B4 RID: 22452 RVA: 0x001C3CFC File Offset: 0x001C1EFC
		protected void TryCastNextBurstShot()
		{
			LocalTargetInfo localTargetInfo = this.currentTarget;
			if (this.Available() && this.TryCastShot())
			{
				if (this.verbProps.muzzleFlashScale > 0.01f)
				{
					FleckMaker.Static(this.caster.Position, this.caster.Map, FleckDefOf.ShotFlash, this.verbProps.muzzleFlashScale);
				}
				if (this.verbProps.soundCast != null)
				{
					this.verbProps.soundCast.PlayOneShot(new TargetInfo(this.caster.Position, this.caster.MapHeld, false));
				}
				if (this.verbProps.soundCastTail != null)
				{
					this.verbProps.soundCastTail.PlayOneShotOnCamera(this.caster.Map);
				}
				if (this.CasterIsPawn)
				{
					this.CasterPawn.Notify_UsedVerb(this.CasterPawn, this);
					if (this.CasterPawn.thinker != null && localTargetInfo == this.CasterPawn.mindState.enemyTarget)
					{
						this.CasterPawn.mindState.Notify_EngagedTarget();
					}
					if (this.CasterPawn.mindState != null)
					{
						this.CasterPawn.mindState.Notify_AttackedTarget(localTargetInfo);
					}
					if (this.CasterPawn.MentalState != null)
					{
						this.CasterPawn.MentalState.Notify_AttackedTarget(localTargetInfo);
					}
					if (this.TerrainDefSource != null)
					{
						this.CasterPawn.meleeVerbs.Notify_UsedTerrainBasedVerb();
					}
					if (this.CasterPawn.health != null)
					{
						this.CasterPawn.health.Notify_UsedVerb(this, localTargetInfo);
					}
					if (this.EquipmentSource != null)
					{
						this.EquipmentSource.Notify_UsedWeapon(this.CasterPawn);
					}
					if (!this.CasterPawn.Spawned)
					{
						this.Reset();
						return;
					}
				}
				if (this.verbProps.consumeFuelPerShot > 0f)
				{
					CompRefuelable compRefuelable = this.caster.TryGetComp<CompRefuelable>();
					if (compRefuelable != null)
					{
						compRefuelable.ConsumeFuel(this.verbProps.consumeFuelPerShot);
					}
				}
				this.burstShotsLeft--;
			}
			else
			{
				this.burstShotsLeft = 0;
			}
			if (this.burstShotsLeft > 0)
			{
				this.ticksToNextBurstShot = this.TicksBetweenBurstShots;
				if (this.CasterIsPawn && !this.NonInterruptingSelfCast)
				{
					this.CasterPawn.stances.SetStance(new Stance_Cooldown(this.TicksBetweenBurstShots + 1, this.currentTarget, this));
					return;
				}
			}
			else
			{
				this.state = VerbState.Idle;
				if (this.CasterIsPawn && !this.NonInterruptingSelfCast)
				{
					this.CasterPawn.stances.SetStance(new Stance_Cooldown(this.verbProps.AdjustedCooldownTicks(this, this.CasterPawn), this.currentTarget, this));
				}
				if (this.castCompleteCallback != null)
				{
					this.castCompleteCallback();
				}
				if (this.verbProps.consumeFuelPerBurst > 0f)
				{
					CompRefuelable compRefuelable2 = this.caster.TryGetComp<CompRefuelable>();
					if (compRefuelable2 != null)
					{
						compRefuelable2.ConsumeFuel(this.verbProps.consumeFuelPerBurst);
					}
				}
			}
		}

		// Token: 0x060057B5 RID: 22453 RVA: 0x001C3FE4 File Offset: 0x001C21E4
		public virtual void OrderForceTarget(LocalTargetInfo target)
		{
			if (this.verbProps.IsMeleeAttack)
			{
				Job job = JobMaker.MakeJob(JobDefOf.AttackMelee, target);
				job.playerForced = true;
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null)
				{
					job.killIncappedTarget = pawn.Downed;
				}
				this.CasterPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				return;
			}
			float num = this.verbProps.EffectiveMinRange(target, this.CasterPawn);
			if ((float)this.CasterPawn.Position.DistanceToSquared(target.Cell) < num * num && this.CasterPawn.Position.AdjacentTo8WayOrInside(target.Cell))
			{
				Messages.Message("MessageCantShootInMelee".Translate(), this.CasterPawn, MessageTypeDefOf.RejectInput, false);
				return;
			}
			Job job2 = JobMaker.MakeJob(this.verbProps.ai_IsWeapon ? JobDefOf.AttackStatic : JobDefOf.UseVerbOnThing);
			job2.verbToUse = this;
			job2.targetA = target;
			job2.endIfCantShootInMelee = true;
			this.CasterPawn.jobs.TryTakeOrderedJob(job2, new JobTag?(JobTag.Misc), false);
		}

		// Token: 0x060057B6 RID: 22454
		protected abstract bool TryCastShot();

		// Token: 0x060057B7 RID: 22455 RVA: 0x001C4103 File Offset: 0x001C2303
		public void Notify_PickedUp()
		{
			this.Reset();
		}

		// Token: 0x060057B8 RID: 22456 RVA: 0x001C410C File Offset: 0x001C230C
		public virtual void Reset()
		{
			this.state = VerbState.Idle;
			this.currentTarget = null;
			this.currentDestination = null;
			this.burstShotsLeft = 0;
			this.ticksToNextBurstShot = 0;
			this.castCompleteCallback = null;
			this.surpriseAttack = false;
			this.preventFriendlyFire = false;
		}

		// Token: 0x060057B9 RID: 22457 RVA: 0x001C415C File Offset: 0x001C235C
		public virtual void Notify_EquipmentLost()
		{
			if (this.CasterIsPawn)
			{
				Pawn casterPawn = this.CasterPawn;
				if (casterPawn.Spawned)
				{
					Stance_Warmup stance_Warmup = casterPawn.stances.curStance as Stance_Warmup;
					if (stance_Warmup != null && stance_Warmup.verb == this)
					{
						casterPawn.stances.CancelBusyStanceSoft();
					}
					if (casterPawn.CurJob != null && casterPawn.CurJob.def == JobDefOf.AttackStatic)
					{
						casterPawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
					}
				}
			}
		}

		// Token: 0x060057BA RID: 22458 RVA: 0x001C41D0 File Offset: 0x001C23D0
		public virtual float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 0f;
		}

		// Token: 0x060057BB RID: 22459 RVA: 0x001C41DC File Offset: 0x001C23DC
		private bool CausesTimeSlowdown(LocalTargetInfo castTarg)
		{
			if (!this.verbProps.CausesTimeSlowdown)
			{
				return false;
			}
			if (!castTarg.HasThing)
			{
				return false;
			}
			Thing thing = castTarg.Thing;
			if (thing.def.category != ThingCategory.Pawn && (thing.def.building == null || !thing.def.building.IsTurret))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			bool flag = pawn != null && pawn.Downed;
			return (this.CasterPawn == null || this.CasterPawn.Faction != Faction.OfPlayer || !this.CasterPawn.IsShambler) && (pawn == null || pawn.Faction != Faction.OfPlayer || !pawn.IsShambler) && ((thing.Faction == Faction.OfPlayer && this.caster.HostileTo(Faction.OfPlayer)) || (this.caster.Faction == Faction.OfPlayer && thing.HostileTo(Faction.OfPlayer) && !flag));
		}

		// Token: 0x060057BC RID: 22460 RVA: 0x001C42D5 File Offset: 0x001C24D5
		public virtual bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.caster != null && this.caster.Spawned && (targ == this.caster || this.CanHitTargetFrom(this.caster.Position, targ));
		}

		// Token: 0x060057BD RID: 22461 RVA: 0x001C4318 File Offset: 0x001C2518
		public virtual bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (this.CasterIsPawn)
			{
				Pawn pawn = target.Thing as Pawn;
				if (pawn != null && (pawn.InSameExtraFaction(this.caster as Pawn, ExtraFactionType.HomeFaction, null) || pawn.InSameExtraFaction(this.caster as Pawn, ExtraFactionType.MiniFaction, null)))
				{
					return false;
				}
			}
			if (this.CasterIsPawn)
			{
				Pawn pawn2 = target.Thing as Pawn;
				if (pawn2 != null && HistoryEventUtility.IsKillingInnocentAnimal(this.CasterPawn, pawn2) && !new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, this.CasterPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo"))
				{
					return false;
				}
			}
			if (this.CasterIsPawn)
			{
				Pawn pawn3 = target.Thing as Pawn;
				if (pawn3 != null && this.CasterPawn.Ideo != null && this.CasterPawn.Ideo.IsVeneratedAnimal(pawn3) && !new HistoryEvent(HistoryEventDefOf.HuntedVeneratedAnimal, this.CasterPawn.Named(HistoryEventArgsNames.Doer)).Notify_PawnAboutToDo("MessagePawnUnwillingToDoDueToIdeo"))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060057BE RID: 22462 RVA: 0x001C4418 File Offset: 0x001C2618
		public virtual void DrawHighlight(LocalTargetInfo target)
		{
			this.verbProps.DrawRadiusRing(this.caster.Position, this);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
				this.DrawHighlightFieldRadiusAroundTarget(target);
			}
		}

		// Token: 0x060057BF RID: 22463 RVA: 0x001C4448 File Offset: 0x001C2648
		protected void DrawHighlightFieldRadiusAroundTarget(LocalTargetInfo target)
		{
			bool flag;
			float num = this.HighlightFieldRadiusAroundTarget(out flag);
			ShootLine shootLine;
			if (num > 0.2f && this.TryFindShootLineFromTo(this.caster.Position, target, out shootLine, false))
			{
				if (flag)
				{
					GenExplosion.RenderPredictedAreaOfEffect(shootLine.Dest, num, this.verbProps.explosionRadiusRingColor);
					return;
				}
				GenDraw.DrawFieldEdges((from x in GenRadial.RadialCellsAround(shootLine.Dest, num, true)
				where x.InBounds(Find.CurrentMap)
				select x).ToList<IntVec3>(), this.verbProps.explosionRadiusRingColor, null, null, 2900);
			}
		}

		// Token: 0x060057C0 RID: 22464 RVA: 0x001C44F4 File Offset: 0x001C26F4
		public virtual void OnGUI(LocalTargetInfo target)
		{
			Texture2D icon;
			if (target.IsValid)
			{
				if (this.UIIcon != BaseContent.BadTex)
				{
					icon = this.UIIcon;
				}
				else
				{
					icon = TexCommand.Attack;
				}
			}
			else
			{
				icon = TexCommand.CannotShoot;
			}
			GenUI.DrawMouseAttachment(icon);
		}

		// Token: 0x060057C1 RID: 22465 RVA: 0x001C453C File Offset: 0x001C273C
		public virtual bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
		{
			if (targ.Thing != null && targ.Thing == this.caster)
			{
				return this.targetParams.canTargetSelf;
			}
			ShootLine shootLine;
			return (targ.Pawn == null || !targ.Pawn.IsPsychologicallyInvisible() || !this.caster.HostileTo(targ.Pawn)) && !this.ApparelPreventsShooting() && this.TryFindShootLineFromTo(root, targ, out shootLine, false);
		}

		// Token: 0x060057C2 RID: 22466 RVA: 0x001C45AF File Offset: 0x001C27AF
		public bool ApparelPreventsShooting()
		{
			return this.FirstApparelPreventingShooting() != null;
		}

		// Token: 0x060057C3 RID: 22467 RVA: 0x001C45BC File Offset: 0x001C27BC
		public Apparel FirstApparelPreventingShooting()
		{
			if (this.CasterIsPawn && this.CasterPawn.apparel != null)
			{
				List<Apparel> wornApparel = this.CasterPawn.apparel.WornApparel;
				for (int i = 0; i < wornApparel.Count; i++)
				{
					if (!wornApparel[i].AllowVerbCast(this))
					{
						return wornApparel[i];
					}
				}
			}
			return null;
		}

		// Token: 0x060057C4 RID: 22468 RVA: 0x001C4618 File Offset: 0x001C2818
		public bool TryFindShootLineFromTo(IntVec3 root, LocalTargetInfo targ, out ShootLine resultingLine, bool ignoreRange = false)
		{
			if (targ.HasThing && targ.Thing.Map != this.caster.Map)
			{
				resultingLine = default(ShootLine);
				return false;
			}
			if (this.verbProps.IsMeleeAttack || this.EffectiveRange <= 1.42f)
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return ReachabilityImmediate.CanReachImmediate(root, targ, this.caster.Map, PathEndMode.Touch, null);
			}
			CellRect occupiedRect = targ.HasThing ? targ.Thing.OccupiedRect() : CellRect.SingleCell(targ.Cell);
			if (!ignoreRange && this.OutOfRange(root, targ, occupiedRect))
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return false;
			}
			if (!this.verbProps.requireLineOfSight)
			{
				resultingLine = new ShootLine(root, targ.Cell);
				return true;
			}
			if (this.CasterIsPawn)
			{
				IntVec3 dest;
				if (this.CanHitFromCellIgnoringRange(root, targ, out dest))
				{
					resultingLine = new ShootLine(root, dest);
					return true;
				}
				ShootLeanUtility.LeanShootingSourcesFromTo(root, occupiedRect.ClosestCellTo(root), this.caster.Map, Verb.tempLeanShootSources);
				for (int i = 0; i < Verb.tempLeanShootSources.Count; i++)
				{
					IntVec3 intVec = Verb.tempLeanShootSources[i];
					if (this.CanHitFromCellIgnoringRange(intVec, targ, out dest))
					{
						resultingLine = new ShootLine(intVec, dest);
						return true;
					}
				}
			}
			else
			{
				foreach (IntVec3 intVec2 in this.caster.OccupiedRect())
				{
					IntVec3 dest;
					if (this.CanHitFromCellIgnoringRange(intVec2, targ, out dest))
					{
						resultingLine = new ShootLine(intVec2, dest);
						return true;
					}
				}
			}
			resultingLine = new ShootLine(root, targ.Cell);
			return false;
		}

		// Token: 0x060057C5 RID: 22469 RVA: 0x001C47FC File Offset: 0x001C29FC
		public bool OutOfRange(IntVec3 root, LocalTargetInfo targ, CellRect occupiedRect)
		{
			float num = this.verbProps.EffectiveMinRange(targ, this.caster);
			float num2 = occupiedRect.ClosestDistSquaredTo(root);
			return num2 > this.EffectiveRange * this.EffectiveRange || num2 < num * num;
		}

		// Token: 0x060057C6 RID: 22470 RVA: 0x001C4840 File Offset: 0x001C2A40
		private bool CanHitFromCellIgnoringRange(IntVec3 sourceCell, LocalTargetInfo targ, out IntVec3 goodDest)
		{
			if (targ.Thing != null)
			{
				if (targ.Thing.Map != this.caster.Map)
				{
					goodDest = IntVec3.Invalid;
					return false;
				}
				ShootLeanUtility.CalcShootableCellsOf(Verb.tempDestList, targ.Thing, sourceCell);
				for (int i = 0; i < Verb.tempDestList.Count; i++)
				{
					if (this.CanHitCellFromCellIgnoringRange(sourceCell, Verb.tempDestList[i], targ.Thing.def.Fillage == FillCategory.Full))
					{
						goodDest = Verb.tempDestList[i];
						return true;
					}
				}
			}
			else if (this.CanHitCellFromCellIgnoringRange(sourceCell, targ.Cell, false))
			{
				goodDest = targ.Cell;
				return true;
			}
			goodDest = IntVec3.Invalid;
			return false;
		}

		// Token: 0x060057C7 RID: 22471 RVA: 0x001C4910 File Offset: 0x001C2B10
		private bool CanHitCellFromCellIgnoringRange(IntVec3 sourceSq, IntVec3 targetLoc, bool includeCorners = false)
		{
			if (this.verbProps.mustCastOnOpenGround && (!targetLoc.Standable(this.caster.Map) || this.caster.Map.thingGrid.CellContains(targetLoc, ThingCategory.Pawn)))
			{
				return false;
			}
			if (this.verbProps.requireLineOfSight)
			{
				if (!includeCorners)
				{
					if (!GenSight.LineOfSight(sourceSq, targetLoc, this.caster.Map, true, null, 0, 0))
					{
						return false;
					}
				}
				else if (!GenSight.LineOfSightToEdges(sourceSq, targetLoc, this.caster.Map, true, null))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060057C8 RID: 22472 RVA: 0x001C499C File Offset: 0x001C2B9C
		public override string ToString()
		{
			string text;
			if (this.verbProps == null)
			{
				text = "null";
			}
			else if (!this.verbProps.label.NullOrEmpty())
			{
				text = this.verbProps.label;
			}
			else if (this.HediffCompSource != null)
			{
				text = this.HediffCompSource.Def.label;
			}
			else if (this.EquipmentSource != null)
			{
				text = this.EquipmentSource.def.label;
			}
			else if (this.verbProps.AdjustedLinkedBodyPartsGroup(this.tool) != null)
			{
				text = this.verbProps.AdjustedLinkedBodyPartsGroup(this.tool).defName;
			}
			else
			{
				text = "unknown";
			}
			if (this.tool != null)
			{
				text = text + "/" + this.loadID;
			}
			return string.Format("{0}({1})", base.GetType(), text);
		}

		// Token: 0x04003FCB RID: 16331
		public VerbProperties verbProps;

		// Token: 0x04003FCC RID: 16332
		public VerbTracker verbTracker;

		// Token: 0x04003FCD RID: 16333
		public ManeuverDef maneuver;

		// Token: 0x04003FCE RID: 16334
		public Tool tool;

		// Token: 0x04003FCF RID: 16335
		public Thing caster;

		// Token: 0x04003FD0 RID: 16336
		public MechanitorControlGroup controlGroup;

		// Token: 0x04003FD1 RID: 16337
		public string loadID;

		// Token: 0x04003FD2 RID: 16338
		public VerbState state;

		// Token: 0x04003FD3 RID: 16339
		protected LocalTargetInfo currentTarget;

		// Token: 0x04003FD4 RID: 16340
		protected LocalTargetInfo currentDestination;

		// Token: 0x04003FD5 RID: 16341
		protected int burstShotsLeft;

		// Token: 0x04003FD6 RID: 16342
		protected int ticksToNextBurstShot;

		// Token: 0x04003FD7 RID: 16343
		protected int lastShotTick = -999999;

		// Token: 0x04003FD8 RID: 16344
		protected bool surpriseAttack;

		// Token: 0x04003FD9 RID: 16345
		protected bool canHitNonTargetPawnsNow = true;

		// Token: 0x04003FDA RID: 16346
		public bool preventFriendlyFire;

		// Token: 0x04003FDB RID: 16347
		protected bool nonInterruptingSelfCast;

		// Token: 0x04003FDC RID: 16348
		public Action castCompleteCallback;

		// Token: 0x04003FDD RID: 16349
		private Texture2D commandIconCached;

		// Token: 0x04003FDE RID: 16350
		private readonly List<Tuple<Effecter, TargetInfo, TargetInfo>> maintainedEffecters = new List<Tuple<Effecter, TargetInfo, TargetInfo>>();

		// Token: 0x04003FDF RID: 16351
		private int? cachedTicksBetweenBurstShots;

		// Token: 0x04003FE0 RID: 16352
		private int? cachedBurstShotCount;

		// Token: 0x04003FE1 RID: 16353
		private static readonly List<IntVec3> tempLeanShootSources = new List<IntVec3>();

		// Token: 0x04003FE2 RID: 16354
		private static readonly List<IntVec3> tempDestList = new List<IntVec3>();
	}
}
