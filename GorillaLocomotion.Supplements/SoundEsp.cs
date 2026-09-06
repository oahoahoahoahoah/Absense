using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

// Token: 0x02000035 RID: 53
public static class SoundEsp
{
	// Token: 0x0600018E RID: 398 RVA: 0x0001BBE4 File Offset: 0x00019DE4
	public static string[] GetPresetNames()
	{
		return SoundEsp.presetNames;
	}

	// Token: 0x0600018F RID: 399 RVA: 0x0001BBFC File Offset: 0x00019DFC
	public static void Update()
	{
		bool soundESPEnabled = Settings.SoundESPEnabled;
		if (soundESPEnabled)
		{
			bool flag = GorillaTagger.Instance == null || VRRigCache.ActiveRigs == null;
			if (!flag)
			{
				bool flag2 = SoundEsp.source == null;
				if (flag2)
				{
					SoundEsp.source = new GameObject("SoundESP_Audio")
					{
						hideFlags = 61
					}.AddComponent<AudioSource>();
					SoundEsp.source.spatialBlend = 0f;
					AudioSource val2 = SoundEsp.source;
					val2.playOnAwake = false;
				}
				bool flag3 = Settings.SoundESPCustomPath != SoundEsp.customPath;
				if (flag3)
				{
					SoundEsp.LoadWavFromFile(Settings.SoundESPCustomPath);
					SoundEsp.customPath = Settings.SoundESPCustomPath;
				}
				bool flag4 = Time.time - SoundEsp.lastPlayTime < Settings.SoundESPCooldown || GorillaTagger.Instance.offlineVRRig == null || SoundEsp.IsTagged(GorillaTagger.Instance.offlineVRRig);
				if (!flag4)
				{
					Vector3 position = GorillaTagger.Instance.bodyCollider.transform.position;
					foreach (VRRig activeRig in VRRigCache.ActiveRigs)
					{
						bool flag5 = !(activeRig == null) && !(activeRig == GorillaTagger.Instance.offlineVRRig) && !(activeRig.headMesh == null) && SoundEsp.IsTagged(activeRig);
						if (flag5)
						{
							float num = Vector3.Distance(activeRig.headMesh.transform.position, position);
							bool flag6 = num <= Settings.SoundESPDistance;
							if (flag6)
							{
								SoundEsp.PlayForDistance(num);
								SoundEsp.lastPlayTime = Time.time;
								break;
							}
						}
					}
					SoundEsp.initialized = true;
				}
			}
		}
		else
		{
			bool flag7 = SoundEsp.initialized;
			if (flag7)
			{
				SoundEsp.initialized = false;
			}
		}
	}

	// Token: 0x06000190 RID: 400 RVA: 0x0001BDF4 File Offset: 0x00019FF4
	private static bool IsTagged(VRRig rig)
	{
		bool flag = !(rig != null);
		bool flag2;
		if (flag)
		{
			flag2 = false;
		}
		else
		{
			bool flag3 = !(rig.mainSkin != null);
			if (flag3)
			{
				flag2 = false;
			}
			else
			{
				bool flag4 = !(rig.mainSkin.material != null);
				flag2 = !flag4 && rig.mainSkin.material.name.ToLower().Contains("fected");
			}
		}
		return flag2;
	}

	// Token: 0x06000191 RID: 401 RVA: 0x0001BE70 File Offset: 0x0001A070
	private static void PlayForDistance(float samples)
	{
		bool flag = !(SoundEsp.source == null);
		if (flag)
		{
			AudioClip val = SoundEsp.BuildCustomClip();
			bool flag2 = !(val == null);
			if (flag2)
			{
				SoundEsp.source.Play();
			}
		}
	}

	// Token: 0x06000192 RID: 402 RVA: 0x0001BEBC File Offset: 0x0001A0BC
	private static AudioClip BuildCustomClip()
	{
		bool flag = Settings.SoundESPPreset != 0;
		AudioClip audioClip;
		if (flag)
		{
			audioClip = SoundEsp.BuildPresetClip(Settings.SoundESPPreset);
		}
		else
		{
			bool flag2 = !(SoundEsp.customClip != null);
			if (flag2)
			{
				audioClip = SoundEsp.BuildPresetClip(Settings.SoundESPPreset);
			}
			else
			{
				audioClip = SoundEsp.customClip;
			}
		}
		return audioClip;
	}

