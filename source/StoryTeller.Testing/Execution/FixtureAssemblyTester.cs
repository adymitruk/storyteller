using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;
using StoryTeller.Engine;
using StoryTeller.Execution;
using StoryTeller.Samples;
using StoryTeller.Workspace;

namespace StoryTeller.Testing.Execution
{
    [TestFixture]
    public class FixtureAssemblyTester
    {
        [Test]
        public void can_serialize_the_fixture_assembly_class()
        {
            var fa = new FixtureAssembly(null, "StoryTeller.Testing");
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, fa);

            stream.Position = 0;

            var fa2 = (FixtureAssembly)formatter.Deserialize(stream);
        }

        [Test]
        public void pulls_the_current_filter_from_the_project()
        {
            var project = new Project
            {
                FixtureAssembly = typeof (GrammarMarker).Assembly.GetName().Name
            };

            project.WorkspaceFor("1").AddFilter(new FixtureFilter()
            {
                Name = "North", Type = FilterType.Fixture
            });

            project.WorkspaceFor("2").AddFilter(new FixtureFilter()
            {
                Name = "South",
                Type = FilterType.Fixture
            });

            project.SelectWorkspaces(new string[]{"1", "2"});

            var fa = new FixtureAssembly(project);

            fa.Filter.Filters.ShouldHaveTheSameElementsAs(project.CurrentFixtureFilter().Filters);
        }
    }

    [TestFixture]
    public class when_the_system_type_name_is_null_or_empty
    {
        private FixtureAssembly fa;

        [SetUp]
        public void SetUp()
        {
            fa = new FixtureAssembly(null, GetType().Assembly.GetName().Name);
        }

        [Test]
        public void the_system_should_be_nullo_system()
        {
            fa.System.ShouldBeOfType<NulloSystem>();
        }

        [Test]
        public void the_assembly_should_be_the_configured_fixture_assembly()
        {
            fa.Assembly.ShouldEqual(GetType().Assembly);
        }
    }

    [TestFixture]
    public class when_the_system_type_is_specified
    {
        private FixtureAssembly fa;

        [SetUp]
        public void SetUp()
        {
            fa = new FixtureAssembly(typeof (GrammarSystem).AssemblyQualifiedName, null);
        }

        [Test]
        public void the_system_type_should_be_the_type_specified()
        {
            fa.System.ShouldBeOfType<GrammarSystem>();
        }

        [Test]
        public void the_assembly_should_be_the_assembly_containing_the_system_specified()
        {
            fa.Assembly.ShouldEqual(typeof (GrammarSystem).Assembly);
        }
    }
}