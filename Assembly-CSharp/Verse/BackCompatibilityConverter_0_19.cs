using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C4F RID: 3151
	public class BackCompatibilityConverter_0_19 : BackCompatibilityConverter
	{
		// Token: 0x060050C9 RID: 20681 RVA: 0x001A2CBC File Offset: 0x001A0EBC
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 && minorVer <= 19;
		}

		// Token: 0x060050CA RID: 20682 RVA: 0x00002C42 File Offset: 0x00000E42
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			return null;
		}

		// Token: 0x060050CB RID: 20683 RVA: 0x00002C42 File Offset: 0x00000E42
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			return null;
		}

		// Token: 0x060050CC RID: 20684 RVA: 0x001A2CCC File Offset: 0x001A0ECC
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Game game = obj as Game;
				if (game != null && game.foodRestrictionDatabase == null)
				{
					game.foodRestrictionDatabase = new FoodRestrictionDatabase();
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Pawn pawn = obj as Pawn;
				if (pawn != null && pawn.foodRestriction == null && pawn.RaceProps.Humanlike && ((pawn.Faction != null && pawn.Faction.IsPlayer) || (pawn.HostFaction != null && pawn.HostFaction.IsPlayer)))
				{
					pawn.foodRestriction = new Pawn_FoodRestrictionTracker(pawn);
				}
			}
		}
	}
}
