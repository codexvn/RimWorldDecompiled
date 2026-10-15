using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet;

public class ArchotechSettlement : Settlement
{
	[CompilerGenerated]
	private sealed class _003Cget_AllStudiableArchotechBuildings_003Ed__2 : IEnumerable<Building>, IEnumerable, IEnumerator<Building>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Building _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ArchotechSettlement _003C_003E4__this;

		private IEnumerator<Building> _003C_003E7__wrap1;

		Building IEnumerator<Building>.Current
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
		public _003Cget_AllStudiableArchotechBuildings_003Ed__2(int _003C_003E1__state)
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
			case 2:
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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ArchotechSettlement archotechSettlement = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = archotechSettlement.Map.listerBuildings.AllBuildingsNonColonistOfDef(ThingDefOf.GrandArchotechStructure).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0081;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0081;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0081:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Building current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = archotechSettlement.Map.listerBuildings.AllBuildingsNonColonistOfDef(ThingDefOf.MajorArchotechStructureStudiable).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Building current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap1 = null;
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
		IEnumerator<Building> IEnumerable<Building>.GetEnumerator()
		{
			_003Cget_AllStudiableArchotechBuildings_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllStudiableArchotechBuildings_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Building>)this).GetEnumerator();
		}
	}

	public List<Building> existingBuildings = new List<Building>();

	private IEnumerable<Building> AllStudiableArchotechBuildings
	{
		[IteratorStateMachine(typeof(_003Cget_AllStudiableArchotechBuildings_003Ed__2))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllStudiableArchotechBuildings_003Ed__2(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public bool AnyArchotechBuildingRequiresStudy
	{
		get
		{
			foreach (Building allStudiableArchotechBuilding in AllStudiableArchotechBuildings)
			{
				CompStudiable compStudiable = allStudiableArchotechBuilding.TryGetComp<CompStudiable>();
				if (compStudiable != null && !compStudiable.Completed)
				{
					return true;
				}
			}
			return false;
		}
	}

	public override void Abandon(bool wasGravshipLaunch)
	{
		AbandonedArchotechStructures abandonedArchotechStructures = (AbandonedArchotechStructures)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedArchotechStructures);
		abandonedArchotechStructures.Tile = base.Tile;
		abandonedArchotechStructures.worldObjectDef = def;
		abandonedArchotechStructures.archotechStructures.AddRange(AllStudiableArchotechBuildings);
		foreach (Building archotechStructure in abandonedArchotechStructures.archotechStructures)
		{
			archotechStructure.DeSpawn();
		}
		Find.WorldObjects.Add(abandonedArchotechStructures);
		Find.LetterStack.ReceiveLetter("LetterLabelArchotechStructuresAbandoned".Translate(), "LetterTextArchotechStructuresAbandoned".Translate(this), LetterDefOf.NeutralEvent, abandonedArchotechStructures);
		Destroy();
	}

	public override void PostMapGenerate()
	{
		base.PostMapGenerate();
		foreach (Building existingBuilding in existingBuildings)
		{
			Building building = base.Map.listerBuildings.AllBuildingsNonColonistOfDef(existingBuilding.def).FirstOrDefault((Building b) => !existingBuildings.Contains(b));
			if (building != null)
			{
				IntVec3 position = building.Position;
				building.DeSpawn();
				GenSpawn.Spawn(existingBuilding, position, base.Map);
			}
			else
			{
				existingBuilding.Destroy();
			}
		}
		existingBuildings.Clear();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref existingBuildings, "existingBuildings", LookMode.Deep);
	}
}
