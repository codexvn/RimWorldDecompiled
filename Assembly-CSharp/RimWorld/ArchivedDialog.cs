using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B54 RID: 6996
	public class ArchivedDialog : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x17001802 RID: 6146
		// (get) Token: 0x06009C3F RID: 39999 RVA: 0x00002C42 File Offset: 0x00000E42
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001803 RID: 6147
		// (get) Token: 0x06009C40 RID: 40000 RVA: 0x000440FD File Offset: 0x000422FD
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17001804 RID: 6148
		// (get) Token: 0x06009C41 RID: 40001 RVA: 0x002D58C1 File Offset: 0x002D3AC1
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x17001805 RID: 6149
		// (get) Token: 0x06009C42 RID: 40002 RVA: 0x002D58CE File Offset: 0x002D3ACE
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x17001806 RID: 6150
		// (get) Token: 0x06009C43 RID: 40003 RVA: 0x002D58D6 File Offset: 0x002D3AD6
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.createdTick;
			}
		}

		// Token: 0x17001807 RID: 6151
		// (get) Token: 0x06009C44 RID: 40004 RVA: 0x000028E7 File Offset: 0x00000AE7
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001808 RID: 6152
		// (get) Token: 0x06009C45 RID: 40005 RVA: 0x00002C42 File Offset: 0x00000E42
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06009C46 RID: 40006 RVA: 0x000024DF File Offset: 0x000006DF
		public ArchivedDialog()
		{
		}

		// Token: 0x06009C47 RID: 40007 RVA: 0x002D58E0 File Offset: 0x002D3AE0
		public ArchivedDialog(string text, string title = null, Faction relatedFaction = null)
		{
			this.text = text;
			this.title = title;
			this.relatedFaction = relatedFaction;
			this.createdTick = GenTicks.TicksGame;
			if (Find.UniqueIDsManager != null)
			{
				this.ID = Find.UniqueIDsManager.GetNextArchivedDialogID();
				return;
			}
			this.ID = Rand.Int;
		}

		// Token: 0x06009C48 RID: 40008 RVA: 0x002D5938 File Offset: 0x002D3B38
		void IArchivable.OpenArchived()
		{
			DiaNode diaNode = new DiaNode(this.text);
			DiaOption diaOption = new DiaOption("OK".Translate());
			diaOption.resolveTree = true;
			diaNode.options.Add(diaOption);
			Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, false, this.title));
		}

		// Token: 0x06009C49 RID: 40009 RVA: 0x002D599C File Offset: 0x002D3B9C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.createdTick, "createdTick", 0, false);
		}

		// Token: 0x06009C4A RID: 40010 RVA: 0x002D5A02 File Offset: 0x002D3C02
		public string GetUniqueLoadID()
		{
			return "ArchivedDialog_" + this.ID.ToString();
		}

		// Token: 0x04006C5D RID: 27741
		public int ID;

		// Token: 0x04006C5E RID: 27742
		public string text;

		// Token: 0x04006C5F RID: 27743
		public string title;

		// Token: 0x04006C60 RID: 27744
		public Faction relatedFaction;

		// Token: 0x04006C61 RID: 27745
		public int createdTick;
	}
}
