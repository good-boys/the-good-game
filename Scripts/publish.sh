#! /bin/sh
pushd ..
git config --global user.email "travis@travis-ci.com"
git config --global user.name "Travis CI"
git config --global credential.helper store
git clone https://github.com/good-boys/good-game-builds.git
cd good-game-builds
cp ../the-good-game/Build/*.zip .
git add --all
git commit -m "Travis build: $TRAVIS_BUILD_NUMBER"
git push "https://${GITHUB_TOKEN}@github.com/good-boys/good-game-builds.git" master > /dev/null 2>&1
popd
