using System;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020033F1 RID: 13297
	public class MainButtonWorker_ToggleWorld : MainButtonWorker
	{
		// Token: 0x06012B2C RID: 76588 RVA: 0x0058925C File Offset: 0x0058745C
		public override void Activate()
		{
			if (Find.World.renderer.wantedMode != WorldRenderMode.Planet)
			{
				AmbientSoundManager.EnsureWorldAmbientSoundCreated();
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				if (this.resetViewNextTime)
				{
					this.resetViewNextTime = false;
					Find.World.UI.Reset();
				}
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.FormCaravan, OpportunityType.Important);
				Find.MainTabsRoot.EscapeCurrentTab(false);
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				return;
			}
			if (Find.MainTabsRoot.OpenTab != null && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Inspect)
			{
				Find.MainTabsRoot.EscapeCurrentTab(false);
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				return;
			}
			Find.World.renderer.wantedMode = WorldRenderMode.None;
			SoundDefOf.TabClose.PlayOneShotOnCamera(null);
		}

		// Token: 0x0400C22C RID: 49708
		public bool resetViewNextTime = true;
	}
}
