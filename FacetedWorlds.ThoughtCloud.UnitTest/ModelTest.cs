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
