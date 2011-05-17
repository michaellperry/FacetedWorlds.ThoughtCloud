using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacetedWorlds.ThoughtCloud.Model;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class CloudViewModel
    {
        private Thought _centralThought;

        public CloudViewModel(Thought centralThought)
        {
            _centralThought = centralThought;
        }

        public IEnumerable<ThoughtViewModel> Thoughts
        {
            get
            {
                yield return new ThoughtViewModel(_centralThought);
            }
        }
    }
}
