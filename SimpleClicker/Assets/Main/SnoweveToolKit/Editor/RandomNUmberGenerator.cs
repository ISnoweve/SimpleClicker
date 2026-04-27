using System.Text;
using UnityEditor;
using UnityEngine;

namespace _Main.Editor
{
    public class RandomNumberGenerator : EditorWindow
    {
        private int _min = 0;
        private int _max = 100;
        private int _count = 1; // 預設產生 1 個
        private string _resultText = "";
        private Vector2 _scrollPos;

        [MenuItem("Tools/隨機數字產生器")]
        public static void ShowWindow()
        {
            GetWindow<RandomNumberGenerator>("隨機產生器");
        }

        private void OnGUI()
        {
            GUILayout.Label("設定範圍與數量", EditorStyles.boldLabel);

            // --- 第一層：兩個輸入框（最小、最大） ---
            EditorGUILayout.BeginHorizontal();
        
            EditorGUIUtility.labelWidth = 40; // 縮短標籤寬度讓排版更緊湊
            _min = EditorGUILayout.IntField("最小", _min);
            _max = EditorGUILayout.IntField("最大", _max);
        
            EditorGUILayout.EndHorizontal();

            // --- 第二層：產生數量與按鈕 ---
            EditorGUILayout.Space(5);
            _count = EditorGUILayout.IntField("產生數量", _count);
            if (_count < 1) _count = 1;

            if (GUILayout.Button("開始隨機產生", GUILayout.Height(30)))
            {
                GenerateRandomNumbers();
            }

            EditorGUILayout.Space(10);
            GUILayout.Label("結果顯示區", EditorStyles.boldLabel);

            // --- 第三層：大的顯示區 (使用 ScrollView 避免數字太多超出視窗) ---
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.ExpandHeight(true));
        
            // 使用 SelectableLabel 或 TextArea 讓使用者可以複製數字
            // GUILayout.ExpandHeight(true) 會讓這個區塊佔據剩下的所有版面
            _resultText = EditorGUILayout.TextArea(_resultText, GUILayout.ExpandHeight(true));
        
            EditorGUILayout.EndScrollView();

            // 清除按鈕
            if (GUILayout.Button("清除結果"))
            {
                _resultText = "";
                GUI.FocusControl(null); // 取消輸入框焦點
            }
        }

        private void GenerateRandomNumbers()
        {
            if (_min > _max)
            {
                EditorUtility.DisplayDialog("錯誤", "最小值不能大於最大值！", "了解");
                return;
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _count; i++)
            {
                int rnd = Random.Range(_min, _max + 1); // 包含最大值
                sb.AppendLine(rnd.ToString());
            }

            _resultText = sb.ToString();
        }
    }
}