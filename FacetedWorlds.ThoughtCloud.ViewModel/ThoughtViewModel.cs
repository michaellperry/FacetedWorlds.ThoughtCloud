using System.Windows;
using System.Windows.Input;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public interface ThoughtViewModel
    {
        string Text { get; set; }
        Thickness Margin { get; }
        bool Editing { get; }
        ICommand Focus { get; }
    }
}