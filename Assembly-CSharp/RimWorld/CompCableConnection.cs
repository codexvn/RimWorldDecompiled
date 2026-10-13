using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C32 RID: 11314
	public class CompCableConnection : ThingComp
	{
		// Token: 0x170027E6 RID: 10214
		// (get) Token: 0x0600FDAD RID: 64941 RVA: 0x004992D5 File Offset: 0x004974D5
		public CompProperties_CableConnection Props
		{
			get
			{
				return (CompProperties_CableConnection)this.props;
			}
		}

		// Token: 0x170027E7 RID: 10215
		// (get) Token: 0x0600FDAE RID: 64942 RVA: 0x004992E2 File Offset: 0x004974E2
		public Building ParentBuilding
		{
			get
			{
				return this.parent as Building;
			}
		}

		// Token: 0x170027E8 RID: 10216
		// (get) Token: 0x0600FDAF RID: 64943 RVA: 0x004992EF File Offset: 0x004974EF
		public bool CanMote
		{
			get
			{
				return this.ParentBuilding.IsWorking();
			}
		}

		// Token: 0x0600FDB0 RID: 64944 RVA: 0x004992FC File Offset: 0x004974FC
		public override void CompTick()
		{
			base.CompTick();
			if (!this.Props.drawMote || !this.CanMote)
			{
				return;
			}
			foreach (CompCableConnection.Cable cable in this.cables)
			{
				cable.Tick();
			}
		}

		// Token: 0x0600FDB1 RID: 64945 RVA: 0x00499368 File Offset: 0x00497568
		public override void PostDraw()
		{
			base.PostDraw();
			foreach (CompCableConnection.Cable cable in this.cables)
			{
				cable.Draw();
			}
		}

		// Token: 0x0600FDB2 RID: 64946 RVA: 0x004993C0 File Offset: 0x004975C0
		private Vector3 GetTargetConnectionPt(Thing target, ref int idx)
		{
			ThingWithComps thingWithComps = target as ThingWithComps;
			int num;
			if (thingWithComps != null)
			{
				CompAttachPoints comp = thingWithComps.GetComp<CompAttachPoints>();
				if (comp != null)
				{
					AttachPointTracker points = comp.points;
					AttachPointType[] cableAttachPoints = this.CableAttachPoints;
					num = idx;
					idx = num + 1;
					return points.GetWorldPos(cableAttachPoints[num % this.CableAttachPoints.Length]);
				}
			}
			Vector3 drawPos = target.DrawPos;
			List<Vector3> list = this.Props.offsets[this.parent.Rotation.AsInt];
			num = idx;
			idx = num + 1;
			return drawPos + list[num];
		}

		// Token: 0x0600FDB3 RID: 64947 RVA: 0x00499444 File Offset: 0x00497644
		public void RebuildCables(List<Thing> connections, Func<Thing, bool> connectionValidator = null)
		{
			this.cables.Clear();
			int num = 0;
			foreach (Thing thing in connections)
			{
				if (connectionValidator == null || connectionValidator(thing))
				{
					Vector3 worldPos = this.parent.GetComp<CompAttachPoints>().points.GetWorldPos(AttachPointType.CableConnection0);
					Vector3 targetConnectionPt = this.GetTargetConnectionPt(thing, ref num);
					Vector3 vector = worldPos - targetConnectionPt;
					Vector2 vector2 = new Vector2(vector.x, vector.z);
					Vector2 a = vector2 * 0.5f;
					Vector3 pos = targetConnectionPt;
					pos.y += -4.9268293f;
					Vector2 vector3 = Vector2.Perpendicular(vector2 * 0.4f);
					bool flag = Vector2.Dot(vector3.normalized, Vector2.up) < 0f;
					if (flag)
					{
						vector3 = -vector3;
					}
					float degrees = flag ? CompCableConnection.CableRotRange1 : CompCableConnection.CableRotRange2;
					float degrees2 = flag ? CompCableConnection.CableRotRange2 : CompCableConnection.CableRotRange1;
					Vector2[] array = LineMeshGenerator.CalculateEvenlySpacedPoints(new List<Vector2>
					{
						new Vector2(0f, 0f),
						a + vector3.RotatedBy(degrees),
						a + vector3.RotatedBy(degrees2),
						vector2
					}, 0.2f, 1f);
					Mesh mesh = LineMeshGenerator.Generate(array, 0.15f);
					Material mat = MaterialPool.MatFrom(new MaterialRequest
					{
						mainTex = ContentFinder<Texture2D>.Get("Things/Building/Cable", true),
						shader = ShaderDatabase.Transparent,
						color = this.Props.color
					});
					List<ValueTuple<Vector2, float>> list = new List<ValueTuple<Vector2, float>>();
					int i = 1;
					int num2 = array.Length;
					while (i < num2)
					{
						Vector2 vector4 = array[i];
						Vector2 b = array[i - 1];
						float item = vector4.AngleTo(b) + 90f;
						list.Add(new ValueTuple<Vector2, float>(vector4, item));
						i++;
					}
					CompCableConnection.Cable item2 = new CompCableConnection.Cable
					{
						mesh = mesh,
						mat = mat,
						pos = pos,
						map = this.parent.Map,
						points = list,
						quat = Quaternion.identity,
						moteDef = this.Props.moteDef
					};
					this.cables.Add(item2);
				}
			}
		}

		// Token: 0x0400A635 RID: 42549
		private readonly List<CompCableConnection.Cable> cables = new List<CompCableConnection.Cable>();

		// Token: 0x0400A636 RID: 42550
		private const string CableTexturePath = "Things/Building/Cable";

		// Token: 0x0400A637 RID: 42551
		private const float CableYOffset = -5f;

		// Token: 0x0400A638 RID: 42552
		private const float CableLineMeshPointsSpacing = 0.2f;

		// Token: 0x0400A639 RID: 42553
		private const float CableLineMeshWidth = 0.15f;

		// Token: 0x0400A63A RID: 42554
		private static readonly float CableRotRange1 = -Rand.Range(35f, 40f);

		// Token: 0x0400A63B RID: 42555
		private static readonly float CableRotRange2 = Rand.Range(20f, 25f);

		// Token: 0x0400A63C RID: 42556
		private readonly AttachPointType[] CableAttachPoints = new AttachPointType[]
		{
			AttachPointType.CableConnection0,
			AttachPointType.CableConnection1,
			AttachPointType.CableConnection2,
			AttachPointType.CableConnection3
		};

		// Token: 0x02002C33 RID: 11315
		private class Cable
		{
			// Token: 0x0600FDB6 RID: 64950 RVA: 0x0049972D File Offset: 0x0049792D
			public void Draw()
			{
				Graphics.DrawMesh(this.mesh, this.pos, this.quat, this.mat, 0);
			}

			// Token: 0x0600FDB7 RID: 64951 RVA: 0x00499750 File Offset: 0x00497950
			public void Tick()
			{
				if (this.motePower == null || this.motePower.Destroyed)
				{
					ValueTuple<Vector2, float> valueTuple = this.points.RandomElement<ValueTuple<Vector2, float>>();
					Vector3 b = valueTuple.Item1.ToVector3();
					float exactRot = valueTuple.Item2 + 180f;
					this.motePower = MoteMaker.MakeStaticMote(this.pos + b, this.map, this.moteDef, 1f, false, exactRot);
				}
			}

			// Token: 0x0400A63D RID: 42557
			public Mesh mesh;

			// Token: 0x0400A63E RID: 42558
			public Vector3 pos;

			// Token: 0x0400A63F RID: 42559
			public Quaternion quat;

			// Token: 0x0400A640 RID: 42560
			public Material mat;

			// Token: 0x0400A641 RID: 42561
			[TupleElementNames(new string[]
			{
				"offset",
				"rot"
			})]
			public List<ValueTuple<Vector2, float>> points;

			// Token: 0x0400A642 RID: 42562
			public Map map;

			// Token: 0x0400A643 RID: 42563
			public ThingDef moteDef;

			// Token: 0x0400A644 RID: 42564
			private Mote motePower;
		}
	}
}
