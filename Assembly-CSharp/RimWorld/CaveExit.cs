using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AFD RID: 11005
	public class CaveExit : PocketMapExit
	{
		// Token: 0x1700266C RID: 9836
		// (get) Token: 0x0600F5B3 RID: 62899 RVA: 0x00475D9B File Offset: 0x00473F9B
		private Graphic RopeGraphic
		{
			get
			{
				if (this.cachedRopeGraphic == null)
				{
					this.cachedRopeGraphic = GraphicDatabase.Get<Graphic_Single_AgeSecs>("Things/Building/Misc/CaveExit/CaveExit_Rope", ShaderDatabase.CaveExitRope, this.def.graphicData.drawSize, Color.white);
				}
				return this.cachedRopeGraphic;
			}
		}

		// Token: 0x0600F5B4 RID: 62900 RVA: 0x00475DD5 File Offset: 0x00473FD5
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.RopeGraphic.Draw(this.DrawPos + CaveExit.RopeDrawOffset, Rot4.North, this, 0f);
		}

		// Token: 0x0400A1C2 RID: 41410
		private static readonly Vector3 RopeDrawOffset = new Vector3(0f, 1f, 1f);

		// Token: 0x0400A1C3 RID: 41411
		[Unsaved(false)]
		private Graphic cachedRopeGraphic;
	}
}
