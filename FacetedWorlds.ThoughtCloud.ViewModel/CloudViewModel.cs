﻿using System;
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
        private const double HorizontalRadius = 110.0;
        private const double VerticalRadius = 60.0;

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
                    List<ThoughtViewModelActual> thoughtViewModels = new List<ThoughtViewModelActual>();
                    thoughtViewModels.Add(new ThoughtViewModelActual(focusThought, this));
                    CreateThoughtViewModels(focusThought, thoughtViewModels, 1);
                    return thoughtViewModels.OfType<ThoughtViewModel>();
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
                Point center = new Point(0.0, 0.0);
                centerByThought.Add(focusThought, center);
                Fan(focusThought, centerByThought, center, 0, 2.0 * Math.PI, 1);
            }
            return centerByThought;
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

                if (depth <= 3)
                    CreateThoughtViewModels(neighbor, viewModels, depth + 1);
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

                if (depth <= 3)
                {
                    Fan(neighbor, centerByThought, center, arc * (double)step + startRadians, arc, depth + 1);
                }

                ++step;
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
    }
}
