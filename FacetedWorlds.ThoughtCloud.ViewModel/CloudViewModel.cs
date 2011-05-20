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
                Thought centralThought = _cloud.CentralThought;
                return Enumerable.Repeat(new ThoughtViewModel(centralThought), 1).Union(
                    from n in centralThought.Neighbors
                    where n != centralThought
                    select new ThoughtViewModel(n));
            }
        }

        public ICommand NewThought
        {
            get
            {
                return MakeCommand.Do(() =>
                {
                    Thought thought = _cloud.NewThought();
                    Thought centralThought = _cloud.CentralThought;
                    centralThought.LinkTo(thought);
                });
            }
        }
    }
}
