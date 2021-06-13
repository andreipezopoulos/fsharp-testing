#!/usr/bin/env bash
d=$(cat extract_score.json | sed --expression='s/{{cpf}}/11111111111/g')
curl -v http://localhost:8085/score -H "Content-type: application/json" -d "$d"
echo ""
