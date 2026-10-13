using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000565 RID: 1381
	public abstract class Mod
	{
		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x000BA807 File Offset: 0x000B8A07
		public ModContentPack Content
		{
			get
			{
				return this.intContent;
			}
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x000BA80F File Offset: 0x000B8A0F
		public Mod(ModContentPack content)
		{
			this.intContent = content;
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x000BA820 File Offset: 0x000B8A20
		public T GetSettings<T>() where T : ModSettings, new()
		{
			if (this.modSettings != null && this.modSettings.GetType() != typeof(T))
			{
				Log.Error(string.Format("Mod {0} attempted to read two different settings classes (was {1}, is now {2})", this.Content.Name, this.modSettings.GetType(), typeof(T)));
				return default(T);
			}
			if (this.modSettings != null)
			{
				return (T)((object)this.modSettings);
			}
			this.modSettings = LoadedModManager.ReadModSettings<T>(this.intContent.FolderName, base.GetType().Name);
			this.modSettings.Mod = this;
			return this.modSettings as T;
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x000BA8E0 File Offset: 0x000B8AE0
		public virtual void WriteSettings()
		{
			if (this.modSettings != null)
			{
				this.modSettings.Write();
			}
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DoSettingsWindowContents(Rect inRect)
		{
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x00035ADE File Offset: 0x00033CDE
		public virtual string SettingsCategory()
		{
			return "";
		}

		// Token: 0x04001DB9 RID: 7609
		private ModSettings modSettings;

		// Token: 0x04001DBA RID: 7610
		private ModContentPack intContent;
	}
}
