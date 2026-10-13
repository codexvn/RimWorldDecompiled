using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CA6 RID: 11430
	public class CompForbiddable : ThingComp
	{
		// Token: 0x1700288D RID: 10381
		// (get) Token: 0x060100AE RID: 65710 RVA: 0x004A5021 File Offset: 0x004A3221
		public CompProperties_Forbiddable Props
		{
			get
			{
				return (CompProperties_Forbiddable)this.props;
			}
		}

		// Token: 0x1700288E RID: 10382
		// (get) Token: 0x060100AF RID: 65711 RVA: 0x004A502E File Offset: 0x004A322E
		// (set) Token: 0x060100B0 RID: 65712 RVA: 0x004A5038 File Offset: 0x004A3238
		public bool Forbidden
		{
			get
			{
				return this.forbiddenInt;
			}
			set
			{
				if (value == this.forbiddenInt)
				{
					return;
				}
				this.forbiddenInt = value;
				if (this.parent.Spawned)
				{
					if (this.forbiddenInt)
					{
						this.parent.Map.listerHaulables.Notify_Forbidden(this.parent);
						this.parent.Map.listerMergeables.Notify_Forbidden(this.parent);
					}
					else
					{
						this.parent.Map.listerHaulables.Notify_Unforbidden(this.parent);
						this.parent.Map.listerMergeables.Notify_Unforbidden(this.parent);
					}
					if (this.parent is Building_Door)
					{
						this.parent.Map.reachability.ClearCache();
					}
				}
				this.UpdateOverlayHandle();
			}
		}

		// Token: 0x1700288F RID: 10383
		// (get) Token: 0x060100B1 RID: 65713 RVA: 0x004A5104 File Offset: 0x004A3304
		private OverlayTypes MyOverlayType
		{
			get
			{
				if (this.parent is Blueprint || this.parent is Frame)
				{
					if (this.parent.def.size.x > 1 || this.parent.def.size.z > 1)
					{
						return OverlayTypes.ForbiddenBig;
					}
					return OverlayTypes.Forbidden;
				}
				else
				{
					if (this.parent.def.category == ThingCategory.Building)
					{
						return OverlayTypes.ForbiddenBig;
					}
					return OverlayTypes.Forbidden;
				}
			}
		}

		// Token: 0x060100B2 RID: 65714 RVA: 0x004A5178 File Offset: 0x004A3378
		private void UpdateOverlayHandle()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayHandle);
			if (this.parent.Spawned && this.forbiddenInt)
			{
				this.overlayHandle = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, this.MyOverlayType));
			}
		}

		// Token: 0x060100B3 RID: 65715 RVA: 0x004A51F5 File Offset: 0x004A33F5
		public override void PostPostMake()
		{
			base.PostPostMake();
			if (this.Props.forbidOnMake)
			{
				this.forbiddenInt = true;
				this.parent.SetForbidden(true, true);
			}
		}

		// Token: 0x060100B4 RID: 65716 RVA: 0x004A521E File Offset: 0x004A341E
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.UpdateOverlayHandle();
		}

		// Token: 0x060100B5 RID: 65717 RVA: 0x004A5226 File Offset: 0x004A3426
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.forbiddenInt, "forbidden", false, false);
		}

		// Token: 0x060100B6 RID: 65718 RVA: 0x004A523A File Offset: 0x004A343A
		public override void PostSplitOff(Thing piece)
		{
			piece.SetForbidden(this.forbiddenInt, true);
		}

		// Token: 0x060100B7 RID: 65719 RVA: 0x004A5249 File Offset: 0x004A3449
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.parent is Building && !this.Props.allowNonPlayer && this.parent.Faction != Faction.OfPlayer)
			{
				yield break;
			}
			Building building = this.parent.SpawnedParentOrMe as Building;
			if (building != null && this.parent != building)
			{
				yield break;
			}
			Command_Toggle command_Toggle = new Command_Toggle();
			command_Toggle.hotKey = KeyBindingDefOf.Command_ItemForbid;
			command_Toggle.icon = TexCommand.ForbidOff;
			command_Toggle.isActive = (() => !this.Forbidden);
			command_Toggle.defaultLabel = "CommandAllow".TranslateWithBackup("DesignatorUnforbid");
			command_Toggle.activateIfAmbiguous = false;
			if (this.forbiddenInt)
			{
				command_Toggle.defaultDesc = "CommandForbiddenDesc".TranslateWithBackup("DesignatorUnforbidDesc");
			}
			else
			{
				command_Toggle.defaultDesc = "CommandNotForbiddenDesc".TranslateWithBackup("DesignatorForbidDesc");
			}
			if (this.parent.def.IsDoor)
			{
				command_Toggle.tutorTag = "ToggleForbidden-Door";
				command_Toggle.toggleAction = delegate()
				{
					this.Forbidden = !this.Forbidden;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.ForbiddingDoors, KnowledgeAmount.SpecificInteraction);
				};
			}
			else
			{
				command_Toggle.tutorTag = "ToggleForbidden";
				command_Toggle.toggleAction = delegate()
				{
					this.Forbidden = !this.Forbidden;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Forbidding, KnowledgeAmount.SpecificInteraction);
				};
			}
			yield return command_Toggle;
			yield break;
		}

		// Token: 0x0400A7BF RID: 42943
		private bool forbiddenInt;

		// Token: 0x0400A7C0 RID: 42944
		private OverlayHandle? overlayHandle;
	}
}
