using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033C5 RID: 13253
	public class ITab_PenAnimals : ITab_PenBase
	{
		// Token: 0x060129F1 RID: 76273 RVA: 0x005828C9 File Offset: 0x00580AC9
		public ITab_PenAnimals()
		{
			this.size = ITab_PenAnimals.WinSize;
			this.labelKey = "TabPenAnimals";
		}

		// Token: 0x060129F2 RID: 76274 RVA: 0x005828F2 File Offset: 0x00580AF2
		public override void OnOpen()
		{
			base.OnOpen();
			this.animalFilterState.quickSearch.Reset();
		}

		// Token: 0x060129F3 RID: 76275 RVA: 0x0058290C File Offset: 0x00580B0C
		protected override void FillTab()
		{
			CompAnimalPenMarker selectedCompAnimalPenMarker = base.SelectedCompAnimalPenMarker;
			Map map = selectedCompAnimalPenMarker.parent.Map;
			Rect rect;
			Rect rect2;
			new Rect(0f, 0f, ITab_PenAnimals.WinSize.x, ITab_PenAnimals.WinSize.y).ContractedBy(10f).SplitHorizontally(18f, out rect, out rect2);
			ThingFilterUI.DoThingFilterConfigWindow(rect2, this.animalFilterState, selectedCompAnimalPenMarker.AnimalFilter, AnimalPenUtility.GetFixedAnimalFilter(), 1, null, null, false, false, false, null, map);
		}

		// Token: 0x0400C166 RID: 49510
		private static readonly Vector2 WinSize = new Vector2(300f, 480f);

		// Token: 0x0400C167 RID: 49511
		private ThingFilterUI.UIState animalFilterState = new ThingFilterUI.UIState();
	}
}
