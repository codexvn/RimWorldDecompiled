using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CAD RID: 11437
	public class CompGatherSpot : ThingComp
	{
		// Token: 0x17002894 RID: 10388
		// (get) Token: 0x060100D7 RID: 65751 RVA: 0x004A5A25 File Offset: 0x004A3C25
		// (set) Token: 0x060100D8 RID: 65752 RVA: 0x004A5A30 File Offset: 0x004A3C30
		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				if (value == this.active)
				{
					return;
				}
				this.active = value;
				if (this.parent.Spawned)
				{
					if (this.active)
					{
						this.parent.Map.gatherSpotLister.RegisterActivated(this);
						return;
					}
					this.parent.Map.gatherSpotLister.RegisterDeactivated(this);
				}
			}
		}

		// Token: 0x060100D9 RID: 65753 RVA: 0x004A5A90 File Offset: 0x004A3C90
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.active, "active", false, false);
		}

		// Token: 0x060100DA RID: 65754 RVA: 0x004A5AA4 File Offset: 0x004A3CA4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.parent.Faction != Faction.OfPlayer && !respawningAfterLoad)
			{
				this.active = false;
			}
			if (this.Active)
			{
				this.parent.Map.gatherSpotLister.RegisterActivated(this);
			}
		}

		// Token: 0x060100DB RID: 65755 RVA: 0x004A5AF2 File Offset: 0x004A3CF2
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			if (this.Active)
			{
				map.gatherSpotLister.RegisterDeactivated(this);
			}
		}

		// Token: 0x060100DC RID: 65756 RVA: 0x004A5B10 File Offset: 0x004A3D10
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Command_Toggle command_Toggle = new Command_Toggle();
			command_Toggle.hotKey = KeyBindingDefOf.Command_TogglePower;
			command_Toggle.defaultLabel = "CommandGatherSpotToggleLabel".Translate();
			command_Toggle.icon = TexCommand.GatherSpotActive;
			command_Toggle.isActive = (() => this.Active);
			command_Toggle.toggleAction = delegate()
			{
				this.Active = !this.Active;
			};
			if (this.Active)
			{
				command_Toggle.defaultDesc = "CommandGatherSpotToggleDescActive".Translate();
			}
			else
			{
				command_Toggle.defaultDesc = "CommandGatherSpotToggleDescInactive".Translate();
			}
			yield return command_Toggle;
			yield break;
		}

		// Token: 0x0400A7CB RID: 42955
		private bool active = true;
	}
}
