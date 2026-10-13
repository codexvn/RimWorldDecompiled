using System;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x020005E0 RID: 1504
	public class PawnUIOverlay
	{
		// Token: 0x06002689 RID: 9865 RVA: 0x000CC281 File Offset: 0x000CA481
		public PawnUIOverlay(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x000CC290 File Offset: 0x000CA490
		public void DrawPawnGUIOverlay()
		{
			if (!this.pawn.Spawned || this.pawn.Map.fogGrid.IsFogged(this.pawn.Position) || WorldComponent_GravshipController.CutsceneInProgress)
			{
				return;
			}
			if (!this.pawn.RaceProps.Humanlike)
			{
				if (this.pawn.RaceProps.Animal)
				{
					if (!Prefs.AnimalNameMode.ShouldDisplayAnimalName(this.pawn))
					{
						return;
					}
				}
				else
				{
					if (!this.pawn.IsColonyMech)
					{
						return;
					}
					if (this.pawn.IsSelfShutdown())
					{
						this.pawn.Map.overlayDrawer.DrawOverlay(this.pawn, OverlayTypes.SelfShutdown);
					}
					if (!Prefs.MechNameMode.ShouldDisplayMechName(this.pawn))
					{
						return;
					}
				}
			}
			if (this.pawn.IsMutant && this.pawn.mutant.Def.hideLabel)
			{
				return;
			}
			Vector2 pos = GenMapUI.LabelDrawPosFor(this.pawn, -0.6f);
			GenMapUI.DrawPawnLabel(this.pawn, pos, 1f, 9999f, null, GameFont.Tiny, true, true);
			if (this.pawn.ShouldShowQuestionMark())
			{
				this.pawn.Map.overlayDrawer.DrawOverlay(this.pawn, OverlayTypes.QuestionMark);
			}
			Lord lord = this.pawn.GetLord();
			if (lord != null && lord.CurLordToil != null)
			{
				lord.CurLordToil.DrawPawnGUIOverlay(this.pawn);
			}
		}

		// Token: 0x04001FF1 RID: 8177
		private Pawn pawn;

		// Token: 0x04001FF2 RID: 8178
		private const float PawnLabelOffsetY = -0.6f;

		// Token: 0x04001FF3 RID: 8179
		private const int PawnStatBarWidth = 32;

		// Token: 0x04001FF4 RID: 8180
		private const float ActivityIconSize = 13f;

		// Token: 0x04001FF5 RID: 8181
		private const float ActivityIconOffsetY = 12f;
	}
}
