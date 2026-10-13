using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using Unity.Mathematics;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000653 RID: 1619
	public static class SilhouetteUtility
	{
		// Token: 0x060028BD RID: 10429 RVA: 0x000D71C4 File Offset: 0x000D53C4
		public static void DrawGraphicSilhouette(Thing thing, Vector3 pos)
		{
			if (SilhouetteUtility.ShouldDrawSilhouette(thing))
			{
				SilhouetteUtility.DrawSilhouette(thing, pos);
			}
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x000D71D8 File Offset: 0x000D53D8
		public static void DrawGUISilhouette(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null && SilhouetteUtility.ShouldDrawPawnDotSilhouette(pawn))
			{
				SilhouetteUtility.DrawPawnDotSilhouette(pawn);
			}
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x000D7200 File Offset: 0x000D5400
		private static SilhouetteUtility.SilhouetteCacheKey GetSilhouetteCacheKey(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				return new SilhouetteUtility.SilhouetteCacheKey(pawn);
			}
			return new SilhouetteUtility.SilhouetteCacheKey(thing);
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x000D7224 File Offset: 0x000D5424
		public static void NotifyGraphicDirty(Thing thing)
		{
			SilhouetteUtility.SilhouetteCacheKey silhouetteCacheKey = SilhouetteUtility.GetSilhouetteCacheKey(thing);
			if (SilhouetteUtility.materialCache.ContainsKey(silhouetteCacheKey))
			{
				SilhouetteUtility.materialCache.Remove(silhouetteCacheKey);
			}
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x000D7254 File Offset: 0x000D5454
		private static void DrawPawnDotSilhouette(Thing thing)
		{
			Color color = SilhouetteUtility.GetColor(thing);
			color.a = SilhouetteUtility.GetAlpha();
			GUI.DrawTexture(SilhouetteUtility.GetAdjustedScreenspaceRect(thing, 0.01f), TexUI.DotHighlight, ScaleMode.ScaleToFit, true, 0f, color, 0f, 0f);
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x000D729C File Offset: 0x000D549C
		public static void DrawSilhouetteJob(Thing thing, Matrix4x4 trs)
		{
			ValueTuple<Mesh, Material> cachedSilhouetteData = SilhouetteUtility.GetCachedSilhouetteData(thing);
			Mesh item = cachedSilhouetteData.Item1;
			Material item2 = cachedSilhouetteData.Item2;
			MaterialPropertyBlock cachedMaterialPropertyBlock = SilhouetteUtility.GetCachedMaterialPropertyBlock(SilhouetteUtility.GetColor(thing));
			GenDraw.DrawMeshNowOrLater(item, trs, item2, false, cachedMaterialPropertyBlock);
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x000D72D4 File Offset: 0x000D54D4
		private static void DrawSilhouette(Thing thing, Vector3 pos)
		{
			ValueTuple<Mesh, Material> cachedSilhouetteData = SilhouetteUtility.GetCachedSilhouetteData(thing);
			Mesh item = cachedSilhouetteData.Item1;
			Material item2 = cachedSilhouetteData.Item2;
			Pawn pawn = thing as Pawn;
			Graphic graphic = (pawn != null) ? pawn.Drawer.renderer.SilhouetteGraphic : thing.Graphic;
			MaterialPropertyBlock cachedMaterialPropertyBlock = SilhouetteUtility.GetCachedMaterialPropertyBlock(SilhouetteUtility.GetColor(thing));
			Vector3 vector = thing.def.rotatable ? new Vector3(graphic.drawSize.y, 0f, graphic.drawSize.x) : new Vector3(graphic.drawSize.x, 0f, graphic.drawSize.y);
			Vector3 inverseFovScale = Find.CameraDriver.InverseFovScale;
			if (vector.x < 2.5f)
			{
				inverseFovScale.x *= vector.x + SilhouetteUtility.AdjustScale(vector.x);
			}
			else
			{
				inverseFovScale.x *= vector.x;
			}
			if (vector.z < 2.5f)
			{
				inverseFovScale.z *= vector.z + SilhouetteUtility.AdjustScale(vector.z);
			}
			else
			{
				inverseFovScale.z *= vector.z;
			}
			Matrix4x4 matrix = Matrix4x4.TRS(pos.SetToAltitude(AltitudeLayer.Silhouettes), Quaternion.AngleAxis(0f, Vector3.up), inverseFovScale);
			GenDraw.DrawMeshNowOrLater(item, matrix, item2, false, cachedMaterialPropertyBlock);
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x000D742F File Offset: 0x000D562F
		public static bool ShouldDrawSilhouette(Thing thing)
		{
			return !WorldComponent_GravshipController.GravshipRenderInProgess && !Find.ScreenshotModeHandler.Active && Prefs.HighlightStyleMode == HighlightStyleMode.Silhouettes && thing.def.drawHighlight && SilhouetteUtility.ShouldHighlight(thing);
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x000D746C File Offset: 0x000D566C
		private static bool ShouldDrawPawnDotSilhouette(Thing thing)
		{
			return Prefs.HighlightStyleMode == HighlightStyleMode.Dots && thing.def.drawHighlight && SilhouetteUtility.ShouldHighlight(thing);
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x000D748C File Offset: 0x000D568C
		public static bool CanHighlightAny()
		{
			if (Prefs.DotHighlightDisplayMode == DotHighlightDisplayMode.None)
			{
				return false;
			}
			CameraDriver cameraDriver = Find.CameraDriver;
			return cameraDriver.ZoomRootSize >= cameraDriver.config.sizeRange.max * 0.9f;
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x000D74CC File Offset: 0x000D56CC
		private static bool ShouldHighlight(Thing thing)
		{
			if (!SilhouetteUtility.CanHighlightAny())
			{
				return false;
			}
			if (!thing.Spawned)
			{
				return false;
			}
			if (thing.shouldHighlightCachedTick == GenTicks.TicksGame)
			{
				return thing.shouldHighlightCached;
			}
			thing.shouldHighlightCachedTick = GenTicks.TicksGame;
			return thing.shouldHighlightCached = SilhouetteUtility.ShouldHighlightInt(thing);
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x000D751C File Offset: 0x000D571C
		private static bool ShouldHighlightInt(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			if (pawn != null)
			{
				if (!pawn.IsPlayerControlled && pawn.Fogged())
				{
					return false;
				}
				if (Prefs.DotHighlightDisplayMode == DotHighlightDisplayMode.HighlightHostiles && !pawn.HostileTo(Faction.OfPlayer))
				{
					return false;
				}
				if (pawn.IsHiddenFromPlayer())
				{
					return false;
				}
				if (ModsConfig.AnomalyActive && pawn.Map.gameConditionManager.MapBrightness < 0.1f && thing.Map.glowGrid.GroundGlowAt(thing.Position, false, false) <= 0f && pawn.Faction != Faction.OfPlayer)
				{
					return false;
				}
			}
			else
			{
				if (thing.Fogged())
				{
					return false;
				}
				if (thing.def.drawHighlightOnlyForHostile && !thing.HostileTo(Faction.OfPlayer))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x000D75DC File Offset: 0x000D57DC
		private static MaterialPropertyBlock GetCachedMaterialPropertyBlock(Color color)
		{
			float3 key = new float3(color.r, color.g, color.b);
			color.a = SilhouetteUtility.GetAlpha();
			MaterialPropertyBlock materialPropertyBlock;
			if (SilhouetteUtility.materialPropertyBlockCache.TryGetValue(key, out materialPropertyBlock))
			{
				materialPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			}
			else
			{
				materialPropertyBlock = new MaterialPropertyBlock();
				materialPropertyBlock.SetColor(ShaderPropertyIDs.Color, color);
				SilhouetteUtility.materialPropertyBlockCache[key] = materialPropertyBlock;
			}
			return materialPropertyBlock;
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x000D764C File Offset: 0x000D584C
		private static Color GetColor(Thing thing)
		{
			if (thing.highlightColorCachedTick == GenTicks.TicksGame)
			{
				return thing.highlightColorCached;
			}
			thing.highlightColorCachedTick = GenTicks.TicksGame;
			return thing.highlightColorCached = SilhouetteUtility.GetColorInt(thing);
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x000D7688 File Offset: 0x000D5888
		private static Color GetColorInt(Thing thing)
		{
			Color result;
			if (thing.def.highlightColor != null)
			{
				result = thing.def.highlightColor.Value;
			}
			else
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					if (pawn.IsColonyMech && !pawn.IsColonyMechPlayerControlled)
					{
						result = SilhouetteUtility.UncontrolledMechDotColor;
					}
					else
					{
						result = PawnNameColorUtility.PawnNameColorOf(pawn);
					}
				}
				else if (thing.HostileTo(Faction.OfPlayer))
				{
					result = PawnNameColorUtility.ColorBaseHostile;
				}
				else
				{
					result = SilhouetteUtility.ThingColor;
				}
			}
			return result;
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x000D7704 File Offset: 0x000D5904
		private static float GetAlpha()
		{
			if (SilhouetteUtility.lastCachedAlphaFrame == RealTime.frameCount)
			{
				return SilhouetteUtility.lastCachedAlpha;
			}
			SilhouetteUtility.lastCachedAlphaFrame = RealTime.frameCount;
			bool flag = Prefs.HighlightStyleMode == HighlightStyleMode.Silhouettes;
			CameraDriver cameraDriver = Find.CameraDriver;
			float num = Mathf.Clamp01(Mathf.InverseLerp(cameraDriver.config.sizeRange.max * 0.84999996f, cameraDriver.config.sizeRange.max, cameraDriver.ZoomRootSize));
			if (flag)
			{
				return SilhouetteUtility.lastCachedAlpha = 0.9f * num;
			}
			return SilhouetteUtility.lastCachedAlpha = 0.75f * num;
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x000D7790 File Offset: 0x000D5990
		[return: TupleElementNames(new string[]
		{
			"mesh",
			"material"
		})]
		private static ValueTuple<Mesh, Material> GetCachedSilhouetteData(Thing thing)
		{
			if (SilhouetteUtility.materialCache.Count > 512)
			{
				SilhouetteUtility.materialCache.Clear();
			}
			SilhouetteUtility.SilhouetteCacheKey silhouetteCacheKey = SilhouetteUtility.GetSilhouetteCacheKey(thing);
			if (!SilhouetteUtility.materialCache.ContainsKey(silhouetteCacheKey))
			{
				Pawn pawn = thing as Pawn;
				Graphic coloredVersion = ((pawn != null) ? pawn.Drawer.renderer.SilhouetteGraphic : thing.Graphic).GetColoredVersion(ShaderDatabase.Silhouette, Color.white, Color.white);
				SilhouetteUtility.materialCache[silhouetteCacheKey] = new SilhouetteUtility.SilhouetteCacheValue(coloredVersion.MatEast, coloredVersion.MatWest);
			}
			Material item;
			Mesh item2;
			if (thing.Rotation == Rot4.West)
			{
				item = SilhouetteUtility.materialCache[silhouetteCacheKey].west;
				item2 = MeshPool.GridPlaneFlip(Vector2.one);
			}
			else
			{
				item = SilhouetteUtility.materialCache[silhouetteCacheKey].east;
				item2 = MeshPool.GridPlane(Vector2.one);
			}
			return new ValueTuple<Mesh, Material>(item2, item);
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x000D7875 File Offset: 0x000D5A75
		public static float AdjustScale(float scale)
		{
			return Mathf.InverseLerp(2.5f, 0f, scale) * 0.75f;
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x000D7890 File Offset: 0x000D5A90
		public static Rect GetAdjustedScreenspaceRect(Thing thing, float screenSizeRatio = 0.01f)
		{
			Vector2 vector = Find.Camera.WorldToScreenPoint(thing.DrawPos) / Prefs.UIScale;
			vector.y = (float)UI.screenHeight - vector.y;
			float num = screenSizeRatio * (float)Mathf.Min(Screen.width, Screen.height) / Prefs.UIScale;
			return new Rect(vector.x - num, vector.y - num / 2f, num * 2f, num);
		}

		// Token: 0x0400217B RID: 8571
		private static readonly Dictionary<SilhouetteUtility.SilhouetteCacheKey, SilhouetteUtility.SilhouetteCacheValue> materialCache = new Dictionary<SilhouetteUtility.SilhouetteCacheKey, SilhouetteUtility.SilhouetteCacheValue>(512);

		// Token: 0x0400217C RID: 8572
		private static readonly Dictionary<float3, MaterialPropertyBlock> materialPropertyBlockCache = new Dictionary<float3, MaterialPropertyBlock>();

		// Token: 0x0400217D RID: 8573
		private static int lastCachedAlphaFrame = -1;

		// Token: 0x0400217E RID: 8574
		private static float lastCachedAlpha = 0f;

		// Token: 0x0400217F RID: 8575
		private const float DotHighlightSizeRatio = 0.01f;

		// Token: 0x04002180 RID: 8576
		private const float DotAlpha = 0.75f;

		// Token: 0x04002181 RID: 8577
		private const float SilhouetteAlpha = 0.9f;

		// Token: 0x04002182 RID: 8578
		private static readonly Color ThingColor = new Color(0.56f, 0.62f, 0.9f);

		// Token: 0x04002183 RID: 8579
		private static readonly Color UncontrolledMechDotColor = new Color(0.8f, 0.55f, 0.17f, 0.75f);

		// Token: 0x04002184 RID: 8580
		public const float DotHighlightStartRange = 0.9f;

		// Token: 0x04002185 RID: 8581
		private const int MaximumMaterialCache = 512;

		// Token: 0x02000654 RID: 1620
		private readonly struct SilhouetteCacheKey
		{
			// Token: 0x060028D1 RID: 10449 RVA: 0x000D7980 File Offset: 0x000D5B80
			public SilhouetteCacheKey(Pawn pawn)
			{
				this.thingDef = pawn.def;
				this.lifeStageDef = pawn.ageTracker.CurLifeStage;
				this.graphicIndex = pawn.GetGraphicIndex();
				this.gender = pawn.gender;
				Pawn_MutantTracker mutant = pawn.mutant;
				this.rotMode = ((mutant != null) ? mutant.rotStage : RotStage.Fresh);
			}

			// Token: 0x060028D2 RID: 10450 RVA: 0x000D79DC File Offset: 0x000D5BDC
			public SilhouetteCacheKey(Thing thing)
			{
				this.thingDef = thing.def;
				this.lifeStageDef = null;
				this.graphicIndex = (thing.OverrideGraphicIndex ?? -1);
				this.gender = Gender.None;
				this.rotMode = RotStage.Fresh;
			}

			// Token: 0x060028D3 RID: 10451 RVA: 0x000D7A2C File Offset: 0x000D5C2C
			public override int GetHashCode()
			{
				int seed = this.thingDef.GetHashCode();
				if (this.lifeStageDef != null)
				{
					seed = Gen.HashCombineInt(seed, this.lifeStageDef.GetHashCode());
				}
				seed = Gen.HashCombineInt(seed, this.graphicIndex);
				seed = Gen.HashCombineInt(seed, this.gender.GetHashCode());
				return Gen.HashCombineInt(seed, this.rotMode.GetHashCode());
			}

			// Token: 0x04002186 RID: 8582
			private readonly ThingDef thingDef;

			// Token: 0x04002187 RID: 8583
			private readonly LifeStageDef lifeStageDef;

			// Token: 0x04002188 RID: 8584
			private readonly int graphicIndex;

			// Token: 0x04002189 RID: 8585
			private readonly Gender gender;

			// Token: 0x0400218A RID: 8586
			private readonly RotStage rotMode;
		}

		// Token: 0x02000655 RID: 1621
		private class SilhouetteCacheValue : IDisposable
		{
			// Token: 0x060028D4 RID: 10452 RVA: 0x000D7A9D File Offset: 0x000D5C9D
			public SilhouetteCacheValue(Material east, Material west)
			{
				this.east = east;
				this.west = west;
			}

			// Token: 0x060028D5 RID: 10453 RVA: 0x000D7AB3 File Offset: 0x000D5CB3
			public void Dispose()
			{
				UnityEngine.Object.Destroy(this.east);
				UnityEngine.Object.Destroy(this.west);
			}

			// Token: 0x0400218B RID: 8587
			public readonly Material east;

			// Token: 0x0400218C RID: 8588
			public readonly Material west;
		}
	}
}
