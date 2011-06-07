using System.Collections.Generic;
using System.Linq;
using ThoughtCloud_Presentation.ViewModels;

namespace ThoughtCloud.Presentation.Navigation
{
    public class NavigationGraph
    {
        private NavigationController _controller;
        private List<IPresentationViewModel> _viewModels = new List<IPresentationViewModel>();
        private int _viewModelIndex = 0;

        public NavigationGraph(NavigationController controller)
        {
            _controller = controller;

            _viewModels.Add(new FactsViewModel());
            _viewModels.Add(new ArchitectureViewModel());
        }

        public void Start()
        {
            _controller.NavigateTo(_viewModels.First());
        }

        public void Forward()
        {
            bool navigated = _viewModels[_viewModelIndex].Forward();
            if (!navigated && _viewModelIndex < _viewModels.Count - 1)
            {
                _viewModelIndex++;
                _controller.NavigateTo(_viewModels[_viewModelIndex]);
            }
        }

        public void Backward()
        {
            bool navigated = _viewModels[_viewModelIndex].Backward();
            if (!navigated && _viewModelIndex > 0)
            {
                _viewModelIndex--;
                _controller.NavigateTo(_viewModels[_viewModelIndex]);
            }
        }
    }
}
