using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace FacetedWorlds.ThoughtCloud.Model
{
    public partial class Thought
    {
        public Link LinkTo(Thought otherThought)
        {
            return Community.AddFact(new Link(new List<Thought> { this, otherThought }));
        }
    }
}
