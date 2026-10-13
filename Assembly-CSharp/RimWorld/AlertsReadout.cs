using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003064 RID: 12388
	public class AlertsReadout
	{
		// Token: 0x17002D06 RID: 11526
		// (get) Token: 0x06011647 RID: 71239 RVA: 0x004FD5AC File Offset: 0x004FB7AC
		public float AlertsHeight
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.activeAlerts.Count; i++)
				{
					num += this.activeAlerts[i].Height;
				}
				return num;
			}
		}

		// Token: 0x06011648 RID: 71240 RVA: 0x004FD5EC File Offset: 0x004FB7EC
		public AlertsReadout()
		{
			DeepProfiler.Start("Instantiating Alerts");
			if (AlertsReadout.allAlertTypesCached == null)
			{
				AlertsReadout.allAlertTypesCached = new List<Type>();
				foreach (Type type in typeof(Alert).AllLeafSubclasses())
				{
					if (!typeof(Alert_Custom).IsAssignableFrom(type) && !typeof(Alert_CustomCritical).IsAssignableFrom(type))
					{
						AlertsReadout.allAlertTypesCached.Add(type);
					}
				}
			}
			this.AllAlerts.Clear();
			foreach (Type type2 in AlertsReadout.allAlertTypesCached)
			{
				Alert alert = (Alert)Activator.CreateInstance(type2);
				if (alert.EnabledWithActiveExpansions)
				{
					this.AllAlerts.Add(alert);
				}
			}
			DeepProfiler.End();
			if (this.PriosInDrawOrder == null)
			{
				this.PriosInDrawOrder = new List<AlertPriority>();
				foreach (object obj in Enum.GetValues(typeof(AlertPriority)))
				{
					AlertPriority item = (AlertPriority)obj;
					this.PriosInDrawOrder.Add(item);
				}
				this.PriosInDrawOrder.Reverse();
			}
		}

		// Token: 0x06011649 RID: 71241 RVA: 0x004FD7B0 File Offset: 0x004FB9B0
		public void AlertsReadoutUpdate()
		{
			if (Mathf.Max(Find.TickManager.TicksGame, Find.TutorialState.endTick) < 600)
			{
				return;
			}
			if (Find.Storyteller.def.disableAlerts)
			{
				this.activeAlerts.Clear();
				return;
			}
			this.curAlertIndex++;
			if (this.curAlertIndex >= 24)
			{
				this.curAlertIndex = 0;
			}
			for (int i = this.curAlertIndex; i < this.AllAlerts.Count; i += 24)
			{
				this.CheckAddOrRemoveAlert(this.AllAlerts[i], false);
			}
			using (new ProfilerBlock("Alerts from quests"))
			{
				if (Time.frameCount % 20 == 0)
				{
					List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
					for (int j = 0; j < questsListForReading.Count; j++)
					{
						List<QuestPart> partsListForReading = questsListForReading[j].PartsListForReading;
						for (int k = 0; k < partsListForReading.Count; k++)
						{
							QuestPartActivable questPartActivable = partsListForReading[k] as QuestPartActivable;
							if (questPartActivable != null)
							{
								Alert cachedAlert = questPartActivable.CachedAlert;
								if (cachedAlert != null)
								{
									bool flag = questsListForReading[j].State != QuestState.Ongoing || questPartActivable.State != QuestPartState.Enabled;
									bool alertDirty = questPartActivable.AlertDirty;
									this.CheckAddOrRemoveAlert(cachedAlert, flag || alertDirty);
									if (alertDirty)
									{
										questPartActivable.ClearCachedAlert();
									}
								}
							}
						}
					}
				}
			}
			using (new ProfilerBlock("Alerts from precepts"))
			{
				if (ModsConfig.IdeologyActive && Time.frameCount % 20 == 0)
				{
					foreach (List<Alert> list in this.activePreceptAlerts.Values)
					{
						list.Clear();
					}
					foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
					{
						foreach (Precept precept in ideo.PreceptsListForReading)
						{
							List<Alert> list2;
							if (!this.activePreceptAlerts.TryGetValue(precept, out list2))
							{
								list2 = new List<Alert>();
								this.activePreceptAlerts[precept] = list2;
							}
							foreach (Alert alert in precept.GetAlerts())
							{
								this.CheckAddOrRemoveAlert(alert, false);
								list2.Add(alert);
							}
						}
					}
					for (int l = 0; l < this.activeAlerts.Count; l++)
					{
						Alert_Precept alert_Precept = this.activeAlerts[l] as Alert_Precept;
						if (alert_Precept != null)
						{
							Precept sourcePrecept = alert_Precept.sourcePrecept;
							this.CheckAddOrRemoveAlert(this.activeAlerts[l], sourcePrecept != null && (!this.activePreceptAlerts.ContainsKey(sourcePrecept) || !this.activePreceptAlerts[sourcePrecept].Contains(alert_Precept)));
						}
					}
				}
			}
			using (new ProfilerBlock("Alerts from scenario"))
			{
				if (Time.frameCount % 20 == 0)
				{
					this.activeScenarioAlerts.Clear();
					foreach (ScenPart scenPart in Find.Scenario.AllParts)
					{
						foreach (Alert alert2 in scenPart.GetAlerts())
						{
							this.CheckAddOrRemoveAlert(alert2, false);
							this.activeScenarioAlerts.Add(alert2);
						}
					}
					for (int m = 0; m < this.activeAlerts.Count; m++)
					{
						Alert_Scenario alert_Scenario = this.activeAlerts[m] as Alert_Scenario;
						if (alert_Scenario != null)
						{
							this.CheckAddOrRemoveAlert(alert_Scenario, !this.activeScenarioAlerts.Contains(alert_Scenario));
						}
					}
					for (int n = 0; n < this.activeScenarioAlerts.Count; n++)
					{
						this.CheckAddOrRemoveAlert(this.activeScenarioAlerts[n], false);
					}
				}
			}
			using (new ProfilerBlock("Alerts from delayed actions"))
			{
				if (Time.frameCount % 20 == 0)
				{
					this.activeSignalActionAlerts.Clear();
					List<Map> maps = Find.Maps;
					for (int num = 0; num < maps.Count; num++)
					{
						List<Thing> list3 = maps[num].listerThings.ThingsInGroup(ThingRequestGroup.ActionDelay);
						for (int num2 = 0; num2 < list3.Count; num2++)
						{
							SignalAction_Delay signalAction_Delay = list3[num2] as SignalAction_Delay;
							if (signalAction_Delay.Activated && signalAction_Delay.Alert != null)
							{
								this.activeSignalActionAlerts.Add(signalAction_Delay.Alert);
							}
						}
					}
					for (int num3 = 0; num3 < this.activeAlerts.Count; num3++)
					{
						Alert_ActionDelay alert_ActionDelay = this.activeAlerts[num3] as Alert_ActionDelay;
						if (alert_ActionDelay != null)
						{
							this.CheckAddOrRemoveAlert(alert_ActionDelay, !this.activeSignalActionAlerts.Contains(alert_ActionDelay));
						}
					}
					for (int num4 = 0; num4 < this.activeSignalActionAlerts.Count; num4++)
					{
						this.CheckAddOrRemoveAlert(this.activeSignalActionAlerts[num4], false);
					}
				}
			}
			for (int num5 = this.activeAlerts.Count - 1; num5 >= 0; num5--)
			{
				Alert alert3 = this.activeAlerts[num5];
				try
				{
					this.activeAlerts[num5].AlertActiveUpdate();
				}
				catch (Exception ex)
				{
					string str = "Exception updating alert ";
					Alert alert4 = alert3;
					string str2 = (alert4 != null) ? alert4.ToString() : null;
					string str3 = ": ";
					Exception ex2 = ex;
					Log.ErrorOnce(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null), 743575);
					this.activeAlerts.RemoveAt(num5);
				}
			}
			if (this.mouseoverAlertIndex >= 0 && this.mouseoverAlertIndex < this.activeAlerts.Count)
			{
				IEnumerable<GlobalTargetInfo> allCulprits = this.activeAlerts[this.mouseoverAlertIndex].GetReport().AllCulprits;
				if (allCulprits != null)
				{
					foreach (GlobalTargetInfo target in allCulprits)
					{
						TargetHighlighter.Highlight(target, true, true, false);
					}
				}
			}
			this.mouseoverAlertIndex = -1;
		}

		// Token: 0x0601164A RID: 71242 RVA: 0x004FDF48 File Offset: 0x004FC148
		private void CheckAddOrRemoveAlert(Alert alert, bool forceRemove = false)
		{
			try
			{
				alert.Recalculate();
				if (!forceRemove && alert.Active)
				{
					if (!this.activeAlerts.Contains(alert))
					{
						this.activeAlerts.Add(alert);
						alert.Notify_Started();
					}
				}
				else
				{
					this.activeAlerts.Remove(alert);
				}
			}
			catch (Exception ex)
			{
				string str = "Exception processing alert ";
				string str2 = (alert != null) ? alert.ToString() : null;
				string str3 = ": ";
				Exception ex2 = ex;
				Log.ErrorOnce(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null), 743575);
				this.activeAlerts.Remove(alert);
			}
		}

		// Token: 0x0601164B RID: 71243 RVA: 0x004FDFEC File Offset: 0x004FC1EC
		public void AlertsReadoutOnGUI()
		{
			if (Event.current.type == EventType.Layout || Event.current.type == EventType.MouseDrag)
			{
				return;
			}
			if (this.activeAlerts.Count == 0)
			{
				return;
			}
			Alert alert = null;
			AlertPriority alertPriority = AlertPriority.Critical;
			bool flag = false;
			float alertsHeight = this.AlertsHeight;
			float num = Find.LetterStack.LastTopY - alertsHeight;
			Rect rect = new Rect((float)UI.screenWidth - 154f, num, 154f, this.lastFinalY - num);
			float num2 = GenUI.BackgroundDarkAlphaForText();
			if (num2 > 0.001f)
			{
				GUI.color = new Color(1f, 1f, 1f, num2);
				Widgets.DrawShadowAround(rect);
				GUI.color = Color.white;
			}
			float num3 = num;
			if (num3 < 0f)
			{
				num3 = 0f;
			}
			for (int i = 0; i < this.PriosInDrawOrder.Count; i++)
			{
				AlertPriority alertPriority2 = this.PriosInDrawOrder[i];
				for (int j = 0; j < this.activeAlerts.Count; j++)
				{
					Alert alert2 = this.activeAlerts[j];
					if (alert2.Priority == alertPriority2)
					{
						if (!flag)
						{
							alertPriority = alertPriority2;
							flag = true;
						}
						Rect rect2 = alert2.DrawAt(num3, alertPriority2 != alertPriority);
						if (Mouse.IsOver(rect2))
						{
							alert = alert2;
							this.mouseoverAlertIndex = j;
						}
						num3 += rect2.height;
					}
				}
			}
			this.lastFinalY = num3;
			UIHighlighter.HighlightOpportunity(rect, "Alerts");
			if (alert != null)
			{
				alert.DrawInfoPane();
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Alerts, KnowledgeAmount.FrameDisplayed);
				this.CheckAddOrRemoveAlert(alert, false);
			}
		}

		// Token: 0x0400B3FC RID: 46076
		private List<Alert> activeAlerts = new List<Alert>(16);

		// Token: 0x0400B3FD RID: 46077
		private int curAlertIndex;

		// Token: 0x0400B3FE RID: 46078
		private float lastFinalY;

		// Token: 0x0400B3FF RID: 46079
		private int mouseoverAlertIndex = -1;

		// Token: 0x0400B400 RID: 46080
		public static List<Type> allAlertTypesCached;

		// Token: 0x0400B401 RID: 46081
		private readonly List<Alert> AllAlerts = new List<Alert>();

		// Token: 0x0400B402 RID: 46082
		private const int StartTickDelay = 600;

		// Token: 0x0400B403 RID: 46083
		private const int AlertCycleLength = 24;

		// Token: 0x0400B404 RID: 46084
		private const int UpdateAlertsFromQuestsIntervalFrames = 20;

		// Token: 0x0400B405 RID: 46085
		private const int UpdateAlertsFromPreceptsIntervalFrames = 20;

		// Token: 0x0400B406 RID: 46086
		private const int UpdateAlertsFromScenarioIntervalFrames = 20;

		// Token: 0x0400B407 RID: 46087
		private const int UpdateAlertsFromSignalActionsIntervalFrames = 20;

		// Token: 0x0400B408 RID: 46088
		private readonly List<AlertPriority> PriosInDrawOrder;

		// Token: 0x0400B409 RID: 46089
		private Dictionary<Precept, List<Alert>> activePreceptAlerts = new Dictionary<Precept, List<Alert>>();

		// Token: 0x0400B40A RID: 46090
		private List<Alert> activeScenarioAlerts = new List<Alert>();

		// Token: 0x0400B40B RID: 46091
		private List<Alert> activeSignalActionAlerts = new List<Alert>();
	}
}