	// Token: 0x06000193 RID: 403 RVA: 0x0001BF10 File Offset: 0x0001A110
	private static AudioClip BuildPresetClip(int index)
	{
		bool flag = SoundEsp.presetClips.ContainsKey(index);
		AudioClip audioClip;
		if (flag)
		{
			audioClip = SoundEsp.presetClips[index];
		}
		else
		{
			int num3 = 44100;
			int num4 = num3 / 4;
			float[] array = new float[num4];
			float num5;
			switch (index)
			{
			case 1:
				num5 = 800f;
				break;
			case 2:
				num5 = 1200f;
				break;
			case 3:
				num5 = 400f;
				break;
			default:
				num5 = 800f;
				break;
			}
			int num6 = 0;
			for (;;)
			{
				bool flag2 = num6 < num4;
				if (!flag2)
				{
					break;
				}
				float num7 = (float)num6 / (float)num3;
				float num8 = 1f - (float)num6 / (float)num4;
				array[num6] = Mathf.Sin(6.28318548f * num5 * num7) * num8 * 0.5f;
				num6++;
			}
			string text = "Preset_" + index.ToString();
			int num9 = num4;
			AudioClip val = AudioClip.Create(text, num9, 1, num3, false);
			SoundEsp.FillSamples(val, array);
			SoundEsp.presetClips[index] = val;
			audioClip = val;
		}
		return audioClip;
	}

	// Token: 0x06000194 RID: 404 RVA: 0x0001C040 File Offset: 0x0001A240
	private static void LoadWavFromFile(string path)
	{
		SoundEsp.customClip = null;
		bool flag = string.IsNullOrEmpty(path) || !File.Exists(path);
		if (!flag)
		{
			try
			{
				byte[] array = File.ReadAllBytes(path);
				bool flag2 = array.Length <= 44 || array[0] != 82 || array[1] != 73;
				if (!flag2)
				{
					int num2 = (int)array[22] | ((int)array[23] << 8);
					int num3 = (int)array[24] | ((int)array[25] << 8) | ((int)array[26] << 16) | ((int)array[27] << 24);
					int num4 = (array.Length - 44) / 2;
					float[] array2 = new float[num4];
					for (int i = 0; i < num4; i++)
					{
						int num5 = 44 + i * 2;
						bool flag3 = num5 + 1 < array.Length;
						if (flag3)
						{
							short num6 = (short)((int)array[num5] | ((int)array[num5 + 1] << 8));
							array2[i] = (float)num6 / 32768f;
						}
					}
					SoundEsp.customClip = AudioClip.Create("CustomSound", num4 / num2, num2, num3, false);
					SoundEsp.FillSamples(SoundEsp.customClip, array2);
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x06000195 RID: 405 RVA: 0x0001C174 File Offset: 0x0001A374
	private static void FillSamples(AudioClip clip, float[] samples)
	{
		try
		{
			Type typeFromHandle = typeof(AudioClip);
			MethodInfo method = typeFromHandle.GetMethod("SetData", new Type[]
			{
				typeof(float[]),
				typeof(int)
			});
			bool flag = method != null;
			if (flag)
			{
				method.Invoke(clip, new object[] { samples, 0 });
			}
		}
		catch
		{
		}
	}

	// Token: 0x04000432 RID: 1074
	private static AudioSource source;

	// Token: 0x04000433 RID: 1075
	private static AudioClip customClip;

	// Token: 0x04000434 RID: 1076
	private static AudioClip presetClip;

	// Token: 0x04000435 RID: 1077
	private static string customPath = "";

	// Token: 0x04000436 RID: 1078
	private static float lastPlayTime = 0f;

	// Token: 0x04000437 RID: 1079
	private static bool initialized = false;

	// Token: 0x04000438 RID: 1080
	private static readonly string[] presetNames = new string[] { "Ping", "Beep", "Click" };

	// Token: 0x04000439 RID: 1081
	private static Dictionary<int, AudioClip> presetClips = new Dictionary<int, AudioClip>();
}
