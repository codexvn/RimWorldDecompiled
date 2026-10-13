using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x0200109A RID: 4250
	public sealed class LordManager : IExposable, IDisposable
	{
		// Token: 0x06006852 RID: 26706 RVA: 0x002101D0 File Offset: 0x0020E3D0
		public LordManager(Map map)
		{
			this.map = map;
			map.events.BuildingSpawned += this.Notify_BuildingSpawned;
			map.events.BuildingDespawned += this.Notify_BuildingDespawned;
		}

		// Token: 0x06006853 RID: 26707 RVA: 0x00210230 File Offset: 0x0020E430
		public void ExposeData()
		{
			Scribe_Collections.Look<Lord>(ref this.lords, "lords", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<StencilDrawerForCells>(ref this.stencilDrawers, "stencilDrawers", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].lordManager = this;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.stencilDrawers == null)
				{
					this.stencilDrawers = new List<StencilDrawerForCells>();
				}
				for (int j = 0; j < this.lords.Count; j++)
				{
					Find.SignalManager.RegisterReceiver(this.lords[j]);
				}
			}
		}

		// Token: 0x06006854 RID: 26708 RVA: 0x002102E0 File Offset: 0x0020E4E0
		public void LordManagerTick()
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				try
				{
					this.lords[i].LordTick();
				}
				catch (Exception arg)
				{
					Lord lord = this.lords[i];
					Log.Error(string.Format("Exception while ticking lord with job {0}: \r\n{1}", (((lord != null) ? lord.LordJob : null) == null) ? "NULL" : lord.LordJob.ToString(), arg));
				}
			}
			for (int j = this.lords.Count - 1; j >= 0; j--)
			{
				LordToil curLordToil = this.lords[j].CurLordToil;
				if (curLordToil == null || curLordToil.ShouldFail)
				{
					this.RemoveLord(this.lords[j]);
				}
			}
			for (int k = this.stencilDrawers.Count - 1; k >= 0; k--)
			{
				if (!this.lords.Contains(this.stencilDrawers[k].sourceLord) && this.stencilDrawers[k].ticksLeftWithoutLord <= 0)
				{
					this.stencilDrawers.RemoveAt(k);
				}
			}
		}

		// Token: 0x06006855 RID: 26709 RVA: 0x00210410 File Offset: 0x0020E610
		public void LordManagerUpdate()
		{
			if (DebugViewSettings.drawLords)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].DebugDraw();
				}
			}
			foreach (StencilDrawerForCells stencilDrawerForCells in this.stencilDrawers)
			{
				stencilDrawerForCells.Draw();
				if (!this.lords.Contains(stencilDrawerForCells.sourceLord))
				{
					stencilDrawerForCells.ticksLeftWithoutLord--;
				}
			}
		}

		// Token: 0x06006856 RID: 26710 RVA: 0x002104B4 File Offset: 0x0020E6B4
		public void LordManagerOnGUI()
		{
			if (DebugViewSettings.drawLords)
			{
				for (int i = 0; i < this.lords.Count; i++)
				{
					this.lords[i].DebugOnGUI();
				}
			}
			if (DebugViewSettings.drawDuties)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
				Text.Font = GameFont.Tiny;
				foreach (Pawn pawn in this.map.mapPawns.AllPawns)
				{
					if (pawn.Spawned)
					{
						string text = "";
						if (!pawn.Dead && pawn.mindState.duty != null)
						{
							text = pawn.mindState.duty.ToString();
						}
						if (pawn.InMentalState)
						{
							string str = text;
							string str2 = "\nMentalState=";
							MentalState mentalState = pawn.MentalState;
							text = str + str2 + ((mentalState != null) ? mentalState.ToString() : null);
						}
						Vector2 vector = pawn.DrawPos.MapToUIPosition();
						Widgets.Label(new Rect(vector.x - 100f, vector.y - 100f, 200f, 200f), text);
					}
				}
				Text.Anchor = TextAnchor.UpperLeft;
			}
		}

		// Token: 0x06006857 RID: 26711 RVA: 0x002105F4 File Offset: 0x0020E7F4
		public void AddLord(Lord newLord)
		{
			this.lords.Add(newLord);
			newLord.lordManager = this;
			Find.SignalManager.RegisterReceiver(newLord);
			this.map.events.Notify_LordAdded(newLord);
		}

		// Token: 0x06006858 RID: 26712 RVA: 0x00210625 File Offset: 0x0020E825
		public void RemoveLord(Lord oldLord)
		{
			this.map.events.Notify_LordRemoved(oldLord);
			this.lords.Remove(oldLord);
			Find.SignalManager.DeregisterReceiver(oldLord);
			oldLord.Cleanup();
		}

		// Token: 0x06006859 RID: 26713 RVA: 0x00210658 File Offset: 0x0020E858
		public bool TryGetLordByJob<T>(Faction faction, out T lord) where T : LordJob
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				if (this.lords[i].faction == faction)
				{
					T t = this.lords[i].LordJob as T;
					if (t != null)
					{
						lord = t;
						return true;
					}
				}
			}
			lord = default(T);
			return false;
		}

		// Token: 0x0600685A RID: 26714 RVA: 0x002106C4 File Offset: 0x0020E8C4
		public Lord LordOf(Pawn p)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				for (int j = 0; j < lord.ownedPawns.Count; j++)
				{
					if (lord.ownedPawns[j] == p)
					{
						return lord;
					}
				}
			}
			return null;
		}

		// Token: 0x0600685B RID: 26715 RVA: 0x0021071C File Offset: 0x0020E91C
		public Lord LordOf(Building b)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				for (int j = 0; j < lord.ownedBuildings.Count; j++)
				{
					if (lord.ownedBuildings[j] == b)
					{
						return lord;
					}
				}
			}
			return null;
		}

		// Token: 0x0600685C RID: 26716 RVA: 0x00210774 File Offset: 0x0020E974
		public Lord LordOf(Corpse c)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				for (int j = 0; j < lord.ownedCorpses.Count; j++)
				{
					if (lord.ownedCorpses[j] == c)
					{
						return lord;
					}
				}
			}
			return null;
		}

		// Token: 0x0600685D RID: 26717 RVA: 0x002107CC File Offset: 0x0020E9CC
		private void Notify_BuildingSpawned(Building b)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				this.lords[i].Notify_BuildingSpawnedOnMap(b);
			}
			BreachingGridDebug.Notify_BuildingStateChanged(b);
		}

		// Token: 0x0600685E RID: 26718 RVA: 0x00210808 File Offset: 0x0020EA08
		private void Notify_BuildingDespawned(Building b)
		{
			for (int i = 0; i < this.lords.Count; i++)
			{
				this.lords[i].Notify_BuildingDespawnedOnMap(b);
			}
			BreachingGridDebug.Notify_BuildingStateChanged(b);
		}

		// Token: 0x0600685F RID: 26719 RVA: 0x00210844 File Offset: 0x0020EA44
		public void Notify_MapRemoved()
		{
			for (int i = this.lords.Count - 1; i >= 0; i--)
			{
				this.lords[i].Notify_MapRemoved();
			}
			for (int j = this.lords.Count - 1; j >= 0; j--)
			{
				this.RemoveLord(this.lords[j]);
			}
		}

		// Token: 0x06006860 RID: 26720 RVA: 0x002108A4 File Offset: 0x0020EAA4
		public void LogLords()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= Lords =======");
			stringBuilder.AppendLine("Count: " + this.lords.Count.ToString());
			for (int i = 0; i < this.lords.Count; i++)
			{
				Lord lord = this.lords[i];
				stringBuilder.AppendLine();
				stringBuilder.Append("#" + (i + 1).ToString() + ": ");
				if (lord.LordJob == null)
				{
					stringBuilder.AppendLine("no-job");
				}
				else
				{
					stringBuilder.AppendLine(lord.LordJob.GetType().Name);
				}
				stringBuilder.Append("Current toil: ");
				if (lord.CurLordToil == null)
				{
					stringBuilder.AppendLine("null");
				}
				else
				{
					stringBuilder.AppendLine(lord.CurLordToil.GetType().Name);
				}
				stringBuilder.AppendLine("Members (count: " + lord.ownedPawns.Count.ToString() + "):");
				for (int j = 0; j < lord.ownedPawns.Count; j++)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					string[] array = new string[5];
					array[0] = "  ";
					array[1] = lord.ownedPawns[j].LabelShort;
					array[2] = " (";
					int num = 3;
					Faction faction = lord.ownedPawns[j].Faction;
					array[num] = ((faction != null) ? faction.ToString() : null);
					array[4] = ")";
					stringBuilder2.AppendLine(string.Concat(array));
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06006861 RID: 26721 RVA: 0x00210A4C File Offset: 0x0020EC4C
		public void Dispose()
		{
			foreach (Lord lord in this.lords)
			{
				lord.Dispose();
			}
		}

		// Token: 0x04004A29 RID: 18985
		public Map map;

		// Token: 0x04004A2A RID: 18986
		public List<Lord> lords = new List<Lord>();

		// Token: 0x04004A2B RID: 18987
		public List<StencilDrawerForCells> stencilDrawers = new List<StencilDrawerForCells>();
	}
}
