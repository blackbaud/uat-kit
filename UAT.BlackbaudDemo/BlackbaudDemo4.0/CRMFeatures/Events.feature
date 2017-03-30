Feature: Events
	In order to validate Events functionality
	As a user of BBCRM
	I want to add, modify, and validate Events

@Events 
Scenario: Add an Event with a Location
	Given I have logged into the BBCRM home page
	And Location "Exhibit Hall" exists
	When I add events
	| Name         | Start date | Location     |
	| Event_469108 | 01/01/2015 | Exhibit Hall |
	Then an event exists with the name 'Event_469108', start date '01/01/2015', and location 'Exhibit Hall'

@Events
Scenario: Add a Coordinator to an Event
	Given I have logged into the BBCRM home page
	And an event exists
	| Name         | Start date |
	| Event_469105 | 01/01/2015 |
	And staff constituent "Constituent_469105" exists
	When I add coordinator 'Constituent_469105' to event 'Event_469105'
	Then 'Constituent_469105' is a coordinator for event 'Event_469105'

@Events
Scenario: Add a multilevel Event
	Given I have logged into the BBCRM home page
	And an event exists
    | Name          | Start date |
    | Event1_469106 | 01/01/2015 |
    | Event2_469106 | 01/01/2015 |
    | Event3_469106 | 01/01/2015 |
	And event management template "AddMultiTemplateA" exists
	When I create a multi-event using template "AddMultiTemplateA"
	| event         | parent        |
	| Event1_469106 |               |
	| Event2_469106 | Event1_469106 |
	| Event3_469106 | Event2_469106 |
	Then a multi-level event "Event1_469106 - Default Summary" exists with hierarchy
	| event         | parent        |
	| Event1_469106 |               |
	| Event2_469106 | Event1_469106 |
	| Event3_469106 | Event2_469106 |

@Events
Scenario: Add Registration options to an Event
	Given I have logged into the BBCRM home page
	And an event exists
    | Name         | Start date |
    | Event_469109 | 01/01/2015 |
	When I add a registration option to event 'Event_469109'
	| Registration type | Name       |
	| Adult             | Full Price |
	Then event "Event_469109" has registration option
	| Registration type | Name       |
	| Adult             | Full Price |

@Events
Scenario: Add multiple Registration options to Events
	Given I have logged into the BBCRM home page
	And an event exists
    | Name          | Start date |
    | Event1_473412 | 01/01/2015 |
	And an event exists
    | Name          | Start date |
    | Event2_473412 | 01/01/2015 |
	When I add a registration option to event 'Event1_473412'
    | Registration type | Name  |
    | Adult             | Adult |
    | Child             | Child |
	And I add a registration option to event 'Event2_473412'
	| Registration type   | Name                |
	| Couple              | Couple              |
	| Hole-in-One Sponsor | Hole-in-One Sponsor |
	Then event "Event1_473412" has registration option
	| Registration type | Name  |
	| Adult             | Adult |
	| Child             | Child |
	And event "Event2_473412" has registration option
	| Registration type   | Name                |
	| Couple              | Couple              |
	| Hole-in-One Sponsor | Hole-in-One Sponsor |

@Events
Scenario: Add an Expense to an event
	Given I have logged into the BBCRM home page
	And an event exists
	| Name         | Start date |
	| Event_473517 | 01/01/2015 |
	And constituent 'Constituent_473517' exists
	When I add expense to 'Event_473517'
	| Type          | Vendor             | Budgeted amount | Comment |
	| Entertainment | Constituent_473517 | $1,500.00       | Meal    |
	Then event 'Event_473517' has expense
	| Type          | Vendor             | Budgeted amount | Comment |
	| Entertainment | Constituent_473517 | $1,500.00       | Meal    |

@Events
Scenario: Add a Task with Reminder to an event
	Given I have logged into the BBCRM home page
	And an event exists
	| Name         | Start date |
	| Event_473577 | 01/01/2015 |
	And constituent 'Constituent_473577' exists
	When I add a task to event 'Event_473577'
	| Name             | Comment             | Owner              | Date due   |
	| Send Invitations | Prepare invitations | Constituent_473577 | 01/01/2050 |
	And add reminder to task 'Send Invitations' on event 'Event_473577'
	| Name       | Date       |
	| Reminder 1 | 01/01/2015 |
	Then reminder 'Reminder 1 (01/01/2015)' exists for task 'Send Invitations'

@Events 
Scenario: Add a 2 Level multi-level Event record
	Given I have logged into the BBCRM home page
	And Location "Exhibit Hall" exists 
	And an event exists
	| Name                             | Start date |
	| Alumni Weekend 2015              | 05/04/2015 |
	| Saturday Daytime with lunch      | 05/04/2015 |
	| Saturday Daytime                 | 05/04/2015 |
	| Saturday night with B&B (single) | 05/04/2015 |
	| Saturday night with B&B (double) | 05/04/2015 |
	And event management template "AddMultiTemplateB" exists
	When I start to create a multi-event using template "AddMultiTemplateB"
	| event                            | parent              |
	| Alumni Weekend 2015              |                     |
	| Saturday Daytime with lunch      | Alumni Weekend 2015 |
	| Saturday Daytime                 | Alumni Weekend 2015 |
	| Saturday night with B&B (single) | Alumni Weekend 2015 |
	| Saturday night with B&B (double) | Alumni Weekend 2015 |
	And I copy from a sub-event "Saturday Daytime with lunch" and name it "Sunday Daytime with lunch" under "Alumni Weekend 2015"
	Then a multi-level event "Alumni Weekend 2015 - Default Summary" exists with hierarchy
	| event                            | parent              |
	| Alumni Weekend 2015              |                     |
	| Saturday Daytime with lunch      | Alumni Weekend 2015 |
	| Saturday Daytime                 | Alumni Weekend 2015 |
	| Saturday night with B&B (single) | Alumni Weekend 2015 |
	| Saturday night with B&B (double) | Alumni Weekend 2015 |
	| Sunday Daytime with lunch        | Alumni Weekend 2015 |

@Events
Scenario: Add Dietary Preferences to an event
	Given I have logged into the BBCRM home page
	And an event exists
	| Name                        | Start date |
	| Saturday Daytime with lunch | 05/04/2015 |
	When I add preference "Dietary Preferences" to event "Saturday Daytime with lunch"
	| Options     |
	| Vegetarian  |
	| Vegan       |
	| Nut allergy |
	Then event "Saturday Daytime with lunch" has a preference with options
	| Name                | Options                        |
	| Dietary Preferences | Nut allergy; Vegan; Vegetarian |
	
@Events
Scenario: Add a registrant and guest to an event
	Given I have logged into the BBCRM home page
	And constituent 'Puppy Enthusiast' exists
	And an event exists
	| Name         | Start date |
	| Puppy Parade | 01/01/2015   |
	And event 'Puppy Parade' has registration option
	| Registration type | Name     | Registration count | Registration fee |
	| Adult             | Adult +1 | 2                  | $20.00           |
	When I add registrant 'Puppy Enthusiast' to event 'Puppy Parade'
	| Registration option | Registrant       |
	| Adult +1            | Puppy Enthusiast |
	| Adult +1            | (Unnamed guest)  |
	Then registrant record 'Puppy Enthusiast' is created for event 'Puppy Parade' with 1 guest
