using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020024EF RID: 9455
	[StaticConstructorOnStartup]
	public static class HostilityResponseModeUtility
	{
		// Token: 0x0600CE97 RID: 52887 RVA: 0x003CAB6F File Offset: 0x003C8D6F
		public static Texture2D GetIcon(this HostilityResponseMode response)
		{
			switch (response)
			{
			case HostilityResponseMode.Ignore:
				return HostilityResponseModeUtility.IgnoreIcon;
			case HostilityResponseMode.Attack:
				return HostilityResponseModeUtility.AttackIcon;
			case HostilityResponseMode.Flee:
				return HostilityResponseModeUtility.FleeIcon;
			default:
				return BaseContent.BadTex;
			}
		}

		// Token: 0x0600CE98 RID: 52888 RVA: 0x003CAB9C File Offset: 0x003C8D9C
		public static HostilityResponseMode GetNextResponse(Pawn pawn)
		{
			switch (pawn.playerSettings.hostilityResponse)
			{
			case HostilityResponseMode.Ignore:
				if (pawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					return HostilityResponseMode.Flee;
				}
				return HostilityResponseMode.Attack;
			case HostilityResponseMode.Attack:
				return HostilityResponseMode.Flee;
			case HostilityResponseMode.Flee:
				return HostilityResponseMode.Ignore;
			default:
				return HostilityResponseMode.Ignore;
			}
		}

		// Token: 0x0600CE99 RID: 52889 RVA: 0x003CABDB File Offset: 0x003C8DDB
		public static string GetLabel(this HostilityResponseMode response)
		{
			return ("HostilityResponseMode_" + response.ToString()).Translate();
		}

		// Token: 0x0600CE9A RID: 52890 RVA: 0x003CAC00 File Offset: 0x003C8E00
		public static void DrawResponseButton(Rect rect, Pawn pawn, bool paintable)
		{
			Widgets.Dropdown<Pawn, HostilityResponseMode>(rect, pawn, HostilityResponseModeUtility.IconColor, new Func<Pawn, HostilityResponseMode>(HostilityResponseModeUtility.DrawResponseButton_GetResponse), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>>(HostilityResponseModeUtility.DrawResponseButton_GenerateMenu), null, pawn.playerSettings.hostilityResponse.GetIcon(), null, null, delegate()
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HostilityResponse, KnowledgeAmount.SpecificInteraction);
			}, paintable, new float?(4f));
			UIHighlighter.HighlightOpportunity(rect, "HostilityResponse");
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, "HostilityReponseTip".Translate() + "\n\n" + "HostilityResponseCurrentMode".Translate() + ": " + pawn.playerSettings.hostilityResponse.GetLabel());
			}
		}

		// Token: 0x0600CE9B RID: 52891 RVA: 0x003CACCE File Offset: 0x003C8ECE
		private static HostilityResponseMode DrawResponseButton_GetResponse(Pawn pawn)
		{
			return pawn.playerSettings.hostilityResponse;
		}

		// Token: 0x0600CE9C RID: 52892 RVA: 0x003CACDB File Offset: 0x003C8EDB
		private static IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>> DrawResponseButton_GenerateMenu(Pawn p)
		{
			using (IEnumerator enumerator = Enum.GetValues(typeof(HostilityResponseMode)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HostilityResponseMode response = (HostilityResponseMode)enumerator.Current;
					if (response != HostilityResponseMode.Attack || !p.WorkTagIsDisabled(WorkTags.Violent))
					{
						yield return new Widgets.DropdownMenuElement<HostilityResponseMode>
						{
							option = new FloatMenuOption(response.GetLabel(), delegate()
							{
								p.playerSettings.hostilityResponse = response;
							}, response.GetIcon(), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false),
							payload = response
						};
					}
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04008C91 RID: 35985
		private static readonly Texture2D IgnoreIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Ignore", true);

		// Token: 0x04008C92 RID: 35986
		private static readonly Texture2D AttackIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Attack", true);

		// Token: 0x04008C93 RID: 35987
		private static readonly Texture2D FleeIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Flee", true);

		// Token: 0x04008C94 RID: 35988
		private static readonly Color IconColor = new Color(0.84f, 0.84f, 0.84f);
	}
}
