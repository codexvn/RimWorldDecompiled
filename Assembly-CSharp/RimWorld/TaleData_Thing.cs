using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class TaleData_Thing : TaleData
{
	[CompilerGenerated]
	private sealed class _003CGetRules_003Ed__6 : IEnumerable<Rule>, IEnumerable, IEnumerator<Rule>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Rule _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string prefix;

		public string _003C_003E3__prefix;

		public TaleData_Thing _003C_003E4__this;

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
		public _003CGetRules_003Ed__6(int _003C_003E1__state)
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
			TaleData_Thing taleData_Thing = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "_label", taleData_Thing.thingDef.label);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(taleData_Thing.thingDef.label));
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(taleData_Thing.thingDef.label));
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				if (taleData_Thing.stuff != null)
				{
					_003C_003E2__current = new Rule_String(prefix + "_stuffLabel", taleData_Thing.stuff.label);
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_0131;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0131;
			case 5:
				_003C_003E1__state = -1;
				goto IL_016a;
			case 6:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0131:
				if (taleData_Thing.title != null)
				{
					_003C_003E2__current = new Rule_String(prefix + "_title", taleData_Thing.title);
					_003C_003E1__state = 5;
					return true;
				}
				goto IL_016a;
				IL_016a:
				_003C_003E2__current = new Rule_String(prefix + "_quality", taleData_Thing.quality.GetLabel());
				_003C_003E1__state = 6;
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
			_003CGetRules_003Ed__6 _003CGetRules_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetRules_003Ed__ = this;
			}
			else
			{
				_003CGetRules_003Ed__ = new _003CGetRules_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetRules_003Ed__.prefix = _003C_003E3__prefix;
			return _003CGetRules_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Rule>)this).GetEnumerator();
		}
	}

	public int thingID;

	public ThingDef thingDef;

	public ThingDef stuff;

	public string title;

	public QualityCategory quality;

	public override void ExposeData()
	{
		Scribe_Values.Look(ref thingID, "thingID", 0);
		Scribe_Defs.Look(ref thingDef, "thingDef");
		Scribe_Defs.Look(ref stuff, "stuff");
		Scribe_Values.Look(ref title, "title");
		Scribe_Values.Look(ref quality, "quality", QualityCategory.Awful);
	}

	[IteratorStateMachine(typeof(_003CGetRules_003Ed__6))]
	public override IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRules_003Ed__6(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__prefix = prefix
		};
	}

	public static TaleData_Thing GenerateFrom(Thing t)
	{
		TaleData_Thing taleData_Thing = new TaleData_Thing();
		taleData_Thing.thingID = t.thingIDNumber;
		taleData_Thing.thingDef = t.def;
		taleData_Thing.stuff = t.Stuff;
		t.TryGetQuality(out taleData_Thing.quality);
		CompArt compArt = t.TryGetComp<CompArt>();
		if (compArt != null && compArt.Active)
		{
			taleData_Thing.title = compArt.Title;
		}
		if (t is Book book)
		{
			taleData_Thing.title = book.Title;
		}
		return taleData_Thing;
	}

	public static TaleData_Thing GenerateRandom()
	{
		ThingDef obj = DefDatabase<ThingDef>.AllDefs.Where((ThingDef d) => d.comps != null && d.comps.Any((CompProperties cp) => cp.compClass == typeof(CompArt))).RandomElement();
		ThingDef thingDef = GenStuff.RandomStuffFor(obj);
		Thing thing = ThingMaker.MakeThing(obj, thingDef);
		ArtGenerationContext artGenerationContext = ((Rand.Value < 0.5f) ? ArtGenerationContext.Colony : ArtGenerationContext.Outsider);
		CompQuality compQuality = thing.TryGetComp<CompQuality>();
		if (compQuality != null && (int)compQuality.Quality < (int)thing.TryGetComp<CompArt>().Props.minQualityForArtistic)
		{
			compQuality.SetQuality(thing.TryGetComp<CompArt>().Props.minQualityForArtistic, artGenerationContext);
		}
		thing.TryGetComp<CompArt>().InitializeArt(artGenerationContext);
		return GenerateFrom(thing);
	}
}
