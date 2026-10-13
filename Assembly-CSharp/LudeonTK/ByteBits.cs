using System;
using System.Runtime.CompilerServices;

namespace LudeonTK
{
	// Token: 0x020000B2 RID: 178
	public struct ByteBits
	{
		// Token: 0x170000A4 RID: 164
		public bool this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ((int)this.intValue & 1 << index) != 0;
			}
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (value)
				{
					this.intValue = (byte)((int)this.intValue | 1 << index);
					return;
				}
				this.intValue = (byte)((int)this.intValue & ~(1 << index));
			}
		}

		// Token: 0x04000263 RID: 611
		private byte intValue;
	}
}
