using UnityEditor;
using UnityEngine;

namespace QuickSearch
{

    public class SearchEditor
    {
        private static string _SearchResult = string.Empty;

        // Seach cancel button 
        private const float _SEACH_CANCEL_WIDTH = 22, _SEACH_CANCEL_BUTTON_HEIGHT = 22;

        /// <summary>
        /// Return search result
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static string DrawSearch(Vector2 position)
        {
            GUILayout.Space(3);

            Rect _Rect = new Rect(position.x, position.y, Screen.width - 10, _SEACH_CANCEL_BUTTON_HEIGHT);
            // Y center
            var _YCenter = (_Rect.center.y / 16);

            // Button rect
            Rect _XButtonRect = new Rect(_Rect.x + _Rect.width - _SEACH_CANCEL_WIDTH / 2, _Rect.y + _YCenter, _SEACH_CANCEL_WIDTH, _SEACH_CANCEL_BUTTON_HEIGHT);

            // Click on seach cancel button ?
            if (Event.current.type == EventType.MouseUp && _XButtonRect.Contains(Event.current.mousePosition))
                ResetSearchResult();

            // Darw text field
            try
            {
                _SearchResult = GUI.TextField(new Rect(_Rect.x + 3, _Rect.y + _YCenter, _Rect.width, _Rect.height - 5), _SearchResult, EditorStyles.toolbarSearchField);
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }

            // Draw x button
            GUI.Button(_XButtonRect, "", !string.IsNullOrEmpty(_SearchResult) ? ToolbarSearchCancelButtonStyle : GUIStyle.none);

            // Return search result
            return _SearchResult;

        }
        /// <summary>
        /// Reset search result
        /// </summary>
        public static void ResetSearchResult() => _SearchResult = string.Empty;

        // Get toolbar search cancel button style 
        public static GUIStyle ToolbarSearchCancelButtonStyle
        {
            get
            {
                // Todo: Unity change this.. The old "ToolbarSeachCancelButton" , The New "ToolbarSearchCancelButton"
                return GUI.skin.FindStyle("ToolbarSeachCancelButton") != null ? GUI.skin.FindStyle("ToolbarSeachCancelButton") : GUI.skin.FindStyle("ToolbarSearchCancelButton");
            }
        }
    }
}
