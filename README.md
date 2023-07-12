#  Ui-Test-Automation Framework
## Code development
* The C# based Specflow BDD test framework
* Page object Model

## Configuration
* Download and Install Visual studio 2022 or latest
* Add specflow to Visual Studio via the "Manage Extensions" menu
* Download the chromedriver for the installed chrome browser version(chrome driver path needs to be updated in appsettings.json) 

## Website used to test
* https://www.automationexercise.com/
* Please sign up for the website above and adjust your login information for "guiTestUserName" and "guiTestUserPassword" in the appsettings.json file.

## Working with Ui-Test-Automation

* Clone this project from Git 
* Build your project and ensure the build is success.
* Refer appsettings.json file to update rge credentials/url to work
* Open Test explorer and Run test
* Results will be stored into your local machine

## Adding new tests/feature file

* Create a new SpecFlow feature by right-clicking on the project name --> Add --> New Item --> Visual C# Items --> SpecFlow Feature File. Name the feature
* Add your steps on the above feature file
* Generate / Create a step defintion for the same
* Implement the Method


## The following should be noted
* Specflow hook (for setup and teardown events on test cases/ whole test run) will take effect on underling test project execution


