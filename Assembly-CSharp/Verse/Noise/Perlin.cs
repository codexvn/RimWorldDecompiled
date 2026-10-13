using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000DAD RID: 3501
	public class Perlin : ModuleBase
	{
		// Token: 0x17000E1D RID: 3613
		// (get) Token: 0x06005919 RID: 22809 RVA: 0x001CAB56 File Offset: 0x001C8D56
		// (set) Token: 0x0600591A RID: 22810 RVA: 0x001CAB5E File Offset: 0x001C8D5E
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

		// Token: 0x17000E1E RID: 3614
		// (get) Token: 0x0600591B RID: 22811 RVA: 0x001CAB67 File Offset: 0x001C8D67
		// (set) Token: 0x0600591C RID: 22812 RVA: 0x001CAB6F File Offset: 0x001C8D6F
		public double Lacunarity
		{
			get
			{
				return this.lacunarity;
			}
			set
			{
				this.lacunarity = value;
			}
		}

		// Token: 0x17000E1F RID: 3615
		// (get) Token: 0x0600591D RID: 22813 RVA: 0x001CAB78 File Offset: 0x001C8D78
		// (set) Token: 0x0600591E RID: 22814 RVA: 0x001CAB80 File Offset: 0x001C8D80
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

		// Token: 0x17000E20 RID: 3616
		// (get) Token: 0x0600591F RID: 22815 RVA: 0x001CAB89 File Offset: 0x001C8D89
		// (set) Token: 0x06005920 RID: 22816 RVA: 0x001CAB91 File Offset: 0x001C8D91
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

		// Token: 0x17000E21 RID: 3617
		// (get) Token: 0x06005921 RID: 22817 RVA: 0x001CABA2 File Offset: 0x001C8DA2
		// (set) Token: 0x06005922 RID: 22818 RVA: 0x001CABAA File Offset: 0x001C8DAA
		public double Persistence
		{
			get
			{
				return this.persistence;
			}
			set
			{
				this.persistence = value;
			}
		}

		// Token: 0x17000E22 RID: 3618
		// (get) Token: 0x06005923 RID: 22819 RVA: 0x001CABB3 File Offset: 0x001C8DB3
		// (set) Token: 0x06005924 RID: 22820 RVA: 0x001CABBB File Offset: 0x001C8DBB
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

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x06005925 RID: 22821 RVA: 0x001CABC4 File Offset: 0x001C8DC4
		// (set) Token: 0x06005926 RID: 22822 RVA: 0x001CABCC File Offset: 0x001C8DCC
		public bool Invert
		{
			get
			{
				return this.invert;
			}
			set
			{
				this.invert = value;
			}
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06005927 RID: 22823 RVA: 0x001CABD5 File Offset: 0x001C8DD5
		// (set) Token: 0x06005928 RID: 22824 RVA: 0x001CABDD File Offset: 0x001C8DDD
		public bool Normalized
		{
			get
			{
				return this.normalized;
			}
			set
			{
				this.normalized = value;
			}
		}

		// Token: 0x06005929 RID: 22825 RVA: 0x001CABE8 File Offset: 0x001C8DE8
		public Perlin() : base(0)
		{
		}

		// Token: 0x0600592A RID: 22826 RVA: 0x001CAC38 File Offset: 0x001C8E38
		public Perlin(double frequency, double lacunarity, double persistence, int octaves, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Quality = quality;
		}

		// Token: 0x0600592B RID: 22827 RVA: 0x001CACB4 File Offset: 0x001C8EB4
		public Perlin(double frequency, double lacunarity, double persistence, int octaves, bool normalized, bool invert, int seed, QualityMode quality) : base(0)
		{
			this.Frequency = frequency;
			this.Lacunarity = lacunarity;
			this.OctaveCount = octaves;
			this.Persistence = persistence;
			this.Seed = seed;
			this.Normalized = normalized;
			this.Invert = invert;
			this.Quality = quality;
		}

		// Token: 0x0600592C RID: 22828 RVA: 0x001CAD40 File Offset: 0x001C8F40
		public static double GetValue(double x, double y, double z, double frequency, int seed, double lacunarity = 2.0, double persistence = 0.5, int octaveCount = 6, bool normalized = false, bool invert = false, QualityMode quality = QualityMode.Medium)
		{
			double num = 0.0;
			double num2 = 1.0;
			x *= frequency;
			y *= frequency;
			z *= frequency;
			for (int i = 0; i < octaveCount; i++)
			{
				double x2 = Utils.MakeInt32Range(x);
				double y2 = Utils.MakeInt32Range(y);
				double z2 = Utils.MakeInt32Range(z);
				long num3 = (long)(seed + i) & (long)((ulong)-1);
				double num4 = Utils.GradientCoherentNoise3D(x2, y2, z2, num3, quality);
				num += num4 * num2;
				x *= lacunarity;
				y *= lacunarity;
				z *= lacunarity;
				num2 *= persistence;
			}
			if (normalized)
			{
				num = num * 0.5 + 0.5;
				if (num < 0.0)
				{
					num = 0.0;
				}
				if (num > 1.0)
				{
					num = 1.0;
				}
			}
			if (invert)
			{
				num = 1.0 - num;
			}
			return num;
		}

		// Token: 0x0600592D RID: 22829 RVA: 0x001CAE2C File Offset: 0x001C902C
		public override double GetValue(double x, double y, double z)
		{
			return Perlin.GetValue(x, y, z, this.frequency, this.seed, this.lacunarity, this.persistence, this.octaveCount, this.normalized, this.invert, this.quality);
		}

		// Token: 0x04004087 RID: 16519
		private double frequency = 1.0;

		// Token: 0x04004088 RID: 16520
		private double lacunarity = 2.0;

		// Token: 0x04004089 RID: 16521
		private QualityMode quality = QualityMode.Medium;

		// Token: 0x0400408A RID: 16522
		private int octaveCount = 6;

		// Token: 0x0400408B RID: 16523
		private double persistence = 0.5;

		// Token: 0x0400408C RID: 16524
		private int seed;

		// Token: 0x0400408D RID: 16525
		private bool normalized;

		// Token: 0x0400408E RID: 16526
		private bool invert;
	}
}
