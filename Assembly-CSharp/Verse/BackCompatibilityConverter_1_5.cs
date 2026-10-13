using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000C5C RID: 3164
	public class BackCompatibilityConverter_1_5 : BackCompatibilityConverter
	{
		// Token: 0x060050FC RID: 20732 RVA: 0x001A427B File Offset: 0x001A247B
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 || (majorVer == 1 && minorVer <= 5);
		}

		// Token: 0x060050FD RID: 20733 RVA: 0x001A4290 File Offset: 0x001A2490
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(ThingDef) && defName == "DropPodIncomingMechanoid")
			{
				return "DropPodIncomingMechanoidRapid";
			}
			if (defType == typeof(ThingDef) && defName == "AncientContainer")
			{
				return "AncientLargeContainer";
			}
			if (defType == typeof(IssueDef) && defName == "SmallSpaces")
			{
				return "Indoors";
			}
			if (defType == typeof(PreceptDef) && defName == "SmallSpaces_Acceptable")
			{
				return "Indoors_Acceptable";
			}
			return null;
		}

		// Token: 0x060050FE RID: 20734 RVA: 0x001A4332 File Offset: 0x001A2532
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (baseType == typeof(Area) && providedClassName == "Area_SnowClear")
			{
				return typeof(Area_SnowOrSandClear);
			}
			return null;
		}

		// Token: 0x060050FF RID: 20735 RVA: 0x001A4360 File Offset: 0x001A2560
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				WorldGenData worldGenData = obj as WorldGenData;
				if (worldGenData != null)
				{
					List<int> list = new List<int>();
					Scribe_Collections.Look<int>(ref list, "roadNodes", LookMode.Undefined, Array.Empty<object>());
					List<PlanetTile> list2 = worldGenData.roadNodes[Find.WorldGrid.Surface] = new List<PlanetTile>();
					foreach (int tileId in list)
					{
						list2.Add(new PlanetTile(tileId, Find.WorldGrid.Surface));
					}
				}
				Quest quest = obj as Quest;
				if (quest != null)
				{
					int num = -1;
					Scribe_Values.Look<int>(ref num, "ticksUntilAcceptanceExpiry", -1, false);
					if (num != -1)
					{
						quest.acceptanceExpireTick = GenTicks.TicksGame + num;
					}
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && obj is WorldInfo)
			{
				WorldGenStep_Mutators.AddMutatorsFromTile(Find.WorldGrid.Surface);
			}
		}
	}
}
