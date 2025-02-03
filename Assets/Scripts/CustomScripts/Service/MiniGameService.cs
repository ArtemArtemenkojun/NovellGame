using System;
using System.Threading.Tasks;
using DTT.MinigameMemory;
using Naninovel;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Service
{
    [InitializeAtRuntime]
    [CommandAlias("miniGame")]
    public class MiniGameService : Naninovel.Command,IEngineService
    {
        private MemoryGameManager _memoryGameManager;
        private MemoryGameResults _memoryGameResults;
        private TaskCompletionSource<MemoryGameResults> _taskCompletionSource = new TaskCompletionSource<MemoryGameResults>();

        public BooleanParameter State;
        
        public UniTask InitializeServiceAsync() => UniTask.CompletedTask;
        
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            if (State)
                return StartGameAsync();

            ResetService();
            return UniTask.CompletedTask;
        }
        
        public void ResetService()
        {
            if (_memoryGameManager != null)
            {
                _memoryGameManager.Finish -= ProcessGameResults;
                _memoryGameManager = null;
            }
            _memoryGameResults = null;
            State = false;
        }

        public void DestroyService() => ResetService();
        
        public async UniTask StartGameAsync()
        {
            try
            {
                var sceneLoadingTask = SceneManager.LoadSceneAsync("Demo");
                await sceneLoadingTask;
                _memoryGameManager = Object.FindObjectOfType<MemoryGameManager>();
                
                if (_memoryGameManager == null)
                {
                    Debug.LogError("MemoryGameManager not found.");
                    return;
                }
                
                _memoryGameManager.Finish += ProcessGameResults;
                await _taskCompletionSource.Task;
                await SceneManager.UnloadSceneAsync("Demo");
                
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error starting game: {ex.Message}");
            }
        }

        private async void CompleteGameSession(MemoryGameResults results)
        {
            try
            {
                _taskCompletionSource.TrySetResult(results);
                
                if (_memoryGameManager != null)
                    _memoryGameManager.Finish -= ProcessGameResults;
                else
                    Debug.LogWarning("MemoryGameManager is null. Can't unsubscribe from event.");
                
                await SceneManager.UnloadSceneAsync("Demo");
                await SceneManager.LoadSceneAsync("SampleScene");
                
                State = false;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error during game finish: {ex.Message}");
            }
        }

        private void ProcessGameResults(MemoryGameResults results)
        {
            if (results.amountOfTurns >= 3)
            {
                CompleteGameSession(results);
                return;
            }
            
            if (_memoryGameResults == null)
            {
                _memoryGameResults = results;
                return;
            }
            
            _memoryGameResults.timeTaken += results.timeTaken;
            _memoryGameResults.amountOfTurns += results.amountOfTurns;
            _taskCompletionSource.TrySetResult(_memoryGameResults);
        }
    }
}