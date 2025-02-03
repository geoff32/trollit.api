Feature: AccountController
  Tests for account API

# Account Creation

  Scenario: Create account
    Given I have a new account request
      | UserName     | Password     | TrollId | Token     |
      | testUserName | testPassword | 1       | testToken |
    And The troll exists
    And The name of troll 1 is trollName
    And The account does not exist
    And The account token is valid
    When I create the account
    Then The account response should be ok
    And The account should be created
    And The authentication cookie should be set

  Scenario: Create account with password too short
    Given I have a new account request with password too short
    And The troll exists
    And The account does not exist
    When I create the account
    Then The response should be bad request
    And The account should not be created
    And The authentication cookie should not be set

  Scenario: Create account with existing account
    Given I have a new valid account request
    And The troll exists
    And The account exists
    When I create the account
    Then The response should be bad request
    And The account should not be created
    And The authentication cookie should not be set

  Scenario: Create account with unknown troll
    Given I have a new valid account request
    And The troll does not exist
    When I create the account
    Then The response should be bad request
    And The account should not be created
    And The authentication cookie should not be set

  Scenario: Create account with invalid token
    Given I have a new valid account request
    And The troll exists
    And The account does not exist
    And The account token is invalid
    When I create the account
    Then The response should be bad request
    And The account should not be created
    And The authentication cookie should not be set

# Account validation
  Scenario: Validate account
    Given I am an authenticated user
    When I validate the account
    Then The response should be ok

  Scenario: Validate account with unauthenticated user
    Given I am an unauthenticated user
    When I validate the account
    Then The response should be unauthorized

# Sign in
  Scenario: Sign in
    Given I have valid credentials
    When I sign in
    Then The account response should be ok
    And The authentication cookie should be set

  Scenario: Sign in with invalid credentials
    Given I have invalid credentials
    When I sign in
    Then The response should be unauthorized
    And The authentication cookie should not be set

# Sign out
  Scenario: Sign out
    Given I am an authenticated user
    When I sign out
    Then The response should be no content
    And The authentication cookie should be unset

