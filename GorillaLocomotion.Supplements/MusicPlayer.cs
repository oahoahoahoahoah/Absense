using System;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

// Token: 0x02000021 RID: 33
public static class MusicPlayer
{
	// Token: 0x060000EA RID: 234 RVA: 0x00012ED4 File Offset: 0x000110D4
	private static AudioSource EnsureAudioSource()
	{
		bool flag = MusicPlayer.source != null;
		AudioSource audioSource;
		if (flag)
		{
			audioSource = MusicPlayer.source;
		}
		else
		{
			Camera camera = Camera.main;
			bool flag2 = camera == null;
			if (flag2)
			{
				audioSource = null;
			}
			else
			{
				MusicPlayer.source = camera.GetComponent<AudioSource>() ?? camera.gameObject.AddComponent<AudioSource>();
				MusicPlayer.source.loop = true;
				MusicPlayer.source.spatialBlend = 0f;
				MusicPlayer.source.volume = 0.6f;
				MusicPlayer.source.playOnAwake = false;
				audioSource = MusicPlayer.source;
			}
		}
		return audioSource;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00012F6C File Offset: 0x0001116C
	public static void Play()
	{
		AudioSource audioSource = MusicPlayer.EnsureAudioSource();
		bool flag = audioSource == null;
		if (!flag)
		{
			bool flag2 = MusicPlayer.clip != null;
			if (flag2)
			{
				bool flag3 = !audioSource.isPlaying;
				if (flag3)
				{
					audioSource.clip = MusicPlayer.clip;
					audioSource.Play();
				}
			}
			else
			{
				bool flag4 = !MusicPlayer.downloading;
				if (flag4)
				{
					MusicPlayer.downloading = true;
					MusicPlayer.playing = false;
					MusicPlayer.cachePath = null;
					ThreadPool.QueueUserWorkItem(delegate(object _)
					{
						MusicPlayer.DownloadClip();
					});
				}
			}
		}
	}

	// Token: 0x060000EC RID: 236 RVA: 0x0001300C File Offset: 0x0001120C
	public static void SetVolume()
	{
		bool flag = MusicPlayer.playing && MusicPlayer.cachePath != null;
		if (flag)
		{
			MusicPlayer.playing = false;
			MusicPlayer.LoadClip(MusicPlayer.cachePath);
		}
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00013044 File Offset: 0x00011244
	public static void Stop()
	{
		bool flag = MusicPlayer.source != null && MusicPlayer.source.isPlaying;
		if (flag)
		{
			MusicPlayer.source.Stop();
		}
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00013080 File Offset: 0x00011280
	private static void DownloadClip()
	{
		try
		{
			string path = Path.Combine(Application.temporaryCachePath, "absense_music.mp3");
			bool flag = !File.Exists(path) || new FileInfo(path).Length == 0L;
			if (flag)
			{
				using (WebClient client = new WebClient())
				{
					client.DownloadFile("https://absentauth.com/assets/music.mp3", path);
				}
			}
			MusicPlayer.cachePath = path;
			MusicPlayer.playing = true;
		}
		catch
		{
			MusicPlayer.downloading = false;
		}
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00013118 File Offset: 0x00011318
	private static void LoadClip(string path)
	{
		try
		{
			UnityWebRequest loadRequest = UnityWebRequestMultimedia.GetAudioClip("file://" + path, 13);
			loadRequest.SendWebRequest();
			MusicPlayer.request = loadRequest;
		}
		catch
		{
			MusicPlayer.downloading = false;
		}
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x00013168 File Offset: 0x00011368
	public static void PollDownload()
	{
		bool flag = MusicPlayer.request == null || !MusicPlayer.request.isDone;
		if (!flag)
		{
			try
			{
				MusicPlayer.clip = DownloadHandlerAudioClip.GetContent(MusicPlayer.request);
				AudioSource audioSource = MusicPlayer.EnsureAudioSource();
				bool flag2 = MusicPlayer.clip != null && audioSource != null;
				if (flag2)
				{
					audioSource.clip = MusicPlayer.clip;
					audioSource.Play();
				}
			}
			finally
			{
				MusicPlayer.request.Dispose();
				MusicPlayer.request = null;
				MusicPlayer.downloading = false;
			}
		}
	}

	// Token: 0x04000221 RID: 545
	private const string MusicUrl = "https://absentauth.com/assets/music.mp3";

	// Token: 0x04000222 RID: 546
	private static AudioClip clip;

	// Token: 0x04000223 RID: 547
	private static AudioSource source;

	// Token: 0x04000224 RID: 548
	private static bool downloading;

	// Token: 0x04000225 RID: 549
	private static bool playing;

	// Token: 0x04000226 RID: 550
	private static string cachePath;

	// Token: 0x04000227 RID: 551
	private static UnityWebRequest request;
}
