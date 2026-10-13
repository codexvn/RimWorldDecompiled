using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020033C4 RID: 13252
	public abstract class ITab_PenBase : ITab
	{
		// Token: 0x170030B1 RID: 12465
		// (get) Token: 0x060129EE RID: 76270 RVA: 0x005828A3 File Offset: 0x00580AA3
		public CompAnimalPenMarker SelectedCompAnimalPenMarker
		{
			get
			{
				Thing selThing = base.SelThing;
				if (selThing == null)
				{
					return null;
				}
				return selThing.TryGetComp<CompAnimalPenMarker>();
			}
		}

		// Token: 0x170030B2 RID: 12466
		// (get) Token: 0x060129EF RID: 76271 RVA: 0x005828B6 File Offset: 0x00580AB6
		public override bool IsVisible
		{
			get
			{
				return this.SelectedCompAnimalPenMarker != null;
			}
		}
	}
}
