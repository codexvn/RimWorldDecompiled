using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x02000148 RID: 328
	public class MathEvaluatorCustomContext : XsltContext
	{
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060009E9 RID: 2537 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool Whitespace
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060009EA RID: 2538 RVA: 0x0002EED5 File Offset: 0x0002D0D5
		public XsltArgumentList ArgList
		{
			get
			{
				return this.argList;
			}
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0002EEDD File Offset: 0x0002D0DD
		public MathEvaluatorCustomContext()
		{
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0002EEE5 File Offset: 0x0002D0E5
		public MathEvaluatorCustomContext(NameTable nt, XsltArgumentList args) : base(nt)
		{
			this.argList = args;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0002EEF8 File Offset: 0x0002D0F8
		public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] argTypes)
		{
			MathEvaluatorCustomFunctions.FunctionType[] functionTypes = MathEvaluatorCustomFunctions.FunctionTypes;
			for (int i = 0; i < functionTypes.Length; i++)
			{
				if (functionTypes[i].name == name)
				{
					return new MathEvaluatorCustomFunction(functionTypes[i], argTypes);
				}
			}
			return null;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0002EF34 File Offset: 0x0002D134
		public override IXsltContextVariable ResolveVariable(string prefix, string name)
		{
			if (this.ArgList.GetParam(name, prefix) != null)
			{
				return new MathEvaluatorCustomVariable(prefix, name);
			}
			return null;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00002501 File Offset: 0x00000701
		public override bool PreserveWhitespace(XPathNavigator node)
		{
			return false;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00002501 File Offset: 0x00000701
		public override int CompareDocument(string baseUri, string nextbaseUri)
		{
			return 0;
		}

		// Token: 0x0400050E RID: 1294
		private XsltArgumentList argList;
	}
}
