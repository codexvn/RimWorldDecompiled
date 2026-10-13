using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005EC RID: 1516
	public class DrawData
	{
		// Token: 0x060026D5 RID: 9941 RVA: 0x000CD7AC File Offset: 0x000CB9AC
		public float ScaleFor(Pawn pawn)
		{
			float num = this.scale;
			if (pawn.RaceProps.Humanlike)
			{
				if (pawn.DevelopmentalStage.Child())
				{
					num *= this.childScale;
				}
				if (this.bodyTypeScales != null)
				{
					foreach (DrawData.BodyTypeDefWithScale bodyTypeDefWithScale in this.bodyTypeScales)
					{
						if (pawn.story.bodyType == bodyTypeDefWithScale.bodyType)
						{
							num *= bodyTypeDefWithScale.scale;
							break;
						}
					}
				}
			}
			return num;
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x000CD84C File Offset: 0x000CBA4C
		public Vector2 PivotForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				Vector2? vector = (this.dataNorth != null) ? this.dataNorth.GetValueOrDefault().pivot : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector2? pivot = this.defaultData.pivot;
				if (pivot == null)
				{
					return DrawData.PivotCenter;
				}
				return pivot.GetValueOrDefault();
			}
			case 1:
			{
				Vector2? vector = (this.dataEast != null) ? this.dataEast.GetValueOrDefault().pivot : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector2? pivot = this.defaultData.pivot;
				if (pivot == null)
				{
					return DrawData.PivotCenter;
				}
				return pivot.GetValueOrDefault();
			}
			case 2:
			{
				Vector2? vector = (this.dataSouth != null) ? this.dataSouth.GetValueOrDefault().pivot : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector2? pivot = this.defaultData.pivot;
				if (pivot == null)
				{
					return DrawData.PivotCenter;
				}
				return pivot.GetValueOrDefault();
			}
			case 3:
			{
				Vector2? vector = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().pivot : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector2? pivot = this.defaultData.pivot;
				if (pivot == null)
				{
					return DrawData.PivotCenter;
				}
				return pivot.GetValueOrDefault();
			}
			default:
			{
				Vector2? vector = this.defaultData.pivot;
				if (vector == null)
				{
					return DrawData.PivotCenter;
				}
				return vector.GetValueOrDefault();
			}
			}
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x000CDA04 File Offset: 0x000CBC04
		public Vector3 OffsetForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				Vector3? vector = (this.dataNorth != null) ? this.dataNorth.GetValueOrDefault().offset : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector3? offset = this.defaultData.offset;
				if (offset == null)
				{
					return Vector3.zero;
				}
				return offset.GetValueOrDefault();
			}
			case 1:
			{
				Vector3? vector2 = (this.dataEast != null) ? this.dataEast.GetValueOrDefault().offset : null;
				if (vector2 != null)
				{
					return vector2.Value;
				}
				Vector3? vector3 = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().offset : null;
				if (vector3 != null)
				{
					Vector3 value = vector3.Value;
					value.x *= -1f;
					return value;
				}
				Vector3? offset2 = this.defaultData.offset;
				if (offset2 != null)
				{
					return offset2.Value;
				}
				return Vector3.zero;
			}
			case 2:
			{
				Vector3? vector = (this.dataSouth != null) ? this.dataSouth.GetValueOrDefault().offset : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector3? offset = this.defaultData.offset;
				if (offset == null)
				{
					return Vector3.zero;
				}
				return offset.GetValueOrDefault();
			}
			case 3:
			{
				Vector3? vector4 = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().offset : null;
				if (vector4 != null)
				{
					return vector4.Value;
				}
				Vector3? vector5 = (this.dataEast != null) ? this.dataEast.GetValueOrDefault().offset : null;
				if (vector5 != null)
				{
					Vector3 value2 = vector5.Value;
					value2.x *= -1f;
					return value2;
				}
				Vector3? offset3 = this.defaultData.offset;
				if (offset3 != null)
				{
					return offset3.Value;
				}
				Vector3? vector = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().offset : null;
				if (vector != null)
				{
					return vector.GetValueOrDefault();
				}
				Vector3? offset = this.defaultData.offset;
				if (offset == null)
				{
					return Vector3.zero;
				}
				return offset.GetValueOrDefault();
			}
			default:
			{
				Vector3? vector = this.defaultData.offset;
				if (vector == null)
				{
					return Vector3.zero;
				}
				return vector.GetValueOrDefault();
			}
			}
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x000CDCB4 File Offset: 0x000CBEB4
		public float RotationOffsetForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				float? num = (this.dataNorth != null) ? this.dataNorth.GetValueOrDefault().rotationOffset : null;
				if (num == null)
				{
					return this.defaultData.rotationOffset.GetValueOrDefault();
				}
				return num.GetValueOrDefault();
			}
			case 1:
			{
				float? num = (this.dataEast != null) ? this.dataEast.GetValueOrDefault().rotationOffset : null;
				if (num == null)
				{
					return this.defaultData.rotationOffset.GetValueOrDefault();
				}
				return num.GetValueOrDefault();
			}
			case 2:
			{
				float? num = (this.dataSouth != null) ? this.dataSouth.GetValueOrDefault().rotationOffset : null;
				if (num == null)
				{
					return this.defaultData.rotationOffset.GetValueOrDefault();
				}
				return num.GetValueOrDefault();
			}
			case 3:
			{
				float? num = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().rotationOffset : null;
				if (num == null)
				{
					return this.defaultData.rotationOffset.GetValueOrDefault();
				}
				return num.GetValueOrDefault();
			}
			default:
				return this.defaultData.rotationOffset.GetValueOrDefault();
			}
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x000CDE10 File Offset: 0x000CC010
		public bool FlipForRot(Rot4 rot)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				bool? flag = (this.dataNorth != null) ? this.dataNorth.GetValueOrDefault().flip : null;
				if (flag == null)
				{
					return this.defaultData.flip.GetValueOrDefault();
				}
				return flag.GetValueOrDefault();
			}
			case 1:
			{
				bool? flag = (this.dataEast != null) ? this.dataEast.GetValueOrDefault().flip : null;
				if (flag == null)
				{
					return this.defaultData.flip.GetValueOrDefault();
				}
				return flag.GetValueOrDefault();
			}
			case 2:
			{
				bool? flag = (this.dataSouth != null) ? this.dataSouth.GetValueOrDefault().flip : null;
				if (flag == null)
				{
					return this.defaultData.flip.GetValueOrDefault();
				}
				return flag.GetValueOrDefault();
			}
			case 3:
			{
				bool? flag = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().flip : null;
				if (flag == null)
				{
					return this.defaultData.flip.GetValueOrDefault();
				}
				return flag.GetValueOrDefault();
			}
			default:
				return this.defaultData.flip.GetValueOrDefault();
			}
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x000CDF6C File Offset: 0x000CC16C
		public float LayerForRot(Rot4 rot, float defaultLayer)
		{
			switch (rot.AsInt)
			{
			case 0:
			{
				float? num = (this.dataNorth != null) ? this.dataNorth.GetValueOrDefault().layer : null;
				if (num != null)
				{
					return num.GetValueOrDefault();
				}
				float? layer = this.defaultData.layer;
				if (layer == null)
				{
					return defaultLayer;
				}
				return layer.GetValueOrDefault();
			}
			case 1:
			{
				float? num = (this.dataEast != null) ? this.dataEast.GetValueOrDefault().layer : null;
				if (num != null)
				{
					return num.GetValueOrDefault();
				}
				float? layer = this.defaultData.layer;
				if (layer == null)
				{
					return defaultLayer;
				}
				return layer.GetValueOrDefault();
			}
			case 2:
			{
				float? num = (this.dataSouth != null) ? this.dataSouth.GetValueOrDefault().layer : null;
				if (num != null)
				{
					return num.GetValueOrDefault();
				}
				float? layer = this.defaultData.layer;
				if (layer == null)
				{
					return defaultLayer;
				}
				return layer.GetValueOrDefault();
			}
			case 3:
			{
				float? num = (this.dataWest != null) ? this.dataWest.GetValueOrDefault().layer : null;
				if (num != null)
				{
					return num.GetValueOrDefault();
				}
				float? layer = this.defaultData.layer;
				if (layer == null)
				{
					return defaultLayer;
				}
				return layer.GetValueOrDefault();
			}
			default:
			{
				float? num = this.defaultData.layer;
				if (num == null)
				{
					return defaultLayer;
				}
				return num.GetValueOrDefault();
			}
			}
		}

		// Token: 0x060026DB RID: 9947 RVA: 0x000CE110 File Offset: 0x000CC310
		public static DrawData NewWithData(params DrawData.RotationalData[] data)
		{
			DrawData drawData = new DrawData();
			foreach (DrawData.RotationalData value in data)
			{
				if (value.rotation == null)
				{
					drawData.defaultData = value;
				}
				else
				{
					switch (value.rotation.Value.AsInt)
					{
					case 0:
						drawData.dataNorth = new DrawData.RotationalData?(value);
						break;
					case 1:
						drawData.dataEast = new DrawData.RotationalData?(value);
						break;
					case 2:
						drawData.dataSouth = new DrawData.RotationalData?(value);
						break;
					case 3:
						drawData.dataWest = new DrawData.RotationalData?(value);
						break;
					}
				}
			}
			return drawData;
		}

		// Token: 0x04002025 RID: 8229
		public static readonly Vector2 PivotCenter = new Vector2(0.5f, 0.5f);

		// Token: 0x04002026 RID: 8230
		private DrawData.RotationalData defaultData;

		// Token: 0x04002027 RID: 8231
		private DrawData.RotationalData? dataNorth;

		// Token: 0x04002028 RID: 8232
		private DrawData.RotationalData? dataEast;

		// Token: 0x04002029 RID: 8233
		private DrawData.RotationalData? dataSouth;

		// Token: 0x0400202A RID: 8234
		private DrawData.RotationalData? dataWest;

		// Token: 0x0400202B RID: 8235
		public bool scaleOffsetByBodySize;

		// Token: 0x0400202C RID: 8236
		public bool useBodyPartAnchor;

		// Token: 0x0400202D RID: 8237
		public float scale = 1f;

		// Token: 0x0400202E RID: 8238
		public float childScale = 1f;

		// Token: 0x0400202F RID: 8239
		private List<DrawData.BodyTypeDefWithScale> bodyTypeScales;

		// Token: 0x020005ED RID: 1517
		public struct RotationalData
		{
			// Token: 0x060026DE RID: 9950 RVA: 0x000CE1F4 File Offset: 0x000CC3F4
			public RotationalData(Rot4? rotation, float layer)
			{
				this = default(DrawData.RotationalData);
				this.rotation = rotation;
				this.layer = new float?(layer);
				this.pivot = null;
				this.offset = null;
				this.rotationOffset = null;
				this.flip = null;
			}

			// Token: 0x04002030 RID: 8240
			public Rot4? rotation;

			// Token: 0x04002031 RID: 8241
			public Vector2? pivot;

			// Token: 0x04002032 RID: 8242
			public Vector3? offset;

			// Token: 0x04002033 RID: 8243
			public float? rotationOffset;

			// Token: 0x04002034 RID: 8244
			public bool? flip;

			// Token: 0x04002035 RID: 8245
			public float? layer;
		}

		// Token: 0x020005EE RID: 1518
		private class BodyTypeDefWithScale
		{
			// Token: 0x060026DF RID: 9951 RVA: 0x000CE24B File Offset: 0x000CC44B
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "bodyType", xmlRoot.Name, null, null, null);
				this.scale = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
			}

			// Token: 0x04002036 RID: 8246
			public BodyTypeDef bodyType;

			// Token: 0x04002037 RID: 8247
			public float scale = 1f;
		}
	}
}
