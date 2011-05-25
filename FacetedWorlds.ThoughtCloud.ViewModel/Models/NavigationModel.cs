using System.Collections.Generic;
using FacetedWorlds.ThoughtCloud.Model;
using UpdateControls.Collections;
using UpdateControls.Fields;

namespace FacetedWorlds.ThoughtCloud.ViewModel.Models
{
    public class NavigationModel
    {
        private Independent<Identity> _currentUser = new Independent<Identity>();
        private IndependentList<Cloud> _openClouds = new IndependentList<Cloud>();
        private Independent<Cloud> _selectedCloud = new Independent<Cloud>();

        public Identity CurrentUser
        {
            get { return _currentUser; }
            set { _currentUser.Value = value; }
        }

        public IEnumerable<Cloud> OpenClouds
        {
            get { return _openClouds; }
        }

        public void OpenCloud(Cloud cloud)
        {
            if (!_openClouds.Contains(cloud))
                _openClouds.Add(cloud);
        }

        public Cloud SelectedCloud
        {
            get { return _selectedCloud; }
            set { _selectedCloud.Value = value; }
        }
    }
}
