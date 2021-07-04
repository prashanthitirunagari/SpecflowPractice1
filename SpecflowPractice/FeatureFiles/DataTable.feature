Feature: Login To the application
	In order to access my appliation
	As a user I launch the application
	I want to provide login details and then I login to the applicaiton

Background: 
Given as a user i should launch the web application
	When user click on SignIn button
	


@Sanity
Scenario: login to the application
	When user enter username and password with the details
	| Username                         | Password    |
	| prashanthi.tirunagaris@gmail.com | Training123 |
	| Welcome@abcxyz.com               | Welcomme123 |
	And user click on Submit button
	Then user should login successfully into the application


@Sanity
Scenario Outline: login to the application with different Data
	When user enter <Username> and <Password> with multiple details
	And user click on Submit button
	Then user should login or not based on <ScenarioType>

Examples:
| Username                         | Password    | ScenarioType |
| prashanthi.tirunagaris@gmail.com | Training123 | Valid        |
| Welcome@abcxyz.com               | Welcomme123 | Valid        |
| Welcome@abcxyz.com22             | Welcomme123 | Invalid      |

