using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200053E RID: 1342
	public class TemporaryThingDrawer : IExposable
	{
		// Token: 0x060022D7 RID: 8919 RVA: 0x000B54C8 File Offset: 0x000B36C8
		public void Tick()
		{
			for (int i = this.drawables.Count - 1; i >= 0; i--)
			{
				TemporaryThingDrawable temporaryThingDrawable = this.drawables[i];
				if (temporaryThingDrawable.ticksLeft >= 0 && temporaryThingDrawable.thing != null)
				{
					temporaryThingDrawable.ticksLeft--;
				}
				else
				{
					this.drawables.RemoveAt(i);
				}
			}
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x000B5528 File Offset: 0x000B3728
		public void Draw()
		{
			foreach (TemporaryThingDrawable temporaryThingDrawable in this.drawables)
			{
				temporaryThingDrawable.thing.DrawNowAt(temporaryThingDrawable.position, false);
			}
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x000B5588 File Offset: 0x000B3788
		public void AddThing(Thing t, Vector3 position, int ticks)
		{
			this.drawables.Add(new TemporaryThingDrawable
			{
				thing = t,
				position = position,
				ticksLeft = ticks
			});
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x000B55AF File Offset: 0x000B37AF
		public void ExposeData()
		{
			Scribe_Collections.Look<TemporaryThingDrawable>(ref this.drawables, "drawables", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x04001D2B RID: 7467
		private List<TemporaryThingDrawable> drawables = new List<TemporaryThingDrawable>();
	}
}
