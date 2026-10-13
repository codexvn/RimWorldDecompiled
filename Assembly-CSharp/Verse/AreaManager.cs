using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020003C3 RID: 963
	public class AreaManager : IExposable
	{
		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060018C3 RID: 6339 RVA: 0x0007D119 File Offset: 0x0007B319
		public List<Area> AllAreas
		{
			get
			{
				return this.areas;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060018C4 RID: 6340 RVA: 0x0007D121 File Offset: 0x0007B321
		public Area_Home Home
		{
			get
			{
				return this.Get<Area_Home>();
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060018C5 RID: 6341 RVA: 0x0007D129 File Offset: 0x0007B329
		public Area_BuildRoof BuildRoof
		{
			get
			{
				return this.Get<Area_BuildRoof>();
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x060018C6 RID: 6342 RVA: 0x0007D131 File Offset: 0x0007B331
		public Area_NoRoof NoRoof
		{
			get
			{
				return this.Get<Area_NoRoof>();
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x060018C7 RID: 6343 RVA: 0x0007D139 File Offset: 0x0007B339
		public Area_SnowOrSandClear SnowOrSandClear
		{
			get
			{
				return this.Get<Area_SnowOrSandClear>();
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x060018C8 RID: 6344 RVA: 0x0007D141 File Offset: 0x0007B341
		public Area_PollutionClear PollutionClear
		{
			get
			{
				return this.Get<Area_PollutionClear>();
			}
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x0007D149 File Offset: 0x0007B349
		public AreaManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060018CA RID: 6346 RVA: 0x0007D164 File Offset: 0x0007B364
		public void AddStartingAreas()
		{
			this.areas.Add(new Area_Home(this));
			this.areas.Add(new Area_BuildRoof(this));
			this.areas.Add(new Area_NoRoof(this));
			this.areas.Add(new Area_SnowOrSandClear(this));
			if (ModsConfig.BiotechActive)
			{
				this.areas.Add(new Area_PollutionClear(this));
			}
			if (!this.map.wasSpawnedViaGravShipLanding)
			{
				Area_Allowed area_Allowed;
				this.TryMakeNewAllowed(out area_Allowed);
			}
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x0007D1E4 File Offset: 0x0007B3E4
		public void ExposeData()
		{
			Scribe_Collections.Look<Area>(ref this.areas, "areas", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateAllAreasLinks();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && ModsConfig.BiotechActive && this.PollutionClear == null)
			{
				Area_PollutionClear area_PollutionClear = new Area_PollutionClear(this);
				area_PollutionClear.areaManager = this;
				this.areas.Add(area_PollutionClear);
			}
		}

		// Token: 0x060018CC RID: 6348 RVA: 0x0007D248 File Offset: 0x0007B448
		public void AreaManagerUpdate()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].AreaUpdate();
			}
		}

		// Token: 0x060018CD RID: 6349 RVA: 0x0007D27C File Offset: 0x0007B47C
		internal void Remove(Area area)
		{
			if (!area.Mutable)
			{
				Log.Error("Tried to delete non-Deletable area " + ((area != null) ? area.ToString() : null));
				return;
			}
			this.areas.Remove(area);
			this.NotifyEveryoneAreaRemoved(area);
			if (Designator_AreaAllowed.SelectedArea == area)
			{
				Designator_AreaAllowed.ClearSelectedArea();
			}
		}

		// Token: 0x060018CE RID: 6350 RVA: 0x0007D2D0 File Offset: 0x0007B4D0
		public Area GetLabeled(string s)
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				if (this.areas[i].Label == s)
				{
					return this.areas[i];
				}
			}
			return null;
		}

		// Token: 0x060018CF RID: 6351 RVA: 0x0007D31C File Offset: 0x0007B51C
		public T Get<T>() where T : Area
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				T t = this.areas[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x060018D0 RID: 6352 RVA: 0x0007D369 File Offset: 0x0007B569
		private void SortAreas()
		{
			this.areas.InsertionSort((Area a, Area b) => b.ListPriority.CompareTo(a.ListPriority));
		}

		// Token: 0x060018D1 RID: 6353 RVA: 0x0007D398 File Offset: 0x0007B598
		private void UpdateAllAreasLinks()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.areas[i].areaManager = this;
			}
		}

		// Token: 0x060018D2 RID: 6354 RVA: 0x0007D3D0 File Offset: 0x0007B5D0
		private void NotifyEveryoneAreaRemoved(Area area)
		{
			foreach (Pawn pawn in PawnsFinder.All_AliveOrDead)
			{
				if (pawn.playerSettings != null)
				{
					pawn.playerSettings.Notify_AreaRemoved(area);
				}
			}
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x0007D430 File Offset: 0x0007B630
		public void Notify_MapRemoved()
		{
			for (int i = 0; i < this.areas.Count; i++)
			{
				this.NotifyEveryoneAreaRemoved(this.areas[i]);
			}
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x0007D465 File Offset: 0x0007B665
		public bool CanMakeNewAllowed()
		{
			return (from a in this.areas
			where a is Area_Allowed
			select a).Count<Area>() < 10;
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x0007D49A File Offset: 0x0007B69A
		public bool TryMakeNewAllowed(out Area_Allowed area)
		{
			if (!this.CanMakeNewAllowed())
			{
				area = null;
				return false;
			}
			area = new Area_Allowed(this, null);
			this.areas.Add(area);
			this.SortAreas();
			return true;
		}

		// Token: 0x040015E7 RID: 5607
		public Map map;

		// Token: 0x040015E8 RID: 5608
		private List<Area> areas = new List<Area>();

		// Token: 0x040015E9 RID: 5609
		public const int MaxAllowedAreas = 10;
	}
}
