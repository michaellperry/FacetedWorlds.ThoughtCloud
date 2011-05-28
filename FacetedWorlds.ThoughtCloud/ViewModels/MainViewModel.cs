using System.Collections.Generic;
using System.Linq;
using FacetedWorlds.ThoughtCloud.ViewModel;
using FacetedWorlds.ThoughtCloud.ViewModel.Models;
using UpdateControls.Correspondence;

namespace FacetedWorlds.ThoughtCloud.ViewModels
{
    public class MainViewModel
    {
        private Community _community;
        private NavigationModel _navigationModel;
        private SynchronizationService _synhronizationService;

        public MainViewModel(Community community, NavigationModel navigationModel, SynchronizationService synhronizationService)
        {
            _community = community;
            _navigationModel = navigationModel;
            _synhronizationService = synhronizationService;
        }

        public bool Synchronizing
        {
            get { return _community.Synchronizing; }
        }

        public bool HasError
        {
            get { return _community.LastException != null; }
        }

        public string LastError
        {
            get
            {
                return _community.LastException == null
                    ? null
                    : _community.LastException.Message;
            }
        }

        public HomeViewModel Home
        {
            get
            {
                return _navigationModel.CurrentUser == null
                    ? null
                    : new HomeViewModel(_navigationModel.CurrentUser, _navigationModel);
            }
        }

        public IEnumerable<CloudTabViewModel> OpenClouds
        {
            get
            {
                return
                    from cloud in _navigationModel.OpenClouds
                    select new CloudTabViewModel(cloud, new CloudNavigationModel(cloud));
            }
        }
    }
}
