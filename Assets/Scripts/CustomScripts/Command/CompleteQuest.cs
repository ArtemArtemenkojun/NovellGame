using Naninovel;
using Views;

namespace Command
{
    [CommandAlias("completeQuest")]
    public class CompleteQuest : Naninovel.Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var uiManager = Engine.GetService<UIManager>();
            var questScreen = uiManager.GetUI<QuestView>();
            
            questScreen.CompleteQuest();
            
            return UniTask.CompletedTask;
        }
    }
}