using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200057E RID: 1406
	public class LoadFolder : IEquatable<LoadFolder>
	{
		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060024E2 RID: 9442 RVA: 0x000BE86C File Offset: 0x000BCA6C
		public bool ShouldLoad
		{
			get
			{
				return (this.requiredAnyOfPackageIds.NullOrEmpty<string>() || ModsConfig.IsAnyActiveOrEmpty(this.requiredAnyOfPackageIds, true)) && (this.requiredAllOfPackageIds.NullOrEmpty<string>() || ModsConfig.AreAllActive(this.requiredAllOfPackageIds)) && (this.disallowedAnyOfPackageIds.NullOrEmpty<string>() || !ModsConfig.IsAnyActiveOrEmpty(this.disallowedAnyOfPackageIds, true));
			}
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x000BE8D0 File Offset: 0x000BCAD0
		public LoadFolder(string folderName, List<string> requiredAnyOfPackageIds, List<string> requiredAllOfPackageIds, List<string> disallowedAnyOfPackageIds)
		{
			this.folderName = folderName;
			this.requiredAnyOfPackageIds = requiredAnyOfPackageIds;
			this.requiredAllOfPackageIds = requiredAllOfPackageIds;
			this.disallowedAnyOfPackageIds = disallowedAnyOfPackageIds;
			this.hashCodeCached = ((folderName != null) ? folderName.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (requiredAnyOfPackageIds != null) ? requiredAnyOfPackageIds.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (requiredAllOfPackageIds != null) ? requiredAllOfPackageIds.GetHashCode() : 0);
			this.hashCodeCached = Gen.HashCombine<int>(this.hashCodeCached, (disallowedAnyOfPackageIds != null) ? disallowedAnyOfPackageIds.GetHashCode() : 0);
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x000BE96B File Offset: 0x000BCB6B
		public bool Equals(LoadFolder other)
		{
			return other != null && this.hashCodeCached == other.GetHashCode();
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x000BE980 File Offset: 0x000BCB80
		public override bool Equals(object obj)
		{
			LoadFolder loadFolder = obj as LoadFolder;
			return loadFolder != null && this.Equals(loadFolder);
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x000BE9A0 File Offset: 0x000BCBA0
		public override int GetHashCode()
		{
			return this.hashCodeCached;
		}

		// Token: 0x04001E62 RID: 7778
		public string folderName;

		// Token: 0x04001E63 RID: 7779
		public List<string> requiredAnyOfPackageIds;

		// Token: 0x04001E64 RID: 7780
		public List<string> requiredAllOfPackageIds;

		// Token: 0x04001E65 RID: 7781
		public List<string> disallowedAnyOfPackageIds;

		// Token: 0x04001E66 RID: 7782
		private readonly int hashCodeCached;
	}
}
