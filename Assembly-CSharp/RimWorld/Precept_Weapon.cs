using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Precept_Weapon : Precept
{
	[CompilerGenerated]
	private sealed class _003CEditFloatMenuOptions_003Ed__20 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Precept_Weapon _003C_003E4__this;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CEditFloatMenuOptions_003Ed__20(int _003C_003E1__state)
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
			Precept_Weapon CS_0024_003C_003E8__locals3 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new FloatMenuOption("SwapNobleAndDespised".Translate(), delegate
				{
					Gen.Swap(ref CS_0024_003C_003E8__locals3.noble, ref CS_0024_003C_003E8__locals3.despised);
					CS_0024_003C_003E8__locals3.RecacheData();
				});
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CEditFloatMenuOptions_003Ed__20 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEditFloatMenuOptions_003Ed__20(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	public WeaponClassDef noble;

	public WeaponClassDef despised;

	[Unsaved(false)]
	private ThingDef iconWeapon;

	[Unsaved(false)]
	private HashSet<ThingDef> nobleWeaponsCached;

	[Unsaved(false)]
	private HashSet<ThingDef> despisedWeaponsCached;

	public override string TipLabel => def.issue.LabelCap + ": " + def.LabelCap;

	public override string UIInfoFirstLine => "Noble".Translate().CapitalizeFirst() + ": " + noble.LabelCap;

	public override string UIInfoSecondLine => "Despised".Translate().CapitalizeFirst() + ": " + despised.LabelCap;

	public override Color LabelColor => Color.white;

	protected HashSet<ThingDef> NobleWeapons
	{
		get
		{
			if (nobleWeaponsCached.EnumerableNullOrEmpty())
			{
				nobleWeaponsCached = new HashSet<ThingDef>(DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => x.IsWeapon && x.PlayerAcquirable && !x.weaponClasses.NullOrEmpty() && x.weaponClasses.Contains(noble)));
			}
			return nobleWeaponsCached;
		}
	}

	protected HashSet<ThingDef> DespisedWeapons
	{
		get
		{
			if (despisedWeaponsCached.EnumerableNullOrEmpty())
			{
				despisedWeaponsCached = new HashSet<ThingDef>(DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => x.IsWeapon && !x.weaponClasses.NullOrEmpty() && x.weaponClasses.Contains(despised)));
			}
			return despisedWeaponsCached;
		}
	}

	private void RecacheData()
	{
		nobleWeaponsCached = null;
		despisedWeaponsCached = null;
		iconWeapon = null;
		ClearTipCache();
	}

	public override void DrawIcon(Rect rect)
	{
		if (iconWeapon == null)
		{
			Rand.PushState(randomSeed);
			iconWeapon = NobleWeapons.Where((ThingDef w) => !w.IsStuff).RandomElementWithFallback();
			Rand.PopState();
		}
		if (iconWeapon == null)
		{
			Rand.PushState(randomSeed);
			iconWeapon = DespisedWeapons.Where((ThingDef w) => !w.IsStuff).RandomElementWithFallback();
			Rand.PopState();
		}
		Widgets.DefIcon(rect, iconWeapon, null, 1f, ideo.GetStyleFor(iconWeapon), drawPlaceholder: false, null, null, null);
	}

	public override string GetTip()
	{
		if (tipCached.NullOrEmpty())
		{
			tipCached = base.GetTip() + "\n\n" + ColorizeDescTitle("Noble".Translate().CapitalizeFirst()) + ":\n" + (from x in NobleWeapons
				where x.canGenerateDefaultDesignator
				select x.LabelCap.Resolve()).ToCommaList() + "\n\n" + ColorizeDescTitle("Despised".Translate().CapitalizeFirst()) + ":\n" + (from x in DespisedWeapons
				where x.canGenerateDefaultDesignator
				select x.LabelCap.Resolve()).ToCommaList();
		}
		return tipCached;
	}

	[IteratorStateMachine(typeof(_003CEditFloatMenuOptions_003Ed__20))]
	public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEditFloatMenuOptions_003Ed__20(-2)
		{
			_003C_003E4__this = this
		};
	}

	public IdeoWeaponDisposition GetDispositionForWeapon(ThingDef td)
	{
		if (NobleWeapons.Contains(td))
		{
			return IdeoWeaponDisposition.Noble;
		}
		if (DespisedWeapons.Contains(td))
		{
			return IdeoWeaponDisposition.Despised;
		}
		return IdeoWeaponDisposition.None;
	}

	public override bool CompatibleWith(Precept other)
	{
		Precept_Weapon wep = other as Precept_Weapon;
		if (wep != null)
		{
			if (!NobleWeapons.Any((ThingDef x) => wep.DespisedWeapons.Contains(x)))
			{
				return !DespisedWeapons.Any((ThingDef x) => wep.NobleWeapons.Contains(x));
			}
			return false;
		}
		return base.CompatibleWith(other);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref noble, "noble");
		Scribe_Defs.Look(ref despised, "despised");
	}

	public override void CopyTo(Precept other)
	{
		base.CopyTo(other);
		Precept_Weapon obj = (Precept_Weapon)other;
		obj.noble = noble;
		obj.despised = despised;
	}
}
