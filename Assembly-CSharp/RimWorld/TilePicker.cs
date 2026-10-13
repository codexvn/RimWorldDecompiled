using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020034F6 RID: 13558
	[StaticConstructorOnStartup]
	public class TilePicker
	{
		// Token: 0x1700319D RID: 12701
		// (get) Token: 0x060130C7 RID: 78023 RVA: 0x005A5BC5 File Offset: 0x005A3DC5
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x1700319E RID: 12702
		// (get) Token: 0x060130C8 RID: 78024 RVA: 0x005A5BCD File Offset: 0x005A3DCD
		public bool AllowEscape
		{
			get
			{
				return this.allowEscape;
			}
		}

		// Token: 0x1700319F RID: 12703
		// (get) Token: 0x060130C9 RID: 78025 RVA: 0x005A5BD5 File Offset: 0x005A3DD5
		public bool ForGravship
		{
			get
			{
				return this.forGravship;
			}
		}

		// Token: 0x170031A0 RID: 12704
		// (get) Token: 0x060130CA RID: 78026 RVA: 0x005A5BDD File Offset: 0x005A3DDD
		public PlanetTile ClosestLayerTile
		{
			get
			{
				return this.closestLayerTile;
			}
		}

		// Token: 0x060130CB RID: 78027 RVA: 0x005A5BE8 File Offset: 0x005A3DE8
		public void StartTargeting(Func<PlanetTile, bool> validator, Action<PlanetTile> tileChosen, Action onGuiAction = null, Action onUpdateAction = null, bool allowEscape = true, Action noTileChosen = null, string title = null, bool showRandomButton = true, bool selectTileBehindObject = false, bool hideFormCaravanGizmo = false, bool canCancel = false, string noTileChosenMessage = null)
		{
			this.validator = validator;
			this.allowEscape = allowEscape;
			this.noTileChosen = noTileChosen;
			this.tileChosen = tileChosen;
			this.title = title;
			this.showRandomButton = showRandomButton;
			this.onGuiAction = onGuiAction;
			this.onUpdateAction = onUpdateAction;
			this.selectTileBehindObject = selectTileBehindObject;
			this.forGravship = hideFormCaravanGizmo;
			this.canCancel = canCancel;
			this.noTileChosenMessage = (noTileChosenMessage ?? "MustSelectStartingSite".Translate());
			Find.WorldSelector.ClearSelection();
			this.active = true;
		}

		// Token: 0x060130CC RID: 78028 RVA: 0x005A5C76 File Offset: 0x005A3E76
		public void StopTargeting()
		{
			if (this.active && this.noTileChosen != null)
			{
				this.noTileChosen();
			}
			this.StopTargetingInt();
		}

		// Token: 0x060130CD RID: 78029 RVA: 0x005A5C99 File Offset: 0x005A3E99
		private void StopTargetingInt()
		{
			this.forGravship = false;
			this.active = false;
			this.closestLayerTile = PlanetTile.Invalid;
		}

		// Token: 0x060130CE RID: 78030 RVA: 0x005A5CB4 File Offset: 0x005A3EB4
		public void TileSelectorOnGUI()
		{
			if (!this.title.NullOrEmpty())
			{
				Text.Font = GameFont.Medium;
				Vector2 vector = Text.CalcSize(this.title);
				Widgets.Label(new Rect((float)UI.screenWidth / 2f - vector.x / 2f, 4f, vector.x + 4f, vector.y), this.title);
				Text.Font = GameFont.Small;
			}
			Action action = this.onGuiAction;
			if (action != null)
			{
				action();
			}
			int num = this.showRandomButton ? 2 : 1;
			Vector2 buttonSize = TilePicker.ButtonSize;
			if (this.canCancel)
			{
				num++;
			}
			int num2 = (num + 1) * 8;
			Rect rect = new Rect((float)UI.screenWidth / 2f - (float)num * buttonSize.x / 2f - (float)num2 / 2f, (float)UI.screenHeight - (buttonSize.y + 8f) + -50f, (float)num * buttonSize.x + (float)num2, buttonSize.y + 16f);
			Widgets.DrawWindowBackground(rect);
			float num3 = rect.x + 8f;
			if (this.canCancel)
			{
				if (Widgets.ButtonText(new Rect(num3, rect.y + 8f, buttonSize.x, buttonSize.y), "Cancel".Translate(), true, true, true, null))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.StopTargeting();
				}
				num3 += buttonSize.x + 2f + 8f;
			}
			if (this.showRandomButton)
			{
				if (Widgets.ButtonText(new Rect(num3, rect.y + 8f, buttonSize.x, buttonSize.y), "SelectRandomSite".Translate(), true, true, true, null))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					Find.WorldInterface.SelectedTile = TileFinder.RandomStartingTile();
					Find.WorldCameraDriver.JumpTo(Find.WorldGrid.GetTileCenter(Find.WorldInterface.SelectedTile));
				}
				num3 += buttonSize.x + 2f + 8f;
			}
			if (Widgets.ButtonText(new Rect(num3, rect.y + 8f, buttonSize.x, buttonSize.y), "Next".Translate(), true, true, true, null) || KeyBindingDefOf.Accept.KeyDownEvent)
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				PlanetTile planetTile = Find.WorldInterface.SelectedTile;
				if (!planetTile.Valid)
				{
					if (this.selectTileBehindObject)
					{
						WorldObject singleSelectedObject = Find.WorldSelector.SingleSelectedObject;
						if (singleSelectedObject != null && singleSelectedObject.Tile.Valid)
						{
							planetTile = singleSelectedObject.Tile;
							if (!planetTile.Valid)
							{
								Messages.Message(this.noTileChosenMessage, MessageTypeDefOf.RejectInput, false);
							}
							else if (this.validator(planetTile))
							{
								this.StopTargetingInt();
								this.tileChosen(planetTile);
								Event.current.Use();
							}
						}
						else
						{
							Messages.Message(this.noTileChosenMessage, MessageTypeDefOf.RejectInput, false);
						}
					}
					else
					{
						Messages.Message(this.noTileChosenMessage, MessageTypeDefOf.RejectInput, false);
					}
				}
				else if (this.validator(planetTile))
				{
					this.StopTargetingInt();
					this.tileChosen(planetTile);
					Event.current.Use();
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.Active && !this.allowEscape)
			{
				Event.current.Use();
			}
		}

		// Token: 0x060130CF RID: 78031 RVA: 0x005A6052 File Offset: 0x005A4252
		public void TileSelectorUpdate()
		{
			Action action = this.onUpdateAction;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x0400C568 RID: 50536
		private static readonly Vector2 ButtonSize = new Vector2(150f, 38f);

		// Token: 0x0400C569 RID: 50537
		private const int Padding = 8;

		// Token: 0x0400C56A RID: 50538
		private const int BottomPanelYOffset = -50;

		// Token: 0x0400C56B RID: 50539
		private Func<PlanetTile, bool> validator;

		// Token: 0x0400C56C RID: 50540
		private bool allowEscape;

		// Token: 0x0400C56D RID: 50541
		private bool active;

		// Token: 0x0400C56E RID: 50542
		private Action<PlanetTile> tileChosen;

		// Token: 0x0400C56F RID: 50543
		private Action noTileChosen;

		// Token: 0x0400C570 RID: 50544
		private Action onGuiAction;

		// Token: 0x0400C571 RID: 50545
		private Action onUpdateAction;

		// Token: 0x0400C572 RID: 50546
		private string title;

		// Token: 0x0400C573 RID: 50547
		private bool showRandomButton = true;

		// Token: 0x0400C574 RID: 50548
		private bool selectTileBehindObject;

		// Token: 0x0400C575 RID: 50549
		private bool forGravship;

		// Token: 0x0400C576 RID: 50550
		private bool canCancel;

		// Token: 0x0400C577 RID: 50551
		private PlanetTile closestLayerTile = PlanetTile.Invalid;

		// Token: 0x0400C578 RID: 50552
		private string noTileChosenMessage;
	}
}
