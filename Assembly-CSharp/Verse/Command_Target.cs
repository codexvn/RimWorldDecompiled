using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000B8A RID: 2954
	public class Command_Target : Command
	{
		// Token: 0x06004B9A RID: 19354 RVA: 0x001813BC File Offset: 0x0017F5BC
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			Find.DesignatorManager.Deselect();
			Find.Targeter.BeginTargeting(this.targetingParams, delegate(LocalTargetInfo target)
			{
				this.action(target);
			}, null, null, null, null, null, true, null, new Action<LocalTargetInfo>(this.Update));
		}

		// Token: 0x06004B9B RID: 19355 RVA: 0x00181414 File Offset: 0x0017F614
		public override void GizmoUpdateOnMouseover()
		{
			this.Update(null);
			base.GizmoUpdateOnMouseover();
		}

		// Token: 0x06004B9C RID: 19356 RVA: 0x00181428 File Offset: 0x0017F628
		private void Update(LocalTargetInfo target)
		{
			if (this.lastUpdate == Time.frameCount)
			{
				return;
			}
			this.lastUpdate = Time.frameCount;
			Action<LocalTargetInfo> action = this.onUpdate;
			if (action == null)
			{
				return;
			}
			action(target);
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x00002501 File Offset: 0x00000701
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			return false;
		}

		// Token: 0x040035DE RID: 13790
		public Action<LocalTargetInfo> action;

		// Token: 0x040035DF RID: 13791
		public Action<LocalTargetInfo> onUpdate;

		// Token: 0x040035E0 RID: 13792
		public TargetingParameters targetingParams;

		// Token: 0x040035E1 RID: 13793
		private int lastUpdate;
	}
}
