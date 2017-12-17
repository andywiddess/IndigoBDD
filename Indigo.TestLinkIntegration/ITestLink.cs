using CookComputing.XmlRpc;

namespace Indigo.TestLinkIntegration
{ 
    /// <summary>
    /// the interface mapping required for the XmlRpc api of testlink. 
    /// This interface is used by the TestLink class. 
    /// </summary>
    [XmlRpcUrl("")]
    public interface ITestLink 
        : IXmlRpcProxy
    {
        #region Builds
        /// <summary>
        /// Creates the build.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="buildname">The buildname.</param>
        /// <param name="buildnotes">The buildnotes.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.createBuild", StructParams=true)]
        object[] createBuild(string devKey, int testplanid, string buildname, string buildnotes);

        /// <summary>
        /// Gets the builds for test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getBuildsForTestPlan", StructParams = true)]
        object getBuildsForTestPlan(string devKey, int testplanid);
        #endregion

        #region TestProject
        /// <summary>
        /// Gets the projects.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getProjects", StructParams = true)]
        object getProjects(string devKey);

        /// <summary>
        /// Gets the name of the test project by.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testprojectname">The testprojectname.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestProjectByName", StructParams = true)]
        object getTestProjectByName(string devKey, string testprojectname);

        /// <summary>
        /// Creates the test project.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testprojectname">The testprojectname.</param>
        /// <param name="testcaseprefix">The testcaseprefix.</param>
        /// <param name="notes">The notes.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.createTestProject", StructParams = true)]
        object createTestProject(string devKey, string testprojectname, string testcaseprefix, string notes = "");

        /// <summary>
        /// Uploads the test project attachment.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testprojectid">The testprojectid.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="content">The content.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.uploadTestProjectAttachment", StructParams = true)]
        object uploadTestProjectAttachment(string devKey, int testprojectid, string filename, string fileType, string content, string title, string description);
        #endregion

        #region TestCase
        /// <summary>
        /// Creates the test case.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="authorlogin">The authorlogin.</param>
        /// <param name="testsuiteid">The testsuiteid.</param>
        /// <param name="testcasename">The testcasename.</param>
        /// <param name="testprojectid">The testprojectid.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="steps">The steps.</param>
        /// <param name="keywords">The keywords.</param>
        /// <param name="order">The order.</param>
        /// <param name="checkduplicatedname">The checkduplicatedname.</param>
        /// <param name="actiononduplicatedname">The actiononduplicatedname.</param>
        /// <param name="executiontype">The executiontype.</param>
        /// <param name="importance">The importance.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.createTestCase", StructParams = true)]
        object createTestCase(string devKey, string authorlogin, int testsuiteid, string testcasename, int testprojectid,
            string summary, TestStep[] steps,  string keywords,
            int order, int checkduplicatedname, string actiononduplicatedname, int executiontype, int importance);

        /// <summary>
        /// Adds the test case to test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testprojectid">The testprojectid.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="testcaseexternalid">The testcaseexternalid.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.addTestCaseToTestPlan", StructParams = true)]
        object addTestCaseToTestPlan(string devKey, int testprojectid, int testplanid, string testcaseexternalid, int version);

        /// <summary>
        /// Adds the test case to test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testprojectid">The testprojectid.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="testcaseexternalid">The testcaseexternalid.</param>
        /// <param name="version">The version.</param>
        /// <param name="platformid">The platformid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.addTestCaseToTestPlan", StructParams = true)]
        object addTestCaseToTestPlan(string devKey, int testprojectid, int testplanid, string testcaseexternalid, int version, int platformid);

        /// <summary>
        /// Adds the test case to test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testprojectid">The testprojectid.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="testcaseexternalid">The testcaseexternalid.</param>
        /// <param name="version">The version.</param>
        /// <param name="platformid">The platformid.</param>
        /// <param name="executionorder">The executionorder.</param>
        /// <param name="urgency">The urgency.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.addTestCaseToTestPlan", StructParams = true)]
        object addTestCaseToTestPlan(string devKey, int testprojectid, int testplanid, string testcaseexternalid, int version, int platformid, int executionorder, int urgency);

        /// <summary>
        /// Gets the test case attachments.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCaseAttachments", StructParams = true)]
        object getTestCaseAttachments(string devKey, int testcaseid);

        /// <summary>
        /// Gets the name of the test case identifier by.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testcasename">The testcasename.</param>
        /// <param name="testsuitename">The testsuitename.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCaseIDByName", StructParams = true)]
        object getTestCaseIDByName(string devKey, string testcasename, string testsuitename);

        /// <summary>
        /// Gets the name of the test case identifier by.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testcasename">The testcasename.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCaseIDByName", StructParams = true)]
        object getTestCaseIDByName(string devKey, string testcasename);

        /// <summary>
        /// get test case specification using external or internal id. returns last version
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCase", StructParams = true)]
        object getTestCase(string devKey, int testcaseid);

        /// <summary>
        /// Gets the test case.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCase", StructParams = true)]
        object getTestCase(string devKey, int testcaseid, int version);

        /// <summary>
        /// Gets the test cases for test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCasesForTestPlan", StructParams = true)]
        object getTestCasesForTestPlan(string devKey, int testplanid);

        /// <summary>
        /// Gets the test cases for test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCasesForTestPlan", StructParams = true)]
        object getTestCasesForTestPlan(string devKey, int testplanid, int testcaseid);

        /// <summary>
        /// Gets the test cases for test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <param name="buildid">The buildid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCasesForTestPlan", StructParams = true)]
        object getTestCasesForTestPlan(string devKey, int testplanid, int testcaseid, int buildid);

        /// <summary>
        /// Gets the test cases for test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <param name="buildid">The buildid.</param>
        /// <param name="keywordid">The keywordid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCasesForTestPlan", StructParams = true)]
        object getTestCasesForTestPlan(string devKey, int testplanid, int testcaseid, int buildid, int keywordid);

        /// <summary>
        /// Gets the test cases for test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <param name="buildid">The buildid.</param>
        /// <param name="keywordid">The keywordid.</param>
        /// <param name="executed">if set to <c>true</c> [executed].</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCasesForTestPlan", StructParams = true)]
        object getTestCasesForTestPlan(string devKey, int testplanid, int testcaseid, int buildid, int keywordid, bool executed);

        /// <summary>
        /// Gets the test cases for test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <param name="buildid">The buildid.</param>
        /// <param name="keywordid">The keywordid.</param>
        /// <param name="executed">if set to <c>true</c> [executed].</param>
        /// <param name="assignedTo">The assigned to.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCasesForTestPlan", StructParams = true)]
        object getTestCasesForTestPlan(string devKey, int testplanid, int testcaseid, int buildid, int keywordid, bool executed, int assignedTo);

        /// <summary>
        /// Gets the test cases for test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <param name="buildid">The buildid.</param>
        /// <param name="keywordid">The keywordid.</param>
        /// <param name="executed">if set to <c>true</c> [executed].</param>
        /// <param name="assignedTo">The assigned to.</param>
        /// <param name="executedstatus">The executedstatus.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCasesForTestPlan", StructParams = true)]
        object getTestCasesForTestPlan(string devKey, int testplanid, int testcaseid, int buildid, int keywordid, bool executed, int assignedTo, string executedstatus);

        /// <summary>
        /// Gets the test cases for test suite.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testsuiteid">The testsuiteid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCasesForTestSuite", StructParams = true)]
        object getTestCasesForTestSuite(string devKey, int testsuiteid);

        /// <summary>
        /// Gets the test cases for test suite.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testsuiteid">The testsuiteid.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCasesForTestSuite", StructParams = true)]
        object getTestCasesForTestSuite(string devKey, int testsuiteid, bool deep);

        /// <summary>
        /// Gets the test cases for test suite.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testsuiteid">The testsuiteid.</param>
        /// <param name="deep">if set to <c>true</c> [deep].</param>
        /// <param name="details">The details.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestCasesForTestSuite", StructParams = true)]
        object getTestCasesForTestSuite(string devKey, int testsuiteid, bool deep, string details);

        /// <summary>
        /// Uploads the test case attachment.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="filetype">The filetype.</param>
        /// <param name="content">The content.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.uploadTestCaseAttachment", StructParams = true)]
        object uploadTestCaseAttachment(string devKey, int testcaseid, string filename, string filetype, string content, string title, string description);
        #endregion

        #region TestSuite
        /// <summary>
        /// Gets the test suite by identifier.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testsuiteid">The testsuiteid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestSuiteByID", StructParams = true)]
        object getTestSuiteByID(string devKey, int testsuiteid);

        /// <summary>
        /// Gets the test suites for test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestSuitesForTestPlan", StructParams = true)]
        object getTestSuitesForTestPlan(string devKey, int testplanid);

        /// <summary>
        /// Gets the first level test suites for test project.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testprojectid">The testprojectid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getFirstLevelTestSuitesForTestProject", StructParams = true)]
        object[] getFirstLevelTestSuitesForTestProject(string devKey, int testprojectid);

        /// <summary>
        /// Gets the test suites for test suite.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testsuiteid">The testsuiteid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestSuitesForTestSuite", StructParams = true)]
        object getTestSuitesForTestSuite(string devKey, int testsuiteid);

        /// <summary>
        /// Creates the test suite.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testprojectid">The testprojectid.</param>
        /// <param name="testsuitename">The testsuitename.</param>
        /// <param name="details">The details.</param>
        /// <param name="parentid">The parentid.</param>
        /// <param name="order">The order.</param>
        /// <param name="checkduplicatedname">if set to <c>true</c> [checkduplicatedname].</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.createTestSuite", StructParams = true)]
        object[] createTestSuite(string devKey, int testprojectid, string testsuitename, string details, int parentid, int order, bool checkduplicatedname);

        /// <summary>
        /// Creates the test suite.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testprojectid">The testprojectid.</param>
        /// <param name="testsuitename">The testsuitename.</param>
        /// <param name="details">The details.</param>
        /// <param name="order">The order.</param>
        /// <param name="checkduplicatedname">if set to <c>true</c> [checkduplicatedname].</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.createTestSuite", StructParams = true)]
        object[] createTestSuite(string devKey, int testprojectid, string testsuitename, string details, int order, bool checkduplicatedname);

        /// <summary>
        /// Uploads the test suite attachment.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testsuiteid">The testsuiteid.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="content">The content.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.uploadTestSuiteAttachment", StructParams = true)]
        object uploadTestSuiteAttachment(string devKey, int testsuiteid, string filename, string fileType, string content, string title, string description);
        #endregion

        #region execution
        /// <summary>
        /// Gets the last execution result.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getLastExecutionResult", StructParams = true)]
        object[] getLastExecutionResult(string devKey, int testplanid, int testcaseid);

        /// <summary>
        /// Reports the tc result.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="status">The status.</param>
        /// <param name="platformid">The platformid.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <param name="notes">The notes.</param>
        /// <param name="guess">if set to <c>true</c> [guess].</param>
        /// <param name="bugid">The bugid.</param>
        /// <param name="buildid">The buildid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.reportTCResult", StructParams = true)]
        object reportTCResult(string devKey, int testcaseid, int testplanid, string status, int platformid, bool overwrite, string notes, bool guess, int bugid, int buildid);

        /// <summary>
        /// Reports the tc result.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="status">The status.</param>
        /// <param name="platformname">The platformname.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <param name="notes">The notes.</param>
        /// <param name="guess">if set to <c>true</c> [guess].</param>
        /// <param name="bugid">The bugid.</param>
        /// <param name="buildid">The buildid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.reportTCResult", StructParams = true)]
        object reportTCResult(string devKey, int testcaseid, int testplanid, string status, string platformname, bool overwrite, string notes, bool guess, int bugid, int buildid);

        /// <summary>
        /// Reports the tc result.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="status">The status.</param>
        /// <param name="platformid">The platformid.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <param name="notes">The notes.</param>
        /// <param name="guess">if set to <c>true</c> [guess].</param>
        /// <param name="bugid">The bugid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.reportTCResult", StructParams = true)]
        object reportTCResult(string devKey, int testcaseid, int testplanid, string status, int platformid, bool overwrite, string notes, bool guess, int bugid);

        /// <summary>
        /// Reports the tc result.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="status">The status.</param>
        /// <param name="platformname">The platformname.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <param name="notes">The notes.</param>
        /// <param name="guess">if set to <c>true</c> [guess].</param>
        /// <param name="bugid">The bugid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.reportTCResult", StructParams = true)]
        object reportTCResult(string devKey, int testcaseid, int testplanid, string status, string platformname, bool overwrite, string notes, bool guess, int bugid);

        /// <summary>
        /// Reports the tc result.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="status">The status.</param>
        /// <param name="platformid">The platformid.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <param name="notes">The notes.</param>
        /// <param name="guess">if set to <c>true</c> [guess].</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.reportTCResult", StructParams = true)]
        object reportTCResult(string devKey, int testcaseid, int testplanid, string status, int platformid, bool overwrite, string notes, bool guess);

        /// <summary>
        /// Reports the tc result.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testcaseid">The testcaseid.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <param name="status">The status.</param>
        /// <param name="platformname">The platformname.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <param name="notes">The notes.</param>
        /// <param name="guess">if set to <c>true</c> [guess].</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.reportTCResult", StructParams = true)]
        object reportTCResult(string devKey, int testcaseid, int testplanid, string status, string platformname, bool overwrite, string notes, bool guess);

        /// <summary>
        /// delete an execution
        /// </summary>
        /// <param name="devKey"></param>
        /// <param name="executionid"></param>
        /// <returns> mixed $resultInfo 
	    /// 				[status]	=> true/false of success
	    /// 				[id]		  => result id or error code
	    /// 				[message]	=> optional message for error message string</returns>
        [XmlRpcMethod("tl.deleteExecution", StructParams = true)]
        object deleteExecution(string devKey, int executionid);

        /// <summary>
        /// Uploads the execution attachment.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="executionid">The executionid.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="fileType">Type of the file.</param>
        /// <param name="content">The content.</param>
        /// <param name="title">The title.</param>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.uploadExecutionAttachment", StructParams = true)]
        object uploadExecutionAttachment(string devKey, int  executionid, string filename, string fileType, string content, string title, string description);

        #endregion

        #region Testplan
        /// <summary>
        /// Gets the test plan platforms.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestPlanPlatforms", StructParams = true)]
        object getTestPlanPlatforms(string devKey, int testplanid);

        /// <summary>
        /// Creates the test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanname">The testplanname.</param>
        /// <param name="testprojectname">The testprojectname.</param>
        /// <param name="notes">The notes.</param>
        /// <param name="active">The active.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.createTestPlan", StructParams = true)]

        object[] createTestPlan(string devKey, string testplanname, string testprojectname, string notes, string active);// can't do parameter called 'public' as it collides with .net

        /// <summary>
        /// Gets the project test plans.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testprojectid">The testprojectid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getProjectTestPlans", StructParams = true)]

        object[] getProjectTestPlans(string devKey, int testprojectid);

        /// <summary>
        /// Gets the latest build for test plan.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testplanid">The testplanid.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getLatestBuildForTestPlan", StructParams = true)]

        object getLatestBuildForTestPlan(string devKey, int testplanid);

        /// <summary>
        /// Gets the name of the test plan by.
        /// </summary>
        /// <param name="devKey">The dev key.</param>
        /// <param name="testprojectname">The testprojectname.</param>
        /// <param name="testplanname">The testplanname.</param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getTestPlanByName", StructParams = true)]
        object[] getTestPlanByName(string devKey, string testprojectname, string testplanname);

        /// <summary>
        /// Gets the summarized results grouped by platform
        /// </summary>
        /// <param name="devKey"></param>
        /// <param name="testplanid"></param>
        /// <returns>map where every element has:
        /// 	 *
        /// 	 *	'type' => 'platform'
        /// 	 *	'total_tc => ZZ
        /// 	 *	'details' => array ( 'passed' => array( 'qty' => X)
        /// 	 *	                     'failed' => array( 'qty' => Y)
        /// 	 *	                     'blocked' => array( 'qty' => U)
        /// 	 *                       ....)</returns>
        [XmlRpcMethod("tl.getTotalsForTestPlan", StructParams = true)]
        object getTotalsForTestPlan(string devKey, int testplanid);
        #endregion
 
        #region other
        /// <summary>
        /// simple Ping.
        /// </summary>
        /// <returns></returns>
        [XmlRpcMethod("tl.sayHello")]
        string sayHello();

        /// <summary>
        /// checks user exists
        /// </summary>
        /// <param name="devKey"></param>
        /// <param name="user"></param>
        /// <returns>true if everything OK, otherwise error structure</returns>
        [XmlRpcMethod("tl.doesUserExist", StructParams = true)]
        object doesUserExist(string devKey, string user);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="devKey"></param>
        /// <returns>true if everything OK, otherwise error structure</returns>
        [XmlRpcMethod("tl.checkDevKey", StructParams = true)]
        object checkDevKey(string devKey);


        /// <summary>
        /// Abouts this instance.
        /// </summary>
        /// <returns></returns>
        [XmlRpcMethod("tl.about")]
        string about();

        /// <summary>
        /// Gets full path from the given node till the top using nodes_hierarchy_table
        /// </summary>
        /// <param name="devKey"></param>
        /// <param name="nodeID"></param>
        /// <returns></returns>
        [XmlRpcMethod("tl.getFullPath", StructParams = true)]
        object getFullPath(string devKey, int nodeID);
        #endregion

    }
}
