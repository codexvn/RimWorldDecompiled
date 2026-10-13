using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000401 RID: 1025
	public abstract class FleckSystem : IFleckCreator, IExposable, ILoadReferenceable
	{
		// Token: 0x060019F9 RID: 6649 RVA: 0x00083264 File Offset: 0x00081464
		public FleckSystem(FleckManager parent)
		{
			this.parent = parent;
		}

		// Token: 0x060019FA RID: 6650
		public abstract void Update(float deltaTime);

		// Token: 0x060019FB RID: 6651
		public abstract void Tick();

		// Token: 0x060019FC RID: 6652
		public abstract void ForceDraw(DrawBatch drawBatch);

		// Token: 0x060019FD RID: 6653 RVA: 0x0008327E File Offset: 0x0008147E
		public virtual void Draw(DrawBatch drawBatch)
		{
			if (WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				return;
			}
			this.ForceDraw(drawBatch);
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void OnGUI()
		{
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x00083290 File Offset: 0x00081490
		public virtual void Prewarm(float prewarmSeconds, Action<float> onUpdate, Action onTick)
		{
			float num = 0f;
			float num2 = 0f;
			while (num < prewarmSeconds)
			{
				this.Update(0.06666667f);
				if (onUpdate != null)
				{
					onUpdate(0.06666667f);
				}
				while (num2 >= 0.016666668f)
				{
					this.Tick();
					if (onTick != null)
					{
						onTick();
					}
					num2 -= 0.016666668f;
				}
				num += 0.06666667f;
				num2 += 0.06666667f;
			}
		}

		// Token: 0x06001A00 RID: 6656
		public abstract void CreateFleck(FleckCreationData fleckData);

		// Token: 0x06001A01 RID: 6657
		public abstract void MergeWith(FleckSystem system);

		// Token: 0x06001A02 RID: 6658
		public abstract IEnumerable<IFleck> EnumerateFlecks();

		// Token: 0x06001A03 RID: 6659
		public abstract void RemoveAllFlecks(Predicate<IFleck> shouldRemove);

		// Token: 0x06001A04 RID: 6660
		public abstract void ExposeData();

		// Token: 0x06001A05 RID: 6661 RVA: 0x000832F9 File Offset: 0x000814F9
		public string GetUniqueLoadID()
		{
			return this.parent.parent.GetUniqueLoadID() + "_FleckSystem_" + base.GetType().FullName;
		}

		// Token: 0x040016FC RID: 5884
		public List<FleckDef> handledDefs = new List<FleckDef>();

		// Token: 0x040016FD RID: 5885
		public FleckManager parent;
	}
}
