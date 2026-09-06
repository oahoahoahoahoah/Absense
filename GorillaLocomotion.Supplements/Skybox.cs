using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

// Token: 0x02000033 RID: 51
public static class Skybox
{
	// Token: 0x06000173 RID: 371 RVA: 0x0001A8AC File Offset: 0x00018AAC
	public static void Update()
	{
		bool flag = !Settings.SkyboxEnabled;
		if (flag)
		{
			Skybox.Disable();
		}
		else
		{
			bool flag2 = !(Skybox.skyboxRenderer == null);
			if (flag2)
			{
				bool flag3 = !(Skybox.skyboxObject == null);
				if (!flag3)
				{
					Skybox.Rebuild();
				}
				int skyboxMode2 = Settings.SkyboxMode;
				bool flag4 = skyboxMode2 != 2;
				if (flag4)
				{
					Skybox.UpdateGradient();
				}
				else
				{
					bool flag5 = !(Skybox.skyboxRenderer != null);
					if (flag5)
					{
						Skybox.UpdateGradient();
					}
					else
					{
						Skybox.BuildStarfield();
					}
				}
				int skyboxMode3 = Settings.SkyboxMode;
				bool flag6 = skyboxMode3 == 3;
				if (flag6)
				{
					bool flag7 = Skybox.skyboxRenderer != null;
					if (flag7)
					{
						Skybox.UpdateStarfield();
					}
					else
					{
						Skybox.ApplyStarfieldMode();
					}
				}
				else
				{
					Skybox.ApplyStarfieldMode();
				}
			}
			else
			{
				Skybox.Rebuild();
				int skyboxMode2 = Settings.SkyboxMode;
				bool flag8 = skyboxMode2 != 2;
				if (flag8)
				{
					Skybox.UpdateGradient();
				}
				else
				{
					bool flag9 = !(Skybox.skyboxRenderer != null);
					if (flag9)
					{
						Skybox.UpdateGradient();
					}
					else
					{
						Skybox.BuildStarfield();
					}
				}
				int skyboxMode3 = Settings.SkyboxMode;
				bool flag10 = skyboxMode3 == 3;
				if (flag10)
				{
					bool flag11 = Skybox.skyboxRenderer != null;
					if (flag11)
					{
						Skybox.UpdateStarfield();
					}
					else
					{
						Skybox.ApplyStarfieldMode();
					}
				}
				else
				{
					Skybox.ApplyStarfieldMode();
				}
			}
		}
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0001AA1C File Offset: 0x00018C1C
	public static void Rebuild()
	{
		try
		{
			Skybox.skyboxObject = GameObject.Find("Environment Objects/LocalObjects_Prefab/Standard Sky");
			bool flag = Skybox.skyboxObject == null;
			if (flag)
			{
				Skybox.Status = "Dome not found (load Forest)";
			}
			else
			{
				Skybox.BuildSkyboxObject();
				bool flag2 = Skybox.skyboxRenderer == null;
				if (flag2)
				{
					Skybox.Status = "Failed to add renderer";
				}
				else
				{
					Skybox.ApplyMode();
					Skybox.skyboxRenderer.sharedMaterial = Skybox.skyboxMaterial;
					Skybox.skyboxRenderer.enabled = true;
					Skybox.Status = "Applied (" + Skybox.GetModeName() + ")";
				}
			}
		}
		catch (Exception ex)
		{
			Skybox.Status = "Error: " + ex.Message;
		}
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0001AAE0 File Offset: 0x00018CE0
	private static void BuildSkyboxObject()
	{
		bool flag = Skybox.skyboxObject.GetComponent<MeshFilter>() == null;
		if (flag)
		{
			Mesh val = Skybox.CreateSphereMesh();
			bool flag2 = !(val != null);
			if (flag2)
			{
			}
		}
		MeshRenderer val2 = Skybox.skyboxObject.GetComponent<MeshRenderer>();
		bool flag3 = !(val2 == null);
		if (flag3)
		{
			bool flag4 = Skybox.imageLoaded;
			if (!flag4)
			{
				Skybox.imageLoaded = true;
			}
		}
		else
		{
			val2 = Skybox.skyboxObject.AddComponent<MeshRenderer>();
		}
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0001AB64 File Offset: 0x00018D64
	private static Mesh CreateSphereMesh()
	{
		MeshCollider val = Skybox.skyboxObject.GetComponent<MeshCollider>();
		bool flag = !(val != null);
		Mesh mesh;
		if (flag)
		{
			bool flag2 = !(Skybox.skyboxObject.GetComponent<CapsuleCollider>() != null);
			if (flag2)
			{
				bool flag3 = !(Skybox.skyboxObject.GetComponent<SphereCollider>() != null);
				if (flag3)
				{
					bool flag4 = Skybox.skyboxObject.GetComponent<BoxCollider>() != null;
					if (flag4)
					{
						mesh = Skybox.GetPrimitiveMesh(3, ref Skybox.quadMesh);
					}
					else
					{
						mesh = Skybox.GetPrimitiveMesh(1, ref Skybox.sphereMesh);
					}
				}
				else
				{
					mesh = Skybox.GetPrimitiveMesh(0, ref Skybox.starMesh);
				}
			}
			else
			{
				mesh = Skybox.GetPrimitiveMesh(1, ref Skybox.sphereMesh);
			}
		}
		else
		{
			bool flag5 = !(val.sharedMesh != null);
			if (flag5)
			{
				bool flag6 = !(Skybox.skyboxObject.GetComponent<CapsuleCollider>() != null);
				if (flag6)
				{
					bool flag7 = !(Skybox.skyboxObject.GetComponent<SphereCollider>() != null);
					if (flag7)
					{
						bool flag8 = Skybox.skyboxObject.GetComponent<BoxCollider>() != null;
						if (flag8)
						{
							mesh = Skybox.GetPrimitiveMesh(3, ref Skybox.quadMesh);
						}
						else
						{
							mesh = Skybox.GetPrimitiveMesh(1, ref Skybox.sphereMesh);
						}
					}
					else
					{
						mesh = Skybox.GetPrimitiveMesh(0, ref Skybox.starMesh);
					}
				}
				else
				{
					mesh = Skybox.GetPrimitiveMesh(1, ref Skybox.sphereMesh);
				}
			}
			else
			{
				mesh = val.sharedMesh;
			}
		}
		return mesh;
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0001ACD4 File Offset: 0x00018ED4
	private static Mesh GetPrimitiveMesh(PrimitiveType primitive, ref Mesh mesh)
	{
		bool flag = mesh == null;
		if (flag)
		{
			GameObject val = GameObject.CreatePrimitive(primitive);
			val.hideFlags = 61;
			MeshFilter component = val.GetComponent<MeshFilter>();
			bool flag2 = component != null;
			if (flag2)
			{
				mesh = component.sharedMesh;
			}
			Object.Destroy(val);
		}
		return mesh;
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0001AD2C File Offset: 0x00018F2C
	private static void ApplyMode()
	{
		bool flag = Skybox.skyboxMaterial == null;
		if (flag)
		{
			Shader val = Shader.Find("Universal Render Pipeline/Unlit");
			bool flag2 = val == null;
			if (flag2)
			{
				val = Shader.Find("Unlit/Texture");
			}
			bool flag3 = val == null;
			if (flag3)
			{
				val = Shader.Find("Sprites/Default");
			}
			Skybox.skyboxMaterial = new Material(val);
			Material val2 = Skybox.skyboxMaterial;
			val2.hideFlags = 61;
		}
		bool flag4 = Skybox.skyboxMaterial.HasProperty("_Cull");
		if (flag4)
		{
			Material val3 = Skybox.skyboxMaterial;
			val3.SetInt("_Cull", 0);
		}
		Texture2D val4 = null;
		Color val5 = Color.white;
		switch (Settings.SkyboxMode)
		{
		case 0:
			val5 = Color.HSVToRGB(Settings.SkyboxColorHue, Settings.SkyboxColorSat, Settings.SkyboxColorVal);
			break;
		case 1:
		{
			val4 = Skybox.LoadTexture(Settings.SkyboxImagePath);
			bool flag5 = val4 == null;
			if (flag5)
			{
				Skybox.Status = "Image not found";
				val5 = Color.HSVToRGB(Settings.SkyboxColorHue, Settings.SkyboxColorSat, Settings.SkyboxColorVal);
			}
			break;
		}
		case 2:
		case 3:
			val5 = Color.black;
			break;
		}
		Skybox.SetMaterialColor(Skybox.skyboxMaterial, (val4 != null) ? Color.white : val5);
		Skybox.SetMaterialTexture(Skybox.skyboxMaterial, val4);
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0001AE80 File Offset: 0x00019080
	private static void SetMaterialColor(Material material, Color color)
	{
		bool flag = !material.HasProperty("_BaseColor");
		if (!flag)
		{
			material.SetColor("_BaseColor", color);
		}
		bool flag2 = !material.HasProperty("_Color");
		if (!flag2)
		{
			material.SetColor("_Color", color);
		}
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0001AEDC File Offset: 0x000190DC
	private static void SetMaterialTexture(Material material, Texture2D texture)
	{
		bool flag = !material.HasProperty("_BaseMap");
		if (!flag)
		{
			material.SetTexture("_BaseMap", texture);
		}
		bool flag2 = !material.HasProperty("_MainTex");
		if (!flag2)
		{
			material.SetTexture("_MainTex", texture);
		}
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0001AF38 File Offset: 0x00019138
	private static Texture2D LoadTexture(string path)
	{
		try
		{
			bool flag = string.IsNullOrEmpty(path) || !File.Exists(path);
			if (flag)
			{
				return null;
			}
			byte[] array = File.ReadAllBytes(path);
			Texture2D val = new Texture2D(2, 2, 4, false);
			val.hideFlags = 61;
			Type typeFromHandle = typeof(ImageConversion);
			MethodInfo method = typeFromHandle.GetMethod("LoadImage", new Type[]
			{
				typeof(Texture2D),
				typeof(byte[]),
				typeof(bool)
			});
			bool flag2 = method != null && (bool)method.Invoke(null, new object[] { val, array, false });
			if (flag2)
			{
				val.wrapMode = 1;
				return val;
			}
			Object.Destroy(val);
		}
		catch (Exception)
		{
		}
		return null;
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0001B034 File Offset: 0x00019234
	public static void Disable()
	{
		bool flag = Skybox.skyboxRenderer != null;
		if (flag)
		{
			bool flag2 = Skybox.built;
			if (flag2)
			{
				Object.Destroy(Skybox.skyboxRenderer);
			}
			else
			{
				bool flag3 = Skybox.imageLoaded;
				if (flag3)
				{
				}
			}
			Skybox.skyboxRenderer = null;
			bool flag4 = Skybox.skyboxFilter != null;
			if (flag4)
			{
				bool flag5 = Skybox.starsBuilt;
				if (flag5)
				{
					Object.Destroy(Skybox.skyboxFilter);
				}
			}
			Skybox.skyboxFilter = null;
			Skybox.built = false;
			Skybox.starsBuilt = false;
			Skybox.imageLoaded = false;
			Skybox.UpdateGradient();
			Skybox.ApplyStarfieldMode();
		}
		else
		{
			bool flag6 = Skybox.skyboxFilter != null;
			if (flag6)
			{
				bool flag7 = Skybox.starsBuilt;
				if (flag7)
				{
					Object.Destroy(Skybox.skyboxFilter);
				}
			}
			Skybox.skyboxFilter = null;
			Skybox.built = false;
			Skybox.starsBuilt = false;
			Skybox.imageLoaded = false;
			Skybox.UpdateGradient();
			Skybox.ApplyStarfieldMode();
		}
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0001B120 File Offset: 0x00019320
	private static void BuildStarfield()
	{
		Quaternion rotation = default(Quaternion);
		Camera val2 = Camera.main;
		bool flag = !(val2 == null);
		if (flag)
		{
			bool flag2 = !(Skybox.starContainer == null);
			if (flag2)
			{
				bool flag3 = Skybox.currentMode == 200;
				if (!flag3)
				{
					Skybox.SpawnStars(200);
				}
			}
			else
			{
				Skybox.SpawnStars(200);
			}
			bool flag4 = Skybox.starParent == null;
			if (!flag4)
			{
				rotation = val2.transform.rotation;
				int num2 = 0;
				for (;;)
				{
					bool flag5 = num2 >= Skybox.starParent.Length;
					if (flag5)
					{
						break;
					}
					Transform val3 = Skybox.starParent[num2];
					bool flag6 = val3 == null;
					if (flag6)
					{
					}
					num2++;
				}
			}
		}
	}

	// Token: 0x0600017E RID: 382 RVA: 0x0001B200 File Offset: 0x00019400
	private static void SpawnStars(int starCount)
	{
		Vector3 val = default(Vector3);
		Skybox.UpdateGradient();
		Skybox.starContainer = new GameObject("Absense_StarField");
		Skybox.starParent = (Transform[])new Transform[starCount];
		Skybox.lastCameraPos = (Vector3[])new Vector3[starCount];
		Skybox.gradientTime = new float[starCount];
		int num2 = 0;
		for (;;)
		{
			bool flag = num2 < starCount;
			if (!flag)
			{
				break;
			}
			float num3 = (float)num2;
			float num4 = Mathf.Acos(1f - 2f * num3 / (float)starCount);
			float num5 = 3.14159274f * (1f + Mathf.Sqrt(5f)) * num3;
			val..ctor(Mathf.Cos(num5) * Mathf.Sin(num4), Mathf.Cos(num4), Mathf.Sin(num5) * Mathf.Sin(num4));
			GameObject val2 = new GameObject("Star_" + num2.ToString());
			val2.transform.SetParent(Skybox.starContainer.transform, false);
			Skybox.starParent[num2] = val2.transform;
			Skybox.lastCameraPos[num2] = val;
			Skybox.gradientTime[num2] = Random.Range(0f, 6.28318548f);
			num2++;
		}
	}

	// Token: 0x0600017F RID: 383 RVA: 0x0001B340 File Offset: 0x00019540
	private static Material CreateGradientMaterial()
	{
		bool flag = !(Skybox.gradientMaterial == null);
		if (!flag)
		{
			Shader val = Shader.Find("Sprites/Default");
			bool flag2 = !(val == null);
			if (!flag2)
			{
				val = Shader.Find("Unlit/Transparent");
			}
			Skybox.gradientMaterial = new Material(val);
		}
		return Skybox.gradientMaterial;
	}

	// Token: 0x06000180 RID: 384 RVA: 0x0001B3A8 File Offset: 0x000195A8
	private static Texture2D GenerateGradientTexture()
	{
		int int_ = -2101770653;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		Vector2 val = default(Vector2);
		float num5 = 0f;
		int num6;
		for (;;)
		{
			num++;
			num6 = ObfConst.Decode(int_) % 9;
			switch (num6 / 4)
			{
			case 0:
				switch (num6 % 4)
				{
				case 0:
				{
					bool flag = Skybox.gradientTexture == null;
					if (flag)
					{
						int num7 = num * (num + 1);
						int_ = -2101770628 ^ (num7 & 1);
					}
					else
					{
						int num8 = num;
						int num9 = num;
						int_ = -2101770629 ^ ((num8 * (num9 + 1)) & 1);
					}
					continue;
				}
				case 1:
				{
					num3 = 32;
					int num10 = num3;
					int num11 = num3;
					Skybox.gradientTexture = new Texture2D(num10, num11, 4, false);
					Skybox.gradientTexture.hideFlags = 61;
					Texture2D val2 = Skybox.gradientTexture;
					val2.wrapMode = 1;
					float num12 = (float)(num3 - 1) * 0.5f;
					int num13 = num3;
					val..ctor(num12, (float)(num13 - 1) * 0.5f);
					num5 = (float)num3 * 0.5f;
					num4 = 0;
					int_ = -2101770651 ^ ((num * (num + 1)) & 1);
					continue;
				}
				case 2:
					num2 = 0;
					int_ = -2101770649 ^ ((num * (num + 1)) & 1);
					continue;
				case 3:
				{
					float num14 = Vector2.Distance(new Vector2((float)num2, (float)num4), val) / num5;
					float num15 = Mathf.Clamp01(1f - num14);
					num15 *= num15;
					Skybox.gradientTexture.SetPixel(num2, num4, new Color(1f, 1f, 1f, num15));
					num2++;
					int_ = -2101770649 ^ ((num * (num + 1)) & 1);
					continue;
				}
				}
				break;
			case 1:
				switch (num6 % 4)
				{
				case 0:
				{
					bool flag2 = num2 >= num3;
					if (flag2)
					{
						int_ = -2101770656 ^ ((num * (num + 1)) & 1);
						continue;
					}
					int num16 = num * (num + 1);
					int_ = -2101770654 ^ (num16 & 1);
					continue;
				}
				case 1:
					num4++;
					int_ = -2101770651 ^ ((num * (num + 1)) & 1);
					continue;
				case 2:
					int_ = ((num4 < num3) ? (-2101770655 ^ ((num * (num + 1)) & 1)) : (-2101770650 ^ ((num * (num + 1)) & 1)));
					continue;
				case 3:
				{
					Skybox.gradientTexture.Apply();
					int num17 = num;
					int num18 = num;
					int_ = -2101770629 ^ ((num17 * (num18 + 1)) & 1);
					continue;
				}
				}
				break;
			case 2:
				goto IL_0285;
			}
			break;
		}
		goto IL_029F;
		IL_0285:
		bool flag3 = num6 % 4 != 0;
		if (!flag3)
		{
			return Skybox.gradientTexture;
		}
		IL_029F:
		throw null;
	}

	// Token: 0x06000181 RID: 385 RVA: 0x0001B664 File Offset: 0x00019864
	private static void UpdateGradient()
	{
		bool flag = !(Skybox.starContainer != null);
		if (!flag)
		{
			Object.Destroy(Skybox.starContainer);
			Skybox.starContainer = null;
		}
	}

	// Token: 0x06000182 RID: 386 RVA: 0x0001B6A0 File Offset: 0x000198A0
	private static void UpdateStarfield()
	{
		Vector3 val2 = default(Vector3);
		Camera val3 = Camera.main;
		bool flag = !(val3 == null);
		if (flag)
		{
			Skybox.UpdateImageMode();
			bool flag2 = !(Skybox.starOfDavid == null);
			if (!flag2)
			{
				Skybox.CreateStarOfDavid();
			}
			Vector3 val4 = val3.transform.position + Vector3.up * 220f;
			Skybox.starOfDavid.transform.position = val4;
			Skybox.twinkleTime += Time.deltaTime * 12f;
			bool flag3 = (val3.transform.position - val4).sqrMagnitude > 0.001f;
			if (flag3)
			{
			}
			bool flag4 = !Settings.JewishMusicEnabled;
			if (flag4)
			{
				MusicPlayer.Stop();
			}
			else
			{
				MusicPlayer.Play();
				MusicPlayer.SetVolume();
				MusicPlayer.PollDownload();
			}
		}
	}

	// Token: 0x06000183 RID: 387 RVA: 0x0001B794 File Offset: 0x00019994
	private static void UpdateImageMode()
	{
		string[] array = Skybox.lastMode;
		int num2 = 0;
		for (;;)
		{
			bool flag = num2 < array.Length;
			if (!flag)
			{
				break;
			}
			GameObject val = GameObject.Find(array[num2]);
			bool flag2 = !(val != null);
			if (!flag2)
			{
				bool flag3 = !val.activeSelf;
				if (!flag3)
				{
					val.SetActive(false);
					bool flag4 = Skybox.stars.Contains(val);
					if (!flag4)
					{
						Skybox.stars.Add(val);
					}
				}
			}
			num2++;
		}
	}

	// Token: 0x06000184 RID: 388 RVA: 0x0001B830 File Offset: 0x00019A30
	private static void ShowStars()
	{
		foreach (GameObject item in Skybox.stars)
		{
			bool flag = item != null;
			if (flag)
			{
				item.SetActive(true);
			}
		}
		Skybox.stars.Clear();
	}

	// Token: 0x06000185 RID: 389 RVA: 0x0001B8A0 File Offset: 0x00019AA0
	private static void CreateStarOfDavid()
	{
		Skybox.starOfDavid = new GameObject("Absense_StarOfDavid");
		GameObject val = Skybox.starOfDavid;
		val.hideFlags = 61;
		Color white = Color.white;
		Skybox.ConfigureStar(Skybox.starOfDavid.transform, 0f, white);
		Skybox.ConfigureStar(Skybox.starOfDavid.transform, 180f, white);
	}

	// Token: 0x06000186 RID: 390 RVA: 0x0001B900 File Offset: 0x00019B00
	private static void ConfigureStar(Transform parent, float size, Color color)
	{
		GameObject val2 = new GameObject("Triangle");
		Transform transform = val2.transform;
		transform.SetParent(parent, false);
		LineRenderer val3 = val2.AddComponent<LineRenderer>();
		int num2 = 0;
		for (;;)
		{
			int num3 = num2;
			bool flag = num3 < 3;
			if (!flag)
			{
				break;
			}
			float num4 = (size + 90f + (float)num2 * 120f) * 0.0174532924f;
			val3.SetPosition(num2, new Vector3(Mathf.Cos(num4), Mathf.Sin(num4), 0f) * 55f);
			num2++;
		}
	}

	// Token: 0x06000187 RID: 391 RVA: 0x0001B99C File Offset: 0x00019B9C
	private static Material CreateStarMaterial()
	{
		bool flag = !(Skybox.imageMaterial == null);
		if (!flag)
		{
			Shader val = Shader.Find("Sprites/Default");
			bool flag2 = !(val == null);
			if (!flag2)
			{
				val = Shader.Find("Hidden/Internal-Colored");
			}
			Skybox.imageMaterial = new Material(val);
		}
		return Skybox.imageMaterial;
	}

	// Token: 0x06000188 RID: 392 RVA: 0x0001BA02 File Offset: 0x00019C02
	private static void ApplyImageMode()
	{
	}

	// Token: 0x06000189 RID: 393 RVA: 0x0001BA08 File Offset: 0x00019C08
	private static void ApplyStarfieldMode()
	{
		Skybox.ShowStars();
		bool flag = Skybox.starOfDavid != null;
		if (flag)
		{
			Object.Destroy(Skybox.starOfDavid);
			Skybox.starOfDavid = null;
			MusicPlayer.Stop();
		}
		else
		{
			MusicPlayer.Stop();
		}
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0001BA50 File Offset: 0x00019C50
	private static string GetModeName()
	{
		string text;
		switch (Settings.SkyboxMode)
		{
		case 0:
			text = "Solid";
			break;
		case 1:
			text = "Image";
			break;
		case 2:
			text = "Absense";
			break;
		case 3:
			text = "Jewish";
			break;
		default:
			text = "?";
			break;
		}
		return text;
	}

	// Token: 0x04000413 RID: 1043
	private const string loadedImagePath = "Environment Objects/LocalObjects_Prefab/Standard Sky";

	// Token: 0x04000414 RID: 1044
	private static GameObject skyboxObject;

	// Token: 0x04000415 RID: 1045
	private static MeshRenderer skyboxRenderer;

	// Token: 0x04000416 RID: 1046
	private static MeshFilter skyboxFilter;

	// Token: 0x04000417 RID: 1047
	private static Material skyboxMaterial;

	// Token: 0x04000418 RID: 1048
	private static Mesh sphereMesh;

	// Token: 0x04000419 RID: 1049
	private static Mesh quadMesh;

	// Token: 0x0400041A RID: 1050
	private static Mesh starMesh;

	// Token: 0x0400041B RID: 1051
	private static bool built;

	// Token: 0x0400041C RID: 1052
	private static bool starsBuilt;

	// Token: 0x0400041D RID: 1053
	private static Material starMaterial;

	// Token: 0x0400041E RID: 1054
	private static bool imageLoaded;

	// Token: 0x0400041F RID: 1055
	private static GameObject starContainer;

	// Token: 0x04000420 RID: 1056
	private static Transform[] starParent;

	// Token: 0x04000421 RID: 1057
	private static Vector3[] lastCameraPos;

	// Token: 0x04000422 RID: 1058
	private static float[] gradientTime;

	// Token: 0x04000423 RID: 1059
	private static Mesh domeMesh;

	// Token: 0x04000424 RID: 1060
	private static Material gradientMaterial;

	// Token: 0x04000425 RID: 1061
	private static Texture2D gradientTexture;

	// Token: 0x04000426 RID: 1062
	private static int currentMode = -1;

	// Token: 0x04000427 RID: 1063
	private const float rotation = 250f;

	// Token: 0x04000428 RID: 1064
	private static GameObject starOfDavid;

	// Token: 0x04000429 RID: 1065
	private static Material imageMaterial;

	// Token: 0x0400042A RID: 1066
	private static float twinkleTime;

	// Token: 0x0400042B RID: 1067
	private static readonly string[] lastMode = new string[] { "Environment Objects/LocalObjects_Prefab/SkyJungleBottom", "Environment Objects/LocalObjects_Prefab/CityToSkyJungle" };

	// Token: 0x0400042C RID: 1068
	private static readonly List<GameObject> stars = new List<GameObject>();

	// Token: 0x0400042D RID: 1069
	public static string Status = "Not applied";

	// Token: 0x0400042E RID: 1070
	private const float spawnTimer = 220f;

	// Token: 0x0400042F RID: 1071
	private const float scale = 55f;
}
