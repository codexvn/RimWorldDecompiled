using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RimWorld;

namespace Verse
{
	// Token: 0x020001F7 RID: 503
	public class Def : Editable, IEquatable<Def>
	{
		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000D7F RID: 3455 RVA: 0x00040924 File Offset: 0x0003EB24
		public virtual TaggedString LabelCap
		{
			get
			{
				if (this.label.NullOrEmpty())
				{
					return null;
				}
				if (this.cachedLabelCap.NullOrEmpty())
				{
					this.cachedLabelCap = this.label.CapitalizeFirst();
				}
				return this.cachedLabelCap;
			}
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x0004099F File Offset: 0x0003EB9F
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.modContentPack != null && !this.modContentPack.IsCoreMod)
			{
				TaggedString t = this.modContentPack.IsOfficialMod ? "Stat_Source_OfficialExpansionReport".Translate() : "Stat_Source_ModReport".Translate();
				yield return new StatDrawEntry(StatCategoryDefOf.Source, "Stat_Source_Label".Translate(), this.modContentPack.Name, t + ": " + this.modContentPack.Name, 90000, null, null, false, true);
			}
			yield break;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x000409B0 File Offset: 0x0003EBB0
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.modExtensions != null)
			{
				for (int i = 0; i < this.modExtensions.Count; i++)
				{
					this.modExtensions[i].ResolveReferences(this);
				}
			}
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x000409F3 File Offset: 0x0003EBF3
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.defName == "UnnamedDef")
			{
				Type type = base.GetType();
				yield return ((type != null) ? type.ToString() : null) + " lacks defName. Label=" + this.label;
			}
			if (this.defName == "null")
			{
				yield return "defName cannot be the string 'null'.";
			}
			if (!Def.AllowedDefNamesRegex.IsMatch(this.defName))
			{
				yield return "defName " + this.defName + " should only contain letters, numbers, underscores, or dashes.";
			}
			if (this.modExtensions != null)
			{
				int num;
				for (int j = 0; j < this.modExtensions.Count; j = num)
				{
					foreach (string text in this.modExtensions[j].ConfigErrors())
					{
						yield return text;
					}
					IEnumerator<string> enumerator = null;
					num = j + 1;
				}
			}
			if (this.description != null)
			{
				if (this.description == "")
				{
					yield return "empty description";
				}
				if (char.IsWhiteSpace(this.description[0]))
				{
					yield return "description has leading whitespace";
				}
				if (char.IsWhiteSpace(this.description[this.description.Length - 1]))
				{
					yield return "description has trailing whitespace";
				}
			}
			if (this.descriptionHyperlinks != null && this.descriptionHyperlinks.Count > 0)
			{
				if (this.descriptionHyperlinks.RemoveAll((DefHyperlink x) => x.def == null) != 0)
				{
					Log.Warning("Some descriptionHyperlinks in " + this.defName + " had null def.");
				}
				int i;
				Func<DefHyperlink, bool> <>9__1;
				int num;
				for (i = this.descriptionHyperlinks.Count - 1; i > 0; i = num - 1)
				{
					IEnumerable<DefHyperlink> enumerable = this.descriptionHyperlinks;
					Func<DefHyperlink, bool> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((DefHyperlink h) => h.def == this.descriptionHyperlinks[i].def));
					}
					if (enumerable.FirstIndexOf(predicate) < i)
					{
						yield return string.Concat(new string[]
						{
							"Hyperlink to ",
							this.descriptionHyperlinks[i].def.defName,
							" more than once on ",
							this.defName,
							" description"
						});
					}
					num = i;
				}
			}
			if (this.label != null && !this.ignoreIllegalLabelCharacterConfigError && Def.DisallowedLabelCharsRegex.IsMatch(this.label))
			{
				yield return "label contains illegal character(s): \"[]{}\". This can cause issues during grammar resolution. If this was intended, you can use the \"ignoreIllegalLabelCharacterConfigError\" flag.";
			}
			yield break;
			yield break;
		}

		// Token: 0x06000D84 RID: 3460 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostSetIndices()
		{
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x00040A03 File Offset: 0x0003EC03
		public virtual void ClearCachedData()
		{
			this.cachedLabelCap = null;
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x00040A11 File Offset: 0x0003EC11
		public override string ToString()
		{
			return this.defName;
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x00040A19 File Offset: 0x0003EC19
		public void ResolveDefNameHash()
		{
			this.defNameHash = this.defName.GetHashCode();
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x00040A2C File Offset: 0x0003EC2C
		public override int GetHashCode()
		{
			return this.defNameHash;
		}

		// Token: 0x06000D89 RID: 3465 RVA: 0x00040A34 File Offset: 0x0003EC34
		public T GetModExtension<T>() where T : DefModExtension
		{
			if (this.modExtensions == null)
			{
				return default(T);
			}
			for (int i = 0; i < this.modExtensions.Count; i++)
			{
				if (this.modExtensions[i] is T)
				{
					return this.modExtensions[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x06000D8A RID: 3466 RVA: 0x00040A9C File Offset: 0x0003EC9C
		public bool HasModExtension<T>() where T : DefModExtension
		{
			return this.GetModExtension<T>() != null;
		}

		// Token: 0x06000D8B RID: 3467 RVA: 0x00040AAC File Offset: 0x0003ECAC
		public bool Equals(Def other)
		{
			return other != null && other.defNameHash == this.defNameHash && other.GetType() == base.GetType();
		}

		// Token: 0x040007CD RID: 1997
		[Description("The name of this Def. It is used as an identifier by the game code.")]
		[NoTranslate]
		public string defName = "UnnamedDef";

		// Token: 0x040007CE RID: 1998
		[Description("A human-readable label used to identify this in game.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string label;

		// Token: 0x040007CF RID: 1999
		[Description("A human-readable description given when the Def is inspected by players.")]
		[DefaultValue(null)]
		[MustTranslate]
		public string description;

		// Token: 0x040007D0 RID: 2000
		[XmlInheritanceAllowDuplicateNodes]
		public List<DefHyperlink> descriptionHyperlinks;

		// Token: 0x040007D1 RID: 2001
		[Description("Disables config error checking. Intended for mod use. (Be careful!)")]
		[DefaultValue(false)]
		[MustTranslate]
		public bool ignoreConfigErrors;

		// Token: 0x040007D2 RID: 2002
		public bool ignoreIllegalLabelCharacterConfigError;

		// Token: 0x040007D3 RID: 2003
		[Description("Mod-specific data. Not used by core game code.")]
		[DefaultValue(null)]
		public List<DefModExtension> modExtensions;

		// Token: 0x040007D4 RID: 2004
		[Unsaved(false)]
		public ushort shortHash;

		// Token: 0x040007D5 RID: 2005
		[Unsaved(false)]
		public ushort index = ushort.MaxValue;

		// Token: 0x040007D6 RID: 2006
		[Unsaved(false)]
		public ModContentPack modContentPack;

		// Token: 0x040007D7 RID: 2007
		[Unsaved(false)]
		public string fileName;

		// Token: 0x040007D8 RID: 2008
		[Unsaved(false)]
		public int defNameHash;

		// Token: 0x040007D9 RID: 2009
		[Unsaved(false)]
		protected TaggedString cachedLabelCap = null;

		// Token: 0x040007DA RID: 2010
		[Unsaved(false)]
		public bool generated;

		// Token: 0x040007DB RID: 2011
		[Unsaved(false)]
		public ushort debugRandomId = (ushort)Rand.RangeInclusive(0, 65535);

		// Token: 0x040007DC RID: 2012
		public const string DefaultDefName = "UnnamedDef";

		// Token: 0x040007DD RID: 2013
		private static readonly Regex AllowedDefNamesRegex = new Regex("^[a-zA-Z0-9\\-_]*$");

		// Token: 0x040007DE RID: 2014
		private static readonly Regex DisallowedLabelCharsRegex = new Regex("\\[|\\]|\\{|\\}");
	}
}
