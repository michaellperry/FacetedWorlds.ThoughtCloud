using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FacetedWorlds.ThoughtCloud.Model;
using UpdateControls.XAML;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class HomeViewModel
    {
        private readonly Identity _identity;

        public HomeViewModel(Identity identity)
        {
            _identity = identity;
        }

        public IEnumerable<CloudViewModel> Clouds
        {
            get
            {
                return
                    from c in _identity.Clouds
                    select new CloudViewModel(_identity, null);
            }
        }

        public ICommand AddCloud
        {
            get
            {
                return MakeCommand
                    .Do(() => _identity.NewCloud());
            }
        }
    }
}
