using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Steam;

namespace RimWorld.Planet
{
	// Token: 0x02003C68 RID: 15464
	public class WorldCameraDriver : MonoBehaviour
	{
		// Token: 0x17003419 RID: 13337
		// (get) Token: 0x06014FF0 RID: 86000 RVA: 0x00649182 File Offset: 0x00647382
		public static float MinAltitude
		{
			get
			{
				return 100f + (SteamDeck.IsSteamDeck ? 17f : 25f);
			}
		}

		// Token: 0x1700341A RID: 13338
		// (get) Token: 0x06014FF1 RID: 86001 RVA: 0x0064919D File Offset: 0x0064739D
		public float TrueAltitude
		{
			get
			{
				return this.layerAltitudeOffset + this.altitude;
			}
		}

		// Token: 0x1700341B RID: 13339
		// (get) Token: 0x06014FF2 RID: 86002 RVA: 0x006491AC File Offset: 0x006473AC
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

		// Token: 0x1700341C RID: 13340
		// (get) Token: 0x06014FF3 RID: 86003 RVA: 0x006491D0 File Offset: 0x006473D0
		public WorldCameraZoomRange CurrentZoom
		{
			get
			{
				float altitudePercent = this.AltitudePercent;
				if (altitudePercent < 0.025f)
				{
					return WorldCameraZoomRange.VeryClose;
				}
				if (altitudePercent < 0.042f)
				{
					return WorldCameraZoomRange.Close;
				}
				if (altitudePercent < 0.125f)
				{
					return WorldCameraZoomRange.Far;
				}
				return WorldCameraZoomRange.VeryFar;
			}
		}

		// Token: 0x1700341D RID: 13341
		// (get) Token: 0x06014FF4 RID: 86004 RVA: 0x0003BF01 File Offset: 0x0003A101
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

		// Token: 0x1700341E RID: 13342
		// (get) Token: 0x06014FF5 RID: 86005 RVA: 0x00649203 File Offset: 0x00647403
		public Vector3 CameraPosition
		{
			get
			{
				return this.MyCamera.transform.position;
			}
		}

		// Token: 0x1700341F RID: 13343
		// (get) Token: 0x06014FF6 RID: 86006 RVA: 0x00649215 File Offset: 0x00647415
		public float AltitudePercent
		{
			get
			{
				return Mathf.InverseLerp(WorldCameraDriver.MinAltitude, 1100f, this.altitude);
			}
		}

		// Token: 0x17003420 RID: 13344
		// (get) Token: 0x06014FF7 RID: 86007 RVA: 0x0064922C File Offset: 0x0064742C
		public Vector3 CurrentlyLookingAtPointOnSphere
		{
			get
			{
				return -(Quaternion.Inverse(this.sphereRotation) * Vector3.forward);
			}
		}

		// Token: 0x17003421 RID: 13345
		// (get) Token: 0x06014FF8 RID: 86008 RVA: 0x00649248 File Offset: 0x00647448
		private bool AnythingPreventsCameraMotion
		{
			get
			{
				return Find.WindowStack.WindowsPreventCameraMotion || !WorldRendererUtility.WorldSelected;
			}
		}

		// Token: 0x06014FF9 RID: 86009 RVA: 0x00649260 File Offset: 0x00647460
		public void Awake()
		{
			this.ResetAltitude();
			this.ApplyPositionToGameObject();
		}

