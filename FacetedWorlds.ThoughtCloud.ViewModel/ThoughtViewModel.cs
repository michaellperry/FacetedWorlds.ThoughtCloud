using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacetedWorlds.ThoughtCloud.Model;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class ThoughtViewModel
    {
        private Thought _thought;

        public ThoughtViewModel(Thought thought)
        {
            _thought = thought;
        }

        public string Text
        {
            get { return _thought.Text.Value ?? "My thought"; }
            set { _thought.Text = value; }
        }

        public string HadBy
        {
            get { return _thought.Creator.AnonymousId; }
        }
    }
}
