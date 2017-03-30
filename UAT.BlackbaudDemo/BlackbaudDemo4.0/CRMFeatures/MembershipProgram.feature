Feature: Membership Programs
	In order to manage Membership programs
	As a BBCRM User
	I want to create a new Membership Program

Scenario: Create a new Membership Program 
	Given I have logged into the BBCRM home page
	And there is no existing Membership Program called "Test Program"
	When I create a new membership with General options
	| field       | value        |
	| Name		  | Test Program |
	| Kind        | Annual       |
	| Obtain      | Dues         |
	And Level options of
	| field | value  |
	| Name  | Level1 |
	| Price | 149.99 |
	And Benefits options of
	| field            | value                  |
	| MembershipFormat | Membership Card Format |
	| other            |                        |
	And Dues options of
	| field        | value |
	| Installments | Yes   |
	| other        |       |
	And Renewal options of
	| field | value															 |
	| Term  | Varies with the membership start date (based on a rolling year)|
	| other |																 |
	And the Review is Saved
	Then "Test Program" should be listed in Membership Programs
