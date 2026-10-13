using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x0200336F RID: 13167
	public class ChoiceLetter_GameEnded : ChoiceLetter
	{
		// Token: 0x1700304E RID: 12366
		// (get) Token: 0x06012828 RID: 75816 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700304F RID: 12367
		// (get) Token: 0x06012829 RID: 75817 RVA: 0x00575B43 File Offset: 0x00573D43
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (base.ArchivedOnly)
				{
					yield return base.Option_Close;
				}
				else
				{
					if (!Find.GameEnder.CanSpawnNewWanderers())
					{
						yield return new DiaOption("GameOverKeepWatching".Translate())
						{
							resolveTree = true
						};
					}
					else
					{
						yield return new DiaOption("GameOverKeepWatchingForNow".Translate())
						{
							resolveTree = true
						};
						ChoiceLetter_GameEnded.<>c__DisplayClass3_0 CS$<>8__locals1;
						CS$<>8__locals1.hoursRemaining = (float)(20000 - (GenTicks.TicksGame - this.arrivalTick)) / 2500f;
						DiaOption diaOption = new DiaOption("GameOverCreateNewWanderers".Translate());
						diaOption.action = delegate()
						{
							Find.WindowStack.Add(new Dialog_ChooseNewWanderers());
						};
						diaOption.resolveTree = true;
						diaOption.disabled = !ChoiceLetter_GameEnded.<get_Choices>g__CanCreateNewWanderers|3_0(ref CS$<>8__locals1);
						diaOption.disabledReason = ChoiceLetter_GameEnded.<get_Choices>g__CanCreateNewWanderers|3_0(ref CS$<>8__locals1).Reason;
						yield return diaOption;
					}
					yield return new DiaOption("GameOverMainMenu".Translate())
					{
						action = new Action(GenScene.GoToMainMenu),
						resolveTree = true
					};
				}
				yield break;
			}
		}

		// Token: 0x0601282B RID: 75819 RVA: 0x00575B54 File Offset: 0x00573D54
		[CompilerGenerated]
		internal static AcceptanceReport <get_Choices>g__CanCreateNewWanderers|3_0(ref ChoiceLetter_GameEnded.<>c__DisplayClass3_0 A_0)
		{
			if (A_0.hoursRemaining > 0f)
			{
				return "GameOverCreateNewWanderersWait".Translate(Math.Ceiling((double)A_0.hoursRemaining));
			}
			bool flag = false;
			if (Current.Game == null)
			{
				return false;
			}
			using (IEnumerator<Map> enumerator = Current.Game.PlayerHomeMaps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Tile.Layer.IsRootSurface)
					{
						return true;
					}
					flag = true;
				}
			}
			if (flag)
			{
				return "NoWandererDestination".Translate();
			}
			return "NoColony".Translate();
		}
	}
}
