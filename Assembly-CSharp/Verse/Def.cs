using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using RimWorld;

namespace Verse;

public class Def : Editable, IEquatable<Def>
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass23_0
	{
		public Def _003C_003E4__this;

		public int i;

		public Func<DefHyperlink, bool> _003C_003E9__1;

		internal bool _003CConfigErrors_003Eb__1(DefHyperlink h)
		{
			return h.def == _003C_003E4__this.descriptionHyperlinks[i].def;
		}
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__23 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Def _003C_003E4__this;

		private _003C_003Ec__DisplayClass23_0 _003C_003E8__1;

		private int _003Ci_003E5__2;

		private IEnumerator<string> _003C_003E7__wrap2;

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
		public _003CConfigErrors_003Ed__23(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 4)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Def def = _003C_003E4__this;
				int num2;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass23_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					if (def.defName == "UnnamedDef")
					{
						_003C_003E2__current = def.GetType()?.ToString() + " lacks defName. Label=" + def.label;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00b5;
				case 1:
					_003C_003E1__state = -1;
					goto IL_00b5;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00e7;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0129;
				case 4:
					_003C_003E1__state = -3;
					goto IL_0194;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0213;
				case 6:
					_003C_003E1__state = -1;
					goto IL_0246;
				case 7:
					_003C_003E1__state = -1;
					goto IL_0285;
				case 8:
					_003C_003E1__state = -1;
					goto IL_03b3;
				case 9:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_03cf:
					if (_003C_003E8__1.i > 0)
					{
						if (def.descriptionHyperlinks.FirstIndexOf((DefHyperlink h) => h.def == _003C_003E8__1._003C_003E4__this.descriptionHyperlinks[_003C_003E8__1.i].def) < _003C_003E8__1.i)
						{
							_003C_003E2__current = "Hyperlink to " + def.descriptionHyperlinks[_003C_003E8__1.i].def.defName + " more than once on " + def.defName + " description";
							_003C_003E1__state = 8;
							return true;
						}
						goto IL_03b3;
					}
					goto IL_03e0;
					IL_01d6:
					if (def.description != null)
					{
						if (def.description == "")
						{
							_003C_003E2__current = "empty description";
							_003C_003E1__state = 5;
							return true;
						}
						goto IL_0213;
					}
					goto IL_0285;
					IL_03b3:
					num2 = _003C_003E8__1.i;
					_003C_003E8__1.i = num2 - 1;
					goto IL_03cf;
					IL_0285:
					if (def.descriptionHyperlinks != null && def.descriptionHyperlinks.Count > 0)
					{
						if (def.descriptionHyperlinks.RemoveAll((DefHyperlink x) => x.def == null) != 0)
						{
							Log.Warning("Some descriptionHyperlinks in " + def.defName + " had null def.");
						}
						_003C_003E8__1.i = def.descriptionHyperlinks.Count - 1;
						goto IL_03cf;
					}
					goto IL_03e0;
					IL_00b5:
					if (def.defName == "null")
					{
						_003C_003E2__current = "defName cannot be the string 'null'.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00e7;
					IL_03e0:
					if (def.label != null && !def.ignoreIllegalLabelCharacterConfigError && DisallowedLabelCharsRegex.IsMatch(def.label))
					{
						_003C_003E2__current = "label contains illegal character(s): \"[]{}\". This can cause issues during grammar resolution. If this was intended, you can use the \"ignoreIllegalLabelCharacterConfigError\" flag.";
						_003C_003E1__state = 9;
						return true;
					}
					break;
					IL_00e7:
					if (!AllowedDefNamesRegex.IsMatch(def.defName))
					{
						_003C_003E2__current = "defName " + def.defName + " should only contain letters, numbers, underscores, or dashes.";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0129;
					IL_0213:
					if (char.IsWhiteSpace(def.description[0]))
					{
						_003C_003E2__current = "description has leading whitespace";
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_0246;
					IL_0129:
					if (def.modExtensions != null)
					{
						_003Ci_003E5__2 = 0;
						goto IL_01c0;
					}
					goto IL_01d6;
					IL_01c0:
					if (_003Ci_003E5__2 < def.modExtensions.Count)
					{
						_003C_003E7__wrap2 = def.modExtensions[_003Ci_003E5__2].ConfigErrors().GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0194;
					}
					goto IL_01d6;
					IL_0246:
					if (char.IsWhiteSpace(def.description[def.description.Length - 1]))
					{
						_003C_003E2__current = "description has trailing whitespace";
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_0285;
					IL_0194:
					if (_003C_003E7__wrap2.MoveNext())
					{
						string current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					num2 = _003Ci_003E5__2 + 1;
					_003Ci_003E5__2 = num2;
					goto IL_01c0;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__23 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__23(0)
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
	private sealed class _003CSpecialDisplayStats_003Ed__19 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Def _003C_003E4__this;

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
		public _003CSpecialDisplayStats_003Ed__19(int _003C_003E1__state)
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
			Def def = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (def.modContentPack != null && !def.modContentPack.IsCoreMod)
				{
					TaggedString taggedString = (def.modContentPack.IsOfficialMod ? "Stat_Source_OfficialExpansionReport".Translate() : "Stat_Source_ModReport".Translate());
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Source, "Stat_Source_Label".Translate(), def.modContentPack.Name, taggedString + ": " + def.modContentPack.Name, 90000, null, null, forceUnfinalizedMode: false, overridesHideStats: true);
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
			_003CSpecialDisplayStats_003Ed__19 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__19(0)
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

	[Description("The name of this Def. It is used as an identifier by the game code.")]
	[NoTranslate]
	public string defName = "UnnamedDef";

	[Description("A human-readable label used to identify this in game.")]
	[DefaultValue(null)]
	[MustTranslate]
	public string label;

	[Description("A human-readable description given when the Def is inspected by players.")]
	[DefaultValue(null)]
	[MustTranslate]
	public string description;

	[XmlInheritanceAllowDuplicateNodes]
	public List<DefHyperlink> descriptionHyperlinks;

	[Description("Disables config error checking. Intended for mod use. (Be careful!)")]
	[DefaultValue(false)]
	[MustTranslate]
	public bool ignoreConfigErrors;

	public bool ignoreIllegalLabelCharacterConfigError;

	[Description("Mod-specific data. Not used by core game code.")]
	[DefaultValue(null)]
	public List<DefModExtension> modExtensions;

	[Unsaved(false)]
	public ushort shortHash;

	[Unsaved(false)]
	public ushort index = ushort.MaxValue;

	[Unsaved(false)]
	public ModContentPack modContentPack;

	[Unsaved(false)]
	public string fileName;

	[Unsaved(false)]
	public int defNameHash;

	[Unsaved(false)]
	protected TaggedString cachedLabelCap = null;

	[Unsaved(false)]
	public bool generated;

	[Unsaved(false)]
	public ushort debugRandomId = (ushort)Rand.RangeInclusive(0, 65535);

	public const string DefaultDefName = "UnnamedDef";

	private static readonly Regex AllowedDefNamesRegex = new Regex("^[a-zA-Z0-9\\-_]*$");

	private static readonly Regex DisallowedLabelCharsRegex = new Regex("\\[|\\]|\\{|\\}");

	public virtual TaggedString LabelCap
	{
		get
		{
			if (label.NullOrEmpty())
			{
				return null;
			}
			if (cachedLabelCap.NullOrEmpty())
			{
				cachedLabelCap = label.CapitalizeFirst();
			}
			return cachedLabelCap;
		}
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__19))]
	public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__19(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		if (modExtensions != null)
		{
			for (int i = 0; i < modExtensions.Count; i++)
			{
				modExtensions[i].ResolveReferences(this);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__23))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__23(-2)
		{
			_003C_003E4__this = this
		};
	}

	public virtual void PostSetIndices()
	{
	}

	public virtual void ClearCachedData()
	{
		cachedLabelCap = null;
	}

	public override string ToString()
	{
		return defName;
	}

	public void ResolveDefNameHash()
	{
		defNameHash = defName.GetHashCode();
	}

	public override int GetHashCode()
	{
		return defNameHash;
	}

	public T GetModExtension<T>() where T : DefModExtension
	{
		if (modExtensions == null)
		{
			return null;
		}
		for (int i = 0; i < modExtensions.Count; i++)
		{
			if (modExtensions[i] is T)
			{
				return modExtensions[i] as T;
			}
		}
		return null;
	}

	public bool HasModExtension<T>() where T : DefModExtension
	{
		return GetModExtension<T>() != null;
	}

	public bool Equals(Def other)
	{
		if (other != null && other.defNameHash == defNameHash)
		{
			return other.GetType() == GetType();
		}
		return false;
	}
}
