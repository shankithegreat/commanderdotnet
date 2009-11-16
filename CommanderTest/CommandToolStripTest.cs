using Commander;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace CommanderTest
{
    
    
    /// <summary>
    ///This is a test class for CommandToolStripTest and is intended
    ///to contain all CommandToolStripTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CommandToolStripTest
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
        ///A test for CommandButtons
        ///</summary>
        [TestMethod()]
        public void CommandButtonsTest()
        {
            CommandToolStrip target = new CommandToolStrip();

            target.CommandButtons = string.Empty;
            string actual = target.CommandButtons;
            Assert.AreEqual(string.Empty, actual);

            target.CommandButtons = null;
            actual = target.CommandButtons;
            Assert.AreEqual(string.Empty, actual);            

            target.CommandButtons = @"C:\Windows";
            actual = target.CommandButtons;
            Assert.AreEqual(@"C:\Windows", actual);

            target.CommandButtons = @"SDF:";
            actual = target.CommandButtons;
            Assert.AreEqual(@"SDF:", actual);

            target.CommandButtons = @"C:\Windows?";
            actual = target.CommandButtons;
            Assert.AreEqual(@"C:\Windows", actual);

            target.CommandButtons = @"C:\Windows?C:\Program Files";
            actual = target.CommandButtons;
            Assert.AreEqual(@"C:\Windows?C:\Program Files", actual);
        }
    }
}
