using System.Collections.Generic;
using System.Linq;
using ThoughtCloud_Presentation.ViewModels;

namespace ThoughtCloud.Presentation.Navigation
{
    public class NavigationGraph
    {
        private NavigationController _controller;
        private List<IPresentationViewModel> _viewModels = new List<IPresentationViewModel>();
        private int _index = 0;

        public NavigationGraph(NavigationController controller)
        {
            _controller = controller;

            _viewModels.Add(new BulletPointViewModel("Correspondence")
                .AddBullet("Local storage")
                .AddBullet("Change queue")
                .AddBullet("Synchronization service")
                .AddBullet("Push notification")
                .AddBullet("Conflict detection"));
            _viewModels.Add(new BulletPointViewModel("Key Concepts")
                .AddBullet("Facts")
                .AddBullet("Storage strategy")
                .AddBullet("Communication strategy")
                .AddBullet("Community"));
            _viewModels.Add(new FactsViewModel());
            _viewModels.Add(new ArchitectureViewModel());
        }

        public void Start()
        {
            _controller.NavigateTo(_viewModels.First());
        }

        public void Forward()
        {
            bool navigated = _viewModels[_index].Forward();
            if (!navigated && _index < _viewModels.Count - 1)
            {
                _index++;
                _controller.NavigateTo(_viewModels[_index]);
            }
        }

        public void Backward()
        {
            bool navigated = _viewModels[_index].Backward();
            if (!navigated && _index > 0)
            {
                _index--;
                _controller.NavigateTo(_viewModels[_index]);
            }
        }
    }
}
