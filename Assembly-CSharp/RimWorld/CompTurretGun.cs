using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class CompTurretGun : ThingComp, IAttackTargetSearcher
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__37 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompTurretGun _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__37(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompTurretGun CS_0024_003C_003E8__locals5 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)CS_0024_003C_003E8__locals5).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0074;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0074;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0074:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals5.parent is Pawn { IsColonyMechPlayerControlled: not false })
					{
						_003C_003E2__current = new Command_Toggle
						{
							defaultLabel = "CommandToggleTurret".Translate(),
							defaultDesc = "CommandToggleTurretDesc".Translate(),
							isActive = () => CS_0024_003C_003E8__locals5.fireAtWill,
							icon = ToggleTurretIcon.Texture,
							toggleAction = delegate
							{
								CS_0024_003C_003E8__locals5.fireAtWill = !CS_0024_003C_003E8__locals5.fireAtWill;
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					break;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__37 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__37(0)
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

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__39 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompTurretGun _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__39(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompTurretGun compTurretGun = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (compTurretGun.Props.turretDef != null)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.PawnCombat, "Turret".Translate(), compTurretGun.Props.turretDef.LabelCap, "Stat_Thing_TurretDesc".Translate(), 5600, null, Gen.YieldSingle(new Dialog_InfoCard.Hyperlink(compTurretGun.Props.turretDef)));
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__39 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__39(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private const int StartShootIntervalTicks = 10;

	private static readonly CachedTexture ToggleTurretIcon = new CachedTexture("UI/Gizmos/ToggleTurret");

	public Thing gun;

	protected int burstCooldownTicksLeft;

	protected int burstWarmupTicksLeft;

	protected LocalTargetInfo currentTarget = LocalTargetInfo.Invalid;

	private bool fireAtWill = true;

	private LocalTargetInfo lastAttackedTarget = LocalTargetInfo.Invalid;

	private int lastAttackTargetTick;

	public float curRotation;

	public Thing Thing => parent;

	public CompProperties_TurretGun Props => (CompProperties_TurretGun)props;

	public Verb CurrentEffectiveVerb => AttackVerb;

	public LocalTargetInfo LastAttackedTarget => lastAttackedTarget;

	public int LastAttackTargetTick => lastAttackTargetTick;

	public CompEquippable GunCompEq => gun.TryGetComp<CompEquippable>();

	public Verb AttackVerb => GunCompEq.PrimaryVerb;

	private bool WarmingUp => burstWarmupTicksLeft > 0;

	private bool CanShoot
	{
		get
		{
			if (parent is Pawn pawn)
			{
				if (!pawn.Spawned || pawn.Downed || pawn.Dead || !pawn.Awake())
				{
					return false;
				}
				if (pawn.stances.stunner.Stunned)
				{
					return false;
				}
				if (TurretDestroyed)
				{
					return false;
				}
				if (pawn.IsColonyMechPlayerControlled && !fireAtWill)
				{
					return false;
				}
			}
			CompCanBeDormant compCanBeDormant = parent.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null && !compCanBeDormant.Awake)
			{
				return false;
			}
			return true;
		}
	}

	public bool TurretDestroyed
	{
		get
		{
			if (parent is Pawn pawn && AttackVerb.verbProps.linkedBodyPartsGroup != null && AttackVerb.verbProps.ensureLinkedBodyPartsGroupAlwaysUsable && PawnCapacityUtility.CalculateNaturalPartsAverageEfficiency(pawn.health.hediffSet, AttackVerb.verbProps.linkedBodyPartsGroup) <= 0f)
			{
				return true;
			}
			return false;
		}
	}

	public bool AutoAttack => Props.autoAttack;

	public override void PostPostMake()
	{
		base.PostPostMake();
		MakeGun();
	}

	private void MakeGun()
	{
		gun = ThingMaker.MakeThing(Props.turretDef);
		UpdateGunVerbs();
	}

	private void UpdateGunVerbs()
	{
		List<Verb> allVerbs = gun.TryGetComp<CompEquippable>().AllVerbs;
		for (int i = 0; i < allVerbs.Count; i++)
		{
			Verb verb = allVerbs[i];
			verb.caster = parent;
			verb.castCompleteCallback = delegate
			{
				burstCooldownTicksLeft = AttackVerb.verbProps.defaultCooldownTime.SecondsToTicks();
			};
		}
	}

	public override void CompTick()
	{
		if (!CanShoot)
		{
			return;
		}
		if (currentTarget.IsValid)
		{
			curRotation = (currentTarget.Cell.ToVector3Shifted() - parent.DrawPos).AngleFlat() + Props.angleOffset;
		}
		AttackVerb.VerbTick();
		if (AttackVerb.state == VerbState.Bursting)
		{
			return;
		}
		if (WarmingUp)
		{
			burstWarmupTicksLeft--;
			if (burstWarmupTicksLeft == 0)
			{
				AttackVerb.TryStartCastOn(currentTarget, surpriseAttack: false, canHitNonTargetPawns: true, preventFriendlyFire: false, nonInterruptingSelfCast: true);
				lastAttackTargetTick = Find.TickManager.TicksGame;
				lastAttackedTarget = currentTarget;
			}
			return;
		}
		if (burstCooldownTicksLeft > 0)
		{
			burstCooldownTicksLeft--;
		}
		if (burstCooldownTicksLeft <= 0 && parent.IsHashIntervalTick(10))
		{
			currentTarget = (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(this, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable);
			if (currentTarget.IsValid)
			{
				burstWarmupTicksLeft = 1;
			}
			else
			{
				ResetCurrentTarget();
			}
		}
	}

	private void ResetCurrentTarget()
	{
		currentTarget = LocalTargetInfo.Invalid;
		burstWarmupTicksLeft = 0;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__37))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__37(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override List<PawnRenderNode> CompRenderNodes()
	{
		if (!Props.renderNodeProperties.NullOrEmpty() && parent is Pawn pawn)
		{
			List<PawnRenderNode> list = new List<PawnRenderNode>();
			{
				foreach (PawnRenderNodeProperties renderNodeProperty in Props.renderNodeProperties)
				{
					PawnRenderNode_TurretGun pawnRenderNode_TurretGun = (PawnRenderNode_TurretGun)Activator.CreateInstance(renderNodeProperty.nodeClass, pawn, renderNodeProperty, pawn.Drawer.renderer.renderTree);
					pawnRenderNode_TurretGun.turretComp = this;
					list.Add(pawnRenderNode_TurretGun);
				}
				return list;
			}
		}
		return base.CompRenderNodes();
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__39))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__39(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref burstCooldownTicksLeft, "burstCooldownTicksLeft", 0);
		Scribe_Values.Look(ref burstWarmupTicksLeft, "burstWarmupTicksLeft", 0);
		Scribe_TargetInfo.Look(ref currentTarget, "currentTarget");
		Scribe_Deep.Look(ref gun, "gun");
		Scribe_Values.Look(ref fireAtWill, "fireAtWill", defaultValue: true);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			if (gun == null)
			{
				Log.Error("CompTurrentGun had null gun after loading. Recreating.");
				MakeGun();
			}
			else
			{
				UpdateGunVerbs();
			}
		}
	}
}
