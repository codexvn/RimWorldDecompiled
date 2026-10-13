using System;
using System.Text;

namespace Verse
{
	// Token: 0x02000335 RID: 821
	public abstract class GameComponent : IExposable
	{
		// Token: 0x060014C9 RID: 5321 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GameComponentUpdate()
		{
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GameComponentTick()
		{
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void GameComponentOnGUI()
		{
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExposeData()
		{
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void FinalizeInit()
		{
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void StartedNewGame()
		{
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void LoadedGame()
		{
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void AppendDebugString(StringBuilder sb)
		{
		}
	}
}
