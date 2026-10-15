using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompAssignableToPawn : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__31 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAssignableToPawn _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CCompGetGizmosExtra_003Ed__31(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompAssignableToPawn CS_0024_003C_003E8__locals7 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals7.ShouldShowAssignmentGizmo())
				{
					Command_Action command_Action = new Command_Action
					{
						defaultLabel = CS_0024_003C_003E8__locals7.GetAssignmentGizmoLabel(),
						icon = ContentFinder<Texture2D>.Get("UI/Commands/AssignOwner"),
						defaultDesc = CS_0024_003C_003E8__locals7.GetAssignmentGizmoDesc(),
						action = delegate
						{
							Find.WindowStack.Add(new Dialog_AssignBuildingOwner(CS_0024_003C_003E8__locals7));
						},
						hotKey = KeyBindingDefOf.Misc4
					};
					if (!CS_0024_003C_003E8__locals7.Props.noAssignablePawnsDesc.NullOrEmpty() && !CS_0024_003C_003E8__locals7.AssigningCandidates.Any())
					{
						command_Action.Disable(CS_0024_003C_003E8__locals7.Props.noAssignablePawnsDesc);
					}
					_003C_003E2__current = command_Action;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__31 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__31(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	protected List<Pawn> assignedPawns = new List<Pawn>();

	protected List<Pawn> uninstalledAssignedPawns = new List<Pawn>();

	public CompProperties_AssignableToPawn Props => (CompProperties_AssignableToPawn)props;

	public int MaxAssignedPawnsCount => Props.maxAssignedPawnsCount;

	public bool PlayerCanSeeAssignments
	{
		get
		{
			if (parent.Faction == Faction.OfPlayer)
			{
				return true;
			}
			for (int i = 0; i < assignedPawns.Count; i++)
			{
				if (assignedPawns[i].Faction == Faction.OfPlayer || assignedPawns[i].HostFaction == Faction.OfPlayer)
				{
					return true;
				}
			}
			return false;
		}
	}

	public virtual IEnumerable<Pawn> AssigningCandidates
	{
		get
		{
			if (!parent.Spawned)
			{
				return Enumerable.Empty<Pawn>();
			}
			return parent.Map.mapPawns.FreeColonists;
		}
	}

	public List<Pawn> AssignedPawnsForReading => assignedPawns;

	public IEnumerable<Pawn> AssignedPawns => assignedPawns;

	public bool HasFreeSlot => assignedPawns.Count < Props.maxAssignedPawnsCount;

	public int TotalSlots => Props.maxAssignedPawnsCount;

	protected virtual bool CanDrawOverlayForPawn(Pawn pawn)
	{
		return true;
	}

	public override void DrawGUIOverlay()
	{
		if (!Props.drawAssignmentOverlay || (!Props.drawUnownedAssignmentOverlay && !assignedPawns.Any()) || Find.CameraDriver.CurrentZoom != 0 || !PlayerCanSeeAssignments)
		{
			return;
		}
		Color defaultThingLabelColor = GenMapUI.DefaultThingLabelColor;
		if (!assignedPawns.Any())
		{
			GenMapUI.DrawThingLabel(parent, "Unowned".Translate(), defaultThingLabelColor);
		}
		if (assignedPawns.Count == 1)
		{
			Pawn pawn = assignedPawns[0];
			if (CanDrawOverlayForPawn(pawn) && (!pawn.RaceProps.Animal || Prefs.AnimalNameMode.ShouldDisplayAnimalName(pawn)))
			{
				GenMapUI.DrawThingLabel(parent, assignedPawns[0].LabelShort, defaultThingLabelColor);
			}
		}
	}

	protected virtual void SortAssignedPawns()
	{
		assignedPawns.RemoveAll((Pawn x) => x == null);
		assignedPawns.SortBy((Pawn x) => x.thingIDNumber);
	}

	public virtual void ForceAddPawn(Pawn pawn)
	{
		if (!assignedPawns.Contains(pawn))
		{
			assignedPawns.Add(pawn);
		}
		SortAssignedPawns();
	}

	public virtual void ForceRemovePawn(Pawn pawn)
	{
		if (assignedPawns.Contains(pawn))
		{
			assignedPawns.Remove(pawn);
		}
		uninstalledAssignedPawns.Remove(pawn);
		SortAssignedPawns();
	}

	public virtual AcceptanceReport CanAssignTo(Pawn pawn)
	{
		return AcceptanceReport.WasAccepted;
	}

	public virtual bool IdeoligionForbids(Pawn pawn)
	{
		return false;
	}

	public virtual void TryAssignPawn(Pawn pawn)
	{
		uninstalledAssignedPawns.Remove(pawn);
		if (!assignedPawns.Contains(pawn))
		{
			assignedPawns.Add(pawn);
			SortAssignedPawns();
		}
	}

	public virtual void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
	{
		if (assignedPawns.Contains(pawn))
		{
			assignedPawns.Remove(pawn);
			if (uninstall && pawn != null && !uninstalledAssignedPawns.Contains(pawn))
			{
				uninstalledAssignedPawns.Add(pawn);
			}
			if (sort)
			{
				SortAssignedPawns();
			}
		}
	}

	public virtual bool AssignedAnything(Pawn pawn)
	{
		return assignedPawns.Contains(pawn);
	}

	protected virtual bool ShouldShowAssignmentGizmo()
	{
		return parent.Faction == Faction.OfPlayer;
	}

	protected virtual string GetAssignmentGizmoLabel()
	{
		return "CommandThingSetOwnerLabel".Translate();
	}

	protected virtual string GetAssignmentGizmoDesc()
	{
		return "";
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__31))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__31(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Collections.Look(ref assignedPawns, "assignedPawns", LookMode.Reference);
		Scribe_Collections.Look(ref uninstalledAssignedPawns, "uninstalledAssignedPawns", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			assignedPawns.RemoveAll((Pawn x) => x == null);
			uninstalledAssignedPawns.RemoveAll((Pawn x) => x == null);
		}
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		if (mode != DestroyMode.WillReplace)
		{
			for (int num = assignedPawns.Count - 1; num >= 0; num--)
			{
				TryUnassignPawn(assignedPawns[num], sort: false, !parent.DestroyedOrNull());
			}
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		base.PostSpawnSetup(respawningAfterLoad);
		for (int num = uninstalledAssignedPawns.Count - 1; num >= 0; num--)
		{
			Pawn pawn = uninstalledAssignedPawns[num];
			if (CanSetUninstallAssignedPawn(pawn))
			{
				TryAssignPawn(pawn);
			}
		}
		uninstalledAssignedPawns.Clear();
	}

	public override void PostSwapMap()
	{
		for (int num = assignedPawns.Count - 1; num >= 0; num--)
		{
			if (assignedPawns[num].DestroyedOrNull() || !assignedPawns[num].SpawnedOrAnyParentSpawned)
			{
				TryUnassignPawn(assignedPawns[num]);
			}
		}
	}

	protected virtual bool CanSetUninstallAssignedPawn(Pawn pawn)
	{
		return false;
	}
}
