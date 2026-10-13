using System;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x0200014C RID: 332
	public class MathEvaluatorCustomVariable : IXsltContextVariable
	{
		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x00002501 File Offset: 0x00000701
		public bool IsLocal
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x00002501 File Offset: 0x00000701
		public bool IsParam
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0002F443 File Offset: 0x0002D643
		public XPathResultType VariableType
		{
			get
			{
				return XPathResultType.Any;
			}
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0002F446 File Offset: 0x0002D646
		public MathEvaluatorCustomVariable(string prefix, string name)
		{
			this.prefix = prefix;
			this.name = name;
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0002F45C File Offset: 0x0002D65C
		public object Evaluate(XsltContext xsltContext)
		{
			return ((MathEvaluatorCustomContext)xsltContext).ArgList.GetParam(this.name, this.prefix);
		}

		// Token: 0x04000516 RID: 1302
		private string prefix;

		// Token: 0x04000517 RID: 1303
		private string name;
	}
}
