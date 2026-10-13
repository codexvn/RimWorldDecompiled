using System;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000DB3 RID: 3507
	public class Noise2D : IDisposable
	{
		// Token: 0x06005951 RID: 22865 RVA: 0x001CB7AA File Offset: 0x001C99AA
		protected Noise2D()
		{
		}

		// Token: 0x06005952 RID: 22866 RVA: 0x001CB7C4 File Offset: 0x001C99C4
		public Noise2D(int size) : this(size, size, null)
		{
		}

		// Token: 0x06005953 RID: 22867 RVA: 0x001CB7CF File Offset: 0x001C99CF
		public Noise2D(int size, ModuleBase generator) : this(size, size, generator)
		{
		}

		// Token: 0x06005954 RID: 22868 RVA: 0x001CB7DA File Offset: 0x001C99DA
		public Noise2D(int width, int height) : this(width, height, null)
		{
		}

		// Token: 0x06005955 RID: 22869 RVA: 0x001CB7E8 File Offset: 0x001C99E8
		public Noise2D(int width, int height, ModuleBase generator)
		{
			this.m_generator = generator;
			this.m_width = width;
			this.m_height = height;
			this.m_data = new float[width, height];
			this.m_ucWidth = width + this.m_ucBorder * 2;
			this.m_ucHeight = height + this.m_ucBorder * 2;
			this.m_ucData = new float[width + this.m_ucBorder * 2, height + this.m_ucBorder * 2];
		}

		// Token: 0x17000E32 RID: 3634
		public float this[int x, int y, bool isCropped = true]
		{
			get
			{
				if (isCropped)
				{
					if (x < 0 && x >= this.m_width)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_height)
					{
						throw new ArgumentOutOfRangeException("Inavlid y position");
					}
					return this.m_data[x, y];
				}
				else
				{
					if (x < 0 && x >= this.m_ucWidth)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_ucHeight)
					{
						throw new ArgumentOutOfRangeException("Inavlid y position");
					}
					return this.m_ucData[x, y];
				}
			}
			set
			{
				if (isCropped)
				{
					if (x < 0 && x >= this.m_width)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_height)
					{
						throw new ArgumentOutOfRangeException("Invalid y position");
					}
					this.m_data[x, y] = value;
					return;
				}
				else
				{
					if (x < 0 && x >= this.m_ucWidth)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_ucHeight)
					{
						throw new ArgumentOutOfRangeException("Inavlid y position");
					}
					this.m_ucData[x, y] = value;
					return;
				}
			}
		}

		// Token: 0x17000E33 RID: 3635
		// (get) Token: 0x06005958 RID: 22872 RVA: 0x001CB98B File Offset: 0x001C9B8B
		// (set) Token: 0x06005959 RID: 22873 RVA: 0x001CB993 File Offset: 0x001C9B93
		public float Border
		{
			get
			{
				return this.m_borderValue;
			}
			set
			{
				this.m_borderValue = value;
			}
		}

		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x0600595A RID: 22874 RVA: 0x001CB99C File Offset: 0x001C9B9C
		// (set) Token: 0x0600595B RID: 22875 RVA: 0x001CB9A4 File Offset: 0x001C9BA4
		public ModuleBase Generator
		{
			get
			{
				return this.m_generator;
			}
			set
			{
				this.m_generator = value;
			}
		}

		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x0600595C RID: 22876 RVA: 0x001CB9AD File Offset: 0x001C9BAD
		public int Height
		{
			get
			{
				return this.m_height;
			}
		}

		// Token: 0x17000E36 RID: 3638
		// (get) Token: 0x0600595D RID: 22877 RVA: 0x001CB9B5 File Offset: 0x001C9BB5
		public int Width
		{
			get
			{
				return this.m_width;
			}
		}

		// Token: 0x0600595E RID: 22878 RVA: 0x001CB9BD File Offset: 0x001C9BBD
		public float[,] GetNormalizedData(bool isCropped = true, int xCrop = 0, int yCrop = 0)
		{
			return this.GetData(isCropped, xCrop, yCrop, true);
		}

		// Token: 0x0600595F RID: 22879 RVA: 0x001CB9CC File Offset: 0x001C9BCC
		public float[,] GetData(bool isCropped = true, int xCrop = 0, int yCrop = 0, bool isNormalized = false)
		{
			int num;
			int num2;
			float[,] array;
			if (isCropped)
			{
				num = this.m_width;
				num2 = this.m_height;
				array = this.m_data;
			}
			else
			{
				num = this.m_ucWidth;
				num2 = this.m_ucHeight;
				array = this.m_ucData;
			}
			num -= xCrop;
			num2 -= yCrop;
			float[,] array2 = new float[num, num2];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					float num3;
					if (isNormalized)
					{
						num3 = (array[i, j] + 1f) / 2f;
					}
					else
					{
						num3 = array[i, j];
					}
					array2[i, j] = num3;
				}
			}
			return array2;
		}

		// Token: 0x06005960 RID: 22880 RVA: 0x001CBA71 File Offset: 0x001C9C71
		public void Clear()
		{
			this.Clear(0f);
		}

		// Token: 0x06005961 RID: 22881 RVA: 0x001CBA80 File Offset: 0x001C9C80
		public void Clear(float value)
		{
			for (int i = 0; i < this.m_width; i++)
			{
				for (int j = 0; j < this.m_height; j++)
				{
					this.m_data[i, j] = value;
				}
			}
		}

		// Token: 0x06005962 RID: 22882 RVA: 0x001CBABD File Offset: 0x001C9CBD
		private double GeneratePlanar(double x, double y)
		{
			return this.m_generator.GetValue(x, 0.0, y);
		}

		// Token: 0x06005963 RID: 22883 RVA: 0x001CBAD5 File Offset: 0x001C9CD5
		public void GeneratePlanar(double left, double right, double top, double bottom)
		{
			this.GeneratePlanar(left, right, top, bottom, true);
		}

		// Token: 0x06005964 RID: 22884 RVA: 0x001CBAE4 File Offset: 0x001C9CE4
		public void GeneratePlanar(double left, double right, double top, double bottom, bool isSeamless)
		{
			if (right <= left || bottom <= top)
			{
				throw new ArgumentException("Invalid right/left or bottom/top combination");
			}
			if (this.m_generator == null)
			{
				throw new ArgumentNullException("Generator is null");
			}
			double num = right - left;
			double num2 = bottom - top;
			double num3 = num / ((double)this.m_width - (double)this.m_ucBorder);
			double num4 = num2 / ((double)this.m_height - (double)this.m_ucBorder);
			double num5 = left;
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				double num6 = top;
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					float num7;
					if (isSeamless)
					{
						num7 = (float)this.GeneratePlanar(num5, num6);
					}
					else
					{
						double a = this.GeneratePlanar(num5, num6);
						double b = this.GeneratePlanar(num5 + num, num6);
						double a2 = this.GeneratePlanar(num5, num6 + num2);
						double b2 = this.GeneratePlanar(num5 + num, num6 + num2);
						double position = 1.0 - (num5 - left) / num;
						double position2 = 1.0 - (num6 - top) / num2;
						double a3 = Utils.InterpolateLinear(a, b, position);
						double b3 = Utils.InterpolateLinear(a2, b2, position);
						num7 = (float)Utils.InterpolateLinear(a3, b3, position2);
					}
					this.m_ucData[i, j] = num7;
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						this.m_data[i - this.m_ucBorder, j - this.m_ucBorder] = num7;
					}
					num6 += num4;
				}
				num5 += num3;
			}
		}

		// Token: 0x06005965 RID: 22885 RVA: 0x001CBC8C File Offset: 0x001C9E8C
		private double GenerateCylindrical(double angle, double height)
		{
			double x = Math.Cos(angle * 0.017453292519943295);
			double z = Math.Sin(angle * 0.017453292519943295);
			return this.m_generator.GetValue(x, height, z);
		}

		// Token: 0x06005966 RID: 22886 RVA: 0x001CBCCC File Offset: 0x001C9ECC
		public void GenerateCylindrical(double angleMin, double angleMax, double heightMin, double heightMax)
		{
			if (angleMax <= angleMin || heightMax <= heightMin)
			{
				throw new ArgumentException("Invalid angle or height parameters");
			}
			if (this.m_generator == null)
			{
				throw new ArgumentNullException("Generator is null");
			}
			double num = angleMax - angleMin;
			double num2 = heightMax - heightMin;
			double num3 = num / ((double)this.m_width - (double)this.m_ucBorder);
			double num4 = num2 / ((double)this.m_height - (double)this.m_ucBorder);
			double num5 = angleMin;
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				double num6 = heightMin;
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					this.m_ucData[i, j] = (float)this.GenerateCylindrical(num5, num6);
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						this.m_data[i - this.m_ucBorder, j - this.m_ucBorder] = (float)this.GenerateCylindrical(num5, num6);
					}
					num6 += num4;
				}
				num5 += num3;
			}
		}

		// Token: 0x06005967 RID: 22887 RVA: 0x001CBDEC File Offset: 0x001C9FEC
		private double GenerateSpherical(double lat, double lon)
		{
			double num = Math.Cos(0.017453292519943295 * lat);
			return this.m_generator.GetValue(num * Math.Cos(0.017453292519943295 * lon), Math.Sin(0.017453292519943295 * lat), num * Math.Sin(0.017453292519943295 * lon));
		}

		// Token: 0x06005968 RID: 22888 RVA: 0x001CBE4C File Offset: 0x001CA04C
		public void GenerateSpherical(double south, double north, double west, double east)
		{
			if (east <= west || north <= south)
			{
				throw new ArgumentException("Invalid east/west or north/south combination");
			}
			if (this.m_generator == null)
			{
				throw new ArgumentNullException("Generator is null");
			}
			double num = east - west;
			double num2 = north - south;
			double num3 = num / ((double)this.m_width - (double)this.m_ucBorder);
			double num4 = num2 / ((double)this.m_height - (double)this.m_ucBorder);
			double num5 = west;
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				double num6 = south;
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					this.m_ucData[i, j] = (float)this.GenerateSpherical(num6, num5);
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						this.m_data[i - this.m_ucBorder, j - this.m_ucBorder] = (float)this.GenerateSpherical(num6, num5);
					}
					num6 += num4;
				}
				num5 += num3;
			}
		}

		// Token: 0x06005969 RID: 22889 RVA: 0x001CBF69 File Offset: 0x001CA169
		public Texture2D GetTexture()
		{
			return this.GetTexture(GradientPresets.Grayscale);
		}

		// Token: 0x0600596A RID: 22890 RVA: 0x001CBF78 File Offset: 0x001CA178
		public Texture2D GetTexture(Gradient gradient)
		{
			Texture2D texture2D = new Texture2D(this.m_width, this.m_height);
			texture2D.name = "Noise2DTex";
			Color[] array = new Color[this.m_width * this.m_height];
			for (int i = 0; i < this.m_width; i++)
			{
				for (int j = 0; j < this.m_height; j++)
				{
					float num;
					if (!float.IsNaN(this.m_borderValue) && (i == 0 || i == this.m_width - this.m_ucBorder || j == 0 || j == this.m_height - this.m_ucBorder))
					{
						num = this.m_borderValue;
					}
					else
					{
						num = this.m_data[i, j];
					}
					array[i + j * this.m_width] = gradient.Evaluate((num + 1f) / 2f);
				}
			}
			texture2D.SetPixels(array);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x0600596B RID: 22891 RVA: 0x001CC06C File Offset: 0x001CA26C
		public Texture2D GetNormalMap(float intensity)
		{
			Texture2D texture2D = new Texture2D(this.m_width, this.m_height);
			texture2D.name = "Noise2DTex";
			Color[] array = new Color[this.m_width * this.m_height];
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					float num = (this.m_ucData[Mathf.Max(0, i - this.m_ucBorder), j] - this.m_ucData[Mathf.Min(i + this.m_ucBorder, this.m_height + this.m_ucBorder), j]) / 2f;
					float num2 = (this.m_ucData[i, Mathf.Max(0, j - this.m_ucBorder)] - this.m_ucData[i, Mathf.Min(j + this.m_ucBorder, this.m_width + this.m_ucBorder)]) / 2f;
					Vector3 a = new Vector3(num * intensity, 0f, 1f);
					Vector3 b = new Vector3(0f, num2 * intensity, 1f);
					Vector3 vector = a + b;
					vector.Normalize();
					Vector3 zero = Vector3.zero;
					zero.x = (vector.x + 1f) / 2f;
					zero.y = (vector.y + 1f) / 2f;
					zero.z = (vector.z + 1f) / 2f;
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						array[i - this.m_ucBorder + (j - this.m_ucBorder) * this.m_width] = new Color(zero.x, zero.y, zero.z);
					}
				}
			}
			texture2D.SetPixels(array);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x0600596C RID: 22892 RVA: 0x001CC275 File Offset: 0x001CA475
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x0600596D RID: 22893 RVA: 0x001CC27D File Offset: 0x001CA47D
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600596E RID: 22894 RVA: 0x001CC299 File Offset: 0x001CA499
		protected virtual bool Disposing()
		{
			if (this.m_data != null)
			{
				this.m_data = null;
			}
			this.m_width = 0;
			this.m_height = 0;
			return true;
		}

		// Token: 0x040040A5 RID: 16549
		public static readonly double South = -90.0;

		// Token: 0x040040A6 RID: 16550
		public static readonly double North = 90.0;

		// Token: 0x040040A7 RID: 16551
		public static readonly double West = -180.0;

		// Token: 0x040040A8 RID: 16552
		public static readonly double East = 180.0;

		// Token: 0x040040A9 RID: 16553
		public static readonly double AngleMin = -180.0;

		// Token: 0x040040AA RID: 16554
		public static readonly double AngleMax = 180.0;

		// Token: 0x040040AB RID: 16555
		public static readonly double Left = -1.0;

		// Token: 0x040040AC RID: 16556
		public static readonly double Right = 1.0;

		// Token: 0x040040AD RID: 16557
		public static readonly double Top = -1.0;

		// Token: 0x040040AE RID: 16558
		public static readonly double Bottom = 1.0;

		// Token: 0x040040AF RID: 16559
		private int m_width;

		// Token: 0x040040B0 RID: 16560
		private int m_height;

		// Token: 0x040040B1 RID: 16561
		private float[,] m_data;

		// Token: 0x040040B2 RID: 16562
		private int m_ucWidth;

		// Token: 0x040040B3 RID: 16563
		private int m_ucHeight;

		// Token: 0x040040B4 RID: 16564
		private int m_ucBorder = 1;

		// Token: 0x040040B5 RID: 16565
		private float[,] m_ucData;

		// Token: 0x040040B6 RID: 16566
		private float m_borderValue = float.NaN;

		// Token: 0x040040B7 RID: 16567
		private ModuleBase m_generator;

		// Token: 0x040040B8 RID: 16568
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed;
	}
}
