using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using MiniGameSDK;
namespace AndroidNativeProxy
{
	public class EmptyActivity
	{
        public static void Apply()
        {
            var empTxt = AssetDatabase.GUIDToAssetPath("1300b39deac701e46a698ccf45636c02");
            string path = "Assets/Plugins/Android";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var java = Path.Combine(path, $"{Path.GetFileNameWithoutExtension(empTxt)}.java");
            if (File.Exists(java))
                File.Delete(java);
            File.Copy(empTxt,java);
            SetXml();
        }
        static void SetXml()
        {
            var doc = XmlHelper.GetAndroidManifest();
            var app = doc.SelectSingleNode("/manifest/application");
            var act = app.FindNode("activity", "android:name", "com.ad.nativeview.EmptyActivity");
            if (act == null)
            {
                var empAct = doc.CreateElement("activity");
                empAct.CreateAttribute("name", "com.ad.nativeview.EmptyActivity");
                empAct.CreateAttribute("configChanges", "orientation|keyboardHidden|screenSize");
                empAct.CreateAttribute("theme", "@android:style/Theme.Translucent.NoTitleBar.Fullscreen");
                app.AppendChild(empAct);
                doc.Save();
            }
           
        }
	}
}
