using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031C0 RID: 12736
	public class Dialog_WorldSearch : Dialog_Search<WorldSearchElement>
	{
		// Token: 0x17002E8C RID: 11916
		// (get) Token: 0x06011ECE RID: 73422 RVA: 0x00537BE9 File Offset: 0x00535DE9
		protected override bool ShouldClose
		{
			get
			{
				return WorldRendererUtility.DrawingMap;
			}
		}

		// Token: 0x17002E8D RID: 11917
		// (get) Token: 0x06011ECF RID: 73423 RVA: 0x00537BF0 File Offset: 0x00535DF0
		protected override List<WorldSearchElement> SearchSet
		{
			get
			{
				return this.searchSet;
			}
		}

		// Token: 0x17002E8E RID: 11918
		// (get) Token: 0x06011ED0 RID: 73424 RVA: 0x00537BF8 File Offset: 0x00535DF8
		protected override TaggedString SearchLabel
		{
			get
			{
				return "SearchTheWorld".Translate();
			}
		}

		// Token: 0x06011ED1 RID: 73425 RVA: 0x00537C04 File Offset: 0x00535E04
		public bool IsListed(WorldObject wo)
		{
			return this.listedWorldObjects.Contains(wo);
		}

		// Token: 0x06011ED2 RID: 73426 RVA: 0x00537C12 File Offset: 0x00535E12
		public bool IsListed(PlanetTile tile)
		{
			return this.listedTiles.Contains(tile);
		}

		// Token: 0x06011ED3 RID: 73427 RVA: 0x00537C20 File Offset: 0x00535E20
		public Dialog_WorldSearch()
		{
			this.searchSet = new List<WorldSearchElement>();
			this.InitializeSearchSet();
		}

		// Token: 0x06011ED4 RID: 73428 RVA: 0x00537C50 File Offset: 0x00535E50
		private void InitializeSearchSet()
		{
			this.searchSet.Clear();
			foreach (WorldObject worldObject in Find.World.worldObjects.AllWorldObjects)
			{
				WorldSearchElement worldSearchElement = new WorldSearchElement
				{
					tile = worldObject.Tile,
					worldObject = worldObject
				};
				if (ModsConfig.OdysseyActive)
				{
					worldSearchElement.landmark = Find.World.landmarks.landmarks.TryGetValue(worldObject.Tile, null);
				}
				worldSearchElement.mutators = Find.World.grid[worldObject.Tile].mutatorsNullable;
				this.searchSet.Add(worldSearchElement);
			}
			if (ModsConfig.OdysseyActive)
			{
				using (Dictionary<PlanetTile, Landmark>.Enumerator enumerator2 = Find.World.landmarks.landmarks.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<PlanetTile, Landmark> landmark = enumerator2.Current;
						if (!this.searchSet.Any((WorldSearchElement o) => o.tile == landmark.Key))
						{
							WorldSearchElement item = new WorldSearchElement
							{
								tile = landmark.Key,
								landmark = landmark.Value,
								mutators = Find.World.grid[landmark.Key].mutatorsNullable
							};
							this.searchSet.Add(item);
						}
					}
				}
			}
		}

		// Token: 0x06011ED5 RID: 73429 RVA: 0x00537DF8 File Offset: 0x00535FF8
		protected override void TryAddElement(WorldSearchElement element)
		{
			if (!this.searchResultsSet.Add(element))
			{
				return;
			}
			if (this.ElementMatch(element))
			{
				this.searchResults.Add(element.DisplayLabel.ToLower(), element);
				if (element.worldObject != null)
				{
					this.listedWorldObjects.Add(element.worldObject);
				}
				if (element.tile.Valid)
				{
					this.listedTiles.Add(element.tile);
				}
				this.SetInitialSizeAndPosition();
			}
		}

		// Token: 0x06011ED6 RID: 73430 RVA: 0x00537E73 File Offset: 0x00536073
		public override void Notify_CommonSearchChanged()
		{
			base.Notify_CommonSearchChanged();
			this.listedWorldObjects.Clear();
			this.listedTiles.Clear();
		}

		// Token: 0x06011ED7 RID: 73431 RVA: 0x00537E94 File Offset: 0x00536094
		private bool ElementMatch(WorldSearchElement element)
		{
			if (element.worldObject != null && base.TextMatch(element.worldObject.Label))
			{
				return true;
			}
			if (element.landmark != null)
			{
				if (base.TextMatch(element.landmark.name))
				{
					return true;
				}
				if (base.TextMatch(element.landmark.def.label))
				{
					return true;
				}
			}
			if (!element.mutators.NullOrEmpty<TileMutatorDef>())
			{
				foreach (TileMutatorDef tileMutatorDef in element.mutators)
				{
					if (base.TextMatch(tileMutatorDef.label))
					{
						return true;
					}
					if (base.TextMatch(tileMutatorDef.Label(element.tile)))
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06011ED8 RID: 73432 RVA: 0x00066B11 File Offset: 0x00064D11
		protected override void TryRemoveElement(WorldSearchElement element)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06011ED9 RID: 73433 RVA: 0x00537F70 File Offset: 0x00536170
		protected override void DoIcon(WorldSearchElement element, Rect iconRect)
		{
			if (element.worldObject != null)
			{
				GUI.color = element.worldObject.ExpandingIconColor;
				GUI.DrawTexture(iconRect, element.worldObject.ExpandingIcon);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06011EDA RID: 73434 RVA: 0x00537FA5 File Offset: 0x005361A5
		protected override void DoLabel(WorldSearchElement element, Rect labelRect)
		{
			Widgets.Label(labelRect, element.DisplayLabel);
		}

		// Token: 0x06011EDB RID: 73435 RVA: 0x00537FB4 File Offset: 0x005361B4
		protected override void ClikedOnElement(WorldSearchElement element)
		{
			Find.WorldSelector.ClearSelection();
			if (element.worldObject != null)
			{
				Find.WorldSelector.Select(element.worldObject, true);
			}
			else
			{
				Find.WorldSelector.SelectedTile = element.tile;
			}
			Find.WorldCameraDriver.JumpTo(element.tile);
		}

		// Token: 0x06011EDC RID: 73436 RVA: 0x00538006 File Offset: 0x00536206
		protected override bool ShouldSkipElement(WorldSearchElement element)
		{
			return element.DisplayLabel.NullOrEmpty();
		}

		// Token: 0x06011EDD RID: 73437 RVA: 0x000026EA File Offset: 0x000008EA
		protected override void OnHighlightUpdate(WorldSearchElement element)
		{
		}

		// Token: 0x0400B995 RID: 47509
		private readonly List<WorldSearchElement> searchSet;

		// Token: 0x0400B996 RID: 47510
		private readonly HashSet<WorldObject> listedWorldObjects = new HashSet<WorldObject>();

		// Token: 0x0400B997 RID: 47511
		private readonly HashSet<PlanetTile> listedTiles = new HashSet<PlanetTile>();
	}
}
