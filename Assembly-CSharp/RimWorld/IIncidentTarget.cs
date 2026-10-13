using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D58 RID: 7512
	public interface IIncidentTarget : ILoadReferenceable
	{
		// Token: 0x17001A02 RID: 6658
		// (get) Token: 0x0600A861 RID: 43105
		PlanetTile Tile { get; }

		// Token: 0x17001A03 RID: 6659
		// (get) Token: 0x0600A862 RID: 43106
		StoryState StoryState { get; }

		// Token: 0x17001A04 RID: 6660
		// (get) Token: 0x0600A863 RID: 43107
		GameConditionManager GameConditionManager { get; }

		// Token: 0x17001A05 RID: 6661
		// (get) Token: 0x0600A864 RID: 43108
		float PlayerWealthForStoryteller { get; }

		// Token: 0x17001A06 RID: 6662
		// (get) Token: 0x0600A865 RID: 43109
		IEnumerable<Pawn> PlayerPawnsForStoryteller { get; }

		// Token: 0x17001A07 RID: 6663
		// (get) Token: 0x0600A866 RID: 43110
		FloatRange IncidentPointsRandomFactorRange { get; }

		// Token: 0x17001A08 RID: 6664
		// (get) Token: 0x0600A867 RID: 43111
		int ConstantRandSeed { get; }

		// Token: 0x0600A868 RID: 43112
		IEnumerable<IncidentTargetTagDef> IncidentTargetTags();
	}
}
