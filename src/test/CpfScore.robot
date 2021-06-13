*** Settings ***
Library    DateTime
Resource   CpfScore.resource
Test setup    Up Server
Test Teardown    Down Server

*** Variables ***
${dateFormat}     %Y-%m-%d %H:%M:%SZ

*** Test Cases ***
Extract CPF Should Work
    ${response} =    Extract CPF    11111111111
    ${body} =    Set Variable    ${response.json()}

    Should Be Equal As Numbers    200    ${response.status_code}
    Should Be Equal As Strings    11111111111    ${body}[cpf]
    Should Be True    ${body}[score] in range(1, 1001)
    Parse Date From Body    ${body}

Extract CPF Should Work With Formatted CPF
    ${response} =    Extract CPF    111.111.111-11
    ${body} =    Set Variable    ${response.json()}

    Should Be Equal As Numbers    200    ${response.status_code}
    Should Be Equal As Strings    11111111111    ${body}[cpf]

Get CPF Should Return the Last Score
    Extract CPF    00233344455

    ${extractResponse} =    Extract CPF    00233344455
    ${getResponse} =    Get CPF    00233344455

    Should Be Equal As Numbers    200    ${getResponse.status_code}

    ${extractBody} =    Set Variable    ${extractResponse.json()}
    ${getBody} =    Set Variable    ${getResponse.json()}

    Should Be Equal As Strings    ${getBody}[cpf]    ${extractBody}[cpf]
    Should Be Equal As Numbers    ${getBody}[score]    ${extractBody}[score]

    ${getCreatedAt} =    Parse Date From Body    ${getBody}
    ${extractCreatedAt} =    Parse Date From Body    ${extractBody}

    Should Be Equal    ${getCreatedAt}    ${extractCreatedAt}

Get CPF Should Return the Last Score With Formatted CPF
    Extract CPF    00233344455
    ${getResponse} =    Get CPF    002.333.444-55

    Should Be Equal As Numbers    200    ${getResponse.status_code}

    ${getBody} =    Set Variable    ${getResponse.json()}

    Should Be Equal As Strings    00233344455    ${getBody}[cpf]

Extract CPF With Malformatted CPF
    Extract CPF    11b11111111    expectedStatus=500

Extract CPF With Malformatted CPF v2
    Extract CPF    011111111111    expectedStatus=500

Get CPF With a CPF Not Extracted
    Get CPF    11111111111    expectedStatus=500

Get CPF With Malformatted CPF
    Get CPF    1111a111111    expectedStatus=500

Get CPF With Malformatted CPF v2
    Get CPF    011111111111    expectedStatus=500

*** Keywords ***
Parse Date From Body
    [Arguments]    ${body}

    ${x} =    Convert Date    ${body}[createdAt]    date_format=${dateFormat}
    Return From Keyword    ${x}
