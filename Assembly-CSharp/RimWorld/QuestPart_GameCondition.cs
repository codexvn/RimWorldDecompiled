using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CFC RID: 7420
	public class QuestPart_GameCondition : QuestPart
	{
		// Token: 0x17001992 RID: 6546
		// (get) Token: 0x0600A5B4 RID: 42420 RVA: 0x002F7117 File Offset: 0x002F5317
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A5B5 RID: 42421 RVA: 0x002F7128 File Offset: 0x002F5328
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && (this.targetWorld || this.mapParent != null) && this.gameCondition != null)
			{
				this.gameCondition.quest = this.quest;
				if (this.targetWorld)
				{
					Find.World.gameConditionManager.RegisterCondition(this.gameCondition);
				}
				else
				{
					Map map = this.mapParent.Map ?? Find.AnyPlayerHomeMap;
					if (!this.gameCondition.CanApplyOnMap(map))
					{
						bool flag = false;
						foreach (Map map2 in Current.Game.PlayerHomeMaps)
						{
							if (this.gameCondition.CanApplyOnMap(map2))
							{
								map = map2;
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							return;
						}
					}
					map.gameConditionManager.RegisterCondition(this.gameCondition);
				}
				if (this.sendStandardLetter)
				{
					Find.LetterStack.ReceiveLetter(this.gameCondition.LabelCap, this.gameCondition.LetterText, this.gameCondition.def.letterDef, LookTargets.Invalid, null, this.quest, null, null, 0, true);
				}
				this.gameCondition = null;
			}
		}

		// Token: 0x0600A5B6 RID: 42422 RVA: 0x002F7284 File Offset: 0x002F5484
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<GameCondition>(ref this.gameCondition, "gameCondition", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.targetWorld, "targetWorld", false, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
		}

		// Token: 0x0600A5B7 RID: 42423 RVA: 0x002F72F4 File Offset: 0x002F54F4
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.gameCondition = GameConditionMaker.MakeCondition(GameConditionDefOf.ColdSnap, Rand.RangeInclusive(2500, 7500));
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
			}
		}

		// Token: 0x0400727B RID: 29307
		public string inSignal;

		// Token: 0x0400727C RID: 29308
		public GameCondition gameCondition;

		// Token: 0x0400727D RID: 29309
		public bool targetWorld;

		// Token: 0x0400727E RID: 29310
		public MapParent mapParent;

		// Token: 0x0400727F RID: 29311
		public bool sendStandardLetter = true;
	}
}
