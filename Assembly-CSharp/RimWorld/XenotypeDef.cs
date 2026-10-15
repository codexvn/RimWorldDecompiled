using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class XenotypeDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__27 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public XenotypeDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<XenotypeChance>.Enumerator _003C_003E7__wrap2;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__27(int _003C_003E1__state)
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
			case 4:
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
			_003C_003E7__wrap2 = default(List<XenotypeChance>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				XenotypeDef xenotypeDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)xenotypeDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00c1;
				case 3:
					_003C_003E1__state = -1;
					goto IL_011d;
				case 4:
					{
						_003C_003E1__state = -4;
						goto IL_0182;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (xenotypeDef.iconPath.NullOrEmpty())
					{
						_003C_003E2__current = "iconPath is empty.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00c1;
					IL_0182:
					while (_003C_003E7__wrap2.MoveNext())
					{
						XenotypeChance current2 = _003C_003E7__wrap2.Current;
						if (!current2.xenotype.inheritable)
						{
							_003C_003E2__current = "doubleXenotypeChance contains non-inheritable xenotype " + current2.xenotype.label;
							_003C_003E1__state = 4;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<XenotypeChance>.Enumerator);
					break;
					IL_00c1:
					if (xenotypeDef.doubleXenotypeChances == null)
					{
						break;
					}
					if (xenotypeDef.doubleXenotypeChances.Sum((XenotypeChance x) => x.chance) > 1f)
					{
						_003C_003E2__current = "doubleXenotypeChance sum is > 1";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_011d;
					IL_011d:
					_003C_003E7__wrap2 = xenotypeDef.doubleXenotypeChances.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0182;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__27 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__27(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__26 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public XenotypeDef _003C_003E4__this;

		private StatRequest req;

		public StatRequest _003C_003E3__req;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

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
		public _003CSpecialDisplayStats_003Ed__26(int _003C_003E1__state)
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
				XenotypeDef xenotypeDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)xenotypeDef).SpecialDisplayStats(req).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007c;
				case 2:
					_003C_003E1__state = -1;
					if (!xenotypeDef.genes.NullOrEmpty())
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "GenesAreInheritable".Translate(), xenotypeDef.inheritable.ToStringYesNo(), "GenesAreInheritableXenotypeDef".Translate(), 990);
						_003C_003E1__state = 3;
						return true;
					}
					break;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "Genes".Translate().CapitalizeFirst(), xenotypeDef.genes.Select((GeneDef x) => x.label).ToCommaList().CapitalizeFirst(), "GenesDesc".Translate() + "\n\n" + xenotypeDef.genes.Select((GeneDef x) => x.label).ToLineList("  - ", capitalizeItems: true), 1000);
					_003C_003E1__state = 2;
					return true;
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__26 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__26(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CSpecialDisplayStats_003Ed__.req = _003C_003E3__req;
			return _003CSpecialDisplayStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public List<GeneDef> genes = new List<GeneDef>();

	public bool inheritable;

	[NoTranslate]
	public string iconPath;

	[MustTranslate]
	public string descriptionShort;

	public float combatPowerFactor = 1f;

	public bool canGenerateAsCombatant = true;

	public float factionlessGenerationWeight = 1f;

	public RulePackDef nameMaker;

	public RulePackDef nameMakerFemale;

	public float chanceToUseNameMaker;

	public List<WeaponClassDef> forbiddenWeaponClasses;

	public float generateWithXenogermReplicatingHediffChance;

	public FloatRange xenogermReplicatingDurationLeftDaysRange = FloatRange.Zero;

	public SoundDef soundDefOnImplant;

	public List<XenotypeChance> doubleXenotypeChances;

	public float displayPriority;

	[Unsaved(false)]
	private Texture2D cachedIcon;

	public static readonly Color IconColor = new Color(0.75f, 0.75f, 0.75f);

	public Texture2D Icon
	{
		get
		{
			if (cachedIcon == null)
			{
				if (iconPath.NullOrEmpty())
				{
					cachedIcon = BaseContent.BadTex;
				}
				else
				{
					cachedIcon = ContentFinder<Texture2D>.Get(iconPath);
				}
			}
			return cachedIcon;
		}
	}

	public List<GeneDef> AllGenes => genes;

	public bool Archite => AllGenes.Any((GeneDef g) => g.displayCategory == GeneCategoryDefOf.Archite);

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		if (genes.NullOrEmpty())
		{
			return;
		}
		if (descriptionHyperlinks == null)
		{
			descriptionHyperlinks = new List<DefHyperlink>();
		}
		foreach (GeneDef gene in genes)
		{
			descriptionHyperlinks.Add(new DefHyperlink(gene));
		}
	}

	public RulePackDef GetNameMaker(Gender gender)
	{
		if (gender == Gender.Female && nameMakerFemale != null)
		{
			return nameMakerFemale;
		}
		return nameMaker;
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__26))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__26(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__req = req
		};
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__27))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__27(-2)
		{
			_003C_003E4__this = this
		};
	}
}
