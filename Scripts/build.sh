#! /bin/sh

project="GoodGame"

buildname() {
     platform=$1
     shorthash=$(openssl sha1 $file | cut -d ' ' -f 2 | cut -c1-7)
     date=$(date "+%F-%H%M")
     return $date-$shorthash-$platform
}

echo "Attempting to build $project for Windows"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd) \
  -buildWindowsPlayer "$(pwd)/Build/windows/$project.exe" \
  -quit

echo "Attempting to build $project for OS X"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd) \
  -buildOSXUniversalPlayer "$(pwd)/Build/osx/$project.app" \
  -quit

echo "Attempting to build $project for Linux"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd) \
  -buildLinuxUniversalPlayer "$(pwd)/Build/linux/$project.exe" \
  -quit

echo 'Logs from build'
cat $(pwd)/unity.log

echo 'Attempting to zip builds'
zip -r $(pwd)/Build/$(buildname linux).zip $(pwd)/Build/linux/
zip -r $(pwd)/Build/$(buildname mac).zip $(pwd)/Build/osx/
zip -r $(pwd)/Build/$(buildname windows).zip $(pwd)/Build/windows/
