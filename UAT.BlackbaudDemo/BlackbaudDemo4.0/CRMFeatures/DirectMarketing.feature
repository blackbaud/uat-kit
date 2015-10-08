Feature: DirectMarketing
	In order to validate Direct Marketing functionality
	As a user of BBCRM
	I want to add, modify, and validate Marketing processes

@DirectMarketing
Scenario: Run receipt process that marks transaction receipted
	Given I have logged into the BBCRM home page
	And constituent exists
	| Last name       | Address type | Country       | Address               | City       | State | ZIP   | Email type | Email address           |
	| Norman Rockwell | Home         | United States | 1990 Daniel Island Dr | Charleston | SC    | 29407 | Email      | testemail@blackbaud.com |
	And I start to add a payment
	| Constituent     | Amount  | Date     |
	| Norman Rockwell | $100.00 | 1/1/2015 |
	And add applications to the payment
	| Application | Applied | Designation           |
	| Donation    | $100.00 | Camp Kids Scholarship |
	And save the the payment
	When I create a receipt process
	| Name            | Output format        | Mark revenue 'Receipted' when process completes |
	| Receipt Process | Email receipt output | true                                            |
	And run receipt process
	| Name            | Output format        |
	| Receipt Process | Email receipt output |
	Then the revenue record for 'Norman Rockwell' is marked as receipted

@DirectMarketing
Scenario: Add Marketing Acknowledgement Template
	Given I have logged into the BBCRM home page
	And marketing acknowledgement template does not exist
	| Name          | Description |
	| Test Template | Test        |
	When I start to add a marketing acknowledgement template
	And set the general tab's fields
	| Name          | Description | Assign letters based on segmentation | Mark letters 'Acknowledged/Receipted' when process completes |
	| Test Template | Test        | true                                 | checked                                                      |
	And set the source code to 'Automation source code layout 1'
	And set universe tab to include all records
	And set the activation tab's fields
	| Activate and export marketing acknowledgement when template processing completes | Appeal      |
	| checked                                                                          | CAMP LETTER |
	And save the template
	Then a marketing acknowledgement template exists
	| Name          | Description |
	| Test Template | Test        |

@DirectMarketing
Scenario: Process Marketing Acknowledgement Effort
	Given I have logged into the BBCRM home page
	And constituent 'James Harden' exists
	And I start to add a payment
	| Constituent  | Amount  | Date     |
	| James Harden | $100.00 | 2/2/2015 |
	And add applications to the payment
	| Application | Applied | Designation           |
	| Donation    | $100.00 | Camp Kids Scholarship |
	And save the the payment
	And unacknowledged revenue query 'Unacknowledged Revenue' exists
	And marketing acknowledgement template does not exist
	| Name                            | Description |
	| Unacknowledged Revenue Template | URT         |
	And segment 'Unacknowledged Revenue Segment' does not exist with activated marketing effort template 'Unacknowledged Revenue Template'
	And a static selection copy 'Static Selection Unacknowledged Revenue' of query 'Unacknowledged Revenue' exists
	And 'Revenue segment' segment exists with selection 'Static Selection Unacknowledged Revenue'
	| Name                           |
	| Unacknowledged Revenue Segment |
	And a mail package exists with
	| Name                                 | Letter                              |
	| Unacknowledged Revenued Mail Package | Automation acknowledgement letter 2 |
	And I start to add a marketing acknowledgement template
	And set the general tab's fields
	| Name                            | Description | Assign letters based on segmentation | Mark letters 'Acknowledged/Receipted' when process completes |
	| Unacknowledged Revenue Template | URT         | true                                 | checked                                                      |
	And set the source code to 'Automation source code layout 1'
	And set universe tab to include all records
	And set the activation tab's fields
	| Activate and export marketing acknowledgement when template processing completes | Appeal      |
	| checked                                                                          | CAMP LETTER |
	And save the template
	When I start to add an acknowledgement rule to the current marketing acknowledgement template
	| Segment                        | Package                              |
	| Unacknowledged Revenue Segment | Unacknowledged Revenued Mail Package |
	And I save the acknowledgement rule accepting the 3 source code changes
	And I run marketing acknowledgement process
	Then the marketing process completes without errors or exceptions
	And the revenue record for 'James Harden' is marked as acknowledged