Feature: Batch
	In order to test batch functionality 
	As a BBCRM user
	I want to manage batch records

@Ready
Scenario: Batch: Process Donation payment for existing constituent via Enhanced Revenue Batch
	Given I have logged into the BBCRM home page
	And I add individual(s)
	| Last name | First name | Title | Nickname | Information source |
	| Baggins   | Benjamin   | Mr.   | Ben      | Other              |
	When I add a batch with template "Enhanced Revenue Batch" and description "ERB Event Payment"
    | Account system                  | Constituent | Amount  | Date  | Revenue type | Payment method | Application | Designation |
    | System Generated Account System | Baggins     | $100.00 | Today | Payment      | Check          | Donation    | GARDEN      |
	And I prepare the batch for commit
	And I commit the batch
    | Batch template         | Description       |
    | Enhanced Revenue Batch | ERB Event Payment |
	And I search for the transaction
	| Last name | Transaction type | Amount  | Date  |
	| Baggins   | Payment          | $100.00 | Today |
	Then Revenue Transaction Page Transaction Summary for batch payment shows
    | Payment amount | Date  |
	| $100.00        | Today |
	And Payment applications details are correct
    | Application | Amount  |
    | Donation    | $100.00 |

@Ready
Scenario: Batch: Split a pledge across 2 designations via Enhanced Revenue Batch
	Given I have logged into the BBCRM home page
	And I add individual
	| Last name | First name | Title | Nickname | Information source |
	| Wilson    | Rusell     | Mr.    | Rus     | Other		         |
	When I add a batch with template "Enhanced Revenue Batch" and description "ERB"
    | Account system                  | Constituent | Amount  | Date  | Revenue type | Payment method | Designation | Installment frequency | Installment start date | No. installments |
    | System Generated Account System | Wilson      | $500.00 | today | Pledge       | None           | GARDEN      | Monthly               | today                  | 5                |
	And split the designations evenly
	| Designation  |
	| GARDEN       |
	| Library Fund |
	Then the 'Designation' cell value is '<split>' for row 1

@Ready
Scenario: Batch: Add a payment record applied to the event registration using Enhanced Revenue Batch
    Given I have logged into the BBCRM home page
	And I add individual(s)
	| Last name | First name | Title | Nickname | Information source |
	| Bradbury  | Tom        | Mr.    | Tom      | Other              |
	And An Event exists that includes Registration Option
    | Name            | Start date      | Category | Registration type | Registration count | Registration fee |
    | College Reunion | Today +6 months | Formal   | Adult             | 1                  | $100.00          |
	And Constituent "Tom Bradbury" is registered for event named "College Reunion" with "Adult" registration option 
	When I add a batch with template "Enhanced Revenue Batch" and description "ERB Event Payment"
	| Account system                  | Constituent | Amount  | Date  | Revenue type | Payment method | Application     |
	| System Generated Account System | Bradbury    | $100.00 | Today | Payment      | Check          | College Reunion |
	And I prepare the batch for commit
	And I commit the batch
	| Batch template         | Description       |
	| Enhanced Revenue Batch | ERB Event Payment |
	Then Event "College Reunion" displays registrant(s) on Registrations tab
	| Registrant First Name | Registrant Last Name | Balance | Extra |
    | Tom                   | Bradbury             | $0.00   |       |

@Ready
Scenario: Batch: Add a donation payment with multiple designations through a batch
	Given I have logged into the BBCRM home page
	And I add individual
	| Last name | First name | Title | Nickname | Information source |
	| Newton    | Cam        | Mr.   | Cam      | Other              |
	When I add a batch with template "Enhanced Revenue Batch" and description "Cam's donation"
    | Account system                  | Constituent | Amount  | Date  | Revenue type | Payment method | GL post status |
    | System Generated Account System | Newton      | $200.00 | today | Payment      | Cash           | Do not post    |
	And apply the payment to designations
	| Additional applications | Applied amount | Designation  |
	| Donation                | $50.00         | Library Fund |
	| Donation                | $150.00        | GARDEN       |
	And I prepare the batch for commit
	And I commit the batch
	| Batch template         | Description    |
	| Enhanced Revenue Batch | Cam's donation |
	Then the batch commits without errors or exceptions and 1 record processed
	And the revenue record for "Newton" has donations
	| Designation           | Recognition credits |
	| Library Fund          | $50.00              |
	| Botanical Garden Fund | $150.00             |

