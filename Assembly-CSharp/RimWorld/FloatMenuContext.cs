using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020032B2 RID: 12978
	public class FloatMenuContext
	{
		// Token: 0x17002F19 RID: 12057
		// (get) Token: 0x06012440 RID: 74816 RVA: 0x005620DD File Offset: 0x005602DD
		public IntVec3 ClickedCell
		{
			get
			{
				return this.cachedClickedCell;
			}
		}

		// Token: 0x17002F1A RID: 12058
		// (get) Token: 0x06012441 RID: 74817 RVA: 0x005620E5 File Offset: 0x005602E5
		public Room ClickedRoom
		{
			get
			{
				return this.cachedClickedRoom;
			}
		}

		// Token: 0x17002F1B RID: 12059
		// (get) Token: 0x06012442 RID: 74818 RVA: 0x005620ED File Offset: 0x005602ED
		public Zone ClickedZone
		{
			get
			{
				return this.cachedClickedZone;
			}
		}

		// Token: 0x17002F1C RID: 12060
		// (get) Token: 0x06012443 RID: 74819 RVA: 0x005620F5 File Offset: 0x005602F5
		public List<Thing> ClickedThings
		{
			get
			{
				return this.cachedClickedThings;
			}
		}

		// Token: 0x17002F1D RID: 12061
		// (get) Token: 0x06012444 RID: 74820 RVA: 0x005620FD File Offset: 0x005602FD
		public List<Pawn> ClickedPawns
		{
			get
			{
				return this.cachedClickedPawns;
			}
		}

		// Token: 0x17002F1E RID: 12062
		// (get) Token: 0x06012445 RID: 74821 RVA: 0x00562105 File Offset: 0x00560305
		public bool IsMultiselect
		{
			get
			{
				return this.allSelectedPawns.Count > 1;
			}
		}

		// Token: 0x17002F1F RID: 12063
		// (get) Token: 0x06012446 RID: 74822 RVA: 0x00562118 File Offset: 0x00560318
		public Pawn FirstSelectedPawn
		{
			get
			{
				foreach (Pawn pawn in this.allSelectedPawns)
				{
					if (FloatMenuMakerMap.currentProvider == null || FloatMenuMakerMap.currentProvider.SelectedPawnValid(pawn, this))
					{
						return pawn;
					}
				}
				return null;
			}
		}

		// Token: 0x17002F20 RID: 12064
		// (get) Token: 0x06012447 RID: 74823 RVA: 0x00562180 File Offset: 0x00560380
		public IEnumerable<Pawn> ValidSelectedPawns
		{
			get
			{
				foreach (Pawn pawn in this.allSelectedPawns)
				{
					if (FloatMenuMakerMap.currentProvider == null || FloatMenuMakerMap.currentProvider.SelectedPawnValid(pawn, this))
					{
						yield return pawn;
					}
				}
				List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06012448 RID: 74824 RVA: 0x00562190 File Offset: 0x00560390
		public FloatMenuContext(List<Pawn> selectedPawns, Vector3 clickPosition, Map map)
		{
			this.allSelectedPawns = selectedPawns;
			this.clickPosition = clickPosition;
			this.map = map;
			this.cachedClickedCell = IntVec3.FromVector3(clickPosition);
			this.cachedClickedRoom = this.cachedClickedCell.GetRoom(map);
			this.cachedClickedZone = this.cachedClickedCell.GetZone(map);
			this.cachedClickedThings = GenUI.ThingsUnderMouse(clickPosition, 0.8f, TargetingParameters.ForThing(), null);
			this.cachedClickedPawns = GenUI.ThingsUnderMouse(clickPosition, 0.8f, TargetingParameters.ForPawns(), null).OfType<Pawn>().ToList<Pawn>();
			selectedPawns.RemoveAll((Pawn pawn) => !pawn.CanTakeOrder);
		}

		// Token: 0x0400BE20 RID: 48672
		public List<Pawn> allSelectedPawns;

		// Token: 0x0400BE21 RID: 48673
		public Vector3 clickPosition;

		// Token: 0x0400BE22 RID: 48674
		public Map map;

		// Token: 0x0400BE23 RID: 48675
		private IntVec3 cachedClickedCell;

		// Token: 0x0400BE24 RID: 48676
		private List<Thing> cachedClickedThings;

		// Token: 0x0400BE25 RID: 48677
		private List<Pawn> cachedClickedPawns;

		// Token: 0x0400BE26 RID: 48678
		private Room cachedClickedRoom;

		// Token: 0x0400BE27 RID: 48679
		private Zone cachedClickedZone;
	}
}
