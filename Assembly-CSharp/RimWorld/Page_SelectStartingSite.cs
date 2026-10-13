using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020032A2 RID: 12962
	public class Page_SelectStartingSite : Page
	{
		// Token: 0x17002F08 RID: 12040
		// (get) Token: 0x060123D1 RID: 74705 RVA: 0x0055CCE4 File Offset: 0x0055AEE4
		public override string PageTitle
		{
			get
			{
				return "SelectStartingSite".TranslateWithBackup("SelectLandingSite");
			}
		}

		// Token: 0x17002F09 RID: 12041
		// (get) Token: 0x060123D2 RID: 74706 RVA: 0x0055CCFF File Offset: 0x0055AEFF
		public override Vector2 InitialSize
		{
			get
			{
				return Vector2.zero;
			}
		}

		// Token: 0x17002F0A RID: 12042
		// (get) Token: 0x060123D3 RID: 74707 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x060123D4 RID: 74708 RVA: 0x0055CD06 File Offset: 0x0055AF06
		public Page_SelectStartingSite()
		{
			this.absorbInputAroundWindow = false;
			this.shadowAlpha = 0f;
			this.preventCameraMotion = false;
		}

		// Token: 0x060123D5 RID: 74709 RVA: 0x0055CD27 File Offset: 0x0055AF27
		public override void PreOpen()
		{
			base.PreOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.Planet;
			Find.WorldInterface.Reset();
			((MainButtonWorker_ToggleWorld)MainButtonDefOf.World.Worker).resetViewNextTime = true;
		}

		// Token: 0x060123D6 RID: 74710 RVA: 0x0055CD60 File Offset: 0x0055AF60
		public override void PostOpen()
		{
			base.PostOpen();
			Find.GameInitData.ChooseRandomStartingTile();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.WorldCameraMovement, OpportunityType.Important);
			TutorSystem.Notify_Event("PageStart-SelectStartingSite");
			this.tutorialStartTilePatch = null;
			if (!TutorSystem.TutorialMode || Find.Tutor.activeLesson == null || Find.Tutor.activeLesson.Current == null || Find.Tutor.activeLesson.Current.Instruction != InstructionDefOf.ChooseLandingSite)
			{
				return;
			}
			Find.WorldCameraDriver.ResetAltitude();
			Find.WorldCameraDriver.Update();
			List<PlanetTile> list = new List<PlanetTile>();
			PlanetLayer layer = Find.GameInitData.startingTile.Layer;
			float[] array = new float[layer.TilesCount];
			Vector2 a = new Vector2((float)Screen.width / 2f, (float)Screen.height / 2f);
			float num = Vector2.Distance(a, Vector2.zero);
			for (int i = 0; i < layer.TilesCount; i++)
			{
				if (TutorSystem.AllowAction(Page_SelectStartingSite.GetActionStringForChoosingTile(layer[i])))
				{
					Page_SelectStartingSite.TmpTileVertices.Clear();
					layer.GetTileVertices(i, Page_SelectStartingSite.TmpTileVertices);
					Vector3 vector = Vector3.zero;
					for (int j = 0; j < Page_SelectStartingSite.TmpTileVertices.Count; j++)
					{
						vector += Page_SelectStartingSite.TmpTileVertices[j];
					}
					vector /= (float)Page_SelectStartingSite.TmpTileVertices.Count;
					Vector3 vector2 = Find.WorldCamera.WorldToScreenPoint(vector) / Prefs.UIScale;
					vector2.y = (float)UI.screenHeight - vector2.y;
					vector2.x = Mathf.Clamp(vector2.x, 0f, (float)UI.screenWidth);
					vector2.y = Mathf.Clamp(vector2.y, 0f, (float)UI.screenHeight);
					float num2 = 1f - Vector2.Distance(a, vector2) / num;
					Transform transform = Find.WorldCamera.transform;
					Vector3 normalized = (vector - transform.position).normalized;
					float num3 = Vector3.Dot(transform.forward, normalized);
					array[i] = num2 * num3;
				}
				else
				{
					array[i] = float.NegativeInfinity;
				}
			}
			for (int k = 0; k < 16; k++)
			{
				for (int l = 0; l < array.Length; l++)
				{
					list.Clear();
					layer.GetTileNeighbors(l, list);
					float num4 = array[l];
					if (num4 >= 0f)
					{
						for (int m = 0; m < list.Count; m++)
						{
							float num5 = array[list[m].tileId];
							if (num5 >= 0f)
							{
								num4 += num5;
							}
						}
						array[l] = num4 / (float)list.Count;
					}
				}
			}
			float num6 = float.NegativeInfinity;
			PlanetTile invalid = PlanetTile.Invalid;
			for (int n = 0; n < array.Length; n++)
			{
				if (array[n] > 0f && num6 < array[n])
				{
					num6 = array[n];
					invalid = new PlanetTile(n, layer);
				}
			}
			if (invalid.Valid)
			{
				this.tutorialStartTilePatch = new PlanetTile?(invalid);
			}
		}

		// Token: 0x060123D7 RID: 74711 RVA: 0x0055D084 File Offset: 0x0055B284
		private static string GetActionStringForChoosingTile(Tile tile)
		{
			StringBuilder stringBuilder = new StringBuilder("ChooseBiome");
			if (tile.Biomes.Count<BiomeDef>() > 1)
			{
				stringBuilder.Append('-').Append(string.Join<string>('-', from x in tile.Biomes
				select x.defName));
			}
			else
			{
				stringBuilder.Append('-').Append(tile.PrimaryBiome.defName);
			}
			StringBuilder stringBuilder2 = stringBuilder.Append('-').Append(tile.hilliness.ToString()).Append('-');
			Landmark landmark = tile.Landmark;
			stringBuilder2.Append(((landmark != null) ? landmark.def.defName : null) ?? "None");
			return stringBuilder.ToString();
		}

		// Token: 0x060123D8 RID: 74712 RVA: 0x0055D155 File Offset: 0x0055B355
		public override void PostClose()
		{
			base.PostClose();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x060123D9 RID: 74713 RVA: 0x0055D170 File Offset: 0x0055B370
		public override void DoWindowContents(Rect rect)
		{
			if (Find.WorldInterface.SelectedTile.Valid)
			{
				Find.GameInitData.startingTile = Find.WorldInterface.SelectedTile;
				return;
			}
			if (Find.WorldSelector.FirstSelectedObject != null)
			{
				Find.GameInitData.startingTile = Find.WorldSelector.FirstSelectedObject.Tile;
			}
		}

		// Token: 0x060123DA RID: 74714 RVA: 0x0055D1CC File Offset: 0x0055B3CC
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			Text.Anchor = TextAnchor.UpperCenter;
			base.DrawPageTitle(new Rect(0f, 5f, (float)UI.screenWidth, 300f));
			Text.Anchor = TextAnchor.UpperLeft;
			this.DoCustomBottomButtons();
			if (this.tutorialStartTilePatch != null)
			{
				Page_SelectStartingSite.TmpTileVertices.Clear();
				Find.WorldGrid.GetTileVertices(this.tutorialStartTilePatch.Value, Page_SelectStartingSite.TmpTileVertices);
				Vector3 a = Vector3.zero;
				for (int i = 0; i < Page_SelectStartingSite.TmpTileVertices.Count; i++)
				{
					a += Page_SelectStartingSite.TmpTileVertices[i];
				}
				Color color = GUI.color;
				GUI.color = Color.white;
				GenUI.DrawArrowPointingAtWorldspace(a / (float)Page_SelectStartingSite.TmpTileVertices.Count, Find.WorldCamera);
				GUI.color = color;
			}
		}

		// Token: 0x060123DB RID: 74715 RVA: 0x0055D2A4 File Offset: 0x0055B4A4
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			PlanetTile selectedTile = Find.WorldInterface.SelectedTile;
			if (!selectedTile.Valid)
			{
				if (Prefs.DevMode && !Find.WorldInterface.selector.AnyObjectOrTileSelected)
				{
					Messages.Message("Tile has been randomly selected (debug mode only)", MessageTypeDefOf.SilentInput, false);
					Find.WorldInterface.SelectedTile = TileFinder.RandomStartingTile();
					return true;
				}
				Messages.Message("MustSelectStartingSite".TranslateWithBackup("MustSelectLandingSite"), MessageTypeDefOf.RejectInput, false);
				return false;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (!TileFinder.IsValidTileForNewSettlement(selectedTile, stringBuilder, false))
				{
					Messages.Message(stringBuilder.ToString(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				return TutorSystem.AllowAction(Page_SelectStartingSite.GetActionStringForChoosingTile(selectedTile.Tile));
			}
		}

		// Token: 0x060123DC RID: 74716 RVA: 0x0055D36C File Offset: 0x0055B56C
		protected override void DoNext()
		{
			PlanetTile selTile = Find.WorldInterface.SelectedTile;
			SettlementProximityGoodwillUtility.CheckConfirmSettle(selTile, delegate
			{
				Find.GameInitData.startingTile = selTile;
				this.<>n__0();
			}, null, null);
		}

		// Token: 0x060123DD RID: 74717 RVA: 0x0055D3B0 File Offset: 0x0055B5B0
		private void DoCustomBottomButtons()
		{
			int num = 4;
			int num2;
			if (num >= 3 && (float)UI.screenWidth < 540f + (float)num * (Page.BottomButSize.x + 10f))
			{
				num2 = 2;
			}
			else
			{
				num2 = 1;
			}
			int num3 = Mathf.CeilToInt((float)num / (float)num2);
			float num4 = Page.BottomButSize.x * (float)num3 + 10f * (float)(num3 + 1);
			float num5 = (float)num2 * Page.BottomButSize.y + 10f * (float)(num2 + 1);
			Rect rect = new Rect(((float)UI.screenWidth - num4) / 2f, (float)UI.screenHeight - num5 - 4f, num4, num5);
			WorldInspectPane worldInspectPane = Find.WindowStack.WindowOfType<WorldInspectPane>();
			if (worldInspectPane != null && rect.x < InspectPaneUtility.PaneWidthFor(worldInspectPane) + 4f)
			{
				rect.x = InspectPaneUtility.PaneWidthFor(worldInspectPane) + 4f;
			}
			Widgets.DrawWindowBackground(rect);
			float num6 = rect.xMin + 10f;
			float num7 = rect.yMin + 10f;
			Text.Font = GameFont.Small;
			if ((Widgets.ButtonText(new Rect(num6, num7, Page.BottomButSize.x, Page.BottomButSize.y), "Back".Translate(), true, true, true, null) || KeyBindingDefOf.Cancel.KeyDownEvent) && this.CanDoBack())
			{
				this.DoBack();
			}
			num6 += Page.BottomButSize.x + 10f;
			if (Widgets.ButtonText(new Rect(num6, num7, Page.BottomButSize.x, Page.BottomButSize.y), "SelectRandomSite".Translate(), true, true, true, null))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				if (ModsConfig.OdysseyActive && Rand.Bool)
				{
					Find.WorldInterface.SelectedTile = TileFinder.RandomSettlementTileFor(Find.WorldGrid.Surface, Faction.OfPlayer, true, (PlanetTile x) => x.Tile.Landmark != null);
				}
				else
				{
					Find.WorldInterface.SelectedTile = TileFinder.RandomStartingTile();
				}
				Find.WorldCameraDriver.JumpTo(Find.WorldGrid.GetTileCenter(Find.WorldInterface.SelectedTile));
			}
			num6 += Page.BottomButSize.x + 10f;
			if (num2 == 2)
			{
				num6 = rect.xMin + 10f;
				num7 += Page.BottomButSize.y + 10f;
			}
			if (Widgets.ButtonText(new Rect(num6, num7, Page.BottomButSize.x, Page.BottomButSize.y), "WorldFactionsTab".Translate(), true, true, true, null))
			{
				Find.WindowStack.Add(new Dialog_FactionDuringLanding());
			}
			num6 += Page.BottomButSize.x + 10f;
			if (Widgets.ButtonText(new Rect(num6, num7, Page.BottomButSize.x, Page.BottomButSize.y), "Next".Translate(), true, true, true, null) && this.CanDoNext())
			{
				this.DoNext();
			}
			num6 += Page.BottomButSize.x + 10f;
			GenUI.AbsorbClicksInRect(rect);
		}

		// Token: 0x060123DE RID: 74718 RVA: 0x0055D6F7 File Offset: 0x0055B8F7
		public override void OnAcceptKeyPressed()
		{
			if (this.CanDoNext())
			{
				this.DoNext();
			}
		}

		// Token: 0x0400BDCC RID: 48588
		private const float GapBetweenBottomButtons = 10f;

		// Token: 0x0400BDCD RID: 48589
		private const float UseTwoRowsIfScreenWidthBelowBase = 540f;

		// Token: 0x0400BDCE RID: 48590
		private static readonly List<Vector3> TmpTileVertices = new List<Vector3>();

		// Token: 0x0400BDCF RID: 48591
		private PlanetTile? tutorialStartTilePatch;
	}
}