		// Token: 0x06014FFA RID: 86010 RVA: 0x00649270 File Offset: 0x00647470
		public void WorldCameraDriverOnGUI()
		{
			if (Input.GetMouseButtonUp(0) && Input.GetMouseButton(2))
			{
				this.releasedLeftWhileHoldingMiddle = true;
			}
			else if (Event.current.rawType == EventType.MouseDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2))
			{
				this.releasedLeftWhileHoldingMiddle = false;
			}
			this.mouseCoveredByUI = false;
			if (Find.WindowStack.GetWindowAt(UI.MousePositionOnUIInverted) != null)
			{
				this.mouseCoveredByUI = true;
			}
			if (WorldRendererUtility.WorldBackgroundNow)
			{
				this.ApplyPositionToGameObject();
				return;
			}
			if (Find.World == null)
			{
				return;
			}
			if (this.prevSelectedLayer != null && Find.WorldSelector.SelectedLayer != this.prevSelectedLayer)
			{
				this.altitudeDecay = 0f;
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				if (UnityGUIBugsFixer.MouseDrag(2) && (!SteamDeck.IsSteamDeck || !Find.WorldSelector.AnyCaravanSelected))
				{
					Vector2 currentEventDelta = UnityGUIBugsFixer.CurrentEventDelta;
					if (Event.current.type == EventType.MouseDrag)
					{
						Event.current.Use();
					}
					if (currentEventDelta != Vector2.zero)
					{
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.FrameInteraction);
						currentEventDelta.x *= -1f;
						this.desiredRotationRaw += currentEventDelta / GenWorldUI.CurUITileSize() * (0.273f * Prefs.MapDragSensitivity);
					}
				}
				float num = 0f;
				if (Event.current.type == EventType.ScrollWheel)
				{
					num -= Event.current.delta.y * 0.1f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapZoom_In.KeyDownEvent)
				{
					num += 2f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapZoom_Out.KeyDownEvent)
				{
					num -= 2f;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				float num2 = this.desiredAltitude - num * this.config.zoomSpeed * this.TrueAltitude / 12f;
				if (Current.ProgramState == ProgramState.Playing && Prefs.ZoomSwitchWorldLayer)
				{
					if (num2 < WorldCameraDriver.MinAltitude)
					{
						PlanetLayer selected = PlanetLayer.Selected;
						PlanetLayer planetLayer = (selected != null) ? selected.zoomInToLayer : null;
						if (planetLayer != null)
						{
							float num3 = this.layerAltitudeOffset + num2 - planetLayer.ExtraCameraAltitude;
							num2 = (this.altitude = num3);
							this.altitudeDecay = planetLayer.ExtraCameraAltitude - this.layerAltitudeOffset;
							PlanetLayer.Selected = planetLayer;
						}
					}
					else if (num2 > 1100f)
					{
						PlanetLayer selected2 = PlanetLayer.Selected;
						PlanetLayer planetLayer2 = (selected2 != null) ? selected2.zoomOutToLayer : null;
						if (planetLayer2 != null)
						{
							float num4 = this.layerAltitudeOffset + num2 - planetLayer2.ExtraCameraAltitude;
							num2 = (this.altitude = num4);
							this.altitudeDecay = planetLayer2.ExtraCameraAltitude - this.layerAltitudeOffset;
							PlanetLayer.Selected = planetLayer2;
						}
					}
				}
				this.desiredAltitude = Mathf.Clamp(num2, WorldCameraDriver.MinAltitude, 1100f);
				this.desiredRotation = Vector2.zero;
				if (KeyBindingDefOf.MapDolly_Left.IsDown)
				{
					this.desiredRotation.x = -this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapDolly_Right.IsDown)
				{
					this.desiredRotation.x = this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapDolly_Up.IsDown)
				{
					this.desiredRotation.y = this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				if (KeyBindingDefOf.MapDolly_Down.IsDown)
				{
					this.desiredRotation.y = -this.config.dollyRateKeys;
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.WorldCameraMovement, KnowledgeAmount.SpecificInteraction);
				}
				this.config.ConfigOnGUI();
				this.prevSelectedLayer = Find.WorldSelector.SelectedLayer;
			}
		}

