using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018B8 RID: 6328
	public class ThoughtWorker_WearingColor_Favorite : ThoughtWorker_WearingColor
	{
		// Token: 0x06009087 RID: 36999 RVA: 0x002A982C File Offset: 0x002A7A2C
		protected override Color? Color(Pawn p)
		{
			if (p.DevelopmentalStage.Baby())
			{
				return null;
			}
			ColorDef favoriteColor = p.story.favoriteColor;
			if (favoriteColor == null)
			{
				return null;
			}
			return new Color?(favoriteColor.color);
		}
	}
}
