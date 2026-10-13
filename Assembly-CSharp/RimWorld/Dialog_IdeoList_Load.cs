using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020031C9 RID: 12745
	public class Dialog_IdeoList_Load : Dialog_IdeoList
	{
		// Token: 0x06011EF9 RID: 73465 RVA: 0x00538B96 File Offset: 0x00536D96
		public Dialog_IdeoList_Load(Action<Ideo> ideoReturner)
		{
			this.interactButLabel = "LoadGameButton".Translate();
			this.ideoReturner = ideoReturner;
		}

		// Token: 0x06011EFA RID: 73466 RVA: 0x00538BBA File Offset: 0x00536DBA
		public Dialog_IdeoList_Load(Action<Ideo> ideoReturner, List<Faction> npcFactions, bool devEditMode) : this(ideoReturner)
		{
			this.npcFactions = npcFactions;
			this.devEditMode = devEditMode;
		}

		// Token: 0x06011EFB RID: 73467 RVA: 0x00538BD4 File Offset: 0x00536DD4
		protected override void DoFileInteraction(string fileName)
		{
			string filePath = GenFilePaths.AbsPathForIdeo(fileName);
			Action <>9__2;
			PreLoadUtility.CheckVersionAndLoad(filePath, ScribeMetaHeaderUtility.ScribeHeaderMode.Ideo, delegate
			{
				HashSet<string> hashSet = new HashSet<string>();
				Ideo ideo;
				if (!GameDataSaveLoader.TryLoadIdeo(filePath, out ideo))
				{
					this.Close(true);
					return;
				}
				if (!this.devEditMode && !this.npcFactions.NullOrEmpty<Faction>())
				{
					foreach (Faction faction in this.npcFactions)
					{
						foreach (MemeDef memeDef in ideo.memes)
						{
							if (!IdeoUtility.IsMemeAllowedFor(memeDef, faction.def))
							{
								hashSet.Add("UnableToLoadIdeoMemeNotAllowed".Translate(memeDef.label.Named("MEME"), faction.def.label.Named("FACTIONTYPE")));
							}
						}
						if (faction.def.requiredMemes != null)
						{
							foreach (MemeDef memeDef2 in faction.def.requiredMemes)
							{
								if (!ideo.HasMeme(memeDef2))
								{
									hashSet.Add("UnableToLoadIdeoMemeRequired".Translate(memeDef2.label.Named("MEME"), faction.def.label.Named("FACTIONTYPE")));
								}
							}
						}
					}
				}
				if (hashSet.Count == 0)
				{
					this.ideoReturner(IdeoGenerator.InitLoadedIdeo(ideo));
					this.Close(true);
					return;
				}
				TaggedString taggedString = "UnableToLoadIdeoDescription".Translate() + "\n\n" + (from txt in hashSet.AsEnumerable<string>()
				orderby txt
				select txt).ToLineList(" - ", false);
				WindowStack windowStack = Find.WindowStack;
				TaggedString text = taggedString;
				Action confirmedAct;
				if ((confirmedAct = <>9__2) == null)
				{
					confirmedAct = (<>9__2 = delegate()
					{
						this.Close(true);
					});
				}
				windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, false, "UnableToLoadIdeoTitle".Translate(), WindowLayer.Dialog));
			}, false);
		}

		// Token: 0x0400B9BD RID: 47549
		private Action<Ideo> ideoReturner;

		// Token: 0x0400B9BE RID: 47550
		public List<Faction> npcFactions;

		// Token: 0x0400B9BF RID: 47551
		public bool devEditMode;
	}
}
