using System.Windows.Input;
using UpdateControls.Fields;
using UpdateControls.XAML;

namespace ThoughtCloud_Presentation.ViewModels
{
    public class FactsViewModel
    {
        public enum StateId
        {
            Start,
            NewIdentityMike,
            NewIdentityRussell,
            NewCloud1,
            NewCloud2,
            NewThought3,
            NewThought3Text,
            NewThought4,
            NewThought4Text,
            NewLink,
            NewShare
        }

        private Independent<StateId> _state = new Independent<StateId>();

        public StateId State
        {
            get { return _state; }
        }

        public ICommand Next
        {
            get
            {
                return MakeCommand
                    .Do(() => _state.Value = _state.Value + 1);
            }
        }

        public string Code
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
