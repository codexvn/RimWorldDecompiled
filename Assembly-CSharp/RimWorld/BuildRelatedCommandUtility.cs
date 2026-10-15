using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class BuildRelatedCommandUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_0
	{
		public BuildableDef building;

		public Func<Ideo, bool> _003C_003E9__1;

		internal bool _003CRelatedBuildCommands_003Eb__1(Ideo i)
		{
			return _003CRelatedBuildCommands_003Eg__IdeoHasBuilding_007C0_0(i, (ThingDef)building);
		}
	}

	[CompilerGenerated]
	private sealed class _003CRelatedBuildCommands_003Ed__0 : IEnumerable<Command>, IEnumerable, IEnumerator<Command>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Command _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private BuildableDef building;

		public BuildableDef _003C_003E3__building;

		private _003C_003Ec__DisplayClass0_0 _003C_003E8__1;

		private ThingDef _003CbuildingDef_003E5__2;

		private IEnumerator<Command> _003C_003E7__wrap2;

		private List<ThingDef>.Enumerator _003C_003E7__wrap3;

		private List<TerrainDef>.Enumerator _003C_003E7__wrap4;

		Command IEnumerator<Command>.Current
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
		public _003CRelatedBuildCommands_003Ed__0(int _003C_003E1__state)
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
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E8__1 = null;
			_003CbuildingDef_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E7__wrap3 = default(List<ThingDef>.Enumerator);
			_003C_003E7__wrap4 = default(List<TerrainDef>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				List<ThingDef> list;
				List<TerrainDef> list2;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass0_0();
					_003C_003E8__1.building = building;
					_003C_003E7__wrap2 = BuildFacilityCommandUtility.BuildFacilityCommands(_003C_003E8__1.building).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0097;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0097;
				case 2:
					_003C_003E1__state = -4;
					goto IL_021a;
				case 3:
					{
						_003C_003E1__state = -5;
						goto IL_029f;
					}
					IL_0097:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Command current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					_003CbuildingDef_003E5__2 = _003C_003E8__1.building as ThingDef;
					if (_003CbuildingDef_003E5__2 == null)
					{
						return false;
					}
					list = _003CbuildingDef_003E5__2.building?.relatedBuildCommands;
					if (list != null)
					{
						_003C_003E7__wrap3 = list.GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_021a;
					}
					goto IL_023c;
					IL_029f:
					while (_003C_003E7__wrap4.MoveNext())
					{
						Designator_Build designator_Build = BuildCopyCommandUtility.FindAllowedDesignator(_003C_003E7__wrap4.Current);
						if (designator_Build != null)
						{
							_003C_003E2__current = designator_Build;
							_003C_003E1__state = 3;
							return true;
						}
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap4 = default(List<TerrainDef>.Enumerator);
					break;
					IL_021a:
					while (_003C_003E7__wrap3.MoveNext())
					{
						ThingDef current2 = _003C_003E7__wrap3.Current;
						if (ModsConfig.IdeologyActive && _003C_003E8__1.building.ideoBuilding && current2.ideoBuilding)
						{
							Ideo ideo = Find.FactionManager.OfPlayer.ideos.AllIdeos.FirstOrDefault((Ideo i) => _003CRelatedBuildCommands_003Eg__IdeoHasBuilding_007C0_0(i, (ThingDef)_003C_003E8__1.building));
							if (ideo != null)
							{
								bool flag = false;
								foreach (Ideo allIdeo in Find.FactionManager.OfPlayer.ideos.AllIdeos)
								{
									if (allIdeo == ideo && _003CRelatedBuildCommands_003Eg__IdeoHasBuilding_007C0_0(allIdeo, current2))
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									continue;
								}
							}
						}
						Designator_Build designator_Build2 = BuildCopyCommandUtility.FindAllowedDesignator(current2);
						if (designator_Build2 != null)
						{
							_003C_003E2__current = designator_Build2;
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = default(List<ThingDef>.Enumerator);
					goto IL_023c;
					IL_023c:
					list2 = _003CbuildingDef_003E5__2.building?.relatedTerrain;
					if (list2 == null)
					{
						break;
					}
					_003C_003E7__wrap4 = list2.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_029f;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
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
		IEnumerator<Command> IEnumerable<Command>.GetEnumerator()
		{
			_003CRelatedBuildCommands_003Ed__0 _003CRelatedBuildCommands_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRelatedBuildCommands_003Ed__ = this;
			}
			else
			{
				_003CRelatedBuildCommands_003Ed__ = new _003CRelatedBuildCommands_003Ed__0(0);
			}
			_003CRelatedBuildCommands_003Ed__.building = _003C_003E3__building;
			return _003CRelatedBuildCommands_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Command>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CRelatedBuildCommands_003Ed__0))]
	public static IEnumerable<Command> RelatedBuildCommands(BuildableDef building)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRelatedBuildCommands_003Ed__0(-2)
		{
			_003C_003E3__building = building
		};
	}

	[CompilerGenerated]
	internal static bool _003CRelatedBuildCommands_003Eg__IdeoHasBuilding_007C0_0(Ideo ideo, ThingDef td)
	{
		if (!ideo.HasPreceptForBuilding(td))
		{
			return ideo.PreceptsListForReading.Any((Precept p) => p is Precept_RitualSeat precept_RitualSeat && precept_RitualSeat.ThingDef == td);
		}
		return true;
	}
}
