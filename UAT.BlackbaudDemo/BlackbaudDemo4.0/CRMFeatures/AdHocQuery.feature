Feature: AdHocQuery
	In order to validate AdHoc Query functionality
	As a user of BBCRM
	I want to add, modify, and validate AdHoc Queries

@Browser:Chrome
@AdHocQuery
Scenario: Add an Ad-Hoc Query
	Given I have logged into the BBCRM home page
	When I add ad-hoc query type 'Revenue'
	And filter by 'Revenue'
	And add filter field 'Amount'
	| FILTEROPERATOR | VALUE1 |
	| Greater than   | 30000  |
	And filter by 'Revenue\Constituent\Spouse'
	And add filter field 'Gender'
	| FILTEROPERATOR | COMBOVALUE |
	| Not equal to   | Male       |
	And add output fields
	| Path                | Field      |
	| Revenue             | Revenue ID |
	| Revenue\Constituent | Name       |
	And set save options
	| Name              | Description      | Suppress duplicate row | Create a selection? | Create a dynamic selection |
	| AdHocQuery_472636 | some description | checked                | checked             | on                         |
	Then ad-hoc query 'AdHocQuery_472636' is saved

