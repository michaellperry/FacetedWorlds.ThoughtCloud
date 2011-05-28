using FacetedWorlds.ThoughtCloud.Model;
using UpdateControls.Fields;

namespace FacetedWorlds.ThoughtCloud.ViewModel.Models
{
    public class CloudNavigationModel
    {
        private readonly Cloud _cloud;

        private Independent<Thought> _focusThought = new Independent<Thought>();
        private Independent<Thought> _editThought = new Independent<Thought>();

        public CloudNavigationModel(Cloud cloud)
        {
            _cloud = cloud;
        }

        public Thought FocusThought
        {
            get { return _focusThought.Value ?? _cloud.CentralThought.Value; }
            set { _focusThought.Value = value; }
        }

        public Thought EditThought
        {
            get { return _editThought; }
            set { _editThought.Value = value; }
        }
    }
}
