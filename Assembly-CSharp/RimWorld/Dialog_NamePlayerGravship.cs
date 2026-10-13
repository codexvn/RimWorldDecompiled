using System;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200319C RID: 12700
	public class Dialog_NamePlayerGravship : Dialog_GiveName
	{
		// Token: 0x17002E72 RID: 11890
		// (get) Token: 0x06011DEF RID: 73199 RVA: 0x0052B3E5 File Offset: 0x005295E5
		protected override int FirstCharLimit
		{
			get
			{
				return 64;
			}
		}

		// Token: 0x06011DF0 RID: 73200 RVA: 0x005304E4 File Offset: 0x0052E6E4
		public Dialog_NamePlayerGravship(Building_GravEngine engine)
		{
			if (!ModLister.CheckOdyssey("Grav engines"))
			{
				return;
			}
			this.engine = engine;
			this.nameGenerator = (() => NameGenerator.GenerateName(RulePackDefOf.NamerGravship, new Predicate<string>(this.IsValidName), false, null, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerGravshipMessage";
			this.gainedNameMessageKey = "PlayerGravshipGainsName";
			this.invalidNameMessageKey = "PlayerGravshipNameIsInvalid";
		}

		// Token: 0x06011DF1 RID: 73201 RVA: 0x0053054F File Offset: 0x0052E74F
		protected override bool IsValidName(string s)
		{
			return s.Length != 0 && s.Length <= 64 && !GrammarResolver.ContainsSpecialChars(s);
		}

		// Token: 0x06011DF2 RID: 73202 RVA: 0x00530572 File Offset: 0x0052E772
		protected override void Named(string s)
		{
			this.engine.RenamableLabel = s;
			this.engine.nameHidden = false;
		}

		// Token: 0x0400B8D1 RID: 47313
		private readonly Building_GravEngine engine;
	}
}
