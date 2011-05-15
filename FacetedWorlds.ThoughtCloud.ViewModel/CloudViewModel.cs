using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class CloudViewModel
    {
        public IEnumerable<ThoughtViewModel> Thoughts
        {
            get
            {
                yield return new ThoughtViewModel();
            }
        }
    }
}
