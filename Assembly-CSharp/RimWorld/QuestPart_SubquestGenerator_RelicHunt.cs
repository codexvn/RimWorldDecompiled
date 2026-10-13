using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CBA RID: 7354
	public class QuestPart_SubquestGenerator_RelicHunt : QuestPart_SubquestGenerator
	{
		// Token: 0x0600A3DB RID: 41947 RVA: 0x002EFF30 File Offset: 0x002EE130
		protected override Slate InitSlate()
		{
			float var = 0f;
			if (this.useMapParentThreatPoints != null)
			{
				if (this.useMapParentThreatPoints.HasMap)
				{
					var = StorytellerUtility.DefaultThreatPointsNow(this.useMapParentThreatPoints.Map);
				}
				else if (Find.AnyPlayerHomeMap != null)
				{
					var = StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap);
				}
				else
				{
					var = StorytellerUtility.DefaultThreatPointsNow(Find.World);
				}
			}
			Slate slate = new Slate();
			slate.Set<float>("points", var, false);
			slate.Set<Precept_Relic>(this.relicSlateName, this.relic, false);
			return slate;
		}

		// Token: 0x0600A3DC RID: 41948 RVA: 0x002EFFB0 File Offset: 0x002EE1B0
		protected override QuestScriptDef GetNextSubquestDef()
		{
			this.ShuffleQueue();
			QuestScriptDef questScriptDef = this.questQueue.First<QuestScriptDef>();
			MapParent mapParent = this.useMapParentThreatPoints;
			IIncidentTarget target;
			if (mapParent != null && mapParent.HasMap)
			{
				target = this.useMapParentThreatPoints.Map;
			}
			else if (Find.AnyPlayerHomeMap != null)
			{
				target = Find.AnyPlayerHomeMap;
			}
			else
			{
				target = Find.World;
			}
			if (!questScriptDef.CanRun(this.InitSlate(), target))
			{
				return null;
			}
			this.questQueue.RemoveAt(0);
			return questScriptDef;
		}

		// Token: 0x0600A3DD RID: 41949 RVA: 0x002F0024 File Offset: 0x002EE224
		private void ShuffleQueue()
		{
			this.questQueue.Clear();
			if (this.subquestDefs.Count == 1)
			{
				this.questQueue.AddRange(this.subquestDefs);
				return;
			}
			Quest quest = (from q in this.quest.GetSubquests(null)
			orderby q.appearanceTick descending
			select q).FirstOrDefault<Quest>();
			QuestScriptDef questScriptDef = (quest != null) ? quest.root : null;
			int i = 100;
			while (i > 0)
			{
				i--;
				this.questQueue.Clear();
				this.questQueue.AddRange(this.subquestDefs.InRandomOrder(null));
				if (questScriptDef == null || this.questQueue.First<QuestScriptDef>() != questScriptDef)
				{
					break;
				}
			}
		}

		// Token: 0x0600A3DE RID: 41950 RVA: 0x002F00E8 File Offset: 0x002EE2E8
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			base.DoDebugWindowContents(innerRect, ref curY);
			if (Widgets.ButtonText(new Rect(innerRect.x, curY, 500f, 25f), "Shuffle subquest queue", true, true, true, null))
			{
				this.ShuffleQueue();
			}
			curY += 29f;
			if (Widgets.ButtonText(new Rect(innerRect.x, curY, 500f, 25f), "Log subquest queue", true, true, true, null))
			{
				if (this.questQueue.Count == 0)
				{
					this.ShuffleQueue();
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.questQueue.Count; i++)
				{
					stringBuilder.AppendLine(string.Format("{0} ->", i) + this.questQueue[i].defName);
				}
				Log.Message(stringBuilder.ToString());
			}
			curY += 29f;
		}

		// Token: 0x0600A3DF RID: 41951 RVA: 0x002F01E0 File Offset: 0x002EE3E0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Precept_Relic>(ref this.relic, "relic", false);
			Scribe_Values.Look<string>(ref this.relicSlateName, "relicSlateName", null, false);
			Scribe_Collections.Look<QuestScriptDef>(ref this.questQueue, "questQueue", LookMode.Def, Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.useMapParentThreatPoints, "useMapParentThreatPoints", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.questQueue == null)
			{
				this.questQueue = new List<QuestScriptDef>();
			}
		}

		// Token: 0x0400712E RID: 28974
		private List<QuestScriptDef> questQueue = new List<QuestScriptDef>();

		// Token: 0x0400712F RID: 28975
		public Precept_Relic relic;

		// Token: 0x04007130 RID: 28976
		public string relicSlateName;

		// Token: 0x04007131 RID: 28977
		public MapParent useMapParentThreatPoints;
	}
}
