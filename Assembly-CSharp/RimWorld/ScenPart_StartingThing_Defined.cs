using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ScenPart_StartingThing_Defined : ScenPart_ThingCount
{
	[CompilerGenerated]
	private sealed class _003CGetSummaryListEntries_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private string tag;

		public string _003C_003E3__tag;

		public ScenPart_StartingThing_Defined _003C_003E4__this;

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
		public _003CGetSummaryListEntries_003Ed__4(int _003C_003E1__state)
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
			ScenPart_StartingThing_Defined scenPart_StartingThing_Defined = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (tag == "PlayerStartsWith")
				{
					_003C_003E2__current = GenLabel.ThingLabel(scenPart_StartingThing_Defined.thingDef, scenPart_StartingThing_Defined.stuff, scenPart_StartingThing_Defined.count).CapitalizeFirst();
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetSummaryListEntries_003Ed__4 _003CGetSummaryListEntries_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetSummaryListEntries_003Ed__ = this;
			}
			else
			{
				_003CGetSummaryListEntries_003Ed__ = new _003CGetSummaryListEntries_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetSummaryListEntries_003Ed__.tag = _003C_003E3__tag;
			return _003CGetSummaryListEntries_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPlayerStartingThings_003Ed__5 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ScenPart_StartingThing_Defined _003C_003E4__this;

		private int _003Ci_003E5__2;

		Thing IEnumerator<Thing>.Current
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
		public _003CPlayerStartingThings_003Ed__5(int _003C_003E1__state)
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
			ScenPart_StartingThing_Defined scenPart_StartingThing_Defined = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (scenPart_StartingThing_Defined.thingDef.stackLimit == 1)
				{
					_003Ci_003E5__2 = 0;
					goto IL_006d;
				}
				_003C_003E2__current = scenPart_StartingThing_Defined.GenerateThing(scenPart_StartingThing_Defined.count);
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_006d;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_006d:
				if (_003Ci_003E5__2 < scenPart_StartingThing_Defined.count)
				{
					_003C_003E2__current = scenPart_StartingThing_Defined.GenerateThing(1);
					_003C_003E1__state = 1;
					return true;
				}
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CPlayerStartingThings_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CPlayerStartingThings_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	public const string PlayerStartWithTag = "PlayerStartsWith";

	public static string PlayerStartWithIntro => "ScenPart_StartWith".Translate();

	public override string Summary(Scenario scen)
	{
		return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", PlayerStartWithIntro);
	}

	[IteratorStateMachine(typeof(_003CGetSummaryListEntries_003Ed__4))]
	public override IEnumerable<string> GetSummaryListEntries(string tag)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetSummaryListEntries_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__tag = tag
		};
	}

	[IteratorStateMachine(typeof(_003CPlayerStartingThings_003Ed__5))]
	public override IEnumerable<Thing> PlayerStartingThings()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPlayerStartingThings_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Thing GenerateThing(int num)
	{
		Thing thing = ThingMaker.MakeThing(thingDef, stuff);
		if (thing.TryGetComp(out CompQuality comp))
		{
			comp.SetQuality(quality ?? QualityCategory.Normal, ArtGenerationContext.Outsider);
		}
		if (thingDef.Minifiable)
		{
			thing = thing.MakeMinified();
		}
		if (thingDef.IsIngestible && thingDef.ingestible.IsMeal)
		{
			FoodUtility.GenerateGoodIngredients(thing, Faction.OfPlayer.ideos.PrimaryIdeo);
		}
		thing.stackCount = num;
		return thing;
	}
}
