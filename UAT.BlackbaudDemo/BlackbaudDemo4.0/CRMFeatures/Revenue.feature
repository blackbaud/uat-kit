Feature: Revenue
	In order to validate Revenue functionality
	As a user of BBCRM
	I want to add, modify, and validate Pledges and Batches

@Revenue
Scenario: Add a pledge
	Given I have logged into the BBCRM home page
	And constituent 'Paul Simon' exists
	When I add a pledge
	| Constituent | Amount  | Designations          | Date     | Frequency | Starting on | No. installments | Pledge is anonymous |
	| Paul Simon  | $300.00 | Camp Kids Scholarship | 1/1/2015 | Monthly   | 1/1/2020    | 3                | checked             |
	Then a pledge exists for constituent "Paul Simon" with amount "$300.00"

@Revenue
Scenario: Add a pledge with multiple designations
	Given I have logged into the BBCRM home page
	And constituent 'Art Garfunkel' exists
	And designation exists 'Building Fund'
	And designation exists 'Library Books'
	When I start to add a pledge
	| Constituent   | Amount  |
	| Art Garfunkel | $300.00 |
	And split the pledge designations evenly
	| Designation   | Amount  |
	| Building Fund | $300.00 |
	| Library Books | $0.00   |
	And save the pledge
	Then a pledge exists with designations
	| Designation   | Amount  | Balance |
	| Building Fund | $150.00 | $150.00 |
	| Library Books | $150.00 | $150.00 |

@Revenue
Scenario: Add a pledge with multiple designations through a batch
	Given I have logged into the BBCRM home page
	And constituent 'Tom Brady' exists
	When I start to add a batch with template "Enhanced Revenue Batch" and description "Tom's pledge"
	| Account system                  | Constituent | Amount  | Date     | Revenue type | Payment method | GL post status |
	| System Generated Account System | Tom Brady   | $200.00 | 1/1/2015 | Pledge       | None           | Do not post    |
	And split the designations
	| Designation   | Amount  |
	| Library Books | $99.00  |
	| Building Fund | $101.00 |
	And save the batch and commit it
	| Batch template         | Description  |
	| Enhanced Revenue Batch | Tom's pledge |
	Then the batch commits without errors or exceptions and 1 record processed
	And the revenue record for "Tom Brady" has designations
	| Designation   | Amount  |
	| Library Books | $99.00  |
	| Building Fund | $101.00 |

@Revenue
Scenario: Add a recurring gift with multiple designations through a batch
	Given I have logged into the BBCRM home page
	And constituent 'Joe Flacco' exists
	When I start to add a batch with template "Enhanced Revenue Batch" and description "Joe's recurring gift"
    | Constituent | Amount  | Date     | Revenue type   | Payment method |
    | Joe Flacco  | $100.00 | 1/1/2015 | Recurring gift | None           |
	And split the designations
	| Designation   | Amount |
	| Library Books | $75.00 |
	| Building Fund | $25.00 |
	And save the batch and commit it
	| Batch template         | Description          |
	| Enhanced Revenue Batch | Joe's recurring gift |
	Then the batch commits without errors or exceptions and 1 record processed
	And the recurring gift revenue record for "Joe Flacco" has designations
	| Designation   | Amount |
	| Library Books | $75.00 |
	| Building Fund | $25.00 |

@Revenue
Scenario: Add an enhanced revenue batch
	Given I have logged into the BBCRM home page
	And constituent 'Eli Manning' exists
	When I add a batch with template "Enhanced Revenue Batch" and description "ERB"
	| Account system                  | Constituent | Amount  | Date     | Revenue type | Payment method | Designation   | Installment frequency | Installment start date | No. installments |
	| System Generated Account System | Eli Manning | $200.00 | 1/1/2015 | Pledge       | None           | Library Fund  | Monthly               | 1/1/2020               | 3                |
	| System Generated Account System | Eli Manning | $200.00 | 1/1/2015 | Pledge       | None           | Library Books | Monthly               | 1/1/2020               | 3                |
	Then a batch exists
	| Batch template         | Description |
	| Enhanced Revenue Batch | ERB         |

