using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003FD RID: 1021
	public sealed class FleckManager : IFleckCreator, IExposable
	{
		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060019E3 RID: 6627 RVA: 0x00082E93 File Offset: 0x00081093
		public IEnumerable<FleckSystem> Systems
		{
			get
			{
				return this.systems.Values;
			}
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00082EA0 File Offset: 0x000810A0
		public FleckManager()
		{
			foreach (FleckDef fleckDef in DefDatabase<FleckDef>.AllDefsListForReading)
			{
				FleckSystem fleckSystem;
				if (!this.systems.TryGetValue(fleckDef.fleckSystemClass, out fleckSystem))
				{
					fleckSystem = this.CreateFleckSystemFor(fleckDef);
					this.systems.Add(fleckDef.fleckSystemClass, fleckSystem);
				}
				fleckSystem.handledDefs.Add(fleckDef);
			}
		}

		// Token: 0x060019E5 RID: 6629 RVA: 0x00082F44 File Offset: 0x00081144
		public FleckManager(Map parent) : this()
		{
			this.parent = parent;
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x00082F53 File Offset: 0x00081153
		public FleckSystem CreateFleckSystemFor(FleckDef def)
		{
			return (FleckSystem)Activator.CreateInstance(def.fleckSystemClass, new object[]
			{
				this
			});
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x00082F70 File Offset: 0x00081170
		public void CreateFleck(FleckCreationData fleckData)
		{
			FleckSystem fleckSystem;
			if (!this.systems.TryGetValue(fleckData.def.fleckSystemClass, out fleckSystem))
			{
				string str = "No system to handle MoteDef ";
				FleckDef def = fleckData.def;
				throw new Exception(str + ((def != null) ? def.ToString() : null) + " found!?");
			}
			fleckData.spawnPosition.y = fleckData.def.altitudeLayer.AltitudeFor(fleckData.def.altitudeLayerIncOffset);
			fleckSystem.CreateFleck(fleckData);
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x00082FEC File Offset: 0x000811EC
		public void FleckManagerUpdate()
		{
			float deltaTime = Time.deltaTime;
			foreach (FleckSystem fleckSystem in this.systems.Values)
			{
				fleckSystem.Update(deltaTime);
			}
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x00083048 File Offset: 0x00081248
		public void FleckManagerTick()
		{
			foreach (FleckSystem fleckSystem in this.systems.Values)
			{
				fleckSystem.Tick();
			}
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x000830A0 File Offset: 0x000812A0
		public void FleckManagerDraw()
		{
			try
			{
				foreach (FleckSystem fleckSystem in this.systems.Values)
				{
					fleckSystem.Draw(this.drawBatch);
				}
			}
			finally
			{
				this.drawBatch.Flush(true);
			}
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x00083114 File Offset: 0x00081314
		public void HandOverSystem(FleckSystem system)
		{
			Type type = system.GetType();
			FleckSystem fleckSystem;
			if (this.systems.TryGetValue(type, out fleckSystem))
			{
				fleckSystem.MergeWith(system);
				return;
			}
			this.systems.Add(type, system);
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x00083150 File Offset: 0x00081350
		public void FleckManagerOnGUI()
		{
			foreach (FleckSystem fleckSystem in this.systems.Values)
			{
				fleckSystem.OnGUI();
			}
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x000026EA File Offset: 0x000008EA
		public void ExposeData()
		{
		}

		// Token: 0x040016F5 RID: 5877
		public readonly Map parent;

		// Token: 0x040016F6 RID: 5878
		private Dictionary<Type, FleckSystem> systems = new Dictionary<Type, FleckSystem>();

		// Token: 0x040016F7 RID: 5879
		private DrawBatch drawBatch = new DrawBatch();
	}
}
