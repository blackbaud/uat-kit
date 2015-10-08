Feature: Constituents
	In order to validate Constituent functionality
	As a user of BBCRM
	I want to add, search, and validate constituent values

@Constituents
Scenario: Add a Constituent
	Given I have logged into the BBCRM home page
	When I add constituent "Constituent_471048"
	Then constituent "Constituent_471048" is created

Scenario: Edit a Constituent in a batch
	Given I have logged into the BBCRM home page
	And constituent 'Philip Rivers' exists
	When I start to add a batch with template "Enhanced Revenue Batch" and description "Edit Philip"
	| Account system                  | Constituent   | Amount  | Date     | Revenue type | Payment method | Designation           | GL post status |
	| System Generated Account System | Philip Rivers | $100.00 | 1/1/2015 | Pledge       | None           | Camp Kids Scholarship | Do not post    |
	And edit the selected constituent
	| Title   | Birth date | State |
	| Admiral | 12/25/1990 | SC    |
	And save the batch and commit it
	| Batch template         | Description |
	| Enhanced Revenue Batch | Edit Philip |
	Then the batch commits without errors or exceptions and 1 record processed
	And constituent 'Philip Rivers' has the title 'Admiral', birth date '12/25/1990' and state address 'SC'