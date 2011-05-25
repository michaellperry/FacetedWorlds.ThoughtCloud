using System;
using System.ComponentModel;
using System.Linq;
using UpdateControls.XAML;
using FacetedWorlds.ThoughtCloud.ViewModel.Models;

namespace FacetedWorlds.ThoughtCloud.ViewModels
{
    public class ViewModelLocator
    {
        private readonly SynchronizationService _synchronizationService;

        private readonly MainViewModel _main;

        public ViewModelLocator()
        {
            NavigationModel navigationModel = new NavigationModel();
            _synchronizationService = new SynchronizationService(navigationModel);
            if (!DesignerProperties.IsInDesignTool)
                _synchronizationService.Initialize();
            _main = new MainViewModel(_synchronizationService.Community, navigationModel, _synchronizationService);
        }

        public object Main
        {
            get { return ForView.Wrap(_main); }
        }
    }
}
