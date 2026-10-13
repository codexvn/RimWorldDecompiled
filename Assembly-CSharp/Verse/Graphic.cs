using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000880 RID: 2176
	public class Graphic
	{
		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06003725 RID: 14117 RVA: 0x00127974 File Offset: 0x00125B74
		public Shader Shader
		{
			get
			{
				Material matSingle = this.MatSingle;
				if (matSingle != null)
				{
					return matSingle.shader;
				}
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06003726 RID: 14118 RVA: 0x0012799D File Offset: 0x00125B9D
		// (set) Token: 0x06003727 RID: 14119 RVA: 0x001279D8 File Offset: 0x00125BD8
		public Graphic_Shadow ShadowGraphic
		{
			get
			{
				if (this.cachedShadowGraphicInt == null && this.data != null && this.data.shadowData != null)
				{
					this.cachedShadowGraphicInt = new Graphic_Shadow(this.data.shadowData);
				}
				return this.cachedShadowGraphicInt;
			}
			set
			{
				this.cachedShadowGraphicInt = value;
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06003728 RID: 14120 RVA: 0x001279E1 File Offset: 0x00125BE1
		public Color Color
		{
			get
			{
				return this.color;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x06003729 RID: 14121 RVA: 0x001279E9 File Offset: 0x00125BE9
		public Color ColorTwo
		{
			get
			{
				return this.colorTwo;
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x0600372A RID: 14122 RVA: 0x001279F1 File Offset: 0x00125BF1
		public virtual Material MatSingle
		{
			get
			{
				return BaseContent.BadMat;
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x0600372B RID: 14123 RVA: 0x001279F8 File Offset: 0x00125BF8
		public virtual Material MatWest
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x0600372C RID: 14124 RVA: 0x001279F8 File Offset: 0x00125BF8
		public virtual Material MatSouth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x0600372D RID: 14125 RVA: 0x001279F8 File Offset: 0x00125BF8
		public virtual Material MatEast
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x0600372E RID: 14126 RVA: 0x001279F8 File Offset: 0x00125BF8
		public virtual Material MatNorth
		{
			get
			{
				return this.MatSingle;
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x0600372F RID: 14127 RVA: 0x00127A00 File Offset: 0x00125C00
		public virtual bool WestFlipped
		{
			get
			{
				return this.DataAllowsFlip && !this.ShouldDrawRotated;
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06003730 RID: 14128 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool EastFlipped
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06003731 RID: 14129 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ShouldDrawRotated
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06003732 RID: 14130 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float DrawRotatedExtraAngleOffset
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06003733 RID: 14131 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool UseSameGraphicForGhost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06003734 RID: 14132 RVA: 0x00127A15 File Offset: 0x00125C15
		protected bool DataAllowsFlip
		{
			get
			{
				return this.data == null || this.data.allowFlip;
			}
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x00127A2C File Offset: 0x00125C2C
		public static bool TryGetTextureAtlasReplacementInfo(Material mat, TextureAtlasGroup group, bool flipUv, bool vertexColors, out Material material, out Vector2[] uvs, out Color32 vertexColor)
		{
			material = mat;
			uvs = null;
			vertexColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			Graphic.AtlasReplacementInfoCacheKey key = new Graphic.AtlasReplacementInfoCacheKey(mat, group, flipUv, vertexColors);
			Graphic.CachedAtlasReplacementInfo cachedAtlasReplacementInfo;
			if (Graphic.replacementInfoCache.TryGetValue(key, out cachedAtlasReplacementInfo))
			{
				material = cachedAtlasReplacementInfo.material;
				uvs = cachedAtlasReplacementInfo.uvs;
				if (vertexColors)
				{
					vertexColor = cachedAtlasReplacementInfo.vertexColor;
				}
				return true;
			}
			StaticTextureAtlasTile staticTextureAtlasTile;
			if (!GlobalTextureAtlasManager.TryGetStaticTile(group, (Texture2D)mat.mainTexture, out staticTextureAtlasTile, false))
			{
				return false;
			}
			MaterialRequest materialRequest;
			if (!MaterialPool.TryGetRequestForMat(mat, out materialRequest))
			{
				Log.Error("Tried getting texture atlas replacement info for a material that was not created by MaterialPool!");
				return false;
			}
			uvs = new Vector2[4];
			Printer_Plane.GetUVs(staticTextureAtlasTile.uvRect, out uvs[0], out uvs[1], out uvs[2], out uvs[3], flipUv);
			materialRequest.mainTex = staticTextureAtlasTile.atlas.ColorTexture;
			if (vertexColors)
			{
				vertexColor = materialRequest.color;
				materialRequest.color = Color.white;
			}
			if (materialRequest.maskTex != null)
			{
				materialRequest.maskTex = staticTextureAtlasTile.atlas.MaskTexture;
			}
			material = MaterialPool.MatFrom(materialRequest);
			Graphic.replacementInfoCache.Add(key, new Graphic.CachedAtlasReplacementInfo
			{
				material = material,
				uvs = uvs,
				vertexColor = vertexColor
			});
			return true;
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x00127B9D File Offset: 0x00125D9D
		public virtual void Init(GraphicRequest req)
		{
			Log.ErrorOnce(string.Format("Cannot init Graphic of class {0}", base.GetType()), 658928);
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x00127BB9 File Offset: 0x00125DB9
		public virtual Material NodeGetMat(PawnDrawParms parms)
		{
			return this.MatAt(parms.facing, parms.pawn);
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x00127BD0 File Offset: 0x00125DD0
		public virtual Material MatAt(Rot4 rot, Thing thing = null)
		{
			switch (rot.AsInt)
			{
			case 0:
				return this.MatNorth;
			case 1:
				return this.MatEast;
			case 2:
				return this.MatSouth;
			case 3:
				return this.MatWest;
			default:
				return BaseContent.BadMat;
			}
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x00127C20 File Offset: 0x00125E20
		public virtual Mesh MeshAt(Rot4 rot)
		{
			Vector2 vector = this.drawSize;
			if (rot.IsHorizontal && !this.ShouldDrawRotated)
			{
				vector = vector.Rotated();
			}
			if ((rot == Rot4.West && this.WestFlipped) || (rot == Rot4.East && this.EastFlipped))
			{
				return MeshPool.GridPlaneFlip(vector);
			}
			return MeshPool.GridPlane(vector);
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x001279F8 File Offset: 0x00125BF8
		public virtual Material MatSingleFor(Thing thing)
		{
			return this.MatSingle;
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x00127C83 File Offset: 0x00125E83
		public Vector3 DrawOffset(Rot4 rot)
		{
			if (this.data == null)
			{
				return Vector3.zero;
			}
			return this.data.DrawOffsetForRot(rot);
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x00127C9F File Offset: 0x00125E9F
		public void Draw(Vector3 loc, Rot4 rot, Thing thing, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thing.def, thing, extraRotation);
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x00127CB2 File Offset: 0x00125EB2
		public void DrawFromDef(Vector3 loc, Rot4 rot, ThingDef thingDef, float extraRotation = 0f)
		{
			this.DrawWorker(loc, rot, thingDef, null, extraRotation);
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x00127CC0 File Offset: 0x00125EC0
		public virtual void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Mesh mesh = this.MeshAt(rot);
			Quaternion quaternion = this.QuatFromRot(rot);
			if (extraRotation != 0f)
			{
				quaternion *= Quaternion.Euler(Vector3.up * extraRotation);
			}
			if (this.data != null && this.data.addTopAltitudeBias)
			{
				quaternion *= Quaternion.Euler(Vector3.left * 2f);
			}
			loc += this.DrawOffset(rot);
			Material mat = this.MatAt(rot, thing);
			this.DrawMeshInt(mesh, loc, quaternion, mat);
			if (this.ShadowGraphic != null)
			{
				this.ShadowGraphic.DrawWorker(loc, rot, thingDef, thing, extraRotation);
			}
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x00127D6B File Offset: 0x00125F6B
		protected virtual void DrawMeshInt(Mesh mesh, Vector3 loc, Quaternion quat, Material mat)
		{
			Graphics.DrawMesh(mesh, loc, quat, mat, 0);
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x00127D78 File Offset: 0x00125F78
		public virtual void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Vector2 vector;
			bool flag;
			if (this.ShouldDrawRotated)
			{
				vector = this.drawSize;
				flag = false;
			}
			else
			{
				if (!thing.Rotation.IsHorizontal)
				{
					vector = this.drawSize;
				}
				else
				{
					vector = this.drawSize.Rotated();
				}
				flag = ((thing.Rotation == Rot4.West && this.WestFlipped) || (thing.Rotation == Rot4.East && this.EastFlipped));
			}
			if (thing.MultipleItemsPerCellDrawn())
			{
				vector *= 0.8f;
			}
			float num = this.AngleFromRot(thing.Rotation) + extraRotation;
			if (flag && this.data != null)
			{
				num += this.data.flipExtraRotation;
			}
			Vector3 center = thing.TrueCenter() + this.DrawOffset(thing.Rotation);
			Material mat = this.MatAt(thing.Rotation, thing);
			Vector2[] uvs;
			Color32 color;
			Graphic.TryGetTextureAtlasReplacementInfo(mat, thing.def.category.ToAtlasGroup(), flag, true, out mat, out uvs, out color);
			Printer_Plane.PrintPlane(layer, center, vector, mat, num, flag, uvs, new Color32[]
			{
				color,
				color,
				color,
				color
			}, 0.01f, 0f);
			Graphic_Shadow shadowGraphic = this.ShadowGraphic;
			if (shadowGraphic == null)
			{
				return;
			}
			shadowGraphic.Print(layer, thing, 0f);
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x00127ED3 File Offset: 0x001260D3
		public virtual Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			Log.ErrorOnce("CloneColored not implemented on this subclass of Graphic: " + base.GetType().ToString(), 66300);
			return BaseContent.BadGraphic;
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x00127EF9 File Offset: 0x001260F9
		[Obsolete("Will be removed in a future release")]
		public virtual Graphic GetCopy(Vector2 newDrawSize)
		{
			return this.GetCopy(newDrawSize, null);
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x00127F03 File Offset: 0x00126103
		public virtual Graphic GetCopy(Vector2 newDrawSize, Shader overrideShader)
		{
			return GraphicDatabase.Get(base.GetType(), this.path, overrideShader ?? this.Shader, newDrawSize, this.color, this.colorTwo, null);
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x00127F30 File Offset: 0x00126130
		public virtual Graphic GetShadowlessGraphic()
		{
			if (this.data == null || this.data.shadowData == null)
			{
				return this;
			}
			if (this.cachedShadowlessGraphicInt == null)
			{
				GraphicData graphicData = new GraphicData();
				graphicData.CopyFrom(this.data);
				graphicData.shadowData = null;
				this.cachedShadowlessGraphicInt = graphicData.Graphic;
			}
			return this.cachedShadowlessGraphicInt;
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x00127F88 File Offset: 0x00126188
		protected float AngleFromRot(Rot4 rot)
		{
			if (this.ShouldDrawRotated)
			{
				float num = rot.AsAngle;
				num += this.DrawRotatedExtraAngleOffset;
				if ((rot == Rot4.West && this.WestFlipped) || (rot == Rot4.East && this.EastFlipped))
				{
					num += 180f;
				}
				return num;
			}
			return 0f;
		}

		// Token: 0x06003747 RID: 14151 RVA: 0x00127FE8 File Offset: 0x001261E8
		public Quaternion QuatFromRot(Rot4 rot)
		{
			float num = this.AngleFromRot(rot);
			if (num == 0f)
			{
				return Quaternion.identity;
			}
			return Quaternion.AngleAxis(num, Vector3.up);
		}

		// Token: 0x0400295A RID: 10586
		public GraphicData data;

		// Token: 0x0400295B RID: 10587
		public string path;

		// Token: 0x0400295C RID: 10588
		public string maskPath;

		// Token: 0x0400295D RID: 10589
		public Color color = Color.white;

		// Token: 0x0400295E RID: 10590
		public Color colorTwo = Color.white;

		// Token: 0x0400295F RID: 10591
		public Vector2 drawSize = Vector2.one;

		// Token: 0x04002960 RID: 10592
		private Graphic_Shadow cachedShadowGraphicInt;

		// Token: 0x04002961 RID: 10593
		private Graphic cachedShadowlessGraphicInt;

		// Token: 0x04002962 RID: 10594
		private static Dictionary<Graphic.AtlasReplacementInfoCacheKey, Graphic.CachedAtlasReplacementInfo> replacementInfoCache = new Dictionary<Graphic.AtlasReplacementInfoCacheKey, Graphic.CachedAtlasReplacementInfo>();

		// Token: 0x02000881 RID: 2177
		private struct AtlasReplacementInfoCacheKey : IEquatable<Graphic.AtlasReplacementInfoCacheKey>
		{
			// Token: 0x0600374A RID: 14154 RVA: 0x0012804C File Offset: 0x0012624C
			public AtlasReplacementInfoCacheKey(Material mat, TextureAtlasGroup group, bool flipUv, bool vertexColors)
			{
				this.mat = mat;
				this.group = group;
				this.flipUv = flipUv;
				this.vertexColors = vertexColors;
				this.hash = Gen.HashCombine<int>(mat.GetHashCode(), group.GetHashCode());
				if (flipUv)
				{
					this.hash = ~this.hash;
				}
				if (vertexColors)
				{
					this.hash ^= 123893723;
				}
			}

			// Token: 0x0600374B RID: 14155 RVA: 0x001280BA File Offset: 0x001262BA
			public bool Equals(Graphic.AtlasReplacementInfoCacheKey other)
			{
				return this.mat == other.mat && this.group == other.group && this.flipUv == other.flipUv && this.vertexColors == other.vertexColors;
			}

			// Token: 0x0600374C RID: 14156 RVA: 0x001280F6 File Offset: 0x001262F6
			public override int GetHashCode()
			{
				return this.hash;
			}

			// Token: 0x04002963 RID: 10595
			public readonly Material mat;

			// Token: 0x04002964 RID: 10596
			public readonly TextureAtlasGroup group;

			// Token: 0x04002965 RID: 10597
			public readonly bool flipUv;

			// Token: 0x04002966 RID: 10598
			public readonly bool vertexColors;

			// Token: 0x04002967 RID: 10599
			private readonly int hash;
		}

		// Token: 0x02000882 RID: 2178
		private struct CachedAtlasReplacementInfo
		{
			// Token: 0x04002968 RID: 10600
			public Material material;

			// Token: 0x04002969 RID: 10601
			public Vector2[] uvs;

			// Token: 0x0400296A RID: 10602
			public Color32 vertexColor;
		}
	}
}
