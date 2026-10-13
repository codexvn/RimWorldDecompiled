using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B7F RID: 7039
	public class QuestPart_Delay : QuestPartActivable
	{
		// Token: 0x1700182C RID: 6188
		// (get) Token: 0x06009D2B RID: 40235 RVA: 0x002D92E4 File Offset: 0x002D74E4
		public int TicksLeft
		{
			get
			{
				if (base.State != QuestPartState.Enabled)
				{
					return 0;
				}
				return this.enableTick + this.delayTicks - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x1700182D RID: 6189
		// (get) Token: 0x06009D2C RID: 40236 RVA: 0x002D9309 File Offset: 0x002D7509
		public override string ExpiryInfoPart
		{
			get
			{
				if (this.quest.Historical)
				{
					return null;
				}
				return this.expiryInfoPart.Formatted(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
		}

		// Token: 0x1700182E RID: 6190
		// (get) Token: 0x06009D2D RID: 40237 RVA: 0x002D933F File Offset: 0x002D753F
		public override string ExpiryInfoPartTip
		{
			get
			{
				return this.expiryInfoPartTip.Formatted(GenDate.DateFullStringWithHourAt((long)GenDate.TickGameToAbs(this.enableTick + this.delayTicks), QuestUtility.GetLocForDates()));
			}
		}

		// Token: 0x1700182F RID: 6191
		// (get) Token: 0x06009D2E RID: 40238 RVA: 0x002D9374 File Offset: 0x002D7574
		public override string AlertLabel
		{
			get
			{
				string text = this.alertLabel;
				TaggedString? taggedString = (text != null) ? new TaggedString?(text.Formatted(this.TicksLeft.ToStringTicksToPeriodVerbose(true, true))) : null;
				if (taggedString == null)
				{
					return null;
				}
				return taggedString.GetValueOrDefault();
			}
		}

		// Token: 0x17001830 RID: 6192
		// (get) Token: 0x06009D2F RID: 40239 RVA: 0x002D93CC File Offset: 0x002D75CC
		public override string AlertExplanation
		{
			get
			{
				string text = this.alertExplanation;
				TaggedString? taggedString = (text != null) ? new TaggedString?(text.Formatted(this.TicksLeft.ToStringTicksToPeriodVerbose(true, true))) : null;
				if (taggedString == null)
				{
					return null;
				}
				return taggedString.GetValueOrDefault();
			}
		}

		// Token: 0x17001831 RID: 6193
		// (get) Token: 0x06009D30 RID: 40240 RVA: 0x002D9422 File Offset: 0x002D7622
		public override AlertReport AlertReport
		{
			get
			{
				if (this.alertCulprits.Count <= 0)
				{
					return AlertReport.Inactive;
				}
				return AlertReport.CulpritsAre(this.alertCulprits);
			}
		}

		// Token: 0x17001832 RID: 6194
		// (get) Token: 0x06009D31 RID: 40241 RVA: 0x002D9443 File Offset: 0x002D7643
		public override bool AlertCritical
		{
			get
			{
				return this.TicksLeft < this.ticksLeftAlertCritical;
			}
		}

		// Token: 0x17001833 RID: 6195
		// (get) Token: 0x06009D32 RID: 40242 RVA: 0x002D9453 File Offset: 0x002D7653
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.inspectStringTargets != null)
				{
					int num;
					for (int i = 0; i < this.inspectStringTargets.Count; i = num + 1)
					{
						ISelectable selectable = this.inspectStringTargets[i];
						if (selectable is Thing)
						{
							yield return (Thing)selectable;
						}
						else if (selectable is WorldObject)
						{
							yield return (WorldObject)selectable;
						}
						num = i;
					}
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06009D33 RID: 40243 RVA: 0x002D9463 File Offset: 0x002D7663
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.waitUntilPlayerHasHomeMap && Find.AnyPlayerHomeMap == null)
			{
				return;
			}
			if (Find.TickManager.TicksGame >= this.enableTick + this.delayTicks)
			{
				this.DelayFinished();
			}
		}

		// Token: 0x06009D34 RID: 40244 RVA: 0x002D949A File Offset: 0x002D769A
		protected virtual void DelayFinished()
		{
			base.Complete();
		}

		// Token: 0x06009D35 RID: 40245 RVA: 0x002D94A4 File Offset: 0x002D76A4
		public override string ExtraInspectString(ISelectable target)
		{
			if (this.inspectStringTargets != null && this.inspectStringTargets.Contains(target))
			{
				return this.inspectString.Formatted(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
			}
			return null;
		}

		// Token: 0x06009D36 RID: 40246 RVA: 0x002D94F4 File Offset: 0x002D76F4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPart, "expiryInfoPart", null, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPartTip, "expiryInfoPartTip", null, false);
			Scribe_Values.Look<string>(ref this.inspectString, "inspectString", null, false);
			Scribe_Collections.Look<ISelectable>(ref this.inspectStringTargets, "inspectStringTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.isBad, "isBad", false, false);
			Scribe_Values.Look<string>(ref this.alertLabel, "alertLabel", null, false);
			Scribe_Values.Look<string>(ref this.alertExplanation, "alertExplanation", null, false);
			Scribe_Values.Look<int>(ref this.ticksLeftAlertCritical, "ticksLeftAlertCritical", 0, false);
			Scribe_Values.Look<bool>(ref this.waitUntilPlayerHasHomeMap, "waitUntilPlayerHasHomeMap", false, false);
			Scribe_Collections.Look<GlobalTargetInfo>(ref this.alertCulprits, "alertCulprits", LookMode.GlobalTargetInfo, Array.Empty<object>());
			if (this.alertCulprits == null)
			{
				this.alertCulprits = new List<GlobalTargetInfo>();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.alertCulprits.RemoveAll((GlobalTargetInfo x) => !x.IsValid);
			}
		}

		// Token: 0x06009D37 RID: 40247 RVA: 0x002D961C File Offset: 0x002D781C
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "End " + this.ToString(), true, true, true, null))
			{
				this.DelayFinished();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x06009D38 RID: 40248 RVA: 0x002D9689 File Offset: 0x002D7889
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.delayTicks = Rand.RangeInclusive(833, 2500);
		}

		// Token: 0x06009D39 RID: 40249 RVA: 0x002D96A6 File Offset: 0x002D78A6
		public void DebugForceEnd()
		{
			this.DelayFinished();
		}

		// Token: 0x04006CF8 RID: 27896
		public int delayTicks;

		// Token: 0x04006CF9 RID: 27897
		public string expiryInfoPart;

		// Token: 0x04006CFA RID: 27898
		public string expiryInfoPartTip;

		// Token: 0x04006CFB RID: 27899
		public string inspectString;

		// Token: 0x04006CFC RID: 27900
		public List<ISelectable> inspectStringTargets;

		// Token: 0x04006CFD RID: 27901
		public bool isBad;

		// Token: 0x04006CFE RID: 27902
		public bool waitUntilPlayerHasHomeMap;

		// Token: 0x04006CFF RID: 27903
		public string alertLabel;

		// Token: 0x04006D00 RID: 27904
		public string alertExplanation;

		// Token: 0x04006D01 RID: 27905
		public List<GlobalTargetInfo> alertCulprits = new List<GlobalTargetInfo>();

		// Token: 0x04006D02 RID: 27906
		public int ticksLeftAlertCritical;
	}
}
