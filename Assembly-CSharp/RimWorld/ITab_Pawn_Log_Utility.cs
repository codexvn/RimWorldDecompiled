using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033BA RID: 13242
	public static class ITab_Pawn_Log_Utility
	{
		// Token: 0x060129C7 RID: 76231 RVA: 0x00581FA8 File Offset: 0x005801A8
		public static List<ITab_Pawn_Log_Utility.LogLineDisplayable> GenerateLogLinesFor(Pawn pawn, bool showAll, bool showCombat, bool showSocial, int maxLines)
		{
			ITab_Pawn_Log_Utility.tmpLogLines.Clear();
			LogEntry[] array = showSocial ? (from e in Find.PlayLog.AllEntries
			where e.Concerns(pawn)
			select e).ToArray<LogEntry>() : new LogEntry[0];
			int num = 0;
			int num2 = 0;
			Battle battle = null;
			if (!showCombat)
			{
				goto IL_219;
			}
			bool flag = true;
			using (List<Battle>.Enumerator enumerator = Find.BattleLog.Battles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Battle battle2 = enumerator.Current;
					if (battle2.Concerns(pawn))
					{
						foreach (LogEntry logEntry in battle2.Entries)
						{
							if (logEntry.Concerns(pawn))
							{
								if (showAll || logEntry.ShowInCompactView())
								{
									while (num < array.Length && array[num].Age < logEntry.Age)
									{
										if (battle != null && battle != battle2)
										{
											ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding));
											battle = null;
										}
										ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableLog(array[num++], pawn));
										num2++;
										if (num2 >= maxLines)
										{
											return ITab_Pawn_Log_Utility.tmpLogLines;
										}
										flag = false;
									}
									if (battle != battle2)
									{
										if (!flag)
										{
											ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding));
										}
										ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableHeader(battle2.GetName()));
										battle = battle2;
										flag = false;
									}
									ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableLog(logEntry, pawn));
									num2++;
									if (num2 >= maxLines)
									{
										return ITab_Pawn_Log_Utility.tmpLogLines;
									}
								}
							}
						}
					}
				}
				goto IL_219;
			}
			IL_1D3:
			if (battle != null)
			{
				ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableGap(ITab_Pawn_Log_Utility.BattleBottomPadding));
				battle = null;
			}
			ITab_Pawn_Log_Utility.tmpLogLines.Add(new ITab_Pawn_Log_Utility.LogLineDisplayableLog(array[num++], pawn));
			num2++;
			if (num2 >= maxLines)
			{
				return ITab_Pawn_Log_Utility.tmpLogLines;
			}
			IL_219:
			if (num >= array.Length)
			{
				return ITab_Pawn_Log_Utility.tmpLogLines;
			}
			goto IL_1D3;
		}

		// Token: 0x0400C156 RID: 49494
		[TweakValue("Interface", 0f, 1f)]
		private static float AlternateAlpha = 0.03f;

		// Token: 0x0400C157 RID: 49495
		[TweakValue("Interface", 0f, 1f)]
		private static float HighlightAlpha = 0.2f;

		// Token: 0x0400C158 RID: 49496
		[TweakValue("Interface", 0f, 10f)]
		private static float HighlightDuration = 4f;

		// Token: 0x0400C159 RID: 49497
		[TweakValue("Interface", 0f, 30f)]
		private static float BattleBottomPadding = 20f;

		// Token: 0x0400C15A RID: 49498
		private static List<ITab_Pawn_Log_Utility.LogLineDisplayable> tmpLogLines = new List<ITab_Pawn_Log_Utility.LogLineDisplayable>();

		// Token: 0x020033BB RID: 13243
		public class LogDrawData
		{
			// Token: 0x060129C9 RID: 76233 RVA: 0x00582244 File Offset: 0x00580444
			public void StartNewDraw()
			{
				this.alternatingBackground = false;
			}

			// Token: 0x0400C15B RID: 49499
			public bool alternatingBackground;

			// Token: 0x0400C15C RID: 49500
			public LogEntry highlightEntry;

			// Token: 0x0400C15D RID: 49501
			public float highlightIntensity;
		}

		// Token: 0x020033BC RID: 13244
		public abstract class LogLineDisplayable
		{
			// Token: 0x060129CB RID: 76235 RVA: 0x0058224D File Offset: 0x0058044D
			public float GetHeight(float width)
			{
				if (this.cachedHeight == -1f)
				{
					this.cachedHeight = this.GetHeight_Worker(width);
				}
				return this.cachedHeight;
			}

			// Token: 0x060129CC RID: 76236
			public abstract float GetHeight_Worker(float width);

			// Token: 0x060129CD RID: 76237
			public abstract void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data);

			// Token: 0x060129CE RID: 76238
			public abstract void AppendTo(StringBuilder sb);

			// Token: 0x060129CF RID: 76239 RVA: 0x00002501 File Offset: 0x00000701
			public virtual bool Matches(LogEntry log)
			{
				return false;
			}

			// Token: 0x0400C15E RID: 49502
			private float cachedHeight = -1f;
		}

		// Token: 0x020033BD RID: 13245
		public class LogLineDisplayableHeader : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x060129D1 RID: 76241 RVA: 0x00582282 File Offset: 0x00580482
			public LogLineDisplayableHeader(TaggedString text)
			{
				this.text = text;
			}

			// Token: 0x060129D2 RID: 76242 RVA: 0x00582294 File Offset: 0x00580494
			public override float GetHeight_Worker(float width)
			{
				GameFont font = Text.Font;
				Text.Font = GameFont.Medium;
				float result = Text.CalcHeight(this.text, width);
				Text.Font = font;
				return result;
			}

			// Token: 0x060129D3 RID: 76243 RVA: 0x005822C4 File Offset: 0x005804C4
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
				Text.Font = GameFont.Medium;
				Widgets.Label(new Rect(0f, position, width, base.GetHeight(width)), this.text);
				Text.Font = GameFont.Small;
			}

			// Token: 0x060129D4 RID: 76244 RVA: 0x005822F0 File Offset: 0x005804F0
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine("--    " + this.text);
			}

			// Token: 0x0400C15F RID: 49503
			private TaggedString text;
		}

		// Token: 0x020033BE RID: 13246
		public class LogLineDisplayableLog : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x060129D5 RID: 76245 RVA: 0x0058230E File Offset: 0x0058050E
			public LogLineDisplayableLog(LogEntry log, Pawn pawn)
			{
				this.log = log;
				this.pawn = pawn;
			}

			// Token: 0x060129D6 RID: 76246 RVA: 0x00582324 File Offset: 0x00580524
			public override float GetHeight_Worker(float width)
			{
				float width2 = width - 29f;
				return Mathf.Max(26f, this.log.GetTextHeight(this.pawn, width2) + 4f);
			}

			// Token: 0x060129D7 RID: 76247 RVA: 0x0058235C File Offset: 0x0058055C
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
				float height = base.GetHeight(width);
				float width2 = width - 29f;
				Rect rect = new Rect(0f, position, width, height);
				if (this.log == data.highlightEntry)
				{
					Widgets.DrawRectFast(rect, new Color(1f, 1f, 1f, ITab_Pawn_Log_Utility.HighlightAlpha * data.highlightIntensity), null);
					data.highlightIntensity = Mathf.Max(0f, data.highlightIntensity - Time.deltaTime / ITab_Pawn_Log_Utility.HighlightDuration);
				}
				else if (data.alternatingBackground)
				{
					Widgets.DrawRectFast(rect, new Color(1f, 1f, 1f, ITab_Pawn_Log_Utility.AlternateAlpha), null);
				}
				data.alternatingBackground = !data.alternatingBackground;
				TaggedString label = this.log.ToGameStringFromPOV(this.pawn, false);
				Widgets.Label(new Rect(29f, position, width2, height), label);
				Texture2D texture2D = this.log.IconFromPOV(this.pawn);
				if (texture2D != null)
				{
					GUI.color = (this.log.IconColorFromPOV(this.pawn) ?? Color.white);
					GUI.DrawTexture(new Rect(0f, position + (height - 26f) / 2f, 26f, 26f), texture2D);
					GUI.color = Color.white;
				}
				if (Mouse.IsOver(rect))
				{
					TooltipHandler.TipRegion(rect, () => this.log.GetTipString(), 613261 + this.log.LogID * 2063);
					Widgets.DrawHighlight(rect);
				}
				if (Widgets.ButtonInvisible(rect, this.log.CanBeClickedFromPOV(this.pawn)))
				{
					this.log.ClickedFromPOV(this.pawn);
				}
				if (DebugViewSettings.logCombatLogMouseover && Mouse.IsOver(rect))
				{
					this.log.ToGameStringFromPOV(this.pawn, true);
				}
			}

			// Token: 0x060129D8 RID: 76248 RVA: 0x00582546 File Offset: 0x00580746
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine(this.log.ToGameStringFromPOV(this.pawn, false));
			}

			// Token: 0x060129D9 RID: 76249 RVA: 0x00582561 File Offset: 0x00580761
			public override bool Matches(LogEntry log)
			{
				return log == this.log;
			}

			// Token: 0x0400C160 RID: 49504
			private LogEntry log;

			// Token: 0x0400C161 RID: 49505
			private Pawn pawn;
		}

		// Token: 0x020033BF RID: 13247
		public class LogLineDisplayableGap : ITab_Pawn_Log_Utility.LogLineDisplayable
		{
			// Token: 0x060129DB RID: 76251 RVA: 0x00582579 File Offset: 0x00580779
			public LogLineDisplayableGap(float height)
			{
				this.height = height;
			}

			// Token: 0x060129DC RID: 76252 RVA: 0x00582588 File Offset: 0x00580788
			public override float GetHeight_Worker(float width)
			{
				return this.height;
			}

			// Token: 0x060129DD RID: 76253 RVA: 0x000026EA File Offset: 0x000008EA
			public override void Draw(float position, float width, ITab_Pawn_Log_Utility.LogDrawData data)
			{
			}

			// Token: 0x060129DE RID: 76254 RVA: 0x00582590 File Offset: 0x00580790
			public override void AppendTo(StringBuilder sb)
			{
				sb.AppendLine();
			}

			// Token: 0x0400C162 RID: 49506
			private float height;
		}
	}
}
