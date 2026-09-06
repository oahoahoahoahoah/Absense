using System;
using System.Reflection;

// Token: 0x02000043 RID: 67
public static class Weather
{
	// Token: 0x17000013 RID: 19
	// (get) Token: 0x060001ED RID: 493 RVA: 0x00020D80 File Offset: 0x0001EF80
	private static BetterDayNightManager BetterDayNightManager_0
	{
		get
		{
			BetterDayNightManager betterDayNightManager;
			try
			{
				betterDayNightManager = BetterDayNightManager.instance;
			}
			catch
			{
				betterDayNightManager = null;
			}
			return betterDayNightManager;
		}
	}

	// Token: 0x060001EE RID: 494 RVA: 0x00020DB0 File Offset: 0x0001EFB0
	private static MethodInfo FindSetTimeMethod()
	{
		bool flag = Weather.initialized;
		MethodInfo methodInfo;
		if (flag)
		{
			methodInfo = Weather.setTimeMethod;
		}
		else
		{
			Weather.initialized = true;
			try
			{
				Type typeFromHandle = typeof(BetterDayNightManager);
				Weather.setTimeMethod = typeFromHandle.GetMethod("SetTimeOfDay", new Type[]
				{
					typeof(int),
					typeof(bool)
				});
			}
			catch
			{
			}
			methodInfo = Weather.setTimeMethod;
		}
		return methodInfo;
	}

	// Token: 0x060001EF RID: 495 RVA: 0x00020E34 File Offset: 0x0001F034
	public static void CycleTime()
	{
		try
		{
			BetterDayNightManager betterDayNightManager_ = Weather.BetterDayNightManager_0;
			bool flag = betterDayNightManager_ == null;
			if (!flag)
			{
				int num = Settings.CurrentTimeOfDay;
				bool flag2 = num <= 0;
				if (!flag2)
				{
					bool flag3 = num >= Weather.timeValues.Length;
					if (flag3)
					{
						num = Weather.timeValues.Length - 1;
					}
					int num2 = Weather.timeValues[num];
					bool flag4 = num2 >= 0;
					if (flag4)
					{
						MethodInfo methodInfo = Weather.FindSetTimeMethod();
						bool flag5 = methodInfo != null;
						if (flag5)
						{
							methodInfo.Invoke(betterDayNightManager_, new object[] { num2, false });
						}
						else
						{
							betterDayNightManager_.SetTimeOfDay(num2, false);
						}
					}
				}
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x060001F0 RID: 496 RVA: 0x00020F08 File Offset: 0x0001F108
	public static void ApplyTime()
	{
		Settings.CurrentTimeOfDay++;
		bool flag = Settings.CurrentTimeOfDay < Weather.TimeNames.Length;
		if (flag)
		{
		}
		Weather.CycleTime();
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x00020F3E File Offset: 0x0001F13E
	public static void SetTimeIndex(int index)
	{
		Settings.CurrentTimeOfDay = index;
		Weather.CycleTime();
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x00020F50 File Offset: 0x0001F150
	public static void CycleWeather()
	{
		try
		{
			bool flag = !(Weather.BetterDayNightManager_0 == null);
			if (flag)
			{
				switch (Settings.CurrentWeather)
				{
				case 1:
					Weather.SetVisualOnly(true);
					break;
				case 2:
					Weather.SetVisualOnly(false);
					break;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x00020FBC File Offset: 0x0001F1BC
	public static void SetVisualOnly(bool visualOnly)
	{
		BetterDayNightManager betterDayNightManager_ = Weather.BetterDayNightManager_0;
		bool flag = !(betterDayNightManager_ == null) && betterDayNightManager_.weatherCycle != null;
		if (flag)
		{
			int num = (visualOnly ? 1 : 0);
			BetterDayNightManager.WeatherType val = num;
			for (int i = 0; i < betterDayNightManager_.weatherCycle.Length; i++)
			{
				betterDayNightManager_.weatherCycle[i] = val;
			}
		}
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x00021020 File Offset: 0x0001F220
	public static void ApplyWeather()
	{
		Settings.CurrentWeather++;
		bool flag = Settings.CurrentWeather < Weather.WeatherNames.Length;
		if (flag)
		{
		}
		Weather.CycleWeather();
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x00021058 File Offset: 0x0001F258
	public static void Update()
	{
		bool flag = Settings.CurrentTimeOfDay == Weather.currentTime;
		if (!flag)
		{
			Weather.currentTime = Settings.CurrentTimeOfDay;
			Weather.CycleTime();
		}
		bool flag2 = Settings.CurrentWeather == Weather.currentWeather;
		if (!flag2)
		{
			Weather.CycleWeather();
		}
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x000210AC File Offset: 0x0001F2AC
	public static string GetTimeLabel()
	{
		int currentTimeOfDay = Settings.CurrentTimeOfDay;
		bool flag = currentTimeOfDay < 0;
		string text;
		if (flag)
		{
			text = "Time: Unknown";
		}
		else
		{
			bool flag2 = Settings.CurrentTimeOfDay >= Weather.TimeNames.Length;
			if (flag2)
			{
				text = "Time: Unknown";
			}
			else
			{
				text = "Time: " + Weather.TimeNames[Settings.CurrentTimeOfDay];
			}
		}
		return text;
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x0002110C File Offset: 0x0001F30C
	public static string GetWeatherLabel()
	{
		bool flag = Settings.CurrentWeather < 0;
		string text;
		if (flag)
		{
			text = "Weather: Unknown";
		}
		else
		{
			bool flag2 = Settings.CurrentWeather >= Weather.WeatherNames.Length;
			if (flag2)
			{
				text = "Weather: Unknown";
			}
			else
			{
				text = "Weather: " + Weather.WeatherNames[Settings.CurrentWeather];
			}
		}
		return text;
	}

	// Token: 0x060001F8 RID: 504 RVA: 0x00021168 File Offset: 0x0001F368
	static Weather()
	{
		int[] array = new int[] { -1, 1, 3, 7, 0 };
		Weather.timeValues = array;
		Weather.WeatherNames = new string[] { "None", "Rain", "Clear" };
		Weather.currentTime = -1;
		Weather.currentWeather = -1;
	}

	// Token: 0x040004A3 RID: 1187
	public static readonly string[] TimeNames = new string[] { "None", "Morning", "Day", "Evening", "Night" };

	// Token: 0x040004A4 RID: 1188
	private static readonly int[] timeValues;

	// Token: 0x040004A5 RID: 1189
	public static readonly string[] WeatherNames;

	// Token: 0x040004A6 RID: 1190
	private static MethodInfo setTimeMethod;

	// Token: 0x040004A7 RID: 1191
	private static bool initialized;

	// Token: 0x040004A8 RID: 1192
	private static int currentTime;

	// Token: 0x040004A9 RID: 1193
	private static int currentWeather;
}
