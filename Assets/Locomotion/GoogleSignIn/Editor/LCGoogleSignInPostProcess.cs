using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
//using UnityEditor.iOS.Xcode;
using System.IO;

public static class LCGoogleSignInPostProcess {

	/*#warning Google Login: Provide iOS's configuration plist file name & android's configuration json file. Template sample follows..
	// It could also be a path relative to Assets folder e.g. "foldername/<file name>.plist"
	const string iOSFileName = "client_<YourFileData>.apps.googleusercontent.com.plist";
	const string androidFileName = "client_secret_<YourFileData>.apps.googleusercontent.com.json";
	
	//If you have a different path for plist file, you can modify the path here and check usage in this class
#if UNITY_IOS
	static string googleInfoPlistFilePath = iOSFileName;
#endif*/

	[PostProcessBuild]
	public static void LCPostProcessGoogleLogin(BuildTarget buildTarget, string pathToBuiltProject) {
		AddGoogleServicesFile (buildTarget, pathToBuiltProject);
		AddDataToPlist (buildTarget, pathToBuiltProject);
	}

	public static void AddDataToPlist(BuildTarget buildTarget, string pathToBuiltProject) {

		if (buildTarget == BuildTarget.iOS) {
			#if UNITY_IOS
			//Build Data From Google Services File
			PlistDocument googlePlist = new PlistDocument ();
			googlePlist.ReadFromString(File.ReadAllText(pathToBuiltProject + "/" + iOSFileName));
			string fireAppID = googlePlist.root ["REVERSED_CLIENT_ID"].AsString();

			// Get plist
			string plistPath = pathToBuiltProject + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromString(File.ReadAllText(plistPath));

			// Get root
			PlistElementDict rootDict = plist.root;


			//1. Build Data
			PlistElementArray bundleUrlSchemaArr1 = new PlistElementArray ();
			bundleUrlSchemaArr1.AddString (fireAppID);

			PlistElementArray bundleUrlSchemaArr2 = new PlistElementArray ();
			bundleUrlSchemaArr2.AddString (Application.identifier);

			var bundleUrlRootKey = "CFBundleURLTypes";
			PlistElement bundleUrlData = rootDict [bundleUrlRootKey];
			PlistElementArray bundleUrlAsArray = null;
			if (bundleUrlData == null) {
				bundleUrlAsArray = new PlistElementArray ();
			} else {
				bundleUrlAsArray = bundleUrlData.AsArray ();
			}

			//2..
			PlistElementDict bundleUrlItem1 = new PlistElementDict ();
			bundleUrlItem1 ["CFBundleTypeRole"] = new PlistElementString("Editor");
			bundleUrlItem1 ["CFBundleURLName"] = new PlistElementString("google");
			bundleUrlItem1 ["CFBundleURLSchemes"] = bundleUrlSchemaArr1;

			bundleUrlAsArray.values.Add (bundleUrlItem1);

			//3.. (A hack for login crash issue)
			PlistElementDict bundleUrlItem2 = new PlistElementDict ();
			bundleUrlItem2 ["CFBundleTypeRole"] = new PlistElementString("Editor");
			bundleUrlItem2 ["CFBundleURLName"] = new PlistElementString("lcgoogle");
			bundleUrlItem2 ["CFBundleURLSchemes"] = bundleUrlSchemaArr2;

			bundleUrlAsArray.values.Add (bundleUrlItem2);

			//3..
			rootDict [bundleUrlRootKey] = bundleUrlAsArray;

			// // ~~ Add GoogleSignIn Data // //


			// Write to file
			File.WriteAllText(plistPath, plist.WriteToString());

			Debug.Log("LCGoogleSignIn: iOS: AddDataToPlist for callbacks: Success");
			#endif
		}
	}

	public static void AddGoogleServicesFile(BuildTarget buildTarget, string path) {

		if (buildTarget == BuildTarget.iOS) {
			#if UNITY_IOS
			string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

			PBXProject proj = new PBXProject ();

			proj.ReadFromString (File.ReadAllText (projPath));


			string target = proj.TargetGuidByName ("Unity-iPhone");

			//Required Frameworks
			proj.AddFrameworkToProject (target, "SystemConfiguration.framework", false);

			try {
				//FileUtil.DeleteFileOrDirectory(path + "/" + googleFileName);
				FileUtil.ReplaceFile ("Assets/" + googleInfoPlistFilePath, path + "/" + iOSFileName);
				proj.AddFileToBuild (target, proj.AddFile (iOSFileName, iOSFileName, PBXSourceTree.Source));
				File.WriteAllText (projPath, proj.WriteToString ());
			}
			catch {
				Debug.LogError("LCGoogleSignIn Failed to copy latest google services info plist file to project directory. Verify manually that google login has no error");
			}

			#endif
		}
	}
}
