Feature: AppealMailing
	In order to create a mailing for an appeal
	As a BBCRM user
	I want to add an Appeal mailing with a selection, package, contact rules, processing and output details

@Ready
@AppealMailing
Scenario: Appeal Mailing: Create a new Appeal Mailing and modify the selection to incorporate an exclusion selection
	Given I have logged into the BBCRM home page
	And I add individual(s)
    | Last name | First name | Title | Nickname | Information source |
    | Smith     | Robert     | Mr.   | Bob      | Other              |
    | Smith     | Mark       | Mr.   | Mark     | Other              |
	And Marketing Export Definitions "UAT-TEST Export 1 - Testing Purposes only" exists
	And Mail Package record "UAT-Test Package 1" exists with Export Definition "UAT-TEST Export 1 - Testing Purposes only"
	And Constituent type Selection exists
	And I include "Constituents" record "Name" field with criteria equal to
	| field          | value    |
	| FILTEROPERATOR | Contains |
	| VALUE1         | Smith    |
	And I save Query Designer with the following options 
	| Name      | Description          | Create Selection | Static | Show in Query Designer |
	| UAT-Smith | Ad-hoc Query UAT Tes | true             | true   |                        |
	And Constituent type Selection exists
	And I include "Constituents" record "First name" field with criteria equal to
	| field          | value    |
	| FILTEROPERATOR | Equal to |
	| VALUE1         | Robert   |
	And I save Query Designer with the following options 
	| Name            | Description                     | Create Selection | Static | Show in Query Designer |
	| UAT-EXCL-Robert | Ad-hoc Query UAT exclusion Test | true             |        | true                   |      
	And Appeal "UAT TEST - General Appeal" exists
	And I add an Appeal mailing
	| Name            | Mail date      | Appeal                    | Selection | Package            |
	| UAT Test Mail C | Today +1 month | UAT TEST - General Appeal | UAT-Smith | UAT-Test Package 1 |
    When I edit Appeal mailing "UAT Test Mail C"
	And I edit the Selection  
	And I include Selection record "UAT-EXCL-Robert" field using criteria equal to
	| field          | value    |
	| FILTEROPERATOR | Equal to |
	| COMBOVALUE     | No       |
	And I save the Appeal mailing
	And I Start mailing
	And Records successfully processed is greater than zero
	Then Appeal "UAT TEST - General Appeal" Mailings tab Appeal mailing List shows
	| Name            | Mail date      | Package            | Selection   |
	| UAT Test Mail C | Today +1 month | UAT-Test Package 1 | UAT-Smith   |


	