		// Token: 0x06014FFB RID: 86011 RVA: 0x00649600 File Offset: 0x00647800
		public void Update()
		{
			if (LongEventHandler.ShouldWaitForEvent)
			{
				return;
			}
			if (Find.World == null)
			{
				this.MyCamera.gameObject.SetActive(false);
				return;
			}
			if (!Find.WorldInterface.everReset)
			{
				Find.WorldInterface.Reset();
			}
			if (WorldRendererUtility.WorldBackgroundNow)
			{
				this.ApplyMapPositionToGameObject();
				return;
			}
			Vector2 vector = this.CalculateCurInputDollyVect();
			if (vector != Vector2.zero)
			{
				float d = (this.altitude - WorldCameraDriver.MinAltitude) / (1100f - WorldCameraDriver.MinAltitude) * 0.85f + 0.15f;
				this.rotationVelocity = new Vector2(vector.x, vector.y) * d;
			}
			if ((Input.GetMouseButtonUp(2) || (SteamDeck.IsSteamDeck && this.releasedLeftWhileHoldingMiddle)) && this.dragTimeStamps.Any<CameraDriver.DragTimeStamp>())
			{
				this.rotationVelocity += CameraDriver.GetExtraVelocityFromReleasingDragButton(this.dragTimeStamps, 5f);
				this.dragTimeStamps.Clear();
			}
			if (!this.AnythingPreventsCameraMotion)
			{
				float num = Time.deltaTime * CameraDriver.HitchReduceFactor;
				this.sphereRotation *= Quaternion.AngleAxis(this.rotationVelocity.x * num * this.config.rotationSpeedScale, this.MyCamera.transform.up);
				this.sphereRotation *= Quaternion.AngleAxis(-this.rotationVelocity.y * num * this.config.rotationSpeedScale, this.MyCamera.transform.right);
				if (this.desiredRotationRaw != Vector2.zero)
				{
					this.sphereRotation *= Quaternion.AngleAxis(this.desiredRotationRaw.x, this.MyCamera.transform.up);
					this.sphereRotation *= Quaternion.AngleAxis(-this.desiredRotationRaw.y, this.MyCamera.transform.right);
				}
				this.dragTimeStamps.Add(new CameraDriver.DragTimeStamp
				{
					posDelta = this.desiredRotationRaw,
					time = Time.time
				});
			}
			this.desiredRotationRaw = Vector2.zero;
			int num2 = Gen.FixedTimeStepUpdate(ref this.fixedTimeStepBuffer, 60f);
			for (int i = 0; i < num2; i++)
			{
				if (this.rotationVelocity != Vector2.zero)
				{
					this.rotationVelocity *= this.config.camRotationDecayFactor;
					if (this.rotationVelocity.magnitude < 0.05f)
					{
						this.rotationVelocity = Vector2.zero;
					}
				}
				if (this.config.smoothZoom)
				{
					float num3 = Mathf.Lerp(this.altitude, this.desiredAltitude, 0.05f);
					this.desiredAltitude += (num3 - this.altitude) * this.config.zoomPreserveFactor;
					this.altitude = num3;
				}
				else
				{
					float num4 = (this.desiredAltitude - this.altitude) * 0.4f;
					this.desiredAltitude += this.config.zoomPreserveFactor * num4;
					this.altitude += num4;
				}
			}
			this.rotationAnimation_lerpFactor += Time.deltaTime * 8f;
			if (Find.PlaySettings.lockNorthUp)
			{
				this.RotateSoNorthIsUp(false);
				this.ClampXRotation(ref this.sphereRotation);
			}
			for (int j = 0; j < num2; j++)
			{
				this.config.ConfigFixedUpdate_60(ref this.rotationVelocity);
			}
			this.ApplyPositionToGameObject();
		}

