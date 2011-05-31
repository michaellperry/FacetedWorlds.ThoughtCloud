using System.Windows;
using FacetedWorlds.ThoughtCloud.Model;
using System.Windows.Input;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class ThoughtViewModelSimulated : ThoughtViewModel
    {
        private readonly Cloud _cloud;
        private readonly IThoughtContainer _container;
        public ThoughtViewModelSimulated(Cloud cloud, IThoughtContainer container)
        {
            _container = container;
            _cloud = cloud;
        }

        public string Text
        {
            get { return "My thought"; }
            set
            {
                Thought thought = _cloud.NewThought();
                _cloud.CentralThought = thought;
                thought.Text = value;
                _container.EditThought = null;
            }
        }

        public bool InConflict
        {
            get { return false; }
        }

        public string Candidates
        {
            get { return string.Empty; }
        }

        public Thickness Margin
        {
            get { return new Thickness(0.0, 0.0, 0.0, 0.0); }
        }

        public bool Editing
        {
            get { return true; }
        }

        public ICommand Focus
        {
            get { return null; }
        }
    }
}
