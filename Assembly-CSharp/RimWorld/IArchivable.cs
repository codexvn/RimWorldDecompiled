using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B55 RID: 6997
	public interface IArchivable : IExposable, ILoadReferenceable
	{
		// Token: 0x17001809 RID: 6153
		// (get) Token: 0x06009C4B RID: 40011
		Texture ArchivedIcon { get; }

		// Token: 0x1700180A RID: 6154
		// (get) Token: 0x06009C4C RID: 40012
		Color ArchivedIconColor { get; }

		// Token: 0x1700180B RID: 6155
		// (get) Token: 0x06009C4D RID: 40013
		string ArchivedLabel { get; }

		// Token: 0x1700180C RID: 6156
		// (get) Token: 0x06009C4E RID: 40014
		string ArchivedTooltip { get; }

		// Token: 0x1700180D RID: 6157
		// (get) Token: 0x06009C4F RID: 40015
		int CreatedTicksGame { get; }

		// Token: 0x1700180E RID: 6158
		// (get) Token: 0x06009C50 RID: 40016
		bool CanCullArchivedNow { get; }

		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x06009C51 RID: 40017
		LookTargets LookTargets { get; }

		// Token: 0x06009C52 RID: 40018
		void OpenArchived();
	}
}
