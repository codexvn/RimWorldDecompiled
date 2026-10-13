using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020039B1 RID: 14769
	public class SymbolStack
	{
		// Token: 0x17003338 RID: 13112
		// (get) Token: 0x06014425 RID: 82981 RVA: 0x0060C831 File Offset: 0x0060AA31
		public bool Empty
		{
			get
			{
				return this.stack.Count == 0;
			}
		}

		// Token: 0x17003339 RID: 13113
		// (get) Token: 0x06014426 RID: 82982 RVA: 0x0060C841 File Offset: 0x0060AA41
		public int Count
		{
			get
			{
				return this.stack.Count;
			}
		}

		// Token: 0x06014427 RID: 82983 RVA: 0x0060C850 File Offset: 0x0060AA50
		public void Push(string symbol, ResolveParams resolveParams, string customNameForPath = null)
		{
			string text = BaseGen.CurrentSymbolPath;
			if (!text.NullOrEmpty())
			{
				text += "_";
			}
			text += (customNameForPath ?? symbol);
			SymbolStack.Element item = default(SymbolStack.Element);
			item.symbol = symbol;
			item.resolveParams = resolveParams;
			item.symbolPath = text;
			this.stack.Push(item);
		}

		// Token: 0x06014428 RID: 82984 RVA: 0x0060C8B0 File Offset: 0x0060AAB0
		public void Push(string symbol, CellRect rect, string customNameForPath = null)
		{
			this.Push(symbol, new ResolveParams
			{
				rect = rect
			}, customNameForPath);
		}

		// Token: 0x06014429 RID: 82985 RVA: 0x0060C8D8 File Offset: 0x0060AAD8
		public void PushMany(ResolveParams resolveParams, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], resolveParams, null);
			}
		}

		// Token: 0x0601442A RID: 82986 RVA: 0x0060C900 File Offset: 0x0060AB00
		public void PushMany(CellRect rect, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], rect, null);
			}
		}

		// Token: 0x0601442B RID: 82987 RVA: 0x0060C926 File Offset: 0x0060AB26
		public SymbolStack.Element Pop()
		{
			return this.stack.Pop();
		}

		// Token: 0x0601442C RID: 82988 RVA: 0x0060C933 File Offset: 0x0060AB33
		public void Clear()
		{
			this.stack.Clear();
		}

		// Token: 0x0400DFD5 RID: 57301
		private Stack<SymbolStack.Element> stack = new Stack<SymbolStack.Element>();

		// Token: 0x020039B2 RID: 14770
		public struct Element
		{
			// Token: 0x0400DFD6 RID: 57302
			public string symbol;

			// Token: 0x0400DFD7 RID: 57303
			public ResolveParams resolveParams;

			// Token: 0x0400DFD8 RID: 57304
			public string symbolPath;
		}
	}
}
