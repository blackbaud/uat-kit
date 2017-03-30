Feature: IndividualConstituent
	In order to record information about alumni and supporters
	As a BBCRM user
	I want to add and edit Individual constituent information in the database

@Ready
@IndividualConstituent
Scenario: Individual Constituent: Add a new Individual Constituent record
	Given I have logged into the BBCRM home page
	When I add individual(s) with address
	| Title | Nickname | Information source | Last name | First name |
	| Mr.   | Bobby    | Other              | Prospect  | Bob        |
	Then individual constituent is created named "Bob Prospect" 
	And Personal information is displayed on Personal Info tab
	| First name | Last name | Title | Nickname |
	| Bob        | Prospect  | Mr.   | Bobby    |

@Ready
@IndividualConstituent
Scenario: Individual Constituent: Add a new Individual Constituent record with an address
	Given I have logged into the BBCRM home page
	When I add individual(s) with address
	| Title | Nickname | Information source | Address type | Country        | Address        | City    | ZIP    | Last name | First name |
	| Mr.   | Bobby    | Other              | Home         | United Kingdom | 12 Main Street | Glasgow | G2 BOB | Prospect  | Bob        |
	Then individual constituent is created named "Bob Prospect" 
	And An address is displayed on contact tab
	| Contact information | Type           | Primary | Start date |
	| 12 Main Street      | Home (Current) | Yes     |            |
	And Personal information is displayed on Personal Info tab
	| First name | Last name | Title | Nickname |
	| Bob        | Prospect  | Mr.   | Bobby    |

@Ready
@IndividualConstituent
Scenario: Individual Constituent: Add a new address to an existing Individual Constituent
	Given I have logged into the BBCRM home page
	And I add an individual
	| Title | Nickname | Information source | Last name | First name |
	| Mr.   | TC       | Other              | Case      | Test       |
	When I add the address to the current individual
	| Address type | Country        | Address        | City    | ZIP    |
	| Home         | United Kingdom | 12 Main Street | Glasgow | G2 BOB |
	Then An address is displayed on contact tab
	| Contact information | Type           | Primary | Start date |
	| 12 Main Street      | Home (Current) | Yes     |            |

@Ready
@IndividualConstituent
Scenario: Individual Constituent: Edit existing address on a Individual Constituent
	Given I have logged into the BBCRM home page
	And Individual constituent exists with an address
	| Address type | Country        | Address        | City    | ZIP    | Information source | Last name | First name |
	| Home         | United Kingdom | Gilbert Street | Glasgow | G3 8QN | Other              | Prospect  | Robert     |
	When I select a row under Addresses
	| Contact information | Type           |
	| Gilbert Street      | Home (Current) |
	And I edit the address
	| Address          | Postcode | Start date     |
	| Buccleuch Street | G3 6DY   | Today -2 weeks |
	Then An address is displayed on contact tab
	| Contact information | Type           | Primary | Start date     |
	| Buccleuch Street    | Home (Current) | Yes     | Today -2 weeks |