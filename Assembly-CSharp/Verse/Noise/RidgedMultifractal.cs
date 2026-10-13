using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000DAE RID: 3502
	public class RidgedMultifractal : ModuleBase
	{
		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x0600592E RID: 22830 RVA: 0x001CAE71 File Offset: 0x001C9071
		// (set) Token: 0x0600592F RID: 22831 RVA: 0x001CAE79 File Offset: 0x001C9079
		public double Frequency
		{
			get
			{
				return this.frequency;
			}
			set
			{
				this.frequency = value;
			}
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06005930 RID: 22832 RVA: 0x001CAE82 File Offset: 0x001C9082
		// (set) Token: 0x06005931 RID: 22833 RVA: 0x001CAE8A File Offset: 0x001C908A
		public double Lacunarity
		{
			get
			{
				return this.lacunarity;
			}
			set
			{
				this.lacunarity = value;
				this.UpdateWeights();
			}
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x06005932 RID: 22834 RVA: 0x001CAE99 File Offset: 0x001C9099
		// (set) Token: 0x06005933 RID: 22835 RVA: 0x001CAEA1 File Offset: 0x001C90A1
		public QualityMode Quality
		{
			get
			{
				return this.quality;
			}
			set
			{
				this.quality = value;
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x06005934 RID: 22836 RVA: 0x001CAEAA File Offset: 0x001C90AA
		// (set) Token: 0x06005935 RID: 22837 RVA: 0x001CAEB2 File Offset: 0x001C90B2
		public int OctaveCount
		{
			get
			{
				return this.octaveCount;
			}
			set
			{
				this.octaveCount = Mathf.Clamp(value, 1, 30);
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06005936 RID: 22838 RVA: 0x001CAEC3 File Offset: 0x001C90C3
		// (set) Token: 0x06005937 RID: 22839 RVA: 0x001CAECB File Offset: 0x001C90CB
		public int Seed
		{
			get
			{
				return this.seed;
			}
			set
			{
				this.seed = value;
			}
		}

		// Token: 0x06005938 RID: 22840 RVA: 0x001CAED4 File Offset: 0x001C90D4
		public RidgedMultifractal() : base(0)
		{
			this.UpdateWeights();
		}

		// Token: 0x06005939 RID: 22841 RVA: 0x001CAF28 File Offset: 0x001C9128
		public RidgedMultifractal(double frequency, double lacunarity, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x0600593A RID: 22842 RVA: 0x001CAF9C File Offset: 0x001C919C
		private void UpdateWeights()
		{
			double num = 1.0;
			for (int i = 0; i < 30; i++)
			{
				this.weights[i] = Math.Pow(num, -1.0);
				num *= this.lacunarity;
			}
		}

		// Token: 0x0600593B RID: 22843 RVA: 0x001CAFE0 File Offset: 0x001C91E0
		public override double GetValue(double x, double y, double z)
		{
			x *= this.frequency;
			y *= this.frequency;
			z *= this.frequency;
			double num = 0.0;
			double num2 = 1.0;
			double num3 = 1.0;
			double num4 = 2.0;
			for (int i = 0; i < this.octaveCount; i++)
			{
				double x2 = Utils.MakeInt32Range(x);
				double y2 = Utils.MakeInt32Range(y);
				double z2 = Utils.MakeInt32Range(z);
				long num5 = (long)(this.seed + i & int.MaxValue);
				double num6 = Utils.GradientCoherentNoise3D(x2, y2, z2, num5, this.quality);
				num6 = Math.Abs(num6);
				num6 = num3 - num6;
				num6 *= num6;
				num6 *= num2;
				num2 = num6 * num4;
				if (num2 > 1.0)
				{
					num2 = 1.0;
				}
				if (num2 < 0.0)
				{
					num2 = 0.0;
				}
				num += num6 * this.weights[i];
				x *= this.lacunarity;
				y *= this.lacunarity;
				z *= this.lacunarity;
			}
			return num * 1.25 - 1.0;
		}

		// Token: 0x0400408F RID: 16527
		private double frequency = 1.0;

		// Token: 0x04004090 RID: 16528
		private double lacunarity = 2.0;

		// Token: 0x04004091 RID: 16529
		private QualityMode quality = QualityMode.Medium;

		// Token: 0x04004092 RID: 16530
		private int octaveCount = 6;

		// Token: 0x04004093 RID: 16531
		private int seed;

		// Token: 0x04004094 RID: 16532
		private double[] weights = new double[30];
	}
}
