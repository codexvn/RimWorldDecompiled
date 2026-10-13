using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003567 RID: 13671
	public class Command_SelectStorage : Command_Action
	{
		// Token: 0x170031B3 RID: 12723
		// (get) Token: 0x060132BA RID: 78522 RVA: 0x005B2E6F File Offset: 0x005B106F
		public override Texture2D BGTexture
		{
			get
			{
				return TexCommand.SelectShelf;
			}
		}

		// Token: 0x170031B4 RID: 12724
		// (get) Token: 0x060132BB RID: 78523 RVA: 0x005B2E6F File Offset: 0x005B106F
		public override Texture2D BGTextureShrunk
		{
			get
			{
				return TexCommand.SelectShelf;
			}
		}
	}
}
