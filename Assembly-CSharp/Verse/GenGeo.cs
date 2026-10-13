using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CFC RID: 3324
	public static class GenGeo
	{
		// Token: 0x0600550B RID: 21771 RVA: 0x001B6998 File Offset: 0x001B4B98
		public static float AngleDifferenceBetween(float A, float B)
		{
			float num = A + 360f;
			float num2 = B + 360f;
			float num3 = 9999f;
			float num4 = A - B;
			if (num4 < 0f)
			{
				num4 *= -1f;
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			num4 = num - B;
			if (num4 < 0f)
			{
				num4 *= -1f;
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			num4 = A - num2;
			if (num4 < 0f)
			{
				num4 *= -1f;
			}
			if (num4 < num3)
			{
				num3 = num4;
			}
			return num3;
		}

		// Token: 0x0600550C RID: 21772 RVA: 0x001B6A0E File Offset: 0x001B4C0E
		public static float MagnitudeHorizontal(this Vector3 v)
		{
			return (float)Math.Sqrt((double)(v.x * v.x + v.z * v.z));
		}

		// Token: 0x0600550D RID: 21773 RVA: 0x001B6A32 File Offset: 0x001B4C32
		public static float MagnitudeHorizontalSquared(this Vector3 v)
		{
			return v.x * v.x + v.z * v.z;
		}

		// Token: 0x0600550E RID: 21774 RVA: 0x001B6A50 File Offset: 0x001B4C50
		public static bool LinesIntersect(Vector3 line1V1, Vector3 line1V2, Vector3 line2V1, Vector3 line2V2)
		{
			float num = line1V2.z - line1V1.z;
			float num2 = line1V1.x - line1V2.x;
			float num3 = num * line1V1.x + num2 * line1V1.z;
			float num4 = line2V2.z - line2V1.z;
			float num5 = line2V1.x - line2V2.x;
			float num6 = num4 * line2V1.x + num5 * line2V1.z;
			float num7 = num * num5 - num4 * num2;
			if (num7 == 0f)
			{
				return false;
			}
			float num8 = (num5 * num3 - num2 * num6) / num7;
			float num9 = (num * num6 - num4 * num3) / num7;
			return (num8 <= line1V1.x || num8 <= line1V2.x) && (num8 <= line2V1.x || num8 <= line2V2.x) && (num8 >= line1V1.x || num8 >= line1V2.x) && (num8 >= line2V1.x || num8 >= line2V2.x) && (num9 <= line1V1.z || num9 <= line1V2.z) && (num9 <= line2V1.z || num9 <= line2V2.z) && (num9 >= line1V1.z || num9 >= line1V2.z) && (num9 >= line2V1.z || num9 >= line2V2.z);
		}

		// Token: 0x0600550F RID: 21775 RVA: 0x001B6B92 File Offset: 0x001B4D92
		public static bool IsQuadSelfIntersecting(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
		{
			return GenGeo.LinesIntersect(p1, p2, p3, p4) || GenGeo.LinesIntersect(p1, p2, p4, p3) || GenGeo.LinesIntersect(p2, p3, p1, p4) || GenGeo.LinesIntersect(p3, p4, p1, p2);
		}

		// Token: 0x06005510 RID: 21776 RVA: 0x001B6BC0 File Offset: 0x001B4DC0
		public static bool IsQuadSelfIntersecting(IntVec3 p1, IntVec3 p2, IntVec3 p3, IntVec3 p4)
		{
			return GenGeo.IsQuadSelfIntersecting(p1.ToVector3(), p2.ToVector3(), p3.ToVector3(), p4.ToVector3());
		}

		// Token: 0x06005511 RID: 21777 RVA: 0x001B6BE4 File Offset: 0x001B4DE4
		public static bool LineRectIntersection(Vector2 pointOnLine, float slope, Vector2 rectA, Vector2 rectB, ref List<Vector2> intersections)
		{
			if (intersections == null)
			{
				intersections = new List<Vector2>();
			}
			else
			{
				intersections.Clear();
			}
			GenGeo.<>c__DisplayClass6_0 CS$<>8__locals1;
			CS$<>8__locals1.xMin = Mathf.Min(rectA.x, rectB.x);
			CS$<>8__locals1.xMax = Mathf.Max(rectA.x, rectB.x);
			CS$<>8__locals1.yMin = Mathf.Min(rectA.y, rectB.y);
			CS$<>8__locals1.yMax = Mathf.Max(rectA.y, rectB.y);
			float y = slope * (CS$<>8__locals1.xMin - pointOnLine.x) + pointOnLine.y;
			Vector2 vector = new Vector2(CS$<>8__locals1.xMin, y);
			if (GenGeo.<LineRectIntersection>g__IsPointInRect|6_0(vector, ref CS$<>8__locals1))
			{
				intersections.Add(vector);
			}
			float y2 = slope * (CS$<>8__locals1.xMax - pointOnLine.x) + pointOnLine.y;
			Vector2 vector2 = new Vector2(CS$<>8__locals1.xMax, y2);
			if (GenGeo.<LineRectIntersection>g__IsPointInRect|6_0(vector2, ref CS$<>8__locals1))
			{
				intersections.Add(vector2);
			}
			if (slope != 0f)
			{
				float x = (CS$<>8__locals1.yMin - pointOnLine.y) / slope + pointOnLine.x;
				Vector2 vector3 = new Vector2(x, CS$<>8__locals1.yMin);
				if (GenGeo.<LineRectIntersection>g__IsPointInRect|6_0(vector3, ref CS$<>8__locals1))
				{
					intersections.Add(vector3);
				}
				float x2 = (CS$<>8__locals1.yMax - pointOnLine.y) / slope + pointOnLine.x;
				Vector2 vector4 = new Vector2(x2, CS$<>8__locals1.yMax);
				if (GenGeo.<LineRectIntersection>g__IsPointInRect|6_0(vector4, ref CS$<>8__locals1))
				{
					intersections.Add(vector4);
				}
			}
			return intersections.Count > 0;
		}

		// Token: 0x06005512 RID: 21778 RVA: 0x001B6D68 File Offset: 0x001B4F68
		public static bool IntersectLineCircle(Vector2 center, float radius, Vector2 lineA, Vector2 lineB)
		{
			Vector2 lhs = center - lineA;
			Vector2 vector = lineB - lineA;
			float num = Vector2.Dot(vector, vector);
			float num2 = Vector2.Dot(lhs, vector) / num;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			else if (num2 > 1f)
			{
				num2 = 1f;
			}
			Vector2 vector2 = vector * num2 + lineA - center;
			return Vector2.Dot(vector2, vector2) <= radius * radius;
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x001B6DD4 File Offset: 0x001B4FD4
		public static bool IntersectLineCircleOutline(Vector2 center, float radius, Vector2 lineA, Vector2 lineB)
		{
			bool flag = (lineA - center).sqrMagnitude <= radius * radius;
			bool flag2 = (lineB - center).sqrMagnitude <= radius * radius;
			return (!flag || !flag2) && GenGeo.IntersectLineCircle(center, radius, lineA, lineB);
		}

		// Token: 0x06005514 RID: 21780 RVA: 0x001B6E20 File Offset: 0x001B5020
		public static Vector3 RegularPolygonVertexPositionVec3(int polygonVertices, int vertexIndex)
		{
			Vector2 vector = GenGeo.RegularPolygonVertexPosition(polygonVertices, vertexIndex, 0f);
			return new Vector3(vector.x, 0f, vector.y);
		}

		// Token: 0x06005515 RID: 21781 RVA: 0x001B6E50 File Offset: 0x001B5050
		public static Vector2 RegularPolygonVertexPosition(int polygonVertices, int vertexIndex, float angleOffset = 0f)
		{
			if (vertexIndex < 0 || vertexIndex >= polygonVertices)
			{
				Log.Warning("Vertex index out of bounds. polygonVertices=" + polygonVertices.ToString() + " vertexIndex=" + vertexIndex.ToString());
				return Vector2.zero;
			}
			if (polygonVertices == 1)
			{
				return Vector2.zero;
			}
			return GenGeo.CalculatePolygonVertexPosition(polygonVertices, vertexIndex, angleOffset);
		}

		// Token: 0x06005516 RID: 21782 RVA: 0x001B6EA0 File Offset: 0x001B50A0
		private static Vector2 CalculatePolygonVertexPosition(int polygonVertices, int vertexIndex, float angleOffset = 0f)
		{
			float num = 6.2831855f / (float)polygonVertices * (float)vertexIndex;
			num += 3.1415927f;
			num += 0.017453292f * angleOffset;
			return new Vector3(Mathf.Cos(num), Mathf.Sin(num));
		}

		// Token: 0x06005517 RID: 21783 RVA: 0x001B6EE4 File Offset: 0x001B50E4
		public static Vector2 InverseQuadBilinear(Vector2 p, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
		{
			float num = (p0 - p).Cross(p0 - p2);
			float num2 = ((p0 - p).Cross(p1 - p3) + (p1 - p).Cross(p0 - p2)) / 2f;
			float num3 = (p1 - p).Cross(p1 - p3);
			float num4 = num2 * num2 - num * num3;
			if (num4 < 0f)
			{
				return new Vector2(-1f, -1f);
			}
			num4 = Mathf.Sqrt(num4);
			float num5;
			if (Mathf.Abs(num - 2f * num2 + num3) < 0.0001f)
			{
				num5 = num / (num - num3);
			}
			else
			{
				float num6 = (num - num2 + num4) / (num - 2f * num2 + num3);
				float num7 = (num - num2 - num4) / (num - 2f * num2 + num3);
				if (Mathf.Abs(num6 - 0.5f) < Mathf.Abs(num7 - 0.5f))
				{
					num5 = num6;
				}
				else
				{
					num5 = num7;
				}
			}
			float num8 = (1f - num5) * (p0.x - p2.x) + num5 * (p1.x - p3.x);
			float num9 = (1f - num5) * (p0.y - p2.y) + num5 * (p1.y - p3.y);
			if (Mathf.Abs(num8) < Mathf.Abs(num9))
			{
				return new Vector2(num5, ((1f - num5) * (p0.y - p.y) + num5 * (p1.y - p.y)) / num9);
			}
			return new Vector2(num5, ((1f - num5) * (p0.x - p.x) + num5 * (p1.x - p.x)) / num8);
		}

		// Token: 0x06005518 RID: 21784 RVA: 0x001B70A4 File Offset: 0x001B52A4
		public static int GetAdjacencyScore(this CellRect rect, CellRect other)
		{
			if (rect.Overlaps(other))
			{
				return 0;
			}
			if (rect.maxZ == other.minZ - 1 && rect.minX < other.maxX && rect.maxX > other.minX)
			{
				int num = Mathf.Max(rect.minX, other.minX);
				return Mathf.Min(rect.maxX, other.maxX) - num;
			}
			if (rect.minZ == other.maxZ + 1 && rect.minX < other.maxX && rect.maxX > other.minX)
			{
				int num2 = Mathf.Max(rect.minX, other.minX);
				return Mathf.Min(rect.maxX, other.maxX) - num2;
			}
			if (rect.minX == other.maxX + 1 && rect.minZ < other.maxZ && rect.maxZ > other.minZ)
			{
				int num3 = Mathf.Max(rect.minZ, other.minZ);
				return Mathf.Min(rect.maxZ, other.maxZ) - num3;
			}
			if (rect.maxX == other.minX - 1 && rect.minZ < other.maxZ && rect.maxZ > other.minZ)
			{
				int num4 = Mathf.Max(rect.minZ, other.minZ);
				return Mathf.Min(rect.maxZ, other.maxZ) - num4;
			}
			return 0;
		}

		// Token: 0x06005519 RID: 21785 RVA: 0x001B7208 File Offset: 0x001B5408
		public static CellRect ExpandToFit(this CellRect rect, IntVec3 position)
		{
			if (rect.Contains(position))
			{
				return rect;
			}
			rect.minX = Mathf.Min(rect.minX, position.x);
			rect.minZ = Mathf.Min(rect.minZ, position.z);
			rect.maxX = Mathf.Max(rect.maxX, position.x);
			rect.maxZ = Mathf.Max(rect.maxZ, position.z);
			return rect;
		}

		// Token: 0x0600551A RID: 21786 RVA: 0x001B7282 File Offset: 0x001B5482
		[CompilerGenerated]
		internal static bool <LineRectIntersection>g__IsPointInRect|6_0(Vector2 point, ref GenGeo.<>c__DisplayClass6_0 A_1)
		{
			return point.x >= A_1.xMin && point.x <= A_1.xMax && point.y >= A_1.yMin && point.y <= A_1.yMax;
		}
	}
}
