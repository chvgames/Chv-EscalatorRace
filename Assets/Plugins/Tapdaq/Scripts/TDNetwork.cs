using System.Collections.Generic;
using System;
using UnityEngine;

namespace Tapdaq
{
	[Serializable]
	public class TDNetwork
	{
		[SerializeField]
		public readonly string name;
        [SerializeField]
        public TDSDKTargetVersion sdkTargetVersion;
        [SerializeField]
		public readonly string cocoapodsAdapterDependency;
		[SerializeField]
		public readonly string mavenAdapterDependency;
		[SerializeField]
		public readonly string mavenNetworkRepository;
		[SerializeField]
		public bool androidEnabled = false;
		[SerializeField]
		public bool iOSEnabled = false;

        public TDNetwork(string network_name, TDSDKTargetVersion targetSdkVersion, string cocoapods_dependency, string maven_dependency, string maven_network_repository = null)
		{
			this.name = network_name;
            this.sdkTargetVersion = targetSdkVersion;
			this.cocoapodsAdapterDependency = cocoapods_dependency;
			this.mavenAdapterDependency = maven_dependency;
			this.mavenNetworkRepository = maven_network_repository;
		}

        public static bool IsAndroidSupported(TDNetwork network)
		{
			return network.mavenAdapterDependency != null;
		}

		public static bool IsIOSSupported(TDNetwork network)
		{
			return network.cocoapodsAdapterDependency != null;
		}

		public static readonly List<TDNetwork> AllNetworks = new List<TDNetwork>(new TDNetwork[]
		{
			new TDNetwork("AdColony",
				new TDSDKTargetVersion("9.0", "14"),
				"AdColony",
				"com.tapdaq.sdk:TapdaqAdColonyAdapter:",
				null),
			new TDNetwork("AdMob",
				new TDSDKTargetVersion("9.0", "16"),
				"AdMob",
				"com.tapdaq.sdk:TapdaqAdMobAdapter:",
				null),
			new TDNetwork("AppLovin",
				new TDSDKTargetVersion("9.0", "16"),
				"AppLovin",
				"com.tapdaq.sdk:TapdaqAppLovinAdapter:",
				null),
			new TDNetwork("Chartboost",
				new TDSDKTargetVersion("9.0", "21"),
				"Chartboost",
				"com.tapdaq.sdk:TapdaqChartboostAdapter:",
				null),
			new TDNetwork("Facebook AN",
				new TDSDKTargetVersion("9.0", "14"),
				"FAN",
				"com.tapdaq.sdk:TapdaqFANAdapter:",
				null),
			new TDNetwork("InMobi",
				new TDSDKTargetVersion("9.0", "16"),
				"InMobi",
				"com.tapdaq.sdk:TapdaqInmobiAdapter:",
				null),
			new TDNetwork("ironSource",
				new TDSDKTargetVersion("9.0", "16"),
				"IronSource",
				"com.tapdaq.sdk:TapdaqIronsourceAdapter:",
				"https://android-sdk.is.com/"),
			new TDNetwork("Maio",
				new TDSDKTargetVersion("9.0", "16"),
				"Maio",
				"com.tapdaq.sdk:TapdaqMaioAdapter:",
				"https://imobile-maio.github.io/maven"),
			new TDNetwork("Pangle",
				new TDSDKTargetVersion("9.0", "14"),
				"Pangle",
				"com.tapdaq.sdk:TapdaqPangleAdapter:",
				"https://artifact.bytedance.com/repository/pangle"),
			new TDNetwork("Tapjoy",
				new TDSDKTargetVersion("9.0", "14"),
				"Tapjoy",
				"com.tapdaq.sdk:TapdaqTapjoyAdapter:",
				"https://sdk.tapjoy.com"),			
			new TDNetwork("UnityAds",
				new TDSDKTargetVersion("9.0", "19"),
				"UnityAds",
				"com.tapdaq.sdk:TapdaqUnityAdsAdapter:",
				null),
			new TDNetwork("Vungle",
				new TDSDKTargetVersion("10.0", "14"),
				"Vungle",
				"com.tapdaq.sdk:TapdaqVungleAdapter:",
				null)
		});
	}

    [Serializable]
    public class TDSDKTargetVersion
    {
        [SerializeField]
        public readonly string iOS;
        [SerializeField]
        public readonly string android;

        public TDSDKTargetVersion(string iosTarget, string androidTarget)
        {
            iOS = iosTarget;
            android = androidTarget;
        }
    }
}

