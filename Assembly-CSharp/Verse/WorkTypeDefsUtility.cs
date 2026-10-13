using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x0200031B RID: 795
	public static class WorkTypeDefsUtility
	{
		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060013D1 RID: 5073 RVA: 0x0005DAC3 File Offset: 0x0005BCC3
		public static IEnumerable<WorkTypeDef> WorkTypeDefsInPriorityOrder
		{
			get
			{
				return from wt in DefDatabase<WorkTypeDef>.AllDefs
				orderby wt.naturalPriority descending
				select wt;
			}
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0005DAF0 File Offset: 0x0005BCF0
		public static string LabelTranslated(this WorkTags tags)
		{
			if (tags <= WorkTags.Crafting)
			{
				if (tags <= WorkTags.Social)
				{
					if (tags <= WorkTags.Violent)
					{
						switch (tags)
						{
						case WorkTags.None:
							return "WorkTagNone".Translate();
						case (WorkTags)1:
						case (WorkTags)3:
							break;
						case WorkTags.ManualDumb:
							return "WorkTagManualDumb".Translate();
						case WorkTags.ManualSkilled:
							return "WorkTagManualSkilled".Translate();
						default:
							if (tags == WorkTags.Violent)
							{
								return "WorkTagViolent".Translate();
							}
							break;
						}
					}
					else
					{
						if (tags == WorkTags.Caring)
						{
							return "WorkTagCaring".Translate();
						}
						if (tags == WorkTags.Social)
						{
							return "WorkTagSocial".Translate();
						}
					}
				}
				else if (tags <= WorkTags.Intellectual)
				{
					if (tags == WorkTags.Commoner)
					{
						return "WorkTagCommoner".Translate();
					}
					if (tags == WorkTags.Intellectual)
					{
						return "WorkTagIntellectual".Translate();
					}
				}
				else
				{
					if (tags == WorkTags.Animals)
					{
						return "WorkTagAnimals".Translate();
					}
					if (tags == WorkTags.Artistic)
					{
						return "WorkTagArtistic".Translate();
					}
					if (tags == WorkTags.Crafting)
					{
						return "WorkTagCrafting".Translate();
					}
				}
			}
			else if (tags <= WorkTags.PlantWork)
			{
				if (tags <= WorkTags.Firefighting)
				{
					if (tags == WorkTags.Cooking)
					{
						return "WorkTagCooking".Translate();
					}
					if (tags == WorkTags.Firefighting)
					{
						return "WorkTagFirefighting".Translate();
					}
				}
				else
				{
					if (tags == WorkTags.Cleaning)
					{
						return "WorkTagCleaning".Translate();
					}
					if (tags == WorkTags.Hauling)
					{
						return "WorkTagHauling".Translate();
					}
					if (tags == WorkTags.PlantWork)
					{
						return "WorkTagPlantWork".Translate();
					}
				}
			}
			else if (tags <= WorkTags.Hunting)
			{
				if (tags == WorkTags.Mining)
				{
					return "WorkTagMining".Translate();
				}
				if (tags == WorkTags.Hunting)
				{
					return "WorkTagHunting".Translate();
				}
			}
			else
			{
				if (tags == WorkTags.Constructing)
				{
					return "WorkTagConstructing".Translate();
				}
				if (tags == WorkTags.Shooting)
				{
					return "WorkTagShooting".Translate();
				}
				if (tags == WorkTags.AllWork)
				{
					return "WorkTagAllWork".Translate();
				}
			}
			string str = "Unknown or mixed worktags for naming: ";
			int num = (int)tags;
			Log.Error(str + num.ToString());
			return "Worktag";
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0005DD9C File Offset: 0x0005BF9C
		public static bool ExactlyOneWorkTagSet(this WorkTags workTags)
		{
			return workTags != WorkTags.None && (workTags & workTags - 1) == WorkTags.None;
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0005DDB8 File Offset: 0x0005BFB8
		public static bool OverlapsWithOnAnyWorkType(this WorkTags a, WorkTags b)
		{
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if ((workTypeDef.workTags & a) != WorkTags.None && (workTypeDef.workTags & b) != WorkTags.None)
				{
					return true;
				}
			}
			return false;
		}
	}
}
