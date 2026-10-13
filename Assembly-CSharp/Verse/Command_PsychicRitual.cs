using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B84 RID: 2948
	public class Command_PsychicRitual : Command_Action
	{
		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x06004B82 RID: 19330 RVA: 0x00180951 File Offset: 0x0017EB51
		public override Texture2D BGTexture
		{
			get
			{
				return ContentFinder<Texture2D>.Get("UI/PsychicRituals/PsychicRitualButtonBackground", true);
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x06004B83 RID: 19331 RVA: 0x00180951 File Offset: 0x0017EB51
		public override Texture2D BGTextureShrunk
		{
			get
			{
				return ContentFinder<Texture2D>.Get("UI/PsychicRituals/PsychicRitualButtonBackground", true);
			}
		}
	}
}
