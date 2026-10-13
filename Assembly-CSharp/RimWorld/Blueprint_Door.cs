using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002950 RID: 10576
	public class Blueprint_Door : Blueprint_Build
	{
		// Token: 0x17002337 RID: 9015
		// (get) Token: 0x0600E7CA RID: 59338 RVA: 0x0043881B File Offset: 0x00436A1B
		public override Graphic Graphic
		{
			get
			{
				return base.DefaultGraphic;
			}
		}

		// Token: 0x0600E7CB RID: 59339 RVA: 0x00438824 File Offset: 0x00436A24
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (this.def.entityDefToBuild.Size == IntVec2.One)
			{
				base.Rotation = DoorUtility.DoorRotationAt(base.Position, base.Map, base.BuildDef.building.preferConnectingToFences);
			}
			base.DrawAt(drawLoc, flip);
		}
	}
}
