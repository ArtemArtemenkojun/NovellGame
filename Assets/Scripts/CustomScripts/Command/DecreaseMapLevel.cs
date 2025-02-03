using Controllers;
using Naninovel;

namespace Command
{
    [CommandAlias("DecreaseMapLevel")]
    public class DecreaseMapLevel : Naninovel.Command
    {
        public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
        {
            var uiManager = Engine.GetService<UIManager>();
            var questScreen = uiManager.GetUI<MapsController>();
            var valueManager = Engine.GetService<ICustomVariableManager>();
            
            valueManager.TryGetVariableValue<int>("MapID", out var intValue);
            intValue -=1;
            valueManager.TrySetVariableValue("MapID", intValue);
            questScreen.SetActiveMapByID();
            
            return UniTask.CompletedTask;
        }
    }
}