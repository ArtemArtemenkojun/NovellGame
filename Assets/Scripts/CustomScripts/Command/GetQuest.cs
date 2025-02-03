using Naninovel;
using Views;

namespace Command
{
    [CommandAlias("getQuest")]
    public class GetQuest : Naninovel.Command
    {
        public StringParameter Name;

        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var uiManager = Engine.GetService<UIManager>();
            var questScreen = uiManager.GetUI<QuestView>();

            if (Assigned(Name))
            {
                questScreen.ActivateQuestItem(Name);
            }
            
            return UniTask.CompletedTask;
        }
    }
}