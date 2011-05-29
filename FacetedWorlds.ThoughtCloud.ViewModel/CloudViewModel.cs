using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using FacetedWorlds.ThoughtCloud.Model;
using FacetedWorlds.ThoughtCloud.ViewModel.Models;
using UpdateControls;
using UpdateControls.XAML;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class CloudViewModel : IThoughtContainer
    {
        private const double HorizontalRadius = 110.0;
        private const double VerticalRadius = 60.0;

        private readonly Cloud _cloud;
        private readonly CloudNavigationModel _navigation;

        private Dictionary<Thought, Point> _centerByThought = new Dictionary<Thought, Point>();
        private Dependent _depCenterByThought;

        private Dictionary<Thought, InertialProperty> _inertialXByThought = new Dictionary<Thought,InertialProperty>();
        private Dictionary<Thought, InertialProperty> _inertialYByThought = new Dictionary<Thought, InertialProperty>();

        public CloudViewModel(Cloud cloud, CloudNavigationModel navigation)
        {
            _cloud = cloud;
            _navigation = navigation;

            _depCenterByThought = new Dependent(() =>
            {
                _centerByThought = CalculateCenterByThought();
                UpdateInertialCoordinates();
            });

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += new EventHandler(AnimateThoughts);
            timer.Start();
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
                    List<ThoughtViewModelActual> thoughtViewModels = new List<ThoughtViewModelActual>();
                    thoughtViewModels.Add(new ThoughtViewModelActual(focusThought, this));
                    CreateThoughtViewModels(focusThought, thoughtViewModels, 1);
                    return thoughtViewModels.OfType<ThoughtViewModel>();
                }
            }
        }

        public string LinkGeometry
        {
            get
            {
                List<Link> links = new List<Link>();
                Thought focusThought = _navigation.FocusThought;
                if (focusThought != null)
                    GetLinks(focusThought, links, 1);
                return string.Join(" ", links.Select(GetGeometry).ToArray());
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

        public Point GetCenterByThought(Thought thought)
        {
            _depCenterByThought.OnGet();
            InertialProperty x;
            InertialProperty y;
            if (_inertialXByThought.TryGetValue(thought, out x) &&
                _inertialYByThought.TryGetValue(thought, out y))
                return new Point(x.Value, y.Value);
            else
                return new Point(0.0, 0.0);
        }

        private void AnimateThoughts(object sender, EventArgs e)
        {
            List<InertialProperty> xProperties;
            List<InertialProperty> yProperties;
            lock (this)
            {
                xProperties = _inertialXByThought.Values.ToList();
                yProperties = _inertialYByThought.Values.ToList();
            }
            foreach (InertialProperty x in xProperties)
                x.OnTimer();
            foreach (InertialProperty y in yProperties)
                y.OnTimer();
        }

        private string GetGeometry(Link link)
        {
            if (link.Thoughts.Count() == 2)
            {
                Point start = GetCenterByThought(link.Thoughts.ElementAt(0));
                Point end = GetCenterByThought(link.Thoughts.ElementAt(1));
                return string.Format("M{0},{1} L{2},{3}", start.X, start.Y, end.X, end.Y);
            }
            else
            {
                return "M0,0 L0,0";
            }
        }

        private Dictionary<Thought, Point> CalculateCenterByThought()
        {
            Thought focusThought = _navigation.FocusThought;
            Dictionary<Thought, Point> centerByThought = new Dictionary<Thought, Point>();
            if (focusThought != null)
            {
                Point center = new Point(0.0, 0.0);
                centerByThought.Add(focusThought, center);
                Fan(focusThought, centerByThought, center, 0, 2.0 * Math.PI, 1);
            }
            return centerByThought;
        }

        private void UpdateInertialCoordinates()
        {
            foreach (Thought thought in _centerByThought.Keys)
            {
                Thought thisThought = thought;
                lock (this)
                {
                    if (!_inertialXByThought.ContainsKey(thought))
                        _inertialXByThought.Add(thisThought, new InertialProperty(() => GetX(thisThought)));
                    if (!_inertialYByThought.ContainsKey(thought))
                        _inertialYByThought.Add(thisThought, new InertialProperty(() => GetY(thisThought)));
                }
            }
        }

        private float GetX(Thought thought)
        {
            _depCenterByThought.OnGet();
            Point center = new Point();
            _centerByThought.TryGetValue(thought, out center);
            return (float)center.X;
        }

        private float GetY(Thought thought)
        {
            _depCenterByThought.OnGet();
            Point center = new Point();
            _centerByThought.TryGetValue(thought, out center);
            return (float)center.Y;
        }

        private void CreateThoughtViewModels(Thought thought, List<ThoughtViewModelActual> viewModels, int depth)
        {
            List<Thought> neighbors = thought.Neighbors
                .Distinct()
                .Where(n => !viewModels.Any(vm => vm.Thought == n))
                .ToList();
            foreach (Thought neighbor in neighbors)
            {
                viewModels.Add(new ThoughtViewModelActual(neighbor, this));
            }

            if (depth <= 3)
            {
                foreach (Thought neighbor in neighbors)
                {
                    CreateThoughtViewModels(neighbor, viewModels, depth + 1);
                }
            }
        }

        private void GetLinks(Thought thought, List<Link> links, int depth)
        {
            List<Link> neighbors = thought.Links
                .Distinct()
                .Where(l => !links.Contains(l))
                .ToList();
            foreach (Link link in neighbors)
                links.Add(link);

            if (depth <= 3)
            {
                foreach (Link link in neighbors)
                {
                    Thought neighbor = link.Thoughts.FirstOrDefault(t => t != thought);
                    if (neighbor != null)
                        GetLinks(neighbor, links, depth + 1);
                }
            }
        }

        private static void Fan(Thought thought, Dictionary<Thought, Point> centerByThought, Point origin, double startRadians, double sweepRadians, int depth)
        {
            List<Thought> neighbors = thought.Neighbors
                .Distinct()
                .Where(n => !centerByThought.ContainsKey(n))
                .ToList();
            int step = 0;
            double arc = sweepRadians / (double)neighbors.Count;
            foreach (Thought neighbor in neighbors)
            {
                double theta = arc * ((double)step + 0.5) + startRadians;
                Point center = new Point(
                    -HorizontalRadius * Math.Sin(theta) + origin.X,
                    VerticalRadius * Math.Cos(theta) + origin.Y);
                centerByThought.Add(neighbor, center);

                ++step;
            }

            if (depth <= 3)
            {
                step = 0;
                foreach (Thought neighbor in neighbors)
                {
                    Fan(neighbor, centerByThought, centerByThought[neighbor], arc * (double)step + startRadians, arc, depth + 1);
                    ++step;
                }
            }
        }
    }
}
