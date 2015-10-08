Feature: Constituent Search
	In order to avoid duplication of data
	As a BBCRM user
	I want to search existing Constituent records

Scenario: Quick Constituent Search
	Given I have logged into the BBCRM home page
	And I have opened the constituent search dialog
	When I search for
	| field			| value		|
	| LastName		| Hampton	|
	Then The results should contain "Hampton Street Elementary School"

Scenario: Quick Multiple Constituent Search
	Given I have logged into the BBCRM home page
	And I have opened the constituent search dialog
	When I search for
	| field       | value |
	| LastName    | Z     |
	Then The results should contain "Ziegler, Dianne A"
	And The results should contain "Ziegler, Douglas P"
	And The results should contain "Douglas and Dianne Ziegler"

@Browser:Chrome
Scenario: Negative Constituent Search
	Given I have logged into the BBCRM home page
	And I have opened the constituent search dialog
	When I search for
	| field     | value     |
	| LastName  | Lovecraft |
	| FirstName | Howard    |
	Then I should get 0 records

@Browser:Chrome
Scenario: Find all Constituent Search
	Given I have logged into the BBCRM home page
	And I have opened the constituent search dialog
	When I search with no parameters
	Then I should get more than 100 records