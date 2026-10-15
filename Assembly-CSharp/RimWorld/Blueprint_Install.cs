using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class Blueprint_Install : Blueprint
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__20 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Blueprint_Install _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		private IEnumerator<Command> _003C_003E7__wrap2;

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
		public _003CGetGizmos_003Ed__20(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Blueprint_Install blueprint_Install = _003C_003E4__this;
				Command command;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Blueprint)blueprint_Install).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00ee;
				case 3:
					{
						_003C_003E1__state = -4;
						goto IL_0148;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					command = BuildCopyCommandUtility.BuildCopyCommand(blueprint_Install.ThingToInstall.def, blueprint_Install.ThingToInstall.Stuff, blueprint_Install.ThingToInstall.StyleSourcePrecept as Precept_Building, blueprint_Install.StyleDef, styleOverridden: true, null);
					if (command != null)
					{
						_003C_003E2__current = command;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00ee;
					IL_0148:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Command current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					break;
					IL_00ee:
					if (blueprint_Install.Faction != Faction.OfPlayer)
					{
						break;
					}
					_003C_003E7__wrap2 = BuildRelatedCommandUtility.RelatedBuildCommands(blueprint_Install.ThingToInstall.def).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0148;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__20 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__20(0)
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

	private MinifiedThing miniToInstall;

	private Building buildingToReinstall;

	public Thing MiniToInstallOrBuildingToReinstall
	{
		get
		{
			if (miniToInstall != null)
			{
				return miniToInstall;
			}
			if (buildingToReinstall != null)
			{
				return buildingToReinstall;
			}
			throw new InvalidOperationException("Nothing to install.");
		}
	}

	public Thing ThingToInstall => MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();

	public override ThingStyleDef StyleDef
	{
		get
		{
			return ThingToInstall?.StyleDef;
		}
		set
		{
			ThingToInstall.StyleDef = value;
		}
	}

	protected override float WorkTotal => (ThingToInstall.def.plant != null) ? (-1) : 150;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref miniToInstall, "miniToInstall");
		Scribe_References.Look(ref buildingToReinstall, "buildingToReinstall");
	}

	public override BuildableDef EntityToBuild()
	{
		return ThingToInstall?.def;
	}

	public override ThingStyleDef EntityToBuildStyle()
	{
		return ThingToInstall?.StyleDef;
	}

	public override ThingDef EntityToBuildStuff()
	{
		return ThingToInstall?.Stuff;
	}

	public override List<ThingDefCountClass> TotalMaterialCost()
	{
		Log.Error("Called MaterialsNeededTotal on a Blueprint_Install.");
		return new List<ThingDefCountClass>();
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		map.listerBuildings.RegisterInstallBlueprint(this);
		if (buildingToReinstall != null)
		{
			overrideGraphicIndex = buildingToReinstall.overrideGraphicIndex ?? buildingToReinstall.thingIDNumber;
		}
		else if (miniToInstall != null)
		{
			overrideGraphicIndex = miniToInstall.InnerThing.overrideGraphicIndex ?? miniToInstall.InnerThing.thingIDNumber;
		}
		if (!respawningAfterLoad && !MiniToInstallOrBuildingToReinstall.BeingTransportedOnGravship && (MiniToInstallOrBuildingToReinstall.Destroyed || !MiniToInstallOrBuildingToReinstall.SpawnedOrAnyParentSpawned))
		{
			Destroy();
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		base.Map.listerBuildings.DeregisterInstallBlueprint(this);
		base.DeSpawn(mode);
	}

	protected override Thing MakeSolidThing(out bool shouldSelect)
	{
		Thing thingToInstall = ThingToInstall;
		if (miniToInstall != null)
		{
			foreach (Designation item in base.Map.designationManager.AllDesignationsOn(miniToInstall))
			{
				Designation newDes = new Designation(thingToInstall, item.def, item.colorDef);
				base.Map.designationManager.AddDesignation(newDes);
			}
			shouldSelect = Find.Selector.IsSelected(miniToInstall);
			miniToInstall.InnerThing = null;
			miniToInstall.Destroy();
		}
		else
		{
			shouldSelect = false;
		}
		return thingToInstall;
	}

	public override bool TryReplaceWithSolidThing(Pawn workerPawn, out Thing createdThing, out bool jobEnded)
	{
		Map map = base.Map;
		bool num = base.TryReplaceWithSolidThing(workerPawn, out createdThing, out jobEnded);
		if (num)
		{
			if (overrideGraphicIndex.HasValue)
			{
				createdThing.overrideGraphicIndex = overrideGraphicIndex;
			}
			if (createdThing is Plant)
			{
				SoundDefOf.Replant_Complete.PlayOneShot(new TargetInfo(base.Position, map));
			}
			else
			{
				SoundDefOf.Building_Complete.PlayOneShot(new TargetInfo(base.Position, map));
			}
			workerPawn.records.Increment(RecordDefOf.ThingsInstalled);
		}
		return num;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__20))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__20(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		if (buildingToReinstall != null)
		{
			GenDraw.DrawLineBetween(buildingToReinstall.TrueCenter(), this.TrueCenter());
		}
		if (ThingToInstall.def.drawPlaceWorkersWhileInstallBlueprintSelected && ThingToInstall.def.PlaceWorkers != null)
		{
			List<PlaceWorker> placeWorkers = ThingToInstall.def.PlaceWorkers;
			for (int i = 0; i < placeWorkers.Count; i++)
			{
				placeWorkers[i].DrawGhost(ThingToInstall.def, base.Position, base.Rotation, Color.white, ThingToInstall);
			}
		}
	}

	internal void SetThingToInstallFromMinified(MinifiedThing itemToInstall)
	{
		miniToInstall = itemToInstall;
		buildingToReinstall = null;
	}

	internal void SetBuildingToReinstall(Building buildingToReinstall)
	{
		if (!buildingToReinstall.def.Minifiable)
		{
			Log.Error("Tried to reinstall non-minifiable building.");
			return;
		}
		miniToInstall = null;
		this.buildingToReinstall = buildingToReinstall;
	}
}
