using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B9B RID: 2971
	public class BundleLetter : Letter
	{
		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06004BF9 RID: 19449 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004BFA RID: 19450 RVA: 0x00182F30 File Offset: 0x00181130
		public void SetLetters(List<Letter> letters)
		{
			if (GenCollection.ListsEqual<Letter>(letters, this.bundledLetters))
			{
				return;
			}
			this.bundledLetters.Clear();
			this.bundledLetters.AddRange(letters);
			this.floatMenuOptions.Clear();
			using (List<Letter>.Enumerator enumerator = letters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Letter letter = enumerator.Current;
					FloatMenuOption item = new FloatMenuOption(letter.Label, delegate()
					{
						letter.OpenLetter();
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					this.floatMenuOptions.Add(item);
				}
			}
			base.Label = this.bundledLetters.Count.ToString() + " " + "MoreLower".Translate() + "...";
		}

		// Token: 0x06004BFB RID: 19451 RVA: 0x0018302C File Offset: 0x0018122C
		public override void OpenLetter()
		{
			if (Event.current.button == 0)
			{
				Find.WindowStack.Add(new FloatMenu(this.floatMenuOptions));
			}
		}

		// Token: 0x06004BFC RID: 19452 RVA: 0x00183050 File Offset: 0x00181250
		protected override string GetMouseoverText()
		{
			return "MoreLetters".Translate(this.bundledLetters.Count) + ":\n\n" + (from l in this.bundledLetters
			select l.Label.ToString()).ToLineList(" - ", false);
		}

		// Token: 0x04003642 RID: 13890
		private List<Letter> bundledLetters = new List<Letter>();

		// Token: 0x04003643 RID: 13891
		private List<FloatMenuOption> floatMenuOptions = new List<FloatMenuOption>();
	}
}
