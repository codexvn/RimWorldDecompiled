using System;
using System.IO;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Analytics;
using Verse.AI;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000357 RID: 855
	public abstract class Root : MonoBehaviour
	{
		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060015E5 RID: 5605 RVA: 0x00066D1E File Offset: 0x00064F1E
		public static float AverageFrameTime
		{
			get
			{
				return Root.smoothedAverageDeltaTime.GetAverage();
			}
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x00066D2C File Offset: 0x00064F2C
		public virtual void Start()
		{
			try
			{
				CultureInfoUtility.EnsureEnglish();
				Current.Notify_LoadedSceneChanged();
				GlobalTextureAtlasManager.FreeAllRuntimeAtlases();
				Root.CheckGlobalInit();
				Action action = delegate()
				{
					DeepProfiler.Start("Misc Init (InitializingInterface)");
					try
					{
						this.soundRoot = new SoundRoot();
						DeepProfiler.Start("Instantiate UIRoot");
						if (GenScene.InPlayScene)
						{
							this.uiRoot = new UIRoot_Play();
						}
						else if (GenScene.InEntryScene)
						{
							this.uiRoot = new UIRoot_Entry();
						}
						DeepProfiler.End();
						this.uiRoot.Init();
						Messages.Notify_LoadedLevelChanged();
						if (Current.SubcameraDriver != null)
						{
							Current.SubcameraDriver.Init();
						}
					}
					finally
					{
						DeepProfiler.End();
					}
				};
				if (!PlayDataLoader.Loaded)
				{
					Application.runInBackground = true;
					LongEventHandler.QueueLongEvent(delegate()
					{
						PlayDataLoader.LoadAllPlayData(false);
					}, null, true, null, true, false, null);
					LongEventHandler.QueueLongEvent(action, "InitializingInterface", false, null, true, false, null);
				}
				else
				{
					action();
				}
			}
			catch (Exception ex)
			{
				string str = "Critical error in root Start(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x00066DDC File Offset: 0x00064FDC
		private static void CheckGlobalInit()
		{
			if (Root.globalInitDone)
			{
				return;
			}
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (commandLineArgs != null && commandLineArgs.Length > 1)
			{
				Log.Message("Command line arguments: " + GenText.ToSpaceList(commandLineArgs.Skip(1)));
			}
			PerformanceReporting.enabled = false;
			Application.targetFrameRate = Mathf.RoundToInt((float)Screen.currentResolution.refreshRateRatio.value);
			UnityDataInitializer.CopyUnityData();
			SteamManager.InitIfNeeded();
			VersionControl.LogVersionNumber();
			Prefs.Init();
			Application.logMessageReceivedThreaded += Log.Notify_MessageReceivedThreadedInternal;
			if (Prefs.DevMode)
			{
				StaticConstructorOnStartupUtility.ReportProbablyMissingAttributes();
			}
			LongEventHandler.QueueLongEvent(new Action(StaticConstructorOnStartupUtility.CallAll), null, false, null, true, false, null);
			Root.globalInitDone = true;
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x00066E90 File Offset: 0x00065090
		public virtual void Update()
		{
			try
			{
				ResolutionUtility.Update();
				RealTime.Update();
				Root.rawAverageDeltaTime.AddValue(RealTime.deltaTime * 1000f);
				Root.smoothedAverageDeltaTime.AddValue(Root.rawAverageDeltaTime.GetAverage());
				bool flag;
				LongEventHandler.LongEventsUpdate(out flag);
				if (flag)
				{
					this.destroyed = true;
				}
				else if (!LongEventHandler.ShouldWaitForEvent)
				{
					Rand.EnsureStateStackEmpty();
					Widgets.EnsureMousePositionStackEmpty();
					SteamManager.Update();
					PortraitsCache.PortraitsCacheUpdate();
					AttackTargetsCache.AttackTargetsCacheStaticUpdate();
					Pawn_MeleeVerbs.PawnMeleeVerbsStaticUpdate();
					Storyteller.StorytellerStaticUpdate();
					CaravanInventoryUtility.CaravanInventoryUtilityStaticUpdate();
					this.uiRoot.UIRootUpdate();
					if (Time.frameCount > 3 && !Root.prefsApplied)
					{
						Root.prefsApplied = true;
						Prefs.Apply();
					}
					this.soundRoot.Update();
				}
			}
			catch (Exception ex)
			{
				string str = "Root level exception in Update(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x00066F78 File Offset: 0x00065178
		public void OnGUI()
		{
			try
			{
				if (!this.destroyed)
				{
					GUI.depth = 50;
					UI.ApplyUIScale();
					LongEventHandler.LongEventsOnGUI();
					if (LongEventHandler.ShouldWaitForEvent)
					{
						ScreenFader.OverlayOnGUI(new Vector2((float)UI.screenWidth, (float)UI.screenHeight));
					}
					else
					{
						this.uiRoot.UIRootOnGUI();
						ScreenFader.OverlayOnGUI(new Vector2((float)UI.screenWidth, (float)UI.screenHeight));
						if (Find.CameraDriver != null && Find.CameraDriver.isActiveAndEnabled)
						{
							Find.CameraDriver.CameraDriverOnGUI();
						}
						if (Find.WorldCameraDriver != null && Find.WorldCameraDriver.isActiveAndEnabled)
						{
							Find.WorldCameraDriver.WorldCameraDriverOnGUI();
						}
					}
				}
			}
			catch (Exception ex)
			{
				string str = "Root level exception in OnGUI(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060015EA RID: 5610 RVA: 0x00067060 File Offset: 0x00065260
		public static void Shutdown()
		{
			try
			{
				SteamManager.ShutdownSteam();
			}
			catch (Exception ex)
			{
				string str = "Error in ShutdownSteam(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.TempFolderPath);
				FileInfo[] files = directoryInfo.GetFiles();
				for (int i = 0; i < files.Length; i++)
				{
					files[i].Delete();
				}
				DirectoryInfo[] directories = directoryInfo.GetDirectories();
				for (int i = 0; i < directories.Length; i++)
				{
					directories[i].Delete(true);
				}
			}
			catch (Exception ex3)
			{
				string str2 = "Could not delete temporary files: ";
				Exception ex4 = ex3;
				Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
			}
			Application.Quit();
		}

		// Token: 0x04001437 RID: 5175
		private static bool globalInitDone;

		// Token: 0x04001438 RID: 5176
		private static bool prefsApplied;

		// Token: 0x04001439 RID: 5177
		protected static bool checkedAutostartSaveFile;

		// Token: 0x0400143A RID: 5178
		protected bool destroyed;

		// Token: 0x0400143B RID: 5179
		private static readonly SimpleMovingAverage rawAverageDeltaTime = new SimpleMovingAverage(200);

		// Token: 0x0400143C RID: 5180
		private static readonly ExponentialMovingAverage smoothedAverageDeltaTime = new ExponentialMovingAverage(0.1f);

		// Token: 0x0400143D RID: 5181
		public SoundRoot soundRoot;

		// Token: 0x0400143E RID: 5182
		public UIRoot uiRoot;
	}
}
