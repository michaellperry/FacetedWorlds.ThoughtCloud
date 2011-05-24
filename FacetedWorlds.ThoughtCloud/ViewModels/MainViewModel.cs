using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FacetedWorlds.ThoughtCloud.Models;
using UpdateControls.Correspondence;
using UpdateControls.XAML;
using FacetedWorlds.ThoughtCloud.ViewModel;
using FacetedWorlds.ThoughtCloud.Model;

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
            get { return _synhronizationService.Synchronizing; }
        }

        public HomeViewModel Home
        {
            get
            {
                return _navigationModel.CurrentUser == null
                    ? null
                    : new HomeViewModel(_navigationModel.CurrentUser);
            }
        }
    }
}
