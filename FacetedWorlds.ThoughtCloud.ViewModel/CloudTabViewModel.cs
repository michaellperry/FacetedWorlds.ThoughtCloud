using System.Windows;
using FacetedWorlds.ThoughtCloud.Model;
using FacetedWorlds.ThoughtCloud.ViewModel.Models;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class CloudTabViewModel
    {
        private readonly Cloud _cloud;
        private readonly CloudNavigationModel _navigation;

        public CloudTabViewModel(Cloud cloud, CloudNavigationModel navigation)
        {
            _cloud = cloud;
            _navigation = navigation;
        }

        internal Cloud Cloud
        {
            get { return _cloud; }
        }

        public string Header
        {
            get
            {
                Thought centralThought = _cloud.CentralThought;
                if (centralThought == null)
                    return "<New cloud>";
                string text = centralThought.Text;
                if (text == null)
                    return "<New cloud>";
                return text;
            }
        }

        public CloudViewModel Content
        {
            get { return new CloudViewModel(_cloud, _navigation); }
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            CloudTabViewModel that = obj as CloudTabViewModel;
            if (that == null)
                return false;
            return _cloud == that._cloud;
        }

        public override int GetHashCode()
        {
            return _cloud.GetHashCode();
        }
    }
}