@Revenue
Scenario: Split a designation in an enhanced revenue batch
	Given I have logged into the BBCRM home page
	And constituent 'Rusell Wilson' exists
	When I start to add a batch with template "Enhanced Revenue Batch" and description "ERB"
	| Account system                  | Constituent   | Amount  | Date     | Revenue type | Payment method | Designation   | Installment frequency | Installment start date | No. installments |
	| System Generated Account System | Rusell Wilson | $500.00 | 1/1/2015 | Pledge       | None           | Library Books | Monthly               | 1/1/2020               | 5                |
	And split the designations evenly
	| Designation   |
	| Library Books |
	| Building Fund |
	Then the 'Designation' cell value is '<split>' for row 1

@Revenue
Scenario: Commit an enhanced revenue batch
	Given I have logged into the BBCRM home page
	And constituent 'Drew Brees' exists
	And an "Enhanced Revenue Batch" with description "TO COMMIT" exists
	| Account system                  | Constituent | Amount  | Date     | Revenue type | Payment method | Designation  | Installment frequency | Installment start date | No. installments | GL post status |
	| System Generated Account System | Drew Brees  | $500.00 | 1/1/2015 | Pledge       | None           | Library Fund | Monthly               | 1/1/2020               | 5                | Do not post    |
	When I commit the batch
	| Batch template         | Description |
	| Enhanced Revenue Batch | TO COMMIT   |
	Then the batch commits without errors or exceptions and 1 record processed

@Revenue
Scenario: Add a pledge with a pledge subtype though a batch
	Given I have logged into the BBCRM home page
	And constituent 'Aaron Rodgers' exists
	And the pledge subtype 'ERBAddPledgeWithSubtype' exists
	And an "Enhanced Revenue Batch" with description "Pledge Subtype" exists
	| Account system                  | Constituent   | Amount  | Date     | Revenue type | Payment method | Designation  | Installment frequency | Installment start date | No. installments | GL post status | Pledge subtype          |
	| System Generated Account System | Aaron Rodgers | $500.00 | 1/1/2015 | Pledge       | None           | Library Fund | Monthly               | 1/1/2020               | 5                | Do not post    | ERBAddPledgeWithSubtype |
	When I commit the batch
	| Batch template         | Description    |
	| Enhanced Revenue Batch | Pledge Subtype |
	And the batch completes successfully
	And navigate to the revenue record for "Aaron Rodgers"
	Then the pledge subtype is "ERBAddPledgeWithSubtype"

@Revenue
Scenario: Add a donation payment with multiple designations through a batch
	Given I have logged into the BBCRM home page
	And constituent 'Cam Newton' exists
	When I start to add a batch with template "Enhanced Revenue Batch" and description "Cam's donation"
	| Account system                  | Constituent | Amount  | Date     | Revenue type | Payment method | GL post status |
	| System Generated Account System | Cam Newton  | $200.00 | 1/1/2015 | Payment      | Cash           | Do not post    |
	And apply the payment to designations
	| Additional applications | Applied amount | Designation           |
	| Donation                | $50.00         | LIBRARY BOOKS         |
	| Donation                | $150.00        | Camp Kids Scholarship |
	And save the batch and commit it
	| Batch template         | Description    |
	| Enhanced Revenue Batch | Cam's donation |
	Then the batch commits without errors or exceptions and 1 record processed
	And the revenue record for "Cam Newton" has payments
	| Designation           | Recognition credits |
	| Library Books         | $50.00              |
	| Camp Kids Scholarship | $150.00             |

@Revenue
Scenario: Add a payment towards multiple commitments through a batch
	Given I have logged into the BBCRM home page
	And constituent 'Tony Romo' exists
	And pledges exist
	| Constituent | Amount  | Designations                 | Frequency | No. installments | Post status |
	| Tony Romo   | $250.00 | Camp Kids Scholarship        | Monthly   | 5                | Do not post |
	| Tony Romo   | $500.00 | Smith-Brown Scholarship Fund | Monthly   | 5                | Do not post |
	When I start to add a batch with template "Enhanced Revenue Batch" and description "Tony's payment"
	| Account system                  | Constituent | Amount | Date     | Payment method | GL post status |
	| System Generated Account System | Tony Romo   | $75.00 | 1/1/2015 | Cash           | Do not post    |
	And set the revenue type for row 1 to "Payment"
	And auto apply the payment
	And save the batch and commit it
	| Batch template         | Description    |
	| Enhanced Revenue Batch | Tony's payment |
	Then the batch commits without errors or exceptions and 1 record processed
	And the revenue record for "Tony Romo" has payments
	| Designation                  | Recognition credits |
	| Smith-Brown Scholarship Fund | $25.00              |
	| Camp Kids Scholarship        | $50.00              |