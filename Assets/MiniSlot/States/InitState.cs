using AxGrid;
using AxGrid.FSM;

namespace MiniSlot.States
{
    /// <summary>
    /// Инициализация машины. Создание полей модели.
    /// Переходит в StateIdle
    /// </summary>
    [State(Consts.State.StateInit)]
    internal class InitState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Settings.Model.Set(Consts.ModelFields.LastReward, -1);

            Parent.Change(Consts.State.StateIdle);
        }
    }
}