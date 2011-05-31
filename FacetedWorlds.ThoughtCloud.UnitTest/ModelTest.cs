using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Memory;
using FacetedWorlds.ThoughtCloud.Model;

namespace FacetedWorlds.ThoughtCloud.UnitTest
{
    [TestClass]
    public class ModelTest : SilverlightTest
    {
        private Community _mikesCommunity;
        private Community _russellsCommunity;
        private Identity _mikesIdentity;
        private Identity _russellsIdentity;

        [TestInitialize]
        public void Initialize()
        {
            var sharedCommunication = new MemoryCommunicationStrategy();
            _mikesCommunity = new Community(new MemoryStorageStrategy())
                .AddCommunicationStrategy(sharedCommunication)
                .Register<Model.CorrespondenceModel>()
                .Subscribe(() => _mikesIdentity)
                .Subscribe(() => _mikesIdentity.SharedClouds)
				;
            _russellsCommunity = new Community(new MemoryStorageStrategy())
                .AddCommunicationStrategy(sharedCommunication)
                .Register<Model.CorrespondenceModel>()
                .Subscribe(() => _russellsIdentity)
                .Subscribe(() => _russellsIdentity.SharedClouds)
				;

            _mikesIdentity = _mikesCommunity.AddFact(new Identity("mike"));
            _russellsIdentity = _russellsCommunity.AddFact(new Identity("russell"));
		}

        [TestMethod]
        public void MikeCanShareCloudWithRussell()
        {
            MikeSharesCloudWithRussell();

            Synchronize();

            Assert.AreEqual(1, _russellsIdentity.SharedClouds.Count());
            Assert.AreEqual("mike", _russellsIdentity.SharedClouds.Single().Creator.AnonymousId);
        }

        [TestMethod]
        public void MikeCanShareAThoughtWithRussell()
        {
            Cloud cloud = MikeSharesCloudWithRussell();
            Thought thought = cloud.NewThought();
            cloud.CentralThought = thought;
            thought.Text = "Lunch suggestions";

            Synchronize();

            Thought sharedThought = _russellsIdentity.SharedClouds.Single().CentralThought;
            string sharedText = sharedThought.Text;
            Assert.AreEqual("Lunch suggestions", sharedText);
        }

        [TestMethod]
        public void RussellCanShareAThoughtWithMike()
        {
            Cloud cloud = MikeSharesCloudWithRussell();
            Thought thought = cloud.NewThought();
            cloud.CentralThought = thought;
            thought.Text = "Lunch suggestions";

            Synchronize();

            Cloud sharedCloud = _russellsIdentity.SharedClouds.Single();
            Thought newThought = sharedCloud.NewThought();
            newThought.Text = "Mi Pueblo";
            Thought centralThought = sharedCloud.CentralThought;
            centralThought.LinkTo(newThought);

            Synchronize();

            IEnumerable<Thought> suggestions = thought.Neighbors.Where(n => n != thought);
            Assert.AreEqual(1, suggestions.Count());
            string suggestionText = suggestions.Single().Text;
            Assert.AreEqual("Mi Pueblo", suggestionText);
        }

        [TestMethod]
        public void ConflictsAreDetected()
        {
            Cloud cloud_Mike = MikeSharesCloudWithRussell();
            Thought thought_Mike = cloud_Mike.NewThought();
            cloud_Mike.CentralThought = thought_Mike;
            thought_Mike.Text = "Initial value";

            Synchronize();

            Thought thought_Russell = _russellsIdentity.SharedClouds.Single().CentralThought;
            thought_Mike.Text = "Mike's change";
            thought_Russell.Text = "Russell's change";

            Synchronize();

            Assert.IsTrue(thought_Mike.Text.InConflict);
            Assert.IsTrue(thought_Mike.Text.Candidates.Contains("Mike's change"));
            Assert.IsTrue(thought_Mike.Text.Candidates.Contains("Russell's change"));
            Assert.IsFalse(thought_Mike.Text.Candidates.Contains("Initial value"));

            Assert.IsTrue(thought_Russell.Text.InConflict);
            Assert.IsTrue(thought_Russell.Text.Candidates.Contains("Mike's change"));
            Assert.IsTrue(thought_Russell.Text.Candidates.Contains("Russell's change"));
            Assert.IsFalse(thought_Russell.Text.Candidates.Contains("Initial value"));
        }

        [TestMethod]
        public void ConflictsCanBeResolved()
        {
            Cloud cloud_Mike = MikeSharesCloudWithRussell();
            Thought thought_Mike = cloud_Mike.NewThought();
            cloud_Mike.CentralThought = thought_Mike;
            thought_Mike.Text = "Initial value";

            Synchronize();

            Thought thought_Russell = _russellsIdentity.SharedClouds.Single().CentralThought;
            thought_Mike.Text = "Mike's change";
            thought_Russell.Text = "Russell's change";

            Synchronize();

            thought_Mike.Text = "Mike's resolution";

            Synchronize();

            Assert.IsFalse(thought_Mike.Text.InConflict);
            Assert.AreEqual("Mike's resolution", thought_Mike.Text.Value);

            Assert.IsFalse(thought_Russell.Text.InConflict);
            Assert.AreEqual("Mike's resolution", thought_Russell.Text.Value);
        }

        private void Synchronize()
        {
            while (_mikesCommunity.Synchronize() || _russellsCommunity.Synchronize()) ;
        }

        private Cloud MikeSharesCloudWithRussell()
        {
            Cloud cloud = _mikesIdentity.NewCloud();
            Identity russell = _mikesCommunity.AddFact(new Identity("russell"));
            russell.NewShare(cloud);
            return cloud;
        }
    }
}
