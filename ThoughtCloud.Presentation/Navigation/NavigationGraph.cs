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

            _viewModels.Add(new TitleViewModel());
            _viewModels.Add(new SlidesAndCodeViewModel());
            _viewModels.Add(new AboutMeViewModel());
            _viewModels.Add(new BrainViewModel());
            _viewModels.Add(new BulletPointViewModel("Occasionally-connected Silverlight clients")
                .AddBullet("Why?")
                .AddBullet("Architecture")
                .AddBullet("Correspondence")
                .AddBullet("Next steps"));
            _viewModels.Add(new BulletPointViewModel("Occasionally Connected Clients")
                .AddBullet("View data off-line")
                .AddBullet("Make changes off-line")
                .AddBullet("Work with a subset of data")
                .AddBullet("Store and forward")
                .AddBullet("Propogate changes to the user"));
            _viewModels.Add(new BulletPointViewModel("Correspondence")
                .AddBullet("Local storage")
                .AddBullet("Change queue")
                .AddBullet("Synchronization service")
                .AddBullet("Push notification")
                .AddBullet("Conflict detection"));
            _viewModels.Add(new ArchitectureViewModel());
            _viewModels.Add(new BulletPointViewModel("The Community is ...")
                .AddBullet("Users")
                .AddBullet("Devices")
                .AddBullet("Data")
                .AddBullet("Services"));
            _viewModels.Add(new BulletPointViewModel("The Community ...")
                .AddBullet("Calls strategies")
                .AddBullet("Caches facts")
                .AddBullet("Subscribes to queues")
                .AddBullet("Is model agnostic"));
            _viewModels.Add(new FactsViewModel());
            _viewModels.Add(new BulletPointViewModel("Next Steps")
                .AddBullet("qedcode.com/correspondence")
                .AddBullet("NuGet Correspondence. Silverlight.AllInOne")
                .AddBullet("Correspondence.CodePlex.com")
                .AddBullet("@MichaelLPerry"));
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
