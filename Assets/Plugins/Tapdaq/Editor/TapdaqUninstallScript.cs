using System.Collections.Generic;
using System;
using UnityEditor;
using System.IO;

namespace Tapdaq {
	public class TapdaqUninstallScript {

		public static string BASE_PLUGIN_PATH = "Assets/Plugins/";

		[MenuItem ("Tapdaq/Uninstall", false, 11111)]
		public static void UninstallTapdaq () {
			var uninstallResult = EditorUtility.DisplayDialogComplex ("Uninstall Tapdaq Plugin", 
				                      "Are you sure you want to remove all Tapdaq files from your project? You can choose an option to keep your local settings in the Project for further use.", 
				"Cancel","Uninstall", "Uninstall But Keep Settings");
			if (uninstallResult >0) Uninstall (uninstallResult == 2);
		}

	private static void Uninstall(bool keepSettings) {
			Delete ("Editor/iOS/Xcode");
			DeleteIfEmpty ("Editor/iOS");
			DeleteIfEmpty ("Editor");
			Delete ("iOS/Tapdaq");
			DeleteIfEmpty ("iOS");
			DeleteIfEmpty ("Android", "AndroidManifest.xml");

			if (keepSettings)
				DeleteButKeep ("Tapdaq", new string[] { "Plugins/Tapdaq","Plugins/Tapdaq/Resources","Plugins/Tapdaq/Resources/Tapdaq","TapdaqSettings.asset" });
			else
				Delete ("Tapdaq");

			AssetDatabase.Refresh (ImportAssetOptions.ImportRecursive);
		}

		private static void Delete(string path) {
			path = BASE_PLUGIN_PATH + path;
			if (File.Exists(path) || Directory.Exists(path))
			{
				FileUtil.DeleteFileOrDirectory(path);
				path += ".meta";
				FileUtil.DeleteFileOrDirectory(path);
			}
			
		}

		private static void DeleteButKeep(string path, string[] keepFiles) {
			path = BASE_PLUGIN_PATH + path;
			if (!Directory.Exists (path)) return;

			List<string> list = new List<string>();
			list.AddRange(Directory.GetFiles(path, "*" ,SearchOption.AllDirectories));
			list.AddRange(Directory.GetDirectories(path, "*" ,SearchOption.AllDirectories));
			var allFiles =list.ToArray();

			//TODO rework for comparator
			Array.Sort(allFiles, (x, y) => x.Length.CompareTo(y.Length));
			Array.Reverse (allFiles);

			foreach (var file in allFiles) {
				bool needToDelete = true;
				foreach (var keepFile in keepFiles) {
                    if (file.EndsWith (keepFile, StringComparison.CurrentCultureIgnoreCase) || file.EndsWith (keepFile + ".meta", StringComparison.CurrentCultureIgnoreCase)) {
						needToDelete = false;
						break;
					}
				}
			
				if (!needToDelete) continue;

				if (Directory.Exists (file)) {
					DeleteIfEmpty (file.Replace(BASE_PLUGIN_PATH,""));
				} else {
					FileUtil.DeleteFileOrDirectory (file);
				}
			}
		}

		private static void DeleteIfEmpty(string path, params string[] ignoreFiles) {
			path = BASE_PLUGIN_PATH + path;
			if (!Directory.Exists (path)) return;
			var allFiles = Directory.GetFiles(path);
			var allDirectories = Directory.GetDirectories (path);

			var fileNames = new HashSet<string> ();

			foreach (var file in allFiles) {
                if (!file.EndsWith (".meta", StringComparison.CurrentCultureIgnoreCase)) {
					var name = Path.GetFileName (file);
					fileNames.Add (name);
				}
			}

			foreach (var directory in allDirectories) {
				var name = Path.GetDirectoryName (directory);
				fileNames.Add (name);
			}

			foreach (var fileName in ignoreFiles) {
				fileNames.Remove (fileName);
			}

			if (fileNames.Count < 1) FileUtil.DeleteFileOrDirectory (path);
		}
	}
}
