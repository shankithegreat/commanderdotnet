using Commander;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CommanderTest
{


    /// <summary>
    ///This is a test class for CommandBarTest and is intended
    ///to contain all CommandBarTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CommandBarTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Title
        ///</summary>
        [TestMethod()]
        public void TitleTest()
        {
            CommandBar_Accessor target = new CommandBar_Accessor();

            string expected = string.Empty;
            target.Title = expected;
            Assert.AreEqual(expected, target.Title);
            Assert.AreEqual(expected + ">", target.cmdLabel.Text);

            expected = "text";
            target.Title = expected;
            Assert.AreEqual(expected, target.Title);
            Assert.AreEqual(expected + ">", target.cmdLabel.Text);

            expected = @"c:\";
            target.Title = expected;
            Assert.AreEqual(expected, target.Title);
            Assert.AreEqual(expected + ">", target.cmdLabel.Text);

        }

        /// <summary>
        ///A test for Text
        ///</summary>
        [TestMethod()]
        public void TextTest()
        {
            CommandBar_Accessor target = new CommandBar_Accessor();

            string expected = string.Empty;
            target.Text = expected;
            Assert.AreEqual(expected, target.Text);
            Assert.AreEqual(expected, target.cmdComboBox.Text);

            expected = "item1";
            target.Text = expected;
            Assert.AreEqual(expected, target.Text);
            Assert.AreEqual(expected, target.cmdComboBox.Text);

            expected = "item2";
            target.Text = expected;
            Assert.AreEqual(expected, target.Text);
            Assert.AreEqual(expected, target.cmdComboBox.Text);
        }

        /// <summary>
        ///A test for Lines
        ///</summary>
        [TestMethod()]
        public void LinesTest()
        {
            CommandBar_Accessor target = new CommandBar_Accessor();

            target.Lines = "item1\r\nitem2";
            Assert.AreEqual("item1\r\nitem2", target.Lines);
            Assert.AreEqual(2, target.cmdComboBox.Items.Count);
            Assert.AreEqual("item1", target.cmdComboBox.Items[0]);
            Assert.AreEqual("item2", target.cmdComboBox.Items[1]);

            target.Lines = "item1\r\nitem2\r\n";
            Assert.AreEqual("item1\r\nitem2", target.Lines);
            Assert.AreEqual(2, target.cmdComboBox.Items.Count);
            Assert.AreEqual("item1", target.cmdComboBox.Items[0]);
            Assert.AreEqual("item2", target.cmdComboBox.Items[1]);

            target.Lines = "item1\r\nitem2\r\n\r\n";
            Assert.AreEqual("item1\r\nitem2", target.Lines);
            Assert.AreEqual(2, target.cmdComboBox.Items.Count);
            Assert.AreEqual("item1", target.cmdComboBox.Items[0]);
            Assert.AreEqual("item2", target.cmdComboBox.Items[1]);

            target.Lines = string.Empty;
            Assert.AreEqual(0, target.cmdComboBox.Items.Count);

            target.Lines = "item1\r\nitem2\r\nitem3\r\n";
            Assert.AreEqual("item1\r\nitem2\r\nitem3", target.Lines);
            Assert.AreEqual(3, target.cmdComboBox.Items.Count);
            Assert.AreEqual("item1", target.cmdComboBox.Items[0]);
            Assert.AreEqual("item2", target.cmdComboBox.Items[1]);
            Assert.AreEqual("item3", target.cmdComboBox.Items[2]);
        }

        /// <summary>
        ///A test for StoryCurrentText
        ///</summary>
        [TestMethod()]
        public void StoryCurrentTextTest()
        {
            CommandBar_Accessor target = new CommandBar_Accessor();

            string expected1 = "cd c:";
            target.Text = expected1;
            target.StoryCurrentText();
            Assert.AreEqual(1, target.cmdComboBox.Items.Count);
            Assert.AreEqual(expected1, target.cmdComboBox.Items[0]);

            string expected2 = "cd d:";
            target.Text = expected2;
            target.StoryCurrentText();
            Assert.AreEqual(2, target.cmdComboBox.Items.Count);
            Assert.AreEqual(expected2, target.cmdComboBox.Items[0]);
            Assert.AreEqual(expected1, target.cmdComboBox.Items[1]);

            target.Text = expected1;
            target.StoryCurrentText();
            Assert.AreEqual(2, target.cmdComboBox.Items.Count);
            Assert.AreEqual(expected1, target.cmdComboBox.Items[0]);
            Assert.AreEqual(expected2, target.cmdComboBox.Items[1]);

            string expected3 = "cd e:";
            target.Text = expected3;
            target.StoryCurrentText();
            Assert.AreEqual(3, target.cmdComboBox.Items.Count);
            Assert.AreEqual(expected3, target.cmdComboBox.Items[0]);
            Assert.AreEqual(expected1, target.cmdComboBox.Items[1]);
            Assert.AreEqual(expected2, target.cmdComboBox.Items[2]);

        }

        /// <summary>
        ///A test for OnLinesChanged
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Commander.exe")]
        public void OnLinesChangedTest()
        {
            CommandBar_Accessor target = new CommandBar_Accessor();

            bool actual = false;
            target.add_LinesChanged(delegate(object sender, EventArgs args) { actual = true; });
            target.OnLinesChanged(EventArgs.Empty);
            Assert.IsTrue(actual);

            actual = false;
            target.Lines = "item1\r\nitem2";
            Assert.IsTrue(actual);

            actual = false;
            target.Lines = "item1\r\nitem2";
            Assert.IsFalse(actual);
        }
    }
}
