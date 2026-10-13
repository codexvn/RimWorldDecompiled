using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000DBA RID: 3514
	public class Curve : ModuleBase
	{
		// Token: 0x0600598C RID: 22924 RVA: 0x001CC64B File Offset: 0x001CA84B
		public Curve() : base(1)
		{
		}

		// Token: 0x0600598D RID: 22925 RVA: 0x001CC65F File Offset: 0x001CA85F
		public Curve(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x0600598E RID: 22926 RVA: 0x001CC67C File Offset: 0x001CA87C
		public int ControlPointCount
		{
			get
			{
				return this.m_data.Count;
			}
		}

		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x0600598F RID: 22927 RVA: 0x001CC689 File Offset: 0x001CA889
		public List<KeyValuePair<double, double>> ControlPoints
		{
			get
			{
				return this.m_data;
			}
		}

		// Token: 0x06005990 RID: 22928 RVA: 0x001CC694 File Offset: 0x001CA894
		public void Add(double input, double output)
		{
			KeyValuePair<double, double> item = new KeyValuePair<double, double>(input, output);
			if (!this.m_data.Contains(item))
			{
				this.m_data.Add(item);
			}
			this.m_data.Sort((KeyValuePair<double, double> lhs, KeyValuePair<double, double> rhs) => lhs.Key.CompareTo(rhs.Key));
		}

		// Token: 0x06005991 RID: 22929 RVA: 0x001CC6EE File Offset: 0x001CA8EE
		public void Clear()
		{
			this.m_data.Clear();
		}

		// Token: 0x06005992 RID: 22930 RVA: 0x001CC6FC File Offset: 0x001CA8FC
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			int num = 0;
			while (num < this.m_data.Count && value >= this.m_data[num].Key)
			{
				num++;
			}
			int index = Mathf.Clamp(num - 2, 0, this.m_data.Count - 1);
			int num2 = Mathf.Clamp(num - 1, 0, this.m_data.Count - 1);
			int num3 = Mathf.Clamp(num, 0, this.m_data.Count - 1);
			int index2 = Mathf.Clamp(num + 1, 0, this.m_data.Count - 1);
			if (num2 == num3)
			{
				return this.m_data[num2].Value;
			}
			double key = this.m_data[num2].Key;
			double key2 = this.m_data[num3].Key;
			double position = (value - key) / (key2 - key);
			return Utils.InterpolateCubic(this.m_data[index].Value, this.m_data[num2].Value, this.m_data[num3].Value, this.m_data[index2].Value, position);
		}

		// Token: 0x040040C1 RID: 16577
		private List<KeyValuePair<double, double>> m_data = new List<KeyValuePair<double, double>>();
	}
}
