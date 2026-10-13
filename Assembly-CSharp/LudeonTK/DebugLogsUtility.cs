using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000076 RID: 118
	public class DebugLogsUtility
	{
		// Token: 0x06000295 RID: 661 RVA: 0x0000E10C File Offset: 0x0000C30C
		public static string ThingListToUniqueCountString(IEnumerable<Thing> things)
		{
			if (things == null)
			{
				return "null";
			}
			Dictionary<ThingDef, int> dictionary = new Dictionary<ThingDef, int>();
			foreach (Thing thing in things)
			{
				if (!dictionary.ContainsKey(thing.def))
				{
					dictionary.Add(thing.def, 0);
				}
				Dictionary<ThingDef, int> dictionary2 = dictionary;
				ThingDef def = thing.def;
				int num = dictionary2[def];
				dictionary2[def] = num + 1;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Registered things in dynamic draw list:");
			foreach (KeyValuePair<ThingDef, int> keyValuePair in from k in dictionary
			orderby k.Value descending
			select k)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				ThingDef key = keyValuePair.Key;
				stringBuilder2.AppendLine(((key != null) ? key.ToString() : null) + " - " + keyValuePair.Value.ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
