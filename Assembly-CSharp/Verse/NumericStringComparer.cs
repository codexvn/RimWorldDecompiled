using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D40 RID: 3392
	public class NumericStringComparer : IComparer<string>
	{
		// Token: 0x06005657 RID: 22103 RVA: 0x001BDE1C File Offset: 0x001BC01C
		public int Compare(string x, string y)
		{
			x = (x ?? "");
			y = (y ?? "");
			if (x.Contains("~"))
			{
				string[] array = x.Split('~', StringSplitOptions.None);
				if (array.Length == 2)
				{
					x = array[0];
				}
			}
			if (y.Contains("~"))
			{
				string[] array2 = y.Split('~', StringSplitOptions.None);
				if (array2.Length == 2)
				{
					y = array2[0];
				}
			}
			if ((x.EndsWith("%") && y.EndsWith("%")) || (x.EndsWith("C") && y.EndsWith("C")))
			{
				x = x.Substring(0, x.Length - 1);
				y = y.Substring(0, y.Length - 1);
			}
			if (x.Length >= 2 && x[0] == '$')
			{
				x = x.Substring(1, x.Length - 1);
			}
			if (y.Length >= 2 && y[0] == '$')
			{
				y = y.Substring(1, y.Length - 1);
			}
			float num;
			float value;
			if (float.TryParse(x, out num) && float.TryParse(y, out value))
			{
				return num.CompareTo(value);
			}
			return x.CompareTo(y);
		}
	}
}
