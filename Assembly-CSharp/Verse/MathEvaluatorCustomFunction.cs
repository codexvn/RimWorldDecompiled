using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x0200014B RID: 331
	public class MathEvaluatorCustomFunction : IXsltContextFunction
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x0002F3F8 File Offset: 0x0002D5F8
		public XPathResultType[] ArgTypes
		{
			get
			{
				return this.argTypes;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x0002F400 File Offset: 0x0002D600
		public int Maxargs
		{
			get
			{
				return this.functionType.maxArgs;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0002F40D File Offset: 0x0002D60D
		public int Minargs
		{
			get
			{
				return this.functionType.minArgs;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x00002501 File Offset: 0x00000701
		public XPathResultType ReturnType
		{
			get
			{
				return XPathResultType.Number;
			}
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0002F41A File Offset: 0x0002D61A
		public MathEvaluatorCustomFunction(MathEvaluatorCustomFunctions.FunctionType functionType, XPathResultType[] argTypes)
		{
			this.functionType = functionType;
			this.argTypes = argTypes;
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0002F430 File Offset: 0x0002D630
		public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator docContext)
		{
			return this.functionType.func(args);
		}

		// Token: 0x04000514 RID: 1300
		private XPathResultType[] argTypes;

		// Token: 0x04000515 RID: 1301
		private MathEvaluatorCustomFunctions.FunctionType functionType;
	}
}
