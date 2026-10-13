using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002491 RID: 9361
	public class Need_RoomSize : Need_Seeker
	{
		// Token: 0x17001EE9 RID: 7913
		// (get) Token: 0x0600CAFF RID: 51967 RVA: 0x003B786C File Offset: 0x003B5A6C
		public override float CurInstantLevel
		{
			get
			{
				return this.SpacePerceptibleNow();
			}
		}

		// Token: 0x17001EEA RID: 7914
		// (get) Token: 0x0600CB00 RID: 51968 RVA: 0x003B7874 File Offset: 0x003B5A74
		public RoomSizeCategory CurCategory
		{
			get
			{
				if (this.CurLevel < 0.01f)
				{
					return RoomSizeCategory.VeryCramped;
				}
				if (this.CurLevel < 0.3f)
				{
					return RoomSizeCategory.Cramped;
				}
				if (this.CurLevel < 0.7f)
				{
					return RoomSizeCategory.Normal;
				}
				return RoomSizeCategory.Spacious;
			}
		}

		// Token: 0x0600CB01 RID: 51969 RVA: 0x003B78A4 File Offset: 0x003B5AA4
		public Need_RoomSize(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.3f);
			this.threshPercents.Add(0.7f);
		}

		// Token: 0x0600CB02 RID: 51970 RVA: 0x003B78D8 File Offset: 0x003B5AD8
		public float SpacePerceptibleNow()
		{
			if (!this.pawn.Spawned)
			{
				return 1f;
			}
			IntVec3 position = this.pawn.Position;
			Need_RoomSize.tempScanRooms.Clear();
			for (int i = 0; i < 5; i++)
			{
				Room room = (position + GenRadial.RadialPattern[i]).GetRoom(this.pawn.Map);
				if (room != null)
				{
					if (i == 0 && room.PsychologicallyOutdoors)
					{
						return 1f;
					}
					if ((i == 0 || (!room.IsDoorway && !room.PsychologicallyOutdoors)) && !Need_RoomSize.tempScanRooms.Contains(room))
					{
						Need_RoomSize.tempScanRooms.Add(room);
					}
				}
			}
			float num = 0f;
			for (int j = 0; j < Need_RoomSize.SampleNumCells; j++)
			{
				IntVec3 loc = position + GenRadial.RadialPattern[j];
				if (Need_RoomSize.tempScanRooms.Contains(loc.GetRoom(this.pawn.Map)))
				{
					num += 1f;
				}
			}
			Need_RoomSize.tempScanRooms.Clear();
			return Need_RoomSize.RoomCellCountSpaceCurve.Evaluate(num);
		}

		// Token: 0x04008A7E RID: 35454
		private static List<Room> tempScanRooms = new List<Room>();

		// Token: 0x04008A7F RID: 35455
		private const float MinCramped = 0.01f;

		// Token: 0x04008A80 RID: 35456
		private const float MinNormal = 0.3f;

		// Token: 0x04008A81 RID: 35457
		private const float MinSpacious = 0.7f;

		// Token: 0x04008A82 RID: 35458
		public static readonly int SampleNumCells = GenRadial.NumCellsInRadius(7.9f);

		// Token: 0x04008A83 RID: 35459
		private static readonly SimpleCurve RoomCellCountSpaceCurve = new SimpleCurve
		{
			{
				new CurvePoint(3f, 0f),
				true
			},
			{
				new CurvePoint(9f, 0.25f),
				true
			},
			{
				new CurvePoint(16f, 0.5f),
				true
			},
			{
				new CurvePoint(42f, 0.71f),
				true
			},
			{
				new CurvePoint(100f, 1f),
				true
			}
		};
	}
}
