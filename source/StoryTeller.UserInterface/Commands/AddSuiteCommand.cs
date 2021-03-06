using System;
using StoryTeller.Domain;
using StoryTeller.UserInterface.Dialogs;

namespace StoryTeller.UserInterface.Commands
{
    public interface IAddSuiteCommand
    {
        void CreateSuite(string suiteName);
    }

    public interface IAddWorkspaceCommand
    {
        void CreateWorkspace(string workspaceName);
    }

    public class AddWorkspaceCommand : ContextualAction<Hierarchy>, IAddWorkspaceCommand
    {
        private readonly IDialogLauncher _launcher;
        private readonly IEventAggregator _events;

        public AddWorkspaceCommand(Hierarchy subject, IDialogLauncher launcher, IEventAggregator events)
            : base(subject, Icon.Plus, "Add Workspace")
        {
            _launcher = launcher;
            _events = events;
        }

        public override bool Enabled
        {
            get { return true; }
        }

        public override void Execute()
        {
            _launcher.LaunchForCommand<IAddWorkspaceCommand>(this);
        }

        public void CreateWorkspace(string workspaceName)
        {
            var workspace = new WorkspaceSuite(workspaceName);
            subject.AddSuite(workspace);

            _events.SendMessage(new SuiteAddedMessage()
            {
                NewSuite = workspace
            });
        }
    }

    public class AddSuiteCommand : ContextualAction<Suite>, IAddSuiteCommand
    {
        private readonly IEventAggregator _events;
        private readonly IDialogLauncher _launcher;

        public AddSuiteCommand(Suite subject, IDialogLauncher launcher, IEventAggregator events)
            : base(subject, Icon.Plus, "Add Suite")
        {
            _launcher = launcher;
            _events = events;
        }

        public override bool Enabled { get { return true; } }

        #region IAddSuiteCommand Members

        public void CreateSuite(string suiteName)
        {
            var suite = new Suite(suiteName);
            subject.AddSuite(suite);

            _events.SendMessage(new SuiteAddedMessage
            {
                NewSuite = suite
            });
        }

        #endregion

        public override void Execute()
        {
            _launcher.LaunchForCommand<IAddSuiteCommand>(this);
        }
    }
}