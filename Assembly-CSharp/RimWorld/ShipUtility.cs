using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public static class ShipUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public KeyValuePair<ThingDef, int> partDef;

		internal bool _003CLaunchFailReasons_003Eb__0(Building pa)
		{
			return pa.def == partDef.Key;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_0
	{
		public Building building;

		public Action _003C_003E9__1;

		internal void _003CShipStartupGizmos_003Eb__0()
		{
			string text = "HibernateWarning";
			if (building.Map.info.parent.GetComponent<EscapeShipComp>() == null)
			{
				text += "Standalone";
			}
			if (!Find.Storyteller.difficulty.allowBigThreats)
			{
				text += "Pacifist";
			}
			DiaNode diaNode = new DiaNode(text.Translate());
			DiaOption item = new DiaOption("Confirm".Translate())
			{
				action = delegate
				{
					StartupHibernatingParts(building);
				},
				resolveTree = true
			};
			diaNode.options.Add(item);
			DiaOption item2 = new DiaOption("GoBack".Translate())
			{
				resolveTree = true
			};
			diaNode.options.Add(item2);
			Find.WindowStack.Add(new Dialog_NodeTree(diaNode, delayInteractivity: true));
		}

		internal void _003CShipStartupGizmos_003Eb__1()
		{
			StartupHibernatingParts(building);
		}
	}

	[CompilerGenerated]
	private sealed class _003CLaunchFailReasons_003Ed__2 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Building rootBuilding;

		public Building _003C_003E3__rootBuilding;

		private List<Building> _003CshipParts_003E5__2;

		private bool _003CfullPodFound_003E5__3;

		private Dictionary<ThingDef, int>.Enumerator _003C_003E7__wrap3;

		private List<Building>.Enumerator _003C_003E7__wrap4;

		string IEnumerator<string>.Current
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
		public _003CLaunchFailReasons_003Ed__2(int _003C_003E1__state)
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
			_003CshipParts_003E5__2 = null;
			_003C_003E7__wrap3 = default(Dictionary<ThingDef, int>.Enumerator);
			_003C_003E7__wrap4 = default(List<Building>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CshipParts_003E5__2 = ShipBuildingsAttachedTo(rootBuilding).ToList();
					_003C_003E7__wrap3 = RequiredParts().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_012d;
				case 1:
					_003C_003E1__state = -3;
					goto IL_012d;
				case 2:
					_003C_003E1__state = -4;
					goto IL_027e;
				case 3:
					_003C_003E1__state = -4;
					goto IL_027e;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_012d:
					while (_003C_003E7__wrap3.MoveNext())
					{
						_003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass2_0
						{
							partDef = _003C_003E7__wrap3.Current
						};
						int num = _003CshipParts_003E5__2.Count((Building pa) => pa.def == CS_0024_003C_003E8__locals5.partDef.Key);
						if (num < CS_0024_003C_003E8__locals5.partDef.Value)
						{
							_003C_003E2__current = string.Format("{0}: {1}x {2} ({3} {4})", "ShipReportMissingPart".Translate(), CS_0024_003C_003E8__locals5.partDef.Value - num, CS_0024_003C_003E8__locals5.partDef.Key.label, "ShipReportMissingPartRequires".Translate(), CS_0024_003C_003E8__locals5.partDef.Value);
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap3 = default(Dictionary<ThingDef, int>.Enumerator);
					_003CfullPodFound_003E5__3 = false;
					foreach (Building item in _003CshipParts_003E5__2)
					{
						if (item.def == ThingDefOf.Ship_CryptosleepCasket && item is Building_CryptosleepCasket { HasAnyContents: not false })
						{
							_003CfullPodFound_003E5__3 = true;
							break;
						}
					}
					_003C_003E7__wrap4 = _003CshipParts_003E5__2.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_027e;
					IL_027e:
					while (_003C_003E7__wrap4.MoveNext())
					{
						Building current2 = _003C_003E7__wrap4.Current;
						CompHibernatable compHibernatable = current2.TryGetComp<CompHibernatable>();
						if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
						{
							_003C_003E2__current = string.Format("{0}: {1}", "ShipReportHibernating".Translate(), current2.LabelCap);
							_003C_003E1__state = 2;
							return true;
						}
						if (compHibernatable != null && !compHibernatable.Running)
						{
							_003C_003E2__current = string.Format("{0}: {1}", "ShipReportNotReady".Translate(), current2.LabelCap);
							_003C_003E1__state = 3;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = default(List<Building>.Enumerator);
					if (!_003CfullPodFound_003E5__3)
					{
						_003C_003E2__current = "ShipReportNoFullPods".Translate();
						_003C_003E1__state = 4;
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
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap4/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CLaunchFailReasons_003Ed__2 _003CLaunchFailReasons_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CLaunchFailReasons_003Ed__ = this;
			}
			else
			{
				_003CLaunchFailReasons_003Ed__ = new _003CLaunchFailReasons_003Ed__2(0);
			}
			_003CLaunchFailReasons_003Ed__.rootBuilding = _003C_003E3__rootBuilding;
			return _003CLaunchFailReasons_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CShipStartupGizmos_003Ed__8 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Building building;

		public Building _003C_003E3__building;

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
		public _003CShipStartupGizmos_003Ed__8(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass8_0 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass8_0
				{
					building = building
				};
				if (!HasHibernatingParts(CS_0024_003C_003E8__locals3.building))
				{
					break;
				}
				_003C_003E2__current = new Command_Action
				{
					action = delegate
					{
						string text = "HibernateWarning";
						if (CS_0024_003C_003E8__locals3.building.Map.info.parent.GetComponent<EscapeShipComp>() == null)
						{
							text += "Standalone";
						}
						if (!Find.Storyteller.difficulty.allowBigThreats)
						{
							text += "Pacifist";
						}
						DiaNode diaNode = new DiaNode(text.Translate());
						DiaOption item = new DiaOption("Confirm".Translate())
						{
							action = delegate
							{
								StartupHibernatingParts(CS_0024_003C_003E8__locals3.building);
							},
							resolveTree = true
						};
						diaNode.options.Add(item);
						DiaOption item2 = new DiaOption("GoBack".Translate())
						{
							resolveTree = true
						};
						diaNode.options.Add(item2);
						Find.WindowStack.Add(new Dialog_NodeTree(diaNode, delayInteractivity: true));
					},
					defaultLabel = "CommandShipStartup".Translate(),
					defaultDesc = "CommandShipStartupDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc1,
					icon = ContentFinder<Texture2D>.Get("UI/Commands/DesirePower")
				};
				_003C_003E1__state = 1;
				return true;
			}
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
			_003CShipStartupGizmos_003Ed__8 _003CShipStartupGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CShipStartupGizmos_003Ed__ = this;
			}
			else
			{
				_003CShipStartupGizmos_003Ed__ = new _003CShipStartupGizmos_003Ed__8(0);
			}
			_003CShipStartupGizmos_003Ed__.building = _003C_003E3__building;
			return _003CShipStartupGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private static Dictionary<ThingDef, int> requiredParts;

	private static List<Building> closedSet = new List<Building>();

	private static List<Building> openSet = new List<Building>();

	public static Dictionary<ThingDef, int> RequiredParts()
	{
		if (requiredParts == null)
		{
			requiredParts = new Dictionary<ThingDef, int>();
			requiredParts[ThingDefOf.Ship_CryptosleepCasket] = 1;
			requiredParts[ThingDefOf.Ship_ComputerCore] = 1;
			requiredParts[ThingDefOf.Ship_Reactor] = 1;
			requiredParts[ThingDefOf.Ship_Engine] = 3;
			requiredParts[ThingDefOf.Ship_Beam] = 1;
			requiredParts[ThingDefOf.Ship_SensorCluster] = 1;
		}
		return requiredParts;
	}

	[IteratorStateMachine(typeof(_003CLaunchFailReasons_003Ed__2))]
	public static IEnumerable<string> LaunchFailReasons(Building rootBuilding)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CLaunchFailReasons_003Ed__2(-2)
		{
			_003C_003E3__rootBuilding = rootBuilding
		};
	}

	public static bool HasHibernatingParts(Building rootBuilding)
	{
		foreach (Building item in ShipBuildingsAttachedTo(rootBuilding).ToList())
		{
			CompHibernatable compHibernatable = item.TryGetComp<CompHibernatable>();
			if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
			{
				return true;
			}
		}
		return false;
	}

	public static void StartupHibernatingParts(Building rootBuilding)
	{
		foreach (Building item in ShipBuildingsAttachedTo(rootBuilding).ToList())
		{
			CompHibernatable compHibernatable = item.TryGetComp<CompHibernatable>();
			if (compHibernatable != null && compHibernatable.State == HibernatableStateDefOf.Hibernating)
			{
				compHibernatable.Startup();
			}
		}
		SoundDefOf.ShipReactor_Startup.PlayOneShot(SoundInfo.InMap(rootBuilding));
	}

	public static List<Building> ShipBuildingsAttachedTo(Building root)
	{
		closedSet.Clear();
		if (root == null || root.Destroyed)
		{
			return closedSet;
		}
		openSet.Clear();
		openSet.Add(root);
		while (openSet.Count > 0)
		{
			Building building = openSet[openSet.Count - 1];
			openSet.Remove(building);
			closedSet.Add(building);
			foreach (IntVec3 item in GenAdj.CellsAdjacentCardinal(building))
			{
				Building edifice = item.GetEdifice(building.Map);
				if (edifice != null && edifice.def.building.shipPart && !closedSet.Contains(edifice) && !openSet.Contains(edifice))
				{
					openSet.Add(edifice);
				}
			}
		}
		return closedSet;
	}

	[IteratorStateMachine(typeof(_003CShipStartupGizmos_003Ed__8))]
	public static IEnumerable<Gizmo> ShipStartupGizmos(Building building)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CShipStartupGizmos_003Ed__8(-2)
		{
			_003C_003E3__building = building
		};
	}
}
