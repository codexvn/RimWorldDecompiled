using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020028C8 RID: 10440
	public class ScenPart_PlanetLayer : ScenPart
	{
		// Token: 0x170022B3 RID: 8883
		// (get) Token: 0x0600E450 RID: 58448 RVA: 0x0042AF35 File Offset: 0x00429135
		public PlanetLayerSettings Settings
		{
			get
			{
				return this.settingsDef.settings;
			}
		}

		// Token: 0x0600E451 RID: 58449 RVA: 0x0042AF42 File Offset: 0x00429142
		public override bool Valid()
		{
			return base.Valid() && this.settingsDef != null;
		}

		// Token: 0x170022B4 RID: 8884
		// (get) Token: 0x0600E452 RID: 58450 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool CanEdit
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600E453 RID: 58451 RVA: 0x0042AF58 File Offset: 0x00429158
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PlanetLayerDef>(ref this.layer, "layer");
			Scribe_Defs.Look<PlanetLayerSettingsDef>(ref this.settingsDef, "settingsDef");
			Scribe_Values.Look<bool>(ref this.hide, "hide", false, false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_Collections.Look<LayerConnection>(ref this.connections, "connections", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.layer == null)
				{
					Log.Error("ScenPart has null layer after loading.");
				}
				if (this.settingsDef == null)
				{
					Log.Error("ScenPart has null playet layer settings def.");
				}
			}
		}

		// Token: 0x0600E454 RID: 58452 RVA: 0x0042AFF4 File Offset: 0x004291F4
		public override int GetHashCode()
		{
			HashCode hashCode = default(HashCode);
			hashCode.Add<int>(base.GetHashCode());
			hashCode.Add<int>((this.layer != null) ? this.layer.GetHashCode() : 0);
			hashCode.Add<int>((this.settingsDef != null) ? this.settingsDef.GetHashCode() : 0);
			hashCode.Add<string>(this.tag);
			foreach (LayerConnection layerConnection in this.connections)
			{
				hashCode.Add<int>(layerConnection.GetHashCode());
			}
			return hashCode.ToHashCode();
		}

		// Token: 0x0600E455 RID: 58453 RVA: 0x0042B0B0 File Offset: 0x004292B0
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (string.IsNullOrEmpty(this.tag))
			{
				yield return "Layer tag empty";
			}
			yield break;
			yield break;
		}

		// Token: 0x0600E456 RID: 58454 RVA: 0x0042B0C0 File Offset: 0x004292C0
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (this.hide)
			{
				return;
			}
			this.ClearInvalidConnections();
			Rect rect = listing.GetScenPartRect(this, this.lastHeight).TopPartPixels(ScenPart.RowHeight);
			float yMin = rect.yMin;
			this.DoTypeRow(ref rect);
			this.DoTagRow(ref rect);
			this.DoSettings(ref rect);
			this.DoConnections(ref rect);
			this.lastHeight = rect.yMax - yMin + 4f;
		}

		// Token: 0x0600E457 RID: 58455 RVA: 0x0042B134 File Offset: 0x00429334
		private bool IsTagValid()
		{
			if (string.IsNullOrEmpty(this.tag))
			{
				return false;
			}
			using (List<string>.Enumerator enumerator = this.GetAllTags().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == this.tag)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600E458 RID: 58456 RVA: 0x00002501 File Offset: 0x00000701
		public override bool TryMerge(ScenPart other)
		{
			return false;
		}

		// Token: 0x0600E459 RID: 58457 RVA: 0x0042B1A4 File Offset: 0x004293A4
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_PlanetLayer scenPart_PlanetLayer = other as ScenPart_PlanetLayer;
			return scenPart_PlanetLayer == null || !(scenPart_PlanetLayer.tag == this.tag);
		}

		// Token: 0x0600E45A RID: 58458 RVA: 0x0042B1D4 File Offset: 0x004293D4
		private List<string> GetAllTags()
		{
			ScenPart_PlanetLayer.tmpTags.Clear();
			foreach (ScenPart scenPart in Find.Scenario.AllParts)
			{
				if (scenPart != this)
				{
					ScenPart_PlanetLayer scenPart_PlanetLayer = scenPart as ScenPart_PlanetLayer;
					if (scenPart_PlanetLayer != null && !string.IsNullOrEmpty(scenPart_PlanetLayer.tag))
					{
						ScenPart_PlanetLayer.tmpTags.Add(scenPart_PlanetLayer.tag);
					}
				}
			}
			return ScenPart_PlanetLayer.tmpTags;
		}

		// Token: 0x0600E45B RID: 58459 RVA: 0x0042B258 File Offset: 0x00429458
		private void DoLabel(Rect row, string label)
		{
			Rect rect = row;
			rect.x -= rect.width + 5f;
			using (new TextBlock(TextAnchor.MiddleRight))
			{
				Widgets.Label(rect, label);
			}
		}

		// Token: 0x0600E45C RID: 58460 RVA: 0x0042B2B4 File Offset: 0x004294B4
		private void DoTypeRow(ref Rect row)
		{
			using (new TextBlock(this.CanEdit ? Color.white : Color.gray))
			{
				Rect rect = row;
				PlanetLayerDef planetLayerDef = this.layer;
				if (Widgets.ButtonText(rect, (planetLayerDef != null) ? planetLayerDef.LabelCap : "ScenPart_SelectQuestDef".Translate(), true, true, this.CanEdit, null))
				{
					FloatMenuUtility.MakeMenu<PlanetLayerDef>(DefDatabase<PlanetLayerDef>.AllDefs, (PlanetLayerDef id) => id.LabelCap, (PlanetLayerDef selected) => delegate()
					{
						this.layer = selected;
					});
				}
				row.y += ScenPart.RowHeight + 4f;
			}
		}

		// Token: 0x0600E45D RID: 58461 RVA: 0x0042B388 File Offset: 0x00429588
		private void DoTagRow(ref Rect row)
		{
			string text = this.tag;
			this.tag = Widgets.TextArea(row, this.tag, false);
			if (!this.IsTagValid())
			{
				using (new TextBlock(ColorLibrary.RedReadable))
				{
					this.DoLabel(row, "ScenPart_PlanetLayerTag_Error".Translate());
					goto IL_79;
				}
			}
			this.DoLabel(row, "ScenPart_PlanetLayerTag".Translate());
			IL_79:
			if (text != this.tag && !string.IsNullOrEmpty(this.tag))
			{
				foreach (ScenPart scenPart in Find.Scenario.AllParts)
				{
					if (scenPart != this)
					{
						ScenPart_PlanetLayer scenPart_PlanetLayer = scenPart as ScenPart_PlanetLayer;
						if (scenPart_PlanetLayer != null)
						{
							foreach (LayerConnection layerConnection in scenPart_PlanetLayer.connections)
							{
								if (layerConnection.tag == text)
								{
									layerConnection.tag = this.tag;
								}
							}
						}
					}
				}
			}
			row.y += ScenPart.RowHeight + 4f;
		}

		// Token: 0x0600E45E RID: 58462 RVA: 0x0042B4FC File Offset: 0x004296FC
		private void DoSettings(ref Rect row)
		{
			this.Seperator(ref row);
			this.DoLabel(row, "ScenPart_PlanetLayerSettings".Translate());
			PlanetLayerSettingsDef planetLayerSettingsDef = this.settingsDef;
			TaggedString taggedString = (planetLayerSettingsDef != null) ? planetLayerSettingsDef.LabelCap : "ScenPart_PlanetLayerSettings_SelectDef".Translate();
			if (Widgets.ButtonText(row, taggedString, true, true, true, null))
			{
				FloatMenuUtility.MakeMenu<PlanetLayerSettingsDef>(DefDatabase<PlanetLayerSettingsDef>.AllDefs, (PlanetLayerSettingsDef x) => x.LabelCap, (PlanetLayerSettingsDef x) => delegate()
				{
					this.settingsDef = x;
				});
			}
			row.y += ScenPart.RowHeight + 4f;
		}

		// Token: 0x0600E45F RID: 58463 RVA: 0x0042B5B4 File Offset: 0x004297B4
		private void DoConnections(ref Rect row)
		{
			this.Seperator(ref row);
			this.DoLabel(row, "ScenPart_PlanetLayerConnections".Translate());
			for (int i = this.connections.Count - 1; i >= 0; i--)
			{
				if (i % 2 == 1)
				{
					Widgets.DrawLightHighlight(row);
				}
				LayerConnection other = this.connections[i];
				Rect rect = row.LeftPart(0.25f);
				Rect rect2 = row.LeftPart(0.25f);
				Rect rect3 = row.RightPart(0.5f);
				rect2.x = rect.xMax;
				rect.xMax -= 4f;
				rect2.xMax += 4f;
				rect2.xMax -= 4f;
				rect3.xMin += 4f;
				if (Widgets.ButtonText(rect, other.tag, true, true, true, null))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					int captured = i;
					list.Add(new FloatMenuOption("Remove", delegate()
					{
						this.connections.RemoveAt(captured);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					using (List<string>.Enumerator enumerator = this.GetAllTags().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string value = enumerator.Current;
							if (!this.connections.Any((LayerConnection x) => x.tag == value))
							{
								list.Add(new FloatMenuOption(value, delegate()
								{
									other.tag = value;
								}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				string label;
				if (other.zoomMode == LayerConnection.ZoomMode.None)
				{
					label = "ScenPart_PlanetLayerConnections_Zoom".Translate();
				}
				else if (other.zoomMode == LayerConnection.ZoomMode.ZoomIn)
				{
					label = "ScenPart_PlanetLayerConnections_ZoomIn".Translate();
				}
				else
				{
					label = "ScenPart_PlanetLayerConnections_ZoomOut".Translate();
				}
				if (Widgets.ButtonText(rect2, label, true, true, true, null))
				{
					List<FloatMenuOption> list2 = new List<FloatMenuOption>();
					int localIndex = i;
					if (other.zoomMode != LayerConnection.ZoomMode.None)
					{
						list2.Add(new FloatMenuOption("ScenPart_PlanetLayerConnections_Zoom".Translate(), delegate()
						{
							other.zoomMode = LayerConnection.ZoomMode.None;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					if (other.zoomMode != LayerConnection.ZoomMode.ZoomIn)
					{
						list2.Add(new FloatMenuOption("ScenPart_PlanetLayerConnections_ZoomIn".Translate(), delegate()
						{
							other.zoomMode = LayerConnection.ZoomMode.ZoomIn;
							for (int j = 0; j < this.connections.Count; j++)
							{
								if (localIndex != j && this.connections[j].zoomMode == LayerConnection.ZoomMode.ZoomIn)
								{
									this.connections[j].zoomMode = LayerConnection.ZoomMode.None;
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					if (other.zoomMode != LayerConnection.ZoomMode.ZoomOut)
					{
						list2.Add(new FloatMenuOption("ScenPart_PlanetLayerConnections_ZoomOut".Translate(), delegate()
						{
							other.zoomMode = LayerConnection.ZoomMode.ZoomOut;
							for (int j = 0; j < this.connections.Count; j++)
							{
								if (localIndex != j && this.connections[j].zoomMode == LayerConnection.ZoomMode.ZoomOut)
								{
									this.connections[j].zoomMode = LayerConnection.ZoomMode.None;
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
					Find.WindowStack.Add(new FloatMenu(list2));
				}
				Rect rect4 = rect3.LeftHalf();
				rect4.xMax -= 4f;
				using (new TextBlock(TextAnchor.MiddleRight))
				{
					Widgets.Label(rect4, "ScenPart_PlanetLayerConnections_Fuel".Translate());
				}
				Widgets.TextFieldNumeric<float>(rect3.RightHalf(), ref other.fuelCost, ref this.numericBuffer, 0f, 1E+09f);
				row.y += ScenPart.RowHeight + 4f;
			}
			List<string> unusedTags = this.GetUnusedTags();
			using (new TextBlock(unusedTags.Any<string>() ? Color.white : Color.gray))
			{
				if (Widgets.ButtonText(row, "ScenPart_PlanetLayerConnections_Add".Translate(), true, true, unusedTags.Any<string>(), null))
				{
					FloatMenuUtility.MakeMenu<string>(unusedTags, (string id) => id, (string nTag) => delegate()
					{
						this.connections.Add(new LayerConnection
						{
							tag = nTag
						});
					});
				}
			}
		}

		// Token: 0x0600E460 RID: 58464 RVA: 0x0042BA80 File Offset: 0x00429C80
		private void Seperator(ref Rect row)
		{
			Widgets.DrawLineHorizontal(row.x + 4f, row.y + row.height / 4f, row.width - 8f, Color.gray);
			row.y += 17f;
		}

		// Token: 0x0600E461 RID: 58465 RVA: 0x0042BAD4 File Offset: 0x00429CD4
		private List<string> GetUnusedTags()
		{
			ScenPart_PlanetLayer.tmpTags.Clear();
			foreach (ScenPart scenPart in Find.Scenario.AllParts)
			{
				if (scenPart != this)
				{
					ScenPart_PlanetLayer other = scenPart as ScenPart_PlanetLayer;
					if (other != null && !string.IsNullOrEmpty(other.tag) && !this.connections.Any((LayerConnection t) => t.tag == other.tag))
					{
						ScenPart_PlanetLayer.tmpTags.Add(other.tag);
					}
				}
			}
			return ScenPart_PlanetLayer.tmpTags;
		}

		// Token: 0x0600E462 RID: 58466 RVA: 0x0042BB8C File Offset: 0x00429D8C
		private void ClearInvalidConnections()
		{
			for (int i = this.connections.Count - 1; i >= 0; i--)
			{
				LayerConnection layerConnection = this.connections[i];
				bool flag = false;
				foreach (ScenPart scenPart in Find.Scenario.AllParts)
				{
					if (this != scenPart)
					{
						ScenPart_PlanetLayer scenPart_PlanetLayer = scenPart as ScenPart_PlanetLayer;
						if (scenPart_PlanetLayer != null && scenPart_PlanetLayer.tag == layerConnection.tag)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					this.connections.RemoveAt(i);
				}
			}
		}

		// Token: 0x040098D2 RID: 39122
		public PlanetLayerDef layer;

		// Token: 0x040098D3 RID: 39123
		public PlanetLayerSettingsDef settingsDef;

		// Token: 0x040098D4 RID: 39124
		public bool hide;

		// Token: 0x040098D5 RID: 39125
		public string tag;

		// Token: 0x040098D6 RID: 39126
		public List<LayerConnection> connections = new List<LayerConnection>();

		// Token: 0x040098D7 RID: 39127
		private float lastHeight;

		// Token: 0x040098D8 RID: 39128
		private static readonly List<string> tmpTags = new List<string>();

		// Token: 0x040098D9 RID: 39129
		private string[] vBuffer;

		// Token: 0x040098DA RID: 39130
		private string radiusBuffer;

		// Token: 0x040098DB RID: 39131
		private string extraCameraAltitudeBuffer;

		// Token: 0x040098DC RID: 39132
		private string backgroundWorldCameraOffsetBuffer;

		// Token: 0x040098DD RID: 39133
		private string backgroundWorldCameraParallaxBuffer;

		// Token: 0x040098DE RID: 39134
		private string subdivisionsBuffer;

		// Token: 0x040098DF RID: 39135
		private string viewAngleBuffer;

		// Token: 0x040098E0 RID: 39136
		private string numericBuffer;
	}
}
