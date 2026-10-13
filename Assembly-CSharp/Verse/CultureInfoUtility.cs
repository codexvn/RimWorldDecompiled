using System;
using System.Globalization;
using System.Threading;

namespace Verse
{
	// Token: 0x02000136 RID: 310
	public static class CultureInfoUtility
	{
		// Token: 0x0600098F RID: 2447 RVA: 0x0002D6DD File Offset: 0x0002B8DD
		public static void EnsureEnglish()
		{
			if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			}
		}

		// Token: 0x040004CF RID: 1231
		private const string EnglishCulture = "en-US";
	}
}
