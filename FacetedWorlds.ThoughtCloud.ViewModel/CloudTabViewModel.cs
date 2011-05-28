using System.Windows;
using FacetedWorlds.ThoughtCloud.Model;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class CloudTabViewModel
    {
        private readonly Cloud _cloud;

        public CloudTabViewModel(Cloud cloud)
        {
            _cloud = cloud;
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
            get { return new CloudViewModel(_cloud); }
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
