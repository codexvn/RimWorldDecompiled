using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020033D8 RID: 13272
	public class ITab_WindTurbineAutoCut : ITab
	{
		// Token: 0x170030CF RID: 12495
		// (get) Token: 0x06012A5A RID: 76378 RVA: 0x005848D4 File Offset: 0x00582AD4
		public CompAutoCut AutoCut
		{
			get
			{
				Thing selThing = base.SelThing;
				if (selThing == null)
				{
					return null;
				}
				return selThing.TryGetComp<CompAutoCut>();
			}
		}

		// Token: 0x170030D0 RID: 12496
		// (get) Token: 0x06012A5B RID: 76379 RVA: 0x005848E7 File Offset: 0x00582AE7
		public override bool IsVisible
		{
			get
			{
				return this.AutoCut != null;
			}
		}

		// Token: 0x06012A5C RID: 76380 RVA: 0x005848F2 File Offset: 0x00582AF2
		public ITab_WindTurbineAutoCut()
		{
			this.size = ITab_WindTurbineAutoCut.WinSize;
			this.labelKey = "TabWindTurbineAutoCut";
		}

		// Token: 0x06012A5D RID: 76381 RVA: 0x0058491B File Offset: 0x00582B1B
		public override void OnOpen()
		{
			base.OnOpen();
			this.plantFilterState.quickSearch.Reset();
		}

		// Token: 0x06012A5E RID: 76382 RVA: 0x00584934 File Offset: 0x00582B34
		protected override void FillTab()
		{
			CompAutoCut autoCut = this.AutoCut;
			Rect rect = new Rect(0f, 0f, ITab_WindTurbineAutoCut.WinSize.x, ITab_WindTurbineAutoCut.WinSize.y).ContractedBy(10f);
			Widgets.BeginGroup(rect);
			float num = 0f;
			this.DrawAutoCutOptions(ref num, rect.width, autoCut);
			num += 4f;
			this.DrawPlantFilter(ref num, rect.width, rect.height - num, autoCut);
			Widgets.EndGroup();
		}

		// Token: 0x06012A5F RID: 76383 RVA: 0x005849B8 File Offset: 0x00582BB8
		private void DrawPlantFilter(ref float curY, float width, float height, CompAutoCut autoCut)
		{
			Rect rect = new Rect(0f, curY, width, height);
			ThingFilterUI.UIState state = this.plantFilterState;
			ThingFilter autoCutFilter = autoCut.AutoCutFilter;
			ThingFilter fixedAutoCutFilter = autoCut.GetFixedAutoCutFilter();
			int openMask = 1;
			IEnumerable<ThingDef> forceHiddenDefs = null;
			Map map = autoCut.parent.Map;
			ThingFilterUI.DoThingFilterConfigWindow(rect, state, autoCutFilter, fixedAutoCutFilter, openMask, forceHiddenDefs, this.HiddenSpecialThingFilters(), true, false, false, null, map);
		}

		// Token: 0x06012A60 RID: 76384 RVA: 0x00584A08 File Offset: 0x00582C08
		private void DrawAutoCutOptions(ref float curY, float width, CompAutoCut autoCut)
		{
			Designator_PlantsCut designator_PlantsCut = Find.ReverseDesignatorDatabase.Get<Designator_PlantsCut>();
			Rect position = new Rect(0f, curY, 24f, 24f);
			Rect rect = new Rect(position.xMax + 4f, curY, width, 24f);
			Rect rect2 = new Rect(0f, rect.yMax + 4f, 150f, 27f);
			GUI.DrawTexture(position, designator_PlantsCut.icon);
			Text.Font = GameFont.Tiny;
			Widgets.CheckboxLabeled(rect, "WindTurbineAutoCut_EnabledCheckbox".Translate(), ref autoCut.autoCut, false, null, null, true, false);
			Text.Font = GameFont.Small;
			if (Widgets.ButtonText(rect2, "AutoCutNow".Translate(), true, true, true, null))
			{
				autoCut.DesignatePlantsToCut();
				SoundDef soundSucceeded = designator_PlantsCut.soundSucceeded;
				if (soundSucceeded != null)
				{
					soundSucceeded.PlayOneShotOnCamera(null);
				}
			}
			curY = rect2.yMax;
		}

		// Token: 0x06012A61 RID: 76385 RVA: 0x00584AF4 File Offset: 0x00582CF4
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			yield return SpecialThingFilterDefOf.AllowFresh;
			if (ModsConfig.IdeologyActive)
			{
				yield return SpecialThingFilterDefOf.AllowVegetarian;
				yield return SpecialThingFilterDefOf.AllowCarnivore;
				yield return SpecialThingFilterDefOf.AllowCannibal;
				yield return SpecialThingFilterDefOf.AllowInsectMeat;
			}
			yield break;
		}

		// Token: 0x0400C1A0 RID: 49568
		private static readonly Vector2 WinSize = new Vector2(300f, 480f);

		// Token: 0x0400C1A1 RID: 49569
		private const int CutNowButtonWidth = 150;

		// Token: 0x0400C1A2 RID: 49570
		private const int CutNowButtonHeight = 27;

		// Token: 0x0400C1A3 RID: 49571
		private const float AutoCutRowHeight = 24f;

		// Token: 0x0400C1A4 RID: 49572
		private ThingFilterUI.UIState plantFilterState = new ThingFilterUI.UIState();
	}
}
