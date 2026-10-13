using System;

namespace Verse
{
	// Token: 0x02000CA0 RID: 3232
	public static class DebugSettings
	{
		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x060052F4 RID: 21236 RVA: 0x001ABC2A File Offset: 0x001A9E2A
		public static bool ShowDevGizmos
		{
			get
			{
				return Prefs.DevMode && DebugSettings.godMode;
			}
		}

		// Token: 0x04003B3D RID: 15165
		public const bool DebugBuild = false;

		// Token: 0x04003B3E RID: 15166
		public static bool enableDamage = true;

		// Token: 0x04003B3F RID: 15167
		public static bool enablePlayerDamage = true;

		// Token: 0x04003B40 RID: 15168
		public static bool enableRandomMentalStates = true;

		// Token: 0x04003B41 RID: 15169
		public static bool enableStoryteller = true;

		// Token: 0x04003B42 RID: 15170
		public static bool enableRandomDiseases = true;

		// Token: 0x04003B43 RID: 15171
		public static bool enableTranslationWindowInEnglish = false;

		// Token: 0x04003B44 RID: 15172
		public static bool godMode = false;

		// Token: 0x04003B45 RID: 15173
		public static bool devPalette = false;

		// Token: 0x04003B46 RID: 15174
		public static bool pauseOnError = false;

		// Token: 0x04003B47 RID: 15175
		public static bool noAnimals = false;

		// Token: 0x04003B48 RID: 15176
		public static bool unlimitedPower = false;

		// Token: 0x04003B49 RID: 15177
		public static bool pathThroughWalls = false;

		// Token: 0x04003B4A RID: 15178
		public static bool instantRecruit = false;

		// Token: 0x04003B4B RID: 15179
		public static bool alwaysSocialFight = false;

		// Token: 0x04003B4C RID: 15180
		public static bool alwaysDoLovin = false;

		// Token: 0x04003B4D RID: 15181
		public static bool detectRegionListersBugs = false;

		// Token: 0x04003B4E RID: 15182
		public static bool instantVisitorsGift = false;

		// Token: 0x04003B4F RID: 15183
		public static bool lowFPS = false;

		// Token: 0x04003B50 RID: 15184
		public static bool allowUndraftedMechOrders = false;

		// Token: 0x04003B51 RID: 15185
		public static bool editableGlowerColors = false;

		// Token: 0x04003B52 RID: 15186
		public static bool showHiddenPawns = false;

		// Token: 0x04003B53 RID: 15187
		public static bool showHiddenInfo = false;

		// Token: 0x04003B54 RID: 15188
		public static bool anomalyDarkeningFX = true;

		// Token: 0x04003B55 RID: 15189
		public static bool fastResearch = false;

		// Token: 0x04003B56 RID: 15190
		public static bool fastLearning = false;

		// Token: 0x04003B57 RID: 15191
		public static bool fastEcology = false;

		// Token: 0x04003B58 RID: 15192
		public static bool fastEcologyRegrowRateOnly = false;

		// Token: 0x04003B59 RID: 15193
		public static bool fastCrafting = false;

		// Token: 0x04003B5A RID: 15194
		public static bool fastCaravans = false;

		// Token: 0x04003B5B RID: 15195
		public static bool fastMapUnpollution = false;

		// Token: 0x04003B5C RID: 15196
		public static bool activateAllBuildingDemands = false;

		// Token: 0x04003B5D RID: 15197
		public static bool activateAllIdeoRoles = false;

		// Token: 0x04003B5E RID: 15198
		public static bool showLocomotionUrgency = false;

		// Token: 0x04003B5F RID: 15199
		public static bool playRitualAmbience = true;

		// Token: 0x04003B60 RID: 15200
		public static bool simulateUsingSteamDeck = false;

		// Token: 0x04003B61 RID: 15201
		public static bool logRaidInfo = false;

		// Token: 0x04003B62 RID: 15202
		public static bool logTranslationLookupErrors = false;

		// Token: 0x04003B63 RID: 15203
		public static bool logPsychicRitualTransitions = false;

		// Token: 0x04003B64 RID: 15204
		public static bool fastMonolithRespawn = false;

		// Token: 0x04003B65 RID: 15205
		public static bool searchIgnoresRestrictions = false;

		// Token: 0x04003B66 RID: 15206
		public static bool alwaysRareCatches = false;

		// Token: 0x04003B67 RID: 15207
		public static bool alwaysNegativeCatches = false;

		// Token: 0x04003B68 RID: 15208
		public static bool logMismatchedLayoutFactions = false;

		// Token: 0x04003B69 RID: 15209
		public static bool loopGravshipCutscene = false;

		// Token: 0x04003B6A RID: 15210
		public static bool skipGravshipTileSelection = false;

		// Token: 0x04003B6B RID: 15211
		public static bool ignoreGravshipRange = false;
	}
}
