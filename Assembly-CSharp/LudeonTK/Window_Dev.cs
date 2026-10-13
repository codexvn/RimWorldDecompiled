using System;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200009E RID: 158
	public abstract class Window_Dev : Window
	{
		// Token: 0x06000353 RID: 851 RVA: 0x00012CA4 File Offset: 0x00010EA4
		public Window_Dev() : base(new DevWindowDrawing())
		{
		}
	}
}
