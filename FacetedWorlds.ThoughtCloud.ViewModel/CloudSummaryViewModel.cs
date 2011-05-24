using FacetedWorlds.ThoughtCloud.Model;

namespace FacetedWorlds.ThoughtCloud.ViewModel
{
    public class CloudSummaryViewModel
    {
        private Cloud _cloud;

        public CloudSummaryViewModel(Cloud cloud)
        {
            _cloud = cloud;
        }

        internal Cloud Cloud
        {
            get { return _cloud; }
        }

        public string Text
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

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;
            CloudSummaryViewModel that = obj as CloudSummaryViewModel;
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
