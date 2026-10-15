using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public abstract class Blueprint : ThingWithComps, IConstructible
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__6 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Blueprint _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Blueprint blueprint = _003C_003E4__this;
				Gizmo selectMonumentMarkerGizmo;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)blueprint).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0071;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0071;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0071:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					selectMonumentMarkerGizmo = QuestUtility.GetSelectMonumentMarkerGizmo(blueprint);
					if (selectMonumentMarkerGizmo != null)
					{
						_003C_003E2__current = selectMonumentMarkerGizmo;
						_003C_003E1__state = 2;
						return true;
					}
					break;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__6(0)
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

	public override string Label => def.entityDefToBuild.label + "BlueprintLabelExtra".Translate();

	protected abstract float WorkTotal { get; }

	public override Graphic Graphic
	{
		get
		{
			ThingStyleDef styleDef = StyleDef;
			if (styleDef?.blueprintGraphicData != null)
			{
				if (styleGraphicInt == null)
				{
					styleGraphicInt = styleDef.blueprintGraphicData.Graphic;
				}
				return styleGraphicInt;
			}
			return base.Graphic;
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__6))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}

	public virtual bool TryReplaceWithSolidThing(Pawn workerPawn, out Thing createdThing, out bool jobEnded)
	{
		bool flag = Find.Selector.IsSelected(this);
		jobEnded = false;
		if (GenConstruct.FirstBlockingThing(this, workerPawn) != null)
		{
			workerPawn.jobs.EndCurrentJob(JobCondition.Incompletable);
			jobEnded = true;
			createdThing = null;
			return false;
		}
		createdThing = MakeSolidThing(out var shouldSelect);
		Map map = base.Map;
		if (Graphic is Graphic_Random)
		{
			createdThing.overrideGraphicIndex = thingIDNumber;
		}
		GenSpawn.WipeExistingThings(base.Position, base.Rotation, createdThing.def, map, DestroyMode.Deconstruct);
		if (createdThing is Plant)
		{
			base.Position.GetPlant(base.Map)?.Destroy();
		}
		if (!base.Destroyed)
		{
			Destroy();
		}
		if (createdThing.def.CanHaveFaction)
		{
			createdThing.SetFactionDirect(workerPawn.Faction);
		}
		Thing thing = GenSpawn.Spawn(createdThing, base.Position, map, base.Rotation);
		if ((shouldSelect || flag) && thing != null)
		{
			Find.Selector.Select(thing, playSound: false, forceDesignatorDeselect: false);
		}
		foreach (Pawn item in map.mapPawns.AllPawnsSpawned)
		{
			item.pather.NotifyThingTransformed(this, thing);
		}
		foreach (IntVec3 cell in thing.OccupiedRect().Cells)
		{
			if (map.planManager.TryGetPlan(cell, out var plan))
			{
				plan.RemoveCell(cell);
			}
		}
		return true;
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		if (!WorldComponent_GravshipController.CutsceneInProgress || WorldComponent_GravshipController.GravshipRenderInProgess)
		{
			base.DrawAt(drawLoc, flip);
		}
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		map.blueprintGrid.Register(this);
		base.SpawnSetup(map, respawningAfterLoad);
	}

	public void InheritStyle(Precept_ThingStyle styleSource, ThingStyleDef styleDef)
	{
		base.StyleSourcePrecept = styleSource;
		if (styleSource == null)
		{
			StyleDef = styleDef;
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		Map map = base.Map;
		base.DeSpawn(mode);
		map.blueprintGrid.DeRegister(this);
	}

	protected abstract Thing MakeSolidThing(out bool shouldSelect);

	public abstract List<ThingDefCountClass> TotalMaterialCost();

	public bool IsCompleted()
	{
		return false;
	}

	public int ThingCountNeeded(ThingDef stuff)
	{
		foreach (ThingDefCountClass item in TotalMaterialCost())
		{
			if (item.thingDef == stuff)
			{
				return item.count;
			}
		}
		return 0;
	}

	public int SpaceRemainingFor(ThingDef stuff)
	{
		return ThingCountNeeded(stuff);
	}

	public abstract BuildableDef EntityToBuild();

	public abstract ThingStyleDef EntityToBuildStyle();

	public abstract ThingDef EntityToBuildStuff();

	public Thing BlockingHaulableOnTop()
	{
		if (def.entityDefToBuild.passability == Traversability.Standable)
		{
			return null;
		}
		foreach (IntVec3 item in this.OccupiedRect())
		{
			List<Thing> thingList = item.GetThingList(base.Map);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing.def.EverHaulable)
				{
					return thing;
				}
			}
		}
		return null;
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		stringBuilder.AppendLineIfNotEmpty();
		float workTotal = WorkTotal;
		if (!Mathf.Approximately(workTotal, -1f))
		{
			stringBuilder.Append("WorkLeft".Translate() + ": " + workTotal.ToStringWorkAmount());
		}
		return stringBuilder.ToString();
	}
}
