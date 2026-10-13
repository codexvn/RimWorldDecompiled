using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020033C6 RID: 13254
	public class ITab_PenAutoCut : ITab_PenBase
	{
		// Token: 0x060129F5 RID: 76277 RVA: 0x0058299C File Offset: 0x00580B9C
		public ITab_PenAutoCut()
		{
			this.size = ITab_PenAutoCut.WinSize;
			this.labelKey = "TabPenAutoCut";
		}

		// Token: 0x060129F6 RID: 76278 RVA: 0x005829C5 File Offset: 0x00580BC5
		public override void OnOpen()
		{
			base.OnOpen();
			this.plantFilterState.quickSearch.Reset();
		}

		// Token: 0x060129F7 RID: 76279 RVA: 0x005829E0 File Offset: 0x00580BE0
		protected override void FillTab()
		{
			CompAnimalPenMarker selectedCompAnimalPenMarker = base.SelectedCompAnimalPenMarker;
			Rect rect = new Rect(0f, 0f, ITab_PenAutoCut.WinSize.x, ITab_PenAutoCut.WinSize.y).ContractedBy(10f);
			Widgets.BeginGroup(rect);
			float num = 0f;
			this.DrawAutoCutOptions(ref num, rect.width, selectedCompAnimalPenMarker);
			num += 4f;
			this.DrawPlantFilter(ref num, rect.width, rect.height - num, selectedCompAnimalPenMarker);
			Widgets.EndGroup();
		}

		// Token: 0x060129F8 RID: 76280 RVA: 0x00582A64 File Offset: 0x00580C64
		private void DrawPlantFilter(ref float curY, float width, float height, CompAnimalPenMarker marker)
		{
			Rect rect = new Rect(0f, curY, width, height);
			ThingFilterUI.UIState state = this.plantFilterState;
			ThingFilter autoCutFilter = marker.AutoCutFilter;
			ThingFilter fixedAutoCutFilter = marker.parent.Map.animalPenManager.GetFixedAutoCutFilter();
			int openMask = 1;
			IEnumerable<ThingDef> forceHiddenDefs = null;
			Map map = marker.parent.Map;
			ThingFilterUI.DoThingFilterConfigWindow(rect, state, autoCutFilter, fixedAutoCutFilter, openMask, forceHiddenDefs, this.HiddenSpecialThingFilters(), true, false, false, null, map);
		}

		// Token: 0x060129F9 RID: 76281 RVA: 0x00582AC4 File Offset: 0x00580CC4
		private void DrawAutoCutOptions(ref float curY, float width, CompAnimalPenMarker marker)
		{
			Text.Font = GameFont.Small;
			bool enclosed = marker.PenState.Enclosed;
			Designator_PlantsCut designator_PlantsCut = Find.ReverseDesignatorDatabase.Get<Designator_PlantsCut>();
			Rect position = new Rect(0f, curY, 24f, 24f);
			Rect rect = new Rect(position.xMax + 4f, curY, width, 24f);
			Rect rect2 = new Rect(0f, rect.yMax + 4f, 150f, 27f);
			Rect rect3 = new Rect(0f, curY, width - 18f, 55f);
			Widgets.DrawHighlightIfMouseover(rect3);
			if (!enclosed)
			{
				GUI.color = Widgets.InactiveColor;
			}
			GUI.DrawTexture(position, designator_PlantsCut.icon);
			Widgets.CheckboxLabeled(rect, "PenAutoCut_EnabledCheckbox".Translate(), ref marker.autoCut, false, null, null, true, false);
			GUI.color = Color.white;
			if (enclosed)
			{
				if (Widgets.ButtonText(rect2, "AutoCutNow".Translate(), true, true, true, null))
				{
					marker.DesignatePlantsToCut();
					SoundDef soundSucceeded = designator_PlantsCut.soundSucceeded;
					if (soundSucceeded != null)
					{
						soundSucceeded.PlayOneShotOnCamera(null);
					}
				}
			}
			else
			{
				GUI.color = ColorLibrary.RedReadable;
				Text.Font = GameFont.Tiny;
				Widgets.Label(rect2, "AutocutUnenclosedPen".Translate());
				Text.Font = GameFont.Small;
				GUI.color = Color.white;
			}
			if (Mouse.IsOver(rect3))
			{
				TaggedString tooltip = "PenAutoCut_EnabledCheckboxTip".Translate();
				if (!enclosed)
				{
					tooltip += "\n\n" + "AutocutUnenclosedPenTip".Translate().Colorize(ColorLibrary.RedReadable);
				}
				TooltipHandler.TipRegion(rect, () => tooltip.Resolve(), 19727181);
			}
			curY = rect2.yMax;
		}

		// Token: 0x060129FA RID: 76282 RVA: 0x00582C92 File Offset: 0x00580E92
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

		// Token: 0x0400C168 RID: 49512
		private static readonly Vector2 WinSize = new Vector2(300f, 480f);

		// Token: 0x0400C169 RID: 49513
		private const float AutoCutRowHeight = 24f;

		// Token: 0x0400C16A RID: 49514
		private const int CutNowButtonWidth = 150;

		// Token: 0x0400C16B RID: 49515
		private const int CutNowButtonHeight = 27;

		// Token: 0x0400C16C RID: 49516
		private ThingFilterUI.UIState plantFilterState = new ThingFilterUI.UIState();
	}
}
