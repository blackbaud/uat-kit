Feature: Revenue 
	As a BBCRM user
	I want to be able to add payments to a variety of revenue types and populate various revenue data

@Ready
@Revenue
Scenario: Revenue: Add a single Donation payment to a constituent
	Given I have logged into the BBCRM home page
	And I add individual
	| Last name | First name | Title | Nickname | Information source |
	| Gamgee    | Sam        | Mr.   | Sam      | Other              |
	When I add a Payment to constituent "Sam Gamgee" with account system "System Generated Account System"
	| Amount  | Date  | Application | Designation   | Payment Method | Payment is anonymous | Reference | Benefit |
	| $100.00 | Today | Donation    | Library Books | Check          |                      |           |         |
	Then the revenue record is presented on the constituent

@Ready
@Revenue
Scenario: Revenue: Add a single Pledge payment to a constituent
	Given I have logged into the BBCRM home page
	And I add individual(s)
	| Last name | First name | Title | Nickname | Information source |
	| Bailey    | Fredrick   | Mr.   | Fred     | Other              |
	And A Pledge has been submitted today for the Constituent "Fredrick Bailey" with account system "System Generated Account System"
	| Amount    | Designation   | Installments | Start date | Post Type   | Number of Installments |
	| $1,200.00 | Library Books | Monthly      | Today      | Do not post | 1                      |
	When I add a Payment to constituent "Fredrick Bailey" with account system "System Generated Account System"
	| Amount  | Date  | Application | Designation   | Payment Method | Payment is anonymous | Reference | Benefit |
	| $100.00 | Today | Pledge      | Library Books | Check          |                      |           |         |
	Then the revenue record is presented on the constituent as Applied to the Pledge for the amount "$100.00"
	And the Pledge record balance is reduced by the payment amount value for constituent "Fredrick Bailey"
	And the Pledge Installment/Write-off Activity tab shows the payment linked to an installment

@Ready
@Revenue
Scenario: Revenue: Add a payment to a constituent to fulfil an Event Registration fee
	Given I have logged into the BBCRM home page
	And I add individual(s)
	| Last name | First name | Title | Nickname | Information source |
	| Bailey    | Tom        | Mr.   | Tom      | Other              |
	And Event "Test Event" exists with Registration Option
	| Registration type | Registration count | Registration fee |
	| Adult             | 1                  | $200.00          |
	And Constituent "Tom Bailey" is registered for event named "Test Event" with "Adult" registration option 
	When I add a Payment to constituent "Tom Bailey" with account system "System Generated Account System"
	| Amount  | Date  | Application        | Designation | Payment Method | Payment is anonymous | Reference | Benefit |
	| $200.00 | Today | Event registration |             | Check          |                      |           |         |
	Then the revenue record is presented on the constituent "Tom Bailey" as Applied to Event Registration for "Test Event"
	And Balance on Registration fee is "$0.00" for constituent "Tom Bailey" as Applied to Event Registration for "Test Event"

@Ready
@Revenue
Scenario: Revenue: Add a pledge with multiple designations
	Given I have logged into the BBCRM home page
	And I add individual(s)
	| Last name | First name | Title | Nickname | Information source |
	| Other     | A N        | Mr.   | Bobby    | Other              |
	And designation exists 'Library Books'
	And designation exists 'Library Furniture'
	When I start to add a pledge
	| Constituent | Amount  |
	| A N Other   | $300.00 |
	And split the pledge designations evenly
	| Designation       | Amount  |
	| Library Books     | $150.00 |
	| Library Furniture | $150.00 |
	And save the pledge
	Then a pledge exists with designations
	| Designation       | Amount  | Balance |
	| Library Books     | $150.00 | $150.00 |
	| Library Furniture | $150.00 | $150.00 |

