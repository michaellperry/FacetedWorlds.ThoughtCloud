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
                if (centralThought == null)
                {
                    return Enumerable.Repeat(new ThoughtViewModelSimulated(_cloud), 1)
                        .OfType<ThoughtViewModel>();
                }
                else
                {
                    return Enumerable.Repeat(new ThoughtViewModelActual(centralThought), 1).Union(
                        from n in centralThought.Neighbors
                        where n != centralThought
                        select new ThoughtViewModelActual(n))
                        .OfType<ThoughtViewModel>();
                }
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
                    if (centralThought == null)
                    {
                        centralThought = _cloud.NewThought();
                        _cloud.CentralThought = centralThought;
                    }
                    centralThought.LinkTo(thought);
                });
            }
        }
    }
}
