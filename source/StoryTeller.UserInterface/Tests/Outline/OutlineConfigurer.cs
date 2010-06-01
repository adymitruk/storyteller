﻿using System.Collections.Generic;
using StoryTeller.Domain;
using StoryTeller.Model;

namespace StoryTeller.UserInterface.Tests.Outline
{
    public class OutlineConfigurer : IOutlineConfigurer
    {
        private readonly IOutlineController _controller;

        public OutlineConfigurer(IOutlineController controller)
        {
            _controller = controller;
        }

        public void ConfigureTableColumnSelector(OutlineNode node, Table table, IStep step)
        {
            // no-op
        }

        public void ConfigureRearrangeCommands(OutlineNode node, IPartHolder holder, ITestPart part)
        {

        }

        public void ConfigurePartAdders(OutlineNode node, FixtureGraph fixture, IPartHolder holder)
        {

        }

        public void WriteSentenceText(OutlineNode node, Sentence sentence, IStep step)
        {
            var writer = new SentenceWriter(node, step);
            sentence.Parts.Each(x => x.AcceptVisitor(writer));
        }

        public class SentenceWriter : ISentenceVisitor
        {
            private readonly OutlineNode _node;
            private readonly IStep _step;

            public SentenceWriter(OutlineNode node, IStep step)
            {
                _node = node;
                _step = step;
            }

            public void Label(Label label)
            {
                _node.AddText(label.Text);
            }

            public void Input(TextInput input)
            {
                var key = input.Cell.Key;
                string value = _step.Has(key) ? _step.Get(key) : input.Cell.DefaultValue ?? "{" + key + "}";

                _node.AddItalicizedText(value);
            }
        }
    }
}