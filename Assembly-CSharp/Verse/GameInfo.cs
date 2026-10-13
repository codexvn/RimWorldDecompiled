using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200033C RID: 828
	public sealed class GameInfo : IExposable
	{
		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001506 RID: 5382 RVA: 0x000638D5 File Offset: 0x00061AD5
		public float RealPlayTimeInteracting
		{
			get
			{
				return this.realPlayTimeInteracting;
			}
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x000638DD File Offset: 0x00061ADD
		public void GameInfoOnGUI()
		{
			if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseMove || Event.current.type == EventType.KeyDown)
			{
				this.lastInputRealTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x00063910 File Offset: 0x00061B10
		public void GameInfoUpdate()
		{
			if (Time.realtimeSinceStartup < this.lastInputRealTime + 90f && Find.MainTabsRoot.OpenTab != MainButtonDefOf.Menu && Current.ProgramState == ProgramState.Playing && !Find.WindowStack.IsOpen<Dialog_Options>())
			{
				this.realPlayTimeInteracting += RealTime.realDeltaTime;
			}
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x00063968 File Offset: 0x00061B68
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.realPlayTimeInteracting, "realPlayTimeInteracting", 0f, false);
			Scribe_Values.Look<bool>(ref this.permadeathMode, "permadeathMode", false, false);
			Scribe_Values.Look<string>(ref this.permadeathModeUniqueName, "permadeathModeUniqueName", null, false);
			Scribe_Values.Look<PlanetTile>(ref this.startingTile, "startingTile", PlanetTile.Invalid, false);
			Scribe_Collections.Look<Pawn>(ref this.startingAndOptionalPawns, "startingAndOptionalPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.startingAndOptionalPawns.Count - 1; i >= 0; i--)
				{
					if (this.startingAndOptionalPawns[i] == null)
					{
						this.startingAndOptionalPawns.RemoveAt(i);
					}
				}
			}
		}

		// Token: 0x040013A3 RID: 5027
		public bool permadeathMode;

		// Token: 0x040013A4 RID: 5028
		public string permadeathModeUniqueName;

		// Token: 0x040013A5 RID: 5029
		private float realPlayTimeInteracting;

		// Token: 0x040013A6 RID: 5030
		public PlanetTile startingTile = PlanetTile.Invalid;

		// Token: 0x040013A7 RID: 5031
		public List<Pawn> startingAndOptionalPawns = new List<Pawn>();

		// Token: 0x040013A8 RID: 5032
		private float lastInputRealTime;
	}
}
