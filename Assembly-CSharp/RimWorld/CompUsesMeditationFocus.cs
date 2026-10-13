using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EC7 RID: 11975
	public class CompUsesMeditationFocus : ThingComp
	{
		// Token: 0x06010E6B RID: 69227 RVA: 0x004DDB7E File Offset: 0x004DBD7E
		public override void PostDrawExtraSelectionOverlays()
		{
			MeditationUtility.DrawMeditationSpotOverlay(this.parent.Position, this.parent.Map);
		}
	}
}
