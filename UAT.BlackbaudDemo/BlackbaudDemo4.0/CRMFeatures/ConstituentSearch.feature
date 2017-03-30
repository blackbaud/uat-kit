Feature: Constituent Search
	In order to avoid duplication of data
	As a BBCRM user
	I want to search existing Constituent records

Scenario: Quick Constituent Search 
	Given I have logged into the BBCRM home page
	And I have opened the constituent search dialog
	When I search for
	| field     | value |
	| LastName  | Bach  |
	| FirstName | Julie |
	Then The results should contain "Bach, Julie M"

Scenario: Quick Multiple Constituent Search
	Given I have logged into the BBCRM home page
	And I have opened the constituent search dialog
	When I search for
	| field       | value |
	| LastName    | B     |
	Then The results should contain "Bach, Julie M"

Scenario: Negative Constituent Search
	Given I have logged into the BBCRM home page
	And I have opened the constituent search dialog
	When I search for
	| field     | value     |
	| LastName  | Lovecraft |
	| FirstName | Howard    |
	Then I should get 0 records

Scenario: Find all Constituent Search
	Given I have logged into the BBCRM home page
	And I have opened the constituent search dialog
	When I search with no parameters
	Then I should get some records
	#Then I should get more than 100 records