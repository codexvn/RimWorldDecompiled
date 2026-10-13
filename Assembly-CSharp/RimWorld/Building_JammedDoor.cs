using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A2C RID: 10796
	public class Building_JammedDoor : Building_Door
	{
		// Token: 0x1700253B RID: 9531
		// (get) Token: 0x0600EF74 RID: 61300 RVA: 0x00458417 File Offset: 0x00456617
		public bool Jammed
		{
			get
			{
				return this.jammed;
			}
		}

		// Token: 0x0600EF75 RID: 61301 RVA: 0x0045841F File Offset: 0x0045661F
		public void UnlockDoor()
		{
			this.jammed = false;
		}

		// Token: 0x0600EF76 RID: 61302 RVA: 0x00458428 File Offset: 0x00456628
		public void UnlockAndOpenDoor()
		{
			this.jammed = false;
			this.DoorOpen(110);
		}

		// Token: 0x0600EF77 RID: 61303 RVA: 0x00458439 File Offset: 0x00456639
		protected override void DoorOpen(int ticksToClose = 110)
		{
			base.DoorOpen(ticksToClose);
			this.holdOpenInt = true;
		}

		// Token: 0x0600EF78 RID: 61304 RVA: 0x00458449 File Offset: 0x00456649
		public override bool PawnCanOpen(Pawn p)
		{
			return !this.jammed;
		}

		// Token: 0x0600EF79 RID: 61305 RVA: 0x00458454 File Offset: 0x00456654
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (!this.jammed)
			{
				base.DrawAt(drawLoc, flip);
				return;
			}
			base.DoorPreDraw();
			foreach (ThingComp thingComp in base.AllComps)
			{
				IJammedDoorDrawer jammedDoorDrawer = thingComp as IJammedDoorDrawer;
				if (jammedDoorDrawer != null)
				{
					jammedDoorDrawer.DrawJammed(base.Rotation);
					break;
				}
			}
		}

		// Token: 0x0600EF7A RID: 61306 RVA: 0x004584D0 File Offset: 0x004566D0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos && !this.holdOpenInt)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Unlock",
					action = new Action(this.UnlockDoor)
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EF7B RID: 61307 RVA: 0x004584E0 File Offset: 0x004566E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.jammed, "jammed", false, false);
		}

		// Token: 0x04009E3E RID: 40510
		private bool jammed = true;
	}
}
