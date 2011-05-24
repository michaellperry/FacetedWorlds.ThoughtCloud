using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FacetedWorlds.ThoughtCloud.Model;
using UpdateControls.XAML;
using System;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class HomeViewModel
    {
        private readonly Identity _identity;

        public HomeViewModel(Identity identity)
        {
            _identity = identity;
        }

        public IEnumerable<CloudSummaryViewModel> Clouds
        {
            get
            {
                return
                    from c in _identity.Clouds
                    select new CloudSummaryViewModel(c);
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
