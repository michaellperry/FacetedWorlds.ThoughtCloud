using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FacetedWorlds.ThoughtCloud.Model;
using UpdateControls.XAML;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class CloudViewModel
    {
        private Cloud _cloud;

        public CloudViewModel(Cloud cloud)
        {
            _cloud = cloud;
        }

        public IEnumerable<ThoughtViewModel> Thoughts
        {
            get
            {
                return Enumerable.Repeat(new ThoughtViewModel(_cloud.CentralThought), 1).Union(
                    from n in _cloud.CentralThought.Neighbors
                    where n != _cloud.CentralThought
                    select new ThoughtViewModel(n));
            }
        }

        public ICommand NewThought
        {
            get
            {
                return MakeCommand.Do(() =>
                {
                    Thought thought = _cloud.Creator.NewThought();
                    _cloud.CentralThought.LinkTo(thought);
                });
            }
        }
    }
}
