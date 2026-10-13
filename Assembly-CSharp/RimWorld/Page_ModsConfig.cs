using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Steamworks;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x0200328C RID: 12940
	[StaticConstructorOnStartup]
	public class Page_ModsConfig : Page
	{
		// Token: 0x17002F00 RID: 12032
		// (get) Token: 0x0601233D RID: 74557 RVA: 0x005583B9 File Offset: 0x005565B9
		private bool ControlIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
			}
		}

		// Token: 0x17002F01 RID: 12033
		// (get) Token: 0x0601233E RID: 74558 RVA: 0x005583D3 File Offset: 0x005565D3
		private bool ShiftIsHeld
		{
			get
			{
				return Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
			}
		}

		// Token: 0x17002F02 RID: 12034
		// (get) Token: 0x0601233F RID: 74559 RVA: 0x005583ED File Offset: 0x005565ED
		private List<ModMetaData> DraggedMods
		{
			get
			{
				if (!ReorderableWidget.Dragging)
				{
					return null;
				}
				if (this.selectedMods.Contains(this.draggedMod))
				{
					return this.selectedMods;
				}
				return new List<ModMetaData>
				{
					this.draggedMod
				};
			}
		}

		// Token: 0x17002F03 RID: 12035
		// (get) Token: 0x06012340 RID: 74560 RVA: 0x00558423 File Offset: 0x00556623
		protected override float Margin
		{
			get
			{
				return 25f;
			}
		}

		// Token: 0x06012341 RID: 74561 RVA: 0x0055842C File Offset: 0x0055662C
		public Page_ModsConfig()
		{
			this.doCloseX = true;
			this.closeOnCancel = true;
		}

		// Token: 0x06012342 RID: 74562 RVA: 0x005584DC File Offset: 0x005566DC
		public override void PreOpen()
		{
			base.PreOpen();
			ModLister.RebuildModList();
			this.quickSearchWidget.Reset();
			this.modListsDirty = true;
			this.primarySelectedMod = this.ModListsInOrder().Item1.FirstOrDefault<ModMetaData>();
			this.activeModsWhenOpenedHash = ModLister.InstalledModsListHash(true);
			Page_ModsConfig.originalModList = ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>();
			this.RecacheSelectedModInfo();
		}

		// Token: 0x06012343 RID: 74563 RVA: 0x00558540 File Offset: 0x00556740
		public override void PostClose()
		{
			if (this.saveChanges)
			{
				ModsConfig.Save();
			}
			else if (this.discardChanges)
			{
				this.DiscardChanges();
			}
			else
			{
				Log.Warning("Page_ModsConfig closed without saving or discarding, discarding anyway...");
				this.DiscardChanges();
			}
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder)
			{
				modMetaData.UnsetPreviewImage();
			}
			Resources.UnloadUnusedAssets();
			if (this.saveChanges && this.activeModsWhenOpenedHash != ModLister.InstalledModsListHash(true))
			{
				ModsConfig.RestartFromChangedMods();
			}
		}

		// Token: 0x06012344 RID: 74564 RVA: 0x005585DC File Offset: 0x005567DC
		public override bool OnCloseRequest()
		{
			if (this.activeModsWhenOpenedHash == ModLister.InstalledModsListHash(true))
			{
				this.discardChanges = true;
			}
			if (this.saveChanges || this.discardChanges)
			{
				return true;
			}
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("DiscardModChanges".Translate(), delegate()
			{
				this.discardChanges = true;
				this.Close(true);
			}, false, null, WindowLayer.Dialog));
			return false;
		}

		// Token: 0x06012345 RID: 74565 RVA: 0x0055863C File Offset: 0x0055683C
		private ValueTuple<List<ModMetaData>, List<ModMetaData>> ModListsInOrder()
		{
			if (this.modListsDirty)
			{
				Page_ModsConfig.activeModListOrderCached.Clear();
				Page_ModsConfig.inactiveModListOrderCached.Clear();
				Page_ModsConfig.filteredActiveModListOrderCached.Clear();
				Page_ModsConfig.filteredInactiveModListOrderCached.Clear();
				Page_ModsConfig.activeModListOrderCached.AddRange(ModsConfig.ActiveModsInLoadOrder);
				Page_ModsConfig.inactiveModListOrderCached.AddRange(from mod in ModLister.AllInstalledMods
				where !mod.Active
				orderby mod.Official descending, mod.ShortName
				select mod);
				Page_ModsConfig.filteredActiveModListOrderCached.AddRange((from mod in Page_ModsConfig.activeModListOrderCached
				where mod.Name.ToLower().Contains(this.filter.ToLower())
				select mod).ToList<ModMetaData>());
				Page_ModsConfig.filteredInactiveModListOrderCached.AddRange((from mod in Page_ModsConfig.inactiveModListOrderCached
				where mod.Name.ToLower().Contains(this.filter.ToLower())
				select mod).ToList<ModMetaData>());
				this.modListsDirty = false;
			}
			return new ValueTuple<List<ModMetaData>, List<ModMetaData>>(Page_ModsConfig.filteredActiveModListOrderCached, Page_ModsConfig.filteredInactiveModListOrderCached);
		}

		// Token: 0x06012346 RID: 74566 RVA: 0x00558768 File Offset: 0x00556968
		public override void DoWindowContents(Rect rect)
		{
			if (this.pendingReorders.Count > 0)
			{
				this.DoPendingReorder();
			}
			ValueTuple<List<ModMetaData>, List<ModMetaData>> valueTuple = this.ModListsInOrder();
			List<ModMetaData> item = valueTuple.Item1;
			List<ModMetaData> item2 = valueTuple.Item2;
			if (Event.current.type == EventType.KeyDown)
			{
				if (Event.current.keyCode == KeyCode.DownArrow)
				{
					this.OnDownKey(this.primarySelectedMod.Active ? item : item2);
					Event.current.Use();
				}
				if (Event.current.keyCode == KeyCode.UpArrow)
				{
					this.OnUpKey(this.primarySelectedMod.Active ? item : item2);
					Event.current.Use();
				}
			}
			float num = 0f;
			Widgets.BeginGroup(rect);
			this.quickSearchWidget.OnGUI(new Rect(0f, num, 354f, 30f), null, null);
			if (this.quickSearchWidget.filter.Text != this.filter)
			{
				this.modListsDirty = true;
			}
			this.filter = this.quickSearchWidget.filter.Text;
			if (Widgets.ButtonText(new Rect(358f, num, 150f, 30f), "ResolveModOrder".Translate(), true, true, true, null))
			{
				ModsConfig.TrySortMods();
				this.modListsDirty = true;
			}
			num += 40f;
			Widgets.Label(new Rect(0f, num, 250f, 30f), "Disabled".Translate());
			Widgets.Label(new Rect(254f, num, 250f, 30f), "Enabled".Translate());
			num += 30f;
			float height = rect.height - num - 30f - 10f;
			Rect modListArea = new Rect(254f, num, 250f, height);
			Rect modListArea2 = new Rect(0f, num, 250f, height);
			this.DoModList(modListArea2, item2, ref this.inactiveModListScrollPosition, ref this.inactiveGroupId, delegate(int from, int to)
			{
			});
			this.DoModList(modListArea, Page_ModsConfig.filteredActiveModListOrderCached, ref this.activeModListScrollPosition, ref this.activeGroupId, new Action<int, int>(this.onReorderedAction));
			ReorderableWidget.NewMultiGroup(new List<int>
			{
				this.activeGroupId,
				this.inactiveGroupId
			}, new Action<int, int, int, int>(this.onMultiReorderedAction));
			this.DoBottomButtons(new Rect(0f, modListArea.yMax + 10f, 508f, 30f));
			Rect r = new Rect(525f, 0f, rect.width - 508f - 17f, rect.height);
			if (this.primarySelectedMod != null)
			{
				this.DoModInfo(r, this.primarySelectedMod);
			}
			if (Widgets.ButtonText(new Rect(r.xMax - 200f, r.yMax - 30f, 200f, 30f), "SaveModChanges".Translate(), true, true, true, null))
			{
				this.saveChanges = true;
				this.Close(true);
			}
			Widgets.EndGroup();
			this.draggedMod = null;
			if (ReorderableWidget.GetDraggedIndex >= 0)
			{
				List<ModMetaData> list = (ReorderableWidget.GetDraggedFromGroupID == this.activeGroupId) ? item : item2;
				int getDraggedIndex = ReorderableWidget.GetDraggedIndex;
				if (!list.NullOrEmpty<ModMetaData>() && list.Count > getDraggedIndex)
				{
					this.draggedMod = list[getDraggedIndex];
				}
			}
			if (!this.DraggedMods.NullOrEmpty<ModMetaData>())
			{
				Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
				Find.WindowStack.ImmediateWindow(12312541, new Rect(mousePositionOnUIInverted.x - 125f, mousePositionOnUIInverted.y - 13f, 250f, 26f * (float)Math.Min(this.DraggedMods.Count, 30)), WindowLayer.Super, new Action(this.DoDraggedMods), false, false, 0f, null, false);
			}
			if (Widgets.ButtonInvisible(rect, false) && !this.ShiftIsHeld && !this.ControlIsHeld)
			{
				this.selectedMods.Clear();
				if (this.primarySelectedMod != null)
				{
					this.lastSelectedIndex = this.GetModIndex(this.primarySelectedMod);
					this.lastSelectedGroupId = (this.primarySelectedMod.Active ? this.activeGroupId : this.inactiveGroupId);
				}
				else
				{
					this.lastSelectedIndex = -1;
					this.lastSelectedGroupId = -1;
				}
			}
			int num2 = ModLister.InstalledModsListHash(true);
			if (this.activeModsHash == -1 || this.activeModsHash != num2)
			{
				Page_ModsConfig.modWarningsCached = ModsConfig.GetModWarnings();
				this.RecacheSelectedModInfo();
				this.activeModsHash = num2;
				this.modListsDirty = true;
			}
		}

		// Token: 0x06012347 RID: 74567 RVA: 0x00558C08 File Offset: 0x00556E08
		private void DoModList(Rect modListArea, List<ModMetaData> modList, ref Vector2 scrollPosition, ref int reorderableGroup, Action<int, int> reorderedAction)
		{
			bool flag = reorderableGroup == this.inactiveGroupId;
			float num = (float)modList.Count * 26f + 8f;
			if (flag)
			{
				num += (float)WorkshopItems.DownloadingItemsCount * 26f;
			}
			Rect viewRect = new Rect(0f, 0f, modListArea.width - 16f, num);
			Widgets.BeginScrollView(modListArea, ref scrollPosition, viewRect, true);
			float num2 = scrollPosition.y - 26f;
			float num3 = scrollPosition.y + modListArea.height;
			bool flag2 = (float)modList.Count * 26f > modListArea.height;
			if (Event.current.type == EventType.Repaint)
			{
				reorderableGroup = ReorderableWidget.NewGroup(reorderedAction, ReorderableDirection.Vertical, modListArea, -1f, null, true);
			}
			int num4 = 0;
			foreach (ModMetaData modMetaData in modList)
			{
				Rect rect = new Rect(0f, (float)num4 * 26f, 250f, 26f);
				if (flag2)
				{
					rect.width -= 16f;
				}
				ReorderableWidget.Reorderable(reorderableGroup, rect, false, false);
				if (rect.y >= num2 && rect.y <= num3)
				{
					this.DoModRow(rect, modMetaData, modList, num4, false);
				}
				if (modMetaData == this.pendingScrollToMod)
				{
					int num5 = (rect.y <= scrollPosition.y) ? -1 : 1;
					while (rect.y <= num2 || rect.y + 26f > num3)
					{
						scrollPosition.y += 26f * (float)num5;
						num2 = scrollPosition.y;
						num3 = scrollPosition.y + modListArea.height;
					}
					this.pendingScrollToMod = null;
				}
				num4++;
			}
			if (flag)
			{
				foreach (WorkshopItem_Downloading item in WorkshopItems.AllDownloadingItems)
				{
					Rect r = new Rect(0f, (float)num4 * 26f, 250f, 26f);
					if (flag2)
					{
						r.width -= 16f;
					}
					if (r.y >= num2 && r.y <= num3)
					{
						this.DoModRowDownloading(r, num4, item);
					}
					num4++;
				}
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06012348 RID: 74568 RVA: 0x00558E84 File Offset: 0x00557084
		private void DoModRow(Rect r, ModMetaData mod, List<ModMetaData> list, int index, bool isDragged = false)
		{
			if (!this.DraggedMods.NullOrEmpty<ModMetaData>() && this.DraggedMods.Contains(mod) && !isDragged)
			{
				return;
			}
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(r);
			}
			if (this.selectedMods.Contains(mod) && !ReorderableWidget.Dragging)
			{
				Widgets.DrawHighlightSelected(r);
			}
			Action clickAction = null;
			if (mod.Source == ContentSource.SteamWorkshop)
			{
				clickAction = delegate()
				{
					SteamUtility.OpenWorkshopPage(mod.GetPublishedFileId());
				};
			}
			if (mod.Source == ContentSource.ModsFolder && (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor))
			{
				clickAction = delegate()
				{
					Application.OpenURL(GenFilePaths.ModsFolderPath + "/" + mod.FolderName);
				};
			}
			ContentSourceUtility.DrawContentSource(r, mod.Source, clickAction);
			Rect rect = new Rect(r);
			rect.xMin += 28f;
			Texture2D texture2D = null;
			string text = "";
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			if (!mod.Active)
			{
				GUI.color = Color.grey;
			}
			if (!mod.VersionCompatible)
			{
				GUI.color = Color.yellow;
				texture2D = Page_ModsConfig.WarningIcon;
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				if (mod.MadeForNewerVersion)
				{
					text += "ModNotMadeForThisVersion_Newer".Translate();
				}
				else
				{
					text += "ModNotMadeForThisVersion".Translate();
				}
			}
			if (mod.Active && !Page_ModsConfig.modWarningsCached.NullOrEmpty<string, string>() && Page_ModsConfig.modWarningsCached.ContainsKey(mod.PackageId) && !Page_ModsConfig.modWarningsCached[mod.PackageId].NullOrEmpty())
			{
				GUI.color = Color.red;
				texture2D = Page_ModsConfig.ErrorIcon;
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += Page_ModsConfig.modWarningsCached[mod.PackageId];
			}
			Text.WordWrap = false;
			Text.Anchor = TextAnchor.MiddleLeft;
			if (texture2D != null)
			{
				float num = rect.width - 32f;
				if (Text.CalcSize(mod.ShortName).x > num)
				{
					text = mod.ShortName.Colorize(Color.yellow) + "\n\n" + text;
				}
				string label = mod.ShortName.Truncate(num, this.truncatedStringCache);
				Widgets.LabelWithIcon(rect, label, texture2D, 0.75f);
			}
			else
			{
				if (Text.CalcSize(mod.ShortName).x > rect.width)
				{
					text = mod.ShortName.Colorize(Color.yellow) + "\n\n" + text;
				}
				string label2 = mod.ShortName.Truncate(rect.width, this.truncatedStringCache);
				Widgets.Label(rect, label2);
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			if (isDragged)
			{
				return;
			}
			if (!ReorderableWidget.Dragging)
			{
				Widgets.DrawHighlightIfMouseover(rect);
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				if (this.ControlIsHeld)
				{
					if (this.selectedMods.Count > 0 && this.selectedMods[0].Active != mod.Active)
					{
						this.selectedMods.Clear();
					}
					if (this.ShiftIsHeld)
					{
						for (int num2 = this.lastSelectedIndex; num2 != index; num2 += Math.Sign(index - this.lastSelectedIndex))
						{
							this.selectedMods.Add(list[num2]);
						}
						this.selectedMods.Add(mod);
						this.primarySelectedMod = mod;
						this.RecacheSelectedModInfo();
					}
					else if (this.selectedMods.Count > 1 && this.selectedMods.Contains(mod))
					{
						this.selectedMods.Remove(mod);
						if (mod == this.primarySelectedMod)
						{
							this.primarySelectedMod = this.selectedMods[0];
							this.RecacheSelectedModInfo();
						}
						this.lastSelectedIndex = this.GetModIndex(this.primarySelectedMod);
						this.lastSelectedGroupId = (this.primarySelectedMod.Active ? this.activeGroupId : this.inactiveGroupId);
					}
					else
					{
						this.primarySelectedMod = mod;
						this.RecacheSelectedModInfo();
						this.selectedMods.Add(this.primarySelectedMod);
						this.lastSelectedIndex = index;
						this.lastSelectedGroupId = (this.primarySelectedMod.Active ? this.activeGroupId : this.inactiveGroupId);
					}
				}
				else
				{
					this.primarySelectedMod = mod;
					int num3 = mod.Active ? this.activeGroupId : this.inactiveGroupId;
					this.RecacheSelectedModInfo();
					if (this.ShiftIsHeld && this.lastSelectedIndex >= 0 && this.lastSelectedGroupId == num3)
					{
						this.selectedMods.Clear();
						for (int num4 = this.lastSelectedIndex; num4 != index; num4 += Math.Sign(index - this.lastSelectedIndex))
						{
							this.selectedMods.Add(list[num4]);
						}
						this.selectedMods.Add(mod);
					}
					else
					{
						this.selectedMods.Clear();
						this.selectedMods.Add(this.primarySelectedMod);
						this.lastSelectedIndex = index;
						this.lastSelectedGroupId = (this.primarySelectedMod.Active ? this.activeGroupId : this.inactiveGroupId);
					}
				}
				if (this.lastClickedMod == mod && (double)RealTime.LastRealTime < this.lastClickTime + 0.5)
				{
					using (List<ModMetaData>.Enumerator enumerator = this.selectedMods.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ModMetaData modMetaData = enumerator.Current;
							if (modMetaData.Active)
							{
								this.TrySetModInactive(modMetaData);
							}
							else
							{
								this.TrySetModActive(modMetaData);
							}
						}
						goto IL_61F;
					}
				}
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				IL_61F:
				this.lastClickTime = (double)RealTime.LastRealTime;
				this.lastClickedMod = mod;
			}
			if (!text.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, new TipSignal(text, mod.GetHashCode() * 3311));
			}
		}

		// Token: 0x06012349 RID: 74569 RVA: 0x00559500 File Offset: 0x00557700
		private void DoModRowDownloading(Rect r, int index, WorkshopItem_Downloading item)
		{
			ContentSourceUtility.DrawContentSource(r, ContentSource.SteamWorkshop, null);
			r.xMin += 28f;
			float height = r.height;
			r.xMax -= height;
			Text.WordWrap = false;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(r, "Downloading".Translate() + GenText.MarchingEllipsis(0f));
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			Rect rect = new Rect(r.xMax, r.y, height, height);
			GUI.DrawTexture(rect, Widgets.CheckboxOffTex);
			Widgets.DrawHighlightIfMouseover(rect);
			if (Widgets.ButtonInvisible(rect, true))
			{
				Workshop.Unsubscribe(item.PublishedFileId);
			}
			TooltipHandler.TipRegion(r, string.Format("{0}: {1}", "WorkshopId".Translate(), item.PublishedFileId.ToString()));
		}

		// Token: 0x0601234A RID: 74570 RVA: 0x005595E8 File Offset: 0x005577E8
		private void DoModInfo(Rect r, ModMetaData mod)
		{
			Widgets.BeginGroup(r);
			float num = r.y;
			Rect position = new Rect(0f, num, 0f, 20f);
			Texture2D previewImage = mod.PreviewImage;
			if (previewImage != null)
			{
				position.width = Mathf.Min((float)previewImage.width, r.width);
				position.height = (float)previewImage.height * (position.width / (float)previewImage.width);
				float num2 = Mathf.Ceil(r.height * 0.35f);
				if (position.height > num2)
				{
					float height = position.height;
					position.height = num2;
					position.width *= position.height / height;
				}
				position.x = r.width / 2f - position.width / 2f;
				GUI.DrawTexture(position, previewImage, ScaleMode.ScaleToFit);
				num += position.height + 10f;
			}
			float num3 = num;
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(0f, num3, r.width, 40f);
			Widgets.Label(rect, mod.Name.Truncate(rect.width, null));
			Text.Font = GameFont.Small;
			num3 += rect.height;
			if (!mod.AuthorsString.NullOrEmpty())
			{
				Rect rect2 = new Rect(0f, num3, r.width / 2f, Text.LineHeight);
				string text = ("Author".Translate() + ": ").Colorize(Color.gray) + mod.AuthorsString;
				Widgets.Label(rect2, text.Truncate(r.width / 2f, null));
				if (Text.CalcSize(text).x > rect2.width)
				{
					Widgets.DrawHighlightIfMouseover(rect2);
					TooltipHandler.TipRegion(rect2, new TipSignal(text));
				}
				num3 += rect2.height;
			}
			Rect rect3 = new Rect(0f, num3, r.width / 2f, Text.LineHeight);
			string text2 = ("ModPackageId".Translate() + ": ").Colorize(Color.gray) + mod.packageIdLowerCase;
			Widgets.Label(rect3, text2.Truncate(r.width / 2f, null));
			if (Text.CalcSize(text2).x > rect3.width)
			{
				Widgets.DrawHighlightIfMouseover(rect3);
				TooltipHandler.TipRegion(rect3, new TipSignal(text2));
			}
			num3 += rect3.height;
			if (!mod.IsCoreMod)
			{
				Rect rect4 = new Rect(0f, num3, r.width / 2f, Text.LineHeight);
				Widgets.Label(rect4, ("ModTargetVersion".Translate() + ": ").Colorize(Color.gray) + mod.SupportedVersionsReadOnly.Select(delegate(System.Version v)
				{
					string text3 = VersionControl.IsCompatible(v) ? "<color=green>" : "<color=red>";
					string text4 = "</color>";
					if (v.Build > 0)
					{
						return string.Format("{0}{1}.{2}.{3}{4}", new object[]
						{
							text3,
							v.Major.ToString(),
							v.Minor.ToString(),
							v.Build.ToString(),
							text4
						});
					}
					return string.Format("{0}{1}.{2}{3}", new object[]
					{
						text3,
						v.Major.ToString(),
						v.Minor.ToString(),
						text4
					});
				}).ToCommaList(false, false));
				num3 += rect4.height;
			}
			if (!mod.ModVersion.NullOrEmpty())
			{
				Rect rect5 = new Rect(0f, num3, r.width / 2f, Text.LineHeight);
				Widgets.Label(rect5, ("ModVersion".Translate() + ": ").Colorize(Color.gray) + mod.ModVersion);
				num3 += rect5.height;
			}
			float num4 = rect.yMax;
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			if (SteamManager.Initialized && mod.OnSteamWorkshop)
			{
				Action <>9__8;
				list.Add(new FloatMenuOption("Unsubscribe".Translate(), delegate()
				{
					WindowStack windowStack = Find.WindowStack;
					TaggedString text3 = "ConfirmUnsubscribeFrom".Translate(mod.Name);
					Action confirmedAct;
					if ((confirmedAct = <>9__8) == null)
					{
						confirmedAct = (<>9__8 = delegate()
						{
							mod.enabled = false;
							Workshop.Unsubscribe(mod);
						});
					}
					windowStack.Add(Dialog_MessageBox.CreateConfirmation(text3, confirmedAct, true, null, WindowLayer.Dialog));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list.Add(new FloatMenuOption("WorkshopPage".Translate(), delegate()
				{
					SteamUtility.OpenWorkshopPage(mod.GetPublishedFileId());
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				if (!mod.Official && (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor))
				{
					list.Add(new FloatMenuOption("ModFolder".Translate(), delegate()
					{
						Application.OpenURL(mod.RootDir.FullName);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			else
			{
				if (!mod.Url.NullOrEmpty())
				{
					list.Add(new FloatMenuOption("ModWebsite".Translate(), delegate()
					{
						Application.OpenURL(mod.Url);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				if (!mod.Official && (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor))
				{
					list.Add(new FloatMenuOption("ModFolder".Translate(), delegate()
					{
						Application.OpenURL(GenFilePaths.ModsFolderPath + "/" + mod.FolderName);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
			}
			if (this.primaryModHandle != null && !this.primaryModHandle.SettingsCategory().NullOrEmpty())
			{
				list.Add(new FloatMenuOption("ModOptions".Translate(), delegate()
				{
					Find.WindowStack.Add(new Dialog_ModSettings(this.primaryModHandle));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			if (Prefs.DevMode && SteamManager.Initialized && mod.CanToUploadToWorkshop())
			{
				Action <>9__10;
				Action <>9__9;
				list.Add(new FloatMenuOption(Workshop.UploadButtonLabel(mod.GetPublishedFileId()), delegate()
				{
					ModLoadFolders loadFolders = mod.loadFolders;
					List<string> list2 = (loadFolders != null) ? loadFolders.GetIssueList(mod) : null;
					if (mod.HadIncorrectlyFormattedVersionInMetadata)
					{
						Messages.Message("MessageModNeedsWellFormattedTargetVersion".Translate(VersionControl.CurrentMajor.ToString() + "." + VersionControl.CurrentMinor.ToString()), MessageTypeDefOf.RejectInput, false);
						return;
					}
					if (mod.HadIncorrectlyFormattedPackageId)
					{
						Find.WindowStack.Add(new Dialog_MessageBox("MessageModNeedsWellFormattedPackageId".Translate(), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
						return;
					}
					if (!list2.NullOrEmpty<string>())
					{
						Find.WindowStack.Add(new Dialog_MessageBox("ModHadLoadFolderIssues".Translate() + "\n" + list2.ToLineList("  - "), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
						return;
					}
					WindowStack windowStack = Find.WindowStack;
					ModMetaData mod2 = mod;
					Action acceptAction;
					if ((acceptAction = <>9__9) == null)
					{
						acceptAction = (<>9__9 = delegate()
						{
							SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
							TaggedString text3 = "ConfirmContentAuthor".Translate();
							Action confirmedAct;
							if ((confirmedAct = <>9__10) == null)
							{
								confirmedAct = (<>9__10 = delegate()
								{
									SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
									Workshop.Upload(mod);
								});
							}
							Dialog_MessageBox dialog_MessageBox = Dialog_MessageBox.CreateConfirmation(text3, confirmedAct, true, null, WindowLayer.Dialog);
							dialog_MessageBox.buttonAText = "Yes".Translate();
							dialog_MessageBox.buttonBText = "No".Translate();
							dialog_MessageBox.interactionDelay = 6f;
							Find.WindowStack.Add(dialog_MessageBox);
						});
					}
					windowStack.Add(new Dialog_ConfirmModUpload(mod2, acceptAction));
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
			}
			WidgetRow widgetRow = new WidgetRow(r.width, num4, UIDirection.LeftThenDown, r.width / 2f, 4f);
			if (list.Count > 0 && widgetRow.ButtonText("MoreActions".Translate(), null, true, true, true, null))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				Find.WindowStack.Add(new FloatMenu(list));
			}
			if (widgetRow.ButtonText(mod.Active ? "Disable".Translate() : "Enable".Translate(), null, true, true, true, null))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				if (mod.Active)
				{
					this.TrySetModInactive(mod);
				}
				else
				{
					this.TrySetModActive(mod);
				}
				this.selectedMods.Clear();
				this.selectedMods.Add(this.primarySelectedMod);
			}
			num4 = widgetRow.FinalY + 25f;
			if (this.anyReqsCached)
			{
				Text.Anchor = TextAnchor.MiddleRight;
				TaggedString taggedString = "ModDisplayFulfilledRequirements".Translate();
				float num5 = Text.CalcSize(taggedString).x + 24f + 4f;
				Rect rect6 = new Rect(r.width - num5, num4, num5, 24f);
				bool flag = this.displayFullfilledRequirements;
				Widgets.CheckboxLabeled(rect6, taggedString, ref this.displayFullfilledRequirements, false, null, null, false, false);
				if (flag != this.displayFullfilledRequirements)
				{
					this.RecacheSelectedModInfo();
				}
				num4 += 34f;
				Text.Anchor = TextAnchor.UpperLeft;
			}
			num = Math.Max(num3, num4) + 10f;
			Rect outRect = new Rect(0f, num, r.width, r.height - num - 30f - 10f);
			float width = outRect.width - 16f;
			float num6 = Text.CalcHeight(mod.Description, width);
			num6 = Mathf.Min(num6 * 1.25f, num6 + 200f);
			Rect viewRect = new Rect(0f, 0f, width, num6 + this.modRequirementsHeightCached + (this.anyReqsInfoToShowCached ? 10f : 0f));
			float num7 = (viewRect.height > outRect.height) ? 16f : 0f;
			Widgets.BeginScrollView(outRect, ref this.modDescriptionScrollPosition, viewRect, true);
			float num8 = 0f;
			if (this.anyReqsInfoToShowCached)
			{
				num8 = this.DoRequirementSection(r.width - num7);
				num8 += 10f;
			}
			Widgets.Label(new Rect(0f, num8, viewRect.width - num7, viewRect.height - num8), WebUtility.HtmlDecode(WebUtility.HtmlDecode(mod.Description)));
			Widgets.EndScrollView();
			Widgets.EndGroup();
		}

		// Token: 0x0601234B RID: 74571 RVA: 0x00559EBC File Offset: 0x005580BC
		private void DoDraggedMods()
		{
			if (this.DraggedMods.NullOrEmpty<ModMetaData>())
			{
				return;
			}
			float num = 0f;
			int num2 = 0;
			Widgets.DrawWindowBackground(new Rect(0f, 0f, 250f, 26f * (float)Math.Min(this.DraggedMods.Count, 30)));
			foreach (ModMetaData mod in this.DraggedMods)
			{
				if (num2 > 30)
				{
					break;
				}
				Rect r = new Rect(0f, num, 250f, 26f);
				this.DoModRow(r, mod, null, num2, true);
				num += r.height;
				num2++;
			}
		}

		// Token: 0x0601234C RID: 74572 RVA: 0x00559F88 File Offset: 0x00558188
		private void DoBottomButtons(Rect r)
		{
			ValueTuple<List<ModMetaData>, List<ModMetaData>> valueTuple = this.ModListsInOrder();
			List<ModMetaData> filteredInactiveMods = valueTuple.Item2;
			Widgets.BeginGroup(r);
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenDown, 99999f, 4f);
			if (widgetRow.ButtonText("GetMods".Translate(), null, true, true, true, new float?(150f)))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				list.Add(new FloatMenuOption("FromWorkshop".Translate(), delegate()
				{
					SteamUtility.OpenSteamWorkshopPage();
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				list.Add(new FloatMenuOption("FromForum".Translate(), delegate()
				{
					Application.OpenURL("http://rimworldgame.com/getmods");
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				List<FloatMenuOption> options = list;
				Find.WindowStack.Add(new FloatMenu(options));
			}
			if (widgetRow.ButtonText("UnsubscribeMultiple".Translate(), null, true, true, true, new float?(150f)))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				List<FloatMenuOption> options2 = new List<FloatMenuOption>
				{
					new FloatMenuOption("AllSelectedMods".Translate(), delegate()
					{
						this.UnsubscribeMods(this.selectedMods);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
					new FloatMenuOption("AllIncompatibleMods".Translate(), delegate()
					{
						this.UnsubscribeMods(from mod in filteredInactiveMods
						where !mod.Official && !mod.VersionCompatible
						select mod);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
					new FloatMenuOption("AllDisabledMods".Translate(), delegate()
					{
						this.UnsubscribeMods(from mod in filteredInactiveMods
						where !mod.Official
						select mod);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
				};
				Find.WindowStack.Add(new FloatMenu(options2));
			}
			if (widgetRow.ButtonText("SaveLoadList".Translate(), null, true, true, true, new float?(150f)))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				Action<ModList> <>9__9;
				List<FloatMenuOption> options3 = new List<FloatMenuOption>
				{
					new FloatMenuOption("SaveModList".Translate(), delegate()
					{
						this.SaveModList(Page_ModsConfig.activeModListOrderCached);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0),
					new FloatMenuOption("LoadModList".Translate(), delegate()
					{
						WindowStack windowStack = Find.WindowStack;
						Action<ModList> modListReturner;
						if ((modListReturner = <>9__9) == null)
						{
							modListReturner = (<>9__9 = delegate(ModList modList)
							{
								this.LoadModList(modList);
							});
						}
						windowStack.Add(new Dialog_ModList_Load(modListReturner));
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
				};
				Find.WindowStack.Add(new FloatMenu(options3));
			}
			Widgets.EndGroup();
		}

		// Token: 0x0601234D RID: 74573 RVA: 0x0055A23C File Offset: 0x0055843C
		private float DoRequirementSection(float width)
		{
			float num = 0f;
			if (this.visibleReqsCached.Count > 0 || this.anyOrderingIssuesCached)
			{
				object obj = this.anyUnfulfilledReqsCached || this.anyOrderingIssuesCached;
				Rect rect = new Rect(0f, 0f, width, this.modRequirementsHeightCached);
				object obj2 = obj;
				if (obj2 != null)
				{
					Widgets.DrawBoxSolid(rect, Page_ModsConfig.UnmetRequirementBoxBGColor);
				}
				GUI.color = ((obj2 != null) ? Page_ModsConfig.UnmetRequirementBoxOutlineColor : Page_ModsConfig.RequirementBoxOutlineColor);
				Widgets.DrawBox(rect, 1, null);
				GUI.color = Color.white;
				num += 10f;
				Text.Anchor = TextAnchor.MiddleLeft;
				for (int i = 0; i < this.visibleReqsCached.Count; i++)
				{
					Rect entryRect = new Rect(11f, num, width - 20f - 1f, 26f);
					this.DrawRequirementEntry(this.visibleReqsCached[i], entryRect, ref num);
					if (i < this.visibleReqsCached.Count - 1)
					{
						num += 4f;
					}
				}
				if (this.anyOrderingIssuesCached)
				{
					num += 4f;
					Widgets.Label(new Rect(10f, num, width - 20f, Text.LineHeight * 2f), "ModOrderingWarning".Translate());
					num += Text.LineHeight * 2f;
				}
				num += 10f;
			}
			Text.Anchor = TextAnchor.UpperLeft;
			return num;
		}

		// Token: 0x0601234E RID: 74574 RVA: 0x0055A390 File Offset: 0x00558590
		private void DrawRequirementEntry(ModRequirement entry, Rect entryRect, ref float y)
		{
			Widgets.DrawBoxSolid(entryRect, entry.IsSatisfied ? Page_ModsConfig.RequirementRowColor : (Mouse.IsOver(entryRect) ? Page_ModsConfig.UnmetRequirementRowColorHighlighted : Page_ModsConfig.UnmetRequirementRowColor));
			Rect rect = entryRect;
			rect.x += 4f;
			rect.width = 150f;
			Widgets.Label(rect, entry.RequirementTypeLabel.Truncate(rect.width, this.truncatedStringCache));
			Rect rect2 = entryRect;
			rect2.x = rect.xMax + 4f;
			rect2.width -= rect2.x + 24f + 8f;
			Text.WordWrap = false;
			Widgets.Label(rect2, entry.displayName.Truncate(rect2.width, this.truncatedStringCache));
			Text.WordWrap = true;
			if (Widgets.ButtonInvisible(entryRect, true))
			{
				entry.OnClicked(this);
			}
			GUI.DrawTexture(new Rect
			{
				xMin = entryRect.xMax - 24f - 4f,
				y = entryRect.y + 1f,
				width = 24f,
				height = 24f
			}, entry.StatusIcon);
			TooltipHandler.TipRegion(entryRect, new TipSignal(entry.Tooltip));
			y += 26f;
		}

		// Token: 0x0601234F RID: 74575 RVA: 0x0055A4EC File Offset: 0x005586EC
		private void RecacheSelectedModInfo()
		{
			this.anyReqsCached = false;
			this.anyReqsInfoToShowCached = false;
			this.anyUnfulfilledReqsCached = false;
			this.anyOrderingIssuesCached = false;
			this.visibleReqsCached.Clear();
			this.primaryModHandle = null;
			if (this.primarySelectedMod != null)
			{
				if (!this.primarySelectedMod.Official)
				{
					this.primaryModHandle = LoadedModManager.ModHandles.FirstOrDefault((Mod p) => this.primarySelectedMod.SamePackageId(p.Content.PackageId, false));
				}
				foreach (ModRequirement modRequirement in (from r in this.primarySelectedMod.GetRequirements()
				orderby r.IsSatisfied, r.RequirementTypeLabel
				select r).ToList<ModRequirement>())
				{
					bool isSatisfied = modRequirement.IsSatisfied;
					if (!isSatisfied || this.displayFullfilledRequirements)
					{
						this.visibleReqsCached.Add(modRequirement);
						if (!isSatisfied)
						{
							this.anyUnfulfilledReqsCached = true;
						}
					}
					this.anyReqsCached = true;
					this.anyReqsInfoToShowCached = true;
				}
				this.anyOrderingIssuesCached = ModsConfig.ModHasAnyOrderingIssues(this.primarySelectedMod);
				if (this.visibleReqsCached.Any<ModRequirement>() || this.anyOrderingIssuesCached)
				{
					this.anyReqsInfoToShowCached = true;
					this.modRequirementsHeightCached = (float)this.visibleReqsCached.Count * 26f + (float)(this.visibleReqsCached.Count - 1) * 4f + 20f + 1f;
					if (this.anyOrderingIssuesCached)
					{
						this.modRequirementsHeightCached += Text.LineHeight * 2f + 4f;
						return;
					}
				}
				else
				{
					this.modRequirementsHeightCached = 0f;
				}
			}
		}

		// Token: 0x06012350 RID: 74576 RVA: 0x0055A6BC File Offset: 0x005588BC
		public void SelectMod(ModMetaData mod)
		{
			this.primarySelectedMod = mod;
			this.lastSelectedIndex = this.GetModIndex(mod);
			this.lastSelectedGroupId = (mod.Active ? this.activeGroupId : this.inactiveGroupId);
			this.selectedMods.Clear();
			this.selectedMods.Add(this.primarySelectedMod);
			this.RecacheSelectedModInfo();
			this.pendingScrollToMod = mod;
		}

		// Token: 0x06012351 RID: 74577 RVA: 0x0055A724 File Offset: 0x00558924
		private void DoPendingReorder()
		{
			ModMetaData placeAfter = null;
			if (this.pendingReorderIndex > 0)
			{
				placeAfter = Page_ModsConfig.filteredActiveModListOrderCached[this.pendingReorderIndex - 1];
			}
			IEnumerable<ModMetaData> activeModsInLoadOrder = ModsConfig.ActiveModsInLoadOrder;
			Func<ModMetaData, bool> <>9__1;
			while (this.pendingReorders.Count > 0)
			{
				string modId = this.pendingReorders.Dequeue();
				int modIndex = activeModsInLoadOrder.FirstIndexOf((ModMetaData mod) => mod.PackageId == modId);
				if (this.pendingReorderIndex > 0 && placeAfter != null)
				{
					IEnumerable<ModMetaData> enumerable = activeModsInLoadOrder;
					Func<ModMetaData, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((ModMetaData m) => m.PackageId == placeAfter.PackageId));
					}
					this.pendingReorderIndex = enumerable.FirstIndexOf(predicate) + 1;
				}
				if (Page_ModsConfig.filteredActiveModListOrderCached.Count<ModMetaData>() == 0)
				{
					this.pendingReorderIndex = activeModsInLoadOrder.Count<ModMetaData>();
				}
				string text;
				if (!ModsConfig.TryReorder(modIndex, this.pendingReorderIndex, out text) && !string.IsNullOrWhiteSpace(text))
				{
					Messages.Message(text, null, MessageTypeDefOf.RejectInput, false);
				}
			}
		}

		// Token: 0x06012352 RID: 74578 RVA: 0x0055A820 File Offset: 0x00558A20
		private void OnDownKey(List<ModMetaData> currentList)
		{
			if (this.lastSelectedIndex + 1 < currentList.Count)
			{
				this.primarySelectedMod = currentList[this.lastSelectedIndex + 1];
				this.lastSelectedIndex++;
				if (!this.ShiftIsHeld)
				{
					this.selectedMods.Clear();
				}
				this.selectedMods.Add(this.primarySelectedMod);
				this.RecacheSelectedModInfo();
				this.pendingScrollToMod = this.primarySelectedMod;
			}
		}

		// Token: 0x06012353 RID: 74579 RVA: 0x0055A898 File Offset: 0x00558A98
		private void OnUpKey(List<ModMetaData> currentList)
		{
			if (this.lastSelectedIndex - 1 >= 0)
			{
				this.primarySelectedMod = currentList[this.lastSelectedIndex - 1];
				this.lastSelectedIndex--;
				if (!this.ShiftIsHeld)
				{
					this.selectedMods.Clear();
				}
				this.selectedMods.Add(this.primarySelectedMod);
				this.RecacheSelectedModInfo();
				this.pendingScrollToMod = this.primarySelectedMod;
			}
		}

		// Token: 0x06012354 RID: 74580 RVA: 0x0055A908 File Offset: 0x00558B08
		private void onReorderedAction(int from, int to)
		{
			List<ModMetaData> item = this.ModListsInOrder().Item1;
			ModMetaData item2 = item[from];
			if (!this.selectedMods.Contains(item2))
			{
				this.selectedMods.Clear();
				this.primarySelectedMod = item2;
				this.selectedMods.Add(this.primarySelectedMod);
				this.RecacheSelectedModInfo();
			}
			List<ModMetaData> list = new List<ModMetaData>();
			foreach (ModMetaData item3 in item)
			{
				if (this.selectedMods.Contains(item3))
				{
					list.Add(item3);
				}
			}
			list.Reverse();
			ModMetaData placeAfter = null;
			if (to > 0)
			{
				placeAfter = item[to - 1];
			}
			if (to == item.Count)
			{
				to = Page_ModsConfig.activeModListOrderCached.Count;
				placeAfter = null;
			}
			IEnumerable<ModMetaData> activeModsInLoadOrder = ModsConfig.ActiveModsInLoadOrder;
			using (List<ModMetaData>.Enumerator enumerator = list.GetEnumerator())
			{
				Func<ModMetaData, bool> <>9__1;
				while (enumerator.MoveNext())
				{
					ModMetaData mod = enumerator.Current;
					from = activeModsInLoadOrder.FirstIndexOf((ModMetaData m) => m.PackageId == mod.PackageId);
					if (to > 0 && placeAfter != null)
					{
						IEnumerable<ModMetaData> enumerable = activeModsInLoadOrder;
						Func<ModMetaData, bool> predicate;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((ModMetaData m) => m.PackageId == placeAfter.PackageId));
						}
						to = enumerable.FirstIndexOf(predicate) + 1;
					}
					string text;
					if (!ModsConfig.TryReorder(from, to, out text) && !string.IsNullOrWhiteSpace(text))
					{
						Messages.Message(text, null, MessageTypeDefOf.RejectInput, false);
					}
				}
			}
			this.modListsDirty = true;
		}

		// Token: 0x06012355 RID: 74581 RVA: 0x0055AAC8 File Offset: 0x00558CC8
		private void onMultiReorderedAction(int from, int fromGroup, int to, int toGroup)
		{
			ValueTuple<List<ModMetaData>, List<ModMetaData>> valueTuple = this.ModListsInOrder();
			List<ModMetaData> item = valueTuple.Item1;
			List<ModMetaData> item2 = valueTuple.Item2;
			if (fromGroup == 0 && toGroup == 1)
			{
				ModMetaData modMetaData = item2[from];
				this.pendingReorderIndex = to;
				if (!this.selectedMods.Contains(modMetaData))
				{
					if (this.TrySetModActive(modMetaData) && !modMetaData.Official)
					{
						this.pendingReorders.Enqueue(modMetaData.PackageId);
					}
					this.selectedMods.Clear();
					this.primarySelectedMod = modMetaData;
					this.selectedMods.Add(this.primarySelectedMod);
					this.RecacheSelectedModInfo();
				}
				else
				{
					foreach (ModMetaData modMetaData2 in this.selectedMods)
					{
						if (this.TrySetModActive(modMetaData2) && !modMetaData2.Official)
						{
							this.pendingReorders.Enqueue(modMetaData2.PackageId);
						}
					}
					this.primarySelectedMod = modMetaData;
					this.RecacheSelectedModInfo();
				}
				this.lastSelectedIndex = this.GetModIndex(this.primarySelectedMod);
				this.lastSelectedGroupId = this.activeGroupId;
			}
			if (fromGroup == 1 && toGroup == 0)
			{
				ModMetaData modMetaData3 = item[from];
				if (!this.selectedMods.Contains(modMetaData3))
				{
					this.TrySetModInactive(modMetaData3);
				}
				else
				{
					foreach (ModMetaData mod in this.selectedMods)
					{
						this.TrySetModInactive(mod);
					}
				}
				this.primarySelectedMod = modMetaData3;
				this.lastSelectedIndex = this.GetModIndex(this.primarySelectedMod);
				this.lastSelectedGroupId = this.inactiveGroupId;
				this.RecacheSelectedModInfo();
			}
		}

		// Token: 0x06012356 RID: 74582 RVA: 0x0055AC90 File Offset: 0x00558E90
		private int GetModIndex(ModMetaData mod)
		{
			if (mod == null)
			{
				return -1;
			}
			return (mod.Active ? Page_ModsConfig.filteredActiveModListOrderCached : Page_ModsConfig.inactiveModListOrderCached).IndexOf(mod);
		}

		// Token: 0x06012357 RID: 74583 RVA: 0x0055ACB4 File Offset: 0x00558EB4
		private bool TrySetModActive(ModMetaData mod)
		{
			ModMetaData activeModWithIdentifier = ModLister.GetActiveModWithIdentifier(mod.packageIdLowerCase, true);
			if (activeModWithIdentifier != null)
			{
				Find.WindowStack.Add(new Dialog_MessageBox("MessageModWithPackageIdAlreadyEnabled".Translate(mod.PackageIdPlayerFacing, activeModWithIdentifier.Name), null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				return false;
			}
			mod.Active = true;
			this.modListsDirty = true;
			this.truncatedStringCache.Remove(mod.Name);
			return true;
		}

		// Token: 0x06012358 RID: 74584 RVA: 0x0055AD2C File Offset: 0x00558F2C
		private void TrySetModInactive(ModMetaData mod)
		{
			if (mod.IsCoreMod)
			{
				Find.WindowStack.Add(new Dialog_MessageBox("ConfirmDisableCoreMod".Translate(), "Confirm".Translate(), delegate()
				{
					mod.Active = false;
					this.modListsDirty = true;
				}, "GoBack".Translate(), delegate()
				{
					this.selectedMods.Remove(mod);
				}, null, false, null, null, WindowLayer.Dialog));
				return;
			}
			mod.Active = false;
			this.modListsDirty = true;
			this.truncatedStringCache.Remove(mod.Name);
		}

		// Token: 0x06012359 RID: 74585 RVA: 0x0055ADDC File Offset: 0x00558FDC
		private void SaveModList(List<ModMetaData> activeMods)
		{
			ModList modList = new ModList();
			modList.ids = (from mod in activeMods
			select mod.PackageId).ToList<string>();
			modList.names = (from mod in activeMods
			select mod.Name).ToList<string>();
			Find.WindowStack.Add(new Dialog_ModList_Save(modList, null));
		}

		// Token: 0x0601235A RID: 74586 RVA: 0x0055AE60 File Offset: 0x00559060
		private void LoadModList(ModList modList)
		{
			Page_ModsConfig.<>c__DisplayClass87_0 CS$<>8__locals1 = new Page_ModsConfig.<>c__DisplayClass87_0();
			CS$<>8__locals1.modList = modList;
			this.selectedMods.Clear();
			this.selectedMods.Add(this.primarySelectedMod);
			List<int> list = new List<int>();
			foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
			{
				modMetaData.Active = false;
			}
			int i;
			int j;
			for (i = 0; i < CS$<>8__locals1.modList.ids.Count; i = j + 1)
			{
				ModMetaData modMetaData2 = ModLister.AllInstalledMods.FirstOrDefault((ModMetaData mod) => mod.PackageId == CS$<>8__locals1.modList.ids[i]);
				if (modMetaData2 != null)
				{
					modMetaData2.Active = true;
				}
				else
				{
					list.Add(i);
				}
				j = i;
			}
			IEnumerable<string> enumerable = from i in list
			select string.Concat(new string[]
			{
				" - ",
				CS$<>8__locals1.modList.names[i],
				" (",
				CS$<>8__locals1.modList.ids[i],
				")"
			});
			if (enumerable.Any<string>())
			{
				Find.WindowStack.Add(new Dialog_MessageBox(string.Format("{0}:\n\n{1}", "MissingMods".Translate(), enumerable.ToLineList(null, false)), "OK".Translate(), null, null, null, null, false, null, null, WindowLayer.Dialog));
			}
			this.modListsDirty = true;
		}

		// Token: 0x0601235B RID: 74587 RVA: 0x0055AFC8 File Offset: 0x005591C8
		private void DiscardChanges()
		{
			this.selectedMods.Clear();
			foreach (ModMetaData modMetaData in ModLister.AllInstalledMods)
			{
				modMetaData.Active = false;
			}
			foreach (ModMetaData modMetaData2 in Page_ModsConfig.originalModList)
			{
				modMetaData2.Active = true;
			}
			this.modListsDirty = true;
		}

		// Token: 0x0601235C RID: 74588 RVA: 0x0055B064 File Offset: 0x00559264
		private void UnsubscribeMods(IEnumerable<ModMetaData> mods)
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmUnsubscribeFromMultiple".Translate((from mod in mods
			select mod.Name).ToLineList(null, false)), delegate()
			{
				foreach (ModMetaData modMetaData in mods)
				{
					modMetaData.enabled = false;
					Workshop.Unsubscribe(modMetaData);
				}
			}, true, null, WindowLayer.Dialog));
		}

		// Token: 0x0601235D RID: 74589 RVA: 0x0055B0DC File Offset: 0x005592DC
		internal void Notify_SteamItemSubscribed(PublishedFileId_t pfid)
		{
			this.modListsDirty = true;
		}

		// Token: 0x0601235E RID: 74590 RVA: 0x0055B0DC File Offset: 0x005592DC
		internal void Notify_SteamItemInstalled(PublishedFileId_t pfid)
		{
			this.modListsDirty = true;
		}

		// Token: 0x0601235F RID: 74591 RVA: 0x0055B0E5 File Offset: 0x005592E5
		internal void Notify_SteamItemUnsubscribed(PublishedFileId_t pfid)
		{
			if (this.primarySelectedMod != null && this.primarySelectedMod.FolderName == pfid.ToString())
			{
				this.primarySelectedMod = null;
			}
			this.RecacheSelectedModInfo();
			this.modListsDirty = true;
		}

		// Token: 0x0400BD55 RID: 48469
		private ModMetaData primarySelectedMod;

		// Token: 0x0400BD56 RID: 48470
		private int lastSelectedIndex = -1;

		// Token: 0x0400BD57 RID: 48471
		private int lastSelectedGroupId = -1;

		// Token: 0x0400BD58 RID: 48472
		private List<ModMetaData> selectedMods = new List<ModMetaData>();

		// Token: 0x0400BD59 RID: 48473
		private string filter = "";

		// Token: 0x0400BD5A RID: 48474
		private Vector2 activeModListScrollPosition = Vector2.zero;

		// Token: 0x0400BD5B RID: 48475
		private Vector2 inactiveModListScrollPosition = Vector2.zero;

		// Token: 0x0400BD5C RID: 48476
		private Vector2 modDescriptionScrollPosition = Vector2.zero;

		// Token: 0x0400BD5D RID: 48477
		private bool displayFullfilledRequirements;

		// Token: 0x0400BD5E RID: 48478
		private int activeModsHash = -1;

		// Token: 0x0400BD5F RID: 48479
		private int activeModsWhenOpenedHash = -1;

		// Token: 0x0400BD60 RID: 48480
		private int activeGroupId = -1;

		// Token: 0x0400BD61 RID: 48481
		private int inactiveGroupId = -1;

		// Token: 0x0400BD62 RID: 48482
		private Queue<string> pendingReorders = new Queue<string>();

		// Token: 0x0400BD63 RID: 48483
		private int pendingReorderIndex;

		// Token: 0x0400BD64 RID: 48484
		private ModMetaData pendingScrollToMod;

		// Token: 0x0400BD65 RID: 48485
		private double lastClickTime;

		// Token: 0x0400BD66 RID: 48486
		private ModMetaData lastClickedMod;

		// Token: 0x0400BD67 RID: 48487
		private ModMetaData draggedMod;

		// Token: 0x0400BD68 RID: 48488
		private QuickSearchWidget quickSearchWidget = new QuickSearchWidget();

		// Token: 0x0400BD69 RID: 48489
		private bool discardChanges;

		// Token: 0x0400BD6A RID: 48490
		private bool saveChanges;

		// Token: 0x0400BD6B RID: 48491
		private static List<ModMetaData> originalModList;

		// Token: 0x0400BD6C RID: 48492
		private bool modListsDirty;

		// Token: 0x0400BD6D RID: 48493
		private static List<ModMetaData> activeModListOrderCached = new List<ModMetaData>();

		// Token: 0x0400BD6E RID: 48494
		private static List<ModMetaData> inactiveModListOrderCached = new List<ModMetaData>();

		// Token: 0x0400BD6F RID: 48495
		private static List<ModMetaData> filteredActiveModListOrderCached = new List<ModMetaData>();

		// Token: 0x0400BD70 RID: 48496
		private static List<ModMetaData> filteredInactiveModListOrderCached = new List<ModMetaData>();

		// Token: 0x0400BD71 RID: 48497
		private static Dictionary<string, string> modWarningsCached = new Dictionary<string, string>();

		// Token: 0x0400BD72 RID: 48498
		private List<ModRequirement> visibleReqsCached = new List<ModRequirement>();

		// Token: 0x0400BD73 RID: 48499
		private bool anyReqsCached;

		// Token: 0x0400BD74 RID: 48500
		private bool anyReqsInfoToShowCached;

		// Token: 0x0400BD75 RID: 48501
		private bool anyUnfulfilledReqsCached;

		// Token: 0x0400BD76 RID: 48502
		private bool anyOrderingIssuesCached;

		// Token: 0x0400BD77 RID: 48503
		private float modRequirementsHeightCached;

		// Token: 0x0400BD78 RID: 48504
		private Dictionary<string, string> truncatedStringCache = new Dictionary<string, string>();

		// Token: 0x0400BD79 RID: 48505
		private Mod primaryModHandle;

		// Token: 0x0400BD7A RID: 48506
		private const float ModListWidth = 250f;

		// Token: 0x0400BD7B RID: 48507
		private const float ColumnPadding = 4f;

		// Token: 0x0400BD7C RID: 48508
		private const float ModRowHeight = 26f;

		// Token: 0x0400BD7D RID: 48509
		private const float MetaRowHeight = 25f;

		// Token: 0x0400BD7E RID: 48510
		private const float ButtonWidth = 150f;

		// Token: 0x0400BD7F RID: 48511
		private const float ButtonHeight = 30f;

		// Token: 0x0400BD80 RID: 48512
		private const float RequirementBoxInnerOffset = 10f;

		// Token: 0x0400BD81 RID: 48513
		private const float DoubleClickTime = 0.5f;

		// Token: 0x0400BD82 RID: 48514
		private const int DraggedModsLimit = 30;

		// Token: 0x0400BD83 RID: 48515
		private static readonly Color RequirementBoxOutlineColor = new Color(0.25f, 0.25f, 0.25f);

		// Token: 0x0400BD84 RID: 48516
		private static readonly Color UnmetRequirementBoxOutlineColor = new Color(0.62f, 0.18f, 0.18f);

		// Token: 0x0400BD85 RID: 48517
		private static readonly Color UnmetRequirementBoxBGColor = new Color(0.1f, 0.065f, 0.072f);

		// Token: 0x0400BD86 RID: 48518
		private static readonly Color RequirementRowColor = new Color(0.13f, 0.13f, 0.13f);

		// Token: 0x0400BD87 RID: 48519
		private static readonly Color UnmetRequirementRowColor = new Color(0.23f, 0.15f, 0.15f);

		// Token: 0x0400BD88 RID: 48520
		private static readonly Color UnmetRequirementRowColorHighlighted = new Color(0.27f, 0.18f, 0.18f);

		// Token: 0x0400BD89 RID: 48521
		private static readonly Texture2D WarningIcon = Resources.Load<Texture2D>("Textures/UI/Widgets/YellowWarning");

		// Token: 0x0400BD8A RID: 48522
		private static readonly Texture2D ErrorIcon = Resources.Load<Texture2D>("Textures/UI/Widgets/Error");
	}
}
