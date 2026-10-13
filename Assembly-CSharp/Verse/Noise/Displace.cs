using System;

namespace Verse.Noise
{
	// Token: 0x02000DBC RID: 3516
	public class Displace : ModuleBase
	{
		// Token: 0x06005996 RID: 22934 RVA: 0x001CC887 File Offset: 0x001CAA87
		public Displace() : base(4)
		{
		}

		// Token: 0x06005997 RID: 22935 RVA: 0x001CC890 File Offset: 0x001CAA90
		public Displace(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z) : base(4)
		{
			this.modules[0] = input;
			this.modules[1] = x;
			this.modules[2] = y;
			this.modules[3] = z;
		}

		// Token: 0x17000E3E RID: 3646
		// (get) Token: 0x06005998 RID: 22936 RVA: 0x001CC8BE File Offset: 0x001CAABE
		// (set) Token: 0x06005999 RID: 22937 RVA: 0x001CC8C8 File Offset: 0x001CAAC8
		public ModuleBase X
		{
			get
			{
				return this.modules[1];
			}
			set
			{
				this.modules[1] = value;
			}
		}

		// Token: 0x17000E3F RID: 3647
		// (get) Token: 0x0600599A RID: 22938 RVA: 0x001CC428 File Offset: 0x001CA628
		// (set) Token: 0x0600599B RID: 22939 RVA: 0x001CC432 File Offset: 0x001CA632
		public ModuleBase Y
		{
			get
			{
				return this.modules[2];
			}
			set
			{
				this.modules[2] = value;
			}
		}

		// Token: 0x17000E40 RID: 3648
		// (get) Token: 0x0600599C RID: 22940 RVA: 0x001CC8D3 File Offset: 0x001CAAD3
		// (set) Token: 0x0600599D RID: 22941 RVA: 0x001CC8DD File Offset: 0x001CAADD
		public ModuleBase Z
		{
			get
			{
				return this.modules[3];
			}
			set
			{
				this.modules[3] = value;
			}
		}

		// Token: 0x0600599E RID: 22942 RVA: 0x001CC8E8 File Offset: 0x001CAAE8
		public override double GetValue(double x, double y, double z)
		{
			double x2 = x + this.modules[1].GetValue(x, y, z);
			double y2 = y + this.modules[2].GetValue(x, y, z);
			double z2 = z + this.modules[3].GetValue(x, y, z);
			return this.modules[0].GetValue(x2, y2, z2);
		}
	}
}
