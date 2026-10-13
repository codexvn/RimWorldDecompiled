using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008BF RID: 2239
	public class MoteDualAttached_Cards : MoteDualAttached
	{
		// Token: 0x060038A2 RID: 14498 RVA: 0x0012DBB0 File Offset: 0x0012BDB0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (this.link1.Linked && this.link1.Target.HasThing)
			{
				Pawn pawn = this.link2.Target.Thing as Pawn;
				if (pawn != null && pawn.Rotation == Rot4.North)
				{
					return;
				}
			}
			if (this.link2.Linked && this.link2.Target.HasThing)
			{
				Pawn pawn2 = this.link2.Target.Thing as Pawn;
				if (pawn2 != null && pawn2.Rotation == Rot4.North)
				{
					return;
				}
			}
			base.DrawAt(drawLoc, flip);
		}
	}
}
