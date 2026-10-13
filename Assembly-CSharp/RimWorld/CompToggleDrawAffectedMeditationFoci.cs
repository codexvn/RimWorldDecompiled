using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E99 RID: 11929
	[StaticConstructorOnStartup]
	public class CompToggleDrawAffectedMeditationFoci : ThingComp
	{
		// Token: 0x17002B31 RID: 11057
		// (get) Token: 0x06010CCF RID: 68815 RVA: 0x004D632C File Offset: 0x004D452C
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
		}

		// Token: 0x17002B32 RID: 11058
		// (get) Token: 0x06010CD0 RID: 68816 RVA: 0x004D6334 File Offset: 0x004D4534
		public CompProperties_ToggleDrawAffectedMeditationFoci Props
		{
			get
			{
				return (CompProperties_ToggleDrawAffectedMeditationFoci)this.props;
			}
		}

		// Token: 0x06010CD1 RID: 68817 RVA: 0x004D6341 File Offset: 0x004D4541
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.enabled = this.Props.defaultEnabled;
			}
		}

		// Token: 0x06010CD2 RID: 68818 RVA: 0x004D6364 File Offset: 0x004D4564
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Command_Toggle command_Toggle = new Command_Toggle();
			command_Toggle.defaultLabel = "CommandWarnInBuildingRadius".Translate();
			command_Toggle.defaultDesc = "CommandWarnInBuildingRadiusDesc".Translate();
			command_Toggle.icon = CompToggleDrawAffectedMeditationFoci.CommandTex;
			command_Toggle.isActive = (() => this.enabled);
			Command_Toggle command_Toggle2 = command_Toggle;
			command_Toggle2.toggleAction = (Action)Delegate.Combine(command_Toggle2.toggleAction, new Action(delegate()
			{
				this.enabled = !this.enabled;
			}));
			yield return command_Toggle;
			yield break;
		}

		// Token: 0x06010CD3 RID: 68819 RVA: 0x004D6374 File Offset: 0x004D4574
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.enabled, "enabled", false, false);
		}

		// Token: 0x0400AEED RID: 44781
		private bool enabled;

		// Token: 0x0400AEEE RID: 44782
		private static readonly Texture2D CommandTex = ContentFinder<Texture2D>.Get("UI/Commands/PlaceBlueprints", true);
	}
}
