using System.IO;
using UnityEditor;
using UnityEngine;

namespace _Main.Editor
{
    public class FolderCreator : EditorWindow
    {
        private string _parentPath = "Assets";
        private int _folderCount = 1;
        private long _startID = 0;

        [MenuItem("Tools/Folder Creator")]
        public static void ShowWindow()
        {
            // 顯示視窗
            GetWindow<FolderCreator>("資料夾生成器");
        }

        private void OnGUI()
        {
            GUILayout.Label("設定參數", EditorStyles.boldLabel);

            // 選擇路徑
            EditorGUILayout.BeginHorizontal();
            _parentPath = EditorGUILayout.TextField("目標路徑", _parentPath);
            if (GUILayout.Button("選擇路徑", GUILayout.Width(80)))
            {
                string path = EditorUtility.OpenFolderPanel("選擇目標資料夾", "Assets", "");
                if (!string.IsNullOrEmpty(path))
                {
                    // 將絕對路徑轉換為 Unity 的相對路徑
                    if (path.StartsWith(Application.dataPath))
                    {
                        _parentPath = "Assets" + path.Substring(Application.dataPath.Length);
                    }
                    else
                    {
                        Debug.LogWarning("請選擇專案內的 Assets 資料夾路徑！");
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            // 輸入數量與 ID
            _folderCount = EditorGUILayout.IntField("創建數量", _folderCount);
            _startID = EditorGUILayout.LongField("基礎 ID (起始值)", _startID);

            // 限制數量最小值
            if (_folderCount < 1) _folderCount = 1;

            EditorGUILayout.Space(10);

            if (GUILayout.Button("開始創建資料夾", GUILayout.Height(30)))
            {
                CreateFolders();
            }
        }

        private void CreateFolders()
        {
            // 檢查路徑是否存在
            if (!AssetDatabase.IsValidFolder(_parentPath))
            {
                EditorUtility.DisplayDialog("錯誤", "目標路徑無效，請確認路徑是否存在。", "確定");
                return;
            }

            int successCount = 0;

            // 開始執行批次處理
            AssetDatabase.StartAssetEditing();

            try
            {
                for (int i = 1; i <= _folderCount; i++)
                {
                    long currentID = _startID + i;
                    string folderName = currentID.ToString();
                    string fullPath = Path.Combine(_parentPath, folderName);

                    if (!AssetDatabase.IsValidFolder(fullPath))
                    {
                        AssetDatabase.CreateFolder(_parentPath, folderName);
                        successCount++;
                    }
                    else
                    {
                        Debug.LogWarning($"資料夾已存在，跳過: {folderName}");
                    }
                }
            }
            finally
            {
                // 結束處理並刷新資料庫
                AssetDatabase.StopAssetEditing();
                AssetDatabase.Refresh();
            }

            EditorUtility.DisplayDialog("完成", $"已成功創建 {successCount} 個資料夾！", "確定");
        }
    }
}