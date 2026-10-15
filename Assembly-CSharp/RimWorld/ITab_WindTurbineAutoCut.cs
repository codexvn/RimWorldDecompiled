using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class ITab_WindTurbineAutoCut : ITab
{
	[CompilerGenerated]
	private sealed class _003CHiddenSpecialThingFilters_003Ed__14 : IEnumerable<SpecialThingFilterDef>, IEnumerable, IEnumerator<SpecialThingFilterDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private SpecialThingFilterDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		SpecialThingFilterDef IEnumerator<SpecialThingFilterDef>.Current
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
		public _003CHiddenSpecialThingFilters_003Ed__14(int _003C_003E1__state)
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
				_003C_003E2__current = SpecialThingFilterDefOf.AllowFresh;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (ModsConfig.IdeologyActive)
				{
					_003C_003E2__current = SpecialThingFilterDefOf.AllowVegetarian;
					_003C_003E1__state = 2;
					return true;
				}
				break;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = SpecialThingFilterDefOf.AllowCarnivore;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = SpecialThingFilterDefOf.AllowCannibal;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = SpecialThingFilterDefOf.AllowInsectMeat;
				_003C_003E1__state = 5;
				return true;
			case 5:
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
		IEnumerator<SpecialThingFilterDef> IEnumerable<SpecialThingFilterDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CHiddenSpecialThingFilters_003Ed__14(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<SpecialThingFilterDef>)this).GetEnumerator();
		}
	}

	private static readonly Vector2 WinSize = new Vector2(300f, 480f);

	private const int CutNowButtonWidth = 150;

	private const int CutNowButtonHeight = 27;

	private const float AutoCutRowHeight = 24f;

	private ThingFilterUI.UIState plantFilterState = new ThingFilterUI.UIState();

	public CompAutoCut AutoCut => base.SelThing?.TryGetComp<CompAutoCut>();

	public override bool IsVisible => AutoCut != null;

	public ITab_WindTurbineAutoCut()
	{
		size = WinSize;
		labelKey = "TabWindTurbineAutoCut";
	}

	public override void OnOpen()
	{
		base.OnOpen();
		plantFilterState.quickSearch.Reset();
	}

	protected override void FillTab()
	{
		CompAutoCut autoCut = AutoCut;
		Rect rect = new Rect(0f, 0f, WinSize.x, WinSize.y).ContractedBy(10f);
		Widgets.BeginGroup(rect);
		float curY = 0f;
		DrawAutoCutOptions(ref curY, rect.width, autoCut);
		curY += 4f;
		DrawPlantFilter(ref curY, rect.width, rect.height - curY, autoCut);
		Widgets.EndGroup();
	}

	private void DrawPlantFilter(ref float curY, float width, float height, CompAutoCut autoCut)
	{
		ThingFilterUI.DoThingFilterConfigWindow(new Rect(0f, curY, width, height), plantFilterState, autoCut.AutoCutFilter, autoCut.GetFixedAutoCutFilter(), 1, null, map: autoCut.parent.Map, forceHiddenFilters: HiddenSpecialThingFilters(), forceHideHitPointsConfig: true);
	}

	private void DrawAutoCutOptions(ref float curY, float width, CompAutoCut autoCut)
	{
		Designator_PlantsCut designator_PlantsCut = Find.ReverseDesignatorDatabase.Get<Designator_PlantsCut>();
		Rect position = new Rect(0f, curY, 24f, 24f);
		Rect rect = new Rect(position.xMax + 4f, curY, width, 24f);
		Rect rect2 = new Rect(0f, rect.yMax + 4f, 150f, 27f);
		GUI.DrawTexture(position, designator_PlantsCut.icon);
		Text.Font = GameFont.Tiny;
		Widgets.CheckboxLabeled(rect, "WindTurbineAutoCut_EnabledCheckbox".Translate(), ref autoCut.autoCut, disabled: false, null, null, placeCheckboxNearText: true);
		Text.Font = GameFont.Small;
		if (Widgets.ButtonText(rect2, "AutoCutNow".Translate(), drawBackground: true, doMouseoverSound: true, active: true, null))
		{
			autoCut.DesignatePlantsToCut();
			designator_PlantsCut.soundSucceeded?.PlayOneShotOnCamera();
		}
		curY = rect2.yMax;
	}

	[IteratorStateMachine(typeof(_003CHiddenSpecialThingFilters_003Ed__14))]
	private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CHiddenSpecialThingFilters_003Ed__14(-2);
	}
}
