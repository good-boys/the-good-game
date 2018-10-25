#! /bin/sh
pushd ..
git config --global user.email "travis@travis-ci.com"
git config --global user.name "Travis CI"
git config --global credential.helper store
git clone https://${GITHUB_TOKEN}github.com/good-boys/good-game-builds.git > /dev/null 2>&1
cd good-game-builds
cp ../the-good-game/Build/*.zip .
git rm origin
git remote add origin https://good-boys:${GITHUB_TOKEN}@github.com/good-boys/good-game-builds.git
git add --all
git commit -m "Travis build: $TRAVIS_BUILD_NUMBER"
git push
popd
