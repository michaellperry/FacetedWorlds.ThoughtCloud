using System.Windows;
using FacetedWorlds.ThoughtCloud.Model;
using System;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public interface IThoughtContainer
    {
        Point GetCenterByThought(Thought thought);
        Thought FocusThought { get; set; }
        Thought EditThought { get; set; }
    }
}
