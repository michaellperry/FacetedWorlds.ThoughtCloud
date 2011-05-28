using System.Windows;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public interface ThoughtViewModel
    {
        string Text { get; set; }
        Thickness Margin { get; }
        bool Editing { get; }
    }
}