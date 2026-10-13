using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000337 RID: 823
	public class GameComponent_DebugTools : GameComponent
	{
		// Token: 0x060014D8 RID: 5336 RVA: 0x0006238C File Offset: 0x0006058C
		public GameComponent_DebugTools(Game game)
		{
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0006239F File Offset: 0x0006059F
		public override void GameComponentUpdate()
		{
			if (this.callbacks.Count > 0 && this.callbacks[0]())
			{
				this.callbacks.RemoveAt(0);
			}
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x000623CE File Offset: 0x000605CE
		public void AddPerFrameCallback(Func<bool> callback)
		{
			this.callbacks.Add(callback);
		}

		// Token: 0x04001376 RID: 4982
		private List<Func<bool>> callbacks = new List<Func<bool>>();
	}
}
