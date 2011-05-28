using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FacetedWorlds.ThoughtCloud.Model;
using FacetedWorlds.ThoughtCloud.ViewModel.Models;
using UpdateControls;
using UpdateControls.XAML;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class CloudViewModel : IThoughtContainer
    {
        private readonly Cloud _cloud;
        private readonly CloudNavigationModel _navigation;

        private Dictionary<Thought, Point> _centerByThought = new Dictionary<Thought, Point>();
        private Dependent _depCenterByThought;

        public CloudViewModel(Cloud cloud, CloudNavigationModel navigation)
        {
            _cloud = cloud;
            _navigation = navigation;

            _depCenterByThought = new Dependent(() => _centerByThought = CalculateCenterByThought());
        }

        public IEnumerable<ThoughtViewModel> Thoughts
        {
            get
            {
                Thought focusThought = _navigation.FocusThought;
                if (focusThought == null)
                {
                    return Enumerable.Repeat(new ThoughtViewModelSimulated(_cloud, this), 1)
                        .OfType<ThoughtViewModel>();
                }
                else
                {
                    return Enumerable.Repeat(new ThoughtViewModelActual(focusThought, this), 1).Union(
                        from n in focusThought.Neighbors
                        where n != focusThought
                        select new ThoughtViewModelActual(n, this))
                        .OfType<ThoughtViewModel>();
                }
            }
        }

        public ICommand NewThought
        {
            get
            {
                return MakeCommand.Do(() =>
                {
                    Thought thought = _cloud.NewThought();
                    Thought focusThought = _navigation.FocusThought;
                    if (focusThought == null)
                    {
                        focusThought = _cloud.NewThought();
                        _cloud.CentralThought = focusThought;
                    }
                    focusThought.LinkTo(thought);
                    _navigation.EditThought = thought;
                });
            }
        }

        public ICommand ClearEdit
        {
            get
            {
                return MakeCommand
                    .Do(() => _navigation.EditThought = null);
            }
        }

        public Point GetCenterByThought(Thought thought)
        {
            _depCenterByThought.OnGet();
            Point center = new Point();
            _centerByThought.TryGetValue(thought, out center);
            return center;
        }

        private Dictionary<Thought, Point> CalculateCenterByThought()
        {
            Thought focusThought = _navigation.FocusThought;
            Dictionary<Thought, Point> centerByThought = new Dictionary<Thought, Point>();
            if (focusThought != null)
            {
                centerByThought.Add(focusThought, new Point(0.0, 0.0));
                List<Thought> neighbors = focusThought.Neighbors
                    .Where(n => n != focusThought)
                    .ToList();
                int step = 0;
                double horizontalRadius = 110.0;
                double verticalRadius = 60.0;
                foreach (Thought neighbor in neighbors)
                {
                    double theta = 2 * Math.PI * (double)step / (double)neighbors.Count;
                    centerByThought.Add(neighbor, new Point(
                        horizontalRadius * Math.Sin(theta),
                        -verticalRadius * Math.Cos(theta)));
                    ++step;
                }
            }
            return centerByThought;
        }


        public Thought FocusThought
        {
            get { return _navigation.FocusThought; }
            set { _navigation.FocusThought = value; }
        }

        public Thought EditThought
        {
            get { return _navigation.EditThought; }
            set { _navigation.EditThought = value; }
        }
    }
}
