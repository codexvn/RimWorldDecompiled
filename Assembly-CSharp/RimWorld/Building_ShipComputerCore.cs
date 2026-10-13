using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002989 RID: 10633
	public class Building_ShipComputerCore : Building
	{
		// Token: 0x170023BC RID: 9148
		// (get) Token: 0x0600E9CA RID: 59850 RVA: 0x00440878 File Offset: 0x0043EA78
		private bool CanLaunchNow
		{
			get
			{
				return !ShipUtility.LaunchFailReasons(this).Any<string>();
			}
		}

		// Token: 0x0600E9CB RID: 59851 RVA: 0x00440888 File Offset: 0x0043EA88
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in ShipUtility.ShipStartupGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			Command_Action command_Action = new Command_Action
			{
				action = new Action(this.TryLaunch),
				defaultLabel = "CommandShipLaunch".Translate(),
				defaultDesc = "CommandShipLaunchDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc1,
				icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true)
			};
			if (!this.CanLaunchNow)
			{
				command_Action.Disable(ShipUtility.LaunchFailReasons(this).First<string>());
			}
			if (ShipCountdown.CountingDown)
			{
				command_Action.Disable(null);
			}
			yield return command_Action;
			yield break;
			yield break;
		}

		// Token: 0x0600E9CC RID: 59852 RVA: 0x00440898 File Offset: 0x0043EA98
		public void ForceLaunch()
		{
			ShipCountdown.InitiateCountdown(this);
			if (base.Spawned)
			{
				QuestUtility.SendQuestTargetSignals(base.Map.Parent.questTags, "LaunchedShip");
			}
		}

		// Token: 0x0600E9CD RID: 59853 RVA: 0x004408C2 File Offset: 0x0043EAC2
		private void TryLaunch()
		{
			if (this.CanLaunchNow)
			{
				this.ForceLaunch();
			}
		}
	}
}
