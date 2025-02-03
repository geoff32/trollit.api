Feature: ProfilesController
  Tests for profiles API
    
  # Get Profile
    
  Scenario: Get profile shared with me
    Given I am an authenticated user
    And An existing profile
    And The profile is shared for reading with me
    When I get the profile
    Then The response should be ok
  
  Scenario: Get profile not shared with me
    Given I am an authenticated user
    And An existing profile
    And The profile is not shared for reading with me
    When I get the profile
    Then The response should be forbidden
  
  Scenario: Get profile with an unauthenticated user
    Given I am an unauthenticated user
    And An existing profile
    When I get the profile
    Then The response should be unauthorized
  
  Scenario: Get missing profile with an unauthenticated user
    Given I am an unauthenticated user
    And A missing profile
    When I get the profile
    Then The response should be unauthorized
  
  Scenario: Get missing profile
    Given I am an authenticated user
    And A missing profile
    When I get the profile
    Then The response should be forbidden
    
# Refresh Profile
    
  Scenario: Refresh profile shared with me
    Given I am an authenticated user
    And An existing profile
    And The profile is shared for refresh with me
    When I refresh the profile
    Then The response should be ok
  
  Scenario: Refresh profile not shared with me
    Given I am an authenticated user
    And An existing profile
    And The profile is not shared for refresh with me
    When I refresh the profile
    Then The response should be forbidden
  
  Scenario: Refresh profile with an unauthenticated user
    Given I am an unauthenticated user
    And An existing profile
    When I refresh the profile
    Then The response should be unauthorized
  
  Scenario: Refresh missing profile with an unauthenticated user
    Given I am an unauthenticated user
    And A missing profile
    When I refresh the profile
    Then The response should be unauthorized
  
  Scenario: Refresh missing profile
    Given I am an authenticated user
    And A missing profile
    When I refresh the profile
    Then The response should be forbidden
