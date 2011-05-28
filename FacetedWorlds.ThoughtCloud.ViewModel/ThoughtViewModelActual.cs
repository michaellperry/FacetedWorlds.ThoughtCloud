using System.Windows;
using FacetedWorlds.ThoughtCloud.Model;
using System;
using System.Windows.Input;
using UpdateControls.XAML;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class ThoughtViewModelActual : ThoughtViewModel
    {
        private readonly Thought _thought;
        private readonly IThoughtContainer _container;

        public ThoughtViewModelActual(Thought thought, IThoughtContainer container)
        {
            _thought = thought;
            _container = container;
        }

        internal Thought Thought
        {
            get { return _thought; }
        }

        public string Text
        {
            get { return _thought.Text.Value ?? "My thought"; }
            set
            {
                _thought.Text = value;
                _container.EditThought = null;
            }
        }

        public Thickness Margin
        {
            get
            {
                Point center = _container.GetCenterByThought(_thought);
                return new Thickness(center.X, center.Y, -center.X, -center.Y);
            }
        }

        public ICommand Focus
        {
            get
            {
                return MakeCommand
                    .Do(() =>
                    {
                        if (_container.FocusThought != _thought)
                            _container.FocusThought = _thought;
                        else if (_container.EditThought != _thought)
                            _container.EditThought = _thought;
                        else
                            _container.EditThought = null;
                    });
            }
        }

        public bool Editing
        {
            get { return _container.EditThought == _thought; }
        }
    }
}
