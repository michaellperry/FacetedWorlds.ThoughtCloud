using System.Windows;
using FacetedWorlds.ThoughtCloud.Model;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class ThoughtViewModelSimulated : ThoughtViewModel
    {
        private Cloud _cloud;

        public ThoughtViewModelSimulated(Cloud cloud)
        {
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
            }
        }

        public Thickness Margin
        {
            get { return new Thickness(0.0, 0.0, 0.0, 0.0); }
        }

        public bool Editing
        {
            get { return true; }
        }
    }
}
