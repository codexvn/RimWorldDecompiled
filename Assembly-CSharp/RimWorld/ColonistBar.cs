using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003068 RID: 12392
	[StaticConstructorOnStartup]
	public class ColonistBar
	{
		// Token: 0x17002D07 RID: 11527
		// (get) Token: 0x06011654 RID: 71252 RVA: 0x004FE874 File Offset: 0x004FCA74
		public List<ColonistBar.Entry> Entries
		{
			get
			{
				this.CheckRecacheEntries();
				return this.cachedEntries;
			}
		}

		// Token: 0x17002D08 RID: 11528
		// (get) Token: 0x06011655 RID: 71253 RVA: 0x004FE884 File Offset: 0x004FCA84
		private bool ShowGroupFrames
		{
			get
			{
				List<ColonistBar.Entry> entries = this.Entries;
				int num = -1;
				for (int i = 0; i < entries.Count; i++)
				{
					num = Mathf.Max(num, entries[i].group);
				}
				return num >= 1;
			}
		}

		// Token: 0x17002D09 RID: 11529
		// (get) Token: 0x06011656 RID: 71254 RVA: 0x004FE8C5 File Offset: 0x004FCAC5
		public float Scale
		{
			get
			{
				return this.cachedScale;
			}
		}

		// Token: 0x17002D0A RID: 11530
		// (get) Token: 0x06011657 RID: 71255 RVA: 0x004FE8CD File Offset: 0x004FCACD
		public List<Vector2> DrawLocs
		{
			get
			{
				return this.cachedDrawLocs;
			}
		}

		// Token: 0x17002D0B RID: 11531
		// (get) Token: 0x06011658 RID: 71256 RVA: 0x004FE8D5 File Offset: 0x004FCAD5
		public Vector2 Size
		{
			get
			{
				return ColonistBar.BaseSize * this.Scale;
			}
		}

		// Token: 0x17002D0C RID: 11532
		// (get) Token: 0x06011659 RID: 71257 RVA: 0x004FE8E7 File Offset: 0x004FCAE7
		public float SpaceBetweenColonistsHorizontal
		{
			get
			{
				return 24f * this.Scale;
			}
		}

		// Token: 0x17002D0D RID: 11533
		// (get) Token: 0x0601165A RID: 71258 RVA: 0x004FE8F5 File Offset: 0x004FCAF5
		private bool Visible
		{
			get
			{
				return UI.screenWidth >= 800 && UI.screenHeight >= 500 && !Find.TilePicker.Active;
			}
		}

		// Token: 0x0601165B RID: 71259 RVA: 0x004FE920 File Offset: 0x004FCB20
		public void MarkColonistsDirty()
		{
			this.entriesDirty = true;
		}

		// Token: 0x0601165C RID: 71260 RVA: 0x004FE92C File Offset: 0x004FCB2C
		public void ColonistBarOnGUI()
		{
			if (!this.Visible)
			{
				return;
			}
			if (Event.current.type != EventType.Layout)
			{
				List<ColonistBar.Entry> entries = this.Entries;
				int num = -1;
				bool showGroupFrames = this.ShowGroupFrames;
				int value = -1;
				for (int i = 0; i < this.cachedDrawLocs.Count; i++)
				{
					Rect rect = new Rect(this.cachedDrawLocs[i].x, this.cachedDrawLocs[i].y, this.Size.x, this.Size.y);
					ColonistBar.Entry entry = entries[i];
					bool flag = num != entry.group;
					num = entry.group;
					if (Event.current.type == EventType.Repaint)
					{
						if (flag)
						{
							value = ReorderableWidget.NewGroup(entry.reorderAction, ReorderableDirection.Horizontal, new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight), this.SpaceBetweenColonistsHorizontal, entry.extraDraggedItemOnGUI, true);
						}
						this.cachedReorderableGroups[i] = value;
					}
					bool reordering;
					if (entry.pawn != null)
					{
						this.drawer.HandleClicks(rect, entry.pawn, this.cachedReorderableGroups[i], out reordering);
					}
					else
					{
						reordering = false;
					}
					if (Event.current.type == EventType.Repaint)
					{
						if (flag && showGroupFrames)
						{
							this.drawer.DrawGroupFrame(entry.group);
						}
						if (entry.pawn != null)
						{
							this.drawer.DrawColonist(rect, entry.pawn, entry.map, this.colonistsToHighlight.Contains(entry.pawn), reordering);
							Pawn_EquipmentTracker equipment = entry.pawn.equipment;
							ThingWithComps thingWithComps = (equipment != null) ? equipment.Primary : null;
							if ((Prefs.ShowWeaponsUnderPortraitMode == ShowWeaponsUnderPortraitMode.Always || (Prefs.ShowWeaponsUnderPortraitMode == ShowWeaponsUnderPortraitMode.WhileDrafted && entry.pawn.Drafted)) && thingWithComps != null && thingWithComps.def.IsWeapon)
							{
								Widgets.ThingIcon(new Rect(rect.x, rect.y + rect.height * 1.05f, rect.width, rect.height).ScaledBy(0.75f), thingWithComps, 1f, null, true, 1f, false);
							}
						}
					}
				}
				num = -1;
				if (showGroupFrames)
				{
					for (int j = 0; j < this.cachedDrawLocs.Count; j++)
					{
						ColonistBar.Entry entry2 = entries[j];
						bool flag2 = num != entry2.group;
						num = entry2.group;
						if (flag2)
						{
							this.drawer.HandleGroupFrameClicks(entry2.group);
						}
					}
				}
			}
			if (Event.current.type == EventType.Repaint)
			{
				this.colonistsToHighlight.Clear();
			}
		}

		// Token: 0x0601165D RID: 71261 RVA: 0x004FEBE0 File Offset: 0x004FCDE0
		private void CheckRecacheEntries()
		{
			if (!this.entriesDirty)
			{
				return;
			}
			this.entriesDirty = false;
			this.cachedEntries.Clear();
			int num = 0;
			if (Find.PlaySettings.showColonistBar)
			{
				ColonistBar.tmpMaps.Clear();
				ColonistBar.tmpMaps.AddRange(Find.Maps);
				ColonistBar.tmpMaps.SortBy((Map x) => !x.IsPlayerHome, (Map x) => x.uniqueID);
				for (int i = 0; i < ColonistBar.tmpMaps.Count; i++)
				{
					ColonistBar.tmpPawns.Clear();
					ColonistBar.tmpPawns.AddRange(ColonistBar.tmpMaps[i].mapPawns.FreeColonists);
					ColonistBar.tmpPawns.AddRange(ColonistBar.tmpMaps[i].mapPawns.ColonySubhumansControllable);
					List<Thing> list = ColonistBar.tmpMaps[i].listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
					for (int j = 0; j < list.Count; j++)
					{
						if (!list[j].IsDessicated())
						{
							Pawn innerPawn = ((Corpse)list[j]).InnerPawn;
							if (innerPawn != null && innerPawn.IsColonist)
							{
								ColonistBar.tmpPawns.Add(innerPawn);
							}
						}
					}
					IReadOnlyList<Pawn> allPawnsSpawned = ColonistBar.tmpMaps[i].mapPawns.AllPawnsSpawned;
					for (int k = 0; k < allPawnsSpawned.Count; k++)
					{
						Corpse corpse = allPawnsSpawned[k].carryTracker.CarriedThing as Corpse;
						if (corpse != null && !corpse.IsDessicated() && corpse.InnerPawn.IsColonist)
						{
							ColonistBar.tmpPawns.Add(corpse.InnerPawn);
						}
					}
					foreach (Pawn pawn in ColonistBar.tmpPawns)
					{
						if (pawn.playerSettings.displayOrder == -9999999)
						{
							pawn.playerSettings.displayOrder = Mathf.Max(ColonistBar.tmpPawns.MaxBy((Pawn p) => p.playerSettings.displayOrder).playerSettings.displayOrder, 0) + 1;
						}
					}
					PlayerPawnsDisplayOrderUtility.Sort(ColonistBar.tmpPawns);
					foreach (Pawn pawn2 in ColonistBar.tmpPawns)
					{
						this.cachedEntries.Add(new ColonistBar.Entry(pawn2, ColonistBar.tmpMaps[i], num));
					}
					if (!ColonistBar.tmpPawns.Any<Pawn>())
					{
						this.cachedEntries.Add(new ColonistBar.Entry(null, ColonistBar.tmpMaps[i], num));
					}
					num++;
				}
				ColonistBar.tmpCaravans.Clear();
				ColonistBar.tmpCaravans.AddRange(Find.WorldObjects.Caravans);
				ColonistBar.tmpCaravans.SortBy((Caravan x) => x.ID);
				for (int l = 0; l < ColonistBar.tmpCaravans.Count; l++)
				{
					if (ColonistBar.tmpCaravans[l].IsPlayerControlled)
					{
						ColonistBar.tmpPawns.Clear();
						ColonistBar.tmpPawns.AddRange(ColonistBar.tmpCaravans[l].PawnsListForReading);
						PlayerPawnsDisplayOrderUtility.Sort(ColonistBar.tmpPawns);
						for (int m = 0; m < ColonistBar.tmpPawns.Count; m++)
						{
							if (ColonistBar.tmpPawns[m].IsColonist || ColonistBar.tmpPawns[m].IsColonySubhumanPlayerControlled)
							{
								this.cachedEntries.Add(new ColonistBar.Entry(ColonistBar.tmpPawns[m], null, num));
							}
						}
						num++;
					}
				}
			}
			this.cachedReorderableGroups.Clear();
			foreach (ColonistBar.Entry entry in this.cachedEntries)
			{
				this.cachedReorderableGroups.Add(-1);
			}
			this.drawer.Notify_RecachedEntries();
			ColonistBar.tmpPawns.Clear();
			ColonistBar.tmpMaps.Clear();
			ColonistBar.tmpCaravans.Clear();
			this.drawLocsFinder.CalculateDrawLocs(this.cachedDrawLocs, out this.cachedScale, num);
		}

		// Token: 0x0601165E RID: 71262 RVA: 0x004FF080 File Offset: 0x004FD280
		public float GetEntryRectAlpha(Rect rect)
		{
			float t;
			if (Messages.CollidesWithAnyMessage(rect, out t))
			{
				return Mathf.Lerp(1f, 0.2f, t);
			}
			return 1f;
		}

		// Token: 0x0601165F RID: 71263 RVA: 0x004FF0AD File Offset: 0x004FD2AD
		public void Highlight(Pawn pawn)
		{
			if (!this.Visible)
			{
				return;
			}
			if (!this.colonistsToHighlight.Contains(pawn))
			{
				this.colonistsToHighlight.Add(pawn);
			}
		}

		// Token: 0x06011660 RID: 71264 RVA: 0x004FF0D4 File Offset: 0x004FD2D4
		public void Reorder(int from, int to, int entryGroup)
		{
			int num = 0;
			Pawn pawn = null;
			Pawn pawn2 = null;
			Pawn pawn3 = null;
			for (int i = 0; i < this.cachedEntries.Count; i++)
			{
				if (this.cachedEntries[i].group == entryGroup && this.cachedEntries[i].pawn != null)
				{
					if (num == from)
					{
						pawn = this.cachedEntries[i].pawn;
					}
					if (num == to)
					{
						pawn2 = this.cachedEntries[i].pawn;
					}
					pawn3 = this.cachedEntries[i].pawn;
					num++;
				}
			}
			if (pawn == null)
			{
				return;
			}
			int num2 = (pawn2 != null) ? pawn2.playerSettings.displayOrder : (pawn3.playerSettings.displayOrder + 1);
			for (int j = 0; j < this.cachedEntries.Count; j++)
			{
				Pawn pawn4 = this.cachedEntries[j].pawn;
				if (pawn4 != null)
				{
					if (pawn4.playerSettings.displayOrder == num2)
					{
						if (pawn2 != null && this.cachedEntries[j].group == entryGroup)
						{
							pawn4.playerSettings.displayOrder++;
						}
					}
					else if (pawn4.playerSettings.displayOrder > num2)
					{
						pawn4.playerSettings.displayOrder++;
					}
					else
					{
						pawn4.playerSettings.displayOrder--;
					}
				}
			}
			pawn.playerSettings.displayOrder = num2;
			this.MarkColonistsDirty();
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
		}

		// Token: 0x06011661 RID: 71265 RVA: 0x004FF260 File Offset: 0x004FD460
		public void DrawColonistMouseAttachment(int index, Vector2 dragStartPos, int entryGroup)
		{
			Pawn pawn = null;
			Vector2 vector = default(Vector2);
			int num = 0;
			for (int i = 0; i < this.cachedEntries.Count; i++)
			{
				if (this.cachedEntries[i].group == entryGroup && this.cachedEntries[i].pawn != null)
				{
					if (num == index)
					{
						pawn = this.cachedEntries[i].pawn;
						vector = this.cachedDrawLocs[i];
						break;
					}
					num++;
				}
			}
			if (pawn != null)
			{
				Texture iconTex = PortraitsCache.Get(pawn, ColonistBarColonistDrawer.PawnTextureSize, Rot4.South, ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f, true, true, true, true, null, null, false, null);
				Rect rect = new Rect(vector.x, vector.y, this.Size.x, this.Size.y);
				Rect pawnTextureRect = this.drawer.GetPawnTextureRect(rect.position);
				pawnTextureRect.position += Event.current.mousePosition - dragStartPos;
				string text = "";
				float angle = 0f;
				Rect? customRect = new Rect?(pawnTextureRect);
				GenUI.DrawMouseAttachment(iconTex, text, angle, default(Vector2), customRect, null, false, default(Color), null, null);
			}
		}

		// Token: 0x06011662 RID: 71266 RVA: 0x004FF3BC File Offset: 0x004FD5BC
		public bool AnyColonistOrCorpseAt(Vector2 pos)
		{
			ColonistBar.Entry entry;
			return this.TryGetEntryAt(pos, out entry) && entry.pawn != null;
		}

		// Token: 0x06011663 RID: 71267 RVA: 0x004FF3E0 File Offset: 0x004FD5E0
		public bool TryGetEntryAt(Vector2 pos, out ColonistBar.Entry entry)
		{
			List<Vector2> drawLocs = this.DrawLocs;
			List<ColonistBar.Entry> entries = this.Entries;
			Vector2 size = this.Size;
			for (int i = 0; i < drawLocs.Count; i++)
			{
				if (new Rect(drawLocs[i].x, drawLocs[i].y, size.x, size.y).Contains(pos))
				{
					entry = entries[i];
					return true;
				}
			}
			entry = default(ColonistBar.Entry);
			return false;
		}

		// Token: 0x06011664 RID: 71268 RVA: 0x004FF460 File Offset: 0x004FD660
		public List<Pawn> GetColonistsInOrder()
		{
			List<ColonistBar.Entry> entries = this.Entries;
			ColonistBar.tmpColonistsInOrder.Clear();
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].pawn != null)
				{
					ColonistBar.tmpColonistsInOrder.Add(entries[i].pawn);
				}
			}
			return ColonistBar.tmpColonistsInOrder;
		}

		// Token: 0x06011665 RID: 71269 RVA: 0x004FF4B8 File Offset: 0x004FD6B8
		public List<Thing> ColonistsOrCorpsesInScreenRect(Rect rect)
		{
			List<Vector2> drawLocs = this.DrawLocs;
			List<ColonistBar.Entry> entries = this.Entries;
			Vector2 size = this.Size;
			ColonistBar.tmpColonistsWithMap.Clear();
			for (int i = 0; i < drawLocs.Count; i++)
			{
				if (rect.Overlaps(new Rect(drawLocs[i].x, drawLocs[i].y, size.x, size.y)))
				{
					Pawn pawn = entries[i].pawn;
					if (pawn != null)
					{
						Thing first;
						if (pawn.Dead && pawn.Corpse != null && pawn.Corpse.SpawnedOrAnyParentSpawned)
						{
							first = pawn.Corpse;
						}
						else
						{
							first = pawn;
						}
						ColonistBar.tmpColonistsWithMap.Add(new Pair<Thing, Map>(first, entries[i].map));
					}
				}
			}
			if (WorldRendererUtility.WorldSelected)
			{
				if (ColonistBar.tmpColonistsWithMap.Any((Pair<Thing, Map> x) => x.Second == null))
				{
					ColonistBar.tmpColonistsWithMap.RemoveAll((Pair<Thing, Map> x) => x.Second != null);
					goto IL_179;
				}
			}
			if (ColonistBar.tmpColonistsWithMap.Any((Pair<Thing, Map> x) => x.Second == Find.CurrentMap))
			{
				ColonistBar.tmpColonistsWithMap.RemoveAll((Pair<Thing, Map> x) => x.Second != Find.CurrentMap);
			}
			IL_179:
			ColonistBar.tmpColonists.Clear();
			for (int j = 0; j < ColonistBar.tmpColonistsWithMap.Count; j++)
			{
				ColonistBar.tmpColonists.Add(ColonistBar.tmpColonistsWithMap[j].First);
			}
			ColonistBar.tmpColonistsWithMap.Clear();
			return ColonistBar.tmpColonists;
		}

		// Token: 0x06011666 RID: 71270 RVA: 0x004FF690 File Offset: 0x004FD890
		public List<Thing> MapColonistsOrCorpsesInScreenRect(Rect rect)
		{
			ColonistBar.tmpMapColonistsOrCorpsesInScreenRect.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpMapColonistsOrCorpsesInScreenRect;
			}
			List<Thing> list = this.ColonistsOrCorpsesInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Spawned)
				{
					ColonistBar.tmpMapColonistsOrCorpsesInScreenRect.Add(list[i]);
				}
			}
			return ColonistBar.tmpMapColonistsOrCorpsesInScreenRect;
		}

		// Token: 0x06011667 RID: 71271 RVA: 0x004FF6F4 File Offset: 0x004FD8F4
		public List<Pawn> CaravanMembersInScreenRect(Rect rect)
		{
			ColonistBar.tmpCaravanPawns.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpCaravanPawns;
			}
			List<Thing> list = this.ColonistsOrCorpsesInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null && pawn.IsCaravanMember())
				{
					ColonistBar.tmpCaravanPawns.Add(pawn);
				}
			}
			return ColonistBar.tmpCaravanPawns;
		}

		// Token: 0x06011668 RID: 71272 RVA: 0x004FF75C File Offset: 0x004FD95C
		public List<Caravan> CaravanMembersCaravansInScreenRect(Rect rect)
		{
			ColonistBar.tmpCaravans.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpCaravans;
			}
			List<Pawn> list = this.CaravanMembersInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				ColonistBar.tmpCaravans.Add(list[i].GetCaravan());
			}
			return ColonistBar.tmpCaravans;
		}

		// Token: 0x06011669 RID: 71273 RVA: 0x004FF7B8 File Offset: 0x004FD9B8
		public Caravan CaravanMemberCaravanAt(Vector2 at)
		{
			if (!this.Visible)
			{
				return null;
			}
			Pawn pawn = this.ColonistOrCorpseAt(at) as Pawn;
			if (pawn != null && pawn.IsCaravanMember())
			{
				return pawn.GetCaravan();
			}
			return null;
		}

		// Token: 0x0601166A RID: 71274 RVA: 0x004FF7F0 File Offset: 0x004FD9F0
		public Thing ColonistOrCorpseAt(Vector2 pos)
		{
			if (!this.Visible)
			{
				return null;
			}
			ColonistBar.Entry entry;
			if (!this.TryGetEntryAt(pos, out entry))
			{
				return null;
			}
			Pawn pawn = entry.pawn;
			Thing result;
			if (pawn != null && pawn.Dead && pawn.Corpse != null && pawn.Corpse.SpawnedOrAnyParentSpawned)
			{
				result = pawn.Corpse;
			}
			else
			{
				result = pawn;
			}
			return result;
		}

		// Token: 0x0400B416 RID: 46102
		public ColonistBarColonistDrawer drawer = new ColonistBarColonistDrawer();

		// Token: 0x0400B417 RID: 46103
		private ColonistBarDrawLocsFinder drawLocsFinder = new ColonistBarDrawLocsFinder();

		// Token: 0x0400B418 RID: 46104
		private List<ColonistBar.Entry> cachedEntries = new List<ColonistBar.Entry>();

		// Token: 0x0400B419 RID: 46105
		private List<Vector2> cachedDrawLocs = new List<Vector2>();

		// Token: 0x0400B41A RID: 46106
		private List<int> cachedReorderableGroups = new List<int>();

		// Token: 0x0400B41B RID: 46107
		private float cachedScale = 1f;

		// Token: 0x0400B41C RID: 46108
		private bool entriesDirty = true;

		// Token: 0x0400B41D RID: 46109
		private List<Pawn> colonistsToHighlight = new List<Pawn>();

		// Token: 0x0400B41E RID: 46110
		public static readonly Texture2D BGTex = Command.BGTex;

		// Token: 0x0400B41F RID: 46111
		public static readonly Vector2 BaseSize = new Vector2(48f, 48f);

		// Token: 0x0400B420 RID: 46112
		public const float BaseSelectedTexJump = 20f;

		// Token: 0x0400B421 RID: 46113
		public const float BaseSelectedTexScale = 0.4f;

		// Token: 0x0400B422 RID: 46114
		public const float EntryInAnotherMapAlpha = 0.4f;

		// Token: 0x0400B423 RID: 46115
		public const float BaseSpaceBetweenGroups = 25f;

		// Token: 0x0400B424 RID: 46116
		public const float BaseSpaceBetweenColonistsHorizontal = 24f;

		// Token: 0x0400B425 RID: 46117
		public const float BaseSpaceBetweenColonistsVertical = 32f;

		// Token: 0x0400B426 RID: 46118
		private const float WeaponIconOffsetScaleFactor = 1.05f;

		// Token: 0x0400B427 RID: 46119
		private const float WeaponIconScaleFactor = 0.75f;

		// Token: 0x0400B428 RID: 46120
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x0400B429 RID: 46121
		private static List<Map> tmpMaps = new List<Map>();

		// Token: 0x0400B42A RID: 46122
		private static List<Caravan> tmpCaravans = new List<Caravan>();

		// Token: 0x0400B42B RID: 46123
		private static List<Pawn> tmpColonistsInOrder = new List<Pawn>();

		// Token: 0x0400B42C RID: 46124
		private static List<Pair<Thing, Map>> tmpColonistsWithMap = new List<Pair<Thing, Map>>();

		// Token: 0x0400B42D RID: 46125
		private static List<Thing> tmpColonists = new List<Thing>();

		// Token: 0x0400B42E RID: 46126
		private static List<Thing> tmpMapColonistsOrCorpsesInScreenRect = new List<Thing>();

		// Token: 0x0400B42F RID: 46127
		private static List<Pawn> tmpCaravanPawns = new List<Pawn>();

		// Token: 0x02003069 RID: 12393
		public struct Entry
		{
			// Token: 0x0601166D RID: 71277 RVA: 0x004FF92C File Offset: 0x004FDB2C
			public Entry(Pawn pawn, Map map, int group)
			{
				this.pawn = pawn;
				this.map = map;
				this.group = group;
				this.reorderAction = delegate(int from, int to)
				{
					Find.ColonistBar.Reorder(from, to, group);
				};
				this.extraDraggedItemOnGUI = delegate(int index, Vector2 dragStartPos)
				{
					Find.ColonistBar.DrawColonistMouseAttachment(index, dragStartPos, group);
				};
			}

			// Token: 0x0400B430 RID: 46128
			public Pawn pawn;

			// Token: 0x0400B431 RID: 46129
			public Map map;

			// Token: 0x0400B432 RID: 46130
			public int group;

			// Token: 0x0400B433 RID: 46131
			public Action<int, int> reorderAction;

			// Token: 0x0400B434 RID: 46132
			public Action<int, Vector2> extraDraggedItemOnGUI;
		}
	}
}
