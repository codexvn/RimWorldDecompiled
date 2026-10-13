using System;

namespace Verse.Noise
{
	// Token: 0x02000DAC RID: 3500
	public class Const : ModuleBase
	{
		// Token: 0x17000E1C RID: 3612
		// (get) Token: 0x06005914 RID: 22804 RVA: 0x001CAB35 File Offset: 0x001C8D35
		// (set) Token: 0x06005915 RID: 22805 RVA: 0x001CAB3D File Offset: 0x001C8D3D
		public double Value
		{
			get
			{
				return this.val;
			}
			set
			{
				this.val = value;
			}
		}

		// Token: 0x06005916 RID: 22806 RVA: 0x001CA600 File Offset: 0x001C8800
		public Const() : base(0)
		{
		}

		// Token: 0x06005917 RID: 22807 RVA: 0x001CAB46 File Offset: 0x001C8D46
		public Const(double value) : base(0)
		{
			this.Value = value;
		}

		// Token: 0x06005918 RID: 22808 RVA: 0x001CAB35 File Offset: 0x001C8D35
		public override double GetValue(double x, double y, double z)
		{
			return this.val;
		}

		// Token: 0x04004086 RID: 16518
		private double val;
	}
}
