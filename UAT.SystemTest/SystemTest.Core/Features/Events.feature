Feature: Events
	In order to test event functionality 
	As a BBCRM user
	I want to manage Event records, registrations, invitations and responses

@Ready
@Events
Scenario: Events: Add a multilevel Event record
	Given I have logged into the BBCRM home page
	And an event exists
	| Name   | Start date | Category |
	| EventA | Today      | Sport    |
	| EventB | Today      | Sport    |
	| EventC | Today      | Sport    |
	And Event management template "Sample Template 1" exists 
	When I create a multi-event using template "Sample Template 1"
	| event  | parent |
	| EventA |        |
	| EventB | EventA |
	| EventC | EventB |
	Then a multi-level event "EventA - Default Summary" exists with hierarchy
	| event  | parent |
	| EventA |        |
	| EventB | EventA |
	| EventC | EventB |

@Ready
@Events
Scenario: Events: Add multiple Registration options to a single event
	Given I have logged into the BBCRM home page
	And an event exists
	| Name         | Start date | Category |
	| Brunch Event | Today      | Brunch   |
	When I add a Registration Option to event "Brunch Event"
	| Registration type | Registration count | Registration fee |
	| Adult             | 1                  | $75.00           |
	| Couple            | 2                  | $150.00          |
	Then event "Brunch Event" has registration option
	| Registration type | Registration fee |
	| Adult             | $75.00           |
	| Couple            | $150.00          |

@Ready
@Events
Scenario: Events: Add multiple invitees to an invitation using selection and excluding Deceased
	Given I have logged into the BBCRM home page	
	And I add individual(s)
	| Last name | First name | Title | Nickname    | Information source |
	| Salmon    | Jon        | Mr.   | Lord Salmon | Other              |
	| Lange     | Ty         | Mr.   | Lord Ty     | Other              |
	| Strong    | Andrea     | Ms.   | Lady Andrea | Other              |
	| Taylor    | Al         | Mr.   | Lord Al     | Other              |
	And an event exists
    | Name    | Start date | Category |
    | Wedding | today      | Formal   |
	And an invitation "Wedding Invitation" exists 
	And I add a Constituent ad-hoc query
	And include "Constituent" record "Constituent record" field with criteria
	| field          | value         |
	| FILTEROPERATOR | One of        |
	| VALUE          | Jon Salmon    |
	| VALUE          | Ty Lange      |
	| VALUE          | Andrea Strong |
	| VALUE          | Al Taylor     |
	And I save Query Designer with the following options 
	| Name                 | Description           | Create Selection | Static | Show in Query Designer |
	| Invitees for Wedding | Ad-hoc Query UAT Test | true             |        |                        |
	And Constituent "Al Taylor" is marked as deceased with source of "Test"
	And I select event "Wedding"
	When I Add multiple invitees to invitation "Wedding Invitation" from selection "Invitees for Wedding"
	Then Event Invitation Invitee List shows Invitees
	| Forename | Surname | Include in next send |
	| Ty       | Lange   | checked              |
	| Jon      | Salmon  | checked              |
	| Andrea   | Strong  | checked              |

@Ready
@Events
Scenario: Events: Register an event invitee for a single event with a guest who is not a constituent
	Given I have logged into the BBCRM home page
	And Individual constituent exists with an address
    | Address type | Country       | Address      | City       | State | ZIP   | Information source | Last name | First name |
    | Home         | United States | 12 Hope Road | Charleston | SC    | 12345 | Other              | Brown     | Merideth   |
	And Event "Test Event" exists with Registration Option and start date "Today +6 months"
    | Registration type | Registration count | Registration fee |
    | Adult             | 1                  | $100.00          |
	And Marketing Export Definitions "UAT Test Export" exists
	And Mail Package record "UAT Events" exists with Export Definition "UAT Test Export"
	And Invitation to the event "Test Event" includes a mail package
    | Name                       | Description                    | Mail date | Mail package | How to send invitation |
    | College Reunion Invitation | College Reunion Invitation UAT | Today     | UAT Events   | Send through mail only |
	And I add Invitees to invitation "College Reunion Invitation"
	| Invitee        |
	| Merideth Brown |      
	When I send the invitation
	And I register the invitee
    | Registration option | Registrant      |
    | Adult               | Merideth Brown  |
    | Adult               | (Unnamed guest) |
	Then Registration(s) are listed on Registrant page
    | Type       | Status     | Registrant     | Guest 1                 | Guest 2 |
    | Invitation | Registered | Merideth Brown | Guest of Merideth Brown |         |
	And Event "Test Event" displays registrant(s) on Registrations tab
    | Registrant First Name | Registrant Last Name | Extra    | Balance |
    | Merideth              | Brown           |          | $200.00 |
    | Merideth              | Brown           | Guest of | $0.00   |

@Ready
@Events
Scenario: Events: Mark Event invitee as declined
	Given I have logged into the BBCRM home page
	And Individual constituent exists with an address
    | Address type | Country       | Address        | City       | State | ZIP   | Information source | Last name | First name |
    | Home         | United States | 14 Shadow Road | Greenville | SC    | 12345 | Other              | Holland   | Mary       |
	And Event "Test Event" exists with Registration Option and start date "Today +6 months"
    | Registration type | Registration count | Registration fee |
    | Adult             | 1                  | $100.00          |
	And Marketing Export Definitions "UAT Test Export" exists
	And Mail Package record "UAT Events" exists with Export Definition "UAT Test Export"
	And Invitation to the event "Test Event" includes a mail package
    | Name                       | Description                    | Mail date | Mail package | How to send invitation |
    | College Reunion Invitation | College Reunion Invitation UAT | Today     | UAT Events   | Send through mail only |
	And I add Invitees to invitation "College Reunion Invitation"
	| Invitee      |
	| Mary Holland |   
	And I send the invitation
	When I mark the invitee "Mary Holland" as declined
	Then Invitees list displays where Declined is "true"
	| Invitee      | Invitation sent on |
	| Mary Holland | Today              |