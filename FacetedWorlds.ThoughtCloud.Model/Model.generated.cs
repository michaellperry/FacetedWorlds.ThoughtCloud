using System.Collections.Generic;
using System.Linq;
using UpdateControls.Correspondence;
using UpdateControls.Correspondence.Mementos;
using UpdateControls.Correspondence.Strategy;
using System;
using System.IO;

/**
/ For use with http://graphviz.org/
digraph "FacetedWorlds.ThoughtCloud.Model"
{
    rankdir=BT
    Cloud -> Identity
    CloudCentralThought -> Cloud [color="red"]
    CloudCentralThought -> CloudCentralThought [label="  *"]
    CloudCentralThought -> Thought
    Share -> Identity [color="red"]
    Share -> Cloud
    Thought -> Cloud [color="red"]
    ThoughtText -> Thought
    ThoughtText -> ThoughtText [label="  *"]
    Link -> Thought [label="  *"]
    DisableToastNotification -> Identity
    EnableToastNotification -> DisableToastNotification [label="  *"]
}
**/

namespace FacetedWorlds.ThoughtCloud.Model
{
    public partial class Identity : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Identity newFact = new Identity(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._anonymousId = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Identity fact = (Identity)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._anonymousId);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.ThoughtCloud.Model.Identity", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles

        // Queries
        public static Query QueryIsToastNotificationDisabled = new Query()
            .JoinSuccessors(DisableToastNotification.RoleIdentity, Condition.WhereIsEmpty(DisableToastNotification.QueryIsReenabled)
            )
            ;
        public static Query QuerySharedClouds = new Query()
            .JoinSuccessors(Share.RoleRecipient)
            .JoinPredecessors(Share.RoleCloud)
            ;

        // Predicates

        // Predecessors

        // Fields
        private string _anonymousId;

        // Results
        private Result<DisableToastNotification> _isToastNotificationDisabled;
        private Result<Cloud> _sharedClouds;

        // Business constructor
        public Identity(
            string anonymousId
            )
        {
            InitializeResults();
            _anonymousId = anonymousId;
        }

        // Hydration constructor
        private Identity(FactMemento memento)
        {
            InitializeResults();
        }

        // Result initializer
        private void InitializeResults()
        {
            _isToastNotificationDisabled = new Result<DisableToastNotification>(this, QueryIsToastNotificationDisabled);
            _sharedClouds = new Result<Cloud>(this, QuerySharedClouds);
        }

        // Predecessor access

        // Field access
        public string AnonymousId
        {
            get { return _anonymousId; }
        }

        // Query result access
        public IEnumerable<DisableToastNotification> IsToastNotificationDisabled
        {
            get { return _isToastNotificationDisabled; }
        }
        public IEnumerable<Cloud> SharedClouds
        {
            get { return _sharedClouds; }
        }

