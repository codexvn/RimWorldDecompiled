using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class CompStyleable : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__14 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompStyleable _003C_003E4__this;

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
		public _003CSpecialDisplayStats_003Ed__14(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
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
				CompStyleable compStyleable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (compStyleable.StyleCategoryDef != null)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Stat_Thing_StyleLabel".Translate(), compStyleable.StyleCategoryDef.LabelCap, "Stat_Thing_StyleDesc".Translate(), 1108);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0092;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0092;
				case 2:
					{
						_003C_003E1__state = -3;
						goto IL_00e6;
					}
					IL_00e6:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					break;
					IL_0092:
					if (compStyleable.SourcePrecept == null)
					{
						break;
					}
					_003C_003E7__wrap1 = compStyleable.SourcePrecept.SpecialDisplayStats(compStyleable.parent).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00e6;
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
			_003CSpecialDisplayStats_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__14(0)
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

	private Precept_ThingStyle sourcePrecept;

	public ThingStyleDef styleDef;

	public bool everSeenByPlayer;

	[Unsaved(false)]
	public StyleCategoryDef cachedStyleCategoryDef;

	public Ideo Ideo => SourcePrecept?.ideo;

	public StyleCategoryDef StyleCategoryDef
	{
		get
		{
			if (cachedStyleCategoryDef == null)
			{
				bool flag = false;
				foreach (StyleCategoryDef allDef in DefDatabase<StyleCategoryDef>.AllDefs)
				{
					for (int i = 0; i < allDef.thingDefStyles.Count; i++)
					{
						if (allDef.thingDefStyles[i].StyleDef == styleDef && allDef.thingDefStyles[i].ThingDef == parent.def)
						{
							cachedStyleCategoryDef = allDef;
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
			return cachedStyleCategoryDef;
		}
	}

	public Precept_ThingStyle SourcePrecept
	{
		get
		{
			return sourcePrecept;
		}
		set
		{
			if (sourcePrecept == value)
			{
				return;
			}
			sourcePrecept = value;
			if (!(parent.def.randomStyleChance > 0f))
			{
				BuildableDef buildableDef = (parent.def.IsBlueprint ? parent.def.entityDefToBuild : parent.def);
				StyleCategoryPair styleCategoryPair = sourcePrecept?.ideo.style.StyleForThingDef((ThingDef)buildableDef, sourcePrecept);
				cachedStyleCategoryDef = styleCategoryPair?.category;
				if (cachedStyleCategoryDef != null)
				{
					styleDef = styleCategoryPair.styleDef;
				}
			}
		}
	}

	public override string TransformLabel(string label)
	{
		if (sourcePrecept != null)
		{
			label = sourcePrecept.TransformThingLabel(label);
		}
		return label;
	}

	public override string CompInspectStringExtra()
	{
		string text = base.CompInspectStringExtra();
		if (sourcePrecept == null)
		{
			return text;
		}
		string text2 = sourcePrecept.InspectStringExtra(parent);
		if (text2.NullOrEmpty())
		{
			return text;
		}
		if (!text.NullOrEmpty())
		{
			text += "\n";
		}
		return text + text2;
	}

	public override void PostPostGeneratedForTrader(TraderKindDef trader, PlanetTile forTile, Faction forFaction)
	{
		StyleCategoryPair styleCategoryPair = forFaction?.ideos?.PrimaryIdeo?.style?.StyleForThingDef(parent.def);
		if (styleCategoryPair != null)
		{
			parent.SetStyleDef(styleCategoryPair.styleDef);
		}
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__14))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__14(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		if (Scribe.mode == LoadSaveMode.Saving && sourcePrecept != null && (sourcePrecept.ideo == null || !Find.IdeoManager.IdeosListForReading.Contains(sourcePrecept.ideo)))
		{
			sourcePrecept = null;
		}
		Scribe_References.Look(ref sourcePrecept, "sourcePrecept");
		Scribe_Defs.Look(ref styleDef, "styleDef");
		Scribe_Values.Look(ref everSeenByPlayer, "everSeenByPlayer", defaultValue: false);
	}
}
