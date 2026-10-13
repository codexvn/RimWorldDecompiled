using System;
using System.Collections.Generic;
using LudeonTK;
using TMPro;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C40 RID: 15424
	public class WorldFeatures : IExposable
	{
		// Token: 0x06014ED5 RID: 85717 RVA: 0x006441E3 File Offset: 0x006423E3
		private static void TextWrapThreshold_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x06014ED6 RID: 85718 RVA: 0x006441E3 File Offset: 0x006423E3
		protected static void ForceLegacyText_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x06014ED7 RID: 85719 RVA: 0x006441F0 File Offset: 0x006423F0
		public void ExposeData()
		{
			Scribe_Collections.Look<WorldFeature>(ref this.features, "features", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				WorldGrid grid = Find.WorldGrid;
				if (grid.Surface.tileFeature != null && grid.Surface.tileFeature.Length != 0)
				{
					DataSerializeUtility.LoadUshort(grid.Surface.tileFeature, grid.TilesCount, delegate(int i, ushort data)
					{
						grid[i].feature = ((data == ushort.MaxValue) ? null : this.GetFeatureWithID((int)data));
					});
				}
				this.textsCreated = false;
			}
		}

		// Token: 0x06014ED8 RID: 85720 RVA: 0x0064428C File Offset: 0x0064248C
		public void UpdateFeatures()
		{
			if (!this.textsCreated)
			{
				this.textsCreated = true;
				this.CreateTextsAndSetPosition();
			}
			bool flag = Find.PlaySettings.showWorldFeatures && !WorldRendererUtility.WorldBackgroundNow;
			for (int i = 0; i < this.features.Count; i++)
			{
				Vector3 position = WorldFeatures.texts[i].Position;
				bool flag2 = flag && !WorldRendererUtility.HiddenBehindTerrainNow(position);
				if (flag2 != WorldFeatures.texts[i].Active)
				{
					WorldFeatures.texts[i].SetActive(flag2);
					WorldFeatures.texts[i].WrapAroundPlanetSurface(this.features[i].layer);
				}
				if (flag2)
				{
					this.UpdateAlpha(WorldFeatures.texts[i], this.features[i]);
				}
			}
		}

		// Token: 0x06014ED9 RID: 85721 RVA: 0x00644368 File Offset: 0x00642568
		public WorldFeature GetFeatureWithID(int uniqueID)
		{
			for (int i = 0; i < this.features.Count; i++)
			{
				if (this.features[i].uniqueID == uniqueID)
				{
					return this.features[i];
				}
			}
			return null;
		}

		// Token: 0x06014EDA RID: 85722 RVA: 0x006443B0 File Offset: 0x006425B0
		private void UpdateAlpha(WorldFeatureTextMesh text, WorldFeature feature)
		{
			float num = 0.3f * feature.alpha;
			if (!Mathf.Approximately(text.Color.a, num))
			{
				text.Color = new Color(1f, 1f, 1f, num);
				text.WrapAroundPlanetSurface(feature.layer);
			}
			float num2 = Time.deltaTime * 5f;
			if (this.GoodCameraAltitudeFor(feature))
			{
				feature.alpha += num2;
			}
			else
			{
				feature.alpha -= num2;
			}
			feature.alpha = Mathf.Clamp01(feature.alpha);
		}

		// Token: 0x06014EDB RID: 85723 RVA: 0x00644448 File Offset: 0x00642648
		private bool GoodCameraAltitudeFor(WorldFeature feature)
		{
			float num = feature.EffectiveDrawSize;
			float altitude = Find.WorldCameraDriver.altitude;
			float num2 = 1f / (altitude / WorldFeatures.AlphaScale * (altitude / WorldFeatures.AlphaScale));
			num *= num2;
			if (!feature.layer.IsSelected)
			{
				return false;
			}
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.VeryClose && num >= 0.56f)
			{
				return false;
			}
			if (num < WorldFeatures.VisibleMinimumSize)
			{
				return Find.WorldCameraDriver.AltitudePercent <= 0.07f;
			}
			return num <= WorldFeatures.VisibleMaximumSize || Find.WorldCameraDriver.AltitudePercent >= 0.35f;
		}

		// Token: 0x06014EDC RID: 85724 RVA: 0x006444E4 File Offset: 0x006426E4
		private void CreateTextsAndSetPosition()
		{
			this.CreateOrDestroyTexts();
			float averageTileSize = Find.WorldGrid.AverageTileSize;
			for (int i = 0; i < this.features.Count; i++)
			{
				WorldFeatures.texts[i].Text = this.features[i].name.WordWrapAt(WorldFeatures.TextWrapThreshold);
				WorldFeatures.texts[i].Size = this.features[i].EffectiveDrawSize * averageTileSize;
				Vector3 normalized = this.features[i].drawCenter.normalized;
				Quaternion quaternion = Quaternion.LookRotation(Vector3.Cross(normalized, Vector3.up), normalized);
				quaternion *= Quaternion.Euler(Vector3.right * 90f);
				quaternion *= Quaternion.Euler(Vector3.forward * (90f - this.features[i].drawAngle));
				WorldFeatures.texts[i].Rotation = quaternion;
				WorldFeatures.texts[i].LocalPosition = this.features[i].drawCenter;
				WorldFeatures.texts[i].WrapAroundPlanetSurface(this.features[i].layer);
				WorldFeatures.texts[i].SetActive(false);
			}
		}

		// Token: 0x06014EDD RID: 85725 RVA: 0x00644644 File Offset: 0x00642844
		private void CreateOrDestroyTexts()
		{
			for (int i = 0; i < WorldFeatures.texts.Count; i++)
			{
				WorldFeatures.texts[i].Destroy();
			}
			WorldFeatures.texts.Clear();
			bool flag = LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage;
			for (int j = 0; j < this.features.Count; j++)
			{
				WorldFeatureTextMesh worldFeatureTextMesh;
				if (WorldFeatures.ForceLegacyText || (!flag && this.HasCharactersUnsupportedByTextMeshPro(this.features[j].name)))
				{
					worldFeatureTextMesh = new WorldFeatureTextMesh_Legacy();
				}
				else
				{
					worldFeatureTextMesh = new WorldFeatureTextMesh_TextMeshPro();
				}
				worldFeatureTextMesh.Init();
				WorldFeatures.texts.Add(worldFeatureTextMesh);
			}
		}

		// Token: 0x06014EDE RID: 85726 RVA: 0x006446E8 File Offset: 0x006428E8
		private bool HasCharactersUnsupportedByTextMeshPro(string str)
		{
			TMP_FontAsset font = WorldFeatureTextMesh_TextMeshPro.WorldTextPrefab.GetComponent<TextMeshPro>().font;
			for (int i = 0; i < str.Length; i++)
			{
				if (!this.HasCharacter(font, str[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06014EDF RID: 85727 RVA: 0x0064472C File Offset: 0x0064292C
		private bool HasCharacter(TMP_FontAsset font, char character)
		{
			if (TMP_FontAsset.GetCharacters(font).IndexOf(character) >= 0)
			{
				return true;
			}
			List<TMP_FontAsset> fallbackFontAssetTable = font.fallbackFontAssetTable;
			for (int i = 0; i < fallbackFontAssetTable.Count; i++)
			{
				if (TMP_FontAsset.GetCharacters(fallbackFontAssetTable[i]).IndexOf(character) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400E956 RID: 59734
		public List<WorldFeature> features = new List<WorldFeature>();

		// Token: 0x0400E957 RID: 59735
		public bool textsCreated;

		// Token: 0x0400E958 RID: 59736
		private static List<WorldFeatureTextMesh> texts = new List<WorldFeatureTextMesh>();

		// Token: 0x0400E959 RID: 59737
		private const float BaseAlpha = 0.3f;

		// Token: 0x0400E95A RID: 59738
		private const float AlphaChangeSpeed = 5f;

		// Token: 0x0400E95B RID: 59739
		[TweakValue("Interface", 0f, 300f)]
		private static float TextWrapThreshold = 150f;

		// Token: 0x0400E95C RID: 59740
		[TweakValue("Interface.World", 0f, 100f)]
		protected static bool ForceLegacyText = false;

		// Token: 0x0400E95D RID: 59741
		[TweakValue("Interface.World", 1f, 150f)]
		protected static float AlphaScale = 30f;

		// Token: 0x0400E95E RID: 59742
		[TweakValue("Interface.World", 0f, 1f)]
		protected static float VisibleMinimumSize = 0.04f;

		// Token: 0x0400E95F RID: 59743
		[TweakValue("Interface.World", 0f, 5f)]
		protected static float VisibleMaximumSize = 1f;
	}
}
