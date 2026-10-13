using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C5A RID: 3162
	public class BackCompatibilityConverter_1_3 : BackCompatibilityConverter
	{
		// Token: 0x060050F2 RID: 20722 RVA: 0x001A3F98 File Offset: 0x001A2198
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 || (majorVer == 1 && minorVer <= 3);
		}

		// Token: 0x060050F3 RID: 20723 RVA: 0x001A3FAC File Offset: 0x001A21AC
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(JobDef) && defName == "TriggerFirefoamPopper")
			{
				return "TriggerObject";
			}
			if (defType == typeof(TerrainDef) && defName == "CarpetDark")
			{
				return "CarpetGreyDark";
			}
			return null;
		}

		// Token: 0x060050F4 RID: 20724 RVA: 0x00002C42 File Offset: 0x00000E42
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			return null;
		}

		// Token: 0x060050F5 RID: 20725 RVA: 0x001A4004 File Offset: 0x001A2204
		public override void PostExposeData(object obj)
		{
			Pawn pawn = obj as Pawn;
			if (pawn != null && pawn.RaceProps.Humanlike && pawn.genes == null)
			{
				pawn.genes = new Pawn_GeneTracker(pawn);
			}
		}
	}
}
