#!/bin/bash

if [[ ${1} == "-help" || ${1} == "--help" ]]; then
	echo "Usage: build.sh [ARCHITECTURE]
Example: ./build.sh linux-x64
Information: see dotnet publish specifications for list of available architectures." 
	exit 0
elif [[ -z "${1// }" ]]; then
	echo "Provide an architecture to build this project for. Use --help for more info."
	exit 1
fi

PROJECT_NAME="ASVLM.Desktop"
DOTNET_VERSION="net9.0"
OUTPUT="$(cd ../Source && dotnet publish -c Release -p:PublishAot=true -p:PublishTrimmed=true -p:EnableCompressionInSingleFile=true -p:TrimMode=Link -r ${1} --self-contained)"

if [[ $? == 0 && ! ${OUTPUT} =~ "error" ]]; then
	chown -R 1000 ../Artifacts/publish/${PROJECT_NAME}/release_${1}
	echo "Done."
	exit 0
else
	echo "${OUTPUT}"
	exit 1
fi