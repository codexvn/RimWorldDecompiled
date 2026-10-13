using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002A55 RID: 10837
	public class Building_SupportedDoor : Building_Door
	{
		// Token: 0x17002579 RID: 9593
		// (get) Token: 0x0600F0B6 RID: 61622 RVA: 0x0045D32C File Offset: 0x0045B52C
		private Graphic SupportGraphic
		{
			get
			{
				if (this.suppportGraphic == null)
				{
					GraphicData doorSupportGraphic = this.def.building.doorSupportGraphic;
					Graphic graphic = (doorSupportGraphic != null) ? doorSupportGraphic.Graphic : null;
					if (graphic == null)
					{
						return null;
					}
					this.suppportGraphic = graphic.GetColoredVersion(graphic.Shader, this.DrawColor, this.Graphic.ColorTwo);
				}
				return this.suppportGraphic;
			}
		}

		// Token: 0x1700257A RID: 9594
		// (get) Token: 0x0600F0B7 RID: 61623 RVA: 0x0045D38C File Offset: 0x0045B58C
		private Graphic TopGraphic
		{
			get
			{
				if (this.topGraphic == null)
				{
					GraphicData doorTopGraphic = this.def.building.doorTopGraphic;
					Graphic graphic = (doorTopGraphic != null) ? doorTopGraphic.Graphic : null;
					if (graphic == null)
					{
						return null;
					}
					this.topGraphic = graphic.GetColoredVersion(graphic.Shader, this.DrawColor, this.Graphic.ColorTwo);
				}
				return this.topGraphic;
			}
		}

		// Token: 0x0600F0B8 RID: 61624 RVA: 0x0045D3EC File Offset: 0x0045B5EC
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DoorPreDraw();
			Vector3 a = this.DrawPos;
			bool flag = base.Rotation == Rot4.North || base.Rotation == Rot4.South;
			a += (flag ? this.def.building.doorTopHorizontalOffset : this.def.building.doorTopVerticalOffset);
			a.y = (flag ? AltitudeLayer.BuildingOnTop.AltitudeFor() : AltitudeLayer.Blueprint.AltitudeFor());
			Graphic supportGraphic = this.SupportGraphic;
			if (supportGraphic != null)
			{
				supportGraphic.Draw(a + this.def.building.doorSupportGraphicOffset, base.Rotation, this, 0f);
			}
			a.y = AltitudeLayer.Blueprint.AltitudeFor();
			Graphic graphic = this.TopGraphic;
			if (graphic != null)
			{
				graphic.Draw(a + this.def.building.doorTopGraphicOffset, base.Rotation, this, 0f);
			}
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x0600F0B9 RID: 61625 RVA: 0x0045D4F0 File Offset: 0x0045B6F0
		protected override void Tick()
		{
			base.Tick();
			if (!this.openInt && this.OpenPct <= 0f && this.openedBefore)
			{
				this.openedBefore = false;
				if (!this.def.building.soundDoorCloseEnd.NullOrUndefined())
				{
					this.def.building.soundDoorCloseEnd.PlayOneShot(this);
				}
			}
		}

		// Token: 0x0600F0BA RID: 61626 RVA: 0x0045D559 File Offset: 0x0045B759
		protected override void DoorOpen(int ticksToClose = 110)
		{
			base.DoorOpen(ticksToClose);
			this.openedBefore = true;
		}

		// Token: 0x0600F0BB RID: 61627 RVA: 0x0045D569 File Offset: 0x0045B769
		public override void Notify_ColorChanged()
		{
			this.suppportGraphic = null;
			this.topGraphic = null;
			base.Notify_ColorChanged();
		}

		// Token: 0x0600F0BC RID: 61628 RVA: 0x0045D57F File Offset: 0x0045B77F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.openedBefore, "openedBefore", false, false);
		}

		// Token: 0x04009EDC RID: 40668
		private bool openedBefore;

		// Token: 0x04009EDD RID: 40669
		[Unsaved(false)]
		private Graphic suppportGraphic;

		// Token: 0x04009EDE RID: 40670
		[Unsaved(false)]
		private Graphic topGraphic;
	}
}