		// Token: 0x06014FFC RID: 86012 RVA: 0x0064999C File Offset: 0x00647B9C
		public void ApplyMapPositionToGameObject()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return;
			}
			MapParent mapParent = currentMap.ParentHolder as MapParent;
			Vector3 vector;
			if (mapParent != null)
			{
				vector = mapParent.WorldCameraPosition;
			}
			else
			{
				vector = Find.WorldGrid.GetTileCenter(currentMap.Tile);
			}
			if (vector == Vector3.zero)
			{
				return;
			}
			Vector3 vector2 = -vector.normalized;
			vector += -vector2 * currentMap.Tile.Layer.BackgroundWorldCameraOffset;
			Transform transform = this.MyCamera.transform;
			Quaternion rotation = Quaternion.LookRotation(vector2, Vector3.up);
			transform.rotation = rotation;
			float num = currentMap.Tile.Layer.BackgroundWorldCameraParallaxDistancePer100Cells;
			if (num == 0f)
			{
				transform.position = vector;
				return;
			}
			Vector2 viewSpacePosition = Find.CameraDriver.ViewSpacePosition;
			IntVec3 size = Find.CurrentMap.Size;
			float num2 = 1f;
			float num3 = 1f;
			if (size.x > size.z)
			{
				num3 = (float)size.z / (float)size.x;
				num = num * (float)size.x / 100f;
			}
			else if (size.z > size.x)
			{
				num2 = (float)size.x / (float)size.z;
				num = num * (float)size.z / 100f;
			}
			Vector3 up = transform.up;
			Vector3 right = transform.right;
			Vector3 b = up * (viewSpacePosition.y * num * num3) - up * num / 2f * num3;
			Vector3 b2 = right * (viewSpacePosition.x * num * num2) - right * num / 2f * num2;
			transform.position = vector + b + b2 + currentMap.Tile.Layer.Origin;
		}

		// Token: 0x06014FFD RID: 86013 RVA: 0x00649BA8 File Offset: 0x00647DA8
		private void ApplyPositionToGameObject()
		{
			Quaternion rotation;
			if (this.rotationAnimation_lerpFactor < 1f)
			{
				rotation = Quaternion.Lerp(this.rotationAnimation_prevSphereRotation, this.sphereRotation, this.rotationAnimation_lerpFactor);
			}
			else
			{
				rotation = this.sphereRotation;
			}
			if (Find.PlaySettings.lockNorthUp)
			{
				this.ClampXRotation(ref rotation);
			}
			float target = 0f;
			Vector3 target2 = Vector3.zero;
			if (PlanetLayer.Selected != null)
			{
				target = PlanetLayer.Selected.ExtraCameraAltitude;
				target2 = PlanetLayer.Selected.Origin;
			}
			float num = this.layerAltitudeOffset;
			this.layerAltitudeOffset = Mathf.SmoothDamp(this.layerAltitudeOffset, target, ref this.layerOffsetVel, 0.1f);
			this.layerOriginOffset = Vector3.SmoothDamp(this.layerOriginOffset, target2, ref this.layerOriginOffsetVel, 0.1f);
			float num2 = this.layerAltitudeOffset - num;
			if (!Mathf.Approximately(this.altitudeDecay, 0f))
			{
				this.altitudeDecay -= num2;
			}
			else
			{
				this.altitudeDecay = 0f;
			}
			Transform transform = this.MyCamera.transform;
			transform.rotation = Quaternion.Inverse(rotation);
			Vector3 a = transform.rotation * Vector3.forward;
			transform.position = -a * (this.altitude + this.layerAltitudeOffset + this.altitudeDecay) + this.layerOriginOffset;
		}

		// Token: 0x06014FFE RID: 86014 RVA: 0x00649CF4 File Offset: 0x00647EF4
		private Vector2 CalculateCurInputDollyVect()
		{
			Vector2 vector = this.desiredRotation;
			bool flag = false;
			if ((UnityData.isEditor || Screen.fullScreen || ResolutionUtility.BorderlessFullscreen) && Prefs.EdgeScreenScroll && !this.mouseCoveredByUI)
			{
				Vector2 mousePositionOnUI = UI.MousePositionOnUI;
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				Rect rect = new Rect((float)(UI.screenWidth - 250), 0f, 255f, 255f);
				Rect rect2 = new Rect(0f, (float)(UI.screenHeight - 250), 225f, 255f);
				Rect rect3 = new Rect((float)(UI.screenWidth - 250), (float)(UI.screenHeight - 250), 255f, 255f);
				WorldInspectPane inspectPane = Find.World.UI.inspectPane;
				if (Find.WindowStack.IsOpen<WorldInspectPane>() && inspectPane.RecentHeight > rect2.height)
				{
					rect2.yMin = (float)UI.screenHeight - inspectPane.RecentHeight;
				}
				if (!rect2.Contains(mousePositionOnUIInverted) && !rect3.Contains(mousePositionOnUIInverted) && !rect.Contains(mousePositionOnUIInverted))
				{
					Vector2 zero = Vector2.zero;
					if (mousePositionOnUI.x >= 0f && mousePositionOnUI.x < 20f)
					{
						zero.x -= this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.x <= (float)UI.screenWidth && mousePositionOnUI.x > (float)UI.screenWidth - 20f)
					{
						zero.x += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y <= (float)UI.screenHeight && mousePositionOnUI.y > (float)UI.screenHeight - 20f)
					{
						zero.y += this.config.dollyRateScreenEdge;
					}
					if (mousePositionOnUI.y >= 0f && mousePositionOnUI.y < this.ScreenDollyEdgeWidthBottom)
					{
						if (this.mouseTouchingScreenBottomEdgeStartTime < 0f)
						{
							this.mouseTouchingScreenBottomEdgeStartTime = Time.realtimeSinceStartup;
						}
						if (Time.realtimeSinceStartup - this.mouseTouchingScreenBottomEdgeStartTime >= 0.28f)
						{
							zero.y -= this.config.dollyRateScreenEdge;
						}
						flag = true;
					}
					vector += zero;
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

		// Token: 0x06014FFF RID: 86015 RVA: 0x00649F4E File Offset: 0x0064814E
		public void ResetAltitude()
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.altitude = 160f;
			}
			else
			{
				this.altitude = 550f;
			}
			this.desiredAltitude = this.altitude;
		}

		// Token: 0x06015000 RID: 86016 RVA: 0x00649F7C File Offset: 0x0064817C
		public void JumpTo(Vector3 newLookAt)
		{
			if (!Find.WorldInterface.everReset)
			{
				Find.WorldInterface.Reset();
			}
			if (newLookAt != Vector3.zero)
			{
				this.sphereRotation = Quaternion.Inverse(Quaternion.LookRotation(-newLookAt.normalized));
			}
		}

		// Token: 0x06015001 RID: 86017 RVA: 0x00649FC8 File Offset: 0x006481C8
		public void JumpTo(PlanetTile tile)
		{
			this.JumpTo(Find.WorldGrid.GetTileCenter(tile));
			PlanetLayer.Selected = tile.Layer;
		}

		// Token: 0x06015002 RID: 86018 RVA: 0x00649FE8 File Offset: 0x006481E8
		public void RotateSoNorthIsUp(bool interpolate = true)
		{
			if (interpolate)
			{
				this.rotationAnimation_prevSphereRotation = this.sphereRotation;
			}
			this.sphereRotation = Quaternion.Inverse(Quaternion.LookRotation(Quaternion.Inverse(this.sphereRotation) * Vector3.forward));
			if (interpolate)
			{
				this.rotationAnimation_lerpFactor = 0f;
			}
		}

		// Token: 0x06015003 RID: 86019 RVA: 0x0064A038 File Offset: 0x00648238
		private void ClampXRotation(ref Quaternion invRot)
		{
			Vector3 eulerAngles = Quaternion.Inverse(invRot).eulerAngles;
			float altitudePercent = this.AltitudePercent;
			float num = Mathf.Lerp(88.6f, 78f, altitudePercent);
			bool flag = false;
			if (eulerAngles.x <= 90f)
			{
				if (eulerAngles.x > num)
				{
					eulerAngles.x = num;
					flag = true;
				}
			}
			else if (eulerAngles.x < 360f - num)
			{
				eulerAngles.x = 360f - num;
				flag = true;
			}
			if (flag)
			{
				invRot = Quaternion.Inverse(Quaternion.Euler(eulerAngles));
			}
		}

		// Token: 0x0400EA35 RID: 59957
		public WorldCameraConfig config = new WorldCameraConfig_Normal();

		// Token: 0x0400EA36 RID: 59958
		public Quaternion sphereRotation = Quaternion.identity;

		// Token: 0x0400EA37 RID: 59959
		private Vector2 rotationVelocity;

		// Token: 0x0400EA38 RID: 59960
		private Vector2 desiredRotation;

		// Token: 0x0400EA39 RID: 59961
		private Vector2 desiredRotationRaw;

		// Token: 0x0400EA3A RID: 59962
		private float desiredAltitude;

		// Token: 0x0400EA3B RID: 59963
		public float altitude;

		// Token: 0x0400EA3C RID: 59964
		private List<CameraDriver.DragTimeStamp> dragTimeStamps = new List<CameraDriver.DragTimeStamp>();

		// Token: 0x0400EA3D RID: 59965
		private bool releasedLeftWhileHoldingMiddle;

		// Token: 0x0400EA3E RID: 59966
		private float layerAltitudeOffset;

		// Token: 0x0400EA3F RID: 59967
		private float layerOffsetVel;

		// Token: 0x0400EA40 RID: 59968
		private Vector3 layerOriginOffset;

		// Token: 0x0400EA41 RID: 59969
		private Vector3 layerOriginOffsetVel;

		// Token: 0x0400EA42 RID: 59970
		private float altitudeDecay;

		// Token: 0x0400EA43 RID: 59971
		private float altitudeDecayVel;

		// Token: 0x0400EA44 RID: 59972
		private PlanetLayer prevSelectedLayer;

		// Token: 0x0400EA45 RID: 59973
		private Camera cachedCamera;

		// Token: 0x0400EA46 RID: 59974
		private bool mouseCoveredByUI;

		// Token: 0x0400EA47 RID: 59975
		private float mouseTouchingScreenBottomEdgeStartTime = -1f;

		// Token: 0x0400EA48 RID: 59976
		private float fixedTimeStepBuffer;

		// Token: 0x0400EA49 RID: 59977
		private Quaternion rotationAnimation_prevSphereRotation = Quaternion.identity;

		// Token: 0x0400EA4A RID: 59978
		private float rotationAnimation_lerpFactor = 1f;

		// Token: 0x0400EA4B RID: 59979
		private const float SphereRadius = 100f;

		// Token: 0x0400EA4C RID: 59980
		private const float ScreenDollyEdgeWidth = 20f;

		// Token: 0x0400EA4D RID: 59981
		private const float ScreenDollyEdgeWidth_BottomFullscreen = 6f;

		// Token: 0x0400EA4E RID: 59982
		private const float MinDurationForMouseToTouchScreenBottomEdgeToDolly = 0.28f;

		// Token: 0x0400EA4F RID: 59983
		private const float MaxXRotationAtMinAltitude = 88.6f;

		// Token: 0x0400EA50 RID: 59984
		private const float MaxXRotationAtMaxAltitude = 78f;

		// Token: 0x0400EA51 RID: 59985
		private const float TileSizeToRotationSpeed = 0.273f;

		// Token: 0x0400EA52 RID: 59986
		private const float VelocityFromMouseDragInitialFactor = 5f;

		// Token: 0x0400EA53 RID: 59987
		private const float StartingAltitude_Playing = 160f;

		// Token: 0x0400EA54 RID: 59988
		private const float StartingAltitude_Entry = 550f;

		// Token: 0x0400EA55 RID: 59989
		private const float MaxAltitude = 1100f;

		// Token: 0x0400EA56 RID: 59990
		private const float ZoomTightness = 0.4f;

		// Token: 0x0400EA57 RID: 59991
		private const float ZoomScaleFromAltDenominator = 12f;

		// Token: 0x0400EA58 RID: 59992
		private const float PageKeyZoomRate = 2f;

		// Token: 0x0400EA59 RID: 59993
		private const float ScrollWheelZoomRate = 0.1f;

		// Token: 0x0400EA5A RID: 59994
		private PlanetLayer lastSelectedLayer;
	}
}
