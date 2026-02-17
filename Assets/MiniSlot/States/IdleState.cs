using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace MiniSlot.States
{
    /// <summary>
    /// Состояние покоя системы.
    /// Слушает событие нажатия кнопки Start.
    /// Переходит в StateStarting
    /// </summary>
    [State(Consts.State.StateIdle)]
    internal class IdleState : FSMState
    {
        [Enter]
        private void EnterThis()
        {
            Settings.Model.Set(Consts.Buttons.StartButton.EnableField, true);
            Settings.Model.Set(Consts.Buttons.StopButton.EnableField, false);
        }

        [Bind(Consts.Buttons.StartButton.OnClick)]
        private void OnStartClick()
        {
            Parent.Change(Consts.State.StateStarting);
        }
    }
}