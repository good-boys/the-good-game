#! /bin/sh
# requires gtimeout from coreutils

TEST_FILE=test.xml
touch $TEST_FILE
echo "Running Editor tests"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile $(pwd)/unity.log \
  -projectPath $(pwd) \
  -runEditorTests \
  -editorTestsResultFile $(pwd)/$TEST_FILE
rc0=$?
echo "STATUS: $rc0"
echo "Unit test results:"
gtimeout 10 cat $(pwd)/test.xml
# exit if tests failed
echo ""
if [ $rc0 -ne 0 ]; then { echo "===Unit tests failed==="; exit $rc0; } fi
echo "===Unit tests passed==="
