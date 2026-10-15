using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class CompShield : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__30 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompShield _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__30(int _003C_003E1__state)
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
			case 2:
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
				CompShield compShield = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)compShield).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_00d7;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!compShield.IsBuiltIn)
					{
						break;
					}
					_003C_003E7__wrap1 = compShield.GetGizmos().GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00d7;
					IL_00d7:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
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
			_003CCompGetGizmosExtra_003Ed__30 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__30(0)
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
	private sealed class _003CCompGetWornGizmosExtra_003Ed__29 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompShield _003C_003E4__this;

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
		public _003CCompGetWornGizmosExtra_003Ed__29(int _003C_003E1__state)
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
			case 2:
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
				CompShield CS_0024_003C_003E8__locals6 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)CS_0024_003C_003E8__locals6).CompGetWornGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00e2;
				case 3:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals6.ticksToReset > 0)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Clear reset",
							action = delegate
							{
								CS_0024_003C_003E8__locals6.ticksToReset = 0;
							}
						};
						_003C_003E1__state = 4;
						return true;
					}
					break;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00e2:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					goto IL_00fc;
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals6.IsApparel)
					{
						_003C_003E7__wrap1 = CS_0024_003C_003E8__locals6.GetGizmos().GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00e2;
					}
					goto IL_00fc;
					IL_00fc:
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Break",
							action = CS_0024_003C_003E8__locals6.Break
						};
						_003C_003E1__state = 3;
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
			_003CCompGetWornGizmosExtra_003Ed__29 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetWornGizmosExtra_003Ed__29(0)
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
	private sealed class _003CGetGizmos_003Ed__31 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompShield _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__31(int _003C_003E1__state)
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
			CompShield compShield = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if ((compShield.PawnOwner.Faction == Faction.OfPlayer || (compShield.parent is Pawn pawn && pawn.RaceProps.IsMechanoid)) && Find.Selector.SingleSelectedThing == compShield.PawnOwner)
				{
					_003C_003E2__current = new Gizmo_EnergyShieldStatus
					{
						shield = compShield
					};
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__31 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__31(0)
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

	protected float energy;

	protected int ticksToReset = -1;

	protected int lastKeepDisplayTick = -9999;

	private Vector3 impactAngleVect;

	private int lastAbsorbDamageTick = -9999;

	private const float MaxDamagedJitterDist = 0.05f;

	private const int JitterDurationTicks = 8;

	private int KeepDisplayingTicks = 1000;

	private float ApparelScorePerEnergyMax = 0.25f;

	private static readonly Material BubbleMat = MaterialPool.MatFrom("Other/ShieldBubble", ShaderDatabase.Transparent);

	public CompProperties_Shield Props => (CompProperties_Shield)props;

	private float EnergyMax => parent.GetStatValue(StatDefOf.EnergyShieldEnergyMax);

	private float EnergyGainPerTick => parent.GetStatValue(StatDefOf.EnergyShieldRechargeRate) / 60f;

	public float Energy => energy;

	public ShieldState ShieldState
	{
		get
		{
			if (parent is Pawn p && (p.IsCharging() || p.IsSelfShutdown()))
			{
				return ShieldState.Disabled;
			}
			CompCanBeDormant comp = parent.GetComp<CompCanBeDormant>();
			if (comp != null && !comp.Awake)
			{
				return ShieldState.Disabled;
			}
			if (ticksToReset <= 0)
			{
				return ShieldState.Active;
			}
			return ShieldState.Resetting;
		}
	}

	protected bool ShouldDisplay
	{
		get
		{
			Pawn pawnOwner = PawnOwner;
			if (!pawnOwner.Spawned || pawnOwner.Dead || pawnOwner.Downed)
			{
				return false;
			}
			if (pawnOwner.InAggroMentalState)
			{
				return true;
			}
			if (pawnOwner.Drafted)
			{
				return true;
			}
			if (pawnOwner.Faction.HostileTo(Faction.OfPlayer) && !pawnOwner.IsPrisoner)
			{
				return true;
			}
			if (Find.TickManager.TicksGame < lastKeepDisplayTick + KeepDisplayingTicks)
			{
				return true;
			}
			if (ModsConfig.BiotechActive && pawnOwner.IsColonyMech && Find.Selector.SingleSelectedThing == pawnOwner)
			{
				return true;
			}
			return false;
		}
	}

	protected Pawn PawnOwner
	{
		get
		{
			if (parent is Apparel apparel)
			{
				return apparel.Wearer;
			}
			if (parent is Pawn result)
			{
				return result;
			}
			return null;
		}
	}

	public bool IsApparel => parent is Apparel;

	private bool IsBuiltIn => !IsApparel;

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref energy, "energy", 0f);
		Scribe_Values.Look(ref ticksToReset, "ticksToReset", -1);
		Scribe_Values.Look(ref lastKeepDisplayTick, "lastKeepDisplayTick", 0);
	}

	[IteratorStateMachine(typeof(_003CCompGetWornGizmosExtra_003Ed__29))]
	public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetWornGizmosExtra_003Ed__29(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__30))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__30(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__31))]
	private IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__31(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override float CompGetSpecialApparelScoreOffset()
	{
		return EnergyMax * ApparelScorePerEnergyMax;
	}

	public override void CompTick()
	{
		base.CompTick();
		if (PawnOwner == null)
		{
			energy = 0f;
		}
		else if (ShieldState == ShieldState.Resetting)
		{
			ticksToReset--;
			if (ticksToReset <= 0)
			{
				Reset();
			}
		}
		else if (ShieldState == ShieldState.Active)
		{
			energy += EnergyGainPerTick;
			if (energy > EnergyMax)
			{
				energy = EnergyMax;
			}
		}
	}

	public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
	{
		absorbed = false;
		if (ShieldState != 0 || PawnOwner == null)
		{
			return;
		}
		if (dinfo.Def == DamageDefOf.EMP)
		{
			energy = 0f;
			Break();
		}
		else if (!dinfo.Def.ignoreShields && (dinfo.Def.isRanged || dinfo.Def.isExplosive))
		{
			energy -= dinfo.Amount * Props.energyLossPerDamage;
			if (energy < 0f)
			{
				Break();
			}
			else
			{
				AbsorbedDamage(dinfo);
			}
			absorbed = true;
		}
	}

	public void KeepDisplaying()
	{
		lastKeepDisplayTick = Find.TickManager.TicksGame;
	}

	private void AbsorbedDamage(DamageInfo dinfo)
	{
		SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(PawnOwner.Position, PawnOwner.Map));
		impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
		Vector3 loc = PawnOwner.TrueCenter() + impactAngleVect.RotatedBy(180f) * 0.5f;
		float num = Mathf.Min(10f, 2f + dinfo.Amount / 10f);
		FleckMaker.Static(loc, PawnOwner.Map, FleckDefOf.ExplosionFlash, num);
		int num2 = (int)num;
		for (int i = 0; i < num2; i++)
		{
			FleckMaker.ThrowDustPuff(loc, PawnOwner.Map, Rand.Range(0.8f, 1.2f));
		}
		lastAbsorbDamageTick = Find.TickManager.TicksGame;
		KeepDisplaying();
	}

	private void Break()
	{
		if (parent.Spawned)
		{
			float scale = Mathf.Lerp(Props.minDrawSize, Props.maxDrawSize, energy);
			EffecterDefOf.Shield_Break.SpawnAttached(parent, parent.MapHeld, scale);
			FleckMaker.Static(PawnOwner.TrueCenter(), PawnOwner.Map, FleckDefOf.ExplosionFlash, 12f);
			for (int i = 0; i < 6; i++)
			{
				FleckMaker.ThrowDustPuff(PawnOwner.TrueCenter() + Vector3Utility.HorizontalVectorFromAngle(Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f), PawnOwner.Map, Rand.Range(0.8f, 1.2f));
			}
		}
		energy = 0f;
		ticksToReset = Props.startingTicksToReset;
	}

	private void Reset()
	{
		if (PawnOwner.Spawned)
		{
			SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(PawnOwner.Position, PawnOwner.Map));
			FleckMaker.ThrowLightningGlow(PawnOwner.TrueCenter(), PawnOwner.Map, 3f);
		}
		ticksToReset = -1;
		energy = Props.energyOnReset;
	}

	public override void CompDrawWornExtras()
	{
		base.CompDrawWornExtras();
		if (IsApparel)
		{
			Draw();
		}
	}

	public override void PostDraw()
	{
		base.PostDraw();
		if (IsBuiltIn)
		{
			Draw();
		}
	}

	private void Draw()
	{
		if (ShieldState == ShieldState.Active && ShouldDisplay)
		{
			float num = Mathf.Lerp(Props.minDrawSize, Props.maxDrawSize, energy);
			Vector3 drawPos = PawnOwner.Drawer.DrawPos;
			drawPos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
			int num2 = Find.TickManager.TicksGame - lastAbsorbDamageTick;
			if (num2 < 8)
			{
				float num3 = (float)(8 - num2) / 8f * 0.05f;
				drawPos += impactAngleVect * num3;
				num -= num3;
			}
			float angle = Rand.Range(0, 360);
			Vector3 s = new Vector3(num, 1f, num);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawPos, Quaternion.AngleAxis(angle, Vector3.up), s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, BubbleMat, 0);
		}
	}

	public override bool CompAllowVerbCast(Verb verb)
	{
		if (Props.blocksRangedWeapons)
		{
			return !(verb is Verb_LaunchProjectile);
		}
		return true;
	}
}
