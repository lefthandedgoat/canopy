#!/usr/bin/env bash
set -e
set -o pipefail
trap finish EXIT

export GECKO_VERSION=v0.19.1

if [[ "$OSTYPE" == "linux-gnu" ]]; then
  curl -LO https://github.com/mozilla/geckodriver/releases/download/$GECKO_VERSION/geckodriver-$GECKO_VERSION-linux64.tar.gz
  tar xf geckodriver-$GECKO_VERSION-linux64.tar.gz

elif [[ "$OSTYPE" == "darwin"* ]]; then
  curl -LO https://github.com/mozilla/geckodriver/releases/download/$GECKO_VERSION/geckodriver-$GECKO_VERSION-macos.tar.gz
  tar xf geckodriver-$GECKO_VERSION-macos.tar.gz

elif [[ "$OSTYPE" == "cygwin" ]]; then
  curl -LO https://github.com/mozilla/geckodriver/releases/download/$GECKO_VERSION/geckodriver-$GECKO_VERSION-win64.zip
  unzip geckodriver-$GECKO_VERSION-win64.zip

elif [[ "$OSTYPE" == "msys" ]]; then
  curl -LO https://github.com/mozilla/geckodriver/releases/download/$GECKO_VERSION/geckodriver-$GECKO_VERSION-win64.zip
  unzip geckodriver-$GECKO_VERSION-win64.zip
fi

function finish {
  rm geckodriver-*.tar.gz 2>/dev/null || true
  rm geckodriver-*.zip 2>/dev/null || true
}
