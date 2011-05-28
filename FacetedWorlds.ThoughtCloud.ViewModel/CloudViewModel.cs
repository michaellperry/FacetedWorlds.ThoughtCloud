using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FacetedWorlds.ThoughtCloud.Model;
using UpdateControls.XAML;
using System.Windows;
using UpdateControls;
using UpdateControls.Collections;
using System;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class CloudViewModel
    {
        private Cloud _cloud;
        private DependentList<ThoughtViewModel> _thoughts;
        private Dictionary<Thought, Point> _centerByThought = new Dictionary<Thought, Point>();
        private Dependent _depCenterByThought;

        public CloudViewModel(Cloud cloud)
        {
            _cloud = cloud;

            _thoughts = new DependentList<ThoughtViewModel>(CreateThoughViewModels);
            _depCenterByThought = new Dependent(() => _centerByThought = CalculateCenterByThought());
        }

        public IEnumerable<ThoughtViewModel> Thoughts
        {
            get { return _thoughts; }
        }

        public ICommand NewThought
        {
            get
            {
                return MakeCommand.Do(() =>
                {
                    Thought thought = _cloud.NewThought();
                    Thought centralThought = _cloud.CentralThought;
                    if (centralThought == null)
                    {
                        centralThought = _cloud.NewThought();
                        _cloud.CentralThought = centralThought;
                    }
                    centralThought.LinkTo(thought);
                });
            }
        }

        private IEnumerable<ThoughtViewModel> CreateThoughViewModels()
        {
            Thought centralThought = _cloud.CentralThought;
            if (centralThought == null)
            {
                return Enumerable.Repeat(new ThoughtViewModelSimulated(_cloud), 1)
                    .OfType<ThoughtViewModel>();
            }
            else
            {
                return Enumerable.Repeat(new ThoughtViewModelActual(centralThought, GetCenterByThought), 1).Union(
                    from n in centralThought.Neighbors
                    where n != centralThought
                    select new ThoughtViewModelActual(n, GetCenterByThought))
                    .OfType<ThoughtViewModel>();
            }
        }

        private Point GetCenterByThought(Thought thought)
        {
            _depCenterByThought.OnGet();
            Point center = new Point();
            _centerByThought.TryGetValue(thought, out center);
            return center;
        }

        private Dictionary<Thought, Point> CalculateCenterByThought()
        {
            Thought centralThought = _cloud.CentralThought;
            Dictionary<Thought, Point> centerByThought = new Dictionary<Thought, Point>();
            if (centralThought != null)
            {
                centerByThought.Add(centralThought, new Point(0.0, 0.0));
                List<Thought> neighbors = centralThought.Neighbors
                    .Where(n => n != centralThought)
                    .ToList();
                int step = 0;
                double horizontalRadius = 230.0;
                double verticalRadius = 120.0;
                foreach (Thought neighbor in neighbors)
                {
                    double theta = 2 * Math.PI * (double)step / (double)neighbors.Count;
                    centerByThought.Add(neighbor, new Point(
                        horizontalRadius * Math.Cos(theta),
                        verticalRadius * Math.Sin(theta)));
                    ++step;
                }
            }
            return centerByThought;
        }
    }
}
