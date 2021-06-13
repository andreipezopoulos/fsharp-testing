*** Settings ***
Library    Process
Library    RequestsLibrary
Library    OperatingSystem

*** Variables ***
${dockerFile}     src/docker/docker-compose.yaml
${url}            http://localhost:8085

*** Keywords ***
Extract CPF
    [Arguments]    ${cpf}    ${expectedStatus}=200

    ${rawRequest} =    Get File    src/requests/extract_score.json
    ${request} =    Evaluate    json.loads("""${rawRequest}""".replace("{{cpf}}", "${cpf}"))    modules=json

    ${response} =    POST    ${url}/score    json=${request}    expected_status=${expectedStatus}

    Return From Keyword    ${response}

Get CPF
    [Arguments]    ${cpf}    ${expectedStatus}=200

    ${response} =    GET    ${url}/score/${cpf}    expected_status=${expectedStatus}
    Return From Keyword    ${response}

Health Check
    GET    ${url}/health    timeout=10

Up Server
    ${dockerBuild} =    Run Process    docker    build
    ...                                          -f    src/docker/Dockerfile
    ...                                          .    --tag    cpfscore    stderr=STDOUT
    Log    ${dockerBuild.stdout}

    ${dockerUp} =    Run Process    docker-compose    --project-directory    .
    ...                                               -f    ${dockerFile}    up    -d    stderr=STDOUT
    Log    ${dockerUp.stdout}

    Wait Until Keyword Succeeds    1 min    5 sec    Health Check

Down Server
    ${dockerDown} =    Run Process    docker-compose     --project-directory    .
    ...                                                  -f    ${dockerFile}    down    stderr=STDOUT
    Log    ${dockerDown.stdout}
