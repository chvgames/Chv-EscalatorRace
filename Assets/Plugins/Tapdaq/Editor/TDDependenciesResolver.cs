// <copyright file="GPGSDependencies.cs" company="Google Inc.">
// Copyright (C) 2015 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using UnityEditor;
using Tapdaq;
using UnityEngine;
using System.Xml.Serialization;
using TDXMLSchema;
using System.IO;

[InitializeOnLoad]
public class TDDependencies : AssetPostprocessor
{
	private static string TAPDAQ_ANDROID_VERSION = "7.10.0";
	private static string TAPDAQ_IOS_VERSION = "7.10.0";

    private static string DEPDENCIES_DIRECTORY = "/Plugins/Tapdaq/Editor/TapdaqDependencies.xml";
	private static string TAPDAQ_REPOSITORY = "https://android-sdk.tapdaq.com";


    public static string minTargetSDKVersion = "10.0";
	public static string cocoapods_respository = "https://github.com/tapdaq/cocoapods-specs.git";


    public static Dependencies dependencies = new Dependencies();

	static TDDependencies()
	{
		RegisterDependencies();
	}

	public static void RegisterDependencies()
	{
        dependencies = new Dependencies();

        RegisterAndroidDependencies();
		RegisterIOSDependencies();
        ResolveAdapters();
        SaveDependencies();
	}

    private static void SaveDependencies()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Dependencies));
        string path = Application.dataPath;
        StreamWriter writer = new StreamWriter(path + DEPDENCIES_DIRECTORY);
        serializer.Serialize(writer.BaseStream, dependencies);
        writer.Close();
    }

    public static void RegisterAndroidDependencies()
	{
        List<String> repositories = new List<string>();
        repositories.Add(TAPDAQ_REPOSITORY);
        // Fetch Tapdaq SDK
        FetchAndroid("com.tapdaq.sdk:Tapdaq-BaseSDK:" + TAPDAQ_ANDROID_VERSION, repositories);
        FetchAndroid("com.tapdaq.sdk:Tapdaq-UnityBridge:" + TAPDAQ_ANDROID_VERSION, repositories);  
	}

	public static void FetchAndroid(string spec, List<String> repositories)
	{
		Debug.Log("FetchAndroid: " + spec);
		// Cretae an AndroidPackage object and add to Dependenies class
		Repositories r = new Repositories();
		r.Repository = repositories;
		AndroidPackage p = new AndroidPackage();
		p.Spec = spec;
		p.Repositories = r;

		dependencies.AndroidPackages.AndroidPackage.Add(p);
	}


	public static void ResolveAdapters()
	{
        // Check status of each network and resolve if enabled
        foreach (TDNetwork network in TDSettings.getInstance().networks)
        {
            Debug.Log("Adapter: " + network.name + "- Android: " + network.mavenAdapterDependency + " Enabled: " + network.androidEnabled + " - iOS: " + network.iOSEnabled);

            if (network.iOSEnabled)
            {
                FetchiOSPod("Tapdaq/" + network.cocoapodsAdapterDependency, TAPDAQ_IOS_VERSION, network.sdkTargetVersion.iOS);
            }

            if (network.androidEnabled)
            {
                List<String> repositories = new List<string>();
                repositories.Add(TAPDAQ_REPOSITORY);

                if (network.mavenNetworkRepository != null)
                {
                    repositories.Add(network.mavenNetworkRepository);
                }
                FetchAndroid(network.mavenAdapterDependency + TAPDAQ_ANDROID_VERSION, repositories);
            }
        }
    }

	public static void RegisterIOSDependencies()
	{
		// ADD TAPDAQ POD  
		Sources s = new Sources();
		s.Source = cocoapods_respository;

		dependencies.IosPods.Sources = s;

		FetchiOSPod("Tapdaq", TAPDAQ_IOS_VERSION, minTargetSDKVersion);
	}

	public static void FetchiOSPod(string podName, string version, string minTargetSDK)
	{
		// Cretae an iosPod object and add to Dependenies class
		IosPod p = new IosPod();
		p.Name = podName;
		p.Version = version;
		p.MinTargetSdk = minTargetSDK;

		dependencies.IosPods.IosPod.Add(p);
	}

    // Handle delayed loading of the dependency resolvers.
    private static void OnPostprocessAllAssets(
		string[] importedAssets, string[] deletedAssets,
		string[] movedAssets, string[] movedFromPath)
	{
		foreach (string asset in importedAssets)
		{
			if (asset.Contains("IOSResolver") ||
				asset.Contains("JarResolver"))
			{
				RegisterDependencies();
				break;
			}
		}
	}
}

// Classes necessary to allow us to serialize XML
namespace TDXMLSchema
{
	[XmlRoot(ElementName = "repositories")]
	public class Repositories
	{
		[XmlElement(ElementName = "repository")]
		public List<string> Repository { get; set; }
	}

	[XmlRoot(ElementName = "androidPackage")]
	public class AndroidPackage
	{
		[XmlElement(ElementName = "repositories")]
		public Repositories Repositories { get; set; }
		[XmlAttribute(AttributeName = "spec")]
		public string Spec { get; set; }
	}

	[XmlRoot(ElementName = "androidPackages")]
	public class AndroidPackages
	{
		[XmlElement(ElementName = "androidPackage")]
		public List<AndroidPackage> AndroidPackage = new List<AndroidPackage>();
	}

	[XmlRoot(ElementName = "sources")]
	public class Sources
	{
		[XmlElement(ElementName = "source")]
		public string Source { get; set; }
	}

	[XmlRoot(ElementName = "iosPod")]
	public class IosPod
	{
		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }
		[XmlAttribute(AttributeName = "version")]
		public string Version { get; set; }
		[XmlAttribute(AttributeName = "minTargetSdk")]
		public string MinTargetSdk { get; set; }
	}

	[XmlRoot(ElementName = "iosPods")]
	public class IosPods
	{
		[XmlElement(ElementName = "sources")]
		public Sources Sources = new Sources();
		[XmlElement(ElementName = "iosPod")]
		public List<IosPod> IosPod = new List<IosPod>();
	}

	[XmlRoot(ElementName = "dependencies")]
	public class Dependencies
	{
		[XmlElement(ElementName = "androidPackages")]
		public AndroidPackages AndroidPackages = new AndroidPackages();
		[XmlElement(ElementName = "iosPods")]
		public IosPods IosPods = new IosPods();
	}
}