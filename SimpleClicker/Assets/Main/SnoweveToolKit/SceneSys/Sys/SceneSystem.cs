using System.Collections.Generic;
using System.Threading.Tasks;
using _Main.ToolKit.SingletonFeature;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Main.SceneSys.Sys
{
    public class SceneSystem : Singleton<SceneSystem>
    {
        private readonly Stack<string> _pageStack = new Stack<string>();
        private bool _isLoading;

        public string CurrentSceneName => SceneManager.GetActiveScene().name;

        public bool IsLoading => _isLoading;

        #region API
        
        public static Task LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return Instance.LoadSceneAsync(sceneName, mode);
        }
        
        public static Task PushPage(string sceneName)
        {
            return Instance.PushPagePrivate(sceneName);
        }
        
        public static Task PopPage()
        {
            return Instance.PopPagePrivate();
        }
        
        public static Task UnloadScene(string sceneName)
        {
            return Instance.UnloadSceneAsync(sceneName);
        }

        #endregion

        #region Original Featrue

        private Task AwaitOperation(AsyncOperation op)
        {
            if (op.isDone) return Task.CompletedTask;
            var tcs = new TaskCompletionSource<bool>();
            op.completed += _ => tcs.TrySetResult(true);
            return tcs.Task;
        }

        private async Task LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (_isLoading) await WaitUntilNotLoading();
            _isLoading = true;
            try
            {
                if (mode == LoadSceneMode.Single)
                {
                    _pageStack.Clear();
                }
                var op = SceneManager.LoadSceneAsync(sceneName, mode);
                await AwaitOperation(op);
                if (mode == LoadSceneMode.Additive)
                {
                    var scene = SceneManager.GetSceneByName(sceneName);
                    if (scene.IsValid()) SceneManager.SetActiveScene(scene);
                }
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task PushPagePrivate(string sceneName)
        {
            if (_isLoading) await WaitUntilNotLoading();
            _isLoading = true;
            try
            {
                var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                await AwaitOperation(op);
                _pageStack.Push(sceneName);
                var scene = SceneManager.GetSceneByName(sceneName);
                if (scene.IsValid()) SceneManager.SetActiveScene(scene);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private async Task PopPagePrivate()
        {
            if (_isLoading) await WaitUntilNotLoading();
            if (_pageStack.Count == 0) return;
            _isLoading = true;
            try
            {
                var top = _pageStack.Pop();
                var op = SceneManager.UnloadSceneAsync(top);
                await AwaitOperation(op);
                if (_pageStack.Count > 0)
                {
                    var prev = _pageStack.Peek();
                    var scene = SceneManager.GetSceneByName(prev);
                    if (scene.IsValid()) SceneManager.SetActiveScene(scene);
                    else SetFirstLoadedSceneActive();
                }
                else
                {
                    SetFirstLoadedSceneActive();
                }
            }
            finally
            {
                _isLoading = false;
            }
        }
        

        private async Task UnloadSceneAsync(string sceneName)
        {
            if (_isLoading) await WaitUntilNotLoading();
            _isLoading = true;
            try
            {
                var op = SceneManager.UnloadSceneAsync(sceneName);
                await AwaitOperation(op);
                if (_pageStack.Contains(sceneName))
                {
                    var temp = new Stack<string>();
                    while (_pageStack.Count > 0)
                    {
                        var s = _pageStack.Pop();
                        if (s != sceneName) temp.Push(s);
                    }
                    while (temp.Count > 0) _pageStack.Push(temp.Pop());
                }
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void SetFirstLoadedSceneActive()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var s = SceneManager.GetSceneAt(i);
                if (s.IsValid())
                {
                    SceneManager.SetActiveScene(s);
                    return;
                }
            }
        }

        private async Task WaitUntilNotLoading()
        {
            while (_isLoading)
            {
                await Task.Yield();
            }
        }

        #endregion
    }
}