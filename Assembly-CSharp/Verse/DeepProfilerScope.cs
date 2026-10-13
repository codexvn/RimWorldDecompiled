using System;

namespace Verse
{
	// Token: 0x02000CA4 RID: 3236
	public class DeepProfilerScope : IDisposable
	{
		// Token: 0x060052FF RID: 21247 RVA: 0x001AC1AD File Offset: 0x001AA3AD
		public DeepProfilerScope(string label = null)
		{
			DeepProfiler.Start(label);
		}

		// Token: 0x06005300 RID: 21248 RVA: 0x001AC1BB File Offset: 0x001AA3BB
		void IDisposable.Dispose()
		{
			DeepProfiler.End();
		}
	}
}
