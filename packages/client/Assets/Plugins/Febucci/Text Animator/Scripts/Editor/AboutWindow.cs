using System;
using Febucci.UI.Core;
using UnityEditor;
using UnityEngine;

namespace Febucci.UI
{
    public class AboutWindow : EditorWindow
    {
        const string currentVersion = "1.3.4";


        #region Utilties

        const string menuParent = "Tools/Febucci/TextAnimator/";
        const string linksCategory = "Links/";
        const string utilsCategory = "Utils/";

        const string page_docs_name = "📄 Documentation";
        const string page_docs_url = "https://www.febucci.com/text-animator-unity/docs/1.X/";
        const string page_changelog_name = "📝 Patch Notes";
        const string page_changelog_url = "https://www.febucci.com/text-animator-unity/changelog/";
        const string page_support_name = "🆘 Support";
        const string page_support_url = "https://www.febucci.com/text-animator-unity/support/";

        [MenuItem(menuParent + utilsCategory + "Locate Global Data", false, 0)]
        static void LocateGlobalData()
        {
            var foundData = Resources.Load(TAnimGlobalDataScriptable.resourcesPath);
            if (foundData != null)
            {
                Selection.activeObject = foundData;
            }
            else
            {
                Debug.LogWarning(
                    $"Text Animator: No Scriptable data found, please create one in path {TAnimGlobalDataScriptable.resourcesPath}");
            }
        }

        #endregion


        const int windowWidth = 350;
        const int windowHeight = 485;

        [InitializeOnLoadMethod]
        private static void FirstSetup()
        {
            EditorApplication.delayCall += TryOpeningWindow;
        }

        private static void TryOpeningWindow()
        {
            const string key_installedVersion = "Febucci.UI.TextAnimator.Version";

            string installedVersion = PlayerPrefs.GetString(key_installedVersion);

            // Same version already exists
            if (!string.IsNullOrWhiteSpace(installedVersion) && currentVersion == installedVersion)
                return;

            PlayerPrefs.SetString(key_installedVersion, currentVersion);
            OpenWindow();
        }

        GUIContent logo;

        void OnEnable()
        {
            var obj = EditorGUIUtility.Load(
                "Assets/Plugins/Febucci/Text Animator/Scripts/Editor/febucci.tanimator.about_logo.png");
            if (obj != null)
                logo = new GUIContent(obj as Texture2D);
        }


        GUIStyle style_rightAligned;

        public void OnGUI()
        {
            var rect = new Rect(5, 10, windowWidth - 10, windowHeight);
            GUILayout.BeginArea(rect);

            //Logo, if present
            if (logo != null) GUILayout.Label(logo, EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(180));

            GUILayout.Label("Welcome!", EditorStyles.boldLabel);

            GUILayout.Label("Thank you for using Text Animator. Have fun bringing your project's texts to life!",
                EditorStyles.wordWrappedLabel);

            if (style_rightAligned == null)
            {
                style_rightAligned = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
                style_rightAligned.alignment = TextAnchor.MiddleRight;
            }

            GUILayout.Label($"Version: {currentVersion}", style_rightAligned);

            GUILayout.Space(5);
            //--Online Resources--
            GUILayout.Label("Online Resources", EditorStyles.boldLabel);
            GUILayout.Label("Here are some useful resources:",
                EditorStyles.wordWrappedLabel);

            GUILayout.BeginHorizontal();
            //Docs
            if (GUILayout.Button(page_docs_name)) Application.OpenURL(page_docs_url);
            //Support
            if (GUILayout.Button(page_support_name)) Application.OpenURL(page_support_url);
            //Patch notes
            if (GUILayout.Button(page_changelog_name)) Application.OpenURL(page_changelog_url);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            //--Extras--
            GUILayout.Label("Extras", EditorStyles.boldLabel);


            GUILayout.Label("[!!!] IMPORTANT", EditorStyles.largeLabel);
            GUILayout.Label("Text Animator 2.X is OUT!! It's a huge update with many improvements and new features." +
                            "To upgrade please visit the Asset Store, thanks!", EditorStyles.wordWrappedLabel);
            if (GUILayout.Button("-> Get 2.X"))
                Application.OpenURL("https://www.febucci.com/text-animator-unity/");
            
            GUILayout.Label("For any help with the update, don't hesitate to ask for support! We'll be happy to help, thanks!", EditorStyles.wordWrappedLabel);
            
            GUILayout.Space(5);
            GUILayout.Label("Cheers! @febucci", EditorStyles.centeredGreyMiniLabel);
            GUILayout.EndArea();
        }

        [MenuItem("Tools/Febucci/TextAnimator/About", priority = 0)]
        private static void OpenWindow()
        {
            var position = new Rect(100, 100, windowWidth, windowHeight);
            GetWindowWithRect<AboutWindow>(position, true, "About Text Animator", true);
        }
    }
}