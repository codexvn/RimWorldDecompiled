using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BF4 RID: 11252
	public class CompAssignableToPawn : ThingComp
	{
		// Token: 0x17002792 RID: 10130
		// (get) Token: 0x0600FC0D RID: 64525 RVA: 0x00492124 File Offset: 0x00490324
		public CompProperties_AssignableToPawn Props
		{
			get
			{
				return (CompProperties_AssignableToPawn)this.props;
			}
		}

		// Token: 0x17002793 RID: 10131
		// (get) Token: 0x0600FC0E RID: 64526 RVA: 0x00492131 File Offset: 0x00490331
		public int MaxAssignedPawnsCount
		{
			get
			{
				return this.Props.maxAssignedPawnsCount;
			}
		}

		// Token: 0x17002794 RID: 10132
		// (get) Token: 0x0600FC0F RID: 64527 RVA: 0x00492140 File Offset: 0x00490340
		public bool PlayerCanSeeAssignments
		{
			get
			{
				if (this.parent.Faction == Faction.OfPlayer)
				{
					return true;
				}
				for (int i = 0; i < this.assignedPawns.Count; i++)
				{
					if (this.assignedPawns[i].Faction == Faction.OfPlayer || this.assignedPawns[i].HostFaction == Faction.OfPlayer)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17002795 RID: 10133
		// (get) Token: 0x0600FC10 RID: 64528 RVA: 0x004921AA File Offset: 0x004903AA
		public virtual IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return this.parent.Map.mapPawns.FreeColonists;
			}
		}

		// Token: 0x17002796 RID: 10134
		// (get) Token: 0x0600FC11 RID: 64529 RVA: 0x004921D4 File Offset: 0x004903D4
		public List<Pawn> AssignedPawnsForReading
		{
			get
			{
				return this.assignedPawns;
			}
		}

		// Token: 0x17002797 RID: 10135
		// (get) Token: 0x0600FC12 RID: 64530 RVA: 0x004921D4 File Offset: 0x004903D4
		public IEnumerable<Pawn> AssignedPawns
		{
			get
			{
				return this.assignedPawns;
			}
		}

		// Token: 0x17002798 RID: 10136
		// (get) Token: 0x0600FC13 RID: 64531 RVA: 0x004921DC File Offset: 0x004903DC
		public bool HasFreeSlot
		{
			get
			{
				return this.assignedPawns.Count < this.Props.maxAssignedPawnsCount;
			}
		}

		// Token: 0x17002799 RID: 10137
		// (get) Token: 0x0600FC14 RID: 64532 RVA: 0x00492131 File Offset: 0x00490331
		public int TotalSlots
		{
			get
			{
				return this.Props.maxAssignedPawnsCount;
			}
		}

		// Token: 0x0600FC15 RID: 64533 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool CanDrawOverlayForPawn(Pawn pawn)
		{
			return true;
		}

		// Token: 0x0600FC16 RID: 64534 RVA: 0x004921F8 File Offset: 0x004903F8
		public override void DrawGUIOverlay()
		{
			if (!this.Props.drawAssignmentOverlay || (!this.Props.drawUnownedAssignmentOverlay && !this.assignedPawns.Any<Pawn>()))
			{
				return;
			}
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest && this.PlayerCanSeeAssignments)
			{
				Color defaultThingLabelColor = GenMapUI.DefaultThingLabelColor;
				if (!this.assignedPawns.Any<Pawn>())
				{
					GenMapUI.DrawThingLabel(this.parent, "Unowned".Translate(), defaultThingLabelColor);
				}
				if (this.assignedPawns.Count == 1)
				{
					Pawn pawn = this.assignedPawns[0];
					if (!this.CanDrawOverlayForPawn(pawn))
					{
						return;
					}
					if (pawn.RaceProps.Animal && !Prefs.AnimalNameMode.ShouldDisplayAnimalName(pawn))
					{
						return;
					}
					GenMapUI.DrawThingLabel(this.parent, this.assignedPawns[0].LabelShort, defaultThingLabelColor);
				}
			}
		}

		// Token: 0x0600FC17 RID: 64535 RVA: 0x004922D4 File Offset: 0x004904D4
		protected virtual void SortAssignedPawns()
		{
			this.assignedPawns.RemoveAll((Pawn x) => x == null);
			this.assignedPawns.SortBy((Pawn x) => x.thingIDNumber);
		}

		// Token: 0x0600FC18 RID: 64536 RVA: 0x00492336 File Offset: 0x00490536
		public virtual void ForceAddPawn(Pawn pawn)
		{
			if (!this.assignedPawns.Contains(pawn))
			{
				this.assignedPawns.Add(pawn);
			}
			this.SortAssignedPawns();
		}

		// Token: 0x0600FC19 RID: 64537 RVA: 0x00492358 File Offset: 0x00490558
		public virtual void ForceRemovePawn(Pawn pawn)
		{
			if (this.assignedPawns.Contains(pawn))
			{
				this.assignedPawns.Remove(pawn);
			}
			this.uninstalledAssignedPawns.Remove(pawn);
			this.SortAssignedPawns();
		}

		// Token: 0x0600FC1A RID: 64538 RVA: 0x000A5EF4 File Offset: 0x000A40F4
		public virtual AcceptanceReport CanAssignTo(Pawn pawn)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600FC1B RID: 64539 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool IdeoligionForbids(Pawn pawn)
		{
			return false;
		}

		// Token: 0x0600FC1C RID: 64540 RVA: 0x00492388 File Offset: 0x00490588
		public virtual void TryAssignPawn(Pawn pawn)
		{
			this.uninstalledAssignedPawns.Remove(pawn);
			if (this.assignedPawns.Contains(pawn))
			{
				return;
			}
			this.assignedPawns.Add(pawn);
			this.SortAssignedPawns();
		}

		// Token: 0x0600FC1D RID: 64541 RVA: 0x004923B8 File Offset: 0x004905B8
		public virtual void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
		{
			if (!this.assignedPawns.Contains(pawn))
			{
				return;
			}
			this.assignedPawns.Remove(pawn);
			if (uninstall && pawn != null && !this.uninstalledAssignedPawns.Contains(pawn))
			{
				this.uninstalledAssignedPawns.Add(pawn);
			}
			if (sort)
			{
				this.SortAssignedPawns();
			}
		}

		// Token: 0x0600FC1E RID: 64542 RVA: 0x0049240A File Offset: 0x0049060A
		public virtual bool AssignedAnything(Pawn pawn)
		{
			return this.assignedPawns.Contains(pawn);
		}

		// Token: 0x0600FC1F RID: 64543 RVA: 0x00492418 File Offset: 0x00490618
		protected virtual bool ShouldShowAssignmentGizmo()
		{
			return this.parent.Faction == Faction.OfPlayer;
		}

		// Token: 0x0600FC20 RID: 64544 RVA: 0x0049242C File Offset: 0x0049062C
		protected virtual string GetAssignmentGizmoLabel()
		{
			return "CommandThingSetOwnerLabel".Translate();
		}

		// Token: 0x0600FC21 RID: 64545 RVA: 0x00035ADE File Offset: 0x00033CDE
		protected virtual string GetAssignmentGizmoDesc()
		{
			return "";
		}

		// Token: 0x0600FC22 RID: 64546 RVA: 0x0049243D File Offset: 0x0049063D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.ShouldShowAssignmentGizmo())
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = this.GetAssignmentGizmoLabel();
				command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/AssignOwner", true);
				command_Action.defaultDesc = this.GetAssignmentGizmoDesc();
				command_Action.action = delegate()
				{
					Find.WindowStack.Add(new Dialog_AssignBuildingOwner(this));
				};
				command_Action.hotKey = KeyBindingDefOf.Misc4;
				if (!this.Props.noAssignablePawnsDesc.NullOrEmpty() && !this.AssigningCandidates.Any<Pawn>())
				{
					command_Action.Disable(this.Props.noAssignablePawnsDesc);
				}
				yield return command_Action;
			}
			yield break;
		}

		// Token: 0x0600FC23 RID: 64547 RVA: 0x00492450 File Offset: 0x00490650
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Collections.Look<Pawn>(ref this.assignedPawns, "assignedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.uninstalledAssignedPawns, "uninstalledAssignedPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.assignedPawns.RemoveAll((Pawn x) => x == null);
				this.uninstalledAssignedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600FC24 RID: 64548 RVA: 0x004924F0 File Offset: 0x004906F0
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				for (int i = this.assignedPawns.Count - 1; i >= 0; i--)
				{
					this.TryUnassignPawn(this.assignedPawns[i], false, !this.parent.DestroyedOrNull());
				}
			}
		}

		// Token: 0x0600FC25 RID: 64549 RVA: 0x0049253C File Offset: 0x0049073C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			for (int i = this.uninstalledAssignedPawns.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.uninstalledAssignedPawns[i];
				if (this.CanSetUninstallAssignedPawn(pawn))
				{
					this.TryAssignPawn(pawn);
				}
			}
			this.uninstalledAssignedPawns.Clear();
		}

		// Token: 0x0600FC26 RID: 64550 RVA: 0x00492590 File Offset: 0x00490790
		public override void PostSwapMap()
		{
			for (int i = this.assignedPawns.Count - 1; i >= 0; i--)
			{
				if (this.assignedPawns[i].DestroyedOrNull() || !this.assignedPawns[i].SpawnedOrAnyParentSpawned)
				{
					this.TryUnassignPawn(this.assignedPawns[i], true, false);
				}
			}
		}

		// Token: 0x0600FC27 RID: 64551 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool CanSetUninstallAssignedPawn(Pawn pawn)
		{
			return false;
		}

		// Token: 0x0400A530 RID: 42288
		protected List<Pawn> assignedPawns = new List<Pawn>();

		// Token: 0x0400A531 RID: 42289
		protected List<Pawn> uninstalledAssignedPawns = new List<Pawn>();
	}
}