@Ready
@Revenue
Scenario: Revenue: Edit a posted payment to apply to an outstanding Pledge
	Given I have logged into the BBCRM home page
	And Allow direct posting of all payments has been set for account system "System Generated Account System"
	And I add individual(s)
	| Last name | First name | Title | Nickname | Information source |
	| Shaw      | Franklin   | Mr.   | Frank    | Other              |
	And I add a Payment to constituent "Franklin Shaw" with account system "System Generated Account System"
	| Amount  | Date  | Application | Designation   | Payment Method | Post Status | Payment Is Anonymous | Reference | Benefit |
	| $100.00 | Today | Donation    | LIBRARY BOOKS | Check          | Not Posted  | false                |           |         |                        
	And a revenue type selection exists for the constituent "Shaw" donation payment called "Test Central Post Shaw"
	And a Post to GL process exists
	| Name         | Account System                  | Output format        | Post date up to | Deposits | Revenue                  | Selection               | Bank Account Adjustments | Mark revenue records Posted |
	| TestPostToGL | System Generated Account System | Standard post format | Today           | None     | Selected revenue records | Test Central Post Shaw  | None                     | true                        |
	And I Start the "TestPostToGL"
	And the process "TestPostToGL" runs without error
	And A Pledge has been submitted today for the Constituent "Franklin Shaw" with account system "System Generated Account System"
    | Amount  | Designation   | Start date | Installments | Number of Installments | Installment amount | Post type   |
    | $100.00 | LIBRARY BOOKS | Today      | Annually     | 1                      |                    | Do not post |
	When I edit posted payment for constituent "Franklin Shaw" for type "default"
	And I Remove the Current applications
	And I change Application dropdown to "Pledge"
	And I select the Pledge for "Franklin Shaw"
	And I click Add
	And I select radio button for Pledge balance
	And I add Adjustment details
	| Adjustment reason                     |
	| Adjustment - ERBApplyEditPostedPledge |
	Then the revenue record (Amount) is presented on the constituent "Franklin Shaw" for amount "$100.00"
	And Revenue Transaction Profile View Form displays
	| Post Status               | Date Started | End date | Remain anonymous |
	| Posted (activity pending) |              |          |                  |

@Ready
@Revenue
Scenario: Revenue: Add a Donation payment to a constituent with payment method of Stock
	Given I have logged into the BBCRM home page
	And I add individual
	| Last name | First name | Title | Nickname | Information source |
	| Stiller     | Susan      | Miss  | Sue    | Other              |
	When I add a Payment to constituent "Susan Stiller" with account system "System Generated Account System"
	| Amount  | Date  | Application | Designation   | Payment method | Issuer         | Number of units | Price per share Median | Payment is anonymous | Reference | Benefit |
	| $152.80 | Today | Donation    | Library Books | Stock          | Burberry Group | 10              | 15.2800                |                      |           |         |
	Then the revenue record is presented on the constituent with Amount "$152.80"
	And Revenue Transaction Page Transaction Summary displays payment method information
	| Payment Amount | Date  | Payment method | Number of units | Median price | Sold |
	| $152.80        | Today | Stock          | 10.000          | 15.2800      |      |

@Ready
@Revenue
Scenario: Revenue: Add a single Donation payment to a constituent and mark as anonymous
	Given I have logged into the BBCRM home page
	And I add individual
	| Last name | First name | Title | Nickname | Information source |
	| Thomas    | Sally      | Mrs.   | Sal      | Other              |
	When I add a Payment to constituent "Sally Thomas" with account system "System Generated Account System"
	| Amount  | Date  | Application | Designation   | Payment Method | Payment is anonymous | Reference | Benefit |
	| $100.00 | Today | Donation    | Library Books | Check          | true                 |           |         |
	Then the revenue record is presented on the constituent with notification "Constituent requests to remain anonymous for this payment."

