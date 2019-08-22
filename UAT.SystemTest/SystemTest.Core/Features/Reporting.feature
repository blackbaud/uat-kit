Feature: Reporting
	As a BBCRM user
	I want to open and use reports 

@Ross
@Ready
Scenario: Constituent profile report returns information
	Given I have logged into the BBCRM home page
	And a constituent record exists
	| Title | Nickname | Information source | Last name | First name |
	| Mr.   | Bobby    | Other              | Case      | Test       |
	When I open and run the "Constituent profile" report for "Case"
	Then the "Constituent Profile" report is presented for "Test Case"