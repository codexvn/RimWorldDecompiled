using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000242 RID: 578
	public class GraphicData
	{
		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x00045763 File Offset: 0x00043963
		public bool Linked
		{
			get
			{
				return this.linkType > LinkDrawerType.None;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x0004576E File Offset: 0x0004396E
		public Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.Init();
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x06000ED6 RID: 3798 RVA: 0x00045784 File Offset: 0x00043984
		public void ExplicitlyInitCachedGraphic()
		{
			this.cachedGraphic = this.Graphic;
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00045794 File Offset: 0x00043994
		public void CopyFrom(GraphicData other)
		{
			this.texPath = other.texPath;
			this.maskPath = other.maskPath;
			this.graphicClass = other.graphicClass;
			this.shaderType = other.shaderType;
			this.color = other.color;
			this.colorTwo = other.colorTwo;
			this.drawSize = other.drawSize;
			this.drawOffset = other.drawOffset;
			this.drawOffsetNorth = other.drawOffsetNorth;
			this.drawOffsetEast = other.drawOffsetEast;
			this.drawOffsetSouth = other.drawOffsetSouth;
			this.drawOffsetWest = other.drawOffsetWest;
			this.attachments = other.attachments;
			this.onGroundRandomRotateAngle = other.onGroundRandomRotateAngle;
			this.drawRotated = other.drawRotated;
			this.allowFlip = other.allowFlip;
			this.flipExtraRotation = other.flipExtraRotation;
			this.shadowData = other.shadowData;
			this.damageData = other.damageData;
			this.linkType = other.linkType;
			this.linkFlags = other.linkFlags;
			this.asymmetricLink = other.asymmetricLink;
			this.allowAtlasing = other.allowAtlasing;
			this.renderInstanced = other.renderInstanced;
			this.renderQueue = other.renderQueue;
			this.ignoreThingDrawColor = other.ignoreThingDrawColor;
			this.maxSnS = other.maxSnS;
			this.offsetSnS = other.offsetSnS;
			this.cachedGraphic = null;
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x000458F8 File Offset: 0x00043AF8
		private void Init()
		{
			if (this.graphicClass == null)
			{
				this.cachedGraphic = null;
				return;
			}
			ShaderTypeDef cutout = this.shaderType;
			if (cutout == null)
			{
				cutout = ShaderTypeDefOf.Cutout;
			}
			Shader shader = cutout.Shader;
			this.cachedGraphic = GraphicDatabase.Get(this.graphicClass, this.texPath, shader, this.drawSize, this.color, this.colorTwo, this, this.shaderParameters, this.maskPath);
			if (this.onGroundRandomRotateAngle > 0.01f)
			{
				this.cachedGraphic = new Graphic_RandomRotated(this.cachedGraphic, this.onGroundRandomRotateAngle);
			}
			if (this.Linked)
			{
				this.cachedGraphic = GraphicUtility.WrapLinked(this.cachedGraphic, this.linkType);
			}
		}

		// Token: 0x06000ED9 RID: 3801 RVA: 0x000459AC File Offset: 0x00043BAC
		public void ResolveReferencesSpecial()
		{
			if (this.damageData != null)
			{
				this.damageData.ResolveReferencesSpecial();
			}
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x000459C4 File Offset: 0x00043BC4
		public Vector3 DrawOffsetForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				Vector3? vector = this.drawOffsetNorth;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 1:
			{
				Vector3? vector = this.drawOffsetEast;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 2:
			{
				Vector3? vector = this.drawOffsetSouth;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			case 3:
			{
				Vector3? vector = this.drawOffsetWest;
				if (vector == null)
				{
					return this.drawOffset;
				}
				return vector.GetValueOrDefault();
			}
			default:
				return this.drawOffset;
			}
		}

		// Token: 0x06000EDB RID: 3803 RVA: 0x00045A74 File Offset: 0x00043C74
		public Graphic GraphicColoredFor(Thing t)
		{
			if (this.ignoreThingDrawColor || (t.DrawColor.IndistinguishableFrom(this.Graphic.Color) && t.DrawColorTwo.IndistinguishableFrom(this.Graphic.ColorTwo)))
			{
				return this.Graphic;
			}
			return this.Graphic.GetColoredVersion(this.Graphic.Shader, t.DrawColor, t.DrawColorTwo);
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00045AE2 File Offset: 0x00043CE2
		internal IEnumerable<string> ConfigErrors(ThingDef thingDef)
		{
			if (this.graphicClass == null)
			{
				yield return "graphicClass is null";
			}
			if (this.texPath.NullOrEmpty())
			{
				yield return "texPath is null or empty";
			}
			if (thingDef != null)
			{
				if (thingDef.drawerType == DrawerType.RealtimeOnly && this.Linked)
				{
					yield return "does not add to map mesh but has a link drawer. Link drawers can only work on the map mesh.";
				}
				if (!thingDef.rotatable && (this.drawOffsetNorth != null || this.drawOffsetEast != null || this.drawOffsetSouth != null || this.drawOffsetWest != null))
				{
					yield return "not rotatable but has rotational draw offset(s).";
				}
			}
			if ((this.shaderType == ShaderTypeDefOf.Cutout || this.shaderType == ShaderTypeDefOf.CutoutComplex) && thingDef.mote != null && (thingDef.mote.fadeInTime > 0f || thingDef.mote.fadeOutTime > 0f))
			{
				yield return "mote fades but uses cutout shader type. It will abruptly disappear when opacity falls under the cutout threshold.";
			}
			if (this.linkType == LinkDrawerType.Asymmetric != (this.asymmetricLink != null))
			{
				yield return "linkType=Asymmetric requires <asymmetricLink> and vice versa";
			}
			yield break;
		}

		// Token: 0x040009DD RID: 2525
		[NoTranslate]
		public string name;

		// Token: 0x040009DE RID: 2526
		[NoTranslate]
		public string texPath;

		// Token: 0x040009DF RID: 2527
		[NoTranslate]
		public string maskPath;

		// Token: 0x040009E0 RID: 2528
		public Type graphicClass;

		// Token: 0x040009E1 RID: 2529
		public ShaderTypeDef shaderType;

		// Token: 0x040009E2 RID: 2530
		public List<ShaderParameter> shaderParameters;

		// Token: 0x040009E3 RID: 2531
		public Color color = Color.white;

		// Token: 0x040009E4 RID: 2532
		public Color colorTwo = Color.white;

		// Token: 0x040009E5 RID: 2533
		public Vector2 drawSize = Vector2.one;

		// Token: 0x040009E6 RID: 2534
		public Vector3 drawOffset = Vector3.zero;

		// Token: 0x040009E7 RID: 2535
		public Vector3? drawOffsetNorth;

		// Token: 0x040009E8 RID: 2536
		public Vector3? drawOffsetEast;

		// Token: 0x040009E9 RID: 2537
		public Vector3? drawOffsetSouth;

		// Token: 0x040009EA RID: 2538
		public Vector3? drawOffsetWest;

		// Token: 0x040009EB RID: 2539
		public float onGroundRandomRotateAngle;

		// Token: 0x040009EC RID: 2540
		public bool drawRotated = true;

		// Token: 0x040009ED RID: 2541
		public bool allowFlip = true;

		// Token: 0x040009EE RID: 2542
		public float flipExtraRotation;

		// Token: 0x040009EF RID: 2543
		public bool renderInstanced;

		// Token: 0x040009F0 RID: 2544
		public bool allowAtlasing = true;

		// Token: 0x040009F1 RID: 2545
		public int renderQueue;

		// Token: 0x040009F2 RID: 2546
		public float overlayOpacity;

		// Token: 0x040009F3 RID: 2547
		public List<GraphicData> attachments;

		// Token: 0x040009F4 RID: 2548
		public List<AttachPoint> attachPoints;

		// Token: 0x040009F5 RID: 2549
		public bool addTopAltitudeBias;

		// Token: 0x040009F6 RID: 2550
		public bool ignoreThingDrawColor;

		// Token: 0x040009F7 RID: 2551
		public Vector2 maxSnS;

		// Token: 0x040009F8 RID: 2552
		public Vector2 offsetSnS;

		// Token: 0x040009F9 RID: 2553
		public ShadowData shadowData;

		// Token: 0x040009FA RID: 2554
		public DamageGraphicData damageData;

		// Token: 0x040009FB RID: 2555
		public LinkDrawerType linkType;

		// Token: 0x040009FC RID: 2556
		public LinkFlags linkFlags;

		// Token: 0x040009FD RID: 2557
		public AsymmetricLinkData asymmetricLink;

		// Token: 0x040009FE RID: 2558
		[NoTranslate]
		public string cornerOverlayPath;

		// Token: 0x040009FF RID: 2559
		[Unsaved(false)]
		private Graphic cachedGraphic;
	}
}
