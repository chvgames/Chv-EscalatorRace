using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections.Generic;
using Tapdaq;


using UnityEditor.iOS.Xcode;


public class TapdaqBuildPostprocessor : MonoBehaviour{
	private const string BuildPathKey = "IOSBuildProjectPath";
	[MenuItem ("Tapdaq/Run iOS Build Postprocess", false, 2222)]
	private static void RunIOSPostProcessManually()	{
		var path = EditorPrefs.GetString (BuildPathKey, null);
		OnPostprocessBuild (BuildTarget.iOS, path);
	}

	[MenuItem ("Tapdaq/Run iOS Build Postprocess", true)]
	static bool validateRunPostBuilder(){
		Debug.Log("validateRunPost");
		var path = EditorPrefs.GetString (BuildPathKey, null);
		if( path == null || !Directory.Exists( path ) )
			return false;

		var projectFile = Path.Combine( path, "Unity-iPhone.xcodeproj/project.pbxproj" );
		if( !File.Exists( projectFile ) )
			return false;

		return true;
	}

	[PostProcessBuild(101)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject) {
		if (buildTarget != BuildTarget.iOS) return;
        TDDebugLogger.Log("OnPostprocessBuild - Path: " + pathToBuiltProject);
        
		EditorPrefs.SetString (BuildPathKey, pathToBuiltProject);

        var path = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        if (!File.Exists(path))
        {
            TDDebugLogger.LogError(string.Format("pbxproj '{0}' does not exists", path));
            return;
        }

        var proj = new PBXProject();
        proj.ReadFromString(File.ReadAllText(path));

        var unityIPhoneTargetGuid = GetUnityIPhoneTargetGuid(path);

        SetPListProperties(pathToBuiltProject);

        proj.UpdateBuildProperty(unityIPhoneTargetGuid, "OTHER_LDFLAGS", new string [] { "-ObjC" }, new string [] {});

        File.WriteAllText(path, proj.WriteToString());
    }

    private static string GetUnityIPhoneTargetGuid(string path) {
        var proj = new UnityEditor.iOS.Xcode.PBXProject();
        proj.ReadFromString(File.ReadAllText(path));
        string mainTargetGuid = null;
                
        var unityMainTargetGuidMethod = proj.GetType().GetMethod("GetUnityMainTargetGuid");                       
        if (unityMainTargetGuidMethod != null) {
            mainTargetGuid = (string)unityMainTargetGuidMethod.Invoke(proj, null);
        } else {
            mainTargetGuid = proj.TargetGuidByName ("Unity-iPhone");
        }
        return mainTargetGuid;
    }

	private static void SetPListProperties(string pathToBuiltProject) {
		
		var plistPath = pathToBuiltProject + "/Info.plist";
		var plist = new PlistDocument();

		plist.ReadFromString(File.ReadAllText(plistPath));
		var rootDict = plist.root;

        foreach (TDNetwork network in TDNetwork.AllNetworks) {
            if (network.name == "AdColony" && network.iOSEnabled) {
                rootDict.SetString("NSMotionUsageDescription", "Some ad content may require access to accelerometer for interactive ad experience.");
                rootDict.SetString("NSPhotoLibraryUsageDescription", "Some ad content may require access to the photo library.");
                rootDict.SetString("NSCalendarsUsageDescription", "Some ad content may create a calendar event.");
                rootDict.SetString("NSCameraUsageDescription", "Some ad content may access camera to take picture.");
            }
        }   

		var transportSecurityKey = "NSAppTransportSecurity";

		if (rootDict [transportSecurityKey] == null) {
			rootDict.CreateDict (transportSecurityKey);
		}

		var appTransportSecurity = rootDict [transportSecurityKey].AsDict ();
		
		appTransportSecurity.SetBoolean ("NSAllowsArbitraryLoads", true);

		if(TDSettings.getInstance().admob_appid_ios.Length > 0) {
            rootDict.SetString("GADApplicationIdentifier", TDSettings.getInstance().admob_appid_ios);
		}

        //SKAdNetworkIds
        List<TDKeyValuePair> skAdNetworkIds = TDSettings.getInstance().skAdNetworkIds;
        if (skAdNetworkIds.Count > 0)
        {
            PlistElementArray skAdNetworkItemsArray = rootDict.CreateArray("SKAdNetworkItems");
            foreach (TDKeyValuePair pair in skAdNetworkIds)
            {
                PlistElementDict itemDict = skAdNetworkItemsArray.AddDict();
                itemDict.SetString("SKAdNetworkIdentifier", pair.getValue());
            }
        }

		// Write to file
		File.WriteAllText(plistPath, plist.WriteToString());
	}
}
