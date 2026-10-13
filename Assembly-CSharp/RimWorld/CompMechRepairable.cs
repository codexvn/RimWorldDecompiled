using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D46 RID: 11590
	public class CompMechRepairable : ThingComp
	{
		// Token: 0x17002966 RID: 10598
		// (get) Token: 0x06010481 RID: 66689 RVA: 0x004B4AD7 File Offset: 0x004B2CD7
		public CompProperties_MechRepairable Props
		{
			get
			{
				return (CompProperties_MechRepairable)this.props;
			}
		}

		// Token: 0x06010482 RID: 66690 RVA: 0x004B4AE4 File Offset: 0x004B2CE4
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.parent.Faction == Faction.OfPlayer)
			{
				Command_Toggle command_Toggle = new Command_Toggle();
				command_Toggle.defaultLabel = "CommandAutoRepair".Translate();
				command_Toggle.defaultDesc = "CommandAutoRepairDesc".Translate();
				command_Toggle.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/AutoRepair", true);
				command_Toggle.isActive = (() => this.autoRepair);
				Command_Toggle command_Toggle2 = command_Toggle;
				command_Toggle2.toggleAction = (Action)Delegate.Combine(command_Toggle2.toggleAction, new Action(delegate()
				{
					this.autoRepair = !this.autoRepair;
				}));
				yield return command_Toggle;
			}
			yield break;
		}

		// Token: 0x06010483 RID: 66691 RVA: 0x004B4AF4 File Offset: 0x004B2CF4
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.autoRepair, "autoRepair", false, false);
		}

		// Token: 0x0400AA13 RID: 43539
		public bool autoRepair;
	}
}
