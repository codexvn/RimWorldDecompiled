using System;
using Unity.Burst;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000CB RID: 203
	[BurstCompile]
	public struct ColorInt : IEquatable<ColorInt>
	{
		// Token: 0x060004F6 RID: 1270 RVA: 0x0001B223 File Offset: 0x00019423
		public ColorInt(int r, int g, int b)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = 255;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001B245 File Offset: 0x00019445
		public ColorInt(int r, int g, int b, int a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001B264 File Offset: 0x00019464
		public ColorInt(Color32 col)
		{
			this.r = (int)col.r;
			this.g = (int)col.g;
			this.b = (int)col.b;
			this.a = (int)col.a;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001B298 File Offset: 0x00019498
		public ColorInt(Color color)
		{
			this.r = (int)ColorInt.FloatToByte(color.r);
			this.g = (int)ColorInt.FloatToByte(color.g);
			this.b = (int)ColorInt.FloatToByte(color.b);
			this.a = (int)ColorInt.FloatToByte(color.a);
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001B2E9 File Offset: 0x000194E9
		private static byte FloatToByte(float value)
		{
			if (value >= 1f)
			{
				return byte.MaxValue;
			}
			if (value <= 0f)
			{
				return 0;
			}
			return (byte)Mathf.Floor(value * 256f);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001B310 File Offset: 0x00019510
		public static ColorInt operator +(ColorInt colA, ColorInt colB)
		{
			return new ColorInt(colA.r + colB.r, colA.g + colB.g, colA.b + colB.b, colA.a + colB.a);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001B34B File Offset: 0x0001954B
		public static ColorInt operator +(ColorInt colA, Color32 colB)
		{
			return new ColorInt(colA.r + (int)colB.r, colA.g + (int)colB.g, colA.b + (int)colB.b, colA.a + (int)colB.a);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001B386 File Offset: 0x00019586
		public static ColorInt operator -(ColorInt a, ColorInt b)
		{
			return new ColorInt(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001B3C1 File Offset: 0x000195C1
		public static ColorInt operator *(ColorInt a, int b)
		{
			return new ColorInt(a.r * b, a.g * b, a.b * b, a.a * b);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001B3E8 File Offset: 0x000195E8
		public static ColorInt operator *(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r * b), (int)((float)a.g * b), (int)((float)a.b * b), (int)((float)a.a * b));
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001B417 File Offset: 0x00019617
		public static ColorInt operator /(ColorInt a, int b)
		{
			return new ColorInt(a.r / b, a.g / b, a.b / b, a.a / b);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001B43E File Offset: 0x0001963E
		public static ColorInt operator /(ColorInt a, float b)
		{
			return new ColorInt((int)((float)a.r / b), (int)((float)a.g / b), (int)((float)a.b / b), (int)((float)a.a / b));
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001B46D File Offset: 0x0001966D
		public static bool operator ==(ColorInt a, ColorInt b)
		{
			return a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001B4A9 File Offset: 0x000196A9
		public static bool operator !=(ColorInt a, ColorInt b)
		{
			return a.r != b.r || a.g != b.g || a.b != b.b || a.a != b.a;
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001B4E8 File Offset: 0x000196E8
		public override bool Equals(object o)
		{
			if (o is ColorInt)
			{
				ColorInt other = (ColorInt)o;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001B50F File Offset: 0x0001970F
		public bool Equals(ColorInt other)
		{
			return this == other;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001B520 File Offset: 0x00019720
		public override int GetHashCode()
		{
			return this.r + this.g * 256 + this.b * 256 * 256 + this.a * 256 * 256 * 256;
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001B56C File Offset: 0x0001976C
		public override string ToString()
		{
			return string.Format("{0}, {1}, {2}, {3}", new object[]
			{
				this.r,
				this.g,
				this.b,
				this.a
			});
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001B5C4 File Offset: 0x000197C4
		public void ClampToNonNegative()
		{
			if (this.r < 0)
			{
				this.r = 0;
			}
			if (this.g < 0)
			{
				this.g = 0;
			}
			if (this.b < 0)
			{
				this.b = 0;
			}
			if (this.a < 0)
			{
				this.a = 0;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x0001B614 File Offset: 0x00019814
		public Color ToColor
		{
			get
			{
				return new Color
				{
					r = (float)this.r / 255f,
					g = (float)this.g / 255f,
					b = (float)this.b / 255f,
					a = (float)this.a / 255f
				};
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001B67C File Offset: 0x0001987C
		public Color32 ProjectToColor32()
		{
			Color32 result = default(Color32);
			if (this.a > 255)
			{
				result.a = byte.MaxValue;
			}
			else
			{
				result.a = (byte)this.a;
			}
			int num = this.r;
			if (this.g > num)
			{
				num = this.g;
			}
			if (this.b > num)
			{
				num = this.b;
			}
			if (num > 255)
			{
				result.r = (byte)(this.r * 255 / num);
				result.g = (byte)(this.g * 255 / num);
				result.b = (byte)(this.b * 255 / num);
			}
			else
			{
				result.r = (byte)this.r;
				result.g = (byte)this.g;
				result.b = (byte)this.b;
			}
			return result;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001B758 File Offset: 0x00019958
		[BurstCompile]
		public void ProjectToColor32Fast(out Color32 outColor)
		{
			outColor = default(Color32);
			if (this.a > 255)
			{
				outColor.a = byte.MaxValue;
			}
			else
			{
				outColor.a = (byte)this.a;
			}
			int num = this.r;
			if (this.g > num)
			{
				num = this.g;
			}
			if (this.b > num)
			{
				num = this.b;
			}
			if (num > 255)
			{
				outColor.r = (byte)(this.r * 255 / num);
				outColor.g = (byte)(this.g * 255 / num);
				outColor.b = (byte)(this.b * 255 / num);
				return;
			}
			outColor.r = (byte)this.r;
			outColor.g = (byte)this.g;
			outColor.b = (byte)this.b;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001B82C File Offset: 0x00019A2C
		public void SetHueSaturation(float hue, float sat)
		{
			float v = (float)Mathf.Max(new int[]
			{
				this.r,
				this.g,
				this.b
			}) / 255f;
			ColorInt colorInt = ColorInt.FromHdrColor(Color.HSVToRGB(hue, sat, v, true), null);
			this.r = colorInt.r;
			this.g = colorInt.g;
			this.b = colorInt.b;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001B8A4 File Offset: 0x00019AA4
		public static ColorInt FromHdrColor(Color color, float? alphaOverride = null)
		{
			return new ColorInt
			{
				r = Mathf.RoundToInt(color.r * 255f),
				g = Mathf.RoundToInt(color.g * 255f),
				b = Mathf.RoundToInt(color.b * 255f),
				a = Mathf.RoundToInt((alphaOverride ?? color.a) * 255f)
			};
		}

		// Token: 0x040002E0 RID: 736
		public int r;

		// Token: 0x040002E1 RID: 737
		public int g;

		// Token: 0x040002E2 RID: 738
		public int b;

		// Token: 0x040002E3 RID: 739
		public int a;
	}
}
