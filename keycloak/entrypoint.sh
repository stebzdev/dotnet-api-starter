#!/bin/sh
set -e

mkdir -p /opt/keycloak/data/import
cp /opt/keycloak/bootstrap/Starter-realm.json /opt/keycloak/data/import/Starter-realm.json

exec /opt/keycloak/bin/kc.sh "$@"