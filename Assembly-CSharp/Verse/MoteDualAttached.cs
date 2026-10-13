using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008BE RID: 2238
	public class MoteDualAttached : Mote
	{
		// Token: 0x0600389C RID: 14492 RVA: 0x0012D997 File Offset: 0x0012BB97
		public void Attach(TargetInfo a, TargetInfo b)
		{
			this.link1 = new MoteAttachLink(a, Vector3.zero, false);
			this.link2 = new MoteAttachLink(b, Vector3.zero, false);
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x0012D9BD File Offset: 0x0012BBBD
		public void Attach(TargetInfo a, TargetInfo b, Vector3 offsetA, Vector3 offsetB)
		{
			this.link1 = new MoteAttachLink(a, offsetA, false);
			this.link2 = new MoteAttachLink(b, offsetB, false);
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x0012D9DC File Offset: 0x0012BBDC
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.UpdatePositionAndRotation();
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x0012D9EC File Offset: 0x0012BBEC
		public void UpdateTargets(TargetInfo a, TargetInfo b, Vector3 offsetA, Vector3 offsetB)
		{
			this.link1.UpdateTarget(a, offsetA);
			this.link2.UpdateTarget(b, offsetB);
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x0012DA0C File Offset: 0x0012BC0C
		protected void UpdatePositionAndRotation()
		{
			if (this.link1.Linked)
			{
				if (this.link2.Linked)
				{
					if (!this.link1.Target.ThingDestroyed)
					{
						this.link1.UpdateDrawPos();
					}
					if (!this.link2.Target.ThingDestroyed)
					{
						this.link2.UpdateDrawPos();
					}
					this.exactPosition = (this.link1.LastDrawPos + this.link2.LastDrawPos) * 0.5f;
					if (this.def.mote.rotateTowardsTarget)
					{
						this.exactRotation = this.link1.LastDrawPos.AngleToFlat(this.link2.LastDrawPos) + 90f;
					}
					if (this.def.mote.scaleToConnectTargets)
					{
						this.linearScale = new Vector3(this.def.graphicData.drawSize.y, 1f, (this.link2.LastDrawPos - this.link1.LastDrawPos).MagnitudeHorizontal());
					}
				}
				else
				{
					if (!this.link1.Target.ThingDestroyed)
					{
						this.link1.UpdateDrawPos();
					}
					this.exactPosition = this.link1.LastDrawPos + this.def.mote.attachedDrawOffset;
				}
			}
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}

		// Token: 0x040029E9 RID: 10729
		protected MoteAttachLink link2 = MoteAttachLink.Invalid;
	}
}
