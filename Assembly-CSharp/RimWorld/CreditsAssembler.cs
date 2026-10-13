using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x020032AE RID: 12974
	public class CreditsAssembler
	{
		// Token: 0x0601241E RID: 74782 RVA: 0x0055F6C9 File Offset: 0x0055D8C9
		public static IEnumerable<CreditsEntry> AllCredits()
		{
			yield return new CreditRecord_Space(200f);
			yield return new CreditRecord_Title("Credits_Design".Translate());
			yield return new CreditRecord_Role("", "Tynan Sylvester", null);
			yield return new CreditRecord_Role("", "Will Stacey", null);
			yield return new CreditRecord_Role("", "Thorsten Klotz", null);
			yield return new CreditRecord_Title("Credits_Developers".Translate());
			yield return new CreditRecord_Role("", "Piotr Walczak", null);
			yield return new CreditRecord_Role("", "Tynan Sylvester", null);
			yield return new CreditRecord_Role("", "Igor Lebedev", null);
			yield return new CreditRecord_Role("", "Matt Ritchie", null);
			yield return new CreditRecord_Role("", "Alex Mulford", null);
			yield return new CreditRecord_Role("", "Joe Gasparich", null);
			yield return new CreditRecord_Role("", "Máté Mészáros", null);
			yield return new CreditRecord_Role("", "Liam Harrison", null);
			yield return new CreditRecord_Role("", "Kenneth Ellersdorfer", null);
			yield return new CreditRecord_Role("", "Sam Byass", null);
			yield return new CreditRecord_Role("", "Ben Rog-Wilhelm", null);
			yield return new CreditRecord_Role("", "Matt Quail", null);
			yield return new CreditRecord_Role("", "Mark Kaldas", null);
			yield return new CreditRecord_Role("", "Nick Barrash", null);
			yield return new CreditRecord_Role("", "Don Bellenger", null);
			yield return new CreditRecord_Role("", "Jay Lemmon", null);
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credit_MusicAndSound".Translate());
			yield return new CreditRecord_Role("", "Alistair Lindsay", null);
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credit_GameArt".Translate());
			yield return new CreditRecord_Role("", "Oskar Potocki", null);
			yield return new CreditRecord_Role("", "Tynan Sylvester", null);
			yield return new CreditRecord_Role("", "Hayden Duvall", null);
			yield return new CreditRecord_Role("", "Mehran Iranloo", null);
			yield return new CreditRecord_Role("", "Ricardo Tomé", null);
			yield return new CreditRecord_Role("", "Rhopunzel", null);
			yield return new CreditRecord_Role("", "Tamara Osborn", null);
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credits_AdditionalDevelopment".Translate());
			foreach (CreditsEntry creditsEntry in CreditsAssembler.<AllCredits>g__Reformat2Cols|0_0(new List<CreditsEntry>
			{
				new CreditRecord_Role("", "August Trollbäck", null).Compress(),
				new CreditRecord_Role("", "John Fernandes-Salling", null).Compress(),
				new CreditRecord_Role("", "Tate Conlon", null).Compress(),
				new CreditRecord_Role("", "Gavan Woolery", null).Compress(),
				new CreditRecord_Role("", "David 'Rez' Graham", null).Compress()
			}))
			{
				yield return creditsEntry;
			}
			IEnumerator<CreditsEntry> enumerator = null;
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credit_Testers".Translate());
			foreach (CreditsEntry creditsEntry2 in CreditsAssembler.<AllCredits>g__Reformat2Cols|0_0(new List<CreditsEntry>
			{
				new CreditRecord_Role("", "Fey Nickel", null),
				new CreditRecord_Role("", "Morg", null),
				new CreditRecord_Role("", "ItchyFlea", null),
				new CreditRecord_Role("", "Sneaks", null),
				new CreditRecord_Role("", "Elliott", null),
				new CreditRecord_Role("", "James Miura", null)
			}))
			{
				yield return creditsEntry2;
			}
			enumerator = null;
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credit_SpecialThanks".Translate());
			foreach (CreditsEntry creditsEntry3 in CreditsAssembler.<AllCredits>g__Reformat2Cols|0_0(new List<CreditsEntry>
			{
				new CreditRecord_Role("", "Tia Young", null).Compress(),
				new CreditRecord_Role("", "Kay Fedewa", null).Compress(),
				new CreditRecord_Role("", "Jon Larson", null).Compress(),
				new CreditRecord_Role("", "Zhentar", null).Compress(),
				new CreditRecord_Role("", "Haplo", null).Compress(),
				new CreditRecord_Role("", "iame6162013", null).Compress(),
				new CreditRecord_Role("", "Shinzy", null).Compress(),
				new CreditRecord_Role("", "John Woolley", null).Compress(),
				new CreditRecord_Role("", "Marta Fijak", null).Compress(),
				new CreditRecord_Role("", "Simon Warrener", null).Compress()
			}))
			{
				yield return creditsEntry3;
			}
			enumerator = null;
			yield return new CreditRecord_Space(50f);
			yield return new CreditRecord_Title("Credits_TitleTester".Translate());
			foreach (CreditsEntry creditsEntry4 in CreditsAssembler.<AllCredits>g__Reformat2Cols|0_0(new List<CreditsEntry>
			{
				new CreditRecord_Role("", "Ramsis", null).Compress(),
				new CreditRecord_Role("", "Haplo", null).Compress(),
				new CreditRecord_Role("", "DubskiDude", null).Compress(),
				new CreditRecord_Role("", "Harry Bryant", null).Compress(),
				new CreditRecord_Role("", "ChJees", null).Compress(),
				new CreditRecord_Role("", "AWiseCorn", null).Compress(),
				new CreditRecord_Role("", "Zero747", null).Compress(),
				new CreditRecord_Role("", "Mehni", null).Compress(),
				new CreditRecord_Role("", "XeoNovaDan", null).Compress(),
				new CreditRecord_Role("", "alphaBeta", null).Compress(),
				new CreditRecord_Role("", "TheDee05", null).Compress(),
				new CreditRecord_Role("", "Oglis", null).Compress(),
				new CreditRecord_Role("", "Vas", null).Compress(),
				new CreditRecord_Role("", "Kiaayo", null).Compress(),
				new CreditRecord_Role("", "JimmyAgnt007", null).Compress(),
				new CreditRecord_Role("", "Gouda Quiche", null).Compress(),
				new CreditRecord_Role("", "Drb89", null).Compress(),
				new CreditRecord_Role("", "Jimyoda", null).Compress(),
				new CreditRecord_Role("", "Semmy", null).Compress(),
				new CreditRecord_Role("", "DianaWinters", null).Compress(),
				new CreditRecord_Role("", "Goldenpotatoes", null).Compress(),
				new CreditRecord_Role("", "Skissor", null).Compress(),
				new CreditRecord_Role("", "Laos", null).Compress(),
				new CreditRecord_Role("", "Evul", null).Compress(),
				new CreditRecord_Role("", "SoraHjort", null).Compress(),
				new CreditRecord_Role("", "Coenmjc", null).Compress(),
				new CreditRecord_Role("", "Boris(Eqz)", null).Compress(),
				new CreditRecord_Role("", "MarvinKosh", null).Compress(),
				new CreditRecord_Role("", "Gaesatae", null).Compress(),
				new CreditRecord_Role("", "Letharion", null).Compress(),
				new CreditRecord_Role("", "HeftySmurf", null).Compress(),
				new CreditRecord_Role("", "Skullywag", null).Compress(),
				new CreditRecord_Role("", "Jaxxa", null).Compress(),
				new CreditRecord_Role("", "Helixien", null).Compress(),
				new CreditRecord_Role("", "DeeGee", null).Compress(),
				new CreditRecord_Role("", "ReZpawner", null).Compress(),
				new CreditRecord_Role("", "Doomdrvk", null).Compress(),
				new CreditRecord_Role("", "tedvs", null).Compress(),
				new CreditRecord_Role("", "OneSpellPerDay", null).Compress(),
				new CreditRecord_Role("", "Turbulent Caterwocky", null).Compress(),
				new CreditRecord_Role("", "RawCode", null).Compress(),
				new CreditRecord_Role("", "Enystrom8734", null).Compress(),
				new CreditRecord_Role("", "TeiXeR", null).Compress(),
				new CreditRecord_Role("", "MortalSmurph", null).Compress(),
				new CreditRecord_Role("", "AdamVsEverything", null).Compress(),
				new CreditRecord_Role("", "Fawkes", null).Compress(),
				new CreditRecord_Role("", "LucyFeonix", null).Compress(),
				new CreditRecord_Role("", "Erin", null).Compress(),
				new CreditRecord_Role("", "Ragnar-F", null).Compress()
			}))
			{
				yield return creditsEntry4;
			}
			enumerator = null;
			yield return new CreditRecord_Space(25f);
			yield return new CreditRecord_Role("", "... " + "AndOtherVolunteers".Translate(), null);
			yield return new CreditRecord_Space(200f);
			foreach (LoadedLanguage lang in LanguageDatabase.AllLoadedLanguages)
			{
				lang.LoadMetadata();
				if (lang.info.credits.Count > 0)
				{
					yield return new CreditRecord_Title("Credits_TitleLanguage".Translate(lang.FriendlyNameEnglish, lang.FriendlyNameNative));
				}
				foreach (CreditsEntry creditsEntry5 in CreditsAssembler.<AllCredits>g__Reformat2Cols|0_0(lang.info.credits))
				{
					yield return creditsEntry5;
				}
				enumerator = null;
				lang = null;
			}
			IEnumerator<LoadedLanguage> enumerator2 = null;
			if (ModLister.AnomalyInstalled)
			{
				yield return new CreditRecord_Title("Credits_Localization_Anomaly".Translate());
				yield return new CreditRecord_Title("Local Heroes Worldwide B.V.");
				yield return new CreditRecord_Role("Credits_LeadLocalizationProjectManager".Translate(), "Iris Kuppen", null);
				yield return new CreditRecord_Role("Credits_LocalizationProjectManager".Translate(), "Maikel Roelofs", null);
				yield return new CreditRecord_Title("Credits_French".Translate() + ": Loc-3 Ltd");
				yield return new CreditRecord_Role("Credits_ProjectManager".Translate(), "Eric Emanuel", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Florie Abélard", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Ophélie Colin", null);
				yield return new CreditRecord_Role("Credits_Editor".Translate(), "Claire Deiller", null);
				yield return new CreditRecord_Title("Credits_Japanese".Translate() + ": DICO Co., Ltd.");
				yield return new CreditRecord_Role("Credits_ProjectManager".Translate(), "Nilgül Durali", null);
				yield return new CreditRecord_Role("Credits_ProjectManager".Translate(), "Ziya Sarper Ekim", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Karien Harimoto", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Keigo Yonemura", null);
				yield return new CreditRecord_Role("Credits_Editor".Translate(), "Henry Buckley", null);
				yield return new CreditRecord_Title("Credits_Korean".Translate() + ": DICO Co., Ltd.");
				yield return new CreditRecord_Role("Credits_ProjectManager".Translate(), "Nilgül Durali", null);
				yield return new CreditRecord_Role("Credits_ProjectManager".Translate(), "Ziya Sarper Ekim", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Doyeon Jeong", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Junglim Kim", null);
				yield return new CreditRecord_Role("Credits_Editor".Translate(), "Lim Yoon", null);
				yield return new CreditRecord_Title("Credits_Polish".Translate() + ": Albion Localisations");
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Aleksandra Lubińska", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Łukasz Gładkowski", null);
				yield return new CreditRecord_Role("Credits_Editor".Translate(), "Marcin Bojko", null);
				yield return new CreditRecord_Title("Credits_Russian".Translate() + ": Levsha");
				yield return new CreditRecord_Role("Credits_ProjectManager".Translate(), "Ekaterina Yamenskova", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Vitaliy Hristyuk", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Polina Karpova", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Artyom Petrov", null);
				yield return new CreditRecord_Title("Credits_ChineseSimplified".Translate() + ": Yeehe");
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Jean-Luc Wu", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Roy Liu", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Twig Yu (爽朗的kk23)", null);
				yield return new CreditRecord_Role("Credits_Editor".Translate(), "Marcos Wang", null);
				yield return new CreditRecord_Role("Credits_Editor".Translate(), "ZISHA", null);
				yield return new CreditRecord_Title("Credits_ChineseTraditional".Translate() + ": Cowbay Entertainment");
				yield return new CreditRecord_Role("Credits_ProjectManager".Translate(), "Sean Chen", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Shiou Chen", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Michelle Wu", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Teddy Wu", null);
				yield return new CreditRecord_Role("Credits_Editor".Translate(), "Jerry Lee", null);
			}
			if (ModLister.OdysseyInstalled)
			{
				yield return new CreditRecord_Title("Credits_Localization_Odyssey".Translate());
				yield return new CreditRecord_Title("Local Heroes Worldwide B.V.");
				yield return new CreditRecord_Role("Credits_LeadLocalizationProjectManager".Translate(), "Iris Kuppen", null);
				yield return new CreditRecord_Role("Credits_LocalizationProjectManager".Translate(), "Maikel Roelofs", null);
				yield return new CreditRecord_Title("Credits_Dutch".Translate() + ": The Translation Fource");
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Sandra Vos", null);
				yield return new CreditRecord_Role("Credits_Editor".Translate(), "René van Vemde", null);
				yield return new CreditRecord_Role("Credits_Editor".Translate(), "Maikel Roelofs", null);
				yield return new CreditRecord_Role("Credits_Editor".Translate(), "Sandra Vos", null);
				yield return new CreditRecord_Title("Credits_Russian".Translate() + ": Levsha");
				yield return new CreditRecord_Role("Credits_ProjectManager".Translate(), "Ekaterina Yamenskova", null);
				yield return new CreditRecord_Role("Credits_Editor".Translate(), "Nadezhda Lynova", null);
				yield return new CreditRecord_Title("Credits_Turkish".Translate() + ": Locpick");
				yield return new CreditRecord_Role("Credits_ProjectManager".Translate(), "Batuhan Öztürk", null);
				yield return new CreditRecord_Role("Credits_Editor".Translate(), "Bigem Gözen", null);
				yield return new CreditRecord_Title("Credits_Ukrainian".Translate() + ": Levsha");
				yield return new CreditRecord_Role("Credits_ProjectManager".Translate(), "Ekaterina Yamenskova", null);
				yield return new CreditRecord_Role("Credits_Translator".Translate(), "Vitalii Khrystiuk", null);
				yield return new CreditRecord_Role("Credits_Editor".Translate(), "Konstantin Kopin", null);
			}
			bool firstModCredit = false;
			HashSet<string> allModders = new HashSet<string>();
			List<string> tmpModders = new List<string>();
			foreach (ModMetaData mod in ModsConfig.ActiveModsInLoadOrder.InRandomOrder(null))
			{
				if (!mod.Official)
				{
					tmpModders.Clear();
					tmpModders.AddRange(mod.Authors);
					for (int i = tmpModders.Count - 1; i >= 0; i--)
					{
						tmpModders[i] = tmpModders[i].Trim();
						if (!allModders.Add(tmpModders[i].ToLowerInvariant()))
						{
							tmpModders.RemoveAt(i);
						}
					}
					if (tmpModders.Count > 0)
					{
						foreach (string modder in tmpModders)
						{
							if (!firstModCredit)
							{
								yield return new CreditRecord_Title("Credits_TitleMods".Translate());
								firstModCredit = true;
							}
							yield return new CreditRecord_Role(mod.Name, modder, null).Compress();
							modder = null;
						}
						List<string>.Enumerator enumerator4 = default(List<string>.Enumerator);
					}
					mod = null;
				}
			}
			IEnumerator<ModMetaData> enumerator3 = null;
			yield break;
			yield break;
		}

		// Token: 0x06012420 RID: 74784 RVA: 0x0055F6D2 File Offset: 0x0055D8D2
		[CompilerGenerated]
		internal static IEnumerable<CreditsEntry> <AllCredits>g__Reformat2Cols|0_0(List<CreditsEntry> entries)
		{
			string crediteePrev = null;
			int num;
			for (int i = 0; i < entries.Count; i = num + 1)
			{
				CreditsEntry langCred = entries[i];
				CreditRecord_Role creditRecord_Role = langCred as CreditRecord_Role;
				if (creditRecord_Role != null)
				{
					if (crediteePrev != null)
					{
						yield return new CreditRecord_RoleTwoCols(crediteePrev, creditRecord_Role.creditee, null).Compress();
						crediteePrev = null;
					}
					else
					{
						crediteePrev = creditRecord_Role.creditee;
					}
				}
				else
				{
					if (crediteePrev != null)
					{
						yield return new CreditRecord_RoleTwoCols(crediteePrev, "", null).Compress();
						crediteePrev = null;
					}
					yield return langCred;
				}
				langCred = null;
				num = i;
			}
			if (crediteePrev != null)
			{
				yield return new CreditRecord_RoleTwoCols(crediteePrev, "", null).Compress();
			}
			yield break;
		}
	}
}
