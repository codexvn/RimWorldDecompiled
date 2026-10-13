using System;

namespace Verse.Noise
{
	// Token: 0x02000DB8 RID: 3512
	public class Cache : ModuleBase
	{
		// Token: 0x0600597E RID: 22910 RVA: 0x001CA658 File Offset: 0x001C8858
		public Cache() : base(1)
		{
		}

		// Token: 0x0600597F RID: 22911 RVA: 0x001CC355 File Offset: 0x001CA555
		public Cache(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x17000E39 RID: 3641
		public override ModuleBase this[int index]
		{
			get
			{
				return base[index];
			}
			set
			{
				base[index] = value;
				this.m_cached = false;
			}
		}

		// Token: 0x06005982 RID: 22914 RVA: 0x001CC4B4 File Offset: 0x001CA6B4
		public override double GetValue(double x, double y, double z)
		{
			if (!this.m_cached || this.m_x != x || this.m_y != y || this.m_z != z)
			{
				this.m_value = this.modules[0].GetValue(x, y, z);
				this.m_x = x;
				this.m_y = y;
				this.m_z = z;
			}
			this.m_cached = true;
			return this.m_value;
		}

		// Token: 0x040040BA RID: 16570
		private double m_value;

		// Token: 0x040040BB RID: 16571
		private bool m_cached;

		// Token: 0x040040BC RID: 16572
		private double m_x;

		// Token: 0x040040BD RID: 16573
		private double m_y;

		// Token: 0x040040BE RID: 16574
		private double m_z;
	}
}
