using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using RimWorld;
using RimWorld.Planet;

namespace Verse.Grammar;

public static class GrammarUtility
{
	[StructLayout(LayoutKind.Auto)]
	[CompilerGenerated]
	private struct _003C_003Ec__DisplayClass7_0
	{
		public WorldObject worldObject;

		public bool addTags;
	}

	[CompilerGenerated]
	private sealed class _003CRulesForBodyPartRecord_003Ed__4 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private BodyPartRecord part;

		public BodyPartRecord _003C_003E3__part;

		private string prefix;

		public string _003C_003E3__prefix;

		Rule IEnumerator<Rule>.Current
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
		public _003CRulesForBodyPartRecord_003Ed__4(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (part == null)
				{
					Log.ErrorOnce("Tried to insert rule " + prefix + " for null body part", 394876778);
					return false;
				}
				if (!prefix.NullOrEmpty())
				{
					prefix += "_";
				}
				_003C_003E2__current = new Rule_String(prefix + "label", part.Label);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "definite", Find.ActiveLanguageWorker.WithDefiniteArticle(part.Label));
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(part.Label));
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "possessive", "Proits".Translate());
				_003C_003E1__state = 4;
				return true;
			case 4:
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
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CRulesForBodyPartRecord_003Ed__4 _003CRulesForBodyPartRecord_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRulesForBodyPartRecord_003Ed__ = this;
			}
			else
			{
				_003CRulesForBodyPartRecord_003Ed__ = new _003CRulesForBodyPartRecord_003Ed__4(0);
			}
			_003CRulesForBodyPartRecord_003Ed__.prefix = _003C_003E3__prefix;
			_003CRulesForBodyPartRecord_003Ed__.part = _003C_003E3__part;
			return _003CRulesForBodyPartRecord_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRulesForDef_003Ed__3 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Def def;

		public Def _003C_003E3__def;

		private string prefix;

		public string _003C_003E3__prefix;

		Rule IEnumerator<Rule>.Current
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
		public _003CRulesForDef_003Ed__3(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (def == null)
				{
					Log.ErrorOnce("Tried to insert rule " + prefix + " for null def", 79641686);
					return false;
				}
				if (!prefix.NullOrEmpty())
				{
					prefix += "_";
				}
				_003C_003E2__current = new Rule_String(prefix + "label", def.label);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (def is PawnKindDef pawnKindDef)
				{
					_003C_003E2__current = new Rule_String(prefix + "labelPlural", pawnKindDef.GetLabelPlural());
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E2__current = new Rule_String(prefix + "labelPlural", Find.ActiveLanguageWorker.Pluralize(def.label));
				_003C_003E1__state = 3;
				return true;
			case 2:
				_003C_003E1__state = -1;
				goto IL_013c;
			case 3:
				_003C_003E1__state = -1;
				goto IL_013c;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "definite", Find.ActiveLanguageWorker.WithDefiniteArticle(def.label));
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(def.label));
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "possessive", "Proits".Translate());
				_003C_003E1__state = 7;
				return true;
			case 7:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_013c:
				_003C_003E2__current = new Rule_String(prefix + "description", def.description);
				_003C_003E1__state = 4;
				return true;
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
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CRulesForDef_003Ed__3 _003CRulesForDef_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRulesForDef_003Ed__ = this;
			}
			else
			{
				_003CRulesForDef_003Ed__ = new _003CRulesForDef_003Ed__3(0);
			}
			_003CRulesForDef_003Ed__.prefix = _003C_003E3__prefix;
			_003CRulesForDef_003Ed__.def = _003C_003E3__def;
			return _003CRulesForDef_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRulesForFaction_003Ed__6 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string prefix;

		public string _003C_003E3__prefix;

		private Faction faction;

		public Faction _003C_003E3__faction;

		private bool addTags;

		public bool _003C_003E3__addTags;

		private Dictionary<string, string> constants;

		public Dictionary<string, string> _003C_003E3__constants;

		Rule IEnumerator<Rule>.Current
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
		public _003CRulesForFaction_003Ed__6(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!prefix.NullOrEmpty())
				{
					prefix += "_";
				}
				if (faction == null)
				{
					_003C_003E2__current = new Rule_String(prefix + "name", "FactionUnaffiliated".Translate());
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = new Rule_String(prefix + "name", addTags ? faction.Name.ApplyTag(faction).Resolve() : faction.Name);
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "pawnSingular", faction.def.pawnSingular);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "pawnSingularDef", Find.ActiveLanguageWorker.WithDefiniteArticle(faction.def.pawnSingular));
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "pawnSingularIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(faction.def.pawnSingular));
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "pawnsPlural", faction.def.pawnsPlural);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "pawnsPluralDef", Find.ActiveLanguageWorker.WithDefiniteArticle(faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(faction.def.pawnsPlural, faction.def.pawnSingular), plural: true));
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "pawnsPluralIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(faction.def.pawnsPlural, LanguageDatabase.activeLanguage.ResolveGender(faction.def.pawnsPlural, faction.def.pawnSingular), plural: true));
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "leaderTitle", faction.LeaderTitle);
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "royalFavorLabel", faction.def.royalFavorLabel);
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				if (constants != null)
				{
					constants.Add(prefix + "temporary", faction.temporary.ToString());
					constants.Add(prefix + "hasLeader", (faction.leader != null) ? "True" : "False");
				}
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
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CRulesForFaction_003Ed__6 _003CRulesForFaction_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRulesForFaction_003Ed__ = this;
			}
			else
			{
				_003CRulesForFaction_003Ed__ = new _003CRulesForFaction_003Ed__6(0);
			}
			_003CRulesForFaction_003Ed__.prefix = _003C_003E3__prefix;
			_003CRulesForFaction_003Ed__.faction = _003C_003E3__faction;
			_003CRulesForFaction_003Ed__.constants = _003C_003E3__constants;
			_003CRulesForFaction_003Ed__.addTags = _003C_003E3__addTags;
			return _003CRulesForFaction_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRulesForHediffDef_003Ed__5 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string prefix;

		public string _003C_003E3__prefix;

		private HediffDef def;

		public HediffDef _003C_003E3__def;

		private BodyPartRecord part;

		public BodyPartRecord _003C_003E3__part;

		private IEnumerator<Rule> _003C_003E7__wrap1;

		Rule IEnumerator<Rule>.Current
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
		public _003CRulesForHediffDef_003Ed__5(int _003C_003E1__state)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = RulesForDef(prefix, def).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0080;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0080;
				case 2:
				{
					_003C_003E1__state = -1;
					string output = ((!def.labelNoun.NullOrEmpty()) ? def.labelNoun : def.label);
					_003C_003E2__current = new Rule_String(prefix + "labelNoun", output);
					_003C_003E1__state = 3;
					return true;
				}
				case 3:
				{
					_003C_003E1__state = -1;
					string text = def.PrettyTextForPart(part);
					if (!text.NullOrEmpty())
					{
						_003C_003E2__current = new Rule_String(prefix + "labelNounPretty", text);
						_003C_003E1__state = 4;
						return true;
					}
					break;
				}
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0080:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Rule current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!prefix.NullOrEmpty())
					{
						prefix += "_";
					}
					_003C_003E2__current = new Rule_String(prefix + "label", def.label);
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
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CRulesForHediffDef_003Ed__5 _003CRulesForHediffDef_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRulesForHediffDef_003Ed__ = this;
			}
			else
			{
				_003CRulesForHediffDef_003Ed__ = new _003CRulesForHediffDef_003Ed__5(0);
			}
			_003CRulesForHediffDef_003Ed__.prefix = _003C_003E3__prefix;
			_003CRulesForHediffDef_003Ed__.def = _003C_003E3__def;
			_003CRulesForHediffDef_003Ed__.part = _003C_003E3__part;
			return _003CRulesForHediffDef_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRulesForIdeo_003Ed__8 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Ideo ideo;

		public Ideo _003C_003E3__ideo;

		private string prefix;

		public string _003C_003E3__prefix;

		Rule IEnumerator<Rule>.Current
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
		public _003CRulesForIdeo_003Ed__8(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (ideo == null)
				{
					Log.ErrorOnce("Tried to insert rule " + prefix + " for null ideo", 453454453);
					return false;
				}
				if (!prefix.NullOrEmpty())
				{
					prefix += "_";
				}
				_003C_003E2__current = new Rule_String(prefix + "name", ideo.name.ApplyTag(ideo).Resolve());
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "memberName", ideo.memberName);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "memberNamePlural", ideo.MemberNamePlural);
				_003C_003E1__state = 3;
				return true;
			case 3:
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
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CRulesForIdeo_003Ed__8 _003CRulesForIdeo_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRulesForIdeo_003Ed__ = this;
			}
			else
			{
				_003CRulesForIdeo_003Ed__ = new _003CRulesForIdeo_003Ed__8(0);
			}
			_003CRulesForIdeo_003Ed__.prefix = _003C_003E3__prefix;
			_003CRulesForIdeo_003Ed__.ideo = _003C_003E3__ideo;
			return _003CRulesForIdeo_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRulesForPawn_003Ed__1 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string pawnSymbol;

		public string _003C_003E3__pawnSymbol;

		private Gender gender;

		public Gender _003C_003E3__gender;

		private PawnKindDef kind;

		public PawnKindDef _003C_003E3__kind;

		private Name name;

		public Name _003C_003E3__name;

		private bool addTags;

		public bool _003C_003E3__addTags;

		private string labelNoParenthesis;

		public string _003C_003E3__labelNoParenthesis;

		private Faction faction;

		public Faction _003C_003E3__faction;

		private Dictionary<string, string> constants;

		public Dictionary<string, string> _003C_003E3__constants;

		private bool isFactionLeader;

		public bool _003C_003E3__isFactionLeader;

		private string title;

		public string _003C_003E3__title;

		private List<RoyalTitle> royalTitles;

		public List<RoyalTitle> _003C_003E3__royalTitles;

		private int age;

		public int _003C_003E3__age;

		private int chronologicalAge;

		public int _003C_003E3__chronologicalAge;

		private bool everBeenColonistOrTameAnimal;

		public bool _003C_003E3__everBeenColonistOrTameAnimal;

		private bool everBeenQuestLodger;

		public bool _003C_003E3__everBeenQuestLodger;

		private string relationInfo;

		public string _003C_003E3__relationInfo;

		private bool cubeInterest;

		public bool _003C_003E3__cubeInterest;

		private string _003Cprefix_003E5__2;

		private string _003CkindLabel_003E5__3;

		private Gender _003CkindGender_003E5__4;

		private string _003CnameShort_003E5__5;

		private string _003CnameShortDef_003E5__6;

		private Gender _003CtitleGender_003E5__7;

		private int _003CroyalTitleIndex_003E5__8;

		private RoyalTitle _003CbestTitle_003E5__9;

		private IEnumerator<RoyalTitle> _003C_003E7__wrap9;

		private RoyalTitle _003CroyalTitle_003E5__11;

		Rule IEnumerator<Rule>.Current
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
		public _003CRulesForPawn_003Ed__1(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 17) <= 6u)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003Cprefix_003E5__2 = null;
			_003CkindLabel_003E5__3 = null;
			_003CnameShort_003E5__5 = null;
			_003CnameShortDef_003E5__6 = null;
			_003CbestTitle_003E5__9 = null;
			_003C_003E7__wrap9 = null;
			_003CroyalTitle_003E5__11 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					_003Cprefix_003E5__2 = "";
					if (!pawnSymbol.NullOrEmpty())
					{
						_003Cprefix_003E5__2 = _003Cprefix_003E5__2 + pawnSymbol + "_";
					}
					if (gender == Gender.Female)
					{
						_003CkindLabel_003E5__3 = (kind.labelFemale.NullOrEmpty() ? kind.label : kind.labelFemale);
					}
					else if (gender == Gender.Male)
					{
						_003CkindLabel_003E5__3 = (kind.labelMale.NullOrEmpty() ? kind.label : kind.labelMale);
					}
					else
					{
						_003CkindLabel_003E5__3 = kind.label;
					}
					_003CkindGender_003E5__4 = GrammarResolverSimple.ResolveGender(_003CkindLabel_003E5__3, gender);
					string text = ((name == null) ? Find.ActiveLanguageWorker.WithIndefiniteArticle(_003CkindLabel_003E5__3, _003CkindGender_003E5__4) : Find.ActiveLanguageWorker.WithIndefiniteArticle(name.ToStringFull, gender, plural: false, name: true));
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "nameFull", addTags ? text.ApplyTag(TagType.Name).Resolve() : text);
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					if (name != null)
					{
						_003CnameShort_003E5__5 = name.ToStringShort;
					}
					else
					{
						_003CnameShort_003E5__5 = _003CkindLabel_003E5__3;
					}
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "label", addTags ? _003CnameShort_003E5__5.ApplyTag(TagType.Name).Resolve() : _003CnameShort_003E5__5);
					_003C_003E1__state = 2;
					return true;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "labelNoParenthesis", addTags ? labelNoParenthesis.ApplyTag(TagType.Name).Resolve() : labelNoParenthesis);
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					if (name != null)
					{
						_003CnameShortDef_003E5__6 = Find.ActiveLanguageWorker.WithDefiniteArticle(name.ToStringShort, gender, plural: false, name: true);
					}
					else
					{
						_003CnameShortDef_003E5__6 = Find.ActiveLanguageWorker.WithDefiniteArticle(_003CkindLabel_003E5__3, _003CkindGender_003E5__4);
					}
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "definite", addTags ? _003CnameShortDef_003E5__6.ApplyTag(TagType.Name).Resolve() : _003CnameShortDef_003E5__6);
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "nameDef", addTags ? _003CnameShortDef_003E5__6.ApplyTag(TagType.Name).Resolve() : _003CnameShortDef_003E5__6);
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					_003CnameShortDef_003E5__6 = null;
					if (name != null)
					{
						_003CnameShortDef_003E5__6 = Find.ActiveLanguageWorker.WithIndefiniteArticle(name.ToStringShort, gender, plural: false, name: true);
					}
					else
					{
						_003CnameShortDef_003E5__6 = Find.ActiveLanguageWorker.WithIndefiniteArticle(_003CkindLabel_003E5__3, _003CkindGender_003E5__4);
					}
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "indefinite", addTags ? _003CnameShortDef_003E5__6.ApplyTag(TagType.Name).Resolve() : _003CnameShortDef_003E5__6);
					_003C_003E1__state = 6;
					return true;
				case 6:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "nameIndef", addTags ? _003CnameShortDef_003E5__6.ApplyTag(TagType.Name).Resolve() : _003CnameShortDef_003E5__6);
					_003C_003E1__state = 7;
					return true;
				case 7:
					_003C_003E1__state = -1;
					_003CnameShortDef_003E5__6 = null;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "pronoun", ((name != null) ? gender : _003CkindGender_003E5__4).GetPronoun());
					_003C_003E1__state = 8;
					return true;
				case 8:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "possessive", ((name != null) ? gender : _003CkindGender_003E5__4).GetPossessive());
					_003C_003E1__state = 9;
					return true;
				case 9:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "objective", ((name != null) ? gender : _003CkindGender_003E5__4).GetObjective());
					_003C_003E1__state = 10;
					return true;
				case 10:
					_003C_003E1__state = -1;
					if (faction != null)
					{
						_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "factionName", addTags ? faction.Name.ApplyTag(faction).Resolve() : faction.Name);
						_003C_003E1__state = 11;
						return true;
					}
					goto IL_064b;
				case 11:
					_003C_003E1__state = -1;
					if (constants != null)
					{
						constants[_003Cprefix_003E5__2 + "faction"] = faction.def.defName;
					}
					goto IL_064b;
				case 12:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "kindPlural", GenLabel.BestKindLabel(kind, _003CkindGender_003E5__4, plural: true));
					_003C_003E1__state = 13;
					return true;
				case 13:
					_003C_003E1__state = -1;
					goto IL_0710;
				case 14:
					_003C_003E1__state = -1;
					_003CtitleGender_003E5__7 = LanguageDatabase.activeLanguage.ResolveGender(title, null, gender);
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "titleIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(title, _003CtitleGender_003E5__7));
					_003C_003E1__state = 15;
					return true;
				case 15:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "titleDef", Find.ActiveLanguageWorker.WithDefiniteArticle(title, _003CtitleGender_003E5__7));
					_003C_003E1__state = 16;
					return true;
				case 16:
					_003C_003E1__state = -1;
					goto IL_0801;
				case 17:
					_003C_003E1__state = -3;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "royalTitle" + _003CroyalTitleIndex_003E5__8 + "Indef", Find.ActiveLanguageWorker.WithIndefiniteArticle(_003CroyalTitle_003E5__11.def.GetLabelFor(gender)));
					_003C_003E1__state = 18;
					return true;
				case 18:
					_003C_003E1__state = -3;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "royalTitle" + _003CroyalTitleIndex_003E5__8 + "Def", Find.ActiveLanguageWorker.WithDefiniteArticle(_003CroyalTitle_003E5__11.def.GetLabelFor(gender)));
					_003C_003E1__state = 19;
					return true;
				case 19:
					_003C_003E1__state = -3;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "royalTitleFaction" + _003CroyalTitleIndex_003E5__8, _003CroyalTitle_003E5__11.faction.Name.ApplyTag(_003CroyalTitle_003E5__11.faction).Resolve());
					_003C_003E1__state = 20;
					return true;
				case 20:
					_003C_003E1__state = -3;
					if (_003CroyalTitle_003E5__11.faction == faction)
					{
						_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "royalTitleInCurrentFaction", _003CroyalTitle_003E5__11.def.GetLabelFor(gender));
						_003C_003E1__state = 21;
						return true;
					}
					goto IL_0b1b;
				case 21:
					_003C_003E1__state = -3;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "royalTitleInCurrentFactionIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(_003CroyalTitle_003E5__11.def.GetLabelFor(gender)));
					_003C_003E1__state = 22;
					return true;
				case 22:
					_003C_003E1__state = -3;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "royalTitleInCurrentFactionDef", Find.ActiveLanguageWorker.WithDefiniteArticle(_003CroyalTitle_003E5__11.def.GetLabelFor(gender)));
					_003C_003E1__state = 23;
					return true;
				case 23:
					_003C_003E1__state = -3;
					if (constants != null)
					{
						constants[_003Cprefix_003E5__2 + "royalInCurrentFaction"] = "True";
					}
					goto IL_0b1b;
				case 24:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "bestRoyalTitleIndef", Find.ActiveLanguageWorker.WithIndefiniteArticle(_003CbestTitle_003E5__9.def.GetLabelFor(gender)));
					_003C_003E1__state = 25;
					return true;
				case 25:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "bestRoyalTitleDef", Find.ActiveLanguageWorker.WithDefiniteArticle(_003CbestTitle_003E5__9.def.GetLabelFor(gender)));
					_003C_003E1__state = 26;
					return true;
				case 26:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "bestRoyalTitleFaction", _003CbestTitle_003E5__9.faction.Name);
					_003C_003E1__state = 27;
					return true;
				case 27:
					_003C_003E1__state = -1;
					goto IL_0cc0;
				case 28:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "chronologicalAge", chronologicalAge.ToString());
					_003C_003E1__state = 29;
					return true;
				case 29:
					_003C_003E1__state = -1;
					if (everBeenColonistOrTameAnimal)
					{
						_003C_003E2__current = new Rule_String("formerlyColonistInfo", "PawnWasFormerlyColonist".Translate(_003CnameShort_003E5__5));
						_003C_003E1__state = 30;
						return true;
					}
					if (everBeenQuestLodger)
					{
						_003C_003E2__current = new Rule_String("formerlyColonistInfo", "PawnWasFormerlyLodger".Translate(_003CnameShort_003E5__5));
						_003C_003E1__state = 31;
						return true;
					}
					goto IL_0e24;
				case 30:
					_003C_003E1__state = -1;
					if (constants != null)
					{
						constants[_003Cprefix_003E5__2 + "formerlyColonist"] = "True";
					}
					goto IL_0e24;
				case 31:
					_003C_003E1__state = -1;
					if (constants != null)
					{
						constants[_003Cprefix_003E5__2 + "formerlyColonist"] = "True";
					}
					goto IL_0e24;
				case 32:
					{
						_003C_003E1__state = -1;
						if (constants != null && kind != null)
						{
							constants[_003Cprefix_003E5__2 + "flesh"] = kind.race.race.FleshType.defName;
						}
						if (constants != null)
						{
							constants[_003Cprefix_003E5__2 + "gender"] = gender.ToString();
							constants[_003Cprefix_003E5__2 + "genderResolved"] = ((name != null) ? gender : _003CkindGender_003E5__4).ToString();
							constants[_003Cprefix_003E5__2 + "cubeInterest"] = cubeInterest.ToString();
						}
						return false;
					}
					IL_0cc0:
					_003CbestTitle_003E5__9 = null;
					goto IL_0cc7;
					IL_064b:
					if ((constants != null) & isFactionLeader)
					{
						constants[_003Cprefix_003E5__2 + "factionLeader"] = "True";
					}
					if (kind != null)
					{
						_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "kind", GenLabel.BestKindLabel(kind, _003CkindGender_003E5__4));
						_003C_003E1__state = 12;
						return true;
					}
					goto IL_0710;
					IL_0cc7:
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "age", age.ToString());
					_003C_003E1__state = 28;
					return true;
					IL_0710:
					if (title != null)
					{
						_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "title", title);
						_003C_003E1__state = 14;
						return true;
					}
					goto IL_0801;
					IL_0801:
					if (royalTitles != null)
					{
						_003CroyalTitleIndex_003E5__8 = 0;
						_003CbestTitle_003E5__9 = null;
						_003C_003E7__wrap9 = royalTitles.OrderBy((RoyalTitle x) => x.def.index).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0b6a;
					}
					goto IL_0cc7;
					IL_0e24:
					_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "relationInfo", relationInfo);
					_003C_003E1__state = 32;
					return true;
					IL_0b1b:
					if (_003CbestTitle_003E5__9 == null || _003CroyalTitle_003E5__11.def.favorCost > _003CbestTitle_003E5__9.def.favorCost)
					{
						_003CbestTitle_003E5__9 = _003CroyalTitle_003E5__11;
					}
					_003CroyalTitleIndex_003E5__8++;
					_003CroyalTitle_003E5__11 = null;
					goto IL_0b6a;
					IL_0b6a:
					if (_003C_003E7__wrap9.MoveNext())
					{
						_003CroyalTitle_003E5__11 = _003C_003E7__wrap9.Current;
						_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "royalTitle" + _003CroyalTitleIndex_003E5__8, _003CroyalTitle_003E5__11.def.GetLabelFor(gender));
						_003C_003E1__state = 17;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap9 = null;
					if (_003CbestTitle_003E5__9 != null)
					{
						_003C_003E2__current = new Rule_String(_003Cprefix_003E5__2 + "bestRoyalTitle", _003CbestTitle_003E5__9.def.GetLabelFor(gender));
						_003C_003E1__state = 24;
						return true;
					}
					goto IL_0cc0;
				}
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
			if (_003C_003E7__wrap9 != null)
			{
				_003C_003E7__wrap9.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CRulesForPawn_003Ed__1 _003CRulesForPawn_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRulesForPawn_003Ed__ = this;
			}
			else
			{
				_003CRulesForPawn_003Ed__ = new _003CRulesForPawn_003Ed__1(0);
			}
			_003CRulesForPawn_003Ed__.pawnSymbol = _003C_003E3__pawnSymbol;
			_003CRulesForPawn_003Ed__.name = _003C_003E3__name;
			_003CRulesForPawn_003Ed__.title = _003C_003E3__title;
			_003CRulesForPawn_003Ed__.kind = _003C_003E3__kind;
			_003CRulesForPawn_003Ed__.gender = _003C_003E3__gender;
			_003CRulesForPawn_003Ed__.faction = _003C_003E3__faction;
			_003CRulesForPawn_003Ed__.age = _003C_003E3__age;
			_003CRulesForPawn_003Ed__.chronologicalAge = _003C_003E3__chronologicalAge;
			_003CRulesForPawn_003Ed__.relationInfo = _003C_003E3__relationInfo;
			_003CRulesForPawn_003Ed__.everBeenColonistOrTameAnimal = _003C_003E3__everBeenColonistOrTameAnimal;
			_003CRulesForPawn_003Ed__.everBeenQuestLodger = _003C_003E3__everBeenQuestLodger;
			_003CRulesForPawn_003Ed__.isFactionLeader = _003C_003E3__isFactionLeader;
			_003CRulesForPawn_003Ed__.royalTitles = _003C_003E3__royalTitles;
			_003CRulesForPawn_003Ed__.cubeInterest = _003C_003E3__cubeInterest;
			_003CRulesForPawn_003Ed__.labelNoParenthesis = _003C_003E3__labelNoParenthesis;
			_003CRulesForPawn_003Ed__.constants = _003C_003E3__constants;
			_003CRulesForPawn_003Ed__.addTags = _003C_003E3__addTags;
			return _003CRulesForPawn_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRulesForPrecept_003Ed__9 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Precept precept;

		public Precept _003C_003E3__precept;

		private string prefix;

		public string _003C_003E3__prefix;

		Rule IEnumerator<Rule>.Current
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
		public _003CRulesForPrecept_003Ed__9(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (precept == null)
				{
					Log.ErrorOnce("Tried to insert rule " + prefix + " for null precet", 823453451);
					return false;
				}
				if (!prefix.NullOrEmpty())
				{
					prefix += "_";
				}
				_003C_003E2__current = new Rule_String(prefix + "name", precept.Label.ApplyTag(precept.ideo).Resolve());
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
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CRulesForPrecept_003Ed__9 _003CRulesForPrecept_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRulesForPrecept_003Ed__ = this;
			}
			else
			{
				_003CRulesForPrecept_003Ed__ = new _003CRulesForPrecept_003Ed__9(0);
			}
			_003CRulesForPrecept_003Ed__.prefix = _003C_003E3__prefix;
			_003CRulesForPrecept_003Ed__.precept = _003C_003E3__precept;
			return _003CRulesForPrecept_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRulesForThing_003Ed__2 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string prefix;

		public string _003C_003E3__prefix;

		private Thing thing;

		public Thing _003C_003E3__thing;

		Rule IEnumerator<Rule>.Current
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
		public _003CRulesForThing_003Ed__2(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				prefix += "_";
				if (thing.TryGetQuality(out var qc))
				{
					_003C_003E2__current = new Rule_String(prefix + "quality", qc.GetLabel().ToLower());
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
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
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CRulesForThing_003Ed__2 _003CRulesForThing_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRulesForThing_003Ed__ = this;
			}
			else
			{
				_003CRulesForThing_003Ed__ = new _003CRulesForThing_003Ed__2(0);
			}
			_003CRulesForThing_003Ed__.prefix = _003C_003E3__prefix;
			_003CRulesForThing_003Ed__.thing = _003C_003E3__thing;
			return _003CRulesForThing_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CRulesForWorldObject_003Ed__7 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private WorldObject worldObject;

		public WorldObject _003C_003E3__worldObject;

		private bool addTags;

		public bool _003C_003E3__addTags;

		private string prefix;

		public string _003C_003E3__prefix;

		private _003C_003Ec__DisplayClass7_0 _003C_003E8__1;

		Rule IEnumerator<Rule>.Current
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
		public _003CRulesForWorldObject_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = default(_003C_003Ec__DisplayClass7_0);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1.worldObject = worldObject;
				_003C_003E8__1.addTags = addTags;
				if (!prefix.NullOrEmpty())
				{
					prefix += "_";
				}
				_003C_003E2__current = new Rule_String(prefix + "label", _003CRulesForWorldObject_003Eg__PossiblyWithTag_007C7_0(_003C_003E8__1.worldObject.Label, ref _003C_003E8__1));
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "definite", _003CRulesForWorldObject_003Eg__PossiblyWithTag_007C7_0(Find.ActiveLanguageWorker.WithDefiniteArticle(_003C_003E8__1.worldObject.Label, plural: false, _003C_003E8__1.worldObject.HasName), ref _003C_003E8__1));
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "indefinite", _003CRulesForWorldObject_003Eg__PossiblyWithTag_007C7_0(Find.ActiveLanguageWorker.WithIndefiniteArticle(_003C_003E8__1.worldObject.Label, plural: false, _003C_003E8__1.worldObject.HasName), ref _003C_003E8__1));
				_003C_003E1__state = 3;
				return true;
			case 3:
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
		IEnumerator<Rule> IEnumerable<Rule>.GetEnumerator()
		{
			_003CRulesForWorldObject_003Ed__7 _003CRulesForWorldObject_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRulesForWorldObject_003Ed__ = this;
			}
			else
			{
				_003CRulesForWorldObject_003Ed__ = new _003CRulesForWorldObject_003Ed__7(0);
			}
			_003CRulesForWorldObject_003Ed__.prefix = _003C_003E3__prefix;
			_003CRulesForWorldObject_003Ed__.worldObject = _003C_003E3__worldObject;
			_003CRulesForWorldObject_003Ed__.addTags = _003C_003E3__addTags;
			return _003CRulesForWorldObject_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	public static IEnumerable<Rule> RulesForPawn(string pawnSymbol, Pawn pawn, Dictionary<string, string> constants = null, bool addRelationInfoSymbol = true, bool addTags = true)
	{
		if (pawn == null)
		{
			Log.ErrorOnce("Tried to insert rule " + pawnSymbol + " for null pawn", 16015097);
			return Enumerable.Empty<Rule>();
		}
		TaggedString text = "";
		if (addRelationInfoSymbol)
		{
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, pawn);
		}
		return RulesForPawn(pawnSymbol, pawn.Name, pawn.story?.Title, pawn.kindDef, pawn.gender, pawn.Faction, pawn.ageTracker.AgeBiologicalYears, pawn.ageTracker.AgeChronologicalYears, text, PawnUtility.EverBeenColonistOrTameAnimal(pawn), PawnUtility.EverBeenQuestLodger(pawn), pawn.Faction != null && pawn.Faction.leader == pawn, pawn.royalty?.AllTitlesForReading, ModsConfig.AnomalyActive && pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest), pawn.LabelNoParenthesis, constants, addTags);
	}

	[IteratorStateMachine(typeof(_003CRulesForPawn_003Ed__1))]
	public static IEnumerable<Rule> RulesForPawn(string pawnSymbol, Name name, string title, PawnKindDef kind, Gender gender, Faction faction, int age, int chronologicalAge, string relationInfo, bool everBeenColonistOrTameAnimal, bool everBeenQuestLodger, bool isFactionLeader, List<RoyalTitle> royalTitles, bool cubeInterest, string labelNoParenthesis, Dictionary<string, string> constants = null, bool addTags = true)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRulesForPawn_003Ed__1(-2)
		{
			_003C_003E3__pawnSymbol = pawnSymbol,
			_003C_003E3__name = name,
			_003C_003E3__title = title,
			_003C_003E3__kind = kind,
			_003C_003E3__gender = gender,
			_003C_003E3__faction = faction,
			_003C_003E3__age = age,
			_003C_003E3__chronologicalAge = chronologicalAge,
			_003C_003E3__relationInfo = relationInfo,
			_003C_003E3__everBeenColonistOrTameAnimal = everBeenColonistOrTameAnimal,
			_003C_003E3__everBeenQuestLodger = everBeenQuestLodger,
			_003C_003E3__isFactionLeader = isFactionLeader,
			_003C_003E3__royalTitles = royalTitles,
			_003C_003E3__cubeInterest = cubeInterest,
			_003C_003E3__labelNoParenthesis = labelNoParenthesis,
			_003C_003E3__constants = constants,
			_003C_003E3__addTags = addTags
		};
	}

	[IteratorStateMachine(typeof(_003CRulesForThing_003Ed__2))]
	public static IEnumerable<Rule> RulesForThing(string prefix, Thing thing)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRulesForThing_003Ed__2(-2)
		{
			_003C_003E3__prefix = prefix,
			_003C_003E3__thing = thing
		};
	}

	[IteratorStateMachine(typeof(_003CRulesForDef_003Ed__3))]
	public static IEnumerable<Rule> RulesForDef(string prefix, Def def)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRulesForDef_003Ed__3(-2)
		{
			_003C_003E3__prefix = prefix,
			_003C_003E3__def = def
		};
	}

	[IteratorStateMachine(typeof(_003CRulesForBodyPartRecord_003Ed__4))]
	public static IEnumerable<Rule> RulesForBodyPartRecord(string prefix, BodyPartRecord part)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRulesForBodyPartRecord_003Ed__4(-2)
		{
			_003C_003E3__prefix = prefix,
			_003C_003E3__part = part
		};
	}

	[IteratorStateMachine(typeof(_003CRulesForHediffDef_003Ed__5))]
	public static IEnumerable<Rule> RulesForHediffDef(string prefix, HediffDef def, BodyPartRecord part)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRulesForHediffDef_003Ed__5(-2)
		{
			_003C_003E3__prefix = prefix,
			_003C_003E3__def = def,
			_003C_003E3__part = part
		};
	}

	[IteratorStateMachine(typeof(_003CRulesForFaction_003Ed__6))]
	public static IEnumerable<Rule> RulesForFaction(string prefix, Faction faction, Dictionary<string, string> constants = null, bool addTags = true)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRulesForFaction_003Ed__6(-2)
		{
			_003C_003E3__prefix = prefix,
			_003C_003E3__faction = faction,
			_003C_003E3__constants = constants,
			_003C_003E3__addTags = addTags
		};
	}

	[IteratorStateMachine(typeof(_003CRulesForWorldObject_003Ed__7))]
	public static IEnumerable<Rule> RulesForWorldObject(string prefix, WorldObject worldObject, bool addTags = true)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRulesForWorldObject_003Ed__7(-2)
		{
			_003C_003E3__prefix = prefix,
			_003C_003E3__worldObject = worldObject,
			_003C_003E3__addTags = addTags
		};
	}

	[IteratorStateMachine(typeof(_003CRulesForIdeo_003Ed__8))]
	public static IEnumerable<Rule> RulesForIdeo(string prefix, Ideo ideo)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRulesForIdeo_003Ed__8(-2)
		{
			_003C_003E3__prefix = prefix,
			_003C_003E3__ideo = ideo
		};
	}

	[IteratorStateMachine(typeof(_003CRulesForPrecept_003Ed__9))]
	public static IEnumerable<Rule> RulesForPrecept(string prefix, Precept precept)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRulesForPrecept_003Ed__9(-2)
		{
			_003C_003E3__prefix = prefix,
			_003C_003E3__precept = precept
		};
	}

	[CompilerGenerated]
	internal static string _003CRulesForWorldObject_003Eg__PossiblyWithTag_007C7_0(string str, ref _003C_003Ec__DisplayClass7_0 P_1)
	{
		if (!((P_1.worldObject.Faction != null) & P_1.addTags))
		{
			return str;
		}
		return str.ApplyTag(TagType.Settlement, P_1.worldObject.Faction.GetUniqueLoadID()).Resolve();
	}
}
