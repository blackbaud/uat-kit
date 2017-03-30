Feature: Major Giving Prospects
	As a BBCRM user
	I want to flag Constituents as Prospects
	And manage Prospect Plans for Major Giving solicition

@Ready
@MajorGiving
Scenario: Major Giving: Add a Prospect Constituent
	Given I have logged into the BBCRM home page
	And a constituent record exists
	| Title | Nickname | Information source | Last name | First name |
	| Mr.   | Bobby    | Other              | Case      | Test       |
	And I add Constituencies to the following Constituents
	| Surname | Date from | Date to       | Constituency          |
	| Case    | Today     | Today +1 year | Major giving prospect |
	Then the Prospect Constituency is added
	| Description           | Date from | Date to       |
	| Major giving prospect | Today     | Today +1 year |

@Ready
@MajorGiving
Scenario: Major Giving: Add a Fundraiser to a Prospect Team on a Prospect
	Given I have logged into the BBCRM home page
	And a constituent record exists
	| Title | Nickname | Information source | Last name          | First name |
	| Mr.   | Bobby    | Other              | ExistingProspect   | Test       |
	| Mr.   | TC       | Other              | ExistingFundraiser | Test       |
	And I add Constituencies to the following Constituents
	| Surname            | Date from | Date to       | Constituency          |
	| ExistingProspect   | Today     | Today +1 year | Major giving prospect |
	| ExistingFundraiser | Today     | Today +5 year | Fundraiser            |
	And Prospect Team role "Relationship Lead" exists for "ExistingProspect"
	When I Add team member to "ExistingProspect"
	| Team member        | Role              | Start date |
	| ExistingFundraiser | Relationship Lead | Today      |
	Then The "Relationship Lead - Current" Team Member exists
	| Name               | Role              | Start date |
	| ExistingFundraiser | Relationship Lead | Today      |

@Ready
@MajorGiving
Scenario: Major Giving: Add a Major Giving Prospect Plan to a Constituent
	Given I have logged into the BBCRM home page
	And a constituent record exists
	| Title | Nickname | Information source | Last name | First name |
	| Mr.   | A        | Other              | Prospect  | Bob        |
	| Mr.   | B        | Other              | Baker     | Fred       |
	| Mr.   | C        | Other              | Keyworth  | Daniel     |
	And I add Constituencies to the following Constituents
	| Surname  | Date from | Date to       | Constituency          |
	| Prospect | Today     | Today +1 year | Major giving prospect |
	| Keyworth | Today     | Today +5 year | Fundraiser            |
	| Baker    | Today     | Today +5 year | Fundraiser            |
	And I add Plan Outline "UAT MG Plan Outline" to Major Giving Setup
	| Objective                     | Fundraiser role  | Stage          | Days from start | Contact method |
	| Clearance to Approach Client  | Prospect manager | Identification | 7               |                |
	| Prepare Ask client            | Primary manager  | Cultivation    | 20              |                |
	| Explore Inclination of client | Primary manager  | Cultivation    | 60              |                |
	| Make Ask of client            | Primary manager  | Negotiation    | 90              |                |
	When I start to add a major giving plan to 'Bob Prospect'
	And set the details
	| Plan name   | Plan type    | Start date | Primary manager | Secondary manager |
	| UAT MG Plan | Major giving | Today      | Baker           | Keyworth          |
	And set the Steps with outline 'UAT MG Plan Outline'
	And insert a step on row 3
	| Expected date | Objective       | Owner | Stage       | Status  | Contact method |
	| Today +1 week | Discuss Mission | Baker | Cultivation | Planned | Phone call     |
	And insert a step on row 4
	| Expected date   | Objective       | Owner | Stage       | Status  | Contact method |
	| Today +3 Months | Discuss mission | Baker | Cultivation | Planned | Meeting        |
	And save the plan
	Then the plan exists on the constituent
	| Constituent  | Plan type    | Plan name   |
	| Bob Prospect | Major giving | UAT MG Plan |

@Ready
@MajorGiving
Scenario: Major Giving: Add a planned gift to a major giving prospect
	Given I have logged into the BBCRM home page
	And I add individual(s)
	| Last name   | First name | Title | Nickname | Information source |
	| Testcase    | Bob        | Mr.   | Test     | Other              |
	| Prospecting | Bob        | Mr.   | Case     | Other              |
	And I add Constituencies to the following Constituents
	| Surname     | Date from | Date to       | Constituency          |
	| Prospecting | Today     | Today +1 year | Major giving prospect |
	And Prospect "Bob Prospecting" has an individual relationship with constituent "Bob Testcase"
	When I add a Planned Gift to Prospect "Bob Prospecting"
	| Planned gift vehicle | Status   | Gift amount | Probate status     |
	| Bequest              | Accepted | $100.00     | Final distribution |
	And I add to planned Gift Details
	| Designation   | Amount  |
	| Library Books | $100.00 |
	And I add to Planned Giving Relationships 
	| Constituent  |
	| Bob Testcase |
	And I add to Assests tab
	| Type  | Description | Value  | Cost Basis | Valuation method | 
	| Books | Test        | 100    | 0.00       | Internal         |
	Then the Planned Gift can be seen with the details
	| Constituent     | Status   | Vehicle | Amount  | Sites |
	| Bob Prospecting | Accepted | Bequest | $100.00 |       |

@Ready
@MajorGiving
Scenario: Major Giving: Add Opportunity to Major Giving Plan on Prospect
	Given I have logged into the BBCRM home page
	And a constituent record exists
	| Title | Nickname | Information source | Last name | First name |
	| Mr.   | Bobby    | Other              | Hannibal  | Test       |
	| Mr.   | Bobby    | Other              | William   | Test       |
	And I add Constituencies to the following Constituents
	| Surname  | Date from | Date to       | Constituency          |
	| Hannibal | Today     | Today +1 year | Major giving prospect |
	| William  | Today     | Today +1 year | Fundraiser            |
	And I add Plan Outline "UAT Test Outline Two" to Major Giving Setup
	| Objective                     | Fundraiser role  | Stage          | Days from start | Contact method |
	| Clearance to Approach Client  | Prospect manager | Identification | 7               |                |
	| Prepare Ask client            | Primary manager  | Cultivation    | 20              |                |
	| Explore Inclination of client | Primary manager  | Cultivation    | 60              |                |
	| Make Ask of client            | Primary manager  | Negotiation    | 90              |                |
	And I start to add a major giving plan to 'Hannibal'
	And set the details
	| Plan name        | Plan type    | Start date | Primary manager | Secondary manager |
	| UAT Test MG Plan | Major giving | Today      | William         |                   |
	And set the Steps with outline 'UAT Test Outline Two'
	And save the plan
	When I go to plan "UAT Test MG Plan" for prospect "Hannibal"
	And I have selected Add from the Opportunities tab
	| Status    | Expected Ask Amount |  Expected Ask date from now |
	| Qualified | $10,000.00          |  90                         |
	Then an Opportunity is associated with the Major Giving Plan called "UAT Test MG Plan"
	| Status    | Expected Ask Amount | Expected Ask date from now |
	| Qualified | $10,000.00          | 90                         |