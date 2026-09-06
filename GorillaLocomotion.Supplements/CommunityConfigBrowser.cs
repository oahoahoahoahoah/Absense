using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

// Token: 0x0200000A RID: 10
public static class CommunityConfigBrowser
{
	// Token: 0x06000012 RID: 18 RVA: 0x00002ADC File Offset: 0x00000CDC
	public static void Refresh(string arg = "")
	{
		CommunityConfigBrowser.IsLoading = true;
		CommunityConfigBrowser.StatusMessage = "";
		ThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				string text = ConfigManager.PostApi("/api/community/list", ",\"search\":\"" + ConfigManager.JsonEscape(arg ?? "") + "\"}");
				CommunityConfigBrowser.CachedConfigs.Clear();
				bool flag = ConfigManager.GetJsonInt(text, "r") == 1;
				if (flag)
				{
					string text2 = "";
					int num = text.IndexOf("\"configs\"");
					bool flag2 = num >= 0;
					if (flag2)
					{
						int num2 = text.IndexOf('[', num);
						int num3 = text.LastIndexOf(']');
						bool flag3 = num2 >= 0 && num3 > num2;
						if (flag3)
						{
							text2 = text.Substring(num2 + 1, num3 - num2 - 1);
						}
					}
					int startIndex = 0;
					for (;;)
					{
						int num4 = text2.IndexOf('{', startIndex);
						bool flag4 = num4 < 0;
						if (flag4)
						{
							break;
						}
						int num5 = text2.IndexOf('}', num4);
						bool flag5 = num5 < 0;
						if (flag5)
						{
							break;
						}
						string data = text2.Substring(num4, num5 - num4 + 1);
						startIndex = num5 + 1;
						CommunityConfig gClass = new CommunityConfig
						{
							Id = ConfigManager.GetJsonString(data, "id"),
							Name = ConfigManager.GetJsonString(data, "name"),
							UploaderName = ConfigManager.GetJsonString(data, "uploaderName"),
							Downloads = ConfigManager.GetJsonInt(data, "downloads"),
							Likes = ConfigManager.GetJsonInt(data, "likes"),
							Liked = (ConfigManager.GetJsonString(data, "liked") == "true"),
							CreatedAt = ConfigManager.GetJsonString(data, "createdAt"),
							UpdatedAt = ConfigManager.GetJsonString(data, "updatedAt")
						};
						bool flag6 = !string.IsNullOrEmpty(gClass.Id);
						if (flag6)
						{
							CommunityConfigBrowser.CachedConfigs.Add(gClass);
						}
					}
				}
				CommunityConfigBrowser.StatusMessage = "Found " + CommunityConfigBrowser.CachedConfigs.Count.ToString() + " configs";
			}
			catch
			{
				CommunityConfigBrowser.StatusMessage = "Failed to load";
			}
			finally
			{
				CommunityConfigBrowser.IsLoading = false;
			}
		});
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002B1C File Offset: 0x00000D1C
	public static void Upload(string arg, string data)
	{
		CommunityConfigBrowser.IsOperating = true;
		CommunityConfigBrowser.StatusMessage = "Downloading...";
		ThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				string string_3 = ConfigManager.PostApi("/api/community/download", ",\"id\":\"" + ConfigManager.JsonEscape(arg) + "\"}");
				string text = ConfigManager.GetJsonString(string_3, "data");
				bool flag = ConfigManager.GetJsonInt(string_3, "r") == 1 && !string.IsNullOrEmpty(text);
				if (flag)
				{
					ConfigManager.UploadConfig(text, data);
					CommunityConfigBrowser.StatusMessage = "Loaded: " + data;
				}
				else
				{
					CommunityConfigBrowser.StatusMessage = "Download failed";
				}
			}
			catch
			{
				CommunityConfigBrowser.StatusMessage = "Download failed";
			}
			finally
			{
				CommunityConfigBrowser.IsOperating = false;
			}
		});
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002B60 File Offset: 0x00000D60
	public static void Like(string arg)
	{
		CommunityConfigBrowser.IsOperating = true;
		CommunityConfigBrowser.StatusMessage = "Publishing...";
		ThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				string data = ConfigManager.ExportCurrentConfig();
				string string_3 = ConfigManager.PostApi("/api/community/publish", string.Concat(new string[]
				{
					",\"name\":\"",
					ConfigManager.JsonEscape(arg),
					"\",\"data\":\"",
					ConfigManager.JsonEscape(data),
					"\"}"
				}));
				bool flag = ConfigManager.GetJsonInt(string_3, "r") == 1;
				if (flag)
				{
					CommunityConfigBrowser.StatusMessage = "Published!";
				}
				else
				{
					CommunityConfigBrowser.StatusMessage = ConfigManager.GetJsonString(string_3, "e");
				}
			}
			catch
			{
				CommunityConfigBrowser.StatusMessage = "Publish failed";
			}
			finally
			{
				CommunityConfigBrowser.IsOperating = false;
			}
		});
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002BA0 File Offset: 0x00000DA0
	public static void Delete(string arg)
	{
		ThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				ConfigManager.PostApi("/api/community/like", ",\"id\":\"" + ConfigManager.JsonEscape(arg) + "\"}");
			}
			catch
			{
			}
		});
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002BD0 File Offset: 0x00000DD0
	public static void Download(string arg)
	{
		CommunityConfigBrowser.IsOperating = true;
		CommunityConfigBrowser.StatusMessage = "Deleting...";
		ThreadPool.QueueUserWorkItem(delegate
		{
			try
			{
				string data = ConfigManager.PostApi("/api/community/delete", ",\"id\":\"" + ConfigManager.JsonEscape(arg) + "\"}");
				bool flag = ConfigManager.GetJsonInt(data, "r") == 1;
				if (flag)
				{
					CommunityConfigBrowser.CachedConfigs.RemoveAll((CommunityConfig gclass36_0) => gclass36_0.Id == arg);
					CommunityConfigBrowser.StatusMessage = "Deleted!";
				}
				else
				{
					CommunityConfigBrowser.StatusMessage = ConfigManager.GetJsonString(data, "e");
				}
			}
			catch
			{
				CommunityConfigBrowser.StatusMessage = "Delete failed";
			}
			finally
			{
				CommunityConfigBrowser.IsOperating = false;
			}
		});
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002C10 File Offset: 0x00000E10
	public static string FormatTimestamp(string arg)
	{
		bool flag = string.IsNullOrEmpty(arg);
		string text;
		if (flag)
		{
			text = "Unknown";
		}
		else
		{
			try
			{
				text = DateTime.Parse(arg).ToString("MMM dd, yyyy");
			}
			catch
			{
				text = ((arg.Length > 10) ? arg.Substring(0, 10) : arg);
			}
		}
		return text;
	}

	// Token: 0x0400002A RID: 42
	private const string string_0 = "https://absentauth.com";

	// Token: 0x0400002B RID: 43
	public static List<CommunityConfig> CachedConfigs = new List<CommunityConfig>();

	// Token: 0x0400002C RID: 44
	public static bool IsLoading = false;

	// Token: 0x0400002D RID: 45
	public static bool IsOperating = false;

	// Token: 0x0400002E RID: 46
	public static string StatusMessage = "";

	// Token: 0x0400002F RID: 47
	public static string SearchQuery = "";

	// Token: 0x04000030 RID: 48
	public static Vector2 ScrollPosition = Vector2.zero;

	// Token: 0x02000047 RID: 71
	[CompilerGenerated]
	private sealed class Class27
	{
		// Token: 0x06000201 RID: 513 RVA: 0x00021C38 File Offset: 0x0001FE38
		internal void method_0(object object_0)
		{
			try
			{
				ConfigManager.PostApi("/api/community/like", ",\"id\":\"" + ConfigManager.JsonEscape(this.configId) + "\"}");
			}
			catch
			{
			}
		}

		// Token: 0x040004B6 RID: 1206
		public string configId;
	}

	// Token: 0x02000048 RID: 72
	[CompilerGenerated]
	private sealed class Class28
	{
		// Token: 0x06000203 RID: 515 RVA: 0x00021C90 File Offset: 0x0001FE90
		internal void method_0(object object_0)
		{
			try
			{
				string string_ = ConfigManager.PostApi("/api/community/delete", ",\"id\":\"" + ConfigManager.JsonEscape(this.configId) + "\"}");
				bool flag = ConfigManager.GetJsonInt(string_, "r") == 1;
				if (flag)
				{
					CommunityConfigBrowser.CachedConfigs.RemoveAll((CommunityConfig gclass36_0) => gclass36_0.Id == this.configId);
					CommunityConfigBrowser.StatusMessage = "Deleted!";
				}
				else
				{
					CommunityConfigBrowser.StatusMessage = ConfigManager.GetJsonString(string_, "e");
				}
			}
			catch
			{
				CommunityConfigBrowser.StatusMessage = "Delete failed";
			}
			finally
			{
				CommunityConfigBrowser.IsOperating = false;
			}
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00021D48 File Offset: 0x0001FF48
		internal bool method_1(CommunityConfig gclass36_0)
		{
			return gclass36_0.Id == this.configId;
		}

		// Token: 0x040004B7 RID: 1207
		public string configId;

		// Token: 0x040004B8 RID: 1208
		public Predicate<CommunityConfig> _003C_003E9__1;
	}

	// Token: 0x02000049 RID: 73
	[CompilerGenerated]
	private sealed class Class29
	{
		// Token: 0x06000207 RID: 519 RVA: 0x00021D88 File Offset: 0x0001FF88
		internal void method_0(object object_0)
		{
			try
			{
				string text = ConfigManager.PostApi("/api/community/list", ",\"search\":\"" + ConfigManager.JsonEscape(this.search ?? "") + "\"}");
				CommunityConfigBrowser.CachedConfigs.Clear();
				bool flag = ConfigManager.GetJsonInt(text, "r") == 1;
				if (flag)
				{
					string text2 = "";
					int num = text.IndexOf("\"configs\"");
					bool flag2 = num >= 0;
					if (flag2)
					{
						int num2 = text.IndexOf('[', num);
						int num3 = text.LastIndexOf(']');
						bool flag3 = num2 >= 0 && num3 > num2;
						if (flag3)
						{
							text2 = text.Substring(num2 + 1, num3 - num2 - 1);
						}
					}
					int startIndex = 0;
					for (;;)
					{
						int num4 = text2.IndexOf('{', startIndex);
						bool flag4 = num4 < 0;
						if (flag4)
						{
							break;
						}
						int num5 = text2.IndexOf('}', num4);
						bool flag5 = num5 < 0;
						if (flag5)
						{
							break;
						}
						string string_ = text2.Substring(num4, num5 - num4 + 1);
						startIndex = num5 + 1;
						CommunityConfig gClass = new CommunityConfig
						{
							Id = ConfigManager.GetJsonString(string_, "id"),
							Name = ConfigManager.GetJsonString(string_, "name"),
							UploaderName = ConfigManager.GetJsonString(string_, "uploaderName"),
							Downloads = ConfigManager.GetJsonInt(string_, "downloads"),
							Likes = ConfigManager.GetJsonInt(string_, "likes"),
							Liked = (ConfigManager.GetJsonString(string_, "liked") == "true"),
							CreatedAt = ConfigManager.GetJsonString(string_, "createdAt"),
							UpdatedAt = ConfigManager.GetJsonString(string_, "updatedAt")
						};
						bool flag6 = !string.IsNullOrEmpty(gClass.Id);
						if (flag6)
						{
							CommunityConfigBrowser.CachedConfigs.Add(gClass);
						}
					}
				}
				CommunityConfigBrowser.StatusMessage = "Found " + CommunityConfigBrowser.CachedConfigs.Count.ToString() + " configs";
			}
			catch
			{
				CommunityConfigBrowser.StatusMessage = "Failed to load";
			}
			finally
			{
				CommunityConfigBrowser.IsLoading = false;
			}
		}

		// Token: 0x040004B9 RID: 1209
		public string search;
	}

	// Token: 0x0200004A RID: 74
	[CompilerGenerated]
	private sealed class Class30
	{
		// Token: 0x06000209 RID: 521 RVA: 0x00021FF4 File Offset: 0x000201F4
		internal void method_0(object object_0)
		{
			try
			{
				string string_ = ConfigManager.PostApi("/api/community/download", ",\"id\":\"" + ConfigManager.JsonEscape(this.configId) + "\"}");
				string text = ConfigManager.GetJsonString(string_, "data");
				bool flag = ConfigManager.GetJsonInt(string_, "r") == 1 && !string.IsNullOrEmpty(text);
				if (flag)
				{
					ConfigManager.UploadConfig(text, this.displayName);
					CommunityConfigBrowser.StatusMessage = "Loaded: " + this.displayName;
				}
				else
				{
					CommunityConfigBrowser.StatusMessage = "Download failed";
				}
			}
			catch
			{
				CommunityConfigBrowser.StatusMessage = "Download failed";
			}
			finally
			{
				CommunityConfigBrowser.IsOperating = false;
			}
		}

		// Token: 0x040004BA RID: 1210
		public string configId;

		// Token: 0x040004BB RID: 1211
		public string displayName;
	}

	// Token: 0x0200004B RID: 75
	[CompilerGenerated]
	private sealed class Class31
	{
		// Token: 0x0600020B RID: 523 RVA: 0x000220CC File Offset: 0x000202CC
		internal void method_0(object object_0)
		{
			try
			{
				string string_ = ConfigManager.ExportCurrentConfig();
				string data = ConfigManager.PostApi("/api/community/publish", string.Concat(new string[]
				{
					",\"name\":\"",
					ConfigManager.JsonEscape(this.configName),
					"\",\"data\":\"",
					ConfigManager.JsonEscape(string_),
					"\"}"
				}));
				bool flag = ConfigManager.GetJsonInt(data, "r") == 1;
				if (flag)
				{
					CommunityConfigBrowser.StatusMessage = "Published!";
				}
				else
				{
					CommunityConfigBrowser.StatusMessage = ConfigManager.GetJsonString(data, "e");
				}
			}
			catch
			{
				CommunityConfigBrowser.StatusMessage = "Publish failed";
			}
			finally
			{
				CommunityConfigBrowser.IsOperating = false;
			}
		}

		// Token: 0x040004BC RID: 1212
		public string configName;
	}
}
