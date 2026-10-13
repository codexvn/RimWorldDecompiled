using System;
using System.Xml;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000C58 RID: 3160
	public class BackCompatibilityConverter_1_2 : BackCompatibilityConverter
	{
		// Token: 0x060050EB RID: 20715 RVA: 0x001A3BAD File Offset: 0x001A1DAD
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 || (majorVer == 1 && minorVer <= 2);
		}

		// Token: 0x060050EC RID: 20716 RVA: 0x001A3BC4 File Offset: 0x001A1DC4
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			if (defType == typeof(RoyalTitleDef) && defName == "Esquire")
			{
				return "Acolyte";
			}
			if (defType == typeof(PawnKindDef) && defName == "Empire_Royal_Esquire")
			{
				return "Empire_Royal_Acolyte";
			}
			if (defType == typeof(HairDef) && defName == "ShavedFemale")
			{
				return "Shaved";
			}
			return null;
		}

		// Token: 0x060050ED RID: 20717 RVA: 0x001A3C41 File Offset: 0x001A1E41
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			if (providedClassName == "Hediff_ImplantWithLevel")
			{
				return typeof(Hediff_Level);
			}
			return null;
		}

		// Token: 0x060050EE RID: 20718 RVA: 0x001A3C5C File Offset: 0x001A1E5C
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Building_Bed building_Bed = obj as Building_Bed;
				if (building_Bed != null && building_Bed.CompAssignableToPawn != null)
				{
					bool flag = false;
					Scribe_Values.Look<bool>(ref flag, "forPrisoners", false, false);
					if (flag)
					{
						building_Bed.ForOwnerType = BedOwnerType.Prisoner;
						return;
					}
				}
				else
				{
					Pawn_GuestTracker pawn_GuestTracker = obj as Pawn_GuestTracker;
					if (pawn_GuestTracker != null)
					{
						bool flag2 = false;
						Scribe_Values.Look<bool>(ref flag2, "prisoner", false, false);
						if (flag2)
						{
							pawn_GuestTracker.guestStatusInt = GuestStatus.Prisoner;
							return;
						}
					}
					else
					{
						QuestPart_RequirementsToAcceptNoDanger questPart_RequirementsToAcceptNoDanger = obj as QuestPart_RequirementsToAcceptNoDanger;
						if (questPart_RequirementsToAcceptNoDanger != null)
						{
							Map map = null;
							Scribe_References.Look<Map>(ref map, "map", false);
							questPart_RequirementsToAcceptNoDanger.mapParent = map.Parent;
							return;
						}
					}
				}
			}
			else if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				World world = obj as World;
				if (world != null)
				{
					if (world.ideoManager == null)
					{
						world.ideoManager = new IdeoManager();
						return;
					}
				}
				else
				{
					Pawn pawn = obj as Pawn;
					if (pawn != null)
					{
						if (Find.World.ideoManager == null)
						{
							Find.World.ideoManager = new IdeoManager();
						}
						if (pawn.ownership == null)
						{
							pawn.ownership = new Pawn_Ownership(pawn);
						}
						if (pawn.RaceProps.Humanlike)
						{
							if (pawn.ideo == null)
							{
								pawn.ideo = new Pawn_IdeoTracker(pawn);
								if (pawn.Faction != null && pawn.Faction.def.humanlikeFaction && pawn.Faction.ideos == null)
								{
									pawn.Faction.ideos = new FactionIdeosTracker(pawn.Faction);
									pawn.Faction.ideos.ChooseOrGenerateIdeo(FactionIdeosTracker.IdeoGenerationParmsForFaction_BackCompatibility(pawn.Faction.def, !ModsConfig.IdeologyActive));
								}
								Faction faction = pawn.Faction;
								bool flag3;
								if (faction == null)
								{
									flag3 = (null != null);
								}
								else
								{
									FactionIdeosTracker ideos = faction.ideos;
									flag3 = (((ideos != null) ? ideos.PrimaryIdeo : null) != null);
								}
								Ideo ideo;
								if (flag3)
								{
									pawn.ideo.SetIdeo(pawn.Faction.ideos.PrimaryIdeo);
								}
								else if (Find.IdeoManager.IdeosListForReading.TryRandomElementByWeight((Ideo x) => IdeoUtility.IdeoChangeToWeight(pawn, x), out ideo))
								{
									pawn.ideo.SetIdeo(ideo);
								}
							}
							if (pawn.style == null)
							{
								pawn.style = new Pawn_StyleTracker(pawn);
								pawn.style.beardDef = BeardDefOf.NoBeard;
								return;
							}
						}
					}
					else
					{
						Faction faction2 = obj as Faction;
						if (faction2 != null)
						{
							if (Find.World.ideoManager == null)
							{
								Find.World.ideoManager = new IdeoManager();
							}
							if (faction2.def.humanlikeFaction && faction2.ideos == null)
							{
								faction2.ideos = new FactionIdeosTracker(faction2);
								faction2.ideos.ChooseOrGenerateIdeo(FactionIdeosTracker.IdeoGenerationParmsForFaction_BackCompatibility(faction2.def, !ModsConfig.IdeologyActive));
							}
						}
					}
				}
			}
		}
	}
}
