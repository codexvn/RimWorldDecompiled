using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class TerrainDefGenerator_Carpet
{
	[CompilerGenerated]
	private sealed class _003CImpliedTerrainDefs_003Ed__0 : IEnumerable<TerrainDef>, IEnumerable, IEnumerator<TerrainDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TerrainDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private IEnumerator<ColorDef> _003C_003E7__wrap1;

		private ColorDef _003Cc_003E5__3;

		private int _003Cindex_003E5__4;

		private IEnumerator<TerrainTemplateDef> _003C_003E7__wrap4;

		TerrainDef IEnumerator<TerrainDef>.Current
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
		public _003CImpliedTerrainDefs_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003Cc_003E5__3 = null;
			_003C_003E7__wrap4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					_003Cindex_003E5__4++;
					goto IL_00e3;
				}
				_003C_003E1__state = -1;
				IEnumerable<ColorDef> enumerable = DefDatabase<ColorDef>.AllDefs.Where((ColorDef x) => x.colorType == ColorType.Structure);
				_003C_003E7__wrap1 = enumerable.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_0104;
				IL_00e3:
				if (_003C_003E7__wrap4.MoveNext())
				{
					TerrainTemplateDef current = _003C_003E7__wrap4.Current;
					_003C_003E2__current = CarpetFromBlueprint(current, _003Cc_003E5__3, _003Cindex_003E5__4, hotReload);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap4 = null;
				_003Cc_003E5__3 = null;
				goto IL_0104;
				IL_0104:
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003Cc_003E5__3 = _003C_003E7__wrap1.Current;
					_003Cindex_003E5__4 = 0;
					_003C_003E7__wrap4 = DefDatabase<TerrainTemplateDef>.AllDefs.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00e3;
				}
				_003C_003Em__Finally1();
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
			_003C_003E1__state = -3;
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
		IEnumerator<TerrainDef> IEnumerable<TerrainDef>.GetEnumerator()
		{
			_003CImpliedTerrainDefs_003Ed__0 _003CImpliedTerrainDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CImpliedTerrainDefs_003Ed__ = this;
			}
			else
			{
				_003CImpliedTerrainDefs_003Ed__ = new _003CImpliedTerrainDefs_003Ed__0(0);
			}
			_003CImpliedTerrainDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CImpliedTerrainDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TerrainDef>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CImpliedTerrainDefs_003Ed__0))]
	public static IEnumerable<TerrainDef> ImpliedTerrainDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CImpliedTerrainDefs_003Ed__0(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}

	public static TerrainDef CarpetFromBlueprint(TerrainTemplateDef tp, ColorDef colorDef, int index, bool hotReload = false)
	{
		string defName = tp.defName + colorDef.defName.Replace("Structure_", "");
		TerrainDef terrainDef = (hotReload ? (DefDatabase<TerrainDef>.GetNamed(defName, errorOnFail: false) ?? new TerrainDef()) : new TerrainDef());
		terrainDef.defName = defName;
		terrainDef.label = tp.label.Formatted(colorDef.label);
		terrainDef.texturePath = tp.texturePath;
		terrainDef.researchPrerequisites = tp.researchPrerequisites;
		terrainDef.burnedDef = tp.burnedDef;
		terrainDef.costList = tp.costList;
		terrainDef.description = tp.description;
		terrainDef.colorDef = colorDef;
		terrainDef.designatorDropdown = tp.designatorDropdown;
		terrainDef.uiOrder = tp.uiOrder;
		terrainDef.statBases = tp.statBases;
		terrainDef.renderPrecedence = tp.renderPrecedenceStart - index;
		terrainDef.constructionSkillPrerequisite = tp.constructionSkillPrerequisite;
		terrainDef.canGenerateDefaultDesignator = tp.canGenerateDefaultDesignator;
		terrainDef.tags = tp.tags;
		terrainDef.dominantStyleCategory = tp.dominantStyleCategory;
		terrainDef.layerable = true;
		terrainDef.affordances = new List<TerrainAffordanceDef>
		{
			TerrainAffordanceDefOf.Walkable,
			TerrainAffordanceDefOf.Light,
			TerrainAffordanceDefOf.Medium,
			TerrainAffordanceDefOf.Heavy
		};
		terrainDef.designationCategory = DesignationCategoryDefOf.Floors;
		terrainDef.fertility = 0f;
		terrainDef.constructEffect = EffecterDefOf.ConstructDirt;
		terrainDef.pollutionColor = new Color(1f, 1f, 1f, 0.8f);
		terrainDef.pollutionOverlayScale = new Vector2(0.75f, 0.75f);
		terrainDef.pollutionOverlayTexturePath = "Terrain/Surfaces/PollutionFloorSmooth";
		terrainDef.terrainAffordanceNeeded = TerrainAffordanceDefOf.Heavy;
		if (ModsConfig.BiotechActive)
		{
			terrainDef.pollutionShaderType = ShaderTypeDefOf.TerrainFadeRoughLinearBurn;
		}
		return terrainDef;
	}
}
