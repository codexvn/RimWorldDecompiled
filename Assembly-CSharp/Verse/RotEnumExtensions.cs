using System;
using System.Collections.Generic;
using Verse.Utility;

namespace Verse
{
	// Token: 0x020000D9 RID: 217
	public static class RotEnumExtensions
	{
		// Token: 0x060005CD RID: 1485 RVA: 0x0001D444 File Offset: 0x0001B644
		public static Rot4 Random(this RotEnum r)
		{
			RotEnumExtensions.options.Clear();
			foreach (Enum @enum in r.GetBitFlags())
			{
				RotEnum rot = (RotEnum)@enum;
				RotEnumExtensions.options.Add(rot);
			}
			if (RotEnumExtensions.options.Count == 0)
			{
				return Rot4.North;
			}
			return RotEnumExtensions.options[Rand.Range(0, RotEnumExtensions.options.Count)];
		}

		// Token: 0x04000306 RID: 774
		private static readonly List<Rot4> options = new List<Rot4>();
	}
}
