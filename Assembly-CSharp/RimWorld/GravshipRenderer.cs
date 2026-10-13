using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020025B0 RID: 9648
	[StaticConstructorOnStartup]
	public class GravshipRenderer
	{
		// Token: 0x0600D362 RID: 54114 RVA: 0x003E442B File Offset: 0x003E262B
		public GravshipRenderer()
		{
			GravshipRenderer.manualTicker = new EventQueue(0.016666668f);
		}

		// Token: 0x0600D363 RID: 54115 RVA: 0x003E4458 File Offset: 0x003E2658
		public void Init(Map map)
		{
			if (!ModLister.CheckOdyssey("Gravship"))
			{
				return;
			}
			this.map = map;
			this.exhaustFleckSystem = new FleckSystemThrown(map.flecks);
			this.flareBlock = new MaterialPropertyBlock();
			this.distortionBlock = new MaterialPropertyBlock();
			this.thrusterFlameBlock = new MaterialPropertyBlock();
		}

		// Token: 0x0600D364 RID: 54116 RVA: 0x003E44AC File Offset: 0x003E26AC
		public void BeginCutscene(Gravship gravship, Vector3 takeoffOrLandingCenter, Vector3 takeoffOrLandingEnginePos, Rot4 landingRotation)
		{
			this.gravshipOffset = Vector3.zero;
			this.gravship = gravship;
			this.takeoffOrLandingPosition = takeoffOrLandingCenter;
			this.takeoffOrLandingEnginePos = takeoffOrLandingEnginePos;
			this.landingRotation = landingRotation;
			this.exhaustFleckSystem.RemoveAllFlecks((IFleck _) => true);
			this.exhaustTimers.Clear();
			foreach (Thing thing in gravship.Thrusters)
			{
				CompGravshipThruster compGravshipThruster;
				if (thing.TryGetComp(out compGravshipThruster) && compGravshipThruster.Props.exhaustSettings != null && compGravshipThruster.Props.exhaustSettings.enabled && compGravshipThruster.Props.exhaustSettings.ExhaustFleckDef != null)
				{
					this.exhaustFleckSystem.handledDefs.AddUnique(compGravshipThruster.Props.exhaustSettings.ExhaustFleckDef);
					this.exhaustTimers.Add(thing, new EventQueue(1f / compGravshipThruster.Props.exhaustSettings.emissionsPerSecond));
				}
			}
		}

		// Token: 0x0600D365 RID: 54117 RVA: 0x003E45E0 File Offset: 0x003E27E0
		public void BeginUpdate()
		{
			this.exhaustFleckSystem.parent = Find.CurrentMap.flecks;
			GravshipRenderer.manualTicker.Push(Time.deltaTime);
			while (GravshipRenderer.manualTicker.Pop())
			{
				this.exhaustFleckSystem.Tick();
			}
			this.exhaustFleckSystem.Update(Time.deltaTime);
		}

		// Token: 0x0600D366 RID: 54118 RVA: 0x003E463C File Offset: 0x003E283C
		public void UpdateTakeoff(float progress)
		{
			float num = Mathf.Pow(progress, 3.5f);
			Vector3 vector = new Vector3(0f, 0f, Find.Camera.orthographicSize * 2.5f * num);
			Vector3 normalized = this.gravship.launchDirection.ToVector3().normalized;
			if (Mathf.Approximately(normalized.z, -1f))
			{
				vector = -vector;
			}
			float num2 = (Mathf.Approximately(normalized.x, 1f) || Mathf.Approximately(normalized.z, -1f)) ? -1f : 1f;
			float y = Mathf.SmoothStep(0f, num2 * 20f, Mathf.InverseLerp(0.2f, 1f, progress + 0.02f * Mathf.Sin(23.561945f * progress)));
			Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
			this.gravshipOffset += quaternion * normalized * (25f * Time.deltaTime * Mathf.InverseLerp(0.2f, 1f, progress));
			this.DrawGravshipGroundEffects(this.gravship.capture, Find.Camera.transform.position, this.takeoffOrLandingPosition + this.gravshipOffset, quaternion, progress, num, true, default(Rot4));
			this.DrawGravship(this.gravship.capture, Find.Camera.transform.position, this.takeoffOrLandingPosition + this.gravshipOffset + vector, this.takeoffOrLandingEnginePos + this.gravshipOffset + vector, quaternion, progress, num, true, default(Rot4));
		}

		// Token: 0x0600D367 RID: 54119 RVA: 0x003E47FC File Offset: 0x003E29FC
		public void UpdateLanding(float progress)
		{
			progress = progress.RemapClamped(0f, 0.95f, 0f, 1f);
			float num = Mathf.Pow(1f - progress, 5f);
			Vector3 b;
			Vector3 b2;
			if (this.landingRotation == Rot4.North || this.landingRotation == Rot4.South)
			{
				b = new Vector3(0f, 0f, 100f * num);
				b2 = this.landingRotation.AsQuat * -this.gravship.launchDirection.ToVector3().normalized * 200f * num;
			}
			else
			{
				b = new Vector3(0f, 0f, 200f * num);
				b2 = this.landingRotation.AsQuat * -this.gravship.launchDirection.ToVector3().normalized * 100f * Mathf.Pow(1f - progress, 9f);
			}
			Quaternion identity = Quaternion.identity;
			this.DrawGravshipGroundEffects(this.gravship.capture, Find.Camera.transform.position, this.takeoffOrLandingPosition + b2, identity, progress, num, false, this.landingRotation);
			this.DrawGravship(this.gravship.capture, Find.Camera.transform.position, this.takeoffOrLandingPosition + b + b2, this.takeoffOrLandingEnginePos + b + b2, identity, progress, num, false, this.landingRotation);
		}

		// Token: 0x0600D368 RID: 54120 RVA: 0x003E49A5 File Offset: 0x003E2BA5
		public void EndUpdate()
		{
			this.exhaustFleckSystem.ForceDraw(this.drawBatch);
			this.drawBatch.Flush(true);
		}

		// Token: 0x0600D369 RID: 54121 RVA: 0x003E49C4 File Offset: 0x003E2BC4
		private void DrawGravship(Capture capture, Vector3 cameraPosition, Vector3 gravshipCenter, Vector3 gravEnginePos, Quaternion gravshipRotation, float cutsceneProgressPercent, float cutsceneHeightPercent, bool isTakeoff, Rot4 landingRotation = default(Rot4))
		{
			Vector3 position = gravEnginePos + this.gravship.Engine.def.Size.ToVector3() * 0.25f;
			Vector3 v = Find.Camera.WorldToViewportPoint(this.RotateAroundPivot(position, gravshipCenter, gravshipRotation));
			this.distortionBlock.SetFloat(ShaderPropertyIDs.Progress, cutsceneProgressPercent);
			this.distortionBlock.SetFloat(GravshipRenderer.ShaderPropertyGravshipHeight, cutsceneHeightPercent);
			this.distortionBlock.SetVector(ShaderPropertyIDs.DrawPos, v);
			this.distortionBlock.SetFloat(GravshipRenderer.ShaderPropertyIsTakeoff, isTakeoff ? 1f : 0f);
			this.DrawLayer(GravshipRenderer.MatGravshipDistortion, cameraPosition.SetToAltitude(AltitudeLayer.Weather).WithYOffset(0.07317074f), this.distortionBlock);
			GravshipRenderer.MatGravship.SetFloat(ShaderPropertyIDs.Progress, cutsceneProgressPercent);
			GravshipRenderer.MatGravship.SetFloat(GravshipRenderer.ShaderPropertyGravshipHeight, cutsceneHeightPercent);
			GravshipRenderer.MatGravship.SetFloat(GravshipRenderer.ShaderPropertyIsTakeoff, isTakeoff ? 1f : 0f);
			GravshipRenderer.MatGravship.mainTexture = (Texture2D)capture.capture;
			GenDraw.DrawQuad(GravshipRenderer.MatGravship, gravshipCenter.SetToAltitude(AltitudeLayer.Skyfaller), gravshipRotation, capture.drawSize, null);
			foreach (LayerSubMesh layerSubMesh in this.gravship.bakedIndoorMasks)
			{
				Graphics.DrawMesh(layerSubMesh.mesh, Matrix4x4.TRS(gravshipCenter + Altitudes.AltIncVect * 2f, gravshipRotation, Vector3.one), layerSubMesh.material, 0);
			}
			if ((isTakeoff && cutsceneProgressPercent <= 0f) || (!isTakeoff && cutsceneProgressPercent >= 1f))
			{
				return;
			}
			Color color = new Color(1f, 1f, 1f, 1f);
			color *= Mathf.Lerp(0.75f, 1f, Mathf.PerlinNoise1D(cutsceneProgressPercent * 100f));
			color.a = Mathf.InverseLerp(0f, 0.1f, isTakeoff ? cutsceneProgressPercent : (1f - cutsceneProgressPercent));
			GravshipRenderer.MatGravshipLensFlare.SetColor(GravshipRenderer.ShaderPropertyColor2, color);
			foreach (KeyValuePair<Thing, PositionData.Data> keyValuePair in this.gravship.ThrusterPlacements)
			{
				Thing thing;
				PositionData.Data data;
				keyValuePair.Deconstruct(out thing, out data);
				Thing thing2 = thing;
				PositionData.Data data2 = data;
				IntVec3 b = -this.gravship.launchDirection;
				Rot4 rotation = data2.rotation;
				if (!(rotation.AsIntVec3 == b))
				{
					CompProperties_GravshipThruster props = thing2.TryGetComp<CompGravshipThruster>().Props;
					float num = (float)thing2.def.size.x * props.flameSize;
					Vector3 b2 = thing2.Rotation.AsQuat * props.flameOffsetsPerDirection[thing2.Rotation.AsInt];
					Vector3 b3 = GenThing.TrueCenter(data2.local, thing2.Rotation, thing2.def.size, 0f) - thing2.Rotation.AsIntVec3.ToVector3() * ((float)thing2.def.size.z * 0.5f + num * 0.5f) + b2;
					Vector3 vector = this.RotateAroundPivot(gravEnginePos + b3, gravshipCenter, gravshipRotation).SetToAltitude(AltitudeLayer.Skyfaller).WithYOffset(0.07317074f);
					Material material = MaterialPool.MatFrom(new MaterialRequest(props.FlameShaderType.Shader)
					{
						renderQueue = 3201
					});
					this.thrusterFlameBlock.Clear();
					this.thrusterFlameBlock.SetColor(GravshipRenderer.ShaderPropertyColor2, color);
					foreach (ShaderParameter shaderParameter in props.flameShaderParameters)
					{
						shaderParameter.Apply(this.thrusterFlameBlock);
					}
					Material mat = material;
					Vector3 position2 = vector;
					rotation = data2.rotation;
					GenDraw.DrawQuad(mat, position2, gravshipRotation * rotation.AsQuat, num, this.thrusterFlameBlock);
					Vector3 v2 = Find.Camera.WorldToViewportPoint(vector);
					this.flareBlock.SetVector(ShaderPropertyIDs.DrawPos, v2);
					this.DrawLayer(GravshipRenderer.MatGravshipLensFlare, cameraPosition.SetToAltitude(AltitudeLayer.MetaOverlays).WithYOffset(0.03658537f), this.flareBlock);
					if (props.exhaustSettings.enabled)
					{
						EventQueue eventQueue = this.exhaustTimers[thing2];
						eventQueue.Push(Time.deltaTime);
						while (eventQueue.Pop())
						{
							CompProperties_GravshipThruster.ExhaustSettings exhaustSettings = props.exhaustSettings;
							Vector3 position3 = vector;
							rotation = data2.rotation;
							this.EmitSmoke(exhaustSettings, position3, gravshipRotation, rotation.AsQuat);
						}
					}
				}
			}
			GravshipRenderer.MatGravFieldExtenderGlow.SetColor(GravshipRenderer.ShaderPropertyColor2, color);
			foreach (IntVec3 intVec in this.gravship.GravFieldExtenderPositions)
			{
				Vector3 b4 = intVec.ToVector3() + ThingDefOf.GravFieldExtender.graphicData.drawSize.ToVector3() * 0.5f;
				Vector3 position4 = this.RotateAroundPivot(gravEnginePos + b4, gravshipCenter, gravshipRotation).SetToAltitude(AltitudeLayer.MetaOverlays).WithYOffset(0.07317074f);
				GenDraw.DrawQuad(GravshipRenderer.MatGravFieldExtenderGlow, position4, Quaternion.identity, 8f, null);
			}
			GravshipRenderer.MatGravEngineGlow.SetColor(GravshipRenderer.ShaderPropertyColor2, color);
			Vector3 position5 = this.RotateAroundPivot(position, gravshipCenter, gravshipRotation).SetToAltitude(AltitudeLayer.MetaOverlays).WithYOffset(0.07317074f);
			GenDraw.DrawQuad(GravshipRenderer.MatGravEngineGlow, position5, Quaternion.identity, 12.5f, null);
		}

		// Token: 0x0600D36A RID: 54122 RVA: 0x003E5008 File Offset: 0x003E3208
		private void DrawGravshipGroundEffects(Capture capture, Vector3 cameraCenter, Vector3 groundCenter, Quaternion rotation, float progress, float height, bool isTakeoff, Rot4 landingRotation = default(Rot4))
		{
			if (progress > 0f && !this.map.Biome.inVacuum)
			{
				GravshipRenderer.MatGravshipDownwash.SetFloat(ShaderPropertyIDs.Progress, progress);
				GravshipRenderer.MatGravshipDownwash.SetFloat(GravshipRenderer.ShaderPropertyGravshipHeight, height);
				GravshipRenderer.MatGravshipDownwash.SetVector(ShaderPropertyIDs.DrawPos, Find.Camera.WorldToViewportPoint(groundCenter));
				GravshipRenderer.MatGravshipDownwash.SetFloat(GravshipRenderer.ShaderPropertyIsTakeoff, isTakeoff ? 1f : 0f);
				this.DrawLayer(GravshipRenderer.MatGravshipDownwash, cameraCenter.SetToAltitude(AltitudeLayer.Gas).WithYOffset(0.03658537f));
			}
			GravshipRenderer.MatGravshipShadow.SetFloat(ShaderPropertyIDs.Progress, 1f - progress);
			GravshipRenderer.MatGravshipShadow.SetFloat(GravshipRenderer.ShaderPropertyGravshipHeight, height);
			GravshipRenderer.MatGravshipShadow.SetFloat(GravshipRenderer.ShaderPropertyIsTakeoff, isTakeoff ? 1f : 0f);
			GravshipRenderer.MatGravshipShadow.color = GravshipRenderer.MatGravshipShadow.color.WithAlpha(progress.RemapClamped(0.9f, 1f, 1f, 0f));
			GravshipRenderer.MatGravshipShadow.mainTexture = (Texture2D)capture.capture;
			GenDraw.DrawQuad(GravshipRenderer.MatGravshipShadow, groundCenter.SetToAltitude(AltitudeLayer.Gas).WithYOffset(0.03658537f), rotation, capture.drawSize * 1.05f, null);
		}

		// Token: 0x0600D36B RID: 54123 RVA: 0x003E5170 File Offset: 0x003E3370
		private void EmitSmoke(CompProperties_GravshipThruster.ExhaustSettings settings, Vector3 position, Quaternion gravshipRotation, Quaternion thrusterRotation)
		{
			Quaternion quaternion = Quaternion.identity;
			if (settings.inheritThrusterRotation)
			{
				quaternion = thrusterRotation * quaternion;
			}
			if (settings.inheritGravshipRotation)
			{
				quaternion = gravshipRotation * quaternion;
			}
			this.exhaustFleckSystem.CreateFleck(new FleckCreationData
			{
				def = settings.ExhaustFleckDef,
				spawnPosition = position + quaternion * settings.spawnOffset + UnityEngine.Random.insideUnitSphere.WithY(0f).normalized * settings.spawnRadiusRange.RandomInRange,
				scale = settings.scaleRange.RandomInRange,
				velocity = new Vector3?(quaternion * Quaternion.Euler(0f, settings.velocityRotationRange.RandomInRange, 0f) * (settings.velocity * settings.velocityMultiplierRange.RandomInRange)),
				rotationRate = settings.rotationOverTimeRange.RandomInRange,
				ageTicksOverride = -1
			});
		}

		// Token: 0x0600D36C RID: 54124 RVA: 0x003E527D File Offset: 0x003E347D
		private Vector3 RotateAroundPivot(Vector3 position, Vector3 pivot, Quaternion rotation)
		{
			return rotation * (position - pivot) + pivot;
		}

		// Token: 0x0600D36D RID: 54125 RVA: 0x003E5292 File Offset: 0x003E3492
		private void DrawLayer(Material mat, Vector3 position)
		{
			this.DrawLayer(mat, position, Quaternion.identity);
		}

		// Token: 0x0600D36E RID: 54126 RVA: 0x003E52A4 File Offset: 0x003E34A4
		private void DrawLayer(Material mat, Vector3 position, Quaternion rotation)
		{
			float num = Find.Camera.orthographicSize * 2f;
			Vector3 s = new Vector3(num * Find.Camera.aspect, 1f, num);
			Matrix4x4 matrix = Matrix4x4.TRS(position, rotation, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mat, 0);
		}

		// Token: 0x0600D36F RID: 54127 RVA: 0x003E52F4 File Offset: 0x003E34F4
		private void DrawLayer(Material mat, Vector3 position, MaterialPropertyBlock props)
		{
			float num = Find.Camera.orthographicSize * 2f;
			Vector3 s = new Vector3(num * Find.Camera.aspect, 1f, num);
			Matrix4x4 matrix = Matrix4x4.TRS(position, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, mat, 0, null, 0, props);
		}

		// Token: 0x04008F86 RID: 36742
		private Gravship gravship;

		// Token: 0x04008F87 RID: 36743
		private MaterialPropertyBlock distortionBlock;

		// Token: 0x04008F88 RID: 36744
		private MaterialPropertyBlock flareBlock;

		// Token: 0x04008F89 RID: 36745
		private MaterialPropertyBlock thrusterFlameBlock;

		// Token: 0x04008F8A RID: 36746
		private static EventQueue manualTicker;

		// Token: 0x04008F8B RID: 36747
		private DrawBatch drawBatch = new DrawBatch();

		// Token: 0x04008F8C RID: 36748
		private FleckSystem exhaustFleckSystem;

		// Token: 0x04008F8D RID: 36749
		private Dictionary<Thing, EventQueue> exhaustTimers = new Dictionary<Thing, EventQueue>();

		// Token: 0x04008F8E RID: 36750
		private Vector3 gravshipOffset;

		// Token: 0x04008F8F RID: 36751
		private Vector3 takeoffOrLandingPosition;

		// Token: 0x04008F90 RID: 36752
		private Vector3 takeoffOrLandingEnginePos;

		// Token: 0x04008F91 RID: 36753
		private Rot4 landingRotation;

		// Token: 0x04008F92 RID: 36754
		private Map map;

		// Token: 0x04008F93 RID: 36755
		private const float GravFieldGlowSize = 8f;

		// Token: 0x04008F94 RID: 36756
		private const float EngineGlowSize = 12.5f;

		// Token: 0x04008F95 RID: 36757
		private const float GravshipMoveSpeed = 25f;

		// Token: 0x04008F96 RID: 36758
		private const float ThrusterFlickerSpeed = 100f;

		// Token: 0x04008F97 RID: 36759
		private const float ThrusterMinBrightness = 0.75f;

		// Token: 0x04008F98 RID: 36760
		private const int ThrusterFlameRenderQueue = 3201;

		// Token: 0x04008F99 RID: 36761
		private static readonly int ShaderPropertyColor2 = Shader.PropertyToID("_Color2");

		// Token: 0x04008F9A RID: 36762
		private static readonly int ShaderPropertyGravshipHeight = Shader.PropertyToID("_GravshipHeight");

		// Token: 0x04008F9B RID: 36763
		private static readonly int ShaderPropertyIsTakeoff = Shader.PropertyToID("_IsTakeoff");

		// Token: 0x04008F9C RID: 36764
		private static readonly Material MatGravship = MatLoader.LoadMat("Map/Gravship/Gravship", -1);

		// Token: 0x04008F9D RID: 36765
		private static readonly Material MatGravshipShadow = MatLoader.LoadMat("Map/Gravship/GravshipShadow", -1);

		// Token: 0x04008F9E RID: 36766
		private static readonly Material MatGravshipDownwash = MatLoader.LoadMat("Map/Gravship/GravshipDownwash", -1);

		// Token: 0x04008F9F RID: 36767
		private static readonly Material MatGravshipDistortion = MatLoader.LoadMat("Map/Gravship/GravshipDistortion", -1);

		// Token: 0x04008FA0 RID: 36768
		private static readonly Material MatGravshipLensFlare = MatLoader.LoadMat("Map/Gravship/GravshipLensFlare", -1);

		// Token: 0x04008FA1 RID: 36769
		private static readonly Material MatGravFieldExtenderGlow = MatLoader.LoadMat("Map/Gravship/GravFieldExtenderGlow", -1);

		// Token: 0x04008FA2 RID: 36770
		private static readonly Material MatGravEngineGlow = MatLoader.LoadMat("Map/Gravship/GravEngineGlow", -1);

		// Token: 0x04008FA3 RID: 36771
		public static readonly Material MatTerrainCurtain = MatLoader.LoadMat("Map/Gravship/FakeTerrain", -1);
	}
}
