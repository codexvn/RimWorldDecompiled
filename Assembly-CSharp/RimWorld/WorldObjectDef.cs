using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public class WorldObjectDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__51 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldObjectDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

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
		public _003CConfigErrors_003Ed__51(int _003C_003E1__state)
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
				WorldObjectDef worldObjectDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)worldObjectDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00f3;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__3 = 0;
					goto IL_011f;
					IL_011f:
					if (_003Ci_003E5__3 < worldObjectDef.comps.Count)
					{
						_003C_003E7__wrap1 = worldObjectDef.comps[_003Ci_003E5__3].ConfigErrors(worldObjectDef).GetEnumerator();
						_003C_003E1__state = -4;
						goto IL_00f3;
					}
					if (worldObjectDef.expandMore && !worldObjectDef.expandingIcon)
					{
						_003C_003E2__current = "has expandMore but doesn't have any expanding icon";
						_003C_003E1__state = 3;
						return true;
					}
					break;
					IL_00f3:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					_003Ci_003E5__3++;
					goto IL_011f;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__51 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__51(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public Type worldObjectClass = typeof(WorldObject);

	public bool canHaveFaction = true;

	public bool saved = true;

	public bool canBePlayerHome;

	public bool treatAsPlayerHome;

	public bool canResizeToGravship;

	public bool requiresSignalJammerToReach;

	public IntVec3? overrideMapSize;

	public List<WorldObjectCompProperties> comps = new List<WorldObjectCompProperties>();

	public bool canHaveMap = true;

	public bool validLaunchTarget = true;

	public bool allowCaravanIncidentsWhichGenerateMap;

	public bool isTempIncidentMapOwner;

	public List<IncidentTargetTagDef> IncidentTargetTags;

	public bool selectable = true;

	public bool neverMultiSelect;

	public MapGeneratorDef mapGenerator;

	public List<Type> inspectorTabs;

	[Unsaved(false)]
	public List<InspectTabBase> inspectorTabsResolved;

	public bool useDynamicDrawer;

	public bool expandingIcon;

	[NoTranslate]
	public string expandingIconTexture;

	[NoTranslate]
	public string expandingIconMaskTexture;

	public float expandingIconPriority;

	[NoTranslate]
	public string texture;

	[NoTranslate]
	public string mapEdgeTexture;

	private ShaderTypeDef mapEdgeShader;

	[Unsaved(false)]
	private Material material;

	[Unsaved(false)]
	private Material expandingMaterial;

	[Unsaved(false)]
	private Texture2D expandingIconTextureInt;

	[Unsaved(false)]
	private Texture2D expandingIconTextureMaskInt;

	public bool expandMore;

	public bool rotateGraphicWhenTraveling;

	public Color? expandingIconColor;

	public float expandingIconDrawSize = 1f;

	public bool fullyExpandedInSpace;

	public ShaderTypeDef shader;

	public ShaderTypeDef expandingShader;

	public float drawAltitudeOffset;

	public bool blockExitGridUntilBattleIsWon;

	public RulePackDef nameMaker;

	public Material Material
	{
		get
		{
			if (texture.NullOrEmpty())
			{
				return null;
			}
			if (material == null)
			{
				material = MaterialPool.MatFrom(texture, shader?.Shader ?? ShaderDatabase.WorldOverlayTransparentLit, 3550);
			}
			return material;
		}
	}

	public Texture2D ExpandingIconTexture
	{
		get
		{
			if (expandingIconTextureInt == null)
			{
				if (expandingIconTexture.NullOrEmpty())
				{
					return null;
				}
				expandingIconTextureInt = ContentFinder<Texture2D>.Get(expandingIconTexture);
			}
			return expandingIconTextureInt;
		}
	}

	public Texture2D ExpandingIconTextureMask
	{
		get
		{
			if (expandingIconTextureMaskInt == null)
			{
				if (expandingIconMaskTexture.NullOrEmpty())
				{
					return null;
				}
				expandingIconTextureMaskInt = ContentFinder<Texture2D>.Get(expandingIconMaskTexture);
			}
			return expandingIconTextureMaskInt;
		}
	}

	public Material MapEdgeMaterial
	{
		get
		{
			if (mapEdgeTexture.NullOrEmpty())
			{
				return null;
			}
			if (material == null)
			{
				material = MaterialPool.MatFrom(mapEdgeTexture, ShaderDatabase.MapEdgeTerrain);
			}
			return material;
		}
	}

	public override void PostLoad()
	{
		base.PostLoad();
		if (inspectorTabs == null)
		{
			return;
		}
		for (int i = 0; i < inspectorTabs.Count; i++)
		{
			if (inspectorTabsResolved == null)
			{
				inspectorTabsResolved = new List<InspectTabBase>();
			}
			try
			{
				inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(inspectorTabs[i]));
			}
			catch (Exception ex)
			{
				Log.Error("Could not instantiate inspector tab of type " + inspectorTabs[i]?.ToString() + ": " + ex);
			}
		}
	}

	public override void ResolveReferences()
	{
		base.ResolveReferences();
		for (int i = 0; i < comps.Count; i++)
		{
			comps[i].ResolveReferences(this);
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__51))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__51(-2)
		{
			_003C_003E4__this = this
		};
	}
}
