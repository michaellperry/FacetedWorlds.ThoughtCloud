using System.Windows;
using FacetedWorlds.ThoughtCloud.Model;
using System;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class ThoughtViewModelActual : ThoughtViewModel
    {
        private readonly Thought _thought;
        private readonly Func<Thought, Point> _getCenterByThought;
        
        public ThoughtViewModelActual(Thought thought, Func<Thought, Point> getCenterByThought)
        {
            _thought = thought;
            _getCenterByThought = getCenterByThought;
        }

        public string Text
        {
            get { return _thought.Text.Value ?? "My thought"; }
            set { _thought.Text = value; }
        }

        public Thickness Margin
        {
            get
            {
                Point center = _getCenterByThought(_thought);
                return new Thickness(center.X, center.Y, 0.0, 0.0);
            }
        }

        public bool Editing
        {
            get { return false; }
        }
    }
}
