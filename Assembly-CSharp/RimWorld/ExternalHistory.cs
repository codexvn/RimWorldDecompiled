using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B56 RID: 6998
	public class ExternalHistory : IExposable
	{
		// Token: 0x17001810 RID: 6160
		// (get) Token: 0x06009C53 RID: 40019 RVA: 0x002D5A1C File Offset: 0x002D3C1C
		public string AllInformation
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("storyteller: ");
				stringBuilder.Append(this.storytellerName);
				stringBuilder.Append("   userName: ");
				stringBuilder.Append(this.userName);
				stringBuilder.Append("   realWorldDate(UTC): ");
				stringBuilder.Append(this.realWorldDate);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06009C54 RID: 40020 RVA: 0x002D5A80 File Offset: 0x002D3C80
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.gameVersion, "gameVersion", null, false);
			Scribe_Values.Look<string>(ref this.gameplayID, "gameplayID", null, false);
			Scribe_Values.Look<string>(ref this.userName, "userName", null, false);
			Scribe_Values.Look<string>(ref this.storytellerName, "storytellerName", null, false);
			Scribe_Values.Look<string>(ref this.realWorldDate, "realWorldDate", null, false);
			Scribe_Values.Look<string>(ref this.firstUploadDate, "firstUploadDate", null, false);
			Scribe_Values.Look<int>(ref this.firstUploadTime, "firstUploadTime", 0, false);
			Scribe_Values.Look<bool>(ref this.devMode, "devMode", false, false);
			Scribe_Deep.Look<History>(ref this.history, "history", Array.Empty<object>());
		}

		// Token: 0x04006C62 RID: 27746
		public string gameVersion = "?";

		// Token: 0x04006C63 RID: 27747
		public string gameplayID = "?";

		// Token: 0x04006C64 RID: 27748
		public string userName = "?";

		// Token: 0x04006C65 RID: 27749
		public string storytellerName = "?";

		// Token: 0x04006C66 RID: 27750
		public string realWorldDate = "?";

		// Token: 0x04006C67 RID: 27751
		public string firstUploadDate = "?";

		// Token: 0x04006C68 RID: 27752
		public int firstUploadTime;

		// Token: 0x04006C69 RID: 27753
		public bool devMode;

		// Token: 0x04006C6A RID: 27754
		public History history = new History();

		// Token: 0x04006C6B RID: 27755
		public static string defaultUserName = "Anonymous";
	}
}
