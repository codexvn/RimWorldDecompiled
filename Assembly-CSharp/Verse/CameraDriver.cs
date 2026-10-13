using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x020001D3 RID: 467
	public class CameraDriver : MonoBehaviour
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x0003BECB File Offset: 0x0003A0CB
		private static float ScrollWheelZoomRate
		{
			get
			{
				if (!SteamDeck.IsSteamDeck)
				{
					return 0.35f;
				}
				return 0.55f;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x0003BEDF File Offset: 0x0003A0DF
		private Camera MyCamera
		{
			get
			{
				if (this.cachedCamera == null)
				{
					this.cachedCamera = base.GetComponent<Camera>();
				}
				return this.cachedCamera;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x0003BF01 File Offset: 0x0003A101
		private float ScreenDollyEdgeWidthBottom
		{
			get
			{
				if (Screen.fullScreen || ResolutionUtility.BorderlessFullscreen)
				{
					return 6f;
				}
				return 20f;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0003BF1C File Offset: 0x0003A11C
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x0003BF24 File Offset: 0x0003A124
		public float RootSize
		{
			get
			{
				return this.rootSize;
			}
			private set
			{
				if (this.rootSize == value)
				{
					return;
				}
				if (Current.ProgramState != ProgramState.Playing || LongEventHandler.ShouldWaitForEvent || Find.Camera == null || !Prefs.ZoomToMouse)
				{
					this.rootSize = value;
					return;
				}
				this.ApplyPositionToGameObject();
				Vector3 a = UI.MouseMapPosition();
				this.rootSize = value;
				this.ApplyPositionToGameObject();
				this.rootPos += a - UI.MouseMapPosition();
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x0003BF9C File Offset: 0x0003A19C
		public CameraZoomRange CurrentZoom
		{
			get
			{
				if (this.RootSize < this.config.sizeRange.min + 1f)
				{
					return CameraZoomRange.Closest;
				}
				if (this.RootSize < this.config.sizeRange.max * 0.23f)
				{
					return CameraZoomRange.Close;
				}
				if (this.RootSize < this.config.sizeRange.max * 0.7f)
				{
					return CameraZoomRange.Middle;
				}
				if (this.RootSize < this.config.sizeRange.max * 0.95f)
				{
					return CameraZoomRange.Far;
				}
				return CameraZoomRange.Furthest;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x0003C02A File Offset: 0x0003A22A
		private Vector3 CurrentRealPosition
		{
			get
			{
				return this.MyCamera.transform.position;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x0003C03C File Offset: 0x0003A23C
		private bool AnythingPreventsCameraMotion
		{
			get
			{
				return Find.WindowStack.WindowsPreventCameraMotion || WorldRendererUtility.WorldSelected || !Current.Game.PlayerHasControl;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x0003C060 File Offset: 0x0003A260
		public IntVec3 MapPosition
		{
			get
			{
				IntVec3 result = this.CurrentRealPosition.ToIntVec3();
				result.y = 0;
				return result;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x0003C084 File Offset: 0x0003A284
		public Vector2 ViewSpacePosition
		{
			get
			{
				Vector3 currentRealPosition = this.CurrentRealPosition;
				float x = Mathf.InverseLerp(2f, (float)Find.CurrentMap.Size.x + -2f, currentRealPosition.x);
				float y = Mathf.InverseLerp(2f, (float)Find.CurrentMap.Size.z + -2f, currentRealPosition.z);
				return new Vector2(x, y);
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0003C0EC File Offset: 0x0003A2EC
		public CellRect CurrentViewRect
		{
			get
			{
				if (Time.frameCount != CameraDriver.lastViewRectGetFrame)
				{
					CameraDriver.lastViewRect = default(CellRect);
					float num = (float)UI.screenWidth / (float)UI.screenHeight;
					Vector3 currentRealPosition = this.CurrentRealPosition;
					CameraDriver.lastViewRect.minX = Mathf.FloorToInt(currentRealPosition.x - this.RootSize * num - 1f);
					CameraDriver.lastViewRect.maxX = Mathf.CeilToInt(currentRealPosition.x + this.RootSize * num);
					CameraDriver.lastViewRect.minZ = Mathf.FloorToInt(currentRealPosition.z - this.RootSize - 1f);
					CameraDriver.lastViewRect.maxZ = Mathf.CeilToInt(currentRealPosition.z + this.RootSize);
					CameraDriver.lastViewRectGetFrame = Time.frameCount;
				}
				return CameraDriver.lastViewRect;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x0003C1B8 File Offset: 0x0003A3B8
		public static float HitchReduceFactor
		{
			get
			{
				float result = 1f;
				if (Time.deltaTime > 0.1f)
				{
					result = 0.1f / Time.deltaTime;
				}
				return result;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0003C1E4 File Offset: 0x0003A3E4
		public float CellSizePixels
		{
			get
			{
				return (float)UI.screenHeight / (this.RootSize * 2f);
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000CD1 RID: 3281 RVA: 0x0003C1F9 File Offset: 0x0003A3F9
		public float ZoomRootSize
		{
			get
			{
				return this.RootSize;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x0003C201 File Offset: 0x0003A401
		public Vector3 InverseFovScale
		{
			get
			{
				float num = 1f - (float)UI.screenHeight / (4f * this.RootSize * this.RootSize);
				return new Vector3(num, num, num);
			}
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0003C22C File Offset: 0x0003A42C
		public bool InViewOf(Thing thing)
		{
			CellRect cellRect = this.CurrentViewRect.ExpandedBy(1);
			cellRect.ClipInsideMap(thing.MapHeld);
			return cellRect.Overlaps(thing.OccupiedDrawRect());
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0003C264 File Offset: 0x0003A464
		public void Awake()
		{
			this.ResetSize();
			this.reverbDummy = GameObject.Find("ReverbZoneDummy");
			this.ApplyPositionToGameObject();
			this.MyCamera.nearClipPlane = 0.5f;
			this.MyCamera.farClipPlane = 65.5f;
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0003C2A2 File Offset: 0x0003A4A2
		public void OnPreCull()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (WorldRendererUtility.DrawingMap)
			{
				Find.CurrentMap.weatherManager.DrawAllWeather();
			}
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x0003C2CC File Offset: 0x0003A4CC
		public void CameraDriverOnGUI()
		{
			if (Find.CurrentMap == null)
			{
				return;
			}
			if (Input.GetMouseButtonUp(0) && Input.GetMouseButton(2))
			{
				this.releasedLeftWhileHoldingMiddle = true;
			}
			else if (Event.current.rawType == EventType.MouseDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2))
			{
				this.releasedLeftWhileHoldingMiddle = false;
			}
			this.mouseCoveredByUI = (Find.WindowStack.GetWindowAt(UI.MousePositionOnUIInverted) != null);
			if (!this.AnythingPreventsCameraMotion)
			{
				if (UnityGUIBugsFixer.MouseDrag(2) && (!SteamDeck.IsSteamDeck || !Find.Selector.AnyPawnSelected))
				{
					Vector2 currentEventDelta = UnityGUIBugsFixer.CurrentEventDelta;
					if (Event.current.type == EventType.MouseDrag)
					{
						Event.current.Use();
					}
					if (currentEventDelta != Vector2.zero)
					{
						currentEventDelta.x *= -1f;
						this.desiredDollyRaw += currentEventDelta / UI.CurUICellSize() * Prefs.MapDragSensitivity;
						this.panner.JumpOnNextUpdate();
					}
				}
				float num = 0f;
				if (Event.current.type == EventType.ScrollWheel)
				{
					num -= Event.current.delta.y * CameraDriver.ScrollWheelZoomRate;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.TinyInteraction);
				}
				if (KeyBindingDefOf.MapZoom_In.KeyDownEvent)
				{
					num += 4f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.SmallInteraction);
				}
				if (KeyBindingDefOf.MapZoom_Out.KeyDownEvent)
				{
					num -= 4f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraZoom, KnowledgeAmount.SmallInteraction);
				}
				this.desiredSize -= num * this.config.zoomSpeed * this.RootSize / 35f;
				this.desiredSize = Mathf.Clamp(this.desiredSize, this.config.sizeRange.min, this.config.sizeRange.max);
				this.desiredDolly = Vector2.zero;
				if (KeyBindingDefOf.MapDolly_Left.IsDown)
				{
					this.desiredDolly.x = -this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Right.IsDown)
				{
					this.desiredDolly.x = this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Up.IsDown)
				{
					this.desiredDolly.y = this.config.dollyRateKeys;
				}
				if (KeyBindingDefOf.MapDolly_Down.IsDown)
				{
					this.desiredDolly.y = -this.config.dollyRateKeys;
				}
				if (this.desiredDolly != Vector2.zero)
				{
					this.panner.JumpOnNextUpdate();
				}
				this.config.ConfigOnGUI();
			}
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x0003C550 File Offset: 0x0003A750
		public void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				if (Current.SubcameraDriver != null)
				{
					Current.SubcameraDriver.UpdatePositions(this.MyCamera);
				}
				return;
			}
			if (Find.CurrentMap == null)
			{
				return;
			}
			Vector2 vector = this.CalculateCurInputDollyVect();
			if (vector != Vector2.zero)
			{
				float d = (this.RootSize - this.config.sizeRange.min) / (this.config.sizeRange.max - this.config.sizeRange.min) * 0.7f + 0.3f;
				this.velocity = new Vector3(vector.x, 0f, vector.y) * d;
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CameraDolly, KnowledgeAmount.FrameInteraction);
			}
			if ((Input.GetMouseButtonUp(2) || (SteamDeck.IsSteamDeck && this.releasedLeftWhileHoldingMiddle)) && this.dragTimeStamps.Any<CameraDriver.DragTimeStamp>())
			{
				Vector2 extraVelocityFromReleasingDragButton = CameraDriver.GetExtraVelocityFromReleasingDragButton(this.dragTimeStamps, 0.75f);
				this.velocity += new Vector3(extraVelocityFromReleasingDragButton.x, 0f, extraVelocityFromReleasingDragButton.y);
				this.dragTimeStamps.Clear();
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				float num = Time.deltaTime * CameraDriver.HitchReduceFactor;
				this.rootPos += this.velocity * (num * this.config.moveSpeedScale);
				this.rootPos += new Vector3(this.desiredDollyRaw.x, 0f, this.desiredDollyRaw.y);
				this.dragTimeStamps.Add(new CameraDriver.DragTimeStamp
				{
					posDelta = this.desiredDollyRaw,
					time = Time.time
				});
				this.rootPos.x = Mathf.Clamp(this.rootPos.x, 2f, (float)Find.CurrentMap.Size.x + -2f);
				this.rootPos.z = Mathf.Clamp(this.rootPos.z, 2f, (float)Find.CurrentMap.Size.z + -2f);
			}
			this.desiredDollyRaw = Vector2.zero;
			int num2 = Gen.FixedTimeStepUpdate(ref this.fixedTimeStepBuffer, 60f);
			for (int i = 0; i < num2; i++)
			{
				if (this.velocity != Vector3.zero)
				{
					this.velocity *= this.config.camSpeedDecayFactor;
					if (this.velocity.magnitude < 0.1f)
					{
						this.velocity = Vector3.zero;
					}
				}
				if (this.config.smoothZoom)
				{
					float num3 = Mathf.Lerp(this.RootSize, this.desiredSize, 0.05f);
					this.desiredSize += (num3 - this.RootSize) * this.config.zoomPreserveFactor;
					this.RootSize = num3;
				}
				else
				{
					float num4 = (this.desiredSize - this.RootSize) * 0.4f;
					this.desiredSize += this.config.zoomPreserveFactor * num4;
					this.RootSize += num4;
				}
				this.config.ConfigFixedUpdate_60(ref this.rootPos, ref this.velocity);
			}
			CameraPanner.Interpolant? interpolant = this.panner.Update();
			if (interpolant != null)
			{
				CameraPanner.Interpolant valueOrDefault = interpolant.GetValueOrDefault();
				this.rootPos = valueOrDefault.Position;
				this.RootSize = valueOrDefault.Size;
				this.desiredSize = this.RootSize;
			}
			this.shaker.Update();
			this.ApplyPositionToGameObject();
			Current.SubcameraDriver.UpdatePositions(this.MyCamera);
			if (Find.CurrentMap != null)
			{
				RememberedCameraPos rememberedCameraPos = Find.CurrentMap.rememberedCameraPos;
				rememberedCameraPos.rootPos = this.rootPos;
				rememberedCameraPos.rootSize = this.RootSize;
			}
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x0003C93C File Offset: 0x0003AB3C
		private void ApplyPositionToGameObject()
		{
			if (base.gameObject == null)
			{
				return;
			}
			this.rootPos.y = 15f + (this.RootSize - this.config.sizeRange.min) / (this.config.sizeRange.max - this.config.sizeRange.min) * 50f;
			this.MyCamera.orthographicSize = this.RootSize;
			this.MyCamera.transform.position = this.rootPos + this.shaker.ShakeOffset;
			if (this.reverbDummy != null)
			{
				Vector3 position = base.transform.position;
				position.y = 65f;
				this.reverbDummy.transform.position = position;
			}
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x0003CA18 File Offset: 0x0003AC18
		private Vector2 CalculateCurInputDollyVect()
		{
			Vector2 vector = this.desiredDolly;
			bool flag = false;
			if ((UnityData.isEditor || Screen.fullScreen || ResolutionUtility.BorderlessFullscreen) && Prefs.EdgeScreenScroll && !this.mouseCoveredByUI)
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				Vector2 vector2 = mousePositionOnUI;
				vector2.y = (float)UI.screenHeight - vector2.y;
				Rect rect = new Rect(0f, 0f, 200f, 200f);
				Rect rect2 = new Rect((float)(UI.screenWidth - 250), 0f, 255f, 255f);
				Rect rect3 = new Rect(0f, (float)(UI.screenHeight - 250), 225f, 255f);
				Rect rect4 = new Rect((float)(UI.screenWidth - 250), (float)(UI.screenHeight - 250), 255f, 255f);
				MainTabWindow_Inspect mainTabWindow_Inspect = (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
				if (Find.MainTabsRoot.OpenTab == MainButtonDefOf.Inspect && mainTabWindow_Inspect.RecentHeight > rect3.height)
				{
					rect3.yMin = (float)UI.screenHeight - mainTabWindow_Inspect.RecentHeight;
				}
				if (!rect.Contains(vector2) && !rect3.Contains(vector2) && !rect2.Contains(vector2) && !rect4.Contains(vector2))
				{
					Vector2 b = new Vector2(0f, 0f);
					if (mousePositionOnUI.x >= 0f && mousePositionOnUI.x < 20f)
					{
						b.x -= this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.x <= (float)UI.screenWidth && mousePositionOnUI.x > (float)UI.screenWidth - 20f)
					{
						b.x += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y <= (float)UI.screenHeight && mousePositionOnUI.y > (float)UI.screenHeight - 20f)
					{
						b.y += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y >= 0f && mousePositionOnUI.y < this.ScreenDollyEdgeWidthBottom)
					{
						if (this.mouseTouchingScreenBottomEdgeStartTime < 0f)
						{
							this.mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
						}
						if (Time.realtimeSinceStartup - this.mouseTouchingScreenBottomEdgeStartTime >= 0.28f)
						{
							b.y -= this.config.dollyRateScreenEdge;
						}
						flag = true;
					}
					vector += b;
				}
			}
			if (!flag)
			{
				this.mouseTouchingScreenBottomEdgeStartTime = -1f;
			}
			if (Input.GetKey(KeyCode.LeftShift))
			{
				vector *= 2.4f;
			}
			return vector;
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x0003CCBC File Offset: 0x0003AEBC
		public void Expose()
		{
			if (Scribe.EnterNode("cameraMap"))
			{
				try
				{
					Scribe_Values.Look<Vector3>(ref this.rootPos, "camRootPos", default(Vector3), false);
					Scribe_Values.Look<float>(ref this.desiredSize, "desiredSize", 0f, false);
					this.RootSize = this.desiredSize;
					this.shaker.Expose();
				}
				finally
				{
					Scribe.ExitNode();
				}
			}
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x0003CD34 File Offset: 0x0003AF34
		public void ResetSize()
		{
			this.desiredSize = 24f;
			this.RootSize = this.desiredSize;
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x0003CD4D File Offset: 0x0003AF4D
		public void JumpToCurrentMapLoc(IntVec3 cell)
		{
			this.JumpToCurrentMapLoc(cell.ToVector3Shifted());
		}

		// Token: 0x06000CDD RID: 3293 RVA: 0x0003CD5C File Offset: 0x0003AF5C
		public void JumpToCurrentMapLoc(Vector3 loc)
		{
			this.rootPos = new Vector3(loc.x, this.rootPos.y, loc.z);
		}

		// Token: 0x06000CDE RID: 3294 RVA: 0x0003CD80 File Offset: 0x0003AF80
		public void PanToMapLoc(IntVec3 cell)
		{
			Vector3 vector = cell.ToVector3Shifted();
			float x = Vector3.Distance(vector, this.CurrentRealPosition);
			float duration = GenMath.LerpDoubleClamped(0f, 70f, 0f, 0.25f, x);
			this.panner.PanTo(new CameraPanner.Interpolant(this.rootPos, this.RootSize), new CameraPanner.Interpolant(new Vector3(vector.x, this.rootPos.y, vector.z), this.RootSize), duration, null);
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x0003CE04 File Offset: 0x0003B004
		public void PanToMapLocAndSize(Vector3 loc, float size, float duration = 0.25f, PanCompletionCallback onComplete = null)
		{
			float x = Vector3.Distance(loc, this.CurrentRealPosition);
			float duration2 = GenMath.LerpDoubleClamped(0f, 70f, 0f, duration, x);
			this.panner.PanTo(new CameraPanner.Interpolant(this.rootPos, this.RootSize), new CameraPanner.Interpolant(new Vector3(loc.x, this.rootPos.y, loc.z), size), duration2, onComplete);
		}

		// Token: 0x06000CE0 RID: 3296 RVA: 0x0003CE78 File Offset: 0x0003B078
		public void SetRootPosAndSize(Vector3 rootPos, float rootSize)
		{
			this.rootPos = rootPos;
			this.rootSize = rootSize;
			this.desiredDolly = Vector2.zero;
			this.desiredDollyRaw = Vector2.zero;
			this.desiredSize = this.RootSize;
			this.dragTimeStamps.Clear();
			LongEventHandler.ExecuteWhenFinished(new Action(this.ApplyPositionToGameObject));
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x0003CED1 File Offset: 0x0003B0D1
		public void SetRootSize(float size)
		{
			this.rootSize = size;
			this.desiredSize = this.RootSize;
			LongEventHandler.ExecuteWhenFinished(new Action(this.ApplyPositionToGameObject));
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x0003CEF8 File Offset: 0x0003B0F8
		public static Vector2 GetExtraVelocityFromReleasingDragButton(List<CameraDriver.DragTimeStamp> dragTimeStamps, float velocityFromMouseDragInitialFactor)
		{
			float num = 0f;
			Vector2 vector = Vector2.zero;
			for (int i = 0; i < dragTimeStamps.Count; i++)
			{
				if (dragTimeStamps[i].time < Time.time - 0.05f)
				{
					num = 0.05f;
				}
				else
				{
					num = Mathf.Max(num, Time.time - dragTimeStamps[i].time);
					vector += dragTimeStamps[i].posDelta;
				}
			}
			if (vector != Vector2.zero && num > 0f)
			{
				return vector / num * velocityFromMouseDragInitialFactor;
			}
			return Vector2.zero;
		}

		// Token: 0x0400074B RID: 1867
		public CameraShaker shaker = new CameraShaker();

		// Token: 0x0400074C RID: 1868
		private CameraPanner panner;

		// Token: 0x0400074D RID: 1869
		private Camera cachedCamera;

		// Token: 0x0400074E RID: 1870
		private GameObject reverbDummy;

		// Token: 0x0400074F RID: 1871
		public CameraMapConfig config = new CameraMapConfig_Normal();

		// Token: 0x04000750 RID: 1872
		private Vector3 velocity;

		// Token: 0x04000751 RID: 1873
		private Vector3 rootPos;

		// Token: 0x04000752 RID: 1874
		private float desiredSize;

		// Token: 0x04000753 RID: 1875
		private Vector2 desiredDolly = Vector2.zero;

		// Token: 0x04000754 RID: 1876
		private Vector2 desiredDollyRaw = Vector2.zero;

		// Token: 0x04000755 RID: 1877
		private List<CameraDriver.DragTimeStamp> dragTimeStamps = new List<CameraDriver.DragTimeStamp>();

		// Token: 0x04000756 RID: 1878
		private bool releasedLeftWhileHoldingMiddle;

		// Token: 0x04000757 RID: 1879
		private bool mouseCoveredByUI;

		// Token: 0x04000758 RID: 1880
		private float mouseTouchingScreenBottomEdgeStartTime = -1f;

		// Token: 0x04000759 RID: 1881
		private float fixedTimeStepBuffer;

		// Token: 0x0400075A RID: 1882
		private static int lastViewRectGetFrame = -1;

		// Token: 0x0400075B RID: 1883
		private static CellRect lastViewRect;

		// Token: 0x0400075C RID: 1884
		public const float MaxDeltaTime = 0.1f;

		// Token: 0x0400075D RID: 1885
		private const float ScreenDollyEdgeWidth = 20f;

		// Token: 0x0400075E RID: 1886
		private const float ScreenDollyEdgeWidth_BottomFullscreen = 6f;

		// Token: 0x0400075F RID: 1887
		private const float MinDurationForMouseToTouchScreenBottomEdgeToDolly = 0.28f;

		// Token: 0x04000760 RID: 1888
		private const float DragTimeStampExpireSeconds = 0.05f;

		// Token: 0x04000761 RID: 1889
		private const float VelocityFromMouseDragInitialFactor = 0.75f;

		// Token: 0x04000762 RID: 1890
		private const float MapEdgeClampMarginCells = -2f;

		// Token: 0x04000763 RID: 1891
		public const float StartingSize = 24f;

		// Token: 0x04000764 RID: 1892
		private const float ZoomTightness = 0.4f;

		// Token: 0x04000765 RID: 1893
		private const float ZoomScaleFromAltDenominator = 35f;

		// Token: 0x04000766 RID: 1894
		private const float PageKeyZoomRate = 4f;

		// Token: 0x04000767 RID: 1895
		public const float MinAltitude = 15f;

		// Token: 0x04000768 RID: 1896
		private const float NearClipPlane = 0.5f;

		// Token: 0x04000769 RID: 1897
		private const float MaxAltitude = 65f;

		// Token: 0x0400076A RID: 1898
		private const float ReverbDummyAltitude = 65f;

		// Token: 0x0400076B RID: 1899
		private float rootSize;

		// Token: 0x0400076C RID: 1900
		public const float FullDurationPanDistance = 70f;

		// Token: 0x020001D4 RID: 468
		public struct DragTimeStamp
		{
			// Token: 0x0400076D RID: 1901
			public Vector2 posDelta;

			// Token: 0x0400076E RID: 1902
			public float time;
		}
	}
}
