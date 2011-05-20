using FacetedWorlds.ThoughtCloud.Model;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class ThoughtViewModelActual : ThoughtViewModel
    {
        private Thought _thought;

        public ThoughtViewModelActual(Thought thought)
        {
            _thought = thought;
        }

        public string Text
        {
            get { return _thought.Text.Value ?? "My thought"; }
            set { _thought.Text = value; }
        }
    }
}
