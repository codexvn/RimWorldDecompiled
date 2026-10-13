using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A36 RID: 10806
	public class Building_MultiTileDoor : Building_SupportedDoor
	{
		// Token: 0x1700254F RID: 9551
		// (get) Token: 0x0600EFCF RID: 61391 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool CanDrawMovers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600EFD0 RID: 61392 RVA: 0x00459A4E File Offset: 0x00457C4E
		protected override void Tick()
		{
			base.Tick();
			this.tmpStuckOpen = base.StuckOpen;
		}

		// Token: 0x0600EFD1 RID: 61393 RVA: 0x00459A64 File Offset: 0x00457C64
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DoorPreDraw();
			if (!this.tmpStuckOpen)
			{
				float offsetDist = 0.25f + 0.35000002f * this.OpenPct;
				base.DrawMovers(drawLoc, offsetDist, this.Graphic, AltitudeLayer.DoorMoveable.AltitudeFor(), Building_MultiTileDoor.MoverDrawScale, this.Graphic.ShadowGraphic);
				if (this.def.building.upperMoverGraphic != null)
				{
					float offsetDist2 = 0.25f + 0.35000002f * Mathf.Clamp01(this.OpenPct * 2.5f);
					base.DrawMovers(drawLoc, offsetDist2, this.def.building.upperMoverGraphic.Graphic, Building_MultiTileDoor.UpperMoverAltitude, Building_MultiTileDoor.MoverDrawScale, null);
				}
			}
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x04009E6A RID: 40554
		private bool tmpStuckOpen;

		// Token: 0x04009E6B RID: 40555
		private const float UpperMoverOffsetStart = 0.25f;

		// Token: 0x04009E6C RID: 40556
		private const float UpperMoverOffsetEnd = 0.6f;

		// Token: 0x04009E6D RID: 40557
		private static readonly float UpperMoverAltitude = AltitudeLayer.DoorMoveable.AltitudeFor() + 0.018292684f;

		// Token: 0x04009E6E RID: 40558
		private static readonly Vector3 MoverDrawScale = new Vector3(0.5f, 1f, 1f);
	}
}
