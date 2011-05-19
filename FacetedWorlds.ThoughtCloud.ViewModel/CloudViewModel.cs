using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FacetedWorlds.ThoughtCloud.Model;
using UpdateControls.XAML;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class CloudViewModel
    {
        private Identity _identity;
        private Thought _centralThought;
        
        public CloudViewModel(Identity identity, Thought centralThought)
        {
            _identity = identity;
            _centralThought = centralThought;
        }

        public IEnumerable<ThoughtViewModel> Thoughts
        {
            get
            {
                return Enumerable.Repeat(new ThoughtViewModel(_centralThought), 1).Union(
                    from n in _centralThought.Neighbors
                    where n != _centralThought
                    select new ThoughtViewModel(n));
            }
        }

        public ICommand NewThought
        {
            get
            {
                return MakeCommand.Do(() => _centralThought.LinkTo(_identity.NewThought()));
            }
        }
    }
}
