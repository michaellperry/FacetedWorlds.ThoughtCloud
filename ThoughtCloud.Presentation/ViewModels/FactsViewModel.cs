using UpdateControls.Fields;

namespace ThoughtCloud_Presentation.ViewModels
{
    public class FactsViewModel : IPresentationViewModel
    {
        public enum StateId
        {
            Start,
            NewIdentityMike,
            NewIdentityRussell,
            NewCloud1,
            NewCloud2,
            NewThought3,
            NewThought3Text,
            NewThought4,
            NewThought4Text,
            NewLink,
            NewShare
        }

        private Independent<StateId> _state = new Independent<StateId>();

        public StateId State
        {
            get { return _state; }
        }

        public bool Forward()
        {
            if (_state.Value != StateId.NewShare)
            {
                _state.Value = _state.Value + 1;
                return true;
            }
            else
                return false;
        }

        public bool Backward()
        {
            if (_state.Value != StateId.Start)
            {
                _state.Value = _state.Value - 1;
                return true;
            }
            else
                return false;
        }

        public string Code
        {
            get
            {
                switch (_state.Value)
                {
                    case StateId.NewIdentityMike:
                        return "Identity mike = Community.AddFact(\nnew Identity(\"mike\"));";
                    case StateId.NewIdentityRussell:
                        return "Identity russell = Community.AddFact(\nnew Identity(\"russell\"));";
                    case StateId.NewCloud1:
                        return "Cloud cloud1 = Community.AddFact(new Cloud());";
                    case StateId.NewCloud2:
                        return "Cloud cloud2 = Community.AddFact(new Cloud());";
                    case StateId.NewThought3:
                        return "Thought thought3 = Community.AddFact(\nnew Thought(cloud1));";
                    case StateId.NewThought3Text:
                        return "thought3.Text = \"Star Wars\";";
                    case StateId.NewThought4:
                        return "Thought thought4 = Community.AddFact(\nnew Thought(cloud1));";
                    case StateId.NewThought4Text:
                        return "thought4.Text = \"Characters\";";
                    case StateId.NewLink:
                        return "Community.AddFact(new Link(\nnew List<Thought> { thought3, thought4 }));";
                    case StateId.NewShare:
                        return "Community.AddFact(new Share(russell, cloud1));";
                }
                return string.Empty;
            }
        }
    }
}
