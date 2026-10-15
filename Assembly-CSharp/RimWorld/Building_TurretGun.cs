using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Building_TurretGun : Building_Turret
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__71 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_TurretGun _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetGizmos_003Ed__71(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_TurretGun CS_0024_003C_003E8__locals24 = _003C_003E4__this;
				CompChangeableProjectile compChangeableProjectile2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals24).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0084;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0084;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0165;
				case 3:
					_003C_003E1__state = -4;
					goto IL_01c3;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0347;
				case 5:
					_003C_003E1__state = -1;
					goto IL_03fb;
				case 6:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0084:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals24.CanExtractShell)
					{
						CompChangeableProjectile compChangeableProjectile = CS_0024_003C_003E8__locals24.gun.TryGetComp<CompChangeableProjectile>();
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CommandExtractShell".Translate(),
							defaultDesc = "CommandExtractShellDesc".Translate(),
							icon = compChangeableProjectile.LoadedShell.uiIcon,
							iconAngle = compChangeableProjectile.LoadedShell.uiIconAngle,
							iconOffset = compChangeableProjectile.LoadedShell.uiIconOffset,
							iconDrawScale = GenUI.IconDrawScale(compChangeableProjectile.LoadedShell),
							action = delegate
							{
								CS_0024_003C_003E8__locals24.ExtractShell();
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0165;
					IL_0347:
					if (CS_0024_003C_003E8__locals24.forcedTarget.IsValid)
					{
						Command_Action command_Action = new Command_Action
						{
							defaultLabel = "CommandStopForceAttack".Translate(),
							defaultDesc = "CommandStopForceAttackDesc".Translate(),
							icon = ContentFinder<Texture2D>.Get("UI/Commands/Halt"),
							action = delegate
							{
								CS_0024_003C_003E8__locals24.ResetForcedTarget();
								SoundDefOf.Tick_Low.PlayOneShotOnCamera();
							}
						};
						if (!CS_0024_003C_003E8__locals24.forcedTarget.IsValid)
						{
							command_Action.Disable("CommandStopAttackFailNotForceAttacking".Translate());
						}
						command_Action.hotKey = KeyBindingDefOf.Misc5;
						_003C_003E2__current = command_Action;
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_03fb;
					IL_03fb:
					if (!CS_0024_003C_003E8__locals24.CanToggleHoldFire)
					{
						break;
					}
					_003C_003E2__current = new Command_Toggle
					{
						defaultLabel = "CommandHoldFire".Translate(),
						defaultDesc = "CommandHoldFireDesc".Translate(),
						icon = ContentFinder<Texture2D>.Get("UI/Commands/HoldFire"),
						hotKey = KeyBindingDefOf.Misc6,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals24.holdFire = !CS_0024_003C_003E8__locals24.holdFire;
							if (CS_0024_003C_003E8__locals24.holdFire)
							{
								CS_0024_003C_003E8__locals24.ResetForcedTarget();
							}
						},
						isActive = () => CS_0024_003C_003E8__locals24.holdFire
					};
					_003C_003E1__state = 6;
					return true;
					IL_0165:
					compChangeableProjectile2 = CS_0024_003C_003E8__locals24.gun.TryGetComp<CompChangeableProjectile>();
					if (compChangeableProjectile2 != null)
					{
						StorageSettings storeSettings = compChangeableProjectile2.GetStoreSettings();
						_003C_003E7__wrap1 = StorageSettingsClipboard.CopyPasteGizmosFor(storeSettings).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_01c3;
					}
					goto IL_01dd;
					IL_01dd:
					if (!CS_0024_003C_003E8__locals24.HideForceTargetGizmo)
					{
						if (CS_0024_003C_003E8__locals24.CanSetForcedTarget)
						{
							Command_VerbTarget command_VerbTarget = new Command_VerbTarget
							{
								defaultLabel = "CommandSetForceAttackTarget".Translate(),
								defaultDesc = "CommandSetForceAttackTargetDesc".Translate(),
								icon = ContentFinder<Texture2D>.Get("UI/Commands/Attack"),
								verb = CS_0024_003C_003E8__locals24.AttackVerb,
								hotKey = KeyBindingDefOf.Misc4,
								drawRadius = false,
								requiresAvailableVerb = false
							};
							if (CS_0024_003C_003E8__locals24.Spawned)
							{
								if (CS_0024_003C_003E8__locals24.IsMortarOrProjectileFliesOverhead && CS_0024_003C_003E8__locals24.Position.Roofed(CS_0024_003C_003E8__locals24.Map))
								{
									command_VerbTarget.Disable("CannotFire".Translate() + ": " + "Roofed".Translate().CapitalizeFirst());
								}
								else
								{
									float curWeatherMaxRangeCap = CS_0024_003C_003E8__locals24.Map.weatherManager.CurWeatherMaxRangeCap;
									if (curWeatherMaxRangeCap > 0f && curWeatherMaxRangeCap < CS_0024_003C_003E8__locals24.AttackVerb.verbProps.minRange)
									{
										command_VerbTarget.Disable("CannotFire".Translate() + ": " + CS_0024_003C_003E8__locals24.Map.weatherManager.curWeather.LabelCap);
									}
								}
							}
							_003C_003E2__current = command_VerbTarget;
							_003C_003E1__state = 4;
							return true;
						}
						goto IL_0347;
					}
					goto IL_03fb;
					IL_01c3:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					goto IL_01dd;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__71 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__71(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	protected int burstCooldownTicksLeft;

	protected int burstWarmupTicksLeft;

	protected LocalTargetInfo currentTargetInt = LocalTargetInfo.Invalid;

	private bool holdFire;

	private bool burstActivated;

	public Thing gun;

	protected TurretTop top;

	protected CompPowerTrader powerComp;

	protected CompCanBeDormant dormantComp;

	protected CompInitiatable initiatableComp;

	protected CompMannable mannableComp;

	protected CompInteractable interactableComp;

	public CompRefuelable refuelableComp;

	protected Effecter progressBarEffecter;

	protected CompMechPowerCell powerCellComp;

	protected CompHackable hackableComp;

	private const int TryStartShootSomethingIntervalTicks = 15;

	public static Material ForcedTargetLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));

	public bool Active
	{
		get
		{
			if ((powerComp == null || powerComp.PowerOn) && (dormantComp == null || dormantComp.Awake) && (initiatableComp == null || initiatableComp.Initiated) && (interactableComp == null || burstActivated) && (powerCellComp == null || !powerCellComp.depleted))
			{
				if (hackableComp != null)
				{
					return !hackableComp.IsHacked;
				}
				return true;
			}
			return false;
		}
	}

	public CompEquippable GunCompEq => gun.TryGetComp<CompEquippable>();

	public override LocalTargetInfo CurrentTarget => currentTargetInt;

	private bool WarmingUp => burstWarmupTicksLeft > 0;

	public override Verb AttackVerb => GunCompEq.PrimaryVerb;

	public bool IsMannable => mannableComp != null;

	private bool PlayerControlled
	{
		get
		{
			if ((base.Faction == Faction.OfPlayer || MannedByColonist) && !MannedByNonColonist)
			{
				return !IsActivable;
			}
			return false;
		}
	}

	protected virtual bool CanSetForcedTarget
	{
		get
		{
			if (mannableComp != null)
			{
				return PlayerControlled;
			}
			return false;
		}
	}

	private bool CanToggleHoldFire => PlayerControlled;

	private bool IsMortar => def.building.IsMortar;

	private bool IsMortarOrProjectileFliesOverhead
	{
		get
		{
			if (!AttackVerb.ProjectileFliesOverhead())
			{
				return IsMortar;
			}
			return true;
		}
	}

	private bool IsActivable => interactableComp != null;

	protected virtual bool HideForceTargetGizmo => false;

	public TurretTop Top => top;

	private bool CanExtractShell
	{
		get
		{
			if (!PlayerControlled)
			{
				return false;
			}
			return gun.TryGetComp<CompChangeableProjectile>()?.Loaded ?? false;
		}
	}

	private bool MannedByColonist
	{
		get
		{
			if (mannableComp != null && mannableComp.ManningPawn != null)
			{
				return mannableComp.ManningPawn.Faction == Faction.OfPlayer;
			}
			return false;
		}
	}

	private bool MannedByNonColonist
	{
		get
		{
			if (mannableComp != null && mannableComp.ManningPawn != null)
			{
				return mannableComp.ManningPawn.Faction != Faction.OfPlayer;
			}
			return false;
		}
	}

	public Building_TurretGun()
	{
		top = new TurretTop(this);
	}

	public override void PostMake()
	{
		base.PostMake();
		burstCooldownTicksLeft = def.building.turretInitialCooldownTime.SecondsToTicks();
		MakeGun();
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		dormantComp = GetComp<CompCanBeDormant>();
		initiatableComp = GetComp<CompInitiatable>();
		powerComp = GetComp<CompPowerTrader>();
		mannableComp = GetComp<CompMannable>();
		interactableComp = GetComp<CompInteractable>();
		refuelableComp = GetComp<CompRefuelable>();
		powerCellComp = GetComp<CompMechPowerCell>();
		hackableComp = GetComp<CompHackable>();
		if (!respawningAfterLoad)
		{
			top.SetRotationFromOrientation();
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		base.DeSpawn(mode);
		ResetCurrentTarget();
		progressBarEffecter?.Cleanup();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref burstCooldownTicksLeft, "burstCooldownTicksLeft", 0);
		Scribe_Values.Look(ref burstWarmupTicksLeft, "burstWarmupTicksLeft", 0);
		Scribe_TargetInfo.Look(ref currentTargetInt, "currentTarget");
		Scribe_Values.Look(ref holdFire, "holdFire", defaultValue: false);
		Scribe_Values.Look(ref burstActivated, "burstActivated", defaultValue: false);
		Scribe_Deep.Look(ref gun, "gun");
		BackCompatibility.PostExposeData(this);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			if (gun == null)
			{
				Log.Error("Turret had null gun after loading. Recreating.");
				MakeGun();
			}
			else
			{
				UpdateGunVerbs();
			}
		}
	}

	public override AcceptanceReport ClaimableBy(Faction by)
	{
		AcceptanceReport result = base.ClaimableBy(by);
		if (!result.Accepted)
		{
			return result;
		}
		if (mannableComp != null && mannableComp.ManningPawn != null)
		{
			return false;
		}
		if (Active && mannableComp == null)
		{
			return false;
		}
		if (((dormantComp != null && !dormantComp.Awake) || (initiatableComp != null && !initiatableComp.Initiated)) && (powerComp == null || powerComp.PowerOn))
		{
			return false;
		}
		return true;
	}

	public override void OrderAttack(LocalTargetInfo targ)
	{
		if (!targ.IsValid)
		{
			if (forcedTarget.IsValid)
			{
				ResetForcedTarget();
			}
			return;
		}
		if ((targ.Cell - base.Position).LengthHorizontal < AttackVerb.verbProps.EffectiveMinRange(targ, this))
		{
			Messages.Message("MessageTargetBelowMinimumRange".Translate(), this, MessageTypeDefOf.RejectInput, historical: false);
			return;
		}
		if ((targ.Cell - base.Position).LengthHorizontal > AttackVerb.EffectiveRange)
		{
			Messages.Message("MessageTargetBeyondMaximumRange".Translate(), this, MessageTypeDefOf.RejectInput, historical: false);
			return;
		}
		if (forcedTarget != targ)
		{
			forcedTarget = targ;
			if (burstCooldownTicksLeft <= 0)
			{
				TryStartShootSomething(canBeginBurstImmediately: false);
			}
		}
		if (holdFire)
		{
			Messages.Message("MessageTurretWontFireBecauseHoldFire".Translate(def.label), this, MessageTypeDefOf.RejectInput, historical: false);
		}
	}

	protected override void Tick()
	{
		base.Tick();
		if (CanExtractShell && MannedByColonist)
		{
			CompChangeableProjectile compChangeableProjectile = gun.TryGetComp<CompChangeableProjectile>();
			if (!compChangeableProjectile.allowedShellsSettings.AllowedToAccept(compChangeableProjectile.LoadedShell))
			{
				ExtractShell();
			}
		}
		if (forcedTarget.IsValid && !CanSetForcedTarget)
		{
			ResetForcedTarget();
		}
		if (!CanToggleHoldFire)
		{
			holdFire = false;
		}
		if (forcedTarget.ThingDestroyed)
		{
			ResetForcedTarget();
		}
		if (Active && (mannableComp == null || mannableComp.MannedNow) && !base.IsStunned && base.Spawned)
		{
			GunCompEq.verbTracker.VerbsTick();
			if (AttackVerb.state == VerbState.Bursting)
			{
				return;
			}
			burstActivated = false;
			if (WarmingUp)
			{
				burstWarmupTicksLeft--;
				if (burstWarmupTicksLeft <= 0)
				{
					BeginBurst();
				}
			}
			else
			{
				if (burstCooldownTicksLeft > 0)
				{
					burstCooldownTicksLeft--;
					if (IsMortar)
					{
						if (progressBarEffecter == null)
						{
							progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
						}
						progressBarEffecter.EffectTick(this, TargetInfo.Invalid);
						MoteProgressBar mote = ((SubEffecter_ProgressBar)progressBarEffecter.children[0]).mote;
						mote.progress = 1f - (float)Math.Max(burstCooldownTicksLeft, 0) / (float)BurstCooldownTime().SecondsToTicks();
						mote.offsetZ = -0.8f;
					}
				}
				if (burstCooldownTicksLeft <= 0 && this.IsHashIntervalTick(15))
				{
					TryStartShootSomething(canBeginBurstImmediately: true);
				}
			}
			top.TurretTopTick();
		}
		else
		{
			ResetCurrentTarget();
		}
	}

	public void TryActivateBurst()
	{
		burstActivated = true;
		TryStartShootSomething(canBeginBurstImmediately: true);
	}

	public void TryStartShootSomething(bool canBeginBurstImmediately)
	{
		if (progressBarEffecter != null)
		{
			progressBarEffecter.Cleanup();
			progressBarEffecter = null;
		}
		if (!base.Spawned || (holdFire && CanToggleHoldFire) || (AttackVerb.ProjectileFliesOverhead() && base.Map.roofGrid.Roofed(base.Position)) || !AttackVerb.Available())
		{
			ResetCurrentTarget();
			return;
		}
		bool isValid = currentTargetInt.IsValid;
		if (forcedTarget.IsValid)
		{
			currentTargetInt = forcedTarget;
		}
		else
		{
			currentTargetInt = TryFindNewTarget();
		}
		if (!isValid && currentTargetInt.IsValid && def.building.playTargetAcquiredSound)
		{
			SoundDefOf.TurretAcquireTarget.PlayOneShot(new TargetInfo(base.Position, base.Map));
		}
		if (currentTargetInt.IsValid)
		{
			float randomInRange = def.building.turretBurstWarmupTime.RandomInRange;
			if (randomInRange > 0f)
			{
				burstWarmupTicksLeft = randomInRange.SecondsToTicks();
			}
			else if (canBeginBurstImmediately)
			{
				BeginBurst();
			}
			else
			{
				burstWarmupTicksLeft = 1;
			}
		}
		else
		{
			ResetCurrentTarget();
		}
	}

	public virtual LocalTargetInfo TryFindNewTarget()
	{
		IAttackTargetSearcher attackTargetSearcher = TargSearcher();
		Faction faction = attackTargetSearcher.Thing.Faction;
		float range = AttackVerb.EffectiveRange;
		if (Rand.Value < 0.5f && AttackVerb.ProjectileFliesOverhead() && faction.HostileTo(Faction.OfPlayer) && base.Map.listerBuildings.allBuildingsColonist.Where(delegate(Building x)
		{
			float num = AttackVerb.verbProps.EffectiveMinRange(x, this);
			float num2 = x.Position.DistanceToSquared(base.Position);
			return num2 > num * num && num2 < range * range;
		}).TryRandomElement(out var result))
		{
			return result;
		}
		TargetScanFlags targetScanFlags = TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
		if (!AttackVerb.ProjectileFliesOverhead())
		{
			targetScanFlags |= TargetScanFlags.NeedLOSToAll;
			targetScanFlags |= TargetScanFlags.LOSBlockableByGas;
		}
		if (AttackVerb.IsIncendiary_Ranged())
		{
			targetScanFlags |= TargetScanFlags.NeedNonBurning;
		}
		if (IsMortar)
		{
			targetScanFlags |= TargetScanFlags.NeedNotUnderThickRoof;
		}
		return (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(attackTargetSearcher, targetScanFlags, IsValidTarget);
	}

	private IAttackTargetSearcher TargSearcher()
	{
		if (mannableComp != null && mannableComp.MannedNow)
		{
			return mannableComp.ManningPawn;
		}
		return this;
	}

	private bool IsValidTarget(Thing t)
	{
		if (t is Pawn pawn)
		{
			if (base.Faction == Faction.OfPlayer && pawn.IsPrisoner)
			{
				return false;
			}
			if (AttackVerb.ProjectileFliesOverhead())
			{
				RoofDef roofDef = base.Map.roofGrid.RoofAt(t.Position);
				if (roofDef != null && roofDef.isThickRoof)
				{
					return false;
				}
			}
			if (mannableComp == null)
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

	protected virtual void BeginBurst()
	{
		AttackVerb.TryStartCastOn(CurrentTarget);
		OnAttackedTarget(CurrentTarget);
	}

	protected virtual void BurstComplete()
	{
		burstCooldownTicksLeft = BurstCooldownTime().SecondsToTicks();
	}

	protected virtual float BurstCooldownTime()
	{
		if (def.building.turretBurstCooldownTime >= 0f)
		{
			return def.building.turretBurstCooldownTime;
		}
		return AttackVerb.verbProps.defaultCooldownTime;
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		string inspectString = base.GetInspectString();
		if (!inspectString.NullOrEmpty())
		{
			stringBuilder.AppendLine(inspectString);
		}
		if (AttackVerb.verbProps.minRange > 0f)
		{
			stringBuilder.AppendLine("MinimumRange".Translate() + ": " + AttackVerb.verbProps.minRange.ToString("F0"));
		}
		if (base.Spawned && IsMortarOrProjectileFliesOverhead && base.Position.Roofed(base.Map))
		{
			stringBuilder.AppendLine("CannotFire".Translate() + ": " + "Roofed".Translate().CapitalizeFirst());
		}
		else if (base.Spawned && burstCooldownTicksLeft > 0 && BurstCooldownTime() > 5f)
		{
			stringBuilder.AppendLine("CanFireIn".Translate() + ": " + burstCooldownTicksLeft.ToStringSecondsFromTicks());
		}
		CompChangeableProjectile compChangeableProjectile = gun.TryGetComp<CompChangeableProjectile>();
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

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		Vector3 drawOffset = Vector3.zero;
		float angleOffset = 0f;
		if (IsMortar)
		{
			EquipmentUtility.Recoil(def.building.turretGunDef, (Verb_LaunchProjectile)AttackVerb, out drawOffset, out angleOffset, top.CurRotation);
		}
		top.DrawTurret(drawLoc, drawOffset, angleOffset);
		base.DrawAt(drawLoc, flip);
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		float effectiveRange = AttackVerb.EffectiveRange;
		float num = AttackVerb.verbProps.EffectiveMinRange(allowAdjacentShot: true);
		if (num < effectiveRange)
		{
			if (effectiveRange < 90f)
			{
				GenDraw.DrawRadiusRing(base.Position, effectiveRange);
			}
			if (num < 90f && num > 0.1f)
			{
				GenDraw.DrawRadiusRing(base.Position, num);
			}
		}
		if (WarmingUp)
		{
			int degreesWide = (int)((float)burstWarmupTicksLeft * 0.5f);
			GenDraw.DrawAimPie(this, CurrentTarget, degreesWide, (float)def.size.x * 0.5f);
		}
		if (forcedTarget.IsValid && (!forcedTarget.HasThing || forcedTarget.Thing.Spawned))
		{
			Vector3 b = ((!forcedTarget.HasThing) ? forcedTarget.Cell.ToVector3Shifted() : forcedTarget.Thing.TrueCenter());
			Vector3 a = this.TrueCenter();
			b.y = AltitudeLayer.MetaOverlays.AltitudeFor();
			a.y = b.y;
			GenDraw.DrawLineBetween(a, b, ForcedTargetLineMat);
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__71))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__71(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void ExtractShell()
	{
		GenPlace.TryPlaceThing(gun.TryGetComp<CompChangeableProjectile>().RemoveShell(), base.Position, base.Map, ThingPlaceMode.Near, null, null, null);
	}

	private void ResetForcedTarget()
	{
		forcedTarget = LocalTargetInfo.Invalid;
		burstWarmupTicksLeft = 0;
		if (burstCooldownTicksLeft <= 0)
		{
			TryStartShootSomething(canBeginBurstImmediately: false);
		}
	}

	private void ResetCurrentTarget()
	{
		currentTargetInt = LocalTargetInfo.Invalid;
		burstWarmupTicksLeft = 0;
	}

	public void MakeGun()
	{
		gun = ThingMaker.MakeThing(def.building.turretGunDef);
		UpdateGunVerbs();
	}

	private void UpdateGunVerbs()
	{
		List<Verb> allVerbs = gun.TryGetComp<CompEquippable>().AllVerbs;
		for (int i = 0; i < allVerbs.Count; i++)
		{
			Verb verb = allVerbs[i];
			verb.caster = this;
			verb.castCompleteCallback = BurstComplete;
		}
	}
}
