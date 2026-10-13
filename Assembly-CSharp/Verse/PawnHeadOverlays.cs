using System;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x020005D9 RID: 1497
	[StaticConstructorOnStartup]
	public class PawnHeadOverlays
	{
		// Token: 0x06002669 RID: 9833 RVA: 0x000CB221 File Offset: 0x000C9421
		public PawnHeadOverlays(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600266A RID: 9834 RVA: 0x000CB230 File Offset: 0x000C9430
		public void RenderStatusOverlays(Vector3 offset, Quaternion quat, Mesh headMesh)
		{
			Vector3 vector = this.pawn.DrawPos + offset + new Vector3(0f, 0f, 0.32f);
			Pawn_NeedsTracker needs = this.pawn.needs;
			if (((needs != null) ? needs.mood : null) != null && !this.pawn.Downed)
			{
				if (this.pawn.HitPoints > 0 && this.pawn.IsColonistPlayerControlled)
				{
					if (this.pawn.mindState.mentalBreaker.BreakExtremeIsImminent)
					{
						if (Time.time % 1.2f < 0.4f)
						{
							this.DrawHeadGlow(vector, PawnHeadOverlays.MentalStateImminentMat);
						}
					}
					else if (this.pawn.mindState.mentalBreaker.BreakExtremeIsApproaching && Time.time % 1.2f < 0.4f)
					{
						this.DrawHeadGlow(vector, PawnHeadOverlays.UnhappyMat);
					}
				}
				Pawn_MindState mindState = this.pawn.mindState;
				MentalStateDef mentalStateDef;
				if (mindState == null)
				{
					mentalStateDef = null;
				}
				else
				{
					MentalStateHandler mentalStateHandler = mindState.mentalStateHandler;
					mentalStateDef = ((mentalStateHandler != null) ? mentalStateHandler.CurStateDef : null);
				}
				MentalStateDef mentalStateDef2 = mentalStateDef;
				if ((ModsConfig.OdysseyActive && mentalStateDef2 == MentalStateDefOf.Terror) || (ModsConfig.BiotechActive && mentalStateDef2 == MentalStateDefOf.PanicFleeFire))
				{
					ThingDefOf.Mote_TerrorHalo.graphicData.Graphic.DrawWorker(vector, this.pawn.Rotation, null, null, 0f);
				}
			}
		}

		// Token: 0x0600266B RID: 9835 RVA: 0x000CB384 File Offset: 0x000C9584
		private void DrawHeadGlow(Vector3 headLoc, Material mat)
		{
			Graphics.DrawMesh(MeshPool.plane20, headLoc, Quaternion.identity, mat, 0);
		}

		// Token: 0x04001FCB RID: 8139
		private Pawn pawn;

		// Token: 0x04001FCC RID: 8140
		private const float AngerBlinkPeriod = 1.2f;

		// Token: 0x04001FCD RID: 8141
		private const float AngerBlinkLength = 0.4f;

		// Token: 0x04001FCE RID: 8142
		private static readonly Material UnhappyMat = MaterialPool.MatFrom("Things/Pawn/Effects/Unhappy");

		// Token: 0x04001FCF RID: 8143
		private static readonly Material MentalStateImminentMat = MaterialPool.MatFrom("Things/Pawn/Effects/MentalStateImminent");
	}
}
