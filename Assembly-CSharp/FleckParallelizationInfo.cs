using System;
using System.Threading;
using Verse;

// Token: 0x02000043 RID: 67
public class FleckParallelizationInfo
{
	// Token: 0x040000AA RID: 170
	public int startIndex;

	// Token: 0x040000AB RID: 171
	public int endIndex;

	// Token: 0x040000AC RID: 172
	public object data;

	// Token: 0x040000AD RID: 173
	public DrawBatch drawBatch = new DrawBatch();

	// Token: 0x040000AE RID: 174
	public ManualResetEvent doneEvent = new ManualResetEvent(false);
}
