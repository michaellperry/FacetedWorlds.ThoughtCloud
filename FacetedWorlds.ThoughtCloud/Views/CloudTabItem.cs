using System.Windows;
using System.Windows.Controls;
using FacetedWorlds.ThoughtCloud.ViewModel;
using UpdateControls;

namespace FacetedWorlds.ThoughtCloud.Views
{
    public class CloudTabItem : TabItem
    {
        private CloudTabViewModel _viewModel;
        private Dependent _depHeader;

        public CloudTabItem(CloudTabViewModel viewModel)
        {
            _viewModel = viewModel;

            Content = new CloudView { DataContext = _viewModel.Content };

            _depHeader = new Dependent(() => Header = _viewModel.Header);
            _depHeader.Invalidated += () => Deployment.Current.Dispatcher.BeginInvoke(() => _depHeader.OnGet());
            _depHeader.Touch();
        }
    }
}
