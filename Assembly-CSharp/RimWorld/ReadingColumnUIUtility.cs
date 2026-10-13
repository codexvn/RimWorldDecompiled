using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003474 RID: 13428
	public static class ReadingColumnUIUtility
	{
		// Token: 0x06012E20 RID: 77344 RVA: 0x00597DC0 File Offset: 0x00595FC0
		public static void DoAssignReadingButtons(Rect rect, Pawn pawn)
		{
			Rect rect2 = rect.ContractedBy(0f, 2f);
			string label = pawn.reading.CurrentPolicy.label;
			Widgets.Dropdown<Pawn, ReadingPolicy>(rect2, pawn, (Pawn p) => p.reading.CurrentPolicy, new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<ReadingPolicy>>>(ReadingColumnUIUtility.Button_GenerateMenu), label.Truncate(rect2.width, null), null, pawn.reading.CurrentPolicy.label, null, null, true);
			UIHighlighter.HighlightOpportunity(rect2, "ReadingAssignPolicy");
		}

		// Token: 0x06012E21 RID: 77345 RVA: 0x00597E4E File Offset: 0x0059604E
		private static IEnumerable<Widgets.DropdownMenuElement<ReadingPolicy>> Button_GenerateMenu(Pawn pawn)
		{
			using (List<ReadingPolicy>.Enumerator enumerator = Current.Game.readingPolicyDatabase.AllReadingPolicies.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ReadingPolicy policy = enumerator.Current;
					yield return new Widgets.DropdownMenuElement<ReadingPolicy>
					{
						option = new FloatMenuOption(policy.label, delegate()
						{
							pawn.reading.CurrentPolicy = policy;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
						payload = policy
					};
				}
			}
			List<ReadingPolicy>.Enumerator enumerator = default(List<ReadingPolicy>.Enumerator);
			yield return new Widgets.DropdownMenuElement<ReadingPolicy>
			{
				option = new FloatMenuOption(string.Format("{0}...", "AssignTabEdit".Translate()), delegate()
				{
					Find.WindowStack.Add(new Dialog_ManageReadingPolicies(pawn.reading.CurrentPolicy));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
			};
			yield break;
			yield break;
		}

		// Token: 0x0400C3C1 RID: 50113
		public const string ReadingAssigningTutorHighlightTag = "ReadingAssignPolicy";
	}
}
