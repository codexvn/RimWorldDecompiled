using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x02003EBF RID: 16063
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000E")]
	public class ZlibException : Exception
	{
		// Token: 0x06016084 RID: 90244 RVA: 0x00698B15 File Offset: 0x00696D15
		public ZlibException()
		{
		}

		// Token: 0x06016085 RID: 90245 RVA: 0x00698B1D File Offset: 0x00696D1D
		public ZlibException(string s) : base(s)
		{
		}
	}
}
