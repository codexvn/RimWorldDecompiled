using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B6B RID: 2923
	public class FloatMenuMap : FloatMenu
	{
		// Token: 0x06004AF6 RID: 19190 RVA: 0x0017D20B File Offset: 0x0017B40B
		public FloatMenuMap(List<FloatMenuOption> options, string title, Vector3 clickPos) : base(options, title, false)
		{
			this.clickPos = clickPos;
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x0017D220 File Offset: 0x0017B420
		public override void DoWindowContents(Rect inRect)
		{
			if (!Find.Selector.AnyPawnSelected)
			{
				Find.WindowStack.TryRemove(this, true);
				return;
			}
			bool flag = this.options.Count >= 3;
			if (Time.frameCount % 4 == 0 || this.lastOptionsForRevalidation == null)
			{
				FloatMenuContext floatMenuContext;
				this.lastOptionsForRevalidation = FloatMenuMakerMap.GetOptions(Find.Selector.SelectedPawns, this.clickPos, out floatMenuContext);
				FloatMenuMap.cachedChoices.Clear();
				FloatMenuMap.cachedChoices.Add(this.clickPos, this.lastOptionsForRevalidation);
				if (!flag)
				{
					for (int i = 0; i < this.options.Count; i++)
					{
						this.<DoWindowContents>g__RevalidateOption|6_0(this.options[i]);
					}
				}
			}
			else if (flag)
			{
				if (this.nextOptionToRevalidate >= this.options.Count)
				{
					this.nextOptionToRevalidate = 0;
				}
				int num = Mathf.CeilToInt((float)this.options.Count / 3f);
				int num2 = this.nextOptionToRevalidate;
				int num3 = 0;
				while (num2 < this.options.Count && num3 < num)
				{
					this.<DoWindowContents>g__RevalidateOption|6_0(this.options[num2]);
					this.nextOptionToRevalidate++;
					num2++;
					num3++;
				}
			}
			base.DoWindowContents(inRect);
		}

		// Token: 0x06004AF8 RID: 19192 RVA: 0x0017D368 File Offset: 0x0017B568
		private static bool StillValid(FloatMenuOption opt, List<FloatMenuOption> curOpts)
		{
			if (opt.revalidateClickTarget == null)
			{
				for (int i = 0; i < curOpts.Count; i++)
				{
					if (FloatMenuMap.OptionsMatch(opt, curOpts[i]))
					{
						return true;
					}
				}
			}
			else
			{
				if (!opt.targetsDespawned && !opt.revalidateClickTarget.Spawned)
				{
					return false;
				}
				Vector3 key = opt.revalidateClickTarget.PositionHeld.ToVector3Shifted();
				List<FloatMenuOption> list;
				if (!FloatMenuMap.cachedChoices.TryGetValue(key, out list))
				{
					FloatMenuContext floatMenuContext;
					List<FloatMenuOption> options = FloatMenuMakerMap.GetOptions(Find.Selector.SelectedPawns, key, out floatMenuContext);
					FloatMenuMap.cachedChoices.Add(key, options);
					list = options;
				}
				for (int j = 0; j < list.Count; j++)
				{
					if (FloatMenuMap.OptionsMatch(opt, list[j]))
					{
						return !list[j].Disabled;
					}
				}
			}
			return false;
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x0017D43C File Offset: 0x0017B63C
		public override void PreOptionChosen(FloatMenuOption opt)
		{
			base.PreOptionChosen(opt);
			FloatMenuContext floatMenuContext;
			if (!opt.Disabled && !FloatMenuMap.StillValid(opt, FloatMenuMakerMap.GetOptions(Find.Selector.SelectedPawns, this.clickPos, out floatMenuContext)))
			{
				opt.Disabled = true;
			}
		}

		// Token: 0x06004AFA RID: 19194 RVA: 0x0017D47E File Offset: 0x0017B67E
		private static bool OptionsMatch(FloatMenuOption a, FloatMenuOption b)
		{
			return a.Label == b.Label;
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x0017D4A2 File Offset: 0x0017B6A2
		[CompilerGenerated]
		private void <DoWindowContents>g__RevalidateOption|6_0(FloatMenuOption option)
		{
			if (!option.Disabled && !FloatMenuMap.StillValid(option, this.lastOptionsForRevalidation))
			{
				option.Disabled = true;
			}
		}

		// Token: 0x04003537 RID: 13623
		private Vector3 clickPos;

		// Token: 0x04003538 RID: 13624
		private static Dictionary<Vector3, List<FloatMenuOption>> cachedChoices = new Dictionary<Vector3, List<FloatMenuOption>>();

		// Token: 0x04003539 RID: 13625
		private List<FloatMenuOption> lastOptionsForRevalidation;

		// Token: 0x0400353A RID: 13626
		private int nextOptionToRevalidate;

		// Token: 0x0400353B RID: 13627
		public const int RevalidateEveryFrame = 4;
	}
}
