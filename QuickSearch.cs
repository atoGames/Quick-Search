using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace QuickSearch
{
    public class QuickSearch : EditorWindow
    {
        // Window settings max size
        private static Vector2 _WindowSize = new Vector2(800, 70);
        // Border
        private const float _BORDER = 3;
        // Width & height
        private const int _WIDTH = 22, _HEIGHT = 22;
        // Scroll view position
        private Vector2 _ScrollViewPosition;
        // Get window
        private static EditorWindow _GetWindow => GetWindow(typeof(QuickSearch));
        // Find all scripts
        private string[] _Files;
        // Program names
        private Dictionary<string, string> _Apps = new()
        {
            {"VS Code" , "code"},
            {"Visual studio" , "devenv"},
            {"Rider" , "rider64"}
        };
        // Program index
        private int _ProgramIndex = 0;
        // How much we show before "Show more"
        private int _ContentCount = 10;

        [MenuItem("Tools/QuickSearch  %/")]
        private static void ShowWindow()
        {
            // Size
            _GetWindow.minSize = _GetWindow.maxSize = _WindowSize;
            // maximized
            _GetWindow.maximized = false;
            // Title
            _GetWindow.titleContent = EditorGUIUtility.TrTextContentWithIcon("Quick Search", "d_Search Icon");
        }
        private void Awake()
        {
            // Get files
            _Files = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);

            // Load save 
            _ProgramIndex = EditorPrefs.GetInt("_ProgramIndex", 0);
        }

        private void OnDisable()
        {
            // Reset search result
            SearchEditor.ResetSearchResult();
            // Save
            EditorPrefs.SetInt("_ProgramIndex", _ProgramIndex);
        }
        private void OnGUI()
        {
            var _SearchPosition = new Vector2(_BORDER, _BORDER);
            // Draw search
            var _SearchResult = SearchEditor.DrawSearch(_SearchPosition);

            // Begin area
            GUILayout.BeginArea(new Rect(_SearchPosition.x, _SearchPosition.y + _WIDTH, Screen.width - 10, _WindowSize.y - _HEIGHT));

            // Filtered results
            var _FilteredResults = new List<string>();
            foreach (var result in _Files)
            {
                var _Name = result.ToLower();
                if (_Name.Contains(_SearchResult.ToLower()))
                    _FilteredResults.Add(result);
            }

            // BeginScrollView
            using (var svs = new GUILayout.ScrollViewScope(_ScrollViewPosition, false, _FilteredResults.Count != 0))
            {
                // Update scroll view position 
                _ScrollViewPosition = svs.scrollPosition;
                // Update window size
                UpdateWindowSize(_FilteredResults.Count);

                for (int i = 0; i < _FilteredResults.Count; i++)
                {
                    if (i < _ContentCount)
                    {
                        var _Script = _FilteredResults[i];

                        // Draw content 
                        using (var info = new GUILayout.HorizontalScope(GUILayout.Height(_HEIGHT)))
                        {
                            var _Path = _Script.Replace(Application.dataPath, "Assets");
                            // Label
                            GUILayout.Label(_Path, GUILayout.Width(Screen.width - (_WIDTH + 128)));
                            // Select script in project
                            if (GUILayout.Button("Select"))
                            {
                                var _Select = AssetDatabase.LoadAssetAtPath(_Path, typeof(Object));
                                Selection.activeObject = _Select;
                            }
                            // Open 
                            if (GUILayout.Button("Open"))
                                ProcessHelper.OpenApp(_Path, _Apps.ElementAt(_ProgramIndex).Value);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Show more", GUILayout.Height(_HEIGHT)))
                        {
                            _ContentCount *= 2;
                        }
                        GUILayout.Space(_BORDER);
                        break;

                    }
                }
                // No result found
                if (_FilteredResults.Count == 0)
                    GUILayout.Label("No result found");
            }
            GUILayout.EndArea();
            // Close
            if (Event.current.keyCode == KeyCode.Escape && _GetWindow.hasFocus)
                _GetWindow.Close();
        }

        private void UpdateWindowSize(int length)
        {
            var _Size = 70;

            if (length > 0)
                _Size = Mathf.Clamp(_HEIGHT * length, 70, 256);

            // Apply
            if (_Size != _WindowSize.y)
            {
                _WindowSize = new Vector2(_WindowSize.x, _Size);
                _GetWindow.minSize = _GetWindow.maxSize = _WindowSize;
                _GetWindow.Repaint();
            }
        }

        private void ShowButton(Rect rect)
        {
            var _Width = 180;
            var _Height = rect.height + 5;

            var _AreaRect = new Rect(Screen.width - 280, rect.y, Screen.width - _Width, _Height);
            GUILayout.BeginArea(_AreaRect);

            if (Screen.width >= 512)
            {
                using (var info = new GUILayout.HorizontalScope())
                {
                    GUILayout.Label(new GUIContent("App: ", EditorGUIUtility.IconContent("EditorSettings Icon").image), GUILayout.Width(45), GUILayout.Height(_Height - 5));
                    _ProgramIndex = EditorGUILayout.Popup(_ProgramIndex, _Apps.Keys.ToArray(), GUILayout.Width(128), GUILayout.Height(_Height - 5));
                    GUILayout.Label(new GUIContent($"Total: ({_Files.Length})", EditorGUIUtility.IconContent("d_cs Script Icon").image), GUILayout.Width(100), GUILayout.Height(_Height - 5));
                }
            }
            GUILayout.EndArea();
        }
    }
}