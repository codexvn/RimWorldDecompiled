using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BBC RID: 15292
	public class QuestPart_MonolithPart : QuestPart_DescriptionPart
	{
		// Token: 0x06014C45 RID: 85061 RVA: 0x00638945 File Offset: 0x00636B45
		public QuestPart_MonolithPart()
		{
		}

		// Token: 0x06014C46 RID: 85062 RVA: 0x0063894D File Offset: 0x00636B4D
		public QuestPart_MonolithPart(Building_VoidMonolith monolith)
		{
			this.inSignalEnable = "MonolithLevelChanged";
			this.monolith = monolith;
		}

		// Token: 0x1700337E RID: 13182
		// (get) Token: 0x06014C47 RID: 85063 RVA: 0x00638967 File Offset: 0x00636B67
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				bool flag = false;
				if (ModsConfig.IdeologyActive)
				{
					foreach (Quest quest in Find.QuestManager.QuestsListForReading)
					{
						if (quest.State == QuestState.Ongoing && (quest.root == QuestScriptDefOf.EndGame_ArchonexusVictory_FirstCycle || quest.root == QuestScriptDefOf.EndGame_ArchonexusVictory_SecondCycle))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					yield return this.monolith;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06014C48 RID: 85064 RVA: 0x00638977 File Offset: 0x00636B77
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == "MonolithLevelChanged" || signal.tag == "EntityDiscovered")
			{
				this.UpdateDescription();
			}
		}

		// Token: 0x06014C49 RID: 85065 RVA: 0x006389AC File Offset: 0x00636BAC
		private void UpdateDescription()
		{
			if (Find.Anomaly.LevelDef.postEndgame)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = Find.Anomaly.Level;
			if (Find.Anomaly.LevelDef == MonolithLevelDefOf.Gleaming)
			{
				num--;
			}
			stringBuilder.AppendLine(Find.Anomaly.LevelDef.extraQuestDescription);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("CurrentMonolithLevel".Translate() + ": " + num.ToString());
			MonolithLevelDef nextLevelDef = Find.Anomaly.NextLevelDef;
			EntityCategoryDef entityCategoryDef = (nextLevelDef != null) ? nextLevelDef.entityCatagoryCompletionRequired : null;
			if (entityCategoryDef != null)
			{
				stringBuilder.AppendLine();
				int num2 = Find.EntityCodex.DiscoveredCount(entityCategoryDef);
				if (num2 >= Find.Anomaly.NextLevelDef.entityCountCompletionRequired)
				{
					stringBuilder.AppendLine("VoidMonolithAllEntitiesDiscovered".Translate()).AppendLine(Find.Anomaly.LevelDef.activateQuestText);
				}
				else
				{
					int entityCountCompletionRequired = Find.Anomaly.NextLevelDef.entityCountCompletionRequired;
					stringBuilder.AppendLine("VoidMonolithRequiredEntityCategory".Translate(entityCountCompletionRequired, entityCategoryDef.label)).AppendLine().AppendLine("DiscoveredCount".Translate(num2.ToString() + " / " + entityCountCompletionRequired.ToString()).CapitalizeFirst());
				}
			}
			this.resolvedDescriptionPart = stringBuilder.ToString();
		}

		// Token: 0x06014C4A RID: 85066 RVA: 0x00638B2C File Offset: 0x00636D2C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Building_VoidMonolith>(ref this.monolith, "monolith", false);
		}

		// Token: 0x06014C4B RID: 85067 RVA: 0x00638B45 File Offset: 0x00636D45
		public override void Cleanup()
		{
			base.Cleanup();
			this.monolith = null;
		}

		// Token: 0x0400E72C RID: 59180
		private Building_VoidMonolith monolith;
	}
}