@Ready
@Revenue
Scenario: Revenue: Add a single Donation payment to a constituent in response to an Appeal Mailing
	Given I have logged into the BBCRM home page
	And I add individual(s)
	| Last name | First name | Title | Nickname | Information source |
	| Grey      | Gary       | Mr.   | Spike    | Other              |
	And Marketing Export Definitions "UAT-TEST Export 2 - Testing Purposes only" exists
	And Mail Package record "UAT-Test Package 2" exists with Export Definition "UAT-TEST Export 2 - Testing Purposes only"
	And Constituent type Selection exists
	And I include "Constituents" record "Name" field with criteria equal to
	| field          | value    |
	| FILTEROPERATOR | Equal to |
	| VALUE1         | Grey     |
	And I save Query Designer with the following options 
	| Name            | Description           | Create Selection | Static | Show in Query Designer |
	| UAT-Appeal-Gary | Ad-hoc Query UAT Test | true             | true   |                        |
	And Appeal "UAT TEST - General Appeal" exists
	And I add an Appeal mailing
	| Name            | Mail date      | Appeal                    | Selection       | Package            |
	| UAT Test Mail D | Today +1 month | UAT TEST - General Appeal | UAT-Appeal-Gary | UAT-Test Package 2 |	
	When I add a Payment to constituent "Gary Grey" with account system "System Generated Account System"
	| Amount  | Date  | Application | Designation   | Payment Method | Payment is anonymous | Reference | Benefit |
	| $100.00 | Today | Donation    | Library Books | Check          |                      |           |         |
	And I enter Marketing appeal "UAT TEST - General Appeal"
	Then the revenue record is presented on the constituent with Appeal "UAT TEST - General Appeal"
	
@Ready
@Revenue
Scenario: Revenue: Add a new campaign with a specified monetary goal
	Given I have logged into the BBCRM home page
	And a Fundraiser exists
	| Last name | First name | Title | Nickname | Information source |
	| Grey      | Gary       | Mr.   | Spike    | Other              |
	And I add site "Site A"
	And I add campaign(s)
	| Name            | LookupID        | Type  | Site   | Start date | End date      |
	| Test Campaign B | Test Campaign B | Smile | Site A | Today      | Today +1 year | 
	And I Add Fundraisers via Fundraiser tab
	| Fundraiser | Start date |
	| Gary Grey  | Today      |         
	And I add campaign(s)
	| Name            | LookupID        | Type  | Site   | Start date | End date      |
	| Test Campaign A | Test Campaign A | Other | Site A | Today      | Today +1 year |
	And I add a Goal to "Test Campaign A"
	| Name        | Amount     | Start date | End date      |
	| Test Goal A | $10,000.00 | Today      | Today +1 year |
	When I edit the campaign hierarchy 
	And I add a campaign to the hierarchy for "Test Campaign A"
	And I use Campaign Search for "Test Campaign B"
	And I add Campaign Goal(s) to "Test Campaign B"
	| Name        | Amount   | 
	| Test Goal A | $5,000.00 | 
	Then "Test Campaign B" Goal tab displays
	| Name        | Amount    | Start date | End date      |
	| Test Goal A | $5,000.00 | Today      | Today +1 year |
	And "Test Campaign B" Fundraisers tab displays
	| Fundraiser | Start date |
	| Gary Grey  | Today      |        
	And "Test Campaign B" Campaign hierarchy displays 
	| Name            | 
	| Test Campaign A |

@Ready
@Revenue
Scenario: Revenue: Add a payment for an event registration fee with additional donation
	Given I have logged into the BBCRM home page
	And I add individual(s)
	| Last name | First name | Title | Nickname | Information source |
	| Bradbury  | Tom        | Mr.   | Tom      | Other              |
	And Event "College Reunion" exists with Registration Option and start date "Today +6 months"
    | Registration type | Registration count | Registration fee |
    | Adult             | 1                  | $100.00          |
	And Constituent "Tom Bradbury" is registered for event named "Test Event" with "Adult" registration option
	When I add a batch with template "Enhanced Revenue Batch" and description "ERB Event Payment"
    | Account system                  | Constituent | Amount  | Date  | Payment method |
    | System Generated Account System | Bradbury    | $150.00 | Today | Check          |
	And I select Revenue type "Payment" 
	And I add Additional applications
	| Additional applications | Applied amount | Designation   |
	| Donation                | $50.00         | LIBRARY BOOKS |
	And I prepare the batch for commit
	And I commit the batch
    | Batch template         | Description       |
	| Enhanced Revenue Batch | ERB Event Payment |		 
	Then Event "College Reunion" displays registrant(s) on Registrations tab
    | Registrant First Name | Registrant Last Name | Balance | Extra |
    | Tom                   | Bradbury             | $0.00   |       |
	And I navigate to payment from Event
	| Payment amount | Date  | Surname  | First Name |
	| $100.00        | today | Bradbury | Tom        |
	And Payment applications details are correct
	| Application        | Amount  |
	| Event registration | $100.00 |
	| Donation           | $50.00  |