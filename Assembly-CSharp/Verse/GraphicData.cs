using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using UnityStubs;

namespace Verse;

public class GraphicData
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__47 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public GraphicData _003C_003E4__this;

		private ThingDef thingDef;

		public ThingDef _003C_003E3__thingDef;

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
		public _003CConfigErrors_003Ed__47(int _003C_003E1__state)
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
			GraphicData graphicData = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (graphicData.graphicClass == null)
				{
					_003C_003E2__current = "graphicClass is null";
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0062;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0062;
			case 2:
				_003C_003E1__state = -1;
				goto IL_008a;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00c6;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0122;
			case 5:
				_003C_003E1__state = -1;
				goto IL_0192;
			case 6:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0062:
				if (graphicData.texPath.NullOrEmpty())
				{
					_003C_003E2__current = "texPath is null or empty";
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_008a;
				IL_00c6:
				if (!thingDef.rotatable && (graphicData.drawOffsetNorth.HasValue || graphicData.drawOffsetEast.HasValue || graphicData.drawOffsetSouth.HasValue || graphicData.drawOffsetWest.HasValue))
				{
					_003C_003E2__current = "not rotatable but has rotational draw offset(s).";
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_0122;
				IL_008a:
				if (thingDef != null)
				{
					if (thingDef.drawerType == DrawerType.RealtimeOnly && graphicData.Linked)
					{
						_003C_003E2__current = "does not add to map mesh but has a link drawer. Link drawers can only work on the map mesh.";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_00c6;
				}
				goto IL_0122;
				IL_0192:
				if (graphicData.linkType == LinkDrawerType.Asymmetric != (graphicData.asymmetricLink != null))
				{
					_003C_003E2__current = "linkType=Asymmetric requires <asymmetricLink> and vice versa";
					_003C_003E1__state = 6;
					return true;
				}
				break;
				IL_0122:
				if ((graphicData.shaderType == ShaderTypeDefOf.Cutout || graphicData.shaderType == ShaderTypeDefOf.CutoutComplex) && thingDef.mote != null && (thingDef.mote.fadeInTime > 0f || thingDef.mote.fadeOutTime > 0f))
				{
					_003C_003E2__current = "mote fades but uses cutout shader type. It will abruptly disappear when opacity falls under the cutout threshold.";
					_003C_003E1__state = 5;
					return true;
				}
				goto IL_0192;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__47 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__47(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.thingDef = _003C_003E3__thingDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[NoTranslate]
	public string name;

	[NoTranslate]
	public string texPath;

	[NoTranslate]
	public string maskPath;

	public Type graphicClass;

	public ShaderTypeDef shaderType;

	public List<ShaderParameter> shaderParameters;

	public bool textureRepeats;

	public bool borderMipMaps;

	public Color color = Color.white;

	public Color colorTwo = Color.white;

	public Vector2 drawSize = Vector2.one;

	public Vector3 drawOffset = Vector3.zero;

	public Vector3? drawOffsetNorth;

	public Vector3? drawOffsetEast;

	public Vector3? drawOffsetSouth;

	public Vector3? drawOffsetWest;

	public float onGroundRandomRotateAngle;

	public bool drawRotated = true;

	public bool allowFlip = true;

	public float flipExtraRotation;

	public bool renderInstanced;

	public bool allowAtlasing = true;

	public int renderQueue;

	public float overlayOpacity;

	public List<GraphicData> attachments;

	public List<AttachPoint> attachPoints;

	public bool addTopAltitudeBias;

	public bool ignoreThingDrawColor;

	public Vector2 maxSnS;

	public Vector2 offsetSnS;

	public ShadowData shadowData;

	public DamageGraphicData damageData;

	public LinkDrawerType linkType;

	public LinkFlags linkFlags;

	public AsymmetricLinkData asymmetricLink;

	[NoTranslate]
	public string cornerOverlayPath;

	[Unsaved(false)]
	private Graphic cachedGraphic;

	public bool Linked => linkType != LinkDrawerType.None;

	public Graphic Graphic
	{
		get
		{
			if (cachedGraphic == null)
			{
				Init();
			}
			return cachedGraphic;
		}
	}

	public void ExplicitlyInitCachedGraphic()
	{
		cachedGraphic = Graphic;
	}

	public void CopyFrom(GraphicData other)
	{
		texPath = other.texPath;
		maskPath = other.maskPath;
		graphicClass = other.graphicClass;
		shaderType = other.shaderType;
		textureRepeats = other.textureRepeats;
		borderMipMaps = other.borderMipMaps;
		color = other.color;
		colorTwo = other.colorTwo;
		drawSize = other.drawSize;
		drawOffset = other.drawOffset;
		drawOffsetNorth = other.drawOffsetNorth;
		drawOffsetEast = other.drawOffsetEast;
		drawOffsetSouth = other.drawOffsetSouth;
		drawOffsetWest = other.drawOffsetWest;
		attachments = other.attachments;
		onGroundRandomRotateAngle = other.onGroundRandomRotateAngle;
		drawRotated = other.drawRotated;
		allowFlip = other.allowFlip;
		flipExtraRotation = other.flipExtraRotation;
		shadowData = other.shadowData;
		damageData = other.damageData;
		linkType = other.linkType;
		linkFlags = other.linkFlags;
		asymmetricLink = other.asymmetricLink;
		allowAtlasing = other.allowAtlasing;
		renderInstanced = other.renderInstanced;
		renderQueue = other.renderQueue;
		ignoreThingDrawColor = other.ignoreThingDrawColor;
		maxSnS = other.maxSnS;
		offsetSnS = other.offsetSnS;
		cornerOverlayPath = other.cornerOverlayPath;
		cachedGraphic = null;
	}

	private void Init()
	{
		if (graphicClass == null)
		{
			cachedGraphic = null;
			return;
		}
		ShaderTypeDef cutout = shaderType;
		if (cutout == null)
		{
			cutout = ShaderTypeDefOf.Cutout;
		}
		Shader shader = cutout.Shader;
		if (borderMipMaps)
		{
			ImportDefaults.BorderMipMaps(texPath);
		}
		cachedGraphic = GraphicDatabase.Get(graphicClass, texPath, shader, drawSize, color, colorTwo, this, shaderParameters, maskPath);
		if (textureRepeats && cachedGraphic.MatSingle?.mainTexture != null)
		{
			cachedGraphic.MatSingle.mainTexture.wrapMode = TextureWrapMode.Repeat;
		}
		if (onGroundRandomRotateAngle > 0.01f)
		{
			cachedGraphic = new Graphic_RandomRotated(cachedGraphic, onGroundRandomRotateAngle);
		}
		if (Linked)
		{
			cachedGraphic = GraphicUtility.WrapLinked(cachedGraphic, linkType);
		}
	}

	public void ResolveReferencesSpecial()
	{
		if (damageData != null)
		{
			damageData.ResolveReferencesSpecial();
		}
	}

	public Vector3 DrawOffsetForRot(Rot4 rot)
	{
		return rot.AsInt switch
		{
			0 => drawOffsetNorth ?? drawOffset, 
			1 => drawOffsetEast ?? drawOffset, 
			2 => drawOffsetSouth ?? drawOffset, 
			3 => drawOffsetWest ?? drawOffset, 
			_ => drawOffset, 
		};
	}

	public Graphic GraphicColoredFor(Thing t)
	{
		if (ignoreThingDrawColor || (t.DrawColor.IndistinguishableFrom(Graphic.Color) && t.DrawColorTwo.IndistinguishableFrom(Graphic.ColorTwo)))
		{
			return Graphic;
		}
		return Graphic.GetColoredVersion(Graphic.Shader, t.DrawColor, t.DrawColorTwo);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__47))]
	internal IEnumerable<string> ConfigErrors(ThingDef thingDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__47(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__thingDef = thingDef
		};
	}
}
