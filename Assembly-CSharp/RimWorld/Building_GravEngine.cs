using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Building_GravEngine : Building, IRenameable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass80_0
	{
		public Pawn selPawn;

		public Building_GravEngine _003C_003E4__this;

		internal void _003CGetFloatMenuOptions_003Eb__0()
		{
			selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.InspectGravEngine, _003C_003E4__this), JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__80 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		public Building_GravEngine _003C_003E4__this;

		private _003C_003Ec__DisplayClass80_0 _003C_003E8__1;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetFloatMenuOptions_003Ed__80(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_GravEngine building_GravEngine = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass80_0();
					_003C_003E8__1.selPawn = selPawn;
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E7__wrap1 = ((ThingWithComps)building_GravEngine).GetFloatMenuOptions(_003C_003E8__1.selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00aa;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00aa;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00aa:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!Find.ResearchManager.gravEngineInspected)
					{
						_003C_003E2__current = new FloatMenuOption("CommandInspectGravEngine".Translate(building_GravEngine), delegate
						{
							_003C_003E8__1.selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.InspectGravEngine, _003C_003E8__1._003C_003E4__this), JobTag.Misc);
						});
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__80 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__80(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__79 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_GravEngine _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__79(int _003C_003E1__state)
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
				Building_GravEngine CS_0024_003C_003E8__locals6 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals6).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Inspect now",
							action = delegate
							{
								CS_0024_003C_003E8__locals6.Inspect();
							}
						};
						_003C_003E1__state = 3;
						return true;
					}
					break;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!CS_0024_003C_003E8__locals6.Spawned)
					{
						return false;
					}
					if (Find.ResearchManager.gravEngineInspected)
					{
						break;
					}
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "CommandInspectGravEngine".Translate(CS_0024_003C_003E8__locals6),
						defaultDesc = "CommandInspectGravEngineDesc".Translate(),
						icon = InspectCommandTex,
						action = delegate
						{
							Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(), delegate(LocalTargetInfo targ)
							{
								if (targ.Thing is Pawn pawn)
								{
									pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.InspectGravEngine, CS_0024_003C_003E8__locals6), JobTag.Misc);
								}
							}, delegate(LocalTargetInfo targ)
							{
								if (_003CGetGizmos_003Eg__ValidateInspectionTarget_007C79_0(targ))
								{
									GenDraw.DrawTargetHighlight(targ);
								}
							}, _003CGetGizmos_003Eg__ValidateInspectionTarget_007C79_0, null, null, InspectCommandTex, playSoundOnAction: true, delegate
							{
								Widgets.MouseAttachedLabel("ChooseWhoShouldInspect".Translate(CS_0024_003C_003E8__locals6), 0f, 0f, null);
							});
						}
					};
					_003C_003E1__state = 2;
					return true;
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
			_003CGetGizmos_003Ed__79 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__79(0)
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

	[CompilerGenerated]
	private sealed class _003CGetOrbitalWarnings_003Ed__83 : IEnumerable<TaggedString>, IEnumerable, IEnumerator<TaggedString>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TaggedString _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_GravEngine _003C_003E4__this;

		private int _003CheatPusherCount_003E5__2;

		private int _003CoxygenPusherCount_003E5__3;

		private string _003CbaseWarning_003E5__4;

		TaggedString IEnumerator<TaggedString>.Current
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
		public _003CGetOrbitalWarnings_003Ed__83(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CbaseWarning_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Building_GravEngine building_GravEngine = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				bool flag = true;
				_003CheatPusherCount_003E5__2 = 0;
				_003CoxygenPusherCount_003E5__3 = 0;
				building_GravEngine.tmpGravshipRooms.Clear();
				foreach (IntVec3 item in building_GravEngine.validSubstructure)
				{
					foreach (Thing thing in item.GetThingList(building_GravEngine.Map))
					{
						if (thing.HasComp<CompOxygenPusher>())
						{
							_003CoxygenPusherCount_003E5__3++;
						}
						if (thing is Building_Heater || (thing.TryGetComp(out CompHeatPusher comp) && comp.Props.heatPerSecond > 20f))
						{
							_003CheatPusherCount_003E5__2++;
						}
					}
					Room room = item.GetRoom(building_GravEngine.Map);
					if (room != null && !building_GravEngine.tmpGravshipRooms.Contains(room) && !room.UsesOutdoorTemperature && !room.IsDoorway)
					{
						building_GravEngine.tmpGravshipRooms.Add(room);
						if (!VacuumUtility.IsRoomAirtight(room))
						{
							flag = false;
						}
					}
				}
				_003CbaseWarning_003E5__4 = ("OrbitalWarning_Warning".Translate() + ": ").Colorize(ColorLibrary.RedReadable);
				if (!flag)
				{
					_003C_003E2__current = _003CbaseWarning_003E5__4 + "OrbitalWarning_Vacuum".Translate();
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_01b1;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_01b1;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0246;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0246;
			case 4:
				_003C_003E1__state = -1;
				break;
			case 5:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0246:
				if (_003CoxygenPusherCount_003E5__3 == 0)
				{
					_003C_003E2__current = _003CbaseWarning_003E5__4 + "OrbitalWarning_NoOxygenPumps".Translate();
					_003C_003E1__state = 4;
					return true;
				}
				if ((float)_003CoxygenPusherCount_003E5__3 < (float)building_GravEngine.ValidSubstructure.Count / 400f)
				{
					_003C_003E2__current = _003CbaseWarning_003E5__4 + "OrbitalWarning_FewOxygenPumps".Translate() + ": " + _003CoxygenPusherCount_003E5__3.ToString();
					_003C_003E1__state = 5;
					return true;
				}
				break;
				IL_01b1:
				if (_003CheatPusherCount_003E5__2 == 0)
				{
					_003C_003E2__current = _003CbaseWarning_003E5__4 + "OrbitalWarning_NoHeatSources".Translate();
					_003C_003E1__state = 2;
					return true;
				}
				if ((float)_003CheatPusherCount_003E5__2 < (float)building_GravEngine.ValidSubstructure.Count / 250f)
				{
					_003C_003E2__current = _003CbaseWarning_003E5__4 + "OrbitalWarning_FewHeatSources".Translate() + ": " + _003CheatPusherCount_003E5__2.ToString();
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_0246;
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
		IEnumerator<TaggedString> IEnumerable<TaggedString>.GetEnumerator()
		{
			_003CGetOrbitalWarnings_003Ed__83 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetOrbitalWarnings_003Ed__83(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TaggedString>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__81 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_GravEngine _003C_003E4__this;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__81(int _003C_003E1__state)
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
				Building_GravEngine building_GravEngine = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)building_GravEngine).SpecialDisplayStats().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (building_GravEngine.FuelSavingsPercent > 0f)
					{
						_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Building, "StatsReport_GravEngineFuelConsumption".Translate(), (0f - building_GravEngine.FuelSavingsPercent).ToStringPercentSigned("F0"), "StatsReport_GravEngineFuelConsumption_Desc".Translate(), 520);
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__81 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__81(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public int cooldownCompleteTick = -1;

	public bool silentlyActivate;

	public bool nameHidden = true;

	private string gravshipName = "Gravship".Translate();

	public HashSet<Pawn> pawnsToBoard;

	public HashSet<Pawn> pawnsToLeave;

	public LaunchInfo launchInfo;

	[Unsaved(false)]
	private CompAffectedByFacilities affectedByFacilities;

	[Unsaved(false)]
	private List<CompGravshipFacility> gravshipComponents = new List<CompGravshipFacility>();

	[Unsaved(false)]
	private HashSet<IntVec3> validSubstructure = new HashSet<IntVec3>();

	[Unsaved(false)]
	private HashSet<IntVec3> allConnectedSubstructure = new HashSet<IntVec3>();

	[Unsaved(false)]
	private List<GravshipComponentTypeDef> missingComponents = new List<GravshipComponentTypeDef>();

	[Unsaved(false)]
	public bool substructureDirty = true;

	[Unsaved(false)]
	private bool missingComponentsDirty = true;

	[Unsaved(false)]
	private bool gravshipComponentsDirty = true;

	[Unsaved(false)]
	private bool haveShownNameDialog;

	[Unsaved(false)]
	private bool hasTickedThisSession;

	private static readonly Texture2D InspectCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Inspect");

	private static readonly CachedMaterial OrbMat = new CachedMaterial("Things/Building/GravEngine/GravEngine_Orb", ShaderDatabase.Cutout);

	private static Graphic onCooldownGraphic;

	private const float BaseFuelPerTile = 10f;

	private const float BobHeight = 0.3f;

	private const int NameDialogSubstructureThreshold = 90;

	private HashSet<Room> tmpGravshipRooms = new HashSet<Room>();

	private static Graphic OnCooldownGraphic => onCooldownGraphic ?? (onCooldownGraphic = GraphicDatabase.Get<Graphic_Single>("Things/Building/GravEngine/GravEngine_Cooldown", ShaderDatabase.Cutout, Vector2.one * 3f, Color.white));

	public string RenamableLabel
	{
		get
		{
			return gravshipName ?? BaseLabel;
		}
		set
		{
			gravshipName = value;
			nameHidden = false;
		}
	}

	public string BaseLabel => def.LabelCap;

	public string InspectLabel => BaseLabel;

	public override Graphic Graphic
	{
		get
		{
			if (Find.TickManager.TicksGame >= cooldownCompleteTick)
			{
				return base.Graphic;
			}
			return OnCooldownGraphic;
		}
	}

	public CompAffectedByFacilities AffectedByFacilities => affectedByFacilities ?? (affectedByFacilities = GetComp<CompAffectedByFacilities>());

	public int MaxLaunchDistance => (int)this.GetStatValue(StatDefOf.GravshipRange);

	public HashSet<IntVec3> ValidSubstructure
	{
		get
		{
			UpdateSubstructureIfNeeded(regenerateSectionLayers: true);
			return validSubstructure;
		}
	}

	public HashSet<IntVec3> AllConnectedSubstructure
	{
		get
		{
			UpdateSubstructureIfNeeded(regenerateSectionLayers: true);
			return allConnectedSubstructure;
		}
	}

	public HashSet<IntVec3> ValidSubstructureNoRegen
	{
		get
		{
			UpdateSubstructureIfNeeded(regenerateSectionLayers: false);
			return validSubstructure;
		}
	}

	public HashSet<IntVec3> AllConnectedSubstructureNoRegen
	{
		get
		{
			UpdateSubstructureIfNeeded(regenerateSectionLayers: false);
			return allConnectedSubstructure;
		}
	}

	public List<CompGravshipFacility> GravshipComponents
	{
		get
		{
			if (gravshipComponentsDirty)
			{
				gravshipComponentsDirty = false;
				gravshipComponents.Clear();
				foreach (Thing item in AffectedByFacilities.LinkedFacilitiesListForReading)
				{
					if (item.TryGetComp(out CompGravshipFacility comp))
					{
						gravshipComponents.Add(comp);
					}
				}
			}
			return gravshipComponents;
		}
	}

	public float TotalFuel
	{
		get
		{
			float num = 0f;
			foreach (CompGravshipFacility gravshipComponent in GravshipComponents)
			{
				if (gravshipComponent.parent.Spawned && gravshipComponent.Props.providesFuel && gravshipComponent.CanBeActive && gravshipComponent.parent.TryGetComp<CompRefuelable>(out var comp))
				{
					num += comp.Fuel;
				}
			}
			return num;
		}
	}

	public float MaxFuel
	{
		get
		{
			float num = 0f;
			foreach (CompGravshipFacility gravshipComponent in GravshipComponents)
			{
				if (gravshipComponent.parent.Spawned && gravshipComponent.Props.providesFuel && gravshipComponent.CanBeActive && gravshipComponent.parent.TryGetComp<CompRefuelable>(out var comp))
				{
					num += comp.Props.fuelCapacity;
				}
			}
			return num;
		}
	}

	public float FuelSavingsPercent
	{
		get
		{
			float num = 0f;
			foreach (CompGravshipFacility gravshipComponent in GravshipComponents)
			{
				if (gravshipComponent.parent.Spawned && gravshipComponent.Props.fuelSavingsPercent > 0f && gravshipComponent.CanBeActive)
				{
					num += gravshipComponent.Props.fuelSavingsPercent;
				}
			}
			return Mathf.Clamp01(num);
		}
	}

	public float FuelPerTile => 10f * FuelUseageFactor;

	public float FuelUseageFactor => 1f - FuelSavingsPercent;

	public List<GravshipComponentTypeDef> MissingComponents
	{
		get
		{
			if (missingComponentsDirty)
			{
				missingComponentsDirty = false;
				missingComponents.Clear();
				foreach (GravshipComponentTypeDef def in DefDatabase<GravshipComponentTypeDef>.AllDefsListForReading)
				{
					if (def.requiredForLaunch && GravshipComponents.Find((CompGravshipFacility c) => c.Props.componentTypeDef == def) == null)
					{
						missingComponents.Add(def);
					}
				}
			}
			return missingComponents;
		}
	}

	public bool HasSignalJammer => GravshipComponents.Any((CompGravshipFacility c) => c.Props.componentTypeDef == GravshipComponentTypeDefOf.SignalJammer);

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		if (ModsConfig.OdysseyActive)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			ForceSubstructureDirty();
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		base.DeSpawn(mode);
		missingComponents.Clear();
		ForceSubstructureDirty();
	}

	public override void PostSwapMap()
	{
		base.PostSwapMap();
		ForceSubstructureDirty();
	}

	protected override void Tick()
	{
		base.Tick();
		hasTickedThisSession = true;
		if (silentlyActivate)
		{
			Inspect(silent: true);
			silentlyActivate = false;
		}
		if (Find.TickManager.TicksGame == cooldownCompleteTick)
		{
			Messages.Message("GravshipReadyToLaunch".Translate(RenamableLabel), this, MessageTypeDefOf.NeutralEvent, historical: false);
		}
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		base.DrawAt(drawLoc, flip);
		if (base.Spawned)
		{
			if (Find.TickManager.TicksGame >= cooldownCompleteTick)
			{
				drawLoc.z += 0.5f * (1f + Mathf.Sin((float)Math.PI * 2f * (float)GenTicks.TicksGame / 500f)) * 0.3f;
			}
			drawLoc.y += 0.03658537f;
			Vector3 s = new Vector3(def.graphicData.drawSize.x, 1f, def.graphicData.drawSize.y);
			Graphics.DrawMesh(MeshPool.plane10Back, Matrix4x4.TRS(drawLoc, base.Rotation.AsQuat, s), OrbMat.Material, 0, null, 0);
		}
	}

	public void AddComponent(CompGravshipFacility comp)
	{
		ForceSubstructureDirty();
	}

	public void RemoveComponent(CompGravshipFacility comp)
	{
		ForceSubstructureDirty();
	}

	public AcceptanceReport CanLaunch(CompPilotConsole console)
	{
		Map map = base.Map;
		if (map != null && map.generatorDef?.pocketMapProperties?.canLaunchGravship == false)
		{
			return new AcceptanceReport(base.Map.generatorDef.label);
		}
		if (ValidSubstructure.Count == 0)
		{
			return new AcceptanceReport("CannotLaunchNoSubstructure".Translate().CapitalizeFirst());
		}
		if (!console.CanBeActive)
		{
			return new AcceptanceReport("CannotLaunchDisconnected".Translate().CapitalizeFirst());
		}
		if (TotalFuel < 10f)
		{
			return new AcceptanceReport("CannotLaunchNotEnoughFuel".Translate().CapitalizeFirst());
		}
		if (MaxLaunchDistance <= 0)
		{
			return new AcceptanceReport("CannotLaunchNoThrusters".Translate().CapitalizeFirst());
		}
		if (GenTicks.TicksGame < cooldownCompleteTick)
		{
			return new AcceptanceReport("CannotLaunchOnCooldown".Translate((cooldownCompleteTick - GenTicks.TicksGame).ToStringTicksToPeriod()).CapitalizeFirst());
		}
		return AcceptanceReport.WasAccepted;
	}

	public void ConsumeFuel(PlanetTile tile)
	{
		if (!GravshipUtility.TryGetPathFuelCost(base.Map.Tile, tile, out var cost, out var _))
		{
			Log.Error($"Failed to get the fuel cost from tile ({base.Map.Tile}) to {tile}.");
			return;
		}
		float num = cost / TotalFuel;
		foreach (CompGravshipFacility gravshipComponent in GravshipComponents)
		{
			if (gravshipComponent.CanBeActive && gravshipComponent.Props.providesFuel)
			{
				CompRefuelable comp = gravshipComponent.parent.GetComp<CompRefuelable>();
				comp?.ConsumeFuel(comp.Fuel * num);
			}
		}
		cooldownCompleteTick = GenTicks.TicksGame + (int)GravshipUtility.LaunchCooldownFromQuality(launchInfo?.quality ?? 1f);
	}

	public bool ValidSubstructureAt(IntVec3 cell)
	{
		return ValidSubstructure.Contains(cell);
	}

	public bool OnValidSubstructure(Thing thing)
	{
		if (!thing.Spawned)
		{
			return false;
		}
		BuildingProperties building = thing.def.building;
		if (building != null && building.isAttachment)
		{
			Thing wallAttachedTo = GenConstruct.GetWallAttachedTo(thing);
			return ValidSubstructureAt(wallAttachedTo.Position);
		}
		foreach (IntVec3 item in thing.OccupiedRect())
		{
			if (!ValidSubstructureAt(item))
			{
				return false;
			}
		}
		return true;
	}

	public bool LooselyConnectedToGravEngine(Thing thing)
	{
		if (!thing.Spawned)
		{
			return false;
		}
		foreach (IntVec3 item in thing.OccupiedRect())
		{
			if (!AllConnectedSubstructure.Contains(item))
			{
				return false;
			}
		}
		return true;
	}

	public void Inspect(bool silent = false)
	{
		if (Find.ResearchManager.gravEngineInspected)
		{
			return;
		}
		Find.ResearchManager.gravEngineInspected = true;
		QuestUtility.SendQuestTargetSignals(questTags, "Inspected", this.Named("SUBJECT"));
		LessonAutoActivator.TeachOpportunity(ConceptDefOf.Gravship, OpportunityType.Important);
		if (base.Faction != Faction.OfPlayer)
		{
			SetFaction(Faction.OfPlayer);
		}
		Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.GravEngine, StorytellerUtility.DefaultThreatPointsNow(base.Map));
		if (!silent)
		{
			DiaNode diaNode = new DiaNode("GravEngineInspectedLetterContents".Translate());
			DiaOption item = new DiaOption("ViewQuest".Translate())
			{
				resolveTree = true,
				action = delegate
				{
					Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Quests);
					((MainTabWindow_Quests)MainButtonDefOf.Quests.TabWindow).Select(quest);
				}
			};
			diaNode.options.Add(item);
			diaNode.options.Add(new DiaOption("Close".Translate())
			{
				resolveTree = true
			});
			Dialog_NodeTree dialog_NodeTree = new Dialog_NodeTree(diaNode);
			dialog_NodeTree.forcePause = true;
			Find.WindowStack.Add(dialog_NodeTree);
		}
	}

	public void ForceSubstructureDirty()
	{
		substructureDirty = true;
		missingComponentsDirty = true;
		gravshipComponentsDirty = true;
	}

	private void UpdateSubstructureIfNeeded(bool regenerateSectionLayers)
	{
		if (!ModsConfig.OdysseyActive || base.Destroyed || !base.Spawned)
		{
			return;
		}
		if (substructureDirty)
		{
			substructureDirty = false;
			GravshipUtility.GetConnectedSubstructure(this, allConnectedSubstructure, int.MaxValue, requireInsideFootprint: false);
			GravshipUtility.GetConnectedSubstructure(this, validSubstructure, (int)this.GetStatValue(StatDefOf.SubstructureSupport));
			base.Map?.substructureGrid?.Drawer.SetDirty();
			if (regenerateSectionLayers && base.Map != null && base.Map.mapDrawer != null)
			{
				base.Map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_GravshipMask));
				base.Map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_GravshipHull));
				base.Map.mapDrawer.RegenerateLayerNow(typeof(SectionLayer_SubstructureProps));
			}
		}
		if (hasTickedThisSession && nameHidden && !haveShownNameDialog && Find.ResearchManager.gravEngineInspected && base.Faction == Faction.OfPlayer && validSubstructure.Count > 90)
		{
			Find.WindowStack.Add(new Dialog_NamePlayerGravship(this));
			haveShownNameDialog = true;
		}
	}

	public override AcceptanceReport ClaimableBy(Faction by)
	{
		AcceptanceReport result = base.ClaimableBy(by);
		if (!result.Accepted)
		{
			return result;
		}
		if (!Find.ResearchManager.gravEngineInspected)
		{
			return "GravEngineNotInspected".Translate();
		}
		return true;
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		if (!base.Spawned)
		{
			return text;
		}
		if (text.Length > 0)
		{
			text += "\n";
		}
		if (!Find.ResearchManager.gravEngineInspected)
		{
			text += "GravEngineNotInspected".Translate() + "\n";
		}
		text = string.Concat(text, "ConnectedSubstructure".Translate() + ": ", AllConnectedSubstructure.Count.ToString(), " / ", StatDefOf.SubstructureSupport.ValueToString(this.GetStatValue(StatDefOf.SubstructureSupport)));
		int count = base.Map.listerThings.ThingsOfDef(ThingDefOf.Blueprint_Substructure).Count;
		count += base.Map.listerThings.ThingsOfDef(ThingDefOf.Frame_Substructure).Count;
		if (count > 0)
		{
			text = string.Concat(text, "\n" + "SubstructureBlueprintsCount".Translate() + ": ", count.ToString());
		}
		if (MissingComponents.Count > 0)
		{
			text = text + "\n" + "Requires".Translate().ToString() + ": " + MissingComponents.Select((GravshipComponentTypeDef d) => d.label).ToCommaList().Colorize(ColorLibrary.RedReadable)
				.CapitalizeFirst();
		}
		return text;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__79))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__79(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__80))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__80(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__81))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__81(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetOrbitalWarnings_003Ed__83))]
	public IEnumerable<TaggedString> GetOrbitalWarnings()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOrbitalWarnings_003Ed__83(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref cooldownCompleteTick, "cooldownCompleteTick", -1);
		Scribe_Values.Look(ref silentlyActivate, "silentlyActivate", defaultValue: false);
		Scribe_Values.Look(ref nameHidden, "nameHidden", defaultValue: true);
		Scribe_Values.Look(ref gravshipName, "gravshipName");
		Scribe_Deep.Look(ref launchInfo, "launchInfo");
		Scribe_Collections.Look(ref pawnsToBoard, "pawnsToBoard", LookMode.Reference);
		Scribe_Collections.Look(ref pawnsToLeave, "pawnsToLeave", LookMode.Reference);
	}

	[CompilerGenerated]
	internal static bool _003CGetGizmos_003Eg__ValidateInspectionTarget_007C79_0(LocalTargetInfo targ)
	{
		if (!(targ.Thing is Pawn pawn))
		{
			return false;
		}
		if (!pawn.IsColonistPlayerControlled)
		{
			return false;
		}
		if (!pawn.CanReach(pawn.Position, PathEndMode.Touch, Danger.Deadly))
		{
			return false;
		}
		if (pawn.Downed || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving))
		{
			return false;
		}
		return true;
	}
}
