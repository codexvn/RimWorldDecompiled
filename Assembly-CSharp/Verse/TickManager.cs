using System;
using System.Collections.Generic;
using System.Diagnostics;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200034E RID: 846
	public sealed class TickManager : IExposable
	{
		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x0600159D RID: 5533 RVA: 0x00066039 File Offset: 0x00064239
		public int TicksGame
		{
			get
			{
				return this.ticksGameInt;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x0600159E RID: 5534 RVA: 0x00066041 File Offset: 0x00064241
		public int TicksAbs
		{
			get
			{
				if (this.gameStartAbsTick == 0)
				{
					Log.ErrorOnce("Accessing TicksAbs but gameStartAbsTick is not set yet (you most likely want to use GenTicks.TicksAbs instead).", 1049580013);
					return this.ticksGameInt;
				}
				return this.ticksGameInt + this.gameStartAbsTick;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x0600159F RID: 5535 RVA: 0x0006606E File Offset: 0x0006426E
		public int TicksSinceSettle
		{
			get
			{
				return this.ticksGameInt - this.lastSettleTicksInt;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060015A0 RID: 5536 RVA: 0x0006607D File Offset: 0x0006427D
		public int SettleTick
		{
			get
			{
				return this.lastSettleTicksInt;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060015A1 RID: 5537 RVA: 0x00066085 File Offset: 0x00064285
		public int StartingYear
		{
			get
			{
				return this.startingYearInt;
			}
		}

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060015A2 RID: 5538 RVA: 0x00066090 File Offset: 0x00064290
		public float TickRateMultiplier
		{
			get
			{
				if (this.slower.ForcedNormalSpeed)
				{
					if (this.curTimeSpeed == TimeSpeed.Paused)
					{
						return 0f;
					}
					return 1f;
				}
				else
				{
					switch (this.curTimeSpeed)
					{
					case TimeSpeed.Paused:
						return 0f;
					case TimeSpeed.Normal:
						return 1f;
					case TimeSpeed.Fast:
						return 3f;
					case TimeSpeed.Superfast:
						if (Find.Maps.Count == 0)
						{
							return 18f;
						}
						if (this.NothingHappeningInGame())
						{
							return 12f;
						}
						return 6f;
					case TimeSpeed.Ultrafast:
						if (Find.Maps.Count == 0 || TickManager.UltraSpeedBoost)
						{
							return 150f;
						}
						return 15f;
					default:
						return -1f;
					}
				}
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060015A3 RID: 5539 RVA: 0x0006613D File Offset: 0x0006433D
		public int TicksThisFrame
		{
			get
			{
				return this.ticksThisFrame;
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060015A4 RID: 5540 RVA: 0x00066145 File Offset: 0x00064345
		private float CurTimePerTick
		{
			get
			{
				if (this.TickRateMultiplier == 0f)
				{
					return 0f;
				}
				return 1f / (60f * this.TickRateMultiplier);
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x060015A5 RID: 5541 RVA: 0x0006616C File Offset: 0x0006436C
		public bool Paused
		{
			get
			{
				return this.curTimeSpeed == TimeSpeed.Paused || this.ForcePaused;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060015A6 RID: 5542 RVA: 0x00066180 File Offset: 0x00064380
		public bool ForcePaused
		{
			get
			{
				if ((Find.WindowStack == null || !Find.WindowStack.WindowsForcePause) && !LongEventHandler.ForcePause && !Find.TilePicker.Active && !WorldComponent_GravshipController.CutsceneInProgress)
				{
					WorldComponent_GravshipController gravshipController = Find.GravshipController;
					if (gravshipController == null || !gravshipController.LandingAreaConfirmationInProgress)
					{
						return MapGenerator.debugMode;
					}
				}
				return true;
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x060015A7 RID: 5543 RVA: 0x000661D4 File Offset: 0x000643D4
		private AcceptanceReport PlayerCanControl
		{
			get
			{
				WorldComponent_GravshipController gravshipController = Find.GravshipController;
				if (gravshipController != null && gravshipController.LandingAreaConfirmationInProgress)
				{
					return "MessageConfirmLandingAreaFirst".Translate();
				}
				if (!Current.Game.PlayerHasControl)
				{
					return false;
				}
				return true;
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x060015A8 RID: 5544 RVA: 0x00066212 File Offset: 0x00064412
		public bool NotPlaying
		{
			get
			{
				return Find.MainTabsRoot.OpenTab == MainButtonDefOf.Menu;
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x060015A9 RID: 5545 RVA: 0x00066228 File Offset: 0x00064428
		// (set) Token: 0x060015AA RID: 5546 RVA: 0x00066230 File Offset: 0x00064430
		public TimeSpeed CurTimeSpeed
		{
			get
			{
				return this.curTimeSpeed;
			}
			set
			{
				if (!this.PlayerCanControl)
				{
					if (!this.PlayerCanControl.Reason.NullOrEmpty())
					{
						Messages.Message(this.PlayerCanControl.Reason, MessageTypeDefOf.RejectInput, false);
					}
					return;
				}
				this.curTimeSpeed = value;
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x060015AB RID: 5547 RVA: 0x00066280 File Offset: 0x00064480
		public bool HasSettledNewColony
		{
			get
			{
				return this.lastSettleTicksInt > 0;
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x060015AC RID: 5548 RVA: 0x0006628B File Offset: 0x0006448B
		public float MeanTickTime
		{
			get
			{
				return this.smoothedTickTimeAverage.GetAverage();
			}
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x00066298 File Offset: 0x00064498
		public void TogglePaused()
		{
			if (!this.PlayerCanControl)
			{
				if (!this.PlayerCanControl.Reason.NullOrEmpty())
				{
					Messages.Message(this.PlayerCanControl.Reason, MessageTypeDefOf.RejectInput, false);
				}
				return;
			}
			if (this.curTimeSpeed != TimeSpeed.Paused)
			{
				this.prePauseTimeSpeed = this.curTimeSpeed;
				this.curTimeSpeed = TimeSpeed.Paused;
				return;
			}
			if (this.prePauseTimeSpeed != this.curTimeSpeed)
			{
				this.curTimeSpeed = this.prePauseTimeSpeed;
				return;
			}
			this.curTimeSpeed = TimeSpeed.Normal;
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x0006631F File Offset: 0x0006451F
		public void Pause()
		{
			if (this.curTimeSpeed != TimeSpeed.Paused)
			{
				this.TogglePaused();
			}
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x00066330 File Offset: 0x00064530
		private bool NothingHappeningInGame()
		{
			if (this.lastNothingHappeningCheckTick != this.TicksGame)
			{
				this.nothingHappeningCached = true;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Pawn> list = maps[i].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
					for (int j = 0; j < list.Count; j++)
					{
						Pawn pawn = list[j];
						if (pawn.HostFaction == null && pawn.RaceProps.Humanlike && pawn.Awake() && !pawn.IsGhoul)
						{
							this.nothingHappeningCached = false;
							break;
						}
					}
					if (!this.nothingHappeningCached)
					{
						break;
					}
				}
				if (this.nothingHappeningCached)
				{
					for (int k = 0; k < maps.Count; k++)
					{
						if (maps[k].IsPlayerHome && maps[k].dangerWatcher.DangerRating >= StoryDanger.Low)
						{
							this.nothingHappeningCached = false;
							break;
						}
					}
				}
				this.lastNothingHappeningCheckTick = this.TicksGame;
			}
			return this.nothingHappeningCached;
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x00066434 File Offset: 0x00064634
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksGameInt, "ticksGame", 0, false);
			Scribe_Values.Look<int>(ref this.gameStartAbsTick, "gameStartAbsTick", 0, false);
			Scribe_Values.Look<int>(ref this.startingYearInt, "startingYear", 0, false);
			Scribe_Values.Look<int>(ref this.lastSettleTicksInt, "lastSettleTicks", 0, false);
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0006648C File Offset: 0x0006468C
		public void RegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.RegisterThing(t);
			}
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x000664AC File Offset: 0x000646AC
		public void DeRegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.DeregisterThing(t);
			}
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x000664CC File Offset: 0x000646CC
		private TickList TickListFor(Thing t)
		{
			if (t is IThingHolder)
			{
				return this.tickListNormal;
			}
			TickList result;
			switch (t.def.tickerType)
			{
			case TickerType.Never:
				result = null;
				break;
			case TickerType.Normal:
				result = this.tickListNormal;
				break;
			case TickerType.Rare:
				result = this.tickListRare;
				break;
			case TickerType.Long:
				result = this.tickListLong;
				break;
			default:
				throw new InvalidOperationException();
			}
			return result;
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x00066534 File Offset: 0x00064734
		public void TickManagerUpdate()
		{
			this.ticksThisFrame = 0;
			if (!this.Paused)
			{
				float curTimePerTick = this.CurTimePerTick;
				if (Mathf.Abs(Time.deltaTime - curTimePerTick) < curTimePerTick * 0.1f)
				{
					this.realTimeToTickThrough += curTimePerTick;
				}
				else
				{
					this.realTimeToTickThrough += Time.deltaTime;
				}
				float tickRateMultiplier = this.TickRateMultiplier;
				this.clock.Reset();
				this.clock.Start();
				while (this.realTimeToTickThrough > 0f && (float)this.ticksThisFrame < tickRateMultiplier * 2f)
				{
					double totalMilliseconds = this.clock.Elapsed.TotalMilliseconds;
					this.DoSingleTick();
					double totalMilliseconds2 = this.clock.Elapsed.TotalMilliseconds;
					this.realTimeToTickThrough -= curTimePerTick;
					this.ticksThisFrame++;
					this.rawTickTimeAverage.AddValue((float)(totalMilliseconds2 - totalMilliseconds));
					this.smoothedTickTimeAverage.AddValue(this.rawTickTimeAverage.GetAverage());
					if (this.Paused || (float)this.clock.ElapsedMilliseconds > 45.454544f)
					{
						break;
					}
				}
				if (this.realTimeToTickThrough > 0f)
				{
					this.realTimeToTickThrough = 0f;
				}
			}
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x00066678 File Offset: 0x00064878
		public void DoSingleTick()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].MapPreTick();
			}
			if (!DebugSettings.fastEcology)
			{
				this.ticksGameInt++;
			}
			else
			{
				this.ticksGameInt += 2000;
			}
			Shader.SetGlobalFloat(ShaderPropertyIDs.GameSeconds, this.TicksGame.TicksToSeconds());
			this.tickListNormal.Tick();
			this.tickListRare.Tick();
			this.tickListLong.Tick();
			try
			{
				Find.DateNotifier.DateNotifierTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
			}
			try
			{
				Find.Scenario.TickScenario();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			try
			{
				Find.World.WorldTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString());
			}
			try
			{
				Find.StoryWatcher.StoryWatcherTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString());
			}
			try
			{
				Find.GameEnder.GameEndTick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString());
			}
			try
			{
				Find.Storyteller.StorytellerTick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString());
			}
			try
			{
				Find.TaleManager.TaleManagerTick();
			}
			catch (Exception ex7)
			{
				Log.Error(ex7.ToString());
			}
			try
			{
				Find.QuestManager.QuestManagerTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString());
			}
			try
			{
				Find.World.WorldPostTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString());
			}
			for (int j = 0; j < maps.Count; j++)
			{
				maps[j].MapPostTick();
			}
			try
			{
				Find.History.HistoryTick();
			}
			catch (Exception ex10)
			{
				Log.Error(ex10.ToString());
			}
			GameComponentUtility.GameComponentTick();
			try
			{
				Find.LetterStack.LetterStackTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString());
			}
			try
			{
				Find.Autosaver.AutosaverTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString());
			}
			if (DebugViewSettings.logHourlyScreenshot && Find.TickManager.TicksGame >= this.lastAutoScreenshot + 2500)
			{
				ScreenshotTaker.QueueSilentScreenshot();
				this.lastAutoScreenshot = Find.TickManager.TicksGame / 2500 * 2500;
			}
			try
			{
				FilthMonitor.FilthMonitorTick();
			}
			catch (Exception ex13)
			{
				Log.Error(ex13.ToString());
			}
			try
			{
				Find.TransportShipManager.ShipObjectsTick();
			}
			catch (Exception ex14)
			{
				Log.Error(ex14.ToString());
			}
			UnityEngine.Debug.developerConsoleVisible = false;
		}

		// Token: 0x060015B6 RID: 5558 RVA: 0x00066974 File Offset: 0x00064B74
		public void RemoveAllFromMap(Map map)
		{
			this.tickListNormal.RemoveWhere((Thing x) => x.Map == map);
			this.tickListRare.RemoveWhere((Thing x) => x.Map == map);
			this.tickListLong.RemoveWhere((Thing x) => x.Map == map);
		}

		// Token: 0x060015B7 RID: 5559 RVA: 0x000669D3 File Offset: 0x00064BD3
		public void DebugSetTicksGame(int newTicksGame)
		{
			this.ticksGameInt = newTicksGame;
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x000669DC File Offset: 0x00064BDC
		public void Notify_GeneratedPotentiallyHostileMap()
		{
			this.Pause();
			this.slower.SignalForceNormalSpeedShort();
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x000669EF File Offset: 0x00064BEF
		public void ResetSettlementTicks()
		{
			this.lastSettleTicksInt = this.TicksGame;
		}

		// Token: 0x0400140C RID: 5132
		[TweakValue("Gameplay", 0f, 100f)]
		private static bool UltraSpeedBoost;

		// Token: 0x0400140D RID: 5133
		private int ticksGameInt;

		// Token: 0x0400140E RID: 5134
		public int gameStartAbsTick;

		// Token: 0x0400140F RID: 5135
		private float realTimeToTickThrough;

		// Token: 0x04001410 RID: 5136
		private TimeSpeed curTimeSpeed = TimeSpeed.Normal;

		// Token: 0x04001411 RID: 5137
		public TimeSpeed prePauseTimeSpeed;

		// Token: 0x04001412 RID: 5138
		private int startingYearInt = 5500;

		// Token: 0x04001413 RID: 5139
		private Stopwatch clock = new Stopwatch();

		// Token: 0x04001414 RID: 5140
		private int lastSettleTicksInt;

		// Token: 0x04001415 RID: 5141
		private TickList tickListNormal = new TickList(TickerType.Normal);

		// Token: 0x04001416 RID: 5142
		private TickList tickListRare = new TickList(TickerType.Rare);

		// Token: 0x04001417 RID: 5143
		private TickList tickListLong = new TickList(TickerType.Long);

		// Token: 0x04001418 RID: 5144
		public TimeSlower slower = new TimeSlower();

		// Token: 0x04001419 RID: 5145
		private int lastAutoScreenshot;

		// Token: 0x0400141A RID: 5146
		private int ticksThisFrame;

		// Token: 0x0400141B RID: 5147
		private SimpleMovingAverage rawTickTimeAverage = new SimpleMovingAverage(720);

		// Token: 0x0400141C RID: 5148
		private ExponentialMovingAverage smoothedTickTimeAverage = new ExponentialMovingAverage(0.1f);

		// Token: 0x0400141D RID: 5149
		public const float WorstAllowedFPS = 22f;

		// Token: 0x0400141E RID: 5150
		private int lastNothingHappeningCheckTick = -1;

		// Token: 0x0400141F RID: 5151
		private bool nothingHappeningCached;
	}
}
