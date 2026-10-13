using System;
using RimWorld.Planet;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200006D RID: 109
	[AttributeUsage(AttributeTargets.Method)]
	public class DebugActionAttribute : Attribute
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000D474 File Offset: 0x0000B674
		public bool IsAllowedInCurrentGameState
		{
			get
			{
				bool flag = (this.allowedGameStates & AllowedGameStates.Entry) == AllowedGameStates.Invalid || Current.ProgramState == ProgramState.Entry;
				bool flag2 = (this.allowedGameStates & AllowedGameStates.Playing) == AllowedGameStates.Invalid || Current.ProgramState == ProgramState.Playing;
				bool flag3 = (this.allowedGameStates & AllowedGameStates.WorldRenderedNow) == AllowedGameStates.Invalid || WorldRendererUtility.WorldSelected;
				bool flag4 = (this.allowedGameStates & AllowedGameStates.IsCurrentlyOnMap) == AllowedGameStates.Invalid || (!WorldRendererUtility.WorldSelected && Find.CurrentMap != null);
				bool flag5 = (this.allowedGameStates & AllowedGameStates.HasGameCondition) == AllowedGameStates.Invalid || (!WorldRendererUtility.WorldSelected && Find.CurrentMap != null && Find.CurrentMap.gameConditionManager.ActiveConditions.Count > 0);
				bool flag6 = (!this.requiresRoyalty || ModsConfig.RoyaltyActive) && (!this.requiresIdeology || ModsConfig.IdeologyActive) && (!this.requiresBiotech || ModsConfig.BiotechActive) && (!this.requiresAnomaly || ModsConfig.AnomalyActive) && (!this.requiresOdyssey || ModsConfig.OdysseyActive);
				return flag && flag2 && flag3 && flag4 && flag5 && flag6;
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000D574 File Offset: 0x0000B774
		public DebugActionAttribute(string category = null, string name = null, bool requiresRoyalty = false, bool requiresIdeology = false, bool requiresBiotech = false, bool requiresAnomaly = false, bool requiresOdyssey = false, int displayPriority = 0, bool hideInSubMenu = false)
		{
			this.name = name;
			this.requiresRoyalty = requiresRoyalty;
			this.requiresIdeology = requiresIdeology;
			this.requiresBiotech = requiresBiotech;
			this.requiresAnomaly = requiresAnomaly;
			this.requiresOdyssey = requiresOdyssey;
			this.displayPriority = displayPriority;
			this.hideInSubMenu = hideInSubMenu;
			if (!string.IsNullOrEmpty(category))
			{
				this.category = category;
			}
		}

		// Token: 0x0400013C RID: 316
		public string name;

		// Token: 0x0400013D RID: 317
		public string category = "General";

		// Token: 0x0400013E RID: 318
		public AllowedGameStates allowedGameStates = AllowedGameStates.Playing;

		// Token: 0x0400013F RID: 319
		public DebugActionType actionType;

		// Token: 0x04000140 RID: 320
		public bool requiresRoyalty;

		// Token: 0x04000141 RID: 321
		public bool requiresIdeology;

		// Token: 0x04000142 RID: 322
		public bool requiresBiotech;

		// Token: 0x04000143 RID: 323
		public bool requiresAnomaly;

		// Token: 0x04000144 RID: 324
		public bool requiresOdyssey;

		// Token: 0x04000145 RID: 325
		public int displayPriority;

		// Token: 0x04000146 RID: 326
		public bool hideInSubMenu;
	}
}