        // Mutable property access

    }
    
    public partial class Cloud : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Cloud newFact = new Cloud(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._unique = (Guid)_fieldSerializerByType[typeof(Guid)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Cloud fact = (Cloud)obj;
				_fieldSerializerByType[typeof(Guid)].WriteData(output, fact._unique);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.ThoughtCloud.Model.Cloud", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleCreator = new Role(new RoleMemento(
			_correspondenceFactType,
			"creator",
			new CorrespondenceFactType("FacetedWorlds.ThoughtCloud.Model.Identity", 1),
			false));

        // Queries
        public static Query QueryCentralThought = new Query()
            .JoinSuccessors(CloudCentralThought.RoleCloud, Condition.WhereIsEmpty(CloudCentralThought.QueryIsCurrent)
            )
            ;

        // Predicates

        // Predecessors
        private PredecessorObj<Identity> _creator;

        // Unique
        private Guid _unique;

        // Fields

        // Results
        private Result<CloudCentralThought> _centralThought;

        // Business constructor
        public Cloud(
            Identity creator
            )
        {
            _unique = Guid.NewGuid();
            InitializeResults();
            _creator = new PredecessorObj<Identity>(this, RoleCreator, creator);
        }

        // Hydration constructor
        private Cloud(FactMemento memento)
        {
            InitializeResults();
            _creator = new PredecessorObj<Identity>(this, RoleCreator, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
            _centralThought = new Result<CloudCentralThought>(this, QueryCentralThought);
        }

        // Predecessor access
        public Identity Creator
        {
            get { return _creator.Fact; }
        }

        // Field access
		public Guid Unique { get { return _unique; } }


        // Query result access

        // Mutable property access

        public Disputable<Thought> CentralThought
        {
            get { return _centralThought.Select(fact => fact.Value).AsDisputable(); }
			set
			{
				Community.AddFact(new CloudCentralThought(this, _centralThought, value.Value));
			}
        }
    }
    
    public partial class CloudCentralThought : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				CloudCentralThought newFact = new CloudCentralThought(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				CloudCentralThought fact = (CloudCentralThought)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.ThoughtCloud.Model.CloudCentralThought", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleCloud = new Role(new RoleMemento(
			_correspondenceFactType,
			"cloud",
			new CorrespondenceFactType("FacetedWorlds.ThoughtCloud.Model.Cloud", 1),
			true));
        public static Role RolePrior = new Role(new RoleMemento(
			_correspondenceFactType,
			"prior",
			new CorrespondenceFactType("FacetedWorlds.ThoughtCloud.Model.CloudCentralThought", 1),
			false));
        public static Role RoleValue = new Role(new RoleMemento(
			_correspondenceFactType,
			"value",
			new CorrespondenceFactType("FacetedWorlds.ThoughtCloud.Model.Thought", 1),
			false));

        // Queries
        public static Query QueryIsCurrent = new Query()
            .JoinSuccessors(CloudCentralThought.RolePrior)
            ;

        // Predicates
        public static Condition IsCurrent = Condition.WhereIsEmpty(QueryIsCurrent);

        // Predecessors
        private PredecessorObj<Cloud> _cloud;
        private PredecessorList<CloudCentralThought> _prior;
        private PredecessorObj<Thought> _value;

        // Fields

        // Results

        // Business constructor
        public CloudCentralThought(
            Cloud cloud
            ,IEnumerable<CloudCentralThought> prior
            ,Thought value
            )
        {
            InitializeResults();
            _cloud = new PredecessorObj<Cloud>(this, RoleCloud, cloud);
            _prior = new PredecessorList<CloudCentralThought>(this, RolePrior, prior);
            _value = new PredecessorObj<Thought>(this, RoleValue, value);
        }

        // Hydration constructor
        private CloudCentralThought(FactMemento memento)
        {
            InitializeResults();
            _cloud = new PredecessorObj<Cloud>(this, RoleCloud, memento);
            _prior = new PredecessorList<CloudCentralThought>(this, RolePrior, memento);
            _value = new PredecessorObj<Thought>(this, RoleValue, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Cloud Cloud
        {
            get { return _cloud.Fact; }
        }
        public IEnumerable<CloudCentralThought> Prior
        {
            get { return _prior; }
        }
             public Thought Value
        {
            get { return _value.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    
    public partial class Share : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Share newFact = new Share(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Share fact = (Share)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.ThoughtCloud.Model.Share", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleRecipient = new Role(new RoleMemento(
			_correspondenceFactType,
			"recipient",
			new CorrespondenceFactType("FacetedWorlds.ThoughtCloud.Model.Identity", 1),
			true));
        public static Role RoleCloud = new Role(new RoleMemento(
			_correspondenceFactType,
			"cloud",
			new CorrespondenceFactType("FacetedWorlds.ThoughtCloud.Model.Cloud", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorObj<Identity> _recipient;
        private PredecessorObj<Cloud> _cloud;

        // Fields

        // Results

        // Business constructor
        public Share(
            Identity recipient
            ,Cloud cloud
            )
        {
            InitializeResults();
            _recipient = new PredecessorObj<Identity>(this, RoleRecipient, recipient);
            _cloud = new PredecessorObj<Cloud>(this, RoleCloud, cloud);
        }

        // Hydration constructor
        private Share(FactMemento memento)
        {
            InitializeResults();
            _recipient = new PredecessorObj<Identity>(this, RoleRecipient, memento);
            _cloud = new PredecessorObj<Cloud>(this, RoleCloud, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Identity Recipient
        {
            get { return _recipient.Fact; }
        }
        public Cloud Cloud
        {
            get { return _cloud.Fact; }
        }

        // Field access

        // Query result access

        // Mutable property access

    }
    
    public partial class Thought : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Thought newFact = new Thought(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._unique = (Guid)_fieldSerializerByType[typeof(Guid)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Thought fact = (Thought)obj;
				_fieldSerializerByType[typeof(Guid)].WriteData(output, fact._unique);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.ThoughtCloud.Model.Thought", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleCloud = new Role(new RoleMemento(
			_correspondenceFactType,
			"cloud",
			new CorrespondenceFactType("FacetedWorlds.ThoughtCloud.Model.Cloud", 1),
			true));

        // Queries
        public static Query QueryText = new Query()
            .JoinSuccessors(ThoughtText.RoleThought, Condition.WhereIsEmpty(ThoughtText.QueryIsCurrent)
            )
            ;
        public static Query QueryNeighbors = new Query()
            .JoinSuccessors(Link.RoleThoughts)
            .JoinPredecessors(Link.RoleThoughts)
            ;
        public static Query QueryLinks = new Query()
            .JoinSuccessors(Link.RoleThoughts)
            ;

        // Predicates

        // Predecessors
        private PredecessorObj<Cloud> _cloud;

        // Unique
        private Guid _unique;

        // Fields

        // Results
        private Result<ThoughtText> _text;
        private Result<Thought> _neighbors;
        private Result<Link> _links;

        // Business constructor
        public Thought(
            Cloud cloud
            )
        {
            _unique = Guid.NewGuid();
            InitializeResults();
            _cloud = new PredecessorObj<Cloud>(this, RoleCloud, cloud);
        }

        // Hydration constructor
        private Thought(FactMemento memento)
        {
            InitializeResults();
            _cloud = new PredecessorObj<Cloud>(this, RoleCloud, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
            _text = new Result<ThoughtText>(this, QueryText);
            _neighbors = new Result<Thought>(this, QueryNeighbors);
            _links = new Result<Link>(this, QueryLinks);
        }

        // Predecessor access
        public Cloud Cloud
        {
            get { return _cloud.Fact; }
        }

        // Field access
		public Guid Unique { get { return _unique; } }


        // Query result access
        public IEnumerable<Thought> Neighbors
        {
            get { return _neighbors; }
        }
        public IEnumerable<Link> Links
        {
            get { return _links; }
        }

        // Mutable property access
        public Disputable<string> Text
        {
            get { return _text.Select(fact => fact.Value).AsDisputable(); }
			set
			{
				Community.AddFact(new ThoughtText(this, _text, value.Value));
			}
        }

    }
    
    public partial class ThoughtText : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				ThoughtText newFact = new ThoughtText(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._value = (string)_fieldSerializerByType[typeof(string)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				ThoughtText fact = (ThoughtText)obj;
				_fieldSerializerByType[typeof(string)].WriteData(output, fact._value);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.ThoughtCloud.Model.ThoughtText", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleThought = new Role(new RoleMemento(
			_correspondenceFactType,
			"thought",
			new CorrespondenceFactType("FacetedWorlds.ThoughtCloud.Model.Thought", 1),
			false));
        public static Role RolePrior = new Role(new RoleMemento(
			_correspondenceFactType,
			"prior",
			new CorrespondenceFactType("FacetedWorlds.ThoughtCloud.Model.ThoughtText", 1),
			false));

        // Queries
        public static Query QueryIsCurrent = new Query()
            .JoinSuccessors(ThoughtText.RolePrior)
            ;

        // Predicates
        public static Condition IsCurrent = Condition.WhereIsEmpty(QueryIsCurrent);

        // Predecessors
        private PredecessorObj<Thought> _thought;
        private PredecessorList<ThoughtText> _prior;

        // Fields
        private string _value;

        // Results

        // Business constructor
        public ThoughtText(
            Thought thought
            ,IEnumerable<ThoughtText> prior
            ,string value
            )
        {
            InitializeResults();
            _thought = new PredecessorObj<Thought>(this, RoleThought, thought);
            _prior = new PredecessorList<ThoughtText>(this, RolePrior, prior);
            _value = value;
        }

        // Hydration constructor
        private ThoughtText(FactMemento memento)
        {
            InitializeResults();
            _thought = new PredecessorObj<Thought>(this, RoleThought, memento);
            _prior = new PredecessorList<ThoughtText>(this, RolePrior, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Thought Thought
        {
            get { return _thought.Fact; }
        }
        public IEnumerable<ThoughtText> Prior
        {
            get { return _prior; }
        }
     
        // Field access
        public string Value
        {
            get { return _value; }
        }

        // Query result access

        // Mutable property access

    }
    
    public partial class Link : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				Link newFact = new Link(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				Link fact = (Link)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.ThoughtCloud.Model.Link", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleThoughts = new Role(new RoleMemento(
			_correspondenceFactType,
			"thoughts",
			new CorrespondenceFactType("FacetedWorlds.ThoughtCloud.Model.Thought", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorList<Thought> _thoughts;

        // Fields

        // Results

        // Business constructor
        public Link(
            IEnumerable<Thought> thoughts
            )
        {
            InitializeResults();
            _thoughts = new PredecessorList<Thought>(this, RoleThoughts, thoughts);
        }

        // Hydration constructor
        private Link(FactMemento memento)
        {
            InitializeResults();
            _thoughts = new PredecessorList<Thought>(this, RoleThoughts, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public IEnumerable<Thought> Thoughts
        {
            get { return _thoughts; }
        }
     
        // Field access

        // Query result access

        // Mutable property access

    }
    
    public partial class DisableToastNotification : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				DisableToastNotification newFact = new DisableToastNotification(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
						newFact._unique = (Guid)_fieldSerializerByType[typeof(Guid)].ReadData(output);
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				DisableToastNotification fact = (DisableToastNotification)obj;
				_fieldSerializerByType[typeof(Guid)].WriteData(output, fact._unique);
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.ThoughtCloud.Model.DisableToastNotification", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleIdentity = new Role(new RoleMemento(
			_correspondenceFactType,
			"identity",
			new CorrespondenceFactType("FacetedWorlds.ThoughtCloud.Model.Identity", 1),
			false));

        // Queries
        public static Query QueryIsReenabled = new Query()
            .JoinSuccessors(EnableToastNotification.RoleDisable)
            ;

        // Predicates
        public static Condition IsReenabled = Condition.WhereIsNotEmpty(QueryIsReenabled);

        // Predecessors
        private PredecessorObj<Identity> _identity;

        // Unique
        private Guid _unique;

        // Fields

        // Results

        // Business constructor
        public DisableToastNotification(
            Identity identity
            )
        {
            _unique = Guid.NewGuid();
            InitializeResults();
            _identity = new PredecessorObj<Identity>(this, RoleIdentity, identity);
        }

        // Hydration constructor
        private DisableToastNotification(FactMemento memento)
        {
            InitializeResults();
            _identity = new PredecessorObj<Identity>(this, RoleIdentity, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public Identity Identity
        {
            get { return _identity.Fact; }
        }

        // Field access
		public Guid Unique { get { return _unique; } }


        // Query result access

        // Mutable property access

    }
    
    public partial class EnableToastNotification : CorrespondenceFact
    {
		// Factory
		internal class CorrespondenceFactFactory : ICorrespondenceFactFactory
		{
			private IDictionary<Type, IFieldSerializer> _fieldSerializerByType;

			public CorrespondenceFactFactory(IDictionary<Type, IFieldSerializer> fieldSerializerByType)
			{
				_fieldSerializerByType = fieldSerializerByType;
			}

			public CorrespondenceFact CreateFact(FactMemento memento)
			{
				EnableToastNotification newFact = new EnableToastNotification(memento);

				// Create a memory stream from the memento data.
				using (MemoryStream data = new MemoryStream(memento.Data))
				{
					using (BinaryReader output = new BinaryReader(data))
					{
					}
				}

				return newFact;
			}

			public void WriteFactData(CorrespondenceFact obj, BinaryWriter output)
			{
				EnableToastNotification fact = (EnableToastNotification)obj;
			}
		}

		// Type
		internal static CorrespondenceFactType _correspondenceFactType = new CorrespondenceFactType(
			"FacetedWorlds.ThoughtCloud.Model.EnableToastNotification", 1);

		protected override CorrespondenceFactType GetCorrespondenceFactType()
		{
			return _correspondenceFactType;
		}

        // Roles
        public static Role RoleDisable = new Role(new RoleMemento(
			_correspondenceFactType,
			"disable",
			new CorrespondenceFactType("FacetedWorlds.ThoughtCloud.Model.DisableToastNotification", 1),
			false));

        // Queries

        // Predicates

        // Predecessors
        private PredecessorList<DisableToastNotification> _disable;

        // Fields

        // Results

        // Business constructor
        public EnableToastNotification(
            IEnumerable<DisableToastNotification> disable
            )
        {
            InitializeResults();
            _disable = new PredecessorList<DisableToastNotification>(this, RoleDisable, disable);
        }

        // Hydration constructor
        private EnableToastNotification(FactMemento memento)
        {
            InitializeResults();
            _disable = new PredecessorList<DisableToastNotification>(this, RoleDisable, memento);
        }

        // Result initializer
        private void InitializeResults()
        {
        }

        // Predecessor access
        public IEnumerable<DisableToastNotification> Disable
        {
            get { return _disable; }
        }
     
        // Field access

        // Query result access

        // Mutable property access

    }
    

	public class CorrespondenceModel : ICorrespondenceModel
	{
		public void RegisterAllFactTypes(Community community, IDictionary<Type, IFieldSerializer> fieldSerializerByType)
		{
			community.AddType(
				Identity._correspondenceFactType,
				new Identity.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Identity._correspondenceFactType }));
			community.AddQuery(
				Identity._correspondenceFactType,
				Identity.QueryIsToastNotificationDisabled.QueryDefinition);
			community.AddQuery(
				Identity._correspondenceFactType,
				Identity.QuerySharedClouds.QueryDefinition);
			community.AddType(
				Cloud._correspondenceFactType,
				new Cloud.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Cloud._correspondenceFactType }));
			community.AddQuery(
				Cloud._correspondenceFactType,
				Cloud.QueryCentralThought.QueryDefinition);
			community.AddType(
				CloudCentralThought._correspondenceFactType,
				new CloudCentralThought.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { CloudCentralThought._correspondenceFactType }));
			community.AddQuery(
				CloudCentralThought._correspondenceFactType,
				CloudCentralThought.QueryIsCurrent.QueryDefinition);
			community.AddType(
				Share._correspondenceFactType,
				new Share.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Share._correspondenceFactType }));
			community.AddType(
				Thought._correspondenceFactType,
				new Thought.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Thought._correspondenceFactType }));
			community.AddQuery(
				Thought._correspondenceFactType,
				Thought.QueryText.QueryDefinition);
			community.AddQuery(
				Thought._correspondenceFactType,
				Thought.QueryNeighbors.QueryDefinition);
			community.AddQuery(
				Thought._correspondenceFactType,
				Thought.QueryLinks.QueryDefinition);
			community.AddType(
				ThoughtText._correspondenceFactType,
				new ThoughtText.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { ThoughtText._correspondenceFactType }));
			community.AddQuery(
				ThoughtText._correspondenceFactType,
				ThoughtText.QueryIsCurrent.QueryDefinition);
			community.AddType(
				Link._correspondenceFactType,
				new Link.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { Link._correspondenceFactType }));
			community.AddType(
				DisableToastNotification._correspondenceFactType,
				new DisableToastNotification.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { DisableToastNotification._correspondenceFactType }));
			community.AddQuery(
				DisableToastNotification._correspondenceFactType,
				DisableToastNotification.QueryIsReenabled.QueryDefinition);
			community.AddType(
				EnableToastNotification._correspondenceFactType,
				new EnableToastNotification.CorrespondenceFactFactory(fieldSerializerByType),
				new FactMetadata(new List<CorrespondenceFactType> { EnableToastNotification._correspondenceFactType }));
		}
	}
}
