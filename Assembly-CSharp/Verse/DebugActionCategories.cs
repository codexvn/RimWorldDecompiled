using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200091E RID: 2334
	public static class DebugActionCategories
	{
		// Token: 0x06003C8B RID: 15499 RVA: 0x0013D5E0 File Offset: 0x0013B7E0
		static DebugActionCategories()
		{
			DebugActionCategories.categoryOrders.Add("Incidents", 100);
			DebugActionCategories.categoryOrders.Add("Quests", 200);
			DebugActionCategories.categoryOrders.Add("Quests (old)", 250);
			DebugActionCategories.categoryOrders.Add("Translation", 300);
			DebugActionCategories.categoryOrders.Add("General", 400);
			DebugActionCategories.categoryOrders.Add("Sound", 450);
			DebugActionCategories.categoryOrders.Add("Pawns", 500);
			DebugActionCategories.categoryOrders.Add("Spawning", 600);
			DebugActionCategories.categoryOrders.Add("Ideoligion", 700);
			DebugActionCategories.categoryOrders.Add("Map", 800);
			DebugActionCategories.categoryOrders.Add("Lighting", 850);
			DebugActionCategories.categoryOrders.Add("Autotests", 900);
			DebugActionCategories.categoryOrders.Add("Mods", 100);
			DebugActionCategories.categoryOrders.Add("More debug actions", 1000);
			DebugActionCategories.categoryOrders.Add("Humanlike", 1100);
			DebugActionCategories.categoryOrders.Add("Animal", 1200);
			DebugActionCategories.categoryOrders.Add("Insect", 1300);
			DebugActionCategories.categoryOrders.Add("Mechanoid", 1400);
			DebugActionCategories.categoryOrders.Add("Anomaly", 1500);
			DebugActionCategories.categoryOrders.Add("Pathing", 1625);
			DebugActionCategories.categoryOrders.Add("Generation", 1650);
			DebugActionCategories.categoryOrders.Add("Other", 1700);
		}

		// Token: 0x06003C8C RID: 15500 RVA: 0x0013D7AC File Offset: 0x0013B9AC
		public static int GetOrderFor(string category)
		{
			int result;
			if (!category.NullOrEmpty() && DebugActionCategories.categoryOrders.TryGetValue(category, out result))
			{
				return result;
			}
			return int.MaxValue;
		}

		// Token: 0x04002B88 RID: 11144
		public const string Incidents = "Incidents";

		// Token: 0x04002B89 RID: 11145
		public const string Quests = "Quests";

		// Token: 0x04002B8A RID: 11146
		public const string QuestsOld = "Quests (old)";

		// Token: 0x04002B8B RID: 11147
		public const string Translation = "Translation";

		// Token: 0x04002B8C RID: 11148
		public const string General = "General";

		// Token: 0x04002B8D RID: 11149
		public const string Sound = "Sound";

		// Token: 0x04002B8E RID: 11150
		public const string Pawns = "Pawns";

		// Token: 0x04002B8F RID: 11151
		public const string Spawning = "Spawning";

		// Token: 0x04002B90 RID: 11152
		public const string Ideoligion = "Ideoligion";

		// Token: 0x04002B91 RID: 11153
		public const string MapManagement = "Map";

		// Token: 0x04002B92 RID: 11154
		public const string Autotests = "Autotests";

		// Token: 0x04002B93 RID: 11155
		public const string Lighting = "Lighting";

		// Token: 0x04002B94 RID: 11156
		public const string Mods = "Mods";

		// Token: 0x04002B95 RID: 11157
		public const string More = "More debug actions";

		// Token: 0x04002B96 RID: 11158
		public const string Anomaly = "Anomaly";

		// Token: 0x04002B97 RID: 11159
		public const string Generation = "Generation";

		// Token: 0x04002B98 RID: 11160
		public const string Pathing = "Pathing";

		// Token: 0x04002B99 RID: 11161
		public const string Humanlike = "Humanlike";

		// Token: 0x04002B9A RID: 11162
		public const string Animal = "Animal";

		// Token: 0x04002B9B RID: 11163
		public const string Insect = "Insect";

		// Token: 0x04002B9C RID: 11164
		public const string Mechanoid = "Mechanoid";

		// Token: 0x04002B9D RID: 11165
		public const string Other = "Other";

		// Token: 0x04002B9E RID: 11166
		public static readonly Dictionary<string, int> categoryOrders = new Dictionary<string, int>();
	}
}