@Ready
Scenario: Batch: Add event payment via Enhanced revenue batch for registered attendee made by their organisation relationship/employer
    Given I have logged into the BBCRM home page
	And I add individual(s)
	| Last name | First name | Title | Nickname | Information source |
	| Potter    | Tom        | Mr.   | Tom      | Other              |
	And I add organization(s)
	| Name      | Industry   |
	| Blackbaud | Accounting |
	And I add an Organization relationship
	| Constituent | Related organization | Constituent relationship type | Related organization relationship type | Start date | The organization will match contributions for this relationship |
	| Tom Potter  | Blackbaud            | Employee                      | Employer/Organisation                  | today      | false                                                           |
	And An Event exists that includes Registration Option
    | Name            | Start date      | Category | Registration type | Registration count | Registration fee |
    | College Reunion | Today +6 months | Brunch   | Adult             | 1                  | $200.00          |
	And Constituent "Tom Potter" is registered for event named "College Reunion" with "Adult" registration option 
	When I add a batch with template "Enhanced Revenue Batch" and description "ERB Event Payment"
    | Account system                  | Constituent | Amount  | Date  | Revenue type | Payment method |
    | System Generated Account System | Blackbaud   | $200.00 | Today | Payment      | Check          |    
	And I load commitments for constituent "Tom Potter" and apply amount of "$200.00"
	And I prepare the batch for commit
	And I commit the batch
    | Batch template         | Description       |
    | Enhanced Revenue Batch | ERB Event Payment |
	Then Event "College Reunion" displays registrant(s) on Registrations tab
	| Registrant First Name | Registrant Last Name | Balance | Extra |
	| Tom                   | Potter               | $0.00   |       |	 

@Ready
Scenario: Batch: Process payment for existing Pledge commitment on existing constituent via Enhanced Revenue Batch
	Given I have logged into the BBCRM home page
	And I add individual(s)
	| Last name | First name | Title | Nickname | Information source |
	| Jenkins   | Timothy    | Mr.   | Tim      | Other              |
	And A Pledge has been submitted today for the Constituent "Timothy Jenkins" with account system "System Generated Account System"
	| Amount    | Designation | Installments | Date  | Start date | Frequency | Installment amount | Number of Installments | Post Type   |
	| $1,200.00 | GARDEN      | Monthly      | Today | Today      | Monthly   | $100.00            | 12                     | Do not post |
	When I add a batch with template "Enhanced Revenue Batch" and description "ERB Event Payment"
    | Account system                  | Constituent | Amount  | Date  | Revenue type | Payment method | Application |
    | System Generated Account System | Jenkins     | $100.00 | Today | Payment      | Check          | Pledge for  |
	And I prepare the batch for commit
	And I commit the batch
    | Batch template         | Description       |
    | Enhanced Revenue Batch | ERB Event Payment |
	And I search for the transaction
    | Last name | Transaction type | Amount  | Date  |
	| Jenkins   | Payment          | $100.00 | Today |    
	Then Revenue Transaction Page Transaction Summary for batch payment shows
	| Payment amount | Date  | 
	| $100.00        | Today | 
	And Payment applications details are correct
    | Application | Amount  |
    | Pledge      | $100.00 |

@Ready
Scenario: Batch: Process payment for existing Recurring Gift commitment on existing constituent via Enhanced Revenue Batch
	Given I have logged into the BBCRM home page
	And I add individual
	| Last name | First name | Title | Nickname | Information source |
	| Smith   | Frankin      | Mr.   | Frank    | Other              |
	And I add a Recurring gift to constituent "Smith"
    | Amount  | Designations | Date  | Installment frequency | Installment schedule begins | End date (optional) | Payment method |
    | $100.00 | GARDEN       | Today | Monthly               | Today                       |                     | None           |
	When I add a batch with template "Enhanced Revenue Batch" and description "ERB"
    | Account system                  | Constituent | Amount  | Date  | Revenue type | Payment method | Application    |
    | System Generated Account System | Smith       | $100.00 | Today | Payment      | Check          | Recurring gift |
	And I prepare the batch for commit
	And I commit the batch
    | Batch template         | Description |
    | Enhanced Revenue Batch | ERB         |
	And I search for the transaction
    | Last name | Transaction type | Amount  | Date  |
    | Smith     | Payment          | $100.00 | Today |
	Then Revenue Transaction Page Transaction Summary for batch payment shows
	| Payment amount | Date  | 
	| $100.00        | Today | 
	And Payment applications details are correct
	| Application    | Amount  |
	| Recurring gift | $100.00 |

@Ready
Scenario: Batch: Add a pledge with the GL post status "Do not post" via Enhanced Revenue Batch
	Given I have logged into the BBCRM home page
	And I add individual
	| Last name | First name | Title | Nickname | Information source |
	| Vegesna   | Bob        | Mr.   | Bob      | Other              |
	When I add a batch with template "Enhanced Revenue Batch" and description "ERB do not post pledge"
	| Account system                  | Constituent | Amount | Date  | Revenue type | Payment method | Designation | GL post status | Installment frequency | Installment start date |
	| System Generated Account System | Vegesna     | $500   | Today | Pledge       | Credit card    | GARDEN      | Do not post    | Single Installment    | Today +7 days          |
	And I prepare the batch for commit
	And I commit the batch
	| Batch template         | Description            |
	| Enhanced Revenue Batch | ERB do not post pledge |
	And I navigate to pledge
	| Last name | Transaction type | Date  | Amount  |
	| Vegesna   | Pledge           | Today | $500.00 |
	Then the transaction summary shows
	| Constituent | Post status |
	| Bob Vegesna | Do not post |