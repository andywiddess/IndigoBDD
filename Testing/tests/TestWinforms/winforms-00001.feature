Feature: Test Application Key Features
  Start the Sample Test Application

Background:
	Given the application "test Windows Forms Application" 
		  using executable path "E:\WIP\Sepura\Indigo\TestApplications\WindowsFormsTestApplication\bin\Debug\WindowsFormsTestApplication.exe" 
		  output to "c:\temp" 
		  loading the form "MainWindow"

Scenario: Load the test application
	When I start the application
		And I wait for 2 seconds
	Then I take a screenshot using the file prefix "Screen1"

Scenario: Check the Main Page has three top level menus
	When I select the menu "File" and choose option "Click Me"
	Then I should see the dialog "Click Me"
	   And I take a screenshot using the file prefix "Screen2"

Scenario: Close the test application
	When I stop the application
		And I wait for 2 seconds
	Then I should see application close