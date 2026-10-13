using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000DCD RID: 3533
	public class Terrace : ModuleBase
	{
		// Token: 0x060059F0 RID: 23024 RVA: 0x001CD359 File Offset: 0x001CB559
		public Terrace() : base(1)
		{
		}

		// Token: 0x060059F1 RID: 23025 RVA: 0x001CD36D File Offset: 0x001CB56D
		public Terrace(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060059F2 RID: 23026 RVA: 0x001CD38A File Offset: 0x001CB58A
		public Terrace(bool inverted, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.IsInverted = inverted;
		}

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x060059F3 RID: 23027 RVA: 0x001CD3AE File Offset: 0x001CB5AE
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x060059F4 RID: 23028 RVA: 0x001CD3BB File Offset: 0x001CB5BB
		public List<double> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x060059F5 RID: 23029 RVA: 0x001CD3C3 File Offset: 0x001CB5C3
		// (set) Token: 0x060059F6 RID: 23030 RVA: 0x001CD3CB File Offset: 0x001CB5CB
		public bool IsInverted
		{
			get
			{
				return this.m_inverted;
			}
			set
			{
				this.m_inverted = value;
			}
		}

		// Token: 0x060059F7 RID: 23031 RVA: 0x001CD3D4 File Offset: 0x001CB5D4
		public void Add(double input)
		{
			if (!this.m_data.Contains(input))
			{
				this.m_data.Add(input);
			}
			this.m_data.Sort((double lhs, double rhs) => lhs.CompareTo(rhs));
		}

		// Token: 0x060059F8 RID: 23032 RVA: 0x001CD425 File Offset: 0x001CB625
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x060059F9 RID: 23033 RVA: 0x001CD434 File Offset: 0x001CB634
		public void Generate(int steps)
		{
			if (steps < 2)
			{
				throw new ArgumentException("Need at least two steps");
			}
			this.Clear();
			double num = 2.0 / ((double)steps - 1.0);
			double num2 = -1.0;
			for (int i = 0; i < steps; i++)
			{
				this.Add(num2);
				num2 += num;
			}
		}

		// Token: 0x060059FA RID: 23034 RVA: 0x001CD490 File Offset: 0x001CB690
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			int num = 0;
			while (num < this.m_data.Count && value >= this.m_data[num])
			{
				num++;
			}
			int num2 = Mathf.Clamp(num - 1, 0, this.m_data.Count - 1);
			int num3 = Mathf.Clamp(num, 0, this.m_data.Count - 1);
			if (num2 == num3)
			{
				return this.m_data[num3];
			}
			double num4 = this.m_data[num2];
			double num5 = this.m_data[num3];
			double num6 = (value - num4) / (num5 - num4);
			if (this.m_inverted)
			{
				num6 = 1.0 - num6;
				double num7 = num4;
				num4 = num5;
				num5 = num7;
			}
			num6 *= num6;
			return Utils.InterpolateLinear(num4, num5, num6);
		}

		// Token: 0x040040DF RID: 16607
		private List<double> m_data = new List<double>();

		// Token: 0x040040E0 RID: 16608
		private bool m_inverted;
	}
}
