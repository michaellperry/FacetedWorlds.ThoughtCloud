using System.Windows;
using System.Windows.Input;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public interface ThoughtViewModel
    {
        string Text { get; set; }
        bool InConflict { get; }
        string Candidates { get; }
        Thickness Margin { get; }
        bool Editing { get; }
        ICommand Focus { get; }
    }
}