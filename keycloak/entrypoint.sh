#!/bin/sh
set -e

mkdir -p /opt/keycloak/data/import
cp /opt/keycloak/bootstrap/starter-realm.json /opt/keycloak/data/import/starter-realm.json

exec /opt/keycloak/bin/kc.sh "$@"