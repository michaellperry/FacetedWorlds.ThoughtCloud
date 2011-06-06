using System.Windows.Input;
using UpdateControls.Fields;
using UpdateControls.XAML;
using System;

namespace ThoughtCloud_Presentation.ViewModels
{
    public interface IPresentationViewModel
    {
        bool Backward();
        bool Forward();
    }
    public class FactsViewModel : IPresentationViewModel
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

        public bool Forward()
        {
            if (_state.Value != StateId.NewShare)
            {
                _state.Value = _state.Value + 1;
                return true;
            }
            else
                return false;
        }

        public bool Backward()
        {
            if (_state.Value != StateId.Start)
            {
                _state.Value = _state.Value - 1;
                return true;
            }
            else
                return false;
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
