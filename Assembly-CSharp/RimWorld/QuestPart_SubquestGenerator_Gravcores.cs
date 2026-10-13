using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CBD RID: 7357
	public class QuestPart_SubquestGenerator_Gravcores : QuestPart_SubquestGenerator
	{
		// Token: 0x17001939 RID: 6457
		// (get) Token: 0x0600A3E9 RID: 41961 RVA: 0x002F0384 File Offset: 0x002EE584
		private int MTB
		{
			get
			{
				if (!this.givenFirstSubquest)
				{
					return 120000;
				}
				return 300000;
			}
		}

		// Token: 0x1700193A RID: 6458
		// (get) Token: 0x0600A3EA RID: 41962 RVA: 0x002F0399 File Offset: 0x002EE599
		private int MinTime
		{
			get
			{
				if (!this.givenFirstSubquest)
				{
					return 300000;
				}
				return 900000;
			}
		}

		// Token: 0x1700193B RID: 6459
		// (get) Token: 0x0600A3EB RID: 41963 RVA: 0x002F03AE File Offset: 0x002EE5AE
		private int MaxTime
		{
			get
			{
				if (!this.givenFirstSubquest)
				{
					return 480000;
				}
				return 1800000;
			}
		}

		// Token: 0x1700193C RID: 6460
		// (get) Token: 0x0600A3EC RID: 41964 RVA: 0x002F03C4 File Offset: 0x002EE5C4
		protected override bool CanGenerateSubquest
		{
			get
			{
				if (!base.CanGenerateSubquest)
				{
					return false;
				}
				if ((float)Find.TickManager.TicksGame - this.lastSubquestTick < (float)this.MinTime)
				{
					return false;
				}
				using (List<Map>.Enumerator enumerator = Find.Maps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.listerBuildings.ColonistsHaveBuilding(ThingDefOf.GravEngine))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0600A3ED RID: 41965 RVA: 0x002F0450 File Offset: 0x002EE650
		protected override QuestScriptDef GetNextSubquestDef()
		{
			this.tmpSubquestDefs.Clear();
			this.GetPossibleSubquests(this.tmpSubquestDefs);
			QuestScriptDef result;
			if (this.tmpSubquestDefs.TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600A3EE RID: 41966 RVA: 0x002F0488 File Offset: 0x002EE688
		private void GetPossibleSubquests(List<QuestScriptDef> outList)
		{
			IEnumerable<Quest> subquests = this.quest.GetSubquests(null);
			IEnumerable<Quest> source = from x in subquests
			where x.State == QuestState.Ongoing
			select x;
			IEnumerable<Quest> source2 = from x in subquests
			where x.State == QuestState.EndedSuccess
			select x;
			int num = source.Count<Quest>() + source2.Count<Quest>();
			bool flag = num >= this.subquestDefs.Count;
			bool flag2;
			if (!source.Any((Quest x) => x.root == QuestScriptDefOf.Gravcore_Mechhive))
			{
				flag2 = source2.Any((Quest x) => x.root == QuestScriptDefOf.Gravcore_Mechhive);
			}
			else
			{
				flag2 = true;
			}
			bool flag3 = flag2;
			using (List<QuestScriptDef>.Enumerator enumerator = this.subquestDefs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QuestScriptDef def = enumerator.Current;
					QuestNode_Root_Gravcore questNode_Root_Gravcore = def.root as QuestNode_Root_Gravcore;
					if (questNode_Root_Gravcore != null && questNode_Root_Gravcore.requiredSubquestsGiven <= num && (flag || (!source2.Any((Quest x) => x.root == def) && !source.Any((Quest x) => x.root == def))) && (!flag3 || def != QuestScriptDefOf.Gravcore_Mechhive) && def.CanRun(this.InitSlate(), Find.World))
					{
						outList.Add(def);
					}
				}
			}
		}

		// Token: 0x0600A3EF RID: 41967 RVA: 0x002F0640 File Offset: 0x002EE840
		protected override bool TryGenerateSubquest()
		{
			bool flag = base.TryGenerateSubquest();
			if (flag)
			{
				this.lastSubquestTick = (float)Find.TickManager.TicksGame;
				this.givenFirstSubquest = true;
				return flag;
			}
			Log.Warning("Failed to generate gravcore subquest, trying again in 6 hours");
			this.lastSubquestTick += 15000f;
			return flag;
		}

		// Token: 0x0600A3F0 RID: 41968 RVA: 0x002F0680 File Offset: 0x002EE880
		protected override Slate InitSlate()
		{
			float var;
			if (Find.AnyPlayerHomeMap != null)
			{
				var = StorytellerUtility.DefaultThreatPointsNow(Find.AnyPlayerHomeMap);
			}
			else
			{
				var = StorytellerUtility.DefaultThreatPointsNow(Find.World);
			}
			Slate slate = new Slate();
			slate.Set<float>("points", var, false);
			return slate;
		}

		// Token: 0x0600A3F1 RID: 41969 RVA: 0x002F06C0 File Offset: 0x002EE8C0
		public override void QuestPartTick()
		{
			if (this.subquestDefs.Count == 0)
			{
				return;
			}
			if (base.Paused)
			{
				return;
			}
			if (this.CanGenerateSubquest && (Rand.MTBEventOccurs((float)this.MTB, 1f, 1f) || (float)Find.TickManager.TicksGame > this.lastSubquestTick + (float)this.MaxTime))
			{
				this.TryGenerateSubquest();
			}
		}

		// Token: 0x0600A3F2 RID: 41970 RVA: 0x002F0726 File Offset: 0x002EE926
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.lastSubquestTick, "lastSubquestTick", 0f, false);
			Scribe_Values.Look<bool>(ref this.givenFirstSubquest, "givenFirstSubquest", false, false);
		}

		// Token: 0x04007137 RID: 28983
		private float lastSubquestTick;

		// Token: 0x04007138 RID: 28984
		private bool givenFirstSubquest;

		// Token: 0x04007139 RID: 28985
		private const int MTBSubquestsTicks = 300000;

		// Token: 0x0400713A RID: 28986
		private const int MTBFirstSubquestTicks = 120000;

		// Token: 0x0400713B RID: 28987
		private const int MinTimeBetweenSubquests = 900000;

		// Token: 0x0400713C RID: 28988
		private const int MaxTimeBetweenSubquests = 1800000;

		// Token: 0x0400713D RID: 28989
		private const int MinTimeFirstSubquest = 300000;

		// Token: 0x0400713E RID: 28990
		private const int MaxTimeFirstSubquest = 480000;

		// Token: 0x0400713F RID: 28991
		private List<QuestScriptDef> tmpSubquestDefs = new List<QuestScriptDef>();
	}
}
