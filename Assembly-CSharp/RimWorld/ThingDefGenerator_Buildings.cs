using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class ThingDefGenerator_Buildings
{
	[CompilerGenerated]
	private sealed class _003CImpliedBlueprintAndFrameDefs_003Ed__5 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private List<ThingDef>.Enumerator _003C_003E7__wrap1;

		private ThingDef _003Cdef_003E5__3;

		private ThingDef _003Cblueprint_003E5__4;

		private IEnumerator<TerrainDef> _003C_003E7__wrap4;

		private TerrainDef _003CterrDef_003E5__6;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003CImpliedBlueprintAndFrameDefs_003Ed__5(int _003C_003E1__state)
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
			case 2:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 4:
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
			_003Cdef_003E5__3 = null;
			_003Cblueprint_003E5__4 = null;
			_003C_003E7__wrap4 = null;
			_003CterrDef_003E5__6 = null;
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
					_003C_003E7__wrap1 = DefDatabase<ThingDef>.AllDefs.ToList().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0131;
				case 1:
					_003C_003E1__state = -3;
					_003C_003E2__current = NewFrameDef_Thing(_003Cdef_003E5__3, hotReload);
					_003C_003E1__state = 2;
					return true;
				case 2:
					_003C_003E1__state = -3;
					goto IL_00e2;
				case 3:
					_003C_003E1__state = -3;
					goto IL_0123;
				case 4:
					_003C_003E1__state = -4;
					_003C_003E2__current = NewFrameDef_Terrain(_003CterrDef_003E5__6, hotReload);
					_003C_003E1__state = 5;
					return true;
				case 5:
					{
						_003C_003E1__state = -4;
						_003CterrDef_003E5__6 = null;
						break;
					}
					IL_0131:
					if (_003C_003E7__wrap1.MoveNext())
					{
						_003Cdef_003E5__3 = _003C_003E7__wrap1.Current;
						_003Cblueprint_003E5__4 = null;
						if (_003Cdef_003E5__3.BuildableByPlayer)
						{
							_003Cblueprint_003E5__4 = NewBlueprintDef_Thing(_003Cdef_003E5__3, isInstallBlueprint: false, null, hotReload);
							_003C_003E2__current = _003Cblueprint_003E5__4;
							_003C_003E1__state = 1;
							return true;
						}
						goto IL_00e2;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
					_003C_003E7__wrap4 = DefDatabase<TerrainDef>.AllDefs.GetEnumerator();
					_003C_003E1__state = -4;
					break;
					IL_00e2:
					if (_003Cdef_003E5__3.Minifiable)
					{
						_003C_003E2__current = NewBlueprintDef_Thing(_003Cdef_003E5__3, isInstallBlueprint: true, _003Cblueprint_003E5__4, hotReload);
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0123;
					IL_0123:
					_003Cblueprint_003E5__4 = null;
					_003Cdef_003E5__3 = null;
					goto IL_0131;
				}
				while (_003C_003E7__wrap4.MoveNext())
				{
					_003CterrDef_003E5__6 = _003C_003E7__wrap4.Current;
					if (_003CterrDef_003E5__6.BuildableByPlayer)
					{
						_003C_003E2__current = NewBlueprintDef_Terrain(_003CterrDef_003E5__6, hotReload);
						_003C_003E1__state = 4;
						return true;
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap4 = null;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CImpliedBlueprintAndFrameDefs_003Ed__5 _003CImpliedBlueprintAndFrameDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CImpliedBlueprintAndFrameDefs_003Ed__ = this;
			}
			else
			{
				_003CImpliedBlueprintAndFrameDefs_003Ed__ = new _003CImpliedBlueprintAndFrameDefs_003Ed__5(0);
			}
			_003CImpliedBlueprintAndFrameDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CImpliedBlueprintAndFrameDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	public static readonly string BlueprintDefNamePrefix = "Blueprint_";

	public static readonly string InstallBlueprintDefNamePrefix = "Install_";

	public static readonly string BuildingFrameDefNamePrefix = "Frame_";

	private static readonly string TerrainBlueprintGraphicPath = "Things/Special/TerrainBlueprint";

	public static readonly Color BlueprintColor = new Color(0.8235294f, 47f / 51f, 1f, 0.6f);

	[IteratorStateMachine(typeof(_003CImpliedBlueprintAndFrameDefs_003Ed__5))]
	public static IEnumerable<ThingDef> ImpliedBlueprintAndFrameDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CImpliedBlueprintAndFrameDefs_003Ed__5(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}

	private static ThingDef BaseBlueprintDef()
	{
		return new ThingDef
		{
			category = ThingCategory.Ethereal,
			label = "Unspecified blueprint",
			altitudeLayer = AltitudeLayer.Blueprint,
			useHitPoints = false,
			selectable = true,
			seeThroughFog = true,
			comps = 
			{
				(CompProperties)new CompProperties_Forbiddable(),
				(CompProperties)new CompProperties_Styleable()
			},
			drawerType = DrawerType.MapMeshOnly
		};
	}

	private static ThingDef BaseFrameDef()
	{
		return new ThingDef
		{
			isFrameInt = true,
			category = ThingCategory.Building,
			label = "Unspecified building frame",
			thingClass = typeof(Frame),
			altitudeLayer = AltitudeLayer.BuildingOnTop,
			useHitPoints = true,
			selectable = true,
			drawerType = DrawerType.RealtimeOnly,
			building = new BuildingProperties(),
			comps = 
			{
				(CompProperties)new CompProperties_Forbiddable(),
				(CompProperties)new CompProperties_Styleable()
			},
			scatterableOnMapGen = false,
			leaveResourcesWhenKilled = true
		};
	}

	private static ThingDef NewBlueprintDef_Thing(ThingDef def, bool isInstallBlueprint, ThingDef normalBlueprint = null, bool hotReload = false)
	{
		string defName = BlueprintDefNamePrefix + def.defName;
		if (isInstallBlueprint)
		{
			defName = BlueprintDefNamePrefix + InstallBlueprintDefNamePrefix + def.defName;
		}
		ThingDef thingDef = (hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, errorOnFail: false) ?? BaseBlueprintDef()) : BaseBlueprintDef());
		thingDef.defName = defName;
		thingDef.label = def.label + "BlueprintLabelExtra".Translate();
		thingDef.size = def.size;
		thingDef.clearBuildingArea = def.clearBuildingArea;
		thingDef.modContentPack = def.modContentPack;
		thingDef.rotatable = def.rotatable;
		thingDef.replaceTags = def.replaceTags;
		if (!isInstallBlueprint)
		{
			thingDef.constructionSkillPrerequisite = def.constructionSkillPrerequisite;
			thingDef.artisticSkillPrerequisite = def.artisticSkillPrerequisite;
		}
		thingDef.drawPlaceWorkersWhileSelected = def.drawPlaceWorkersWhileSelected;
		if (def.placeWorkers != null)
		{
			thingDef.placeWorkers = new List<Type>(def.placeWorkers);
		}
		if (isInstallBlueprint && normalBlueprint != null)
		{
			thingDef.graphicData = normalBlueprint.graphicData;
		}
		else
		{
			thingDef.graphicData = new GraphicData();
			if (def.building != null && def.building.blueprintGraphicData != null)
			{
				thingDef.graphicData.CopyFrom(def.building.blueprintGraphicData);
				if (thingDef.graphicData.graphicClass == null)
				{
					thingDef.graphicData.graphicClass = typeof(Graphic_Single);
				}
				if (thingDef.graphicData.shaderType == null)
				{
					thingDef.graphicData.shaderType = ShaderTypeDefOf.Transparent;
				}
				if (def.graphicData != null)
				{
					if (def.building.blueprintGraphicData.drawSize == Vector2.one)
					{
						thingDef.graphicData.drawSize = def.graphicData.drawSize;
					}
					thingDef.graphicData.linkFlags = def.graphicData.linkFlags;
					thingDef.graphicData.linkType = def.graphicData.linkType;
					thingDef.graphicData.asymmetricLink = def.graphicData.asymmetricLink;
				}
				thingDef.graphicData.color = BlueprintColor;
			}
			else
			{
				thingDef.graphicData.CopyFrom(def.graphicData);
				thingDef.graphicData.shaderType = ShaderTypeDefOf.EdgeDetect;
				thingDef.graphicData.color = BlueprintColor;
				thingDef.graphicData.colorTwo = Color.white;
				thingDef.graphicData.shadowData = null;
			}
		}
		thingDef.graphicData.renderQueue = 2950;
		thingDef.defaultPlacingRot = def.defaultPlacingRot;
		if (thingDef.graphicData.shadowData != null)
		{
			Log.Error("Blueprint has shadow: " + def);
		}
		if (isInstallBlueprint)
		{
			thingDef.thingClass = typeof(Blueprint_Install);
		}
		else if (def.building != null)
		{
			thingDef.thingClass = def.building.blueprintClass;
		}
		else
		{
			Log.Error("Tried creating build blueprint for thing that has no blueprint class assigned!");
		}
		if (def.thingClass == typeof(Building_Door))
		{
			thingDef.drawerType = DrawerType.RealtimeOnly;
		}
		else
		{
			thingDef.drawerType = def.drawerType;
		}
		thingDef.forceMoveItemsBeforeConstruction = def.forceMoveItemsBeforeConstruction;
		thingDef.entityDefToBuild = def;
		if (isInstallBlueprint)
		{
			def.installBlueprintDef = thingDef;
		}
		else
		{
			def.blueprintDef = thingDef;
		}
		return thingDef;
	}

	private static ThingDef NewFrameDef_Thing(ThingDef def, bool hotReload = false)
	{
		string defName = BuildingFrameDefNamePrefix + def.defName;
		ThingDef thingDef = (hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, errorOnFail: false) ?? BaseFrameDef()) : BaseFrameDef());
		thingDef.defName = defName;
		thingDef.label = def.label + "FrameLabelExtra".Translate();
		thingDef.size = def.size;
		thingDef.SetStatBaseValue(StatDefOf.MaxHitPoints, (float)def.BaseMaxHitPoints * 0.25f);
		thingDef.SetStatBaseValue(StatDefOf.Beauty, -8f);
		thingDef.SetStatBaseValue(StatDefOf.Flammability, def.BaseFlammability);
		thingDef.fillPercent = 0.2f;
		thingDef.pathCost = 14;
		thingDef.description = def.description;
		thingDef.passability = def.passability;
		thingDef.altitudeLayer = def.altitudeLayer;
		if ((int)thingDef.passability > 1)
		{
			thingDef.passability = Traversability.PassThroughOnly;
		}
		thingDef.selectable = def.selectable;
		thingDef.constructEffect = def.constructEffect;
		thingDef.building.isEdifice = def.building.isEdifice;
		thingDef.building.watchBuildingInSameRoom = def.building.watchBuildingInSameRoom;
		thingDef.building.watchBuildingStandDistanceRange = def.building.watchBuildingStandDistanceRange;
		thingDef.building.watchBuildingStandRectWidth = def.building.watchBuildingStandRectWidth;
		thingDef.building.artificialForMeditationPurposes = def.building.artificialForMeditationPurposes;
		thingDef.constructionSkillPrerequisite = def.constructionSkillPrerequisite;
		thingDef.artisticSkillPrerequisite = def.artisticSkillPrerequisite;
		thingDef.clearBuildingArea = def.clearBuildingArea;
		thingDef.forceMoveItemsBeforeConstruction = def.forceMoveItemsBeforeConstruction;
		thingDef.modContentPack = def.modContentPack;
		thingDef.blocksAltitudes = def.blocksAltitudes;
		thingDef.replaceTags = def.replaceTags;
		thingDef.rotatable = def.rotatable;
		thingDef.drawPlaceWorkersWhileSelected = def.drawPlaceWorkersWhileSelected;
		if (def.placeWorkers != null)
		{
			thingDef.placeWorkers = new List<Type>(def.placeWorkers);
		}
		if (def.BuildableByPlayer)
		{
			thingDef.stuffCategories = def.stuffCategories;
			thingDef.costListForDifficulty = def.costListForDifficulty;
		}
		thingDef.entityDefToBuild = def;
		def.frameDef = thingDef;
		return thingDef;
	}

	private static ThingDef NewBlueprintDef_Terrain(TerrainDef terrDef, bool hotReload = false)
	{
		string defName = BlueprintDefNamePrefix + terrDef.defName;
		ThingDef thingDef = (hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, errorOnFail: false) ?? BaseBlueprintDef()) : BaseBlueprintDef());
		thingDef.thingClass = typeof(Blueprint_Build);
		thingDef.defName = defName;
		thingDef.label = terrDef.label + "BlueprintLabelExtra".Translate();
		thingDef.entityDefToBuild = terrDef;
		thingDef.graphicData = new GraphicData();
		thingDef.graphicData.shaderType = ShaderTypeDefOf.MetaOverlay;
		thingDef.graphicData.texPath = TerrainBlueprintGraphicPath;
		thingDef.graphicData.renderQueue = 2950;
		thingDef.graphicData.graphicClass = typeof(Graphic_Single);
		thingDef.constructionSkillPrerequisite = terrDef.constructionSkillPrerequisite;
		thingDef.artisticSkillPrerequisite = terrDef.artisticSkillPrerequisite;
		thingDef.clearBuildingArea = false;
		thingDef.forceMoveItemsBeforeConstruction = terrDef.forceMoveItemsBeforeConstruction;
		thingDef.modContentPack = terrDef.modContentPack;
		thingDef.entityDefToBuild = terrDef;
		terrDef.blueprintDef = thingDef;
		return thingDef;
	}

	private static ThingDef NewFrameDef_Terrain(TerrainDef terrDef, bool hotReload = false)
	{
		string defName = BuildingFrameDefNamePrefix + terrDef.defName;
		ThingDef thingDef = (hotReload ? (DefDatabase<ThingDef>.GetNamed(defName, errorOnFail: false) ?? BaseFrameDef()) : BaseFrameDef());
		thingDef.building.artificialForMeditationPurposes = false;
		thingDef.defName = defName;
		thingDef.label = terrDef.label + "FrameLabelExtra".Translate();
		thingDef.entityDefToBuild = terrDef;
		thingDef.useHitPoints = false;
		thingDef.fillPercent = 0f;
		thingDef.description = "Terrain building in progress.";
		thingDef.passability = Traversability.Standable;
		thingDef.selectable = true;
		thingDef.constructEffect = terrDef.constructEffect;
		thingDef.building.isEdifice = false;
		thingDef.constructionSkillPrerequisite = terrDef.constructionSkillPrerequisite;
		thingDef.artisticSkillPrerequisite = terrDef.artisticSkillPrerequisite;
		thingDef.clearBuildingArea = false;
		thingDef.forceMoveItemsBeforeConstruction = terrDef.forceMoveItemsBeforeConstruction;
		thingDef.modContentPack = terrDef.modContentPack;
		thingDef.category = ThingCategory.Ethereal;
		thingDef.entityDefToBuild = terrDef;
		terrDef.frameDef = thingDef;
		if (!thingDef.IsFrame)
		{
			Log.Error("Framedef is not frame: " + thingDef);
		}
		return thingDef;
	}
}
