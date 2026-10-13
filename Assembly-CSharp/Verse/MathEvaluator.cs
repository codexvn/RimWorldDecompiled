using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Verse
{
	// Token: 0x02000147 RID: 327
	public static class MathEvaluator
	{
		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0002ED69 File Offset: 0x0002CF69
		private static XPathNavigator Navigator
		{
			get
			{
				if (MathEvaluator.doc == null)
				{
					MathEvaluator.doc = new XPathDocument(new StringReader("<root />"));
				}
				if (MathEvaluator.navigator == null)
				{
					MathEvaluator.navigator = MathEvaluator.doc.CreateNavigator();
				}
				return MathEvaluator.navigator;
			}
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x0002EDA4 File Offset: 0x0002CFA4
		public static double Evaluate(string expr)
		{
			if (expr.NullOrEmpty())
			{
				return 0.0;
			}
			expr = MathEvaluator.AddSpacesRegex.Replace(expr, " ${1} ");
			expr = expr.Replace("/", " div ");
			expr = expr.Replace("%", " mod ");
			double result;
			try
			{
				XPathExpression xpathExpression = XPathExpression.Compile("number(" + expr + ")");
				xpathExpression.SetContext(MathEvaluator.Context);
				double num = (double)MathEvaluator.Navigator.Evaluate(xpathExpression);
				if (double.IsNaN(num))
				{
					Log.ErrorOnce("Expression \"" + expr + "\" evaluated to NaN.", expr.GetHashCode() ^ 48337162);
					num = 0.0;
				}
				result = num;
			}
			catch (XPathException ex)
			{
				string str = "Could not evaluate expression \"";
				string str2 = expr;
				string str3 = "\". Error: ";
				XPathException ex2 = ex;
				Log.ErrorOnce(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null), expr.GetHashCode() ^ 980986121);
				result = 0.0;
			}
			return result;
		}

		// Token: 0x0400050A RID: 1290
		private static XPathDocument doc;

		// Token: 0x0400050B RID: 1291
		private static XPathNavigator navigator;

		// Token: 0x0400050C RID: 1292
		private static readonly Regex AddSpacesRegex = new Regex("([\\+\\-\\*])");

		// Token: 0x0400050D RID: 1293
		private static readonly MathEvaluatorCustomContext Context = new MathEvaluatorCustomContext(new NameTable(), new XsltArgumentList());
	}
}
