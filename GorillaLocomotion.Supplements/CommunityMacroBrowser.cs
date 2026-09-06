using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200000C RID: 12
public static class CommunityMacroBrowser
{
	// Token: 0x0600001A RID: 26 RVA: 0x00002CB7 File Offset: 0x00000EB7
	public static void Refresh(string arg = "")
	{
		CommunityMacroBrowser.StatusMessage = "Offline — auth disabled.";
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002CC4 File Offset: 0x00000EC4
	public static void Upload(string arg, string data)
	{
		CommunityMacroBrowser.StatusMessage = "Offline — auth disabled.";
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002CD1 File Offset: 0x00000ED1
	public static void Like(string arg, string data)
	{
		CommunityMacroBrowser.StatusMessage = "Offline — auth disabled.";
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002CDE File Offset: 0x00000EDE
	public static void Download(string arg)
	{
		CommunityMacroBrowser.StatusMessage = "Offline — auth disabled.";
	}

	// Token: 0x04000037 RID: 55
	public static List<CommunityMacro> CachedMacros = new List<CommunityMacro>();

	// Token: 0x04000038 RID: 56
	public static bool IsLoading = false;

	// Token: 0x04000039 RID: 57
	public static bool IsOperating = false;

	// Token: 0x0400003A RID: 58
	public static string StatusMessage = "";

	// Token: 0x0400003B RID: 59
	public static string SearchQuery = "";

	// Token: 0x0400003C RID: 60
	public static Vector2 ScrollPosition = Vector2.zero;
}
