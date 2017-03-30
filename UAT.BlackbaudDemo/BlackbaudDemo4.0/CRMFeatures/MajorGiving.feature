Feature: Major Giving Prospects
	In order to validate Major Giving functionality
	As a user of BBCRM
	I want to add, modify, and validate Prospects and Major Giving Plans

@MajorGiving 
Scenario: Add a Prospect Constituent
	Given I have logged into the BBCRM home page
	And constituent 'Constituent_473330' exists
	When I add prospect constituency to 'Constituent_473330'
	|Date from|Date to|
	Then a prospect constituency is added to 'Constituent_473330'
	| Date from | Date to | Description           |
	|           |         | Major giving prospect |

@MajorGiving
Scenario: Add a Documentation Note based Notification to a Constituent
	Given I have logged into the BBCRM home page
	And constituent 'Constituent_473331' exists
	When I add a Note to 'Constituent_473331'
	| Type   | Date       | Title     | Author             | Notes     |
	| Career | 01/01/2015 | Test Note | Constituent_473331 | Test note |
	And add a notification to note 'Test Note'
	| Displays for |
	| All users    |
	Then the notification bar displays the Note 'Test Note'

@MajorGiving
Scenario: Add a Major Giving Plan outline in Major Giving Setup
	Given I have logged into the BBCRM home page
	When I add plan outline "MajorGivingPlan_472635" to major giving setup
	| Objective             | Fundraiser role  | Stage          | Days from start | Contact method |
	| Clearance to Approach | Prospect manager | Identification | 7               |                |
	| Prepare Ask           | Primary manager  | Cultivation    | 20              |                |
	| Explore Inclination   | Primary manager  | Cultivation    | 60              |                |
	| Make Ask              | Primary manager  | Negotiation    | 90              |                |
	Then the plan outline "MajorGivingPlan_472635" is created with "4" steps

#376252
@MajorGiving
Scenario: Add a step with Status as Completed
	Given I have logged into the BBCRM home page
	And prospect 'Prospect_376252' exists
	And major giving plan exists
	| Type                      | Stages | Steps |
	| ERBOpportunityPlanOutline | 3      | 3     |
	And prospect 'Prospect_376252' is associated with major giving plan
	| Plan name         | Plan type    | Start date | Outlines                  |
	| Major giving plan | Major giving | 01/01/2015 | ERBOpportunityPlanOutline |
	When I go to the plan 'Major giving plan' for prospect 'Prospect_376252'
	And add a step
	| Objective     | Stage          | Expected date | Status    | Actual date | Actual start time | Actual end time |
	| 1st Objective | Identification | 01/01/2015    | Completed | 01/01/2015  | 12:00             | 12:00           |
	Then a completed step is saved
	| Status    | Date       | Start time | End time | Objective     | Stage          |
	| Completed | 01/01/2015 | 12:00      | 12:00    | 1st Objective | Identification |